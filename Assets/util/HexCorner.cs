using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.util
{
    public class HexCorner
    {

        public HexCoords[] Hexes { get; set; }

        public HexCorner(HexCoords h1, HexCoords h2, HexCoords h3)
        {
            Hexes = new HexCoords[3];
            Hexes[0] = h1;
            Hexes[1] = h2;
            Hexes[2] = h3;
            Array.Sort(Hexes, (HexCoords a, HexCoords b) =>
            {
                if (a.X < b.X)
                {
                    return -1;
                }
                else if (a.X > b.X)
                {
                    return 1;
                }
                else
                {
                    if (a.Z < b.Z)
                    {
                        return -1;
                    }
                    else if (a.Z > b.Z)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            });
        }

        public static Vector3 ToLocalCoords(HexCorner corner)
        {
            Vector3 coords = new Vector3();
            for (int i = 0; i < 3; i++)
            {
                coords += HexCoords.ToLocalCoords(corner.Hexes[i]);
            }
            coords /= 3;
            coords.y = 0.1f;
            return coords;
        }

        public static HexCorner GetNearestCorner(Vector3 position)
        {
            //get the current hex the position is in
            HexCoords current = HexCoords.FromLocalCoords(position);
            //get all the corners
            Vector3[] corners = HexCoords.GetCorners(current);
            //calculate the distances to every corner
            float[] distances = new float[6];
            for (int i = 0; i < 6; i++)
            {
                distances[i] = (corners[i] - position).magnitude;
            }
            //find the corner with the minimum distance
            float minDist = Mathf.Min(distances);
            int cornerIdx = Array.IndexOf(distances, minDist);
            return GetCornerFromCornerIdx(current, cornerIdx);
        }

        private static HexCorner GetCornerFromCornerIdx(HexCoords hex, int cornerIdx)
        {
            HexCoords h2 = null;
            HexCoords h3 = null;
            switch (cornerIdx)
            {
                case 0:
                    h2 = new HexCoords(hex.X - 1, hex.Z + 1);
                    h3 = new HexCoords(hex.X, hex.Z + 1);
                    break;
                case 1:
                    h2 = new HexCoords(hex.X, hex.Z + 1);
                    h3 = new HexCoords(hex.X + 1, hex.Z);
                    break;
                case 2:
                    h2 = new HexCoords(hex.X + 1, hex.Z);
                    h3 = new HexCoords(hex.X + 1, hex.Z - 1);
                    break;
                case 3:
                    h2 = new HexCoords(hex.X + 1, hex.Z - 1);
                    h3 = new HexCoords(hex.X, hex.Z - 1);
                    break;
                case 4:
                    h2 = new HexCoords(hex.X - 1, hex.Z);
                    h3 = new HexCoords(hex.X, hex.Z - 1);
                    break;
                case 5:
                    h2 = new HexCoords(hex.X - 1, hex.Z + 1);
                    h3 = new HexCoords(hex.X - 1, hex.Z);
                    break;
            }
            return new HexCorner(hex, h2, h3);
        }

        public static HexCorner[] GetNeighbors(HexCorner corner)
        {
            HexCorner[] neighbors = new HexCorner[3];
            HexCoords h1 = corner.Hexes[0];
            HexCoords h2 = corner.Hexes[1];
            HexCoords h3 = corner.Hexes[2];
            HexCoords newCoords1 = h1 + (h2 - h1) + (h3 - h1);
            neighbors[0] = new HexCorner(newCoords1, h2, h3);
            HexCoords newCoords2 = h2 + (h1 - h2) + (h3 - h2);
            neighbors[1] = new HexCorner(h1, newCoords2, h3);
            HexCoords newCoords3 = h3 + (h1 - h3) + (h2 - h3);
            neighbors[2] = new HexCorner(h1, h2, newCoords3);
            return neighbors;
        }

        public override bool Equals(object b)
        {
            if (b == null)
            {
                return false;
            }
            if (!(b is HexCorner))
            {
                return false;
            }
            for (int i = 0; i < 3; i++)
            {
                HexCoords bCoords = (b as HexCorner).Hexes[i];
                if (Hexes[i].X != bCoords.X || Hexes[i].Y != bCoords.Y || Hexes[i].Z != bCoords.Z)
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int code = 0;
            for (int i = 0; i < 3; i++)
            {
                code ^= Hexes[i].X;
                code ^= Hexes[i].Y;
                code ^= Hexes[i].Z;
            }
            return code.GetHashCode();
        }
    }
}
