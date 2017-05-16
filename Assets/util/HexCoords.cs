using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.util
{
    [System.Serializable]
    public class HexCoords : HexLocation
    {

        private static readonly Vector2 XBasisHex = new Vector2(1.5f * HexInfo.HexRadius, HexInfo.InnerHexRadius);
        private static readonly Vector2 ZBasisHex = new Vector2(0, 2f * HexInfo.InnerHexRadius);

        private static readonly Vector2 XBasisPixel = new Vector2(2f / (3 * HexInfo.HexRadius), -1f / (3f * HexInfo.HexRadius));
        private static readonly Vector2 ZBasisPixel = new Vector2(0, 1f / (2f * HexInfo.InnerHexRadius));


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

        public static HexCoords operator+ (HexCoords h1, HexCoords h2)
        {
            return new HexCoords(h1.X + h2.X, h1.Z + h2.Z);
        }

        public static HexCoords operator- (HexCoords h1, HexCoords h2)
        {
            return new HexCoords(h1.X - h2.X, h1.Z - h2.Z);
        }

        public override Vector3 ToLocalCoords()
        {
            Vector2 localCoords = (X * XBasisHex) + (Z * ZBasisHex);
            return new Vector3(localCoords.x, 0, localCoords.y);
        }

        public override Quaternion ToLocalRot()
        {
            return Quaternion.Euler(-90, 0, -90);
        }

        public static HexCoords FromLocalCoords(Vector3 localCoords)
        {
            Vector2 axialHexCoords = (localCoords.x * XBasisPixel) + (localCoords.z * ZBasisPixel);
            Vector3 cubeHexCoords = new Vector3(axialHexCoords.x, -axialHexCoords.x - axialHexCoords.y, axialHexCoords.y);
            return RoundHexCoords(cubeHexCoords);
        }
        
        public static Vector3[] GetCorners(HexCoords hexCoords)
        {
            Vector3 hexPos = hexCoords.ToLocalCoords();
            Vector3[] corners = new Vector3[6];
            for (int i = 0; i < 6; i++)
            {
                corners[i] = HexInfo.HexCornerOffsets[i] + hexPos;
            }
            return corners;
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

        public static int HexDistance(HexCoords h1, HexCoords h2)
        {
            //return the greatest difference between each coordinate
            return Math.Max(Math.Max(Math.Abs(h1.X - h2.X), Math.Abs(h1.Y - h2.Y)), Math.Abs(h1.Z - h2.Z));
        }

        public override string ToString()
        {
            return string.Format("HexCoords: X: {0} Y: {1} Z: {2}", X, Y, Z);
        }

    }
}
