using AxWMPLib;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using WMPLib;

namespace Aerial.Players
{
    /// <summary>
    /// Legacy playback through the Windows Media Player ActiveX control.
    /// </summary>
    public class WmpPlayer : IVideoPlayer
    {
        private readonly AxWindowsMediaPlayer player;
        private readonly Timer stateTimer = new Timer();
        private bool started = false;

        public event EventHandler MediaEnded;
        public event EventHandler<string> MediaError;
        public event MouseEventHandler PlayerMouseDown;
        public event MouseEventHandler PlayerMouseMove;
        public event KeyPressEventHandler PlayerKeyPress;

        public WmpPlayer()
        {
            player = new AxWindowsMediaPlayer();
            ((ISupportInitialize)player).BeginInit();
            player.Enabled = true;
            player.Name = "player";
            player.TabIndex = 1;
            ((ISupportInitialize)player).EndInit();

            player.HandleCreated += Player_HandleCreated;
            player.PlayStateChange += Player_PlayStateChange;
            player.MediaError += Player_MediaError;
            player.MouseDownEvent += Player_MouseDownEvent;
            player.MouseMoveEvent += Player_MouseMoveEvent;
            player.KeyPressEvent += Player_KeyPressEvent;

            // WMP has no reliable "clip finished" event, so its state is polled like in the original form
            stateTimer.Interval = 1000;
            stateTimer.Tick += StateTimer_Tick;
        }

        public Control View => player;

        public PlayerType Type => PlayerType.Wmp;

        public bool IsPlaying => started && player.playState == WMPPlayState.wmppsPlaying;

        public bool Mute
        {
            get { return player.settings.mute; }
            set { player.settings.mute = value; }
        }

        public void Play(string url)
        {
            Trace.WriteLine("WmpPlayer.Play() " + url);
            player.URL = url;
            started = true;
            stateTimer.Enabled = true;
        }

        public void Stop()
        {
            started = false;
            stateTimer.Enabled = false;
            player.Ctlcontrols.stop();
        }

        private void Player_HandleCreated(object sender, EventArgs e)
        {
            player.enableContextMenu = false;
            player.settings.autoStart = true;
            player.settings.enableErrorDialogs = false;
            player.stretchToFit = true;
            player.uiMode = "none";
        }

        private void StateTimer_Tick(object sender, EventArgs e)
        {
            if (!started) return;

            var state = player.playState;
            if (state == WMPPlayState.wmppsReady ||
                state == WMPPlayState.wmppsUndefined ||
                state == WMPPlayState.wmppsStopped)
            {
                started = false;
                MediaEnded?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Player_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            NativeMethods.EnableMonitorSleep();
        }

        private void Player_MediaError(object sender, _WMPOCXEvents_MediaErrorEvent e)
        {
            started = false;
            MediaError?.Invoke(this, player.URL);
        }

        private void Player_MouseDownEvent(object sender, _WMPOCXEvents_MouseDownEvent e)
        {
            PlayerMouseDown?.Invoke(this, new MouseEventArgs(e.nButton == 1 ? MouseButtons.Left : MouseButtons.Right, 0, e.fX, e.fY, 0));
        }

        private void Player_MouseMoveEvent(object sender, _WMPOCXEvents_MouseMoveEvent e)
        {
            PlayerMouseMove?.Invoke(this, new MouseEventArgs(MouseButtons.None, 0, e.fX, e.fY, 0));
        }

        private void Player_KeyPressEvent(object sender, _WMPOCXEvents_KeyPressEvent e)
        {
            PlayerKeyPress?.Invoke(this, new KeyPressEventArgs((char)e.nKeyAscii));
        }

        public void Dispose()
        {
            stateTimer.Dispose();
            player.Dispose();
        }
    }
}
