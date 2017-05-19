using Assets.defs;
using Assets.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.action
{
    class PlaceRoadAction : IAction
    {

        HexEdge Edge { get; set; }
        PlayerColor Color { get; set; }
        Road Road { get; set; }

        public PlaceRoadAction(HexEdge edge, PlayerColor color)
        {
            Edge = edge;
            Color = color;
        }

        public void Apply(BoardView boardView)
        {
            boardView.Board.AddRoad(Edge, Road);
        }

        public void Display(BoardView boardView)
        {
            Road = boardView.InstantiateRoad(Edge, Color);
        }

        public void Undo(BoardView boardView)
        {
            if (Road != null)
            {
                GameObject.Destroy(Road.gameObject);
            }
        }

        public bool IsValid(BoardView boardView)
        {
            return boardView.Board.IsValidRoad(Edge);
        }
    }
}
