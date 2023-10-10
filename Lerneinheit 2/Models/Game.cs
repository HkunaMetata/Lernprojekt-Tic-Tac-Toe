using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lerneinheit_2.Models
{
    internal class Game
    {



        #region Properties


        public int Draws
        {
            get => MatchHistory.Count(OldMatchs => OldMatchs.Draw == true);
        }
        #endregion

        #region vars
        internal int NumberPlayers = 2;
        internal int RowsMatch;
        internal int ColumnsMatch;
        internal int WinCondition;
        //internal string OpponentMatch = "🧘‍♂️";

        internal GameType Type;

        static bool SetDefaultNames = false;
        internal bool ValidNumber = true;

        internal List<Match> MatchHistory = new();
        internal List<Player> Players = new();
        internal List<string> Signs = new() { "❌", "⭕", "+", "\\", "❤️", "🚢", "🟨", "🟦", "🔴", "🥐" };
        //internal List<string> Opponents = new() { "🧘‍♂","🎅","⚽","🫅" };
        #endregion
        internal Game(GameType p_GameType)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Type = p_GameType;
        }
        internal void AddPlayers()
        {



            Programm.SetSystemColor();

            if (SetDefaultNames)
            {
                for (int PlayersAdded = 0; PlayersAdded < NumberPlayers; PlayersAdded++)
                {
                    Players.Add(new Player("Player" + Signs[PlayersAdded], Signs[PlayersAdded]));
                }
            }
            else
            {
                bool PlayerWasAdded;

                for (int PlayersAdded = 0; PlayersAdded < NumberPlayers; PlayersAdded++)
                {
                    do
                    {
                        PlayerWasAdded = AddIndividualPlayers(PlayersAdded);
                    } while (PlayerWasAdded == false);
                }
            }

        }
        internal bool AddIndividualPlayers(int p_PlayersAdded)
        {

            string NameInput;

            Console.Write($"Zum Anmelden + oder zum registrieren ++ als Namen Eingeben\n\n\n\tSpieler{p_PlayersAdded + 1} Bitte Namen Eingeben: ");
            NameInput = Console.ReadLine();

            if (NameInput != "+" && NameInput != "++")
            {
                Players.Add(new Player(NameInput, GetSign()));
            }

            else
            {
                bool PlayerAdded = false;
                do
                {
                    if (NameInput == "+")
                    {
                        Player PlayerLogingIn = DataManager.PlayerLogin();
                        if (PlayerLogingIn == null)
                        {
                            return false;
                        }
                        else
                        {
                            PlayerLogingIn.Sign = GetSign();
                            Players.Add(PlayerLogingIn);
                            PlayerAdded = true;
                            Programm.SetSystemColor();
                        }
                    }


                    if (NameInput == "++")
                    {
                        Player PlayerWhoRegisterd = DataManager.CreatePlayerAccount();
                        if (PlayerWhoRegisterd == null)
                        {
                            return false;
                        }
                        else
                        {
                            PlayerWhoRegisterd.Sign = GetSign();
                            Players.Add(PlayerWhoRegisterd);
                            PlayerAdded = true;
                            Programm.SetSystemColor();
                        }
                    }
                } while (PlayerAdded == false);
            }
            return true;

        }
        internal bool OptionMenu()
        {
            char SettingChoice;
            bool OptionsAccepted = false;

            do
            {

                Console.Clear();
                DisplaySettings();

                Console.SetCursorPosition(0, 16);
                Console.Write("Neue Anzahl Auswählen für Option ( ):");
                Console.SetCursorPosition(34, 16);
                ConsoleKeyInfo SettingChoiceInput = Console.ReadKey();
                SettingChoice = SettingChoiceInput.KeyChar;
                Console.SetCursorPosition(37, 16);
                switch (SettingChoice)
                {
                    case '1':
                        NumberPlayers = GetNumber(0, 5, NumberPlayers);
                        break;
                    case '2':
                        ColumnsMatch = GetNumber(0, 18, ColumnsMatch);
                        break;
                    case '3':
                        RowsMatch = GetNumber(0, 18, RowsMatch);
                        break;
                    case '4':
                        WinCondition = GetNumber(0, 18, WinCondition);
                        break;
                    case '\t':
                        SetDefaultNames = true;
                        OptionsAccepted = true;
                        break;
                    case '\r':
                        OptionsAccepted = true;
                        break;
                    case '\x1B':
                        Console.Clear();
                        return false;
                }
            } while (OptionsAccepted == false);
            Console.Clear();
            return true;
        }
        internal void DisplaySettings()
        {
            Console.WriteLine($"\n(1)Anzahl Spieler: {NumberPlayers}\n\n(2)Spielfelder Horizontal: {ColumnsMatch}\n\n(3)Spielfelder Vertikal: {RowsMatch}" +
                $"\n\n(4)Zum Sieg benötigte Zeichen Hintereinander: {WinCondition}\n\n\n(tab)Um  mit Automatischen Namen Fortzufahren\n\n" +
                $"(Enter)Wenn du Deine Einstellungen Übernehmen Möchtest\n\n\n\n\n");
            if (ValidNumber == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Ihre Anzahl gibt es nicht.Bitte Eingabe wiederholen");
                Programm.SetSystemColor();
            }
            Console.WriteLine("\n\n\n\n\n\n\n\n\n(Esc)Zurück");
        }
        internal int GetNumber(int LowestValid, int HighestValid, int OriginalNumber)
        {
            ValidNumber = true;
            int Number;
            if (!int.TryParse(Console.ReadLine(), out Number))
            {
                return OriginalNumber;
            }
            else if (Number < LowestValid || Number > HighestValid)
            {
                ValidNumber = false;
                return OriginalNumber;
            }
            else
            {
                return Number;
            }

        }
        internal string GetSign()
        {
            bool SignInputControl;
            int SignChoice;

            do
            {
                for (int SignsDisplayed = 0; SignsDisplayed < Signs.Count; SignsDisplayed++)
                {
                    Console.WriteLine($"({SignsDisplayed}) " + Signs[SignsDisplayed]);
                }
                Console.Write("Wählen sie ein zeichen:");
                ConsoleKeyInfo UnfilteredChoice = Console.ReadKey();

                SignInputControl = int.TryParse(UnfilteredChoice.KeyChar.ToString(), out SignChoice);
                Console.Clear();
            } while (!SignInputControl);

            return Signs[SignChoice];
        }
        internal Match CreateMatch()
        {
            Match PlaySequence = null;
            if (Type == GameType.TicTacToe)
            {
                PlaySequence = new TicTacToe(RowsMatch, ColumnsMatch, WinCondition, Players);
            }
            else if (Type == GameType.ConnectFour)
            {
                PlaySequence = new ConnectFour(RowsMatch, ColumnsMatch, WinCondition, Players);
            }
            return PlaySequence;
        }
        internal int CountWins(Player p_Player)
        {
            return MatchHistory.Count(OldMatchs => OldMatchs.Winner == p_Player);
        }
        internal PostGameOptions EndScreen()
        {
            char SelectionAfterGame;
            do
            {
                Console.Clear();
                MatchHistory[MatchHistory.Count - 1].DisplayBoard();
                Console.WriteLine("\t\t\t\t\tSpiel zuende\n\n \t\t\t\t\tSpielstand\n\n");

                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                Console.ForegroundColor = ConsoleColor.White;

                for (int ResultsDisplayed = 0; ResultsDisplayed < Players.Count; ResultsDisplayed++)
                {
                    Console.Write("Siege " + Players[ResultsDisplayed].PlayerName + ": " + CountWins(Players[ResultsDisplayed]) + "\t");
                }

                Console.WriteLine("Unentschieden: " + Draws);
                Programm.SetSystemColor();

                Console.WriteLine("\n\n\t\t\t\t(Enter)Erneut spielen\n\n" +
                                 $"\t\t\t\t(esc){Type} beenden\n\n");
                ConsoleKeyInfo UnfilteredChoice = Console.ReadKey();
                SelectionAfterGame = UnfilteredChoice.KeyChar;

                char.ToLower(SelectionAfterGame);

                switch (SelectionAfterGame)
                {
                    case '\r':
                        return PostGameOptions.Rematch;
                    case '\x1B':
                        return PostGameOptions.Quit;
                }

                Console.Clear();
                Console.WriteLine("Falsche TastenEingabe\tBitte Enter entf oder esc drücken\n\nUm ihre Auswahl zu wiederholen Enter drücken");
                Console.ReadKey();

            } while (true);
        }

    }
    public enum GameType
    {
        TicTacToe,
        ConnectFour
    }
}
