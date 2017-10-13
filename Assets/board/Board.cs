using Assets.defs;
using Assets.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.board
{
    public class Board
    {
        private System.Random random;

        public Dictionary<HexCoords, TileTypes> TileMap { get; set; }
        public Dictionary<HexCorner, Unit> Units { get; set; }
        public Dictionary<HexEdge, Road> Roads { get; set; }

        public HexCornerGraph CornerGraph { get; set; }

        private GameCoordinator coordinator;

        public Board(GameCoordinator coordinator)
        {
            this.coordinator = coordinator;
            random = new System.Random();
            TileMap = new Dictionary<HexCoords, TileTypes>();
            Units = new Dictionary<HexCorner, Unit>();
            Roads = new Dictionary<HexEdge, Road>();
            CornerGraph = new HexCornerGraph(this);
        }

        public void AddUnit(HexCorner intersection, Unit unit)
        {
            //if something's already there, delete it
            if (Units.ContainsKey(intersection))
            {
                GameObject.Destroy(Units[intersection]);
                Units.Remove(intersection);
            }
            Units.Add(intersection, unit);
        }

        public void AddRoad(HexEdge edge, Road road)
        {
            Roads.Add(edge, road);
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
            return (type == UnitTypes.Settlement && IsValidSettlement(color, corner)) || (type == UnitTypes.City && IsValidCity(corner, color));
        }

        public bool IsValidSettlement(PlayerColor color, HexCorner corner)
        {
            var unitLocations = Units.Keys;
            //true if this placement is connected by road to a settlement of the same color
            bool isConnected = false;
            foreach (var location in unitLocations)
            {
                if ((Units[location].Type == UnitTypes.Settlement || Units[location].Type == UnitTypes.City) && CornerGraph.GraphSearch(location, corner).Length < 2)
                {
                    return false;
                }
                if (Units[location].Color == color && CornerGraph.GraphRoadSearch(corner, location) != null)
                {
                    isConnected = true;
                }
            }
            //the road restriction only applies if the game isn't in setup mode
            if (coordinator.GameState.State != GameStates.Setup && isConnected == false)
            {
                return false;
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

        public bool IsValidRoad(PlayerColor color, HexEdge edge)
        {
            if (Roads.ContainsKey(edge))
            {
                return false;
            }
            if (CornerGraph.IsOutOfRange(edge.Start) || CornerGraph.IsOutOfRange(edge.End))
            {
                return false;
            }
            if (coordinator.GameState.State == GameStates.Setup)
            {
                //check if there's a settlement on one of the road's corners
                if (!((Units.ContainsKey(edge.Start) && Units[edge.Start].Color == color) ||
                    (Units.ContainsKey(edge.End) && Units[edge.End].Color == color)))
                {
                    return false;
                }
            }
            return true;
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
