using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.action
{
    public interface IAction
    {

        void Display(BoardView boardView);
        void Apply(BoardView boardView);
        void Undo(BoardView boardView);
    
    }
}
