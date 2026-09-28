using System.ComponentModel;

namespace Aerial.Players
{
    public enum PlayerType
    {
        [Description("LibVLC (recommended)")]
        LibVlc = 0,
        [Description("Windows Media Player")]
        Wmp = 1,
    }
}
