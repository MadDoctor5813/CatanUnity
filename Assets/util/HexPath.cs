using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.util
{
    public class HexPath
    {

        public HexCorner Start { get; set; }
        public HexCorner End { get; set; }

        public int Length { get; set; }

        public List<HexCorner> Nodes { get; set; }

    }
}
