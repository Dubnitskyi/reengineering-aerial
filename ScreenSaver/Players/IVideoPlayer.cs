using System;
using System.Windows.Forms;

namespace Aerial.Players
{
    /// <summary>
    /// Common contract for video playback engines used by the screen saver.
    /// </summary>
    public interface IVideoPlayer : IDisposable
    {
        /// <summary>
        /// Control that renders the video, hosted by the screen saver form.
        /// </summary>
        Control View { get; }

        PlayerType Type { get; }

        bool IsPlaying { get; }

        bool Mute { get; set; }

        void Play(string url);

        void Stop();

        event EventHandler MediaEnded;

        event EventHandler<string> MediaError;

        event MouseEventHandler PlayerMouseDown;

        event MouseEventHandler PlayerMouseMove;

        event KeyPressEventHandler PlayerKeyPress;
    }
}
