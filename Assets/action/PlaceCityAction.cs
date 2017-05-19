using Assets.defs;
using Assets.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.action
{
    class PlaceCityAction : IAction
    {

        public HexCorner Location { get; set; }
        public PlayerColor Color { get; set; }
        public Unit City { get; set; }

        public PlaceCityAction(HexCorner location, PlayerColor color)
        {
            Location = location;
            Color = color;
        }

        public void Apply(BoardView boardView)
        {
            boardView.Board.AddUnit(Location, City);
        }

        public void Display(BoardView boardView)
        {
            City = boardView.InstantiateUnit(Location, Color, UnitTypes.City);
            //hide the settlement under this city
            boardView.SetUnitVisible(Location, false);
        }

        public void Undo(BoardView boardView)
        {
            //unhide the settlement under this city
            boardView.SetUnitVisible(Location, true);
            if (City != null)
            {
                GameObject.Destroy(City.gameObject);
            }
        }

        public bool IsValid(BoardView boardView)
        {
            return boardView.Board.IsValidCity(Location, Color);
        }
    }
}
