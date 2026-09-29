using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Aerial.Players
{
    /// <summary>
    /// Playback through the embedded LibVLC engine (LibVLCSharp).
    /// </summary>
    public class LibVlcPlayer : IVideoPlayer
    {
        private static LibVLC libVLC;
        private static readonly object initLock = new object();

        private readonly VideoView view;
        private readonly MediaPlayer mediaPlayer;
        private Media media;
        private string currentUrl;
        private string pendingUrl;
        private VideoAdjustments adjustments = new VideoAdjustments();

        public event EventHandler MediaEnded;
        public event EventHandler<string> MediaError;
        public event MouseEventHandler PlayerMouseDown;
        public event MouseEventHandler PlayerMouseMove;
        public event KeyPressEventHandler PlayerKeyPress;

        public LibVlcPlayer()
        {
            mediaPlayer = new MediaPlayer(GetLibVLC())
            {
                EnableHardwareDecoding = true,
                EnableKeyInput = false,
                EnableMouseInput = false
            };

            view = new VideoView
            {
                Name = "player",
                BackColor = Color.Black,
                MediaPlayer = mediaPlayer,
                TabIndex = 1
            };

            view.HandleCreated += View_HandleCreated;
            view.MouseDown += (s, e) => PlayerMouseDown?.Invoke(this, e);
            view.MouseMove += (s, e) => PlayerMouseMove?.Invoke(this, e);
            view.KeyPress += (s, e) => PlayerKeyPress?.Invoke(this, e);

            mediaPlayer.EndReached += MediaPlayer_EndReached;
            mediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
            mediaPlayer.Playing += MediaPlayer_Playing;
        }

        public Control View => view;

        public PlayerType Type => PlayerType.LibVlc;

        public bool IsPlaying => mediaPlayer.IsPlaying;

        public bool Mute
        {
            get { return mediaPlayer.Mute; }
            set { mediaPlayer.Mute = value; }
        }

        public void Play(string url)
        {
            Trace.WriteLine("LibVlcPlayer.Play() " + url);

            // VLC needs the window handle, otherwise it opens its own output window
            if (!view.IsHandleCreated)
            {
                pendingUrl = url;
                return;
            }

            var previous = media;
            currentUrl = url;
            media = CreateMedia(url);
            mediaPlayer.Play(media);
            previous?.Dispose();
        }

        /// <summary>
        /// Brightness, contrast, etc. Applied immediately and to every next video.
        /// </summary>
        public VideoAdjustments Adjustments
        {
            get { return adjustments; }
            set
            {
                adjustments = value ?? new VideoAdjustments();
                ApplyAdjustments();
            }
        }

        public void ApplyAdjustments()
        {
            if (adjustments.IsDefault)
            {
                mediaPlayer.SetAdjustInt(VideoAdjustOption.Enable, 0);
            }
            else
            {
                mediaPlayer.SetAdjustInt(VideoAdjustOption.Enable, 1);
                mediaPlayer.SetAdjustFloat(VideoAdjustOption.Brightness, adjustments.Brightness);
                mediaPlayer.SetAdjustFloat(VideoAdjustOption.Contrast, adjustments.Contrast);
                mediaPlayer.SetAdjustFloat(VideoAdjustOption.Saturation, adjustments.Saturation);
                mediaPlayer.SetAdjustFloat(VideoAdjustOption.Hue, adjustments.Hue);
                mediaPlayer.SetAdjustFloat(VideoAdjustOption.Gamma, adjustments.Gamma);
            }
            mediaPlayer.SetRate(adjustments.Speed);
        }

        public void Stop()
        {
            pendingUrl = null;
            mediaPlayer.Stop();
        }

        private static LibVLC GetLibVLC()
        {
            lock (initLock)
            {
                if (libVLC == null)
                {
                    Core.Initialize();
                    libVLC = new LibVLC(
                        "--no-osd",
                        "--no-video-title-show",
                        "--no-snapshot-preview",
                        "--network-caching=1500",
                        "--file-caching=1000",
                        "--quiet");
                }
                return libVLC;
            }
        }

        private static Media CreateMedia(string url)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri) && !uri.IsFile)
                return new Media(libVLC, url, FromType.FromLocation);

            return new Media(libVLC, uri != null ? uri.LocalPath : url, FromType.FromPath);
        }

        private void View_HandleCreated(object sender, EventArgs e)
        {
            if (pendingUrl != null)
            {
                var url = pendingUrl;
                pendingUrl = null;
                Play(url);
            }
        }

        // LibVLC raises its events on an internal thread, calls back into the player must go through the UI thread
        private void RunOnUiThread(Action action)
        {
            if (view.IsDisposed || !view.IsHandleCreated) return;
            view.BeginInvoke(action);
        }

        private void MediaPlayer_Playing(object sender, EventArgs e)
        {
            // a new video output is created for every file, the filter values must be set again
            RunOnUiThread(ApplyAdjustments);
        }

        private void MediaPlayer_EndReached(object sender, EventArgs e)
        {
            RunOnUiThread(() => MediaEnded?.Invoke(this, EventArgs.Empty));
        }

        private void MediaPlayer_EncounteredError(object sender, EventArgs e)
        {
            Trace.WriteLine("LibVlcPlayer error " + currentUrl);
            RunOnUiThread(() => MediaError?.Invoke(this, currentUrl));
        }

        public void Dispose()
        {
            mediaPlayer.EndReached -= MediaPlayer_EndReached;
            mediaPlayer.EncounteredError -= MediaPlayer_EncounteredError;
            mediaPlayer.Playing -= MediaPlayer_Playing;

            if (mediaPlayer.IsPlaying)
                mediaPlayer.Stop();

            view.MediaPlayer = null;
            mediaPlayer.Dispose();
            media?.Dispose();
            view.Dispose();
        }
    }
}
