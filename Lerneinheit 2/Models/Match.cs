using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lerneinheit_2.Models
{
    internal class Match
    {
        #region Vars
        internal Player[,] Board;

        internal int RowsBoard;
        internal int ColumnsBoard;
        internal int NeededToWin;
        //internal string Opponent = "🧘‍♂️";

        internal int PlacementCount;
        internal int RowsOfPlacement;
        internal int ColumnsOfPlacement;
        internal bool Draw;
        internal bool WithdrawalActivated;
        internal bool InputValid;
        internal int SignsTogether;
        internal string DisplayalType = "Base";

        internal List<int> PlacementChoiceHistory;
        
        internal List<Player> Players;
        internal Player Winner;
        internal Player CurrentPlayer;
        #endregion
        internal Match(int p_RowsGameField, int p_ColumnsGameField, int p_NeededToWin, List<Player> p_Players)
        {
            RowsBoard = p_RowsGameField;
            ColumnsBoard = p_ColumnsGameField;
            NeededToWin = p_NeededToWin;

            Players = p_Players;

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            InitMatch();
        }
        internal void InitMatch()
        {
            Draw = false;
            SignsTogether = 0;
            PlacementCount = 0;
            PlacementChoiceHistory = new List<int>();

            Board = new Player[RowsBoard, ColumnsBoard];
            Random PlayerRandomizer = new();
            CurrentPlayer = Players[PlayerRandomizer.Next(Players.Count)];

            Console.Clear();
        }
        internal virtual void DisplayBoard()
        {
            Programm.SetSystemColor();
            Console.Clear();

            if (InputValid)
            {
                CommentMove();
            }
            else
            {
                Console.WriteLine("\n");
            }

            int FieldsDisplayed = 0;

            for (int RowCounter = 0; RowCounter < RowsBoard; RowCounter++)
            {
                for (int ColumnCounter = 0; ColumnCounter < ColumnsBoard; ColumnCounter++)
                {

                    if (Board[RowCounter, ColumnCounter] == null)
                    {

                        for (int BlanksCounter = 0; BlanksCounter + (FieldsDisplayed + 1).ToString().Length < 3; BlanksCounter++)
                        {
                            Console.Write(" ");
                        }

                        Console.Write($"{FieldsDisplayed + 1}");

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
        internal void NextPlayer()
        {
            int NextPlayerIndex = Players.IndexOf(CurrentPlayer) + 1;

            if (NextPlayerIndex > Players.Count - 1)
                NextPlayerIndex = 0;

            CurrentPlayer = Players[NextPlayerIndex];
        }
        internal void GetAndPlaceInput()
        {
            GetPlacementChoice();
            GetPlacementPoint();
            Place();
        }
        internal void GetPlacementChoice()
        {
            int PlacementChoice;
            do
            {
                Console.WriteLine("");
                Console.WriteLine(CurrentPlayer.PlayerName + " Bitte ein Feld aussuchen um " + CurrentPlayer.Sign + " zu plazieren und mit Enter bestätigen");
                string UnfilteredInput = Console.ReadLine();

                InputValid = false;
                WithdrawalActivated = false;

                if (int.TryParse(UnfilteredInput, out PlacementChoice))
                {

                    if (CheckInput(PlacementChoice))
                    {
                        int SavedPlacementChoice = PlacementChoice;
                        PlacementChoiceHistory.Add(SavedPlacementChoice);
                        InputValid = true;
                    }
                    else
                    {

                        WrongInput();
                    }
                }
                else
                {
                    if (UnfilteredInput == "z"&& PlacementCount>0)
                    {
                        WithdrawalActivated = true;
                        InputValid = true;
                        PlacementCount--;
                    }
                    else
                    {
                        WrongInput();
                    }
                }

            } while (InputValid == false);

        }
        internal virtual bool CheckInput(int p_PlacementChoice)
        {
            if (p_PlacementChoice >ColumnsBoard*RowsBoard || p_PlacementChoice < 1)
            {
                return false;
            }
            else
            {
                if (Board[(p_PlacementChoice - 1) / ColumnsBoard, (p_PlacementChoice - 1) % ColumnsBoard] != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        internal void WrongInput()
        {
            Console.WriteLine("\tFalsche Eingabe\n\n\t Zum wiederholen der Eingabe Enter drücken");
            Console.ReadKey();
            Console.Clear();
            DisplayBoard();
        }
        internal virtual void GetPlacementPoint()
        {
            

        }
        internal void Place()
        {
            if (WithdrawalActivated == false)
            {
                Board[RowsOfPlacement, ColumnsOfPlacement] = CurrentPlayer;
                PlacementCount++;
            }
            else
            {
                Board[RowsOfPlacement, ColumnsOfPlacement] = null;
                PlacementChoiceHistory.Remove(PlacementChoiceHistory[PlacementCount]);
                
            }
        }
        internal void CommentMove()
        {
            if (PlacementCount < 0)
            {
                Console.WriteLine("\n");
            }
            else
            {
                List<string> Comments = new() { "Eine Entäuschender Wahl", "Grandioser Zug", "Wow!", "Es gibt nur ein Rudi Völler", "Das war nur Glück", "Hmmmmmm", "So spielt ein Wahrer Champion", "Wirklich?", "Aufgepasst", "+10 garnichts" };

                Random Random = new();
                int CommentSaid = Random.Next(1, Comments.Count * 3);
                if (CommentSaid > Comments.Count)
                {
                    Console.WriteLine("\n");
                }
                else
                {
                    Console.WriteLine("🧘‍♂" + Comments[CommentSaid-1]+"\n");
                }
            }
        }
        #region CheckGameState und Unterfunktionen
        internal GameState CheckGameState()
        {

            if (CheckForWin())
            {
                Winner = CurrentPlayer;
                return GameState.HasWinner;
            }
            else if(IsDraw())
            {
                return GameState.Draw;
            }
            else
            {
                return GameState.IsRunning;
            }


        }
        internal bool CheckForWin()
        {
            SignsTogether = 0;

            if (CheckHorizontal())
            {
                return true;
            }

            SignsTogether = 0;
            if (CheckVertical())
            {
                return true;
            }
            SignsTogether = 0;
            if (CheckDecliningDiagonal())
            {
                return true;
            }
            SignsTogether = 0;
            if (CheckRisingDiagonal())
            {
                return true;
            }

            return false;
        }
        internal bool IsDraw()
        {

            if (PlacementCount == ColumnsBoard * RowsBoard)
            {
                Draw = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        internal bool CheckHorizontal()
        {
            int HorizontalMover = 0;

            while (ColumnsOfPlacement + HorizontalMover < ColumnsBoard - 1 && Board[RowsOfPlacement, ColumnsOfPlacement + HorizontalMover + 1] == CurrentPlayer)
            {
                HorizontalMover++;
            }

            while (HorizontalMover + ColumnsOfPlacement >= 0 && Board[RowsOfPlacement, ColumnsOfPlacement + HorizontalMover] == CurrentPlayer)
            {
                SignsTogether++;
                HorizontalMover--;
                if (NeededToWin == SignsTogether)
                {
                    return true;
                }
            }
            return false;
        }
        internal bool CheckVertical()
        {
            int VerticalMover = 0;


            while (RowsOfPlacement + VerticalMover < RowsBoard - 1 && Board[RowsOfPlacement + VerticalMover + 1, ColumnsOfPlacement] == CurrentPlayer)
            {
                VerticalMover++;
            }

            while (RowsOfPlacement + VerticalMover >= 0 && Board[RowsOfPlacement + VerticalMover, ColumnsOfPlacement] == CurrentPlayer)
            {
                SignsTogether++;
                VerticalMover--;
                if (NeededToWin == SignsTogether)
                {
                    return true;
                }
            }
            return false;
        }
        internal bool CheckDecliningDiagonal()
        {
            int DiagonalMoverTop = 0;
            while (RowsOfPlacement > DiagonalMoverTop && ColumnsOfPlacement > DiagonalMoverTop && CurrentPlayer == Board[RowsOfPlacement - DiagonalMoverTop - 1, ColumnsOfPlacement - DiagonalMoverTop - 1])
            {
                DiagonalMoverTop++;
            }


            while (RowsOfPlacement - DiagonalMoverTop < RowsBoard && ColumnsOfPlacement - DiagonalMoverTop < ColumnsBoard && Board[RowsOfPlacement - DiagonalMoverTop, ColumnsOfPlacement - DiagonalMoverTop] == CurrentPlayer)
            {
                SignsTogether++;
                if (NeededToWin == SignsTogether)
                {
                    return true;
                }
                else
                {
                    DiagonalMoverTop--;
                }
            }

            return false;
        }
        internal bool CheckRisingDiagonal()
        {
            int DiagonalMoverBottom = 0;

            while (RowsOfPlacement + DiagonalMoverBottom + 1 < RowsBoard && ColumnsOfPlacement > DiagonalMoverBottom && CurrentPlayer == Board[RowsOfPlacement + DiagonalMoverBottom + 1, ColumnsOfPlacement - DiagonalMoverBottom - 1])
            {
                DiagonalMoverBottom++;
            }


            while (RowsOfPlacement + DiagonalMoverBottom >= 0 && ColumnsOfPlacement - DiagonalMoverBottom < ColumnsBoard && Board[RowsOfPlacement + DiagonalMoverBottom, ColumnsOfPlacement - DiagonalMoverBottom] == CurrentPlayer)
            {

                SignsTogether++;
                if (NeededToWin == SignsTogether)
                {
                    return true;
                }
                else
                {
                    DiagonalMoverBottom--;
                }
            }
            return false;
        }
        #endregion
    }
}
