using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lerneinheit_2.Models
{
    internal class ConnectFour : Match
    {
        internal ConnectFour(int p_RowsGameField, int p_ColumnsGameField, int p_NeededToWin, List<Player> p_Players) : base(p_RowsGameField, p_ColumnsGameField, p_NeededToWin, p_Players) { }

        internal override bool CheckInput(int p_PlacementChoice)
        {
            if (p_PlacementChoice > ColumnsBoard || p_PlacementChoice < 1)
            {
                return false;
            }
            else if (Board[0, p_PlacementChoice - 1] != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        internal override void GetPlacementPoint()
        {
            bool PlacementCompleted = false;
            int RowsUp = 1;
            do
            {
                if (Board[RowsBoard - RowsUp, PlacementChoiceHistory[PlacementCount] - 1] != null)
                {
                    RowsUp++;
                }
                else
                {
                    if (WithdrawalActivated)
                    {
                        RowsUp--;
                    }
                    RowsOfPlacement = RowsBoard - RowsUp;
                    ColumnsOfPlacement = PlacementChoiceHistory[PlacementCount] - 1;

                    PlacementCompleted = true;
                }

            } while (!PlacementCompleted);

        }
        internal override void DisplayBoard()
        {
            Programm.SetSystemColor();
            Console.Clear();
            int FieldsDisplayed = 0;

            for (int RowCounter = 0; RowCounter < RowsBoard; RowCounter++)
            {
                for (int ColumnCounter = 0; ColumnCounter < ColumnsBoard; ColumnCounter++)
                {

                    if (Board[RowCounter, ColumnCounter] == null)
                    {

                        if (FieldsDisplayed + 1 > ColumnsBoard)
                        {
                            Console.Write("   ");
                        }
                        else
                        {
                            for (int BlanksCounter = 0; BlanksCounter + (FieldsDisplayed + 1).ToString().Length < 3; BlanksCounter++)
                            {
                                Console.Write(" ");
                            }
                            Console.Write(FieldsDisplayed + 1);
                        }

                    }
                    else
                    {
                        Console.Write(" ");
                        Programm.SetPlayerColor();
                        Console.Write(Board[RowCounter, ColumnCounter]);
                        Programm.SetSystemColor();
                    }

                    FieldsDisplayed++;
                    Console.Write(" |");
                }
                Console.WriteLine();
                for (int ColumnCounter = 0; ColumnCounter < ColumnsBoard; ColumnCounter++)
                {
                    Console.Write("-----");
                }
                Console.WriteLine();
            }
        }

    }
}
