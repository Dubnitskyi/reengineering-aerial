using System;
using System.Diagnostics;

namespace Aerial.Players
{
    /// <summary>
    /// Creates the playback engine selected in the settings.
    /// </summary>
    public static class PlayerFactory
    {
        public static IVideoPlayer Create()
        {
            return Create(new RegSettings().PlayerType);
        }

        public static IVideoPlayer Create(PlayerType type)
        {
            if (type == PlayerType.LibVlc)
            {
                try
                {
                    return new LibVlcPlayer { Adjustments = new RegSettings().Adjustments };
                }
                catch (Exception ex)
                {
                    // missing or broken native libvlc, keep the screen saver working with WMP
                    Trace.WriteLine("LibVLC init failed, falling back to WMP: " + ex);
                }
            }

            return new WmpPlayer();
        }
    }
}
