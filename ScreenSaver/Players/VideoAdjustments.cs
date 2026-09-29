using System;
using System.Globalization;
using System.Linq;

namespace Aerial.Players
{
    /// <summary>
    /// Picture and speed settings applied by the LibVLC "adjust" video filter.
    /// </summary>
    public class VideoAdjustments
    {
        public float Brightness = 1f;   // 0..2
        public float Contrast = 1f;     // 0..2
        public float Saturation = 1f;   // 0..3
        public float Hue = 0f;          // -180..180
        public float Gamma = 1f;        // 0.01..3
        public float Speed = 1f;        // 0.25..4

        public bool IsDefault =>
            Brightness == 1f && Contrast == 1f && Saturation == 1f &&
            Hue == 0f && Gamma == 1f && Speed == 1f;

        public override string ToString()
        {
            return string.Join(";", new[] { Brightness, Contrast, Saturation, Hue, Gamma, Speed }
                .Select(v => v.ToString(CultureInfo.InvariantCulture)));
        }

        public static VideoAdjustments Parse(string value)
        {
            var result = new VideoAdjustments();
            if (string.IsNullOrWhiteSpace(value)) return result;

            var parts = value.Split(';');
            if (parts.Length != 6) return result;

            var numbers = new float[6];
            for (int i = 0; i < parts.Length; i++)
            {
                if (!float.TryParse(parts[i], NumberStyles.Float, CultureInfo.InvariantCulture, out numbers[i]))
                    return result;
            }

            result.Brightness = numbers[0];
            result.Contrast = numbers[1];
            result.Saturation = numbers[2];
            result.Hue = numbers[3];
            result.Gamma = numbers[4];
            result.Speed = numbers[5];
            return result;
        }
    }
}
