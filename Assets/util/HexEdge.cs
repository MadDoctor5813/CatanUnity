using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.util
{
    public class HexEdge
    {
        HexCorner Start { get; set; }
        HexCorner End { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is HexEdge)
            {
                HexEdge other = obj as HexEdge;
                return (other.Start.Equals(Start) && other.End.Equals(End));
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ End.GetHashCode();
        }
    }

}
