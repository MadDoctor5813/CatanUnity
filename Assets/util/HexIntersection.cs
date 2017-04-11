using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.util
{
    public class HexIntersection
    {

        public HexCoords[] Hexes { get; set; }

        public HexIntersection(HexCoords h1, HexCoords h2, HexCoords h3)
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

        public static Vector3 ToLocalCoords(HexIntersection intersection)
        {
            Vector3 coords = new Vector3();
            for (int i = 0; i < 3; i++)
            {
                coords += HexCoords.ToLocalCoords(intersection.Hexes[i]);
            }
            coords /= 3;
            coords.y = 0.1f;
            return coords;
        }

        public static HexIntersection GetNearestIntersection(Vector3 position)
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
            return GetIntersectionFromCornerIdx(current, cornerIdx);
        }

        private static HexIntersection GetIntersectionFromCornerIdx(HexCoords hex, int cornerIdx)
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
            return new HexIntersection(hex, h2, h3);
        }

    }
}
