using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

        public static Vector3 ToLocalCoords(HexIntersection intersection)
        {
            Vector3 coords = new Vector3();
            for (int i = 0; i < 3; i++)
            {
                coords += HexCoords.ToLocalCoords(intersection.Neighbors[i]);
            }
            coords /= 3;
            coords.y = 0.1f;
            return coords;
        }

    }
}
