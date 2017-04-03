using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.defs
{
    public class TileInfo
    {

        public static Dictionary<TileTypes, int> TileCounts = new Dictionary<TileTypes, int>()
        {
            { TileTypes.Wheat, 4 },
            { TileTypes.Wood, 4 },
            { TileTypes.Sheep, 4 },
            { TileTypes.Ore, 3 },
            { TileTypes.Brick, 3 },
            { TileTypes.Desert, 1 }
        };
    }
}
