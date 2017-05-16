using Assets.defs;
using Assets.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.board
{
    class Board
    {
        private System.Random random;

        public Dictionary<HexCoords, TileTypes> TileMap { get; set; }
        public Dictionary<HexCorner, Unit> Units { get; set; }
        public Dictionary<HexEdge, Road> Roads { get; set; }

        public HexCornerGraph CornerGraph { get; set; }

        public Board()
        {
            random = new System.Random();
            TileMap = new Dictionary<HexCoords, TileTypes>();
            Units = new Dictionary<HexCorner, Unit>();
            Roads = new Dictionary<HexEdge, Road>();
            CornerGraph = new HexCornerGraph();
        }

        public void AddUnit(HexCorner intersection, Unit unit)
        {
            //if something's already there, delete it
            if (Units.ContainsKey(intersection))
            {
                Units.Remove(intersection);
            }
            Units.Add(intersection, unit);
        }

        public void GenerateMap()
        {
            //generate the list of tiles
            List<TileTypes> tileList = new List<TileTypes>();
            foreach (TileTypes type in Enum.GetValues(typeof(TileTypes)))
            {
                int tileCount = TileInfo.TileCounts[type];
                for (int i = 0; i < tileCount; i++)
                {
                    tileList.Add(type);
                }
            }
            //generate columns of the map
            GenerateMapColumn(-2, 0, 3, tileList);
            GenerateMapColumn(-1, -1, 4, tileList);
            GenerateMapColumn(0, -2, 5, tileList);
            GenerateMapColumn(1, -2, 4, tileList);
            GenerateMapColumn(2, -2, 3, tileList);
        }

        public bool IsValidUnitPlacement(HexCorner corner, UnitTypes type, PlayerColor color)
        {
            return (type == UnitTypes.Settlement && IsValidSettlement(corner)) || (type == UnitTypes.City && IsValidCity(corner, color));
        }

        public bool IsValidSettlement(HexCorner corner)
        {
            var unitLocations = Units.Keys;
            foreach (var location in unitLocations)
            {
                if ((Units[location].Type == UnitTypes.Settlement || Units[location].Type == UnitTypes.City) && CornerGraph.CornerDistance(location, corner) < 2)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsValidCity(HexCorner corner, PlayerColor color)
        {
            if (Units.ContainsKey(corner) && Units[corner].Type == UnitTypes.Settlement && Units[corner].Color == color)
            {
                return true;
            }
            return false;
        }

        public bool IsValidRoad(HexEdge edge)
        {
            return !Roads.ContainsKey(edge);
        }

        private void GenerateMapColumn(int x, int startZ, int count, List<TileTypes> tileList)
        {
            for (int i = 0; i < count; i++)
            {
                HexCoords hexCoords = new HexCoords(x, startZ);
                int chosenTileIdx = random.Next(tileList.Count);
                TileMap.Add(hexCoords, tileList[chosenTileIdx]);
                //delete the chosen tile from the list so we don't use it again
                tileList.RemoveAt(chosenTileIdx);
                startZ++;
            }
        }
    }
}
