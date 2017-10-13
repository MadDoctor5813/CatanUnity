using Assets.defs;
using Assets.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.action
{
    class PlaceSettlementAction : IAction
    {

        public HexCorner Location { get; set; }
        public PlayerColor Color { get; set; }

        public Unit Settlement { get; set; }

        public PlaceSettlementAction(HexCorner location, PlayerColor color)
        {
            Location = location;
            Color = color;
            Settlement = null;
        }

        public void Display(BoardView boardView)
        {
            Settlement = boardView.InstantiateUnit(Location, Color, UnitTypes.Settlement);
        }

        public void Apply(BoardView boardView)
        {
            boardView.Board.AddUnit(Location, Settlement);
        }

        public void Undo(BoardView boardView)
        {
            if (Settlement != null)
            {
                GameObject.Destroy(Settlement.gameObject);
            }
        }

        public bool IsValid(BoardView boardView)
        {
            return boardView.Board.IsValidSettlement(Color, Location);
        }
    }
}
