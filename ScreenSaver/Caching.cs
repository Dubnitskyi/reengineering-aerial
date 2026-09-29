using System.IO;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using System.Threading;

namespace Aerial
{
    public class Caching
    {
        public static string TempFolder = "";
        public static string CacheFolder = new RegSettings().CacheLocation;

        public static int DelayAmount = 1000 * 10; // 10 seconds.
        public static int NumOfCurrentDownloads = 0;

        internal static readonly HttpClient Http = CreateHttpClient();



        /// <summary>
        /// Init cache. Clear partially downloaded files from temp folder.
        /// </summary>
        internal static void Setup()
        {
            // If there is no location stored in the Registry, use the default location
            if (string.IsNullOrWhiteSpace(CacheFolder))
            {
                CacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Aerial");
            }
            TempFolder = Path.Combine(CacheFolder, "temp");

            // Ensure folders exist
            Directory.CreateDirectory(CacheFolder);
            Directory.CreateDirectory(TempFolder);

            // Delete partial temp files if any 
            foreach (var file in Directory.CreateDirectory(TempFolder).GetFiles())
            {
                file.Delete();
            }
        }

        internal static bool IsHit(string url)
        {
            if (!IsRemote(url)) return false;
            string filename = Path.GetFileName(url);
            return File.Exists(Path.Combine(CacheFolder, filename));
        }

        internal static bool IsCaching(string url)
        {
            string filename = Path.GetFileName(url);
            return File.Exists(Path.Combine(TempFolder, filename));
        }

        internal static string Get(string url)
        {
            string filename = Path.GetFileName(url);
            return Path.Combine(CacheFolder, filename);
        }

        internal static void StartDelayedCache(string url)
        {
            if (!IsRemote(url)) return;

            if (EnsureEnoughSpace())
            {
                Task.Delay(DelayAmount).ContinueWith(async t =>
                {
                    if (!IsCaching(url))
                        await DownloadFile(url);
                });
            }
        }

        private static async Task DownloadFile(string url)
        {
            string filename = Path.GetFileName(url);
            var tempFullPath = Path.Combine(TempFolder, filename);
            var cacheFullpath = Path.Combine(CacheFolder, filename);

            DownloadStart();
            try
            {
                using (var response = await Http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    using (var file = new FileStream(tempFullPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(file);
                    }
                }

                // delete if old file exists
                if (File.Exists(cacheFullpath))
                    File.Delete(cacheFullpath);

                File.Move(tempFullPath, cacheFullpath);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error caching " + url + ": " + ex.Message);

                // attempt to remove partially downloaded file
                if (File.Exists(tempFullPath))
                    File.Delete(tempFullPath);
            }
            finally
            {
                DownloadEnd();
            }
        }

        internal static bool IsRemote(string url)
        {
            return url != null && (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Stores a downloaded text document (video list) so it's available offline.
        /// </summary>
        internal static void SaveText(string url, string text)
        {
            if (!IsRemote(url)) return;
            try
            {
                File.WriteAllText(Path.Combine(CacheFolder, Path.GetFileName(url)), text);
            }
            catch (IOException ex)
            {
                Trace.WriteLine("Error saving " + url + ": " + ex.Message);
            }
        }

        private static HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Aerial"); //github will give a 403 if we don't define the user agent
            return client;
        }

        internal static async void UpdateCachePath(string oldCacheDirectory, string cacheLocation)
        {
            if (oldCacheDirectory == cacheLocation) return;
            CacheFolder = cacheLocation;

            // Move old cache to new location if space allows
            var currentCacheSpace = GetDirectorySize(oldCacheDirectory);
            if (currentCacheSpace < CacheSpace() - (1000 * 1000 * 1000))
            {
                // Note might take a while, hanging the save dialog
                // video blocks this command: Directory.Move(oldCacheDirectory, cacheLocation);
                foreach (var f in Directory.GetFiles(oldCacheDirectory))
                {
                    var newfile = Path.Combine(cacheLocation, Path.GetFileName(f));
                    if (!File.Exists(newfile))
                        await Task.Factory.StartNew(() => File.Move(f, newfile));

                }
            }

            DeleteCache(oldCacheDirectory);

            // Delete old cache
            try
            {
                await Task.Factory.StartNew(() => Directory.Delete(oldCacheDirectory, true));
            }
            catch (UnauthorizedAccessException)
            {
                // Leave dir for now.
                // todo - windows removes all files after the video player stops using them,
                // yet leaves the folder, we need to redo this operation in 3 mins, for example.
            }
        }

        public static async void DeleteCache(string folder = null)
        {
            if (folder == null) folder = CacheFolder;
            foreach (var f in Directory.GetFiles(folder))
            {
                try
                {
                    await Task.Factory.StartNew(() => File.Delete(f));
                }
                catch (UnauthorizedAccessException ex)
                {
                    // video may be used while deleting
                    Trace.WriteLine("Access denied while moving cached files " + ex);
                }
            }
        }

        public static long CacheSpace()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (CacheFolder.StartsWith(drive.Name))
                    return drive.TotalFreeSpace;
            }
            return 0;
        }

        public static long GetDirectorySize(string path = null)
        {
            if (path == null) path = CacheFolder;
            long size = 0;
            if (Directory.Exists(path)) 
                foreach (string name in Directory.GetFiles(path, "*.*"))
                    size += new FileInfo(name).Length;

            return size;
        }

        /// <summary>
        ///  Ensures the drive with user folder has more than 1 gig space left.
        /// </summary>
        /// <returns></returns>
        private static bool EnsureEnoughSpace()
        {
            return CacheSpace() > 1000000000;
        }

        public static string TryHit(string url)
        {
            if (IsHit(url))
                return Get(url);
            return url;
        }

        private static void DownloadStart()
        {
            Interlocked.Increment(ref NumOfCurrentDownloads);
        }

        private static void DownloadEnd()
        {
            Interlocked.Decrement(ref NumOfCurrentDownloads);
        }
    }
}