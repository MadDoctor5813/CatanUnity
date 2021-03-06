﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.util
{
    public class HexEdge : HexLocation
    {
        public HexCorner Start { get; set; }
        public HexCorner End { get; set; }

        public HexEdge(HexCorner start, HexCorner end)
        {
            Start = start;
            End = end;
        }

        public override Vector3 ToLocalCoords()
        {
            return (Start.ToLocalCoords() + End.ToLocalCoords()) / 2;
        }

        public override bool Equals(object obj)
        {
            if (obj is HexEdge)
            {
                HexEdge other = obj as HexEdge;
                return (other.Start.Equals(Start) && other.End.Equals(End)) ||
                    (other.Start.Equals(End) && other.End.Equals(Start));
            }
            else
            {
                return false;
            }
        }

        public override Quaternion ToLocalRot()
        {
            Vector3 start = Start.ToLocalCoords();
            Vector3 end = End.ToLocalCoords();
            Vector3 diff = start - end;
            float angle = Mathf.Rad2Deg * Mathf.Atan2(diff.z, diff.x);
            //move the angle returned from atan2 into a 0-360 range
            if (angle > 90)
            {
                angle = 450 - angle;
            }
            else
            {
                angle = 90 - angle;
            }
            return Quaternion.AngleAxis(angle, Vector3.up);
        }

        public static HexEdge GetNearestEdge(Vector3 coords)
        {
            HexCorner corner = HexCorner.GetNearestCorner(coords);
            HexCorner[] neighbors = HexCorner.GetNeighbors(corner);
            float[] distances = new float[neighbors.Length];
            for (int i = 0; i < neighbors.Length; i++)
            {
                distances[i] = Vector3.Distance(coords, neighbors[i].ToLocalCoords());
            }
            return new HexEdge(corner, neighbors[Array.IndexOf(distances, Mathf.Min(distances))]);
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ End.GetHashCode();
        }

    }

}
