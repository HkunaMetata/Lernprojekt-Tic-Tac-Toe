using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Lerneinheit_2.Models
{
    internal static class DataManager
    {
        static string SqlConnectionString = "Data Source=zub-PC144\\SQLEXPRESS01;" + "Initial Catalog=GameCollection;" + "User ID=Gustaf;" + "Password=4321;";

        public static Player CreatePlayerAccount()
        {
            bool MistakeOccured;
            string PlayerName = "";
            string Mail = "";
            string Password = "";

            using (SqlConnection ConnToGameCollection = new SqlConnection(SqlConnectionString))
            {
                ConnToGameCollection.Open();
                do
                {
                    if (PlayerName != "")
                    {
                        Console.WriteLine("Erneut Registrierung Probieren(Enter) oder zurück(z)");

                        bool LoginChoice = false;
                        do
                        {
                            ConsoleKeyInfo ContinueLoginChoice = Console.ReadKey();
                            if (ContinueLoginChoice.KeyChar == '\r')
                            {
                                LoginChoice = true;
                            }
                            else if (Char.ToLower(ContinueLoginChoice.KeyChar) == 'z')
                            {
                                Console.Clear();
                                return null;
                            }

                        } while (!LoginChoice);

                    }

                    using (SqlCommand CreatePlayerAccount = ConnToGameCollection.CreateCommand())
                    {
                        CreatePlayerAccount.CommandType = CommandType.StoredProcedure;
                        CreatePlayerAccount.CommandText = "CreatePlayerAccount";

                        MistakeOccured = false;
                        Console.SetCursorPosition(0, 0);
                        Console.Write($"\n\n\n\t Standard Benutzer Namen Eingeben(max.99 z.):{PlayerName}");

                        if (PlayerName == "" && MistakeOccured == false)
                        {
                            PlayerName = Console.ReadLine();
                            if (Mail.Length > 99 || Mail.Length < 0)
                            {
                                PlayerName = "";
                                MistakeOccured = true;
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("99 zeichen sind doch wirklich genug zeichen");
                                Programm.SetSystemColor();
                            }
                            else
                            {
                                CreatePlayerAccount.Parameters.Add("@p_PlayerName", SqlDbType.VarChar, 99).Value = PlayerName;
                            }
                        }
                        else
                        {
                            CreatePlayerAccount.Parameters.Add("@p_PlayerName", SqlDbType.VarChar, 99).Value = PlayerName;
                        }

                        Console.Write($"\n\t Einmalige ID Namen Eingeben(max. 40 z.):{Mail}");
                        if (Mail == "" && MistakeOccured == false)
                        {
                            Mail = Console.ReadLine();
                            if (Mail.Length > 40 || Mail.Length < 4)
                            {
                                Mail = "";
                                MistakeOccured = true;
                                Console.Clear();
                                Console.WriteLine();
                            }
                            else
                            {
                                CreatePlayerAccount.Parameters.Add("@p_Mail", SqlDbType.VarChar, 40).Value = Mail;
                            }
                        }

                        Console.Write($"\n\t Passwort Eingeben:{Password}");
                        if (Password == "" && MistakeOccured == false)
                        {
                            Password = Console.ReadLine();
                            if (Password.Length > 20 || Mail.Length < 4)
                            {
                                Password = "";
                                MistakeOccured = true;
                                Console.Clear();
                                Console.WriteLine("Die Passwortlänge enspricht nicht den Vorgaben (min.4 max20)");
                            }
                            else
                            {
                                CreatePlayerAccount.Parameters.Add("@p_Password", SqlDbType.VarChar, 20).Value = Password;
                            }
                        }
                        else
                        {
                            CreatePlayerAccount.Parameters.Add("@p_Password", SqlDbType.VarChar, 20).Value = Password;
                            Console.Clear();
                        }



                        try
                        {
                            CreatePlayerAccount.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            MistakeOccured = true;
                            Console.WriteLine($"Fehler {ex.Number}");
                            switch (ex.Number)
                            {
                                case 2627:
                                    Mail = "";
                                    Console.Clear();
                                    Console.WriteLine("Es existiert bereits ein Account mit dieser Namens ID oder die Zeichnanzahl ist invalide ");
                                    break;
                                default:
                                    PlayerName = "";
                                    Mail = "";
                                    Password = "";
                                    Console.WriteLine(ex.Message);
                                    break;
                            }
                        }

                    }

                } while (MistakeOccured);
                ConnToGameCollection.Close();
            }
            return new Player(PlayerName, "++", GetLastIdent("PlayerAccounts"));
        }
        public static Player PlayerLogin()
        {
            int AccountFound = 0;
            string PlayerName = "";
            string Mail = "";
            string Password = "";
            int PlayerIdent = 0;
            Console.Clear();
            using (SqlConnection ConnToGameCollection = new SqlConnection(SqlConnectionString))
            {
                ConnToGameCollection.Open();

                do
                {
                    if (Mail != "" || Password != "")
                    {
                        Console.WriteLine("Erneut Login Probieren(Enter) oder Zurück(z)");

                        bool LoginChoice = false;
                        do
                        {
                            ConsoleKeyInfo ContinueLoginChoice = Console.ReadKey();
                            if (ContinueLoginChoice.KeyChar == '\r')
                            {
                                LoginChoice = true;
                            }
                            else if (Char.ToLower(ContinueLoginChoice.KeyChar) == 'z')
                            {
                                Console.Clear();
                                return null;
                            }

                        } while (!LoginChoice);

                    }
                    using (SqlCommand LoginCmd = ConnToGameCollection.CreateCommand())
                    {
                        LoginCmd.CommandType = CommandType.StoredProcedure;
                        LoginCmd.CommandText = "SignUp";

                        Console.Clear();

                        Console.Write("\n\n\n\t\tBitte geben sie ihre IDNamen ein:");
                        Mail = Console.ReadLine();

                        Console.Write("\n\n\t\tBitte geben sie ihr Passwort ein:");
                        Password = Console.ReadLine();

                        LoginCmd.Parameters.Add("@p_Mail", SqlDbType.VarChar, 40).Value = Mail;
                        LoginCmd.Parameters.Add("@p_Password", SqlDbType.VarChar, 20).Value = Password;

                        SqlParameter outputParameter = new SqlParameter("@AccountDataCorrect", SqlDbType.Int);
                        outputParameter.Direction = ParameterDirection.ReturnValue;
                        LoginCmd.Parameters.Add(outputParameter);

                        try
                        {
                            AccountFound = LoginCmd.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine(ex.Message + ex.Number);
                        }

                        AccountFound = Convert.ToInt32(outputParameter.Value);



                    }
                } while (AccountFound == 0);


                using (SqlCommand GetNameFromMailCmd = ConnToGameCollection.CreateCommand())
                {
                    GetNameFromMailCmd.CommandText = $"SELECT PlayerName FROM PlayerAccounts WHERE Mail = '{Mail}'";

                    using (SqlDataReader reader = GetNameFromMailCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            //wenn ich diese schleife Fußgesteuert mache werden trotz der gleichen eingabe keine Daten gefunden
                            while (reader.Read())
                            {
                                PlayerName = reader.GetString(0);
                            }
                        }
                    }
                }

                using (SqlCommand GetIdentFromEmailCmd = ConnToGameCollection.CreateCommand())
                {
                    GetIdentFromEmailCmd.CommandText = $"SELECT Ident FROM PlayerAccounts WHERE Mail = '{Mail}'";

                    PlayerIdent = Convert.ToInt32(GetIdentFromEmailCmd.ExecuteScalar());

                }

                return new Player(PlayerName, "+", PlayerIdent);

            }
        }
        public static void AddStatsToPlayers(List<Player> p_PlayersOfMatch,bool p_Draw,Player p_Matchwinner)
        {
            using (SqlConnection conn = new SqlConnection(SqlConnectionString))
            {
                conn.Open();

                for (int StatsUpdated = 0; StatsUpdated < p_PlayersOfMatch.Count ; StatsUpdated++)
                {

                    using (SqlCommand Cmd = conn.CreateCommand())
                    {
                        if (p_PlayersOfMatch[StatsUpdated].Ident != 1)
                        {
                            if (p_Draw)
                            {
                                Cmd.CommandText = $"UPDATE PlayerAccounts SET MatchesDrawn = MatchesDrawn +1 WHERE Ident = {p_PlayersOfMatch[StatsUpdated].Ident} ";
                            }
                            else if (p_PlayersOfMatch[StatsUpdated] == p_Matchwinner)
                            {
                                Cmd.CommandText = $"UPDATE PlayerAccounts SET MatchesWon = MatchesWon +1 WHERE Ident = {p_PlayersOfMatch[StatsUpdated].Ident} ";
                            }
                            else
                            {
                                Cmd.CommandText = $"UPDATE PlayerAccounts SET MatchesLost = MatchesLost +1 WHERE Ident = {p_PlayersOfMatch[StatsUpdated].Ident} ";
                            }

                            try
                            {
                                Cmd.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                    }
                }

            }
        }
        public static void SaveGame(Game p_GameToSave)
        {
            using (SqlConnection ConnToGameCollection = new SqlConnection(SqlConnectionString))
            {
                ConnToGameCollection.Open();
                using (SqlCommand SaveGameCommand = ConnToGameCollection.CreateCommand())
                {
                    SaveGameCommand.CommandType = CommandType.StoredProcedure;
                    SaveGameCommand.CommandText = "SaveGame";

                    SaveGameCommand.Parameters.Add("@p_GameColumns", SqlDbType.SmallInt).Value = p_GameToSave.ColumnsMatch;
                    SaveGameCommand.Parameters.Add("@p_GameRows", SqlDbType.SmallInt).Value = p_GameToSave.RowsMatch;
                    SaveGameCommand.Parameters.Add("@p_WinCondition", SqlDbType.SmallInt).Value = p_GameToSave.WinCondition;
                    SaveGameCommand.Parameters.Add("@p_GameType", SqlDbType.VarChar, 99).Value = p_GameToSave.Type;

                    try
                    {
                        SaveGameCommand.ExecuteNonQuery();
                    }
                    catch (SqlException Mistake)
                    {
                        Console.WriteLine(Mistake.Message);
                    }
                }
                ConnToGameCollection.Close();

            }
        }
        public static void SaveMatch()
        {

            using (SqlConnection ConnToGameCollection = new SqlConnection(SqlConnectionString))
            {
                ConnToGameCollection.Open();

                using (SqlCommand SaveMatchCommand = ConnToGameCollection.CreateCommand())
                {
                    SaveMatchCommand.CommandType = CommandType.StoredProcedure;
                    SaveMatchCommand.CommandText = "SaveMatch";

                    SaveMatchCommand.Parameters.Add("@p_GameID", SqlDbType.SmallInt).Value = GetLastIdent("Games");

                    SaveMatchCommand.Parameters.Add("@p_WinnerID", SqlDbType.VarChar).Value = 0;

                    try
                    {
                        SaveMatchCommand.ExecuteNonQuery();
                    }
                    catch (SqlException Mistake)
                    {
                        Console.Write(Mistake.Message);
                        Console.ReadKey();
                    }

                }

            }
        }
        public static void AddWinnerToMatch(int p_WinnerIdent)
        {
            using (SqlConnection ConnToGameCollection = new SqlConnection(SqlConnectionString))
            {
                ConnToGameCollection.Open();

                using (SqlCommand AddToMatchCmd = ConnToGameCollection.CreateCommand())
                {
                    AddToMatchCmd.CommandText = "UPDATE Matches " +
                        $"SET Winner = {p_WinnerIdent}" +
                        $"WHERE Ident = {GetLastIdent("Matches")}";
                }

            }
        }
        public static void SaveMove(Player p_PlayerWhoPlaced, int p_PlacementChoice)
        {
            using (SqlConnection ConnToGameCollection = new SqlConnection(SqlConnectionString))
            {
                ConnToGameCollection.Open();

                using (SqlCommand SaveMoveCmd = ConnToGameCollection.CreateCommand())
                {
                    SaveMoveCmd.CommandType = CommandType.StoredProcedure;
                    SaveMoveCmd.CommandText = "SaveMove";

                    SaveMoveCmd.Parameters.Add("@p_MatchID", SqlDbType.SmallInt).Value = GetLastIdent("Matches");
                    SaveMoveCmd.Parameters.Add("@p_PlayerID", SqlDbType.SmallInt).Value = p_PlayerWhoPlaced.Ident;
                    SaveMoveCmd.Parameters.Add("@p_PlacementChoice", SqlDbType.SmallInt).Value = p_PlacementChoice;
                    SaveMoveCmd.Parameters.Add("@p_Sign", SqlDbType.NChar).Value = p_PlayerWhoPlaced.Sign;

                    try
                    {
                        SaveMoveCmd.ExecuteNonQuery();
                    }
                    catch (SqlException Mistake)
                    {
                        Console.Write(Mistake.Message);
                        Console.ReadKey();
                    }

                }

            }
        }
        public static void CreateLeaderboard()
        {
            using(SqlConnection connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM [LeaderBoard]";

                    using (SqlDataReader Reader = cmd.ExecuteReader())
                    {
                        int StatsDisplayed = 0;

                        Console.WriteLine("Name\t\tMatches Won\t lost\t Drawn\t\t Playerscore");
                        while (Reader.Read())
                        {
                            Console.WriteLine($"{StatsDisplayed}. {Reader.GetString(0)}\t\t {Reader.GetInt16(1)}\t {Reader.GetInt16(2)}\t {Reader.GetInt16(3)}\t\t {Reader.GetByte(4)}");
                            StatsDisplayed++;
                        }
                    }
                }
            }

        }
        public static bool IsNameExisting(string p_Name)
        {
            using( SqlConnection Connection = new SqlConnection(SqlConnectionString) )
            {
                Connection.Open();
                using(SqlCommand CmdCheckName = Connection.CreateCommand())
                {
                    CmdCheckName.CommandText = "SELECT * FROM PlayerAccounts WHERE PlayerName = @p_Name";
                    CmdCheckName.Parameters.AddWithValue("@p_Name", p_Name);

                    using (SqlDataReader reader = CmdCheckName.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

            }
        }
        public static void GetStatsToName(string p_PlayerName)
        {
            using(SqlConnection Conn = new SqlConnection(SqlConnectionString))
            {
                Conn.Open();
                using(SqlCommand CmdGetStats = Conn.CreateCommand())
                {
                    CmdGetStats.CommandType = CommandType.StoredProcedure;
                    CmdGetStats.CommandText = "GetPlayerStats";

                    CmdGetStats.Parameters.Add("@p_PlayerName",SqlDbType.VarChar,99).Value = p_PlayerName;

                    using (SqlDataReader Reader = CmdGetStats.ExecuteReader())
                    {

                        Console.WriteLine("Name\t\tMatches Won\t lost\t Drawn\t\t Total Games");
                        while (Reader.Read())
                        {
                            Console.WriteLine($"{Reader.GetString(0)}\t\t {Reader.GetInt16(1)}\t {Reader.GetInt16(2)}\t {Reader.GetInt16(3)}\t\t {Reader.GetInt16(4)}");
                        }
                    }

                }
            }

        }
        public static int GetLastIdent(string p_Table)
        {
            using (SqlConnection ConnToGameCollection = new SqlConnection(SqlConnectionString))
            {
                ConnToGameCollection.Open();

                using (SqlCommand CmdGetLastRecord = ConnToGameCollection.CreateCommand())
                {
                    CmdGetLastRecord.CommandText = $"SELECT MAX(Ident) FROM {p_Table}";
                    int HighestIdent = Convert.ToInt32(CmdGetLastRecord.ExecuteScalar());

                    return HighestIdent;
                }
            }
        }
    }


}

