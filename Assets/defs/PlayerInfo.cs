using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.defs
{
    public class PlayerInfo
    {

        public enum PlayerColor
        {
            Red,
            Blue,
            White,
            Orange
        }

        private static Dictionary<PlayerColor, Color> rgbValues = new Dictionary<PlayerColor, Color>()
        {
            { PlayerColor.Red, new Color(0.91765f, 0.14902f, 0.10980f) },
            { PlayerColor.Blue, new Color(0.11373f, 0.14118f, 0.89804f) },
            { PlayerColor.White, new Color(0.97647f, 0.97647f, 0.97647f) },
            { PlayerColor.Orange, new Color(0.98824f, 0.62745f, 0.18431f) }
        };

        public static Color GetPlayerColorRGB(PlayerColor color)
        {
            return rgbValues[color];
        }

    }
}
