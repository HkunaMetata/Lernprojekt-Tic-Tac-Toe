// See https://aka.ms/new-console-template for more information

using System.Data;
using System.Data.SqlClient;
using Lerneinheit_2;
using Lerneinheit_2.Models;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Security.Cryptography;
class Programm
{
    #region Vars
    static Game CurrentGame;

    static PostGameOptions RevancheWanted;
    //public static object UnfilteredInput { get; private set; }

    #endregion
    static void Main()
    {
        bool BackToMenu ;
        do
        {
            CurrentGame = MainMenu();

            if(CurrentGame == null)
            {
                BackToMenu = true;
            }
            else
            {
                BackToMenu = !CurrentGame.OptionMenu();
            }

        } while (BackToMenu );
        CurrentGame.AddPlayers();

        do
        {
            Match ActiveMatch = CurrentGame.CreateMatch();

            DataManager.SaveGame(CurrentGame);
            DataManager.SaveMatch();
            do
            {
                ActiveMatch.DisplayBoard();
                ActiveMatch.NextPlayer();
                ActiveMatch.GetAndPlaceInput();
                DataManager.SaveMove(ActiveMatch.CurrentPlayer, ActiveMatch.PlacementChoiceHistory[ActiveMatch.PlacementCount-1]);
            } while (ActiveMatch.CheckGameState() == GameState.IsRunning);
            DataManager.AddWinnerToMatch(ActiveMatch.CurrentPlayer.Ident);
            DataManager.AddStatsToPlayers(ActiveMatch.Players, ActiveMatch.Draw, ActiveMatch.CurrentPlayer);

            CurrentGame.MatchHistory.Add(ActiveMatch);

            RevancheWanted = CurrentGame.EndScreen();
        } while (RevancheWanted == PostGameOptions.Rematch);

    }
    static Game MainMenu()
    {
        bool GameChoiceValid;
        Game InterimGame = null;
        int GameChoice;
        do
        {
            Console.Clear();
            SetSystemColor();
            Console.WriteLine("\t\t\t\tSpielWahl\n\n\n(1)Tic Tac Toe\n(2)Vier Gewinnt\n(3)Leaderboard\n(4)Spieler-Statistik");

            ConsoleKeyInfo UnfilteredChoice = Console.ReadKey();
            GameChoiceValid = int.TryParse(UnfilteredChoice.KeyChar.ToString(), out GameChoice);


        } while (!GameChoiceValid || GameChoice > 4 || GameChoice < 1);

        if (GameChoice == 1)
        {
            InterimGame = new Game(GameType.TicTacToe)
            {
                RowsMatch = 3,
                ColumnsMatch = 3,
                WinCondition = 3
            };
        }
        else if (GameChoice == 2)
        {
            InterimGame = new Game(GameType.ConnectFour)
            {
                RowsMatch = 6,
                ColumnsMatch = 7,
                WinCondition = 4
            };
        }
        else if (GameChoice == 3)
        {
            Console.Clear();
            Console.WriteLine("Um zurück zum Hauptmenü zu gelangen 'z' drücken\n");
            DataManager.CreateLeaderboard();
            Console.ReadKey();
            return null;
        }
        else if (GameChoice == 4)
        {
            string NameToSearch = null;
            do
            {
                Console.Clear();
                if(NameToSearch != null)
                {
                    Console.Write("Dieser Name konnte nicht gefunden werden");
                }
                Console.WriteLine("\n\n\nNamen eingeben dessen Statistik sie sehen wollen:");
                NameToSearch = Console.ReadLine();
            } while (!DataManager.IsNameExisting(NameToSearch));

            Console.Clear();
            Console.WriteLine("Um zurück zum Hauptmenü zu gelangen 'z' drücken\n");
            DataManager.GetStatsToName(NameToSearch);
            Console.ReadKey();
            return null;

        }
        return InterimGame;
    }
    internal static void SetSystemColor()
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Cyan;
    }
    internal static void SetPlayerColor()
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
    }
}

