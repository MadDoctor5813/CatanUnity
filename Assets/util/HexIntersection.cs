using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.util
{
    public class HexIntersection
    {

        public HexCoords[] Neighbors { get; set; }

        public HexIntersection(HexCoords h1, HexCoords h2, HexCoords h3)
        {
            Neighbors = new HexCoords[3];
            Neighbors[0] = h1;
            Neighbors[1] = h2;
            Neighbors[2] = h3;
        }

    }
}
