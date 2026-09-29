using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Aerial
{
    // Parses http://a1.phobos.apple.com/us/r1000/000/Features/atv/AutumnResources/videos/entries.json
    public class AerialContext
    {
        static IdAsset[] cachedEntities;
        static List<Asset> cachedPlaylist;
        static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { IncludeFields = true };
        static readonly string[] localVideoExtensions = { ".mp4", ".mov", ".m4v", ".mkv", ".webm", ".avi", ".wmv" };

        /// <summary>
        /// True when the video list could not be downloaded and the cached copy is used instead.
        /// </summary>
        public static bool IsOffline { get; private set; }

        public static List<Asset> GetMovies()
        {
            var urls = GetAllEntries();

            return FilterEntries(urls);
        }
        public static List<Asset> GetAllMovies()
        {
            //if no Entries, just return an empty list
            if (GetAllEntries() == null) { return new List<Asset>(); }

            return GetAllEntries().SelectMany(s => s.assets).ToList();
        }

        private static List<Asset> FilterEntries(IdAsset[] urls)
        {
            if (urls == null) { return new List<Asset>(); }; //if no URLS, return an empty list

            var time = (DateTime.Now.Hour < 6 || DateTime.Now.Hour > 19) ? "night" : "day";
            var ran = new Random();
            var settings = new RegSettings();
            List<Asset> links = urls.SelectMany(s => s.assets)
                .Where(t => AssetSelected(t)) //only return videos that have been selected to be played
                .OrderBy(t => ran.Next()) // randomize
                .OrderByDescending(t => settings.UseTimeOfDay && t.timeOfDay == time)
                .ToList();

            //If the links list is empty or null for some reason, just populate with all movies
            if (links == null || links.Count == 0)
            {
                links = urls.SelectMany(s => s.assets).ToList();
            }

            // without internet only already downloaded or local videos can be played
            if (IsOffline)
            {
                links = links.Where(t => Caching.IsHit(t.url) || !Caching.IsRemote(t.url)).ToList();
            }

            if (settings.MultiMonitorMode == RegSettings.MultiMonitorModeEnum.DifferentVideos)
                return links;

            if (cachedPlaylist == null)
                cachedPlaylist = links;

            return cachedPlaylist;
        }

        public static IdAsset[] GetAllEntries()
        {
            if (cachedEntities != null) return cachedEntities;

            var settings = new RegSettings();
            if (settings.UseLocalFolder)
            {
                cachedEntities = GetLocalFolderEntries(settings.LocalFolder);
                return cachedEntities;
            }

            var aerialUrl = settings.JsonURL;
            if (string.IsNullOrWhiteSpace(aerialUrl))
                aerialUrl = AerialGlobalVars.appleVideosURI;
#if OFFLINE
            aerialUrl = "http://BOGUS/entries.json";
#endif

            string entries = null;
            try
            {
                entries = DownloadString(aerialUrl);
                IsOffline = false;
                Caching.SaveText(aerialUrl, entries);
            }
            catch (Exception e) when (e is HttpRequestException || e is IOException || e is OperationCanceledException || e is UriFormatException)
            {
                // no connection, fall back to the last downloaded list
                IsOffline = true;
                if (Caching.IsHit(aerialUrl))
                    entries = File.ReadAllText(Caching.Get(aerialUrl));
            }

            if (entries == null) return null;

            try
            {
                cachedEntities = JsonSerializer.Deserialize<IdAsset[]>(entries, jsonOptions);
            }
            catch (JsonException)
            {
                //the passed in entities document is invalid.
                return null;
            }


            return cachedEntities;
        }

        private static string DownloadString(string url)
        {
            var uri = new Uri(url);
            if (uri.IsFile)
                return File.ReadAllText(uri.LocalPath);

            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                return Task.Run(() => Caching.Http.GetStringAsync(uri, cts.Token)).GetAwaiter().GetResult();
            }
        }

        private static IdAsset[] GetLocalFolderEntries(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
                return null;

            var assets = Directory.EnumerateFiles(folder)
                .Where(f => localVideoExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
                .OrderBy(f => f)
                .Select(f => new Asset
                {
                    url = f,
                    accessibilityLabel = Path.GetFileNameWithoutExtension(f),
                    type = "video",
                    id = Path.GetFileName(f),
                    timeOfDay = "day"
                })
                .ToArray();

            if (assets.Length == 0) return null;

            return new[] { new IdAsset { id = "local", assets = assets } };
        }

        /**
         * Returns true if the asset (movie) is in the chosen movies in the registry key, false if it isn't 
         */
        private static bool AssetSelected(Asset a)
        {
            var settings = new RegSettings();

            //if no movies are selected to be played, just allow all
            if(String.IsNullOrEmpty(settings.ChosenMovies))
            {
                return true;
            }

            var selected = new RegSettings().ChosenMovies.Split(';').ToList();
            List<string> selectedIds = selected.Select(s => GetIdFromTimeAndIdNumbered(s)).ToList(); ;

            return selectedIds.Contains(a.id);

        }

        /*
         * Parses the ID from the TimeAndIdNumbered string. Expecting the ID to be between parenthasis ex: China/day 1 (b4-1)
         * Added the ID to the node for the movie filtering
         */
        public static string GetIdFromTimeAndIdNumbered(string TimeAndId)
        {
            var splitString = TimeAndId.Split('(', ')');

            if (splitString.Length > 1)
            {
                return splitString[1];
            } else
            {
                return "NO ID IN STRING";
            }
        }
    }

    public class IdAsset
    {
        public string id;
        public Asset[] assets;
    }

    public class Asset : IComparable<Asset>
    {
        public string url;//" : "http://a1.phobos.apple.com/us/r1000/000/Features/atv/AutumnResources/videos/b1-1.mov",
        public string accessibilityLabel;//" : "Hawaii",
        public string type;//" : "video",
        public string id;// : "b1-1",
        public string timeOfDay;//" : "day"

        internal int numeric = 0;
        
        public override string ToString()
        {
            return accessibilityLabel + (numeric == 0 ? "" : " " + numeric) + " " + timeOfDay + "";
        }
        public string ShortName()
        {
            return accessibilityLabel + " — " + timeOfDay;
        }
        public string ToFullName()
        {
            return accessibilityLabel + " — " + timeOfDay + " (" + id + ")";
        }
        public string TimeNumbered()
        {
            return timeOfDay + (numeric == 0 ? "" : " " + numeric);
        }

        public string TimeAndIdNumbered()
        {
            return timeOfDay + (numeric == 0 ? "" : " " + numeric) + " (" + id + ")";
        }

        public int CompareTo(Asset other)
        {
            if (other == null) return 1;
            if (other == this) return 0;
            return NativeMethods.StrCmpLogicalW(ToFullName(), other.ToFullName());
        }
    }
}
