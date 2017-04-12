using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.util
{

    public class HexCornerComparer : IEqualityComparer<HexCorner>
    {
        public bool Equals(HexCorner a, HexCorner b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null)
            {
                return false;
            }
            for (int i = 0; i < 3; i++)
            {
                HexCoords aCoords = a.Hexes[i];
                HexCoords bCoords = b.Hexes[i];
                if (aCoords.X != bCoords.X || aCoords.Y != bCoords.Y || aCoords.Z != bCoords.Z)
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(HexCorner corner)
        {
            int code = 0;
            for (int i = 0; i < 3; i++)
            {
                code ^= corner.Hexes[0].X;
                code ^= corner.Hexes[0].Y;
                code ^= corner.Hexes[0].Z;
            }
            return code.GetHashCode();
        }
    }

    class HexCornerGraph
    {

        public Dictionary<HexCorner, HexCorner[]> Neighbors { get; set; }

        public HexCornerGraph()
        {
            Neighbors = new Dictionary<HexCorner, HexCorner[]>(new HexCornerComparer());

            Build(new HexCorner(new HexCoords(0, 0), new HexCoords(0, 1), new HexCoords(-1, 1)));
        }

        private void Build(HexCorner corner)
        {
            //dont add duplicates
            if (Neighbors.ContainsKey(corner))
            {
                return;
            }
            HexCorner[] possibleNeighbors = HexCorner.GetNeighbors(corner);
            HexCorner[] validNeighbors = possibleNeighbors.Where((HexCorner val) => { return !IsOutOfRange(val); }).ToArray();
            Neighbors.Add(corner, validNeighbors);
            for (int i = 0; i < validNeighbors.Length; i++)
            {
                Build(validNeighbors[i]);
            }
        }

        private bool IsOutOfRange(HexCorner corner)
        {
            HexCoords origin = new HexCoords(0, 0);
            for (int i = 0; i < 3; i++)
            {
                HexCoords coords = corner.Hexes[i];
                if (HexCoords.HexDistance(coords, origin) > 3)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
