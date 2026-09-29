using Aerial;
using Aerial.Players;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace ScreenSaver
{
    /// <summary>
    /// Live picture adjustments for the LibVLC player.
    /// </summary>
    public partial class VideoAdjustForm : Form
    {
        private readonly LibVlcPlayer player;
        private readonly VideoAdjustments adjustments;
        private bool loading = false;

        public VideoAdjustForm(LibVlcPlayer player)
        {
            InitializeComponent();

            this.player = player;
            this.adjustments = new RegSettings().Adjustments;

            LoadValues();
        }

        private void LoadValues()
        {
            loading = true;

            trkBrightness.Value = ToTrack(trkBrightness, adjustments.Brightness * 100);
            trkContrast.Value = ToTrack(trkContrast, adjustments.Contrast * 100);
            trkSaturation.Value = ToTrack(trkSaturation, adjustments.Saturation * 100);
            trkHue.Value = ToTrack(trkHue, adjustments.Hue);
            trkGamma.Value = ToTrack(trkGamma, adjustments.Gamma * 100);
            trkSpeed.Value = ToTrack(trkSpeed, adjustments.Speed * 100);

            loading = false;
            UpdateLabels();
        }

        private static int ToTrack(TrackBar trackBar, float value)
        {
            return Math.Max(trackBar.Minimum, Math.Min(trackBar.Maximum, (int)Math.Round(value)));
        }

        private void UpdateLabels()
        {
            lblBrightness.Text = "Brightness: " + adjustments.Brightness.ToString("0.00", CultureInfo.InvariantCulture);
            lblContrast.Text = "Contrast: " + adjustments.Contrast.ToString("0.00", CultureInfo.InvariantCulture);
            lblSaturation.Text = "Saturation: " + adjustments.Saturation.ToString("0.00", CultureInfo.InvariantCulture);
            lblHue.Text = "Hue: " + adjustments.Hue.ToString("0", CultureInfo.InvariantCulture) + "°";
            lblGamma.Text = "Gamma: " + adjustments.Gamma.ToString("0.00", CultureInfo.InvariantCulture);
            lblSpeed.Text = "Speed: " + adjustments.Speed.ToString("0.00", CultureInfo.InvariantCulture) + "x";
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            if (loading) return;

            adjustments.Brightness = trkBrightness.Value / 100f;
            adjustments.Contrast = trkContrast.Value / 100f;
            adjustments.Saturation = trkSaturation.Value / 100f;
            adjustments.Hue = trkHue.Value;
            adjustments.Gamma = trkGamma.Value / 100f;
            adjustments.Speed = trkSpeed.Value / 100f;

            UpdateLabels();
            player.Adjustments = adjustments;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            var defaults = new VideoAdjustments();
            adjustments.Brightness = defaults.Brightness;
            adjustments.Contrast = defaults.Contrast;
            adjustments.Saturation = defaults.Saturation;
            adjustments.Hue = defaults.Hue;
            adjustments.Gamma = defaults.Gamma;
            adjustments.Speed = defaults.Speed;

            LoadValues();
            player.Adjustments = adjustments;
        }

        private void VideoAdjustForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var settings = new RegSettings();
            settings.Adjustments = adjustments;
            settings.SaveSettings();
        }
    }
}
