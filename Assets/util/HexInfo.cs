using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.util
{
    public class HexInfo
    {

        public const float HexRadius = 1f;
        public const float InnerHexRadius = 0.866f * HexRadius;

        public static readonly Vector3[] HexCornerOffsets =
{
            new Vector3(-0.5f * HexRadius, 0, InnerHexRadius),
            new Vector3(0.5f * HexRadius, 0, InnerHexRadius),
            new Vector3(HexRadius, 0, 0),
            new Vector3(0.5f * HexRadius, 0, -InnerHexRadius),
            new Vector3(-0.5f * HexRadius, 0, -InnerHexRadius),
            new Vector3(-HexRadius, 0, 0)
        };

    }
}
