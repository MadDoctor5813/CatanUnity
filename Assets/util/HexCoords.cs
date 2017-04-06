using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.util
{
    [System.Serializable]
    public class HexCoords
    {

        public const float HexRadius = 1f;
        public const float InnerHexRadius = 0.866f * HexRadius;

        private static readonly Vector2 XBasisHex = new Vector2(1.5f * HexRadius, InnerHexRadius);
        private static readonly Vector2 ZBasisHex = new Vector2(0, 2f * InnerHexRadius);

        private static readonly Vector2 XBasisPixel = new Vector2(2f / (3 * HexRadius), -1f / (3f * HexRadius));
        private static readonly Vector2 ZBasisPixel = new Vector2(0, 1f / (2f * InnerHexRadius));


        [SerializeField]
        private int x, z;

        public HexCoords(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public int X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public int Z
        {
            get
            {
                return z;
            }

            set
            {
                z = value;
            }
        }

        public int Y
        {
            get
            {
                return -x - z;
            }
        }

        public static Vector3 ToLocalCoords(HexCoords hexCoords)
        {
            Vector2 localCoords = (hexCoords.X * XBasisHex) + (hexCoords.Z * ZBasisHex);
            return new Vector3(localCoords.x, 0, localCoords.y);
        }

        public static HexCoords FromLocalCoords(Vector3 localCoords)
        {
            Vector2 axialHexCoords = (localCoords.x * XBasisPixel) + (localCoords.z * ZBasisPixel);
            Vector3 cubeHexCoords = new Vector3(axialHexCoords.x, -axialHexCoords.x - axialHexCoords.y, axialHexCoords.y);
            return RoundHexCoords(cubeHexCoords);
        }

        private static HexCoords RoundHexCoords(Vector3 hexCoords)
        {
            int rX = Mathf.RoundToInt(hexCoords.x);
            int rY = Mathf.RoundToInt(hexCoords.y);
            int rZ = Mathf.RoundToInt(hexCoords.z);

            float dX = Mathf.Abs(hexCoords.x - rX);
            float dY = Mathf.Abs(hexCoords.y - rY);
            float dZ = Mathf.Abs(hexCoords.z - rZ);

            if (dX > dY && dX > dZ)
            {
                rX = -rY - rZ;
            }
            else if (dY > dZ)
            {
                rY = -rZ - rX;
            }
            else
            {
                rZ = -rX - rY;
            }

            return new HexCoords(rX, rZ);
        }

        public override string ToString()
        {
            return string.Format("HexCoords: X: {0} Y: {1} Z: {2}", X, Y, Z);
        }

    }
}
