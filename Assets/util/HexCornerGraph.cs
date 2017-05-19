using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.util
{

    public class HexCornerGraph
    {

        public Dictionary<HexCorner, HexCorner[]> Neighbors { get; set; }

        public HexCornerGraph()
        {
            Neighbors = new Dictionary<HexCorner, HexCorner[]>();

            Build(new HexCorner(new HexCoords(0, 0), new HexCoords(0, 1), new HexCoords(-1, 1)));
        }

        public int CornerDistance(HexCorner start, HexCorner end)
        {
            Dictionary<HexCorner, HexCorner> parents = new Dictionary<HexCorner, HexCorner>();
            Queue<HexCorner> queue = new Queue<HexCorner>();
            queue.Enqueue(start);
            while (queue.Count != 0)
            {
                HexCorner current = queue.Dequeue();
                if (current.Equals(end))
                {
                    break;
                }
                HexCorner[] currNeighbors = Neighbors[current];
                for (int i = 0; i < currNeighbors.Length; i++)
                {
                    if (!parents.ContainsKey(currNeighbors[i]))
                    {
                        parents.Add(currNeighbors[i], current);
                        queue.Enqueue(currNeighbors[i]);
                    }
                }
            }
            //find distance
            int distance = 0;
            HexCorner pathNode = end;
            while (!pathNode.Equals(start))
            {
                pathNode = parents[pathNode];
                distance++;
            }
            return distance;
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

        public bool IsOutOfRange(HexCorner corner)
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
