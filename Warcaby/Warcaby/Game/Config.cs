using System.Drawing;

namespace Warcaby
{
    internal sealed class Config
    {
        #region Constants
        internal static readonly int AREA_SIZE = 48;
        internal static readonly int AREA_COUNT = 8;
        internal static readonly Color WAR_AREA_COLOR = Color.FromArgb(85, 136, 34);
        internal static readonly Color EMPTY_AREA_COLOR = Color.FromArgb(237, 27, 36);
        internal static readonly Color PLAYER_A_COLOR = Color.White;
        internal static readonly Color PLAYER_B_COLOR = Color.FromArgb(100, 100, 100);
        #endregion
    }
}