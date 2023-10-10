using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lernprojekt_Tic_Tac_Toe.Models
{
    internal class Game
    {
        /*
        #region Vars
        static char[] PlayArea = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };


        internal string FutureNameX;
        internal string FutureNameO;
        internal Player PlayerX;
        internal Player PlayerO;
        internal Player FirstPlayer;
        internal string GameName;

        internal Game AktivGame;

        internal char SignFirstUser;
        internal char SignThatsPlaced;
        internal int RandomizedOrderSetter;
        internal int TurnCount = 0;

        internal int PlacementPoint;
        internal int VictorysX = 0;
        internal int Victorys0 = 0;
        internal int Draws = 0;

        internal char GameChoice = 'z';
        static char SelectionAfterGame;
        //Name
        static bool ValidPlacementPoint;
        internal bool Solution1;
        internal bool Solution2;
        internal bool Solution3;
        internal bool Solution4;
        internal bool Solution5;
        internal bool Solution6;
        internal bool Solution7;
        internal bool Solution8;
        #endregion

        public Game(string p_GameName,string PlayerOne, string PlayerTwo)
        {
            GameName = p_GameName;
            PlayerX = new Player(PlayerXName);
            PlayerO = new Player(PlayerOName);
            

        }

        internal void Greeting()
        {
            SetSystemColor();
            Console.WriteLine("\t\t\t\t\t Tic Tac Toe");

            Console.WriteLine("\n\n\n\n Zum Fortfahren Enter drücken");
            Console.ReadKey();
            
        }

        internal void InitPlayers()
        {
            Console.Clear();
            Console.WriteLine("\t\t\t\t geben sie Spielernamen ein\n\n\n");
            Console.Write("Benutzer mit X: ");
            SetPlayerColor();
            FutureNameX = Console.ReadLine();
            PlayerX.PlayerName = FutureNameX;
            SetSystemColor();
            Console.Write("Benutzer mit 0: ");
            SetPlayerColor();
            FutureNameO = Console.ReadLine();
            PlayerO.PlayerName = FutureNameO;
            SetSystemColor();

        }

        internal void InitGame()
        {
            Console.WriteLine("\n\n\n\n Für Tic Tac Toe T drücken\t\t Für Vier gewinnt V drücken");
            GameChoice = char.ToLower(Console.ReadLine()[0]);

            switch (GameChoice)
            {
                case 't':
                    AktivGame = new Game("Tic Tac Toe", FutureNameX, FutureNameO);
                    break;
                case 'v':
                    AktivGame = new Game("Connect Four", FutureNameX, FutureNameO);
                    break;
                default:
                    Console.WriteLine("Dieses Spiel gibt es leider nicht.\nEnter Drücken um Die Spielauswahl zu wiederholen");
                    Console.ReadKey();
                    Console.Clear();
                    InitGame();//Rekursiv Ok?
                        break;
            }
        }

        internal void InitOrder()
        {
            Random Random = new Random();
            RandomizedOrderSetter = Random.Next(1, 3);
            if (RandomizedOrderSetter == 2)
            {
                SignFirstUser = 'O';
                FirstPlayer = PlayerO;

            }
            else
            {
                SignFirstUser = 'X';
                FirstPlayer = PlayerX;

            }

            Console.Clear();
            SetSystemColor();


            Console.WriteLine("\n\n\t\t\t\t\t " + FirstPlayer.PlayerName + " Beginnt\n\n");

        }

        internal void StartGame()
        {
            DrawPlayArea();
            Console.SetCursorPosition(0, 11);
            if (IsEven(TurnCount))
            {
                Console.WriteLine(FirstPlayer.PlayerName + " Plaziere " + SignFirstUser + " auf Feld _");
                SignThatsPlaced = SignFirstUser;
            }
            else
            {
                if (SignFirstUser == 'X')
                {
                    Console.WriteLine(PlayerO.PlayerName + " Plaziere 0 auf Feld _");

                    SignThatsPlaced = 'O';
                    FirstPlayer = PlayerO;
                }
                else
                {
                    Console.WriteLine(PlayerX.PlayerName + " Plaziere X auf Feld _");
                    SignThatsPlaced = 'X';
                    FirstPlayer = PlayerX;

                }
            }
            Console.SetCursorPosition(21 + FirstPlayer.PlayerName.Length, 11);

            SetPlayerColor();
            bool ValidPlacementPoint = int.TryParse(Console.ReadLine(), out PlacementPoint);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
        }

        internal void PlayGame()
        {

            while (TurnCount < 9)
            {

                InitGame();



                if (ValidPlacementPoint && PlacementPoint < 10 && PlacementPoint > 0)
                {

                    if (PlayArea[PlacementPoint - 1] != 'X' && PlayArea[PlacementPoint - 1] != 'O')
                    {
                        PlayArea[PlacementPoint - 1] = Convert.ToChar(SignThatsPlaced);

                        DrawPlayArea();
                        

                        CheckPlayArea(PlayArea[PlacementPoint - 1]);
                        TurnCount++;

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Dieses Feld ist bereits belegt. Um deine eingabe zu wiederholen drücke Enter");
                        Console.ReadKey();
                        Console.Clear();
                        DrawPlayArea();

                    }

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Feld existiert nicht. um deine Eingabe zu Wiederholen drück Enter");
                    Console.ReadKey();
                    Console.Clear();

                    DrawPlayArea();

                }

            }

            Draws++;
            EndingScreen();

        }
        
        internal void DrawPlayArea()
        {
            SetSystemColor();
            Console.SetCursorPosition(0, 5);
            Console.WriteLine($"\t\t\t {PlayArea[0]}  | {PlayArea[1]}  | {PlayArea[2]} \n\t\t\t---+---+--- \n\t\t\t {PlayArea[3]}  | {PlayArea[4]}  | {PlayArea[5]}\n\t\t\t---+---+--- \n\t\t\t {PlayArea[6]}  | {PlayArea[7]}  | {PlayArea[8]}");


        }

        internal void CheckPlayArea(char LastUser)
        {
            Solution1 = PlayArea[0] == PlayArea[1] && PlayArea[2] == PlayArea[0] && PlayArea[1] != '\0';
            Solution2 = PlayArea[3] == PlayArea[4] && PlayArea[5] == PlayArea[3] && PlayArea[5] != '\0';
            Solution3 = PlayArea[6] == PlayArea[7] && PlayArea[8] == PlayArea[6] && PlayArea[8] != '\0';
            Solution4 = PlayArea[0] == PlayArea[3] && PlayArea[6] == PlayArea[0] && PlayArea[6] != '\0';
            Solution5 = PlayArea[1] == PlayArea[4] && PlayArea[7] == PlayArea[1] && PlayArea[7] != '\0';
            Solution6 = PlayArea[2] == PlayArea[5] && PlayArea[8] == PlayArea[2] && PlayArea[8] != '\0';
            Solution7 = PlayArea[0] == PlayArea[4] && PlayArea[8] == PlayArea[0] && PlayArea[8] != '\0';
            Solution8 = PlayArea[2] == PlayArea[4] && PlayArea[6] == PlayArea[2] && PlayArea[6] != '\0';


            if (Solution1 || Solution2 || Solution3 || Solution4 || Solution5 || Solution6 || Solution7 || Solution8)
            {
                if (LastUser == 'X')
                {
                    VictorysX++;

                }
                else
                {
                    Victorys0++;
                }
                EndingScreen();
            }
        }

        internal void EndingScreen()
        {
            Console.Clear();
            Console.WriteLine("\t\t\t\t\tSpiel zuende\n\n \t\t\t\t\tSpielstand\n\n");
            Console.WriteLine("\t\tSiege " + PlayerO + " :" + Victorys0 + "\t Unentschieden: " + Draws + "\tSiege " + PlayerX + " :" + VictorysX);
            Console.WriteLine("\n\n\t\t\t\tN drücken um erneut zu spielen\n");
            Console.WriteLine("\t\t\t\tE drücken um Tic Tac Toe zu beenden");
            SetPlayerColor();
            SelectionAfterGame = Console.ReadLine()[0];

            if (SelectionAfterGame == 'N' || SelectionAfterGame == 'n')
            {
                ResetGame();
                Console.Clear();
                DrawPlayArea();//Wie playGame aber zeichnet noch
                InitOrder();
            }
            else if (SelectionAfterGame == 'E' || SelectionAfterGame == 'e')
            {

                Environment.Exit(0);
            }
            else
            {
                Console.Clear();
                EndingScreen();
            }

        }

        internal void ResetGame()
        {
            PlayArea = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            TurnCount = 0;





        }

        internal void SetSystemColor()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
        }

        internal void SetPlayerColor()
        {


            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

        }

        //IsEven in Game? Wenn in Programm dann in zeile 189 Objektverweis gefordert. Objekt von Progamm erlaubt?
        internal bool IsEven(int NumberToCheck)
        {
            if (NumberToCheck % 2 == 0)
            { return true; }
            else
            { return false; }

        }*/

        


    }
}
