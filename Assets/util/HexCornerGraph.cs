using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.util
{

    class HexCornerGraph
    {

        public Dictionary<HexCorner, HexCorner[]> Neighbors { get; set; }

        public HexCornerGraph()
        {
            Neighbors = new Dictionary<HexCorner, HexCorner[]>();

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
