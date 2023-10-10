using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lerneinheit_2.Models
{
    internal class TicTacToe : Match
    {
        internal TicTacToe(int p_RowsGameField, int p_ColumnsGameField, int p_NeededToWin, List<Player> p_Players) : base(p_RowsGameField,p_ColumnsGameField,p_NeededToWin, p_Players) { }
        internal override void GetPlacementPoint()
        {
            RowsOfPlacement = (PlacementChoiceHistory[PlacementCount] - 1) / ColumnsBoard;
            ColumnsOfPlacement = (PlacementChoiceHistory[PlacementCount] - 1) % ColumnsBoard;
        }

    }
}
