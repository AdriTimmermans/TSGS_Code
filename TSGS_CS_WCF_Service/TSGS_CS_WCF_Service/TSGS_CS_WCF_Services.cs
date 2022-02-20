using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using FTPLib;

namespace TSGS_CS_WCF_Service
{
    public class TSGS_CS_WCF_Service : ITSGS_CS_WCF_Service
    {
        public bool DebugFlag = true;
        private struct Exchange_Combination
        {
            public int S1, S2, Exchange_Value;
            public Exchange_Combination(int P1, int P2, int E_Value)
            {
                S1 = P1;
                S2 = P2;
                Exchange_Value = E_Value;
            }
        }

        private String GetConfigValue(String key)
        {
            return "5";
            // return System.Web.Configuration.WebConfigurationManager.AppSettings[key].ToString();
        }

        protected string Connection_String_CS()
        {
            string cs = "";
            string Status = ConfigurationManager.AppSettings["Phase"];
            if (Status == "DEV")
            {
                cs = ConfigurationManager.ConnectionStrings["DBCSDEV"].ConnectionString;
            }
            else if (Status == "PROD")
            {
                cs = ConfigurationManager.ConnectionStrings["DBCSPROD"].ConnectionString;
            }
            return cs;
        }

        public CompetitionManager GetCompetitionManager(string CompetitionManagerName)
        {
            //
            // Get the name of the competition manager
            //

            CompetitionManager CompetitionManager = new CompetitionManager();
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetCompetitionManagerData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parameterId = new SqlParameter();
                cmd.Parameters.Add("@Gebruiker", SqlDbType.NVarChar).Value = CompetitionManagerName;
                con.Open();

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    // 
                    // initialize
                    //
                    CompetitionManager.CompetitionManagerId = 0;

                    while (reader.Read())
                    {
                        // 
                        // get relevant data
                        //
                        CompetitionManager.CompetitionManagerName = Convert.ToString(reader["Gebruiker"]);
                        CompetitionManager.Password = Convert.ToString(reader["Wachtwoord"]);
                        CompetitionManager.CompetitionManagerId = Convert.ToInt16(reader["Gebruiker_Id"]);
                        CompetitionManager.EmailAddress = Convert.ToString(reader["EmailAddress"]);
                        CompetitionManager.UserPrivileges = Convert.ToInt16(reader["UserPrivileges"]);
                        CompetitionManager.Fontsize = Convert.ToInt16(reader["Fontsize"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetCompetitionManager", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return CompetitionManager;
        }

        public void SaveCompetitionManager(CompetitionManager CompetitionManager)
        {
            //
            // Save the new combination between the competition manager and a competition
            //
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spSaveCompetitionManagerData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Gebruiker", SqlDbType.NVarChar).Value = CompetitionManager.CompetitionManagerName;
                cmd.Parameters.Add("@Wachtwoord", SqlDbType.NVarChar).Value = CompetitionManager.Password;
                cmd.Parameters.Add("@EmailAddress", SqlDbType.NVarChar).Value = CompetitionManager.EmailAddress;

                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "SaveCompetitionManager", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }

        }
        public DataSet GetCompetitionList(string CompetitionManagerName, int Manager_Id)
        {
            //
            // Get a dataset with all competitions under control of a specific competition manager
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetCompetitieLijst";
                cmd.Parameters.Add("@Gebruiker", SqlDbType.NVarChar).Value = CompetitionManagerName;
                cmd.Parameters.Add("@MID", SqlDbType.Int).Value = Manager_Id;

                da.SelectCommand = cmd;
                con.Open();
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetCompetitionList", "Error", 1, lit_Error);
                    ds = null;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                } con.Close();
            }
            //
            // Result is a dataset to be used in a gridview control
            //
            return ds;
        }

        public bool IsPlayerInChampiongroup(int PID)
        {
            //
            // Get number of the last ound of a specific competition
            //
            bool aux = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spIsPlayerMemberOfChampionsgroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    aux = (Convert.ToInt16(reader["Member_Premier_Group"]) == 1);
                }

            }
            //
            // return value is a boolean
            //
            return aux;
        }

        public int GetRoundNumber(int CompetitieId)
        {
            //
            // Get number of the last ound of a specific competition
            //
            int LastRound = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetLastRound", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompetitieId", SqlDbType.Int).Value = CompetitieId;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    LastRound = Convert.ToInt16(reader["Laatste_Ronde"]);
                }
            }
            //
            // return value is an integer
            //
            return LastRound;
        }

        public string GetStringFromAlgemeneInfo(int CID, string AttributeName)
        {
            //
            // Get value of an attribute of the general info record belonging to this competition
            //
            CID = System.Math.Max(1, CID);
            string aux = "";
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetGeneralInfoAttribute", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@AttributeName", SqlDbType.NVarChar).Value = AttributeName;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    aux = (string)(reader["AttributeValue"]);
                }
            }
            //
            // return value is a string
            //
            return aux;
        }


        public int GetNumberExternal(int CID, int RNR)
        {
            //
            // Get the number of players that are going to play agains an other club during this round
            // The information comes from the database table "nietIntern" which registers all players within the current competition that are not available
            // for a game in the internal competition
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetNumberExternal", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    aux = Convert.ToInt16(reader["Aantal"]);
                }
            }
            //
            // The result is an integer value
            //
            return aux;
        }

        public int GetNumberAbsentees(int CID, int RNR)
        {
            //
            // Get the number of players not available for the current competition
            // The information comes from the database table "nietIntern" which registers all players within the current competition that are not available
            // for a game in the internal competition
            // 
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetNumberAbsentees", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                // ?? cmd.ExecuteNonQuery();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    aux = Convert.ToInt16(reader["Aantal"]);
                }
            }
            //
            // The return value is in integer number
            //
            return aux;
        }

        public int GetNumberWithdrawn(int CID)
        {
            //
            // Get the number of players not available anymore for the current competition. They started out, playing in this competition but have 
            // withdrawn themselves.
            // The information comes from the database table "Deelnemers" which registers all players
            //
            int Aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetNumberWithdrawn", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                // ?? cmd.ExecuteNonQuery();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Aux = Convert.ToInt16(reader["Aantal"]);
                }
            }
            //
            // Return value is an integer number
            //
            return Aux;
        }

        public int GetNumberTotal(int CID)
        {
            //
            // Get the number of players within the current competition at the start of the competition 
            // withdrawn themselves.
            // The information comes from the database table "Deelnemers" which registers all players
            //
            int Aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetNumberTotal", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Aux = Convert.ToInt16(reader["Aantal"]);
                }
            }
            //
            // The return value is an integer number
            //
            return Aux;
        }


        public DataSet GetWorkFlowRecord(int CompetitionManagerId, int RoundNumber)
        {
            //
            // The progress of the round is covered by a workflow record per round for each competition. In this workflow record information about initialisation
            // absenteelists, calculating new round, getting results, corrections, prints and uploads are registered.
            // This function retrieves the information from a specific round of a specific competition
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetWorkflowRecord";
                cmd.Parameters.Add("@CompetitionId", SqlDbType.Int).Value = CompetitionManagerId;
                cmd.Parameters.Add("@RoundNumber", SqlDbType.Int).Value = RoundNumber;

                da.SelectCommand = cmd;
                da.Fill(ds);
                con.Close();
            }
            //
            // The return value is a dataset containing all information of this specific workflow item
            //
            return ds;
        }

        public DataSet GetPlayerList(int CompetitionId)
        {
            //
            // This function gets all names (combined into readable names) of the players of a specific competition plus some additional player data for use in
            // multiple situations
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetPlayerList";
                cmd.Parameters.Add("@CompetitionId", SqlDbType.Int).Value = CompetitionId;

                da.SelectCommand = cmd;
                da.Fill(ds);
                con.Close();
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public DataSet GetAbsenteeRegisterList(int CompetitionId, int RoundNumber)
        {
            //
            // This function results in a dataset with all previously registered absentees and externals from the NietIntern table of a specific round of a 
            // specific competition
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetAbsenteeRegisterList";
                cmd.Parameters.Add("@CompetitionId", SqlDbType.Int).Value = CompetitionId;
                cmd.Parameters.Add("@RoundNumber", SqlDbType.Int).Value = RoundNumber;

                da.SelectCommand = cmd;
                con.Open();
                da.Fill(ds);
                con.Close();
            }
            return ds;
        }

        public DataSet GetAbsenteeList(int CompetitionId, int RoundNumber)
        {
            //
            // This function results in a dataset with all previously registered absentees and externals from the NietIntern tablethrough a view that also 
            // contains a number od additional deelnemer-data such as team, vrijgeloot and naame attributes
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetAbsenteeList";
                cmd.Parameters.Add("@RoundNumber", SqlDbType.Int).Value = RoundNumber;
                cmd.Parameters.Add("@CompetitionId", SqlDbType.Int).Value = CompetitionId;

                da.SelectCommand = cmd;
                con.Open();
                da.Fill(ds);
                con.Close();
            }
            //
            // Return value a dataset
            //
            return ds;
        }

        public string GetPlayerName(int PID)
        {
            //
            // Get a readable name of a specific player
            //
            string aux;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmdFindName = new SqlCommand("spGetPlayerName", con);
                cmdFindName.CommandType = CommandType.StoredProcedure;
                cmdFindName.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                con.Open();

                SqlDataReader readPlayerName = cmdFindName.ExecuteReader();
                //
                // format of the name is: <<Achternaam>>, <<Voorletters>> <<voorvoegsel>> (<<Roepnaam>>)
                //
                readPlayerName.Read();
                string Achternaam = readPlayerName.GetString(0);
                aux = Achternaam.Trim() + ", ";
                string Voorletters = readPlayerName.GetString(2);
                aux += Voorletters.Trim() + " ";
                if (!readPlayerName.IsDBNull(1))
                {
                    string Tussenvoegsel = readPlayerName.GetString(1);
                    aux += Tussenvoegsel.Trim() + " ";
                }
                if (!readPlayerName.IsDBNull(3))
                {
                    string Roepnaam = readPlayerName.GetString(3);
                    aux += "(" + Roepnaam.Trim() + ")";
                }
                con.Close();
                con.Dispose();
            }
            //
            // Result is a string with the combined name
            //
            return aux;
        }

        public string GetPlayerFirstName(int PID)
        {
            //
            // Get a readable name of a specific player
            //
            string aux;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmdFindName = new SqlCommand("spGetPlayerFirstName", con);
                cmdFindName.CommandType = CommandType.StoredProcedure;
                cmdFindName.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                con.Open();

                SqlDataReader readPlayerName = cmdFindName.ExecuteReader();
                //
                // format of the name is: Roepnaam
                //
                readPlayerName.Read();
                aux = readPlayerName.GetString(0);
                con.Close();
                con.Dispose();
            }
            //
            // Result is a string with the combined name
            //
            return aux.Trim();
        }

        public float GetPlayerRatingInCompetition(int PID, int RNR, int CID)
        {
            //
            // This function returns the club-rating of a player after a specific round in a specific competition. It is NOT the last known rating
            //
            float aux;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetPlayerRatingInCompetition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                con.Open();

                SqlDataReader readRating = cmd.ExecuteReader();
                readRating.Read();
                if (!readRating.IsDBNull(0))
                {
                    aux = GetPlayerStartRating(PID, CID) + Convert.ToSingle(readRating.GetValue(0));
                }
                else
                {
                    aux = GetPlayerStartRating(PID, CID);
                }

                con.Close();
                con.Dispose();
            }
            //
            // the return value is a float
            //
            return aux;
        }

        public float GetPlayerPointsInCompetition(int PID, int RNR, int CID)
        {
            //
            // This function returns the Competitionpoints after a specific round in a specific competition
            //
            float aux;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetPlayerPointsInCompetition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                con.Open();

                SqlDataReader readRating = cmd.ExecuteReader();
                readRating.Read();
                if (!readRating.IsDBNull(0))
                {
                    aux = GetPlayerStartCompetitionPoints(PID, CID) + Convert.ToSingle(readRating.GetValue(0));
                }
                else
                {
                    aux = GetPlayerStartCompetitionPoints(PID, CID);
                }
                con.Close();
                con.Dispose();
            }
            //
            // Returnvalue is a float
            //
            return aux;
        }

        public bool AddAbsentee(AbsenteeData Absentee)
        {
            //
            // Add a record to the table "nietIntern"
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spSaveAbsenteeData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Speler_ID", SqlDbType.Int).Value = Absentee.Speler_ID;
                    cmd.Parameters.Add("@Rondenr", SqlDbType.Int).Value = Absentee.Rondenummer;
                    cmd.Parameters.Add("@Afwezigheidscode", SqlDbType.Int).Value = Absentee.Afwezigheidscode;
                    cmd.Parameters.Add("@Competitie_id", SqlDbType.Int).Value = Absentee.Competitie_ID;
                    cmd.Parameters.Add("@Partijnummer", SqlDbType.Int).Value = Absentee.Kroongroep_partijnummer;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", Absentee.Competitie_ID, "AddAbsentee", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value with error status. At the moment, the calling routine does not handle any exception status because 
            // there is no correct way to continue when the exception occurs.
            //
            return error_occurred;
        }

        public bool DeleteAbsentees(int CompetitieID, int RondeNummer)
        {
            //
            // Clean up table "nietIntern" before new cycle starts
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spDeleteAbsentees", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompetitieID", SqlDbType.Int).Value = CompetitieID;
                    cmd.Parameters.Add("@RondeNummer", SqlDbType.Int).Value = RondeNummer;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CompetitieID, "DeleteAbsentees", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // Return value with error status. At the moment, the calling routine does not handle any exception status because 
                // there is no correct way to continue when the exception occurs.
                //
                return error_occurred;
            }
        }

        public DataSet GetKNSBPlayerList(string Playername)
        {
            //
            // This function returns a list of players from the KNSB ratinglist to choose from, including KNSB rating, when entering a new member into the competition
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetKNSBRatinghouders";
                    cmd.Parameters.Add("@Spelernaam", SqlDbType.Text).Value = Playername;
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetKNSBPlayerList", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public DataSet GetFIDEPlayerList(string Playername)
        {
            //
            // This function returns a list of players from the FIDE ratinglist to choose from, including FIDE rating, when entering a new member into the competition
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetFIDERatinghouders";
                    cmd.Parameters.Add("@Spelernaam", SqlDbType.Text).Value = Playername;
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetFIDEPlayerList", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }


        public int GetNoPlayCode(int PID, int CID, int RNR)
        {
            //
            // This function returns the reason why a player is absent from the normal competition
            //
            int NoPlayCode = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetNoPlayCode", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PlayerId", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@CompetitieId", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RoundNumber", SqlDbType.Int).Value = RNR;

                    SqlDataReader readerNoPlayCode = cmd.ExecuteReader();
                    while (readerNoPlayCode.Read())
                    {
                        NoPlayCode = Convert.ToInt16(readerNoPlayCode["Afwezigheidscode"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetNoPlayCode", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                return NoPlayCode;
            }
        }

        public int GetKroongroepPartijNummer(int PID, int CID, int RNR)
        {
            //
            // This function returns the the game number of a Championsgroup game
            //
            int Gamenumber = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetKroongroepPartijnummer", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PlayerId", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@CompetitieId", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RoundNumber", SqlDbType.Int).Value = RNR;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Gamenumber = Convert.ToInt16(reader["Kroongroep_Partijnummer"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetKroongroepPartijnummer", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                return Gamenumber;
            }
        }

        public float GetPlayerStartRating(int PID, int CID)
        {
            float StartRating = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetPlayerStartRating", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    SqlDataReader readerValue = cmd.ExecuteReader();
                    while (readerValue.Read())
                    {
                        StartRating = Convert.ToSingle(readerValue["StartRating"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetPlayerStartRating", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                return StartRating;
            }
        }

        public float GetPlayerStartCompetitionPoints(int PID, int CID)
        {
            float StartCompetitionPoints = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetPlayerStartCompetitionPoints", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    SqlDataReader readerValue = cmd.ExecuteReader();
                    while (readerValue.Read())
                    {
                        StartCompetitionPoints = Convert.ToSingle(readerValue["StartCompetitionPoints"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetPlayerStartCompetitionPoints", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                return StartCompetitionPoints;
            }
        }

        public PlayerBasicData GetPlayerFullData(int Player, int CID)
        {
            //
            // This function returns an instance of a class of all data related to a single player. Sources are multiple database tables
            //
            PlayerBasicData pl = new PlayerBasicData();
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    //
                    // Get data from Deelnemer table, some of the rating data will be overridden by new data from rating history table
                    // In a future update this information need to be removed from the table
                    //
                    SqlCommand cmd = new SqlCommand("spGetPlayerFullData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PlayerId", SqlDbType.Int).Value = Player;
                    SqlDataReader readerNoPlayCode = cmd.ExecuteReader();

                    while (readerNoPlayCode.Read())
                    {
                        pl.Speler_ID = Convert.ToInt32(readerNoPlayCode["Speler_ID"]);
                        pl.Competitie_Id = CID;
                        pl.Titel = Convert.ToString(readerNoPlayCode["Titel"]);
                        pl.Achternaam = Convert.ToString(readerNoPlayCode["Achternaam"]);
                        pl.Tussenvoegsel = Convert.ToString(readerNoPlayCode["Tussenvoegsel"]);
                        pl.Voorletters = Convert.ToString(readerNoPlayCode["Voorletters"]);
                        pl.Roepnaam = Convert.ToString(readerNoPlayCode["Roepnaam"]);
                        pl.KNSBnummer = Convert.ToInt32(readerNoPlayCode["KNSBnummer"]);
                        pl.FIDEnummer = Convert.ToInt32(readerNoPlayCode["FIDEnummer"]);
                        pl.Telefoonnummer = Convert.ToString(readerNoPlayCode["Telefoonnummer"]);
                        pl.Team = Convert.ToSByte(readerNoPlayCode["Team"]);
                        pl.Clublid = Convert.ToSByte(readerNoPlayCode["Clublid"]);
                        pl.Deelnemer_teruggetrokken = Convert.ToSByte(readerNoPlayCode["Deelnemer_teruggetrokken"]);
                        pl.Speelt_mee_sinds_ronde = Convert.ToSByte(readerNoPlayCode["Speelt_mee_sinds_ronde"]);
                        pl.Doet_mee_met_snelschaak = Convert.ToSByte(readerNoPlayCode["Doet_mee_met_snelschaak"]);
                        pl.Speelt_blitz_sinds_ronde = Convert.ToSByte(readerNoPlayCode["Speelt_blitz_sinds_ronde"]);
                        pl.Vrijgeloot = Convert.ToSByte(readerNoPlayCode["Vrijgeloot"]);
                        pl.Wants_Email = Convert.ToSByte(readerNoPlayCode["Wants_Email"]);
                        pl.Email_Address = Convert.ToString(readerNoPlayCode["Email_Address"]);
                        pl.Wants_SMS = Convert.ToSByte(readerNoPlayCode["Wants_SMS"]);
                        pl.Mobile_Number = Convert.ToString(readerNoPlayCode["Mobile_Number"]);
                        pl.Member_Premier_Group = Convert.ToSByte(readerNoPlayCode["Member_Premier_Group"]);
                        if (DBNull.Value != readerNoPlayCode["ProfilePicture"])
                        {
                            pl.ProfilePicture = (byte[])readerNoPlayCode["ProfilePicture"];
                        }
                    }
                    readerNoPlayCode.Close();
                    //
                    // Get specific rating info for this player from rating history table
                    //
                    SqlCommand cmd2 = new SqlCommand("spGetRatingSpecifics", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.Add("@PlayerId", SqlDbType.Int).Value = Player;
                    cmd2.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(cmd2);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    DataTable Club = ds.Tables[0];
                    if (Club.Rows.Count > 0)
                    {
                        DataRow ClubRow = Club.Rows[0];
                        if (DBNull.Value != ClubRow["RatingPoints"])
                        {
                            pl.Rating = System.Convert.ToSingle(ClubRow["RatingPoints"]);
                        }
                        else
                        {
                            pl.Rating = (float)0.0;
                        }
                    }
                    else
                    {
                        pl.Rating = (float)0.0;
                    }

                    DataTable Blitz2 = ds.Tables[2];
                    if (Blitz2.Rows.Count > 0)
                    {
                        DataRow BlitzRow = Blitz2.Rows[0];
                        if (DBNull.Value != BlitzRow["RatingPoints"])
                        {
                            pl.Snelschaakrating = System.Convert.ToSingle(BlitzRow["RatingPoints"]);
                        }
                        else
                        {
                            pl.Snelschaakrating = (float)0.0;
                        }
                    }
                    else
                    {
                        pl.Snelschaakrating = (float)0.0;
                    }

                    DataTable Rapid = ds.Tables[3];
                    if (Rapid.Rows.Count > 0)
                    {
                        DataRow RapidRow = Rapid.Rows[0];
                        if (DBNull.Value != RapidRow["RatingPoints"])
                        {
                            pl.Rapidrating = System.Convert.ToSingle(RapidRow["RatingPoints"]);
                        }
                        else
                        {
                            pl.Rapidrating = (float)0.0;
                        }
                    }
                    else
                    {
                        pl.Rapidrating = (float)0.0;
                    }

                    DataTable StartCompetition = ds.Tables[1];
                    if (StartCompetition.Rows.Count > 0)
                    {
                        DataRow StartCompetitionRow = StartCompetition.Rows[0];
                        if (StartCompetitionRow["StartRating"] != System.DBNull.Value)
                        {
                            pl.Startrating = System.Convert.ToSingle(StartCompetitionRow["StartRating"]);
                        }
                        else
                        {
                            pl.Startrating = (float)0.0;
                        }
                    }
                    else
                    {
                        pl.Startrating = (float)0.0;
                    }

                    DataTable FIDERating = ds.Tables[4];
                    if (FIDERating.Rows.Count > 0)
                    {
                        DataRow FIDERow = FIDERating.Rows[0];
                        if (DBNull.Value != FIDERow["RatingPoints"])
                        {
                            pl.FIDErating = System.Convert.ToSingle(FIDERow["RatingPoints"]);
                        }
                        else
                        {
                            pl.FIDErating = (float)0.0;
                        }
                    }
                    else
                    {
                        pl.FIDErating = (float)0.0;
                    }

                    DataTable KNSBRating = ds.Tables[5];
                    if (KNSBRating.Rows.Count > 0)
                    {
                        DataRow KNSBRow = KNSBRating.Rows[0];
                        if (DBNull.Value != KNSBRow["RatingPoints"])
                        {
                            pl.KNSBrating = System.Convert.ToSingle(KNSBRow["RatingPoints"]);
                        }
                        else
                        {
                            pl.KNSBrating = (float)0.0;
                        }
                    }
                    else
                    {
                        pl.KNSBrating = (float)0.0;
                    }

                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetPlayerFullData", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
            //
            // This function returns an instance of a class of  player data
            //
            return pl;
        }

        public bool UpdatePlayer(PlayerBasicData Player, int CID)
        {
            //
            // This function executes an update of player data in the different database tables
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spUpdatePlayerData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Player_Id", SqlDbType.Int).Value = Player.Speler_ID;
                    cmd.Parameters.Add("@Competitie_Id", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@Titel", SqlDbType.Text).Value = Player.Titel;
                    cmd.Parameters.Add("@Achternaam", SqlDbType.Text).Value = Player.Achternaam;
                    cmd.Parameters.Add("@Tussenvoegsel", SqlDbType.Text).Value = Player.Tussenvoegsel;
                    cmd.Parameters.Add("@Voorletters", SqlDbType.Text).Value = Player.Voorletters;
                    cmd.Parameters.Add("@Roepnaam", SqlDbType.Text).Value = Player.Roepnaam;
                    cmd.Parameters.Add("@KNSBnummer", SqlDbType.Int).Value = Player.KNSBnummer;
                    cmd.Parameters.Add("@FIDEnummer", SqlDbType.Int).Value = Player.FIDEnummer;
                    cmd.Parameters.Add("@Telefoonnummer", SqlDbType.Text).Value = Player.Telefoonnummer;
                    cmd.Parameters.Add("@Team", SqlDbType.SmallInt).Value = Player.Team;
                    cmd.Parameters.Add("@Clublid", SqlDbType.Bit).Value = Player.Clublid;
                    cmd.Parameters.Add("@Deelnemer_teruggetrokken", SqlDbType.Bit).Value = Player.Deelnemer_teruggetrokken;
                    cmd.Parameters.Add("@Speelt_mee_sinds_ronde", SqlDbType.SmallInt).Value = Player.Speelt_mee_sinds_ronde;
                    cmd.Parameters.Add("@Doet_mee_met_snelschaak", SqlDbType.Bit).Value = Player.Doet_mee_met_snelschaak;
                    cmd.Parameters.Add("@Speelt_blitz_sinds_ronde", SqlDbType.SmallInt).Value = Player.Speelt_blitz_sinds_ronde;
                    cmd.Parameters.Add("@Vrijgeloot", SqlDbType.Bit).Value = Player.Vrijgeloot;
                    cmd.Parameters.Add("@Wants_Email", SqlDbType.Bit).Value = Player.Wants_Email;
                    cmd.Parameters.Add("@Email_Address", SqlDbType.Text).Value = Player.Email_Address;
                    cmd.Parameters.Add("@Wants_SMS", SqlDbType.Bit).Value = Player.Wants_SMS;
                    cmd.Parameters.Add("@Mobile_Number", SqlDbType.Text).Value = Player.Mobile_Number;
                    cmd.Parameters.Add("@Member_Premier_Group", SqlDbType.Bit).Value = Player.Member_Premier_Group;
                    cmd.Parameters.Add("@ProfilePicture", SqlDbType.Image).Value = Player.ProfilePicture;

                    cmd.Parameters.Add("@Rating", SqlDbType.Float).Value = Player.Rating;
                    cmd.Parameters.Add("@KNSBrating", SqlDbType.Float).Value = Player.KNSBrating;
                    cmd.Parameters.Add("@FIDErating", SqlDbType.Float).Value = Player.FIDErating;
                    cmd.Parameters.Add("@Snelschaakrating", SqlDbType.Float).Value = Player.Snelschaakrating;
                    cmd.Parameters.Add("@Rapidrating", SqlDbType.Float).Value = Player.Rapidrating;
                    cmd.Parameters.Add("@Startrating", SqlDbType.Float).Value = Player.Startrating;
                    cmd.Parameters.Add("@StartCompetitiepunten", SqlDbType.Float).Value = Player.Startrating;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "UpdatePlayer", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
            //
            return error_occurred;
        }

        public bool AddPlayer(PlayerBasicData Player)
        {
            //
            // This function saves the data belonging to a new player into the "Deelnemer" table, it will not create any new entrys into the rating history table
            // because there is no history yet
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spSavePlayerData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Competitie_Id", SqlDbType.Int).Value = Player.Competitie_Id;
                    cmd.Parameters.Add("@Titel", SqlDbType.Text).Value = Player.Titel;
                    cmd.Parameters.Add("@Achternaam", SqlDbType.Text).Value = Player.Achternaam;
                    cmd.Parameters.Add("@Tussenvoegsel", SqlDbType.Text).Value = Player.Tussenvoegsel;
                    cmd.Parameters.Add("@Voorletters", SqlDbType.Text).Value = Player.Voorletters;
                    cmd.Parameters.Add("@Roepnaam", SqlDbType.Text).Value = Player.Roepnaam;
                    cmd.Parameters.Add("@KNSBnummer", SqlDbType.Int).Value = Player.KNSBnummer;
                    cmd.Parameters.Add("@FIDEnummer", SqlDbType.Int).Value = Player.FIDEnummer;
                    cmd.Parameters.Add("@Telefoonnummer", SqlDbType.Text).Value = Player.Telefoonnummer;
                    cmd.Parameters.Add("@Team", SqlDbType.SmallInt).Value = Player.Team;
                    cmd.Parameters.Add("@Clublid", SqlDbType.Bit).Value = Player.Clublid;
                    cmd.Parameters.Add("@Deelnemer_teruggetrokken", SqlDbType.Bit).Value = Player.Deelnemer_teruggetrokken;
                    cmd.Parameters.Add("@Speelt_mee_sinds_ronde", SqlDbType.SmallInt).Value = Player.Speelt_mee_sinds_ronde;
                    cmd.Parameters.Add("@Doet_mee_met_snelschaak", SqlDbType.Bit).Value = Player.Doet_mee_met_snelschaak;
                    cmd.Parameters.Add("@Speelt_blitz_sinds_ronde", SqlDbType.SmallInt).Value = Player.Speelt_blitz_sinds_ronde;
                    cmd.Parameters.Add("@Vrijgeloot", SqlDbType.Bit).Value = Player.Vrijgeloot;
                    cmd.Parameters.Add("@Wants_Email", SqlDbType.Bit).Value = Player.Wants_Email;
                    cmd.Parameters.Add("@Email_Address", SqlDbType.Text).Value = Player.Email_Address;
                    cmd.Parameters.Add("@Wants_SMS", SqlDbType.Bit).Value = Player.Wants_SMS;
                    cmd.Parameters.Add("@Mobile_Number", SqlDbType.Text).Value = Player.Mobile_Number;
                    cmd.Parameters.Add("@Member_Premier_Group", SqlDbType.Bit).Value = Player.Member_Premier_Group;
                    cmd.Parameters.Add("@ProfilePicture", SqlDbType.Image).Value = Player.ProfilePicture;

                    cmd.Parameters.Add("@Rating", SqlDbType.Float).Value = Player.Rating;
                    cmd.Parameters.Add("@KNSBrating", SqlDbType.Float).Value = Player.KNSBrating;
                    cmd.Parameters.Add("@FIDErating", SqlDbType.Float).Value = Player.FIDErating;
                    cmd.Parameters.Add("@Snelschaakrating", SqlDbType.Float).Value = Player.Snelschaakrating;
                    cmd.Parameters.Add("@Rapidrating", SqlDbType.Float).Value = Player.Rapidrating;
                    cmd.Parameters.Add("@Startrating", SqlDbType.Float).Value = Player.Startrating;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", Player.Competitie_Id, "AddPlayer", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
            //
            return error_occurred;
        }

        public bool Update_Workflow_Item(string Item_Name, int CID, int RNR, int Item_Value)
        {
            //
            // This function updates any value in a workflow record  in the Workflow Table
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spUpdateWorkflow", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompetitieID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@Rondenummer", SqlDbType.Int).Value = RNR;
                    cmd.Parameters.Add("@ItemNaam", SqlDbType.NVarChar).Value = Item_Name;
                    cmd.Parameters.Add("@ItemWaarde", SqlDbType.Int).Value = Item_Value;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "Update_Workflow_Item", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        // start module for TSGS_CS_Initialize_Next_Round

        public bool Initialize_New_Round(int CID, int RNR)
        {
            //
            // This function adds a new record to the workflow table
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spResetWorkflow", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                    //
                    // after starting a new round, remove the old ranking list data
                    //
                    RemovePlayerStatusCalculated(CID, RNR);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "Initialise_New_Round", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }
        /*
         * Start module for calculating new pairing
         */
        public bool Remove_Templist(int CID, int RNR)
        {
            //
            // This function adds a new record to the workflow table
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spRemoveTemplist", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "Remove_Templist", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        public void Create_Worklist(int CID, int RNR)
        {
            //
            // This module creates a list of entries into the temp_table to facilitate printing lists and generating a new round
            //

            //
            // Remove old templist
            // 
            Remove_Templist(CID, RNR);
            //
            // Get all players from current competition
            //
            PairingWorklist wl = new PairingWorklist();
            DataSet ds = new DataSet();
            ds = GetPlayerList(CID);
            DataTable dt = ds.Tables[0];
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DataRow Single_Player = dt.Rows[j];
                wl.Speler_Id = System.Convert.ToInt32(Single_Player["Speler_ID"]);
                wl.Round_Free = 0;
                //
                // assemble all relevant data for print lists and new pairing
                //
                Calculate_Player_Data(wl, CID, RNR, Single_Player);
                //
                // Add all information to the database table temp_list
                //
                AddWorklistRecord(wl);
            }
        }

        private bool AddWorklistRecord(PairingWorklist wl)
        {
            //
            // This function adds a record to the temp_list table
            //
            // first compile the data of the free rounds
            //
            int GameCount = CountGamesInCompetitions(wl.Speler_Id);
            int GameCountInCompetition = CountGamesInOneCompetition(wl.Speler_Id, wl.Competition_Id, wl.Round_Number);
            if (GameCount < 20 || wl.Current_Rating > 3000 || GameCountInCompetition == 0)
            {
                wl.Date_Last_Free_Round = System.DateTime.Now;
            }
            else
            {
                wl.Date_Last_Free_Round = GetDateLastFreeRound(wl.Speler_Id);
            }
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spSaveWorkListData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Speler_Id", SqlDbType.Int).Value = wl.Speler_Id;
                    cmd.Parameters.Add("@Competition_points", SqlDbType.Float).Value = wl.Competition_points;
                    cmd.Parameters.Add("@Start_Competition_points", SqlDbType.Float).Value = wl.Start_Competition_Points;
                    cmd.Parameters.Add("@Color_Balance", SqlDbType.Int).Value = wl.Color_Balance;
                    cmd.Parameters.Add("@Round_Free", SqlDbType.Int).Value = wl.Round_Free;
                    cmd.Parameters.Add("@Mandatory_White", SqlDbType.Int).Value = wl.Mandatory_White;
                    cmd.Parameters.Add("@Mandatory_Black", SqlDbType.Int).Value = wl.Mandatory_Black;
                    cmd.Parameters.Add("@Percentage", SqlDbType.Float).Value = wl.Percentage;
                    cmd.Parameters.Add("@Gain_Loss", SqlDbType.Float).Value = wl.Gain_Loss;
                    cmd.Parameters.Add("@Competition_Id", SqlDbType.Int).Value = wl.Competition_Id;
                    cmd.Parameters.Add("@Round_Number", SqlDbType.Int).Value = wl.Round_Number;
                    cmd.Parameters.Add("@Start_Rating", SqlDbType.Float).Value = wl.Start_Rating;
                    cmd.Parameters.Add("@Current_Rating", SqlDbType.Float).Value = wl.Current_Rating;
                    cmd.Parameters.Add("@Rating_Gain", SqlDbType.Float).Value = wl.Rating_Gain;
                    cmd.Parameters.Add("@Aantal_Afmelding", SqlDbType.Int).Value = wl.Aantal_Afmeldingen;
                    cmd.Parameters.Add("@Aantal_Punten", SqlDbType.Float).Value = wl.Aantal_Punten;
                    cmd.Parameters.Add("@Aantal_Partijen", SqlDbType.Int).Value = wl.Aantal_Partijen;
                    cmd.Parameters.Add("@Date_Last_Free_Round", SqlDbType.DateTime).Value = wl.Date_Last_Free_Round;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", wl.Competition_Id, "AddWorklistRecord", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
            //
            return error_occurred;
        }

        private void GetResultsData_Simple(int PID, int CID, int RNR, ref int Color_Balance, ref int Color_Balance_Last_Two, ref float ELO_Result, ref float Competition_Result, ref int Absent, ref int Games, ref float Points)
        {
            //
            // this function calculates the colorbalance, the mandatory color assignments, the elo and competition gains and the score parameters
            //
            int Last_Color = 0;

            GetColorBalanceData(PID, CID, RNR, ref Color_Balance, ref Color_Balance_Last_Two, ref Last_Color);

            //
            // continue with rating and games points
            //
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetScores", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                con.Open();

                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                DataTable Ratings = ds.Tables[0];
                DataTable Scores = ds.Tables[1];

                if (Ratings.Rows.Count > 0)
                {
                    DataRow raRow = Ratings.Rows[0];
                    if (raRow["ELOResultaat"] != DBNull.Value)
                    {

                        ELO_Result = Convert.ToSingle(raRow["ELOResultaat"]);
                    }
                    else
                    {
                        ELO_Result = (float)0.0;
                    }
                    if (raRow["CompetitieResultaat"] != DBNull.Value)
                    {

                        Competition_Result = Convert.ToSingle(raRow["CompetitieResultaat"]);
                    }
                    else
                    {
                        Competition_Result = (float)0.0;
                    }
                }
                else
                {
                    ELO_Result = (float)0.0;
                    Competition_Result = (float)0.0;
                }
                //
                // Calculate scores and percentages
                //
                Absent = 0;
                Games = 0;
                int Other_Games = 0;
                Points = (float)0.0;
                int aux = 0;
                //
                if (Scores.Rows.Count > 0)
                {
                    for (int j = 0; j < Scores.Rows.Count; j++)
                    {
                        DataRow Single_Score = Scores.Rows[j];
                        aux = Convert.ToInt16(Single_Score["Resultaat"]);
                        switch (aux)
                        {
                            case 1:
                                Games++;
                                Points += (float)1.0;
                                break;
                            case 2:
                                Games++;
                                Points += (float)0.5;
                                break;
                            case 3:
                                Games++;
                                break;
                            case 9:
                                Games++;
                                Points += (float)1.0;
                                break;
                            case 10:
                                Games++;
                                Points += (float)0.5;
                                break;
                            case 11:
                                Games++;
                                break;
                            case 12:
                                Absent++;
                                break;
                            case 13:
                                Games++;
                                Points += (float)1.0;
                                break;
                            default:
                                Other_Games++;
                                break;
                        }

                    }
                }
            }
        }

        private void Calculate_Player_Data(PairingWorklist wl, int CID, int RNR, DataRow Single_Player)
        {
            //
            // This module prepares several information items for the Worklist record to be added to the temp_list table
            //
            int Color_Balance = 0;
            int Color_Balance_Last_Two = 0;
            float ELO_Result = 0;
            float CompetitionResult = 0;
            int Absent = 0;
            int Games = 0;
            float Points = (float)0.0;
            wl.Start_Rating = GetPlayerStartRating(wl.Speler_Id, CID);
            wl.Competition_Id = CID;
            wl.Start_Competition_Points = GetPlayerStartCompetitionPoints(wl.Speler_Id, CID);
            wl.Round_Number = RNR;
            GetResultsData_Simple(wl.Speler_Id, CID, RNR, ref Color_Balance, ref Color_Balance_Last_Two, ref ELO_Result, ref CompetitionResult, ref Absent, ref Games, ref Points);
            wl.Mandatory_White = Convert.ToByte((Color_Balance <= -2) || (Color_Balance_Last_Two <= -2));
            wl.Mandatory_Black = Convert.ToByte((Color_Balance >= 2) || (Color_Balance_Last_Two >= 2));
            wl.Color_Balance = Color_Balance;
            wl.Current_Rating = wl.Start_Rating + ELO_Result;
            wl.Competition_points = wl.Start_Competition_Points + CompetitionResult;
            wl.Gain_Loss = CompetitionResult;
            wl.Rating_Gain = ELO_Result;
            wl.Aantal_Afmeldingen = Absent;
            wl.Aantal_Partijen = Games;
            wl.Aantal_Punten = Points;
            if (Games > 0)
            {
                wl.Percentage = ((float)Points * (float)100.0) / (float)Games;
            }
            else
            {
                wl.Percentage = (float)0.0;
            }
        }

        private bool Cleanup_Before_Pairing(int CID, int RNR, int GameType)
        {
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spPreparationsNewPairing", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    cmd.Parameters.Add("@GT", SqlDbType.Int).Value = GameType;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "Cleanup_Before_Pairing", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        public int Count_Player_Absent(int PID, int CID, int RNR)
        {
            // Get the number of times a player has been absent within the current competition until now.
            // The information comes from the database table "Competitie resultaten"
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetTimesAbsent", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = 0;
                        if (!reader.IsDBNull(0))
                        {
                            aux = Convert.ToInt16(reader["Aantal"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "Count_Player_Absent", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is an integer number
            //

            return aux;
        }

        public int CountUnhandledCriticalAlarms()
        {
            //
            // Get the number of times a player has been absent within the current competition until now.
            // The information comes from the database table "Competitie resultaten"
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spCountUnhandledCriticalAlarms", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = 0;
                        if (!reader.IsDBNull(0))
                        {
                            aux = Convert.ToInt16(reader["CriticalAlarms"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "CountUnhandledCriticalAlarms", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is an integer number
            //

            return aux;
        }


        private int Get_Free_Absent(int CID)
        {
            //
            // Get the number of times a player may be absent without penalty points
            // The information comes from the database table "Algemene info" 
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetFreeAbsent", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    // ?? cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Aantal"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetFreeAbsent", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
            //
            // The return value is an integer number
            //

            return aux;
        }

        private int Get_Penalty_Absent(int CID)
        {
            //
            // Get the number of times a player may be absent without penalty points
            // The information comes from the database table "Algemene info" 
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {

                    SqlCommand cmd = new SqlCommand("spGetPenaltyAbsent", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    // ?? cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Aantal"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetPenaltyAbsent", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is an integer number
            //

            return aux;
        }

        private int Get_Reward_External(int CID)
        {
            //
            // Get the reward a player receives after playing for his team
            // The information comes from the database table "Algemene info" 
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand("spGetRewardExternal", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    // ?? cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Aantal"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "Get_Reward_External", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is an integer number
            //
            return aux;
        }

        private bool RemoveLastGame (int CID, int RNR)
        {
            //
            // This module removes one game from the table "Wedstrijden" and also remove paired data information (is only called in Swiss module, to restart searching in a 
            // transposed bracket
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spRemoveLastGameFromSwissLists", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    int Ret_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "RemoveLastGame", "Error", 2, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //

            return error_occurred;
        }

        public bool AddGame(GamesData OneGame)
        {
            //
            // This module adds one game to the table "Wedstrijden"
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spSaveOneGame", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Competitie_Id", SqlDbType.Int).Value = OneGame.Competitie_Id;
                    cmd.Parameters.Add("@Id_Witspeler", SqlDbType.Int).Value = OneGame.Id_Witspeler;
                    cmd.Parameters.Add("@Id_Zwartspeler", SqlDbType.Int).Value = OneGame.Id_Zwartspeler;
                    cmd.Parameters.Add("@Rondenr", SqlDbType.Int).Value = OneGame.Rondernr;
                    cmd.Parameters.Add("@Sorteerwaarde", SqlDbType.Int).Value = OneGame.Sorteerwaarde;
                    cmd.Parameters.Add("@Wedstrijdresultaat", SqlDbType.Int).Value = OneGame.Wedstrijdresultaat;
                    cmd.Parameters.Add("@Wedstrijdtype", SqlDbType.Int).Value = OneGame.Wedstrijdtype;
                    cmd.Parameters.Add("@Wit_Remise", SqlDbType.Float).Value = OneGame.Wit_Remise;
                    cmd.Parameters.Add("@Wit_Verlies", SqlDbType.Float).Value = OneGame.Wit_Verlies;
                    cmd.Parameters.Add("@Wit_Winst", SqlDbType.Float).Value = OneGame.Wit_Winst;
                    cmd.Parameters.Add("@Zwart_Remise", SqlDbType.Float).Value = OneGame.Zwart_Remise;
                    cmd.Parameters.Add("@Zwart_Verlies", SqlDbType.Float).Value = OneGame.Zwart_Verlies;
                    cmd.Parameters.Add("@Zwart_Winst", SqlDbType.Float).Value = OneGame.Zwart_Winst;
                    cmd.Parameters.Add("@NumberChampionsgroupGame", SqlDbType.Int).Value = OneGame.NumberChampionsgroupGame;
                    cmd.Parameters.Add("@Matchpoints_White", SqlDbType.Float).Value = OneGame.Matchpoints_White;
                    cmd.Parameters.Add("@Matchpoints_Black", SqlDbType.Float).Value = OneGame.Matchpoints_Black;
                    cmd.Parameters.Add("@Pairing_Status_Level", SqlDbType.Int).Value = OneGame.Pairing_Status_Level;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", OneGame.Competitie_Id, "Add_Game", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }

        public DataSet GetChampionsgroupList(int CID, int RNR)
        {
            //
            // This function returns a list of players from the Champions competition
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetChampionsGroupWhiteList";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "Get_ChampionsGroupList", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        private int GetChampionsgroupAdversary(int CID, int RNR, int Game_Number)
        {
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand("spGetChampionsgroupBlack", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    cmd.Parameters.Add("@Game_Number", SqlDbType.Int).Value = 0 - Game_Number;

                    // ?? cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Speler_ID"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "Get_ChampionsGroupList", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // return value is an integer
            //
            return aux;
        }

        public float GetKFactor(int CID)
        {
            //
            // This function gets the K_Factor from the tabel Algemene_Info
            //
            float aux = (float)0.0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand("spGetKFactor", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToSingle(reader["KFactor"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetKFactor", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is a floating number
            //
            return aux;
        }

        public float GetAcceleration(int CID)
        {
            //
            // This function gets the K_Factor from the tabel Algemene_Info
            //
            float aux = (float)0.0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand("spGetAcceleration", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToSingle(reader["Acceleration"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetAccelerator", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is a floating number
            //
            return aux;
        }

        private float Calculate_Competitionpoints_One_Game(float Elo_Wit, float Elo_Zwart, float K_Factor, int Result, float Acceleration)
        {
            //
            // This function returns the value of the competition point gain for the whiteplayer
            //
            float aux = (float)0.0;
            //
            // Kansberekenings benadering gebruikt door KNSB:
            //
            float TEN = (float)10.0;
            float Fourhundred = (float)400.0;
            float One = (float)1.0;

            float P_normal = One / (float)(System.Math.Pow(TEN, ((Elo_Wit - Elo_Zwart) / Fourhundred)) + One);
            P_normal = System.Math.Min((float)0.6666, System.Math.Max((float)0.3333, P_normal));
            switch (Result)
            {
                case 1:
                    {
                        aux = K_Factor * P_normal * Acceleration;
                        break;
                    }
                case 2:
                    {
                        aux = K_Factor * (P_normal - (float)0.5) * Acceleration;
                        break;
                    }
                case 3:
                    {
                        aux = K_Factor * (P_normal - (float)1.0) * Acceleration;
                        break;
                    }
            }

            return aux;
        }


        private float Calculate_Rating_One_Game(float Elo_Wit, float Elo_Zwart, float K_Factor, int Result)
        {
            //
            // This function returns the value of the gain for the whiteplayer
            //
            float aux = (float)0.0;
            float K_Factor_Calc = (float)0.0;
            //
            // K factor formule: K_Factor * (1.0-Max(0, min(0.834,(gem-rating-2000)/(500/0.834))))
            // Bij K_factor van 30 levert dit een K_Factor van 30 op beneden gem rating van 2000,
            // een aflopende K_Factor to 5 bij gemiddelde rating van 2500 of hoger
            // 
            float Avg_Rating = (Elo_Wit + Elo_Zwart) / (float)2.0;
            if (Avg_Rating < 2000)
            {
                K_Factor_Calc = K_Factor;
            }
            else
            {
                K_Factor_Calc = K_Factor * ((float)1.0 - System.Math.Max(0, System.Math.Min((float)0.833333, (Avg_Rating - (float)2000.0) / ((float)500.0 / (float)0.833333))));
            }
            //
            // Kansberekenings benadering gebruikt door KNSB:
            //
            float TEN = (float)10.0;
            float Fourhundred = (float)400.0;
            float One = (float)1.0;

            float P_normal = One / (float)(System.Math.Pow(TEN, ((Elo_Wit - Elo_Zwart) / Fourhundred)) + One);
            switch (Result)
            {
                case 1:
                    {
                        aux = K_Factor_Calc * P_normal;
                        break;
                    }
                case 2:
                    {
                        aux = K_Factor_Calc * (P_normal - (float)0.5);
                        break;
                    }
                case 3:
                    {
                        aux = K_Factor_Calc * (P_normal - (float)1.0);
                        break;
                    }
            }

            return aux;
        }

        private void SetUp_ChampionsGroup_Games(int CID, int RNR)
        {
            //
            // This function creates games in the tabel "Wedstrijden"connected to the championsgroup games
            //
            float RatingWhite = (float)0.0;
            float RatingBlack = (float)0.0;

            int Game_Number = 0;
            //
            // Get championsgroup games white players and gamenumbers  from "NietIntern"
            //
            DataSet ds = new DataSet();
            ds = GetChampionsgroupList(CID, RNR);
            if (ds != null)
            {
                float KFactor = GetKFactor(CID);
                float Acceleration = GetAcceleration(CID);

                GamesData game = new GamesData();
                DataTable tbl = ds.Tables[0];
                for (int j = 0; j < tbl.Rows.Count; j++)
                {
                    // Get white championship group member and find the corresponding black player
                    DataRow myRow = tbl.Rows[j];
                    game.Id_Witspeler = (int)myRow["Speler_ID"];
                    Game_Number = (int)myRow["Kroongroep_Partijnummer"];
                    game.Id_Zwartspeler = GetChampionsgroupAdversary(CID, RNR, Game_Number);
                    //
                    // fill in all relevant information
                    //
                    game.Competitie_Id = CID;
                    game.Rondernr = RNR;
                    game.Wedstrijdresultaat = 0;
                    game.Wedstrijdtype = 8;
                    RatingWhite = GetPlayerRatingInCompetition(game.Id_Witspeler, RNR, CID);
                    RatingBlack = GetPlayerRatingInCompetition(game.Id_Zwartspeler, RNR, CID);
                    game.Sorteerwaarde = System.Convert.ToInt16(System.Math.Max(RatingWhite, RatingBlack)) + 3000;
                    game.Wit_Winst = Calculate_Competitionpoints_One_Game(RatingWhite, RatingBlack, KFactor, 1, Acceleration);
                    game.Wit_Remise = Calculate_Competitionpoints_One_Game(RatingWhite, RatingBlack, KFactor, 2, Acceleration);
                    game.Wit_Verlies = Calculate_Competitionpoints_One_Game(RatingWhite, RatingBlack, KFactor, 3, Acceleration);
                    game.Zwart_Winst = 0 - game.Wit_Verlies;
                    game.Zwart_Remise = 0 - game.Wit_Remise;
                    game.Zwart_Verlies = 0 - game.Wit_Winst;
                    game.NumberChampionsgroupGame = Game_Number;
                    game.Matchpoints_White = 0;
                    game.Matchpoints_Black = 0;
                    game.Pairing_Status_Level = 0;
                    AddGame(game);
                }
            }
        }

        private void SetUp_Non_Competition_Games(int CID, int RNR)
        {
            //
            // This function creates "games" for all players that are not playing in the regular clubcompetition (external, absent and Championsgroup
            //
            int Afwezigheidscode = 0;
            float Rating = (float)0.0;
            int Player_ID = 0;
            DataSet ds = new DataSet();
            ds = GetAbsenteeList(CID, RNR);
            GamesData game = new GamesData();
            DataTable tbl = ds.Tables[0];
            for (int j = 0; j < tbl.Rows.Count; j++)
            {
                // Calculate for each participant all relevant information and add it to the dataset
                DataRow myRow = tbl.Rows[j];
                if (((int)myRow["Afwezigheidscode"] == 1) || (int)myRow["Afwezigheidscode"] == 5)
                {
                    Afwezigheidscode = (int)myRow["Afwezigheidscode"];
                    Player_ID = (int)myRow["Speler_ID"];
                    Rating = GetPlayerRatingInCompetition(Player_ID, RNR, CID);
                    game.Rondernr = (short)RNR;
                    game.Competitie_Id = (short)CID;
                    game.Id_Witspeler = (int)myRow["Speler_ID"];
                    game.Zwart_Winst = 0;
                    game.Zwart_Remise = 0;
                    game.Zwart_Verlies = 0;
                    game.Wedstrijdtype = (byte)Afwezigheidscode;
                    game.Sorteerwaarde = (int)Rating + 3000;
                    game.NumberChampionsgroupGame = 0;
                    if (Afwezigheidscode == 1)
                    {
                        game.Id_Zwartspeler = -1;
                        game.Wedstrijdresultaat = 12;
                        game.Wit_Winst = 0;
                        int aux = Count_Player_Absent(Player_ID, CID, RNR);
                        if (aux >= Get_Free_Absent(CID))
                        {
                            game.Wit_Winst = 0 - Get_Penalty_Absent(CID);
                        }
                        game.Wit_Remise = game.Wit_Winst;
                        game.Wit_Verlies = game.Wit_Winst;
                    }
                    else
                    {
                        game.Id_Zwartspeler = -5;
                        game.Wedstrijdresultaat = 5;
                        game.Wit_Winst = Get_Reward_External(CID);
                        game.Wit_Remise = game.Wit_Winst;
                        game.Wit_Verlies = game.Wit_Winst;
                    }
                    game.Matchpoints_White = 0;
                    game.Matchpoints_Black = 0;
                    game.Pairing_Status_Level = 0;
                    AddGame(game);
                }
            }
        }

        public void Remove_Manual_From_List(int CID, int RNR)
        {
            //
            // Remove manually created games in a swiss tournament from the list of participants to be pairedthe new combination between the competition manager and a competition
            //
            /*
                        string cs = Connection_String_CS();
                        using (SqlConnection con = new SqlConnection(cs))
                        {
                            SqlCommand cmd = new SqlCommand("spRemoveNietInternFromList", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                            cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                            con.Open();
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                //
                                // no action (yet?) with this message, used during debugging
                                //
                                string lit_Error = ex.Message;
                                WriteLogLine("C#", CID, "RemoveNietInternFromList", "Error", 1, lit_Error);

                            }
                            finally
                            {
                                con.Close();
                                con.Dispose();
                            }
                        }
             */
        }

        public void Remove_NietIntern_From_List(int CID, int RNR, int Competition_Type)
        {
            //
            // Remove manually created games from the list of participants to be paired
            //
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spRemoveNietInternFromList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@CTY", SqlDbType.Int).Value = Competition_Type;


                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "RemoveNietInternFromList", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public bool IPEntryDenied(string IPNumber, bool IsCrawler)
        {
            //
            // This function checks the IP number against a blacklist
            //
            bool aux = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {

                    SqlCommand cmd = new SqlCommand("spIPEntryDenied", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IPN", SqlDbType.Text).Value = IPNumber;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Denied"]) == 1;
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "spIPEntryDenied", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                if (aux)
                {
                    if (IsCrawler)
                    {
                        aux = true;
                        using (SqlConnection con2 = new SqlConnection(cs))
                        {
                            con.Open();

                            try
                            {

                                SqlCommand cmd = new SqlCommand("spIPIsCrawler", con2);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@IPN", SqlDbType.Text).Value = IPNumber;
                                int Result = (int)cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                //
                                // no action (yet?) with this message, used during debugging
                                //
                                string lit_Error = ex.Message;
                                WriteLogLine("C#", 0, "spIPIsCrawler", "Error", 1, lit_Error);

                            }
                            finally
                            {
                                con2.Close();
                                con2.Dispose();
                            }
                        }
                    }
                }
            }
            //
            // The return value is a integer number
            //
            return aux;
        }


        private int CountGamesInCompetitions(int PID)
        {
            //
            // This function gets the number of player available for a normal club evening
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {

                    SqlCommand cmd = new SqlCommand("spCountGamesInCompetitions", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Results"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "spCountGamesInCompetitions", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is a integer number
            //
            return aux;
        }

        private int CountGamesInOneCompetition(int PID, int CID, int RNR)
        {
            //
            // This function gets the number of player available for a normal club evening
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {

                    SqlCommand cmd = new SqlCommand("spCountResultsInOneCompetition", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Results"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "spCountGamesInOneCompetition", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is a integer number
            //
            return aux;
        }

        
        private DateTime GetDateLastFreeRound(int PID)
        {
            //
            // This function gets last date on which PID was given a bye. If it has not happened before, the date is set to 01-01-2000
            //
            DateTime aux = System.DateTime.Now ;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {

                    SqlCommand cmd = new SqlCommand("spGetDateLastFreeRound", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToDateTime(reader["Date_of_Round"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "spGetDateLastFreeRound", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is a integer number
            //
            return aux;
        }
        private int Count_Candidates(int CID, int RNR)
        {
            //
            // This function gets the number of player available for a normal club evening
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {

                    SqlCommand cmd = new SqlCommand("spCountPlayers", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Aantal"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "CountCandidates", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is a floating number
            //
            return aux;
        }

        private int Count_Swiss_Pairable_Candidates(int CID, int RNR)
        {
            //
            // This function gets the number of player available for a normal club evening
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {

                    SqlCommand cmd = new SqlCommand("spCountSwissPairablePlayers", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Aantal"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "CountSwissPairableCandidates", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is a floating number
            //
            return aux;
        }

        private int Find_Free_Round_Player(int CID, int RNR)
        {
            //
            // This function gets the number of player available for a normal club evening
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spFreeRoundPlayers", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["FreeGame"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "FreeRoundPlayers", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is a floating number
            //
            return aux;
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private void SetUp_Free_Game(int Free_Player, int CID, int RNR)
        {
            //
            // This function create the game for the Free_Game
            //
            GamesData game = new GamesData();
            game.Id_Witspeler = Free_Player;
            game.Id_Zwartspeler = -3;
            game.Competitie_Id = CID;
            game.Rondernr = RNR;
            game.Wedstrijdresultaat = 13;
            game.Wedstrijdtype = 3;
            game.Sorteerwaarde = 4500;
            game.Wit_Winst = GetKFactor(CID);
            game.Wit_Remise = 0;
            game.Wit_Verlies = 0;
            game.Zwart_Winst = 0;
            game.Zwart_Remise = 0;
            game.Zwart_Verlies = 0;
            game.NumberChampionsgroupGame = 0;
            game.Matchpoints_White = 0;
            game.Matchpoints_Black = 0;
            game.Pairing_Status_Level = 0;
            AddGame(game);

        }

        public void Update_Free_Round_Date (int PID)
        {
            //
            // The new round is published, now the person with the free-round is marked as such
            //
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spUpdateFreeRoundDate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "UpdateFreeroundDate", "Error", 1, "PID = " + PID.ToString());

                }
                finally
                {

                }
                con.Close();
                con.Dispose();
            }
        }

        public void Remove_Games_From_List(int PIDWhite, int PIDBlack, int CID, int RNR)
        {
            //
            // Save the new combination between the competition manager and a competition
            //
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd1 = new SqlCommand("spRemoveOneGameFromList", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd1.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd1.Parameters.Add("@PID", SqlDbType.Int).Value = PIDWhite;

                con.Open();
                try
                {
                    cmd1.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "RemoveOneGameFromList", "Error", 1, "First -" + lit_Error);

                }
                finally
                {

                }
                if (PIDBlack > 0)
                {

                    try
                    {
                        SqlCommand cmd2 = new SqlCommand("spRemoveOneGameFromList", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                        cmd2.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                        cmd2.Parameters.Add("@PID", SqlDbType.Int).Value = PIDBlack;
                        cmd2.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //
                        // no action (yet?) with this message, used during debugging
                        //
                        string lit_Error = ex.Message;
                        WriteLogLine("C#", CID, "RemoveOneGameFromList", "Error", 1, "Second -" + lit_Error);

                    }
                    finally
                    {
                    }

                }
                con.Close();
                con.Dispose();
            }
        }

        private void Remove_Player_From_List(int PID, int CID, int RNR, int Competition_Type)
        {
            //
            // Save the new combination between the competition manager and a competition
            //
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spRemoveOnePlayerFromList", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@CTY", SqlDbType.Int).Value = Competition_Type;


                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "RemoveOneplayerFromList", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
        }

        private int GetUniqueRounds(int CID)
        {
            //
            // This function gets the number of unique rounds within a competition from the tabel Algemene_Info
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetUniqueRounds", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Aantal"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetUniqueRecords", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
            //
            // The return value is a integer number
            //
            return aux;
        }

        public int ChampionsGroupGameNumber(int WID, int BID, int CID)
        {
            //
            // This function gets the number of the championship game in this competition
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetChampionsGroupGameNumber", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@WID", SqlDbType.Int).Value = WID;
                    cmd.Parameters.Add("@BID", SqlDbType.Int).Value = BID;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Game_Number"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetChampionsGroupGameNumber", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
            //
            // The return value is an integer
            //
            return aux;
        }

        public int GetMaxRounds(int CID)
        {
            //
            // This function gets the number of unique rounds within a competition from the tabel Algemene_Info
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetMaxRounds", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Aantal"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetMaxRounds", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
            //
            // The return value is a floating number
            //
            return aux;
        }

        private PairingWorklist GetHighestPlayerLeft(int CID, int RNR)
        {
            //
            // This function gets the highest player on the worklist that has to be paired
            //
            PairingWorklist wl = new PairingWorklist();
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetHighestWorklistRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        wl.Color_Balance = Convert.ToInt16(reader["Color_Balance"]);
                        wl.Mandatory_White = Convert.ToByte(reader["Mandatory_White"]);
                        wl.Mandatory_Black = Convert.ToByte(reader["Mandatory_Black"]);
                        wl.Speler_Id = Convert.ToInt16(reader["Speler_Id"]);
                        wl.Competition_points = Convert.ToSingle(reader["Competition_Points"]);
                        wl.Current_Rating = Convert.ToSingle(reader["Current_Rating"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetHighestWorklistRecord", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is a floating number
            //
            return wl;
        }

        private PairingWorklist GetFollowerPlayerLeft(int PID, int CID, int RNR)
        {
            //
            // This function gets the highest player on the worklist that has to be paired
            //
            int recordcount = 0;
            PairingWorklist wl = new PairingWorklist();
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand("spGetFollowerWorklistRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        recordcount++;
                        wl.Color_Balance = Convert.ToInt16(reader["Color_Balance"]);
                        wl.Mandatory_White = Convert.ToByte(reader["Mandatory_White"]);
                        wl.Mandatory_Black = Convert.ToByte(reader["Mandatory_Black"]);
                        wl.Speler_Id = Convert.ToInt16(reader["Speler_Id"]);
                        wl.Competition_points = Convert.ToSingle(reader["Competition_Points"]);
                        wl.Current_Rating = Convert.ToSingle(reader["Current_Rating"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetFollowerWorklist", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
            //
            // The return value is a floating number
            //
            if (recordcount == 0)
            {
                return null;
            }
            else
            {
                return wl;
            }
        }

        private bool GamePlayed(int PID1, int PID2, int CID, int RNR, int UniqueRounds)
        {
            //
            // This function determines 
            //
            bool aux = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("spCheckGamePresence", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    cmd.Parameters.Add("@PID1", SqlDbType.Int).Value = PID1;
                    cmd.Parameters.Add("@PID2", SqlDbType.Int).Value = PID2;
                    cmd.Parameters.Add("@UNIQUE", SqlDbType.Int).Value = UniqueRounds;
                    da.SelectCommand = cmd;
                    da.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        aux = true;
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "CheckGamePresence", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            return aux;
        }

        private int GetLastColor(int PID, int CID, int RNR)
        {
            //
            // This function returns the color code of the last game for this player
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetLastColor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Kleur"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetLastColor", "Info", 4, "No color found, so no previous game");
                    //
                    // If no color was found, the last color is 0
                    //
                    aux = 0;
                }
            }
            //
            // The return value is either -1, 0 or 1 oating number
            //
            return aux;
        }

        public int GetKNSBNumber(int PID)
        {
            int KNSBRating = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetKNSBnummer", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                    SqlDataReader readerValue = cmd.ExecuteReader();
                    while (readerValue.Read())
                    {
                        KNSBRating = Convert.ToInt32(readerValue["KNSBnummer"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetKNSBNumber", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return KNSBRating;
            }
        }

        private bool SwitchIndicatorHistory(int GCB, int GCBLT)
        {
            bool aux = false;
            if (GCB <= -2) GCB = -2;
            if (GCB >= 2) GCB = 2;

            switch (GCB)
            {
                case -2:
                    {
                        aux = false;
                        break;
                    }
                case -1:
                    {
                        switch (GCBLT)
                        {
                            case -2:
                                {
                                    aux = false;
                                    break;
                                }
                            case 0:
                                {
                                    aux = false;
                                    break;
                                }
                            case 2:
                                {
                                    aux = true;
                                    break;
                                }
                        }
                        break;
                    }
                case 0:
                    {
                        switch (GCBLT)
                        {
                            case -2:
                                {
                                    aux = false;
                                    break;
                                }
                            case 0:
                                {
                                    aux = false;
                                    break;
                                }
                            case 2:
                                {
                                    aux = true;
                                    break;
                                }
                        }
                        break;

                    }
                case 1:
                    {
                        switch (GCBLT)
                        {
                            case -2:
                                {
                                    aux = false;
                                    break;
                                }
                            case 0:
                                {
                                    aux = true;
                                    break;
                                }
                            case 2:
                                {
                                    aux = true;
                                    break;
                                }
                        }
                        break;

                    }
                case 2:
                    {
                        aux = true;
                        break;
                    }

            }
            return aux;
        }


        private bool SwitchGame(PairingWorklist wl1, PairingWorklist wl2, int CID, int RNR, int Color_Last_Game_Player_1)
        {
            //
            // This function calculates the optimisation of the colorbalance after this game is played
            //
            // The return value  is a boolean, telling whether or not it is better to switch the game or not. First check the history over 
            // all available games between the same players, if that is in balance, optimize game history
            //
            // the following table shows the optimised game setup
            /*
            Balance wit	Laatste kleur	Balance zwart	Laatste kleur		Wedstrijd A			Nieuwe Balance Wit		Nieuwe Balance Zwart		Wedstrijd B			Nieuwe Balance Wit		Nieuwe Balance Zwart		TotalBalanceA	TotalBalanceB	LastTwoPlayersAGameA	LastTwoPlayersBGameA	LastTwoPlayersAGameB	LastTwoPlayersBGameB							
            -1	-1	-1	-1		1		-1	0		-2		-1		1	-2		0		-2	-2	0	-2	-2	0		random		or check game history			
            -1	-1	-1	1		1		-1	0		-2		-1		1	-2		0		-2	-2	0	0	-2	2		switch = off					
            -1	1	-1	-1		1		-1	0		-2		-1		1	-2		0		-2	-2	2	-2	0	0		switch = on					
            -1	1	-1	1		1		-1	0		-2		-1		1	-2		0		-2	-2	2	0	0	2		random		or check game history			
																															
            -1	-1	0	-1		1		-1	0		-1		-1		1	-2		1		-1	-1	0	-2	-2	0		random		or check game history			
            -1	-1	0	0		1		-1	0		-1		-1		1	-2		1		-1	-1	0	-1	-2	1		switch = off					
            -1	-1	0	1		1		-1	0		-1		-1		1	-2		1		-1	-1	0	0	-2	2		switch = off					
            -1	1	0	-1		1		-1	0		-1		-1		1	-2		1		-1	-1	2	-2	0	0		switch = on					
            -1	1	0	0		1		-1	0		-1		-1		1	-2		1		-1	-1	2	-1	0	1		switch = on					
            -1	1	0	1		1		-1	0		-1		-1		1	-2		1		-1	-1	2	0	0	2		random		or check game history			
																															
            -1	-1	1	-1		1		-1	0		0		-1		1	-2		2		0	0	0	-2	-2	0		random		or check game history			
            -1	-1	1	1		1		-1	0		0		-1		1	-2		2		0	0	0	0	-2	2		switch = off					
            -1	1	1	-1		1		-1	0		0		-1		1	-2		2		0	0	2	-2	0	0		switch = on					
            -1	1	1	1		1		-1	0		0		-1		1	-2		2		0	0	2	0	0	2		random		or check game history			
																															
            0	-1	-1	-1		1		-1	1		-2		-1		1	-1		0		-1	-1	0	-2	-2	0		random		or check game history			
            0	-1	-1	1		1		-1	1		-2		-1		1	-1		0		-1	-1	0	0	-2	2		switch = off					
            0	0	-1	-1		1		-1	1		-2		-1		1	-1		0		-1	-1	1	-2	-1	0		switch = on					
            0	0	-1	1		1		-1	1		-2		-1		1	-1		0		-1	-1	1	0	-1	2		switch = off					
            0	1	-1	-1		1		-1	1		-2		-1		1	-1		0		-1	-1	2	-2	0	0		switch = on					
            0	1	-1	1		1		-1	1		-2		-1		1	-1		0		-1	-1	2	0	0	2		random		or check game history			
																															
            0	-1	0	-1		1		-1	1		-1		-1		1	-1		1		0	0	0	-2	-2	0		random		or check game history			
            0	-1	0	0		1		-1	1		-1		-1		1	-1		1		0	0	0	-1	-2	1		switch = off					
            0	-1	0	1		1		-1	1		-1		-1		1	-1		1		0	0	0	0	-2	2		switch = off					
            0	0	0	-1		1		-1	1		-1		-1		1	-1		1		0	0	1	-2	-1	0		switch = on					
            0	0	0	0		1		-1	1		-1		-1		1	-1		1		0	0	1	-1	-1	1		random		or check game history			
            0	0	0	1		1		-1	1		-1		-1		1	-1		1		0	0	1	0	-1	2		switch = off					
            0	1	0	-1		1		-1	1		-1		-1		1	-1		1		0	0	2	-2	0	0		switch = on					
            0	1	0	0		1		-1	1		-1		-1		1	-1		1		0	0	2	-1	0	1		switch = on					
            0	1	0	1		1		-1	1		-1		-1		1	-1		1		0	0	2	0	0	2		random		or check game history			
																															
            0	-1	1	-1		1		-1	1		0		-1		1	-1		2		1	1	0	-2	-2	0		random		or check game history			
            0	-1	1	1		1		-1	1		0		-1		1	-1		2		1	1	0	0	-2	2		switch = off					
            0	0	1	-1		1		-1	1		0		-1		1	-1		2		1	1	1	-2	-1	0		switch = on					
            0	0	1	1		1		-1	1		0		-1		1	-1		2		1	1	1	0	-1	2		switch = off					
            0	1	1	-1		1		-1	1		0		-1		1	-1		2		1	1	2	-2	0	0		switch = on					
            0	1	1	1		1		-1	1		0		-1		1	-1		2		1	1	2	0	0	2		random		or check game history			
																															
            1	-1	-1	-1		1		-1	2		-2		-1		1	0		0		0	0	0	-2	-2	0		random		or check game history			
            1	-1	-1	1		1		-1	2		-2		-1		1	0		0		0	0	0	0	-2	2		switch = off					
            1	1	-1	-1		1		-1	2		-2		-1		1	0		0		0	0	2	-2	0	0		switch = on					
            1	1	-1	1		1		-1	2		-2		-1		1	0		0		0	0	2	0	0	2		random		or check game history			
																															
            1	-1	0	-1		1		-1	2		-1		-1		1	0		1		1	1	0	-2	-2	0		random		or check game history			
            1	-1	0	0		1		-1	2		-1		-1		1	0		1		1	1	0	-1	-2	1		switch = off					
            1	-1	0	1		1		-1	2		-1		-1		1	0		1		1	1	0	0	-2	2		switch = off					
            1	1	0	-1		1		-1	2		-1		-1		1	0		1		1	1	2	-2	0	0		switch = on					
            1	1	0	0		1		-1	2		-1		-1		1	0		1		1	1	2	-1	0	1		switch = on					
            1	1	0	1		1		-1	2		-1		-1		1	0		1		1	1	2	0	0	2		random		or check game history			
																															
            1	-1	1	-1		1		-1	2		0		-1		1	0		2		2	2	0	-2	-2	0		random		or check game history			
            1	-1	1	1		1		-1	2		0		-1		1	0		2		2	2	0	0	-2	2		switch = off					
            1	1	1	-1		1		-1	2		0		-1		1	0		2		2	2	2	-2	0	0		switch = on					
            1	1	1	1		1		-1	2		0		-1		1	0		2		2	2	2	0	0	2		random		or check game history			

            */
            bool aux = false;
            bool finished = false;

            int ColorP1 = GetLastColor(wl1.Speler_Id, CID, RNR);
            int ColorP2 = GetLastColor(wl2.Speler_Id, CID, RNR);

            if ((wl1.Mandatory_White == 1) || (wl2.Mandatory_Black == 1))
            {
                aux = false;
                finished = true;
            }
            if ((wl2.Mandatory_White == 1) || (wl1.Mandatory_Black == 1))
            {
                aux = true;
                finished = true;
            }
            //
            // If no mandatory color, than always switch the color
            //
            if (!finished)
            {
                switch (Color_Last_Game_Player_1)
                {
                    case 1:
                        {
                            aux = true;
                            break;
                        }
                    case 0:
                        {
                            aux = (RandomNumber(1, 100) % 2) == 1;
                            break;
                        }
                    case -1:
                        {
                            aux = false;
                            break;
                        }
                }
            }
            /*
                        if (!finished)
                        {
                            switch (wl1.Color_Balance)
                            {
                                case -1:
                                    switch (wl2.Color_Balance)
                                    {
                                        case -1:
                                            switch (ColorP1)
                                            {
                                                case -1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                        case 1:
                                                            aux = false;
                                                            break;
                                                    }
                                                    break;
                                                case 1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = true;
                                                            break;
                                                        case 1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case 0:
                                            switch (ColorP1)
                                            {
                                                case -1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                        case 0:
                                                            aux = false;
                                                            break;
                                                        case 1:
                                                            aux = false;
                                                            break;
                                                    }
                                                    break;
                                                case 1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = true;
                                                            break;
                                                        case 0:
                                                            aux = true;
                                                            break;
                                                        case 1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case 1:
                                            switch (ColorP1)
                                            {
                                                case -1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                        case 1:
                                                            aux = false;
                                                            break;
                                                    }
                                                    break;
                                                case 1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = true;
                                                            break;
                                                        case 1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case 0:
                                    switch (wl2.Color_Balance)
                                    {
                                        case -1:
                                            switch (ColorP1)
                                            {
                                                case -1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                        case 1:
                                                            aux = false;
                                                            break;
                                                    }
                                                    break;
                                                case 0:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = true;
                                                            break;
                                                        case 1:
                                                            aux = false;
                                                            break;
                                                    }
                                                    break;
                                                case 1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = true;
                                                            break;
                                                        case 1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case 0:
                                            switch (ColorP1)
                                            {
                                                case -1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                        case 0:
                                                            aux = false;
                                                            break;
                                                        case 1:
                                                            aux = false;
                                                            break;
                                                    }
                                                    break;
                                                case 0:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = true;
                                                            break;
                                                        case 0:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                        case 1:
                                                            aux = false;
                                                            break;
                                                    }
                                                    break;
                                                case 1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = true;
                                                            break;
                                                        case 0:
                                                            aux = true;
                                                            break;
                                                        case 1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case 1:
                                            switch (ColorP1)
                                            {
                                                case -1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0;
                                                            break;
                                                        case 1:
                                                            aux = false;
                                                            break;
                                                    }
                                                    break;
                                                case 0:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = true;
                                                            break;
                                                        case 1:
                                                            aux = false;
                                                            break;
                                                    }
                                                    break;
                                                case 1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = true;
                                                            break;
                                                        case 1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case 1:
                                    switch (wl2.Color_Balance)
                                    {
                                        case -1:
                                            switch (ColorP1)
                                            {
                                                case -1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                        case 1:
                                                            aux = false;
                                                            break;
                                                    }
                                                    break;
                                                case 1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = true;
                                                            break;
                                                        case 1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case 0:
                                            switch (ColorP1)
                                            {
                                                case -1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                        case 0:
                                                            aux = false;
                                                            break;
                                                        case 1:
                                                            aux = false;
                                                            break;
                                                    }
                                                    break;
                                                case 1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = true;
                                                            break;
                                                        case 0:
                                                            aux = true;
                                                            break;
                                                        case 1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case 1:
                                            switch (ColorP1)
                                            {
                                                case -1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                        case 1:
                                                            aux = false;
                                                            break;
                                                    }
                                                    break;
                                                case 1:
                                                    switch (ColorP2)
                                                    {
                                                        case -1:
                                                            aux = true;
                                                            break;
                                                        case 1:
                                                            aux = (RandomNumber(1, 100) % 2) == 0; 
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }
             */
            return aux;
        }

        private void ResetWorklist(int CID, int RNR)
        {
            //
            // Save the new combination between the competition manager and a competition
            //
            int Color_Balance = 0;
            int Color_Balance_Last_Two = 0;
            float ELO_Result = 0;
            float CompetitionResult = 0;
            int Absent = 0;
            int Games = 0;
            float Points = (float)0.0;
            float Restart_CompetitionPoints = (float)1000.0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd1 = new SqlCommand("spResetWorklistStep1", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd1.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    cmd1.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "ResetWorklistStep1", "Error", 1, lit_Error);
                }

                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();

                SqlCommand cmd2 = new SqlCommand("spResetWorklistStep2", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd2.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                da.SelectCommand = cmd2;
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "ResetWorklistStep2", "Error", 1, lit_Error);
                }

                PairingWorklist wl = new PairingWorklist();
                DataTable dt = ds.Tables[0];
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    Restart_CompetitionPoints = Restart_CompetitionPoints - 1.0f;
                    DataRow Single_Game = dt.Rows[j];
                    wl.Speler_Id = System.Convert.ToInt32(Single_Game["ID_Witspeler"]);
                    wl.Round_Free = 0;
                    //
                    // assemble all relevant data for print lists and new pairing
                    //
                    wl.Start_Rating = GetPlayerStartRating(wl.Speler_Id, CID);
                    wl.Competition_Id = CID;
                    wl.Start_Competition_Points = GetPlayerStartCompetitionPoints(wl.Speler_Id, CID);
                    wl.Round_Number = RNR;
                    GetResultsData_Simple(wl.Speler_Id, CID, RNR, ref Color_Balance, ref Color_Balance_Last_Two, ref ELO_Result, ref CompetitionResult, ref Absent, ref Games, ref Points);
                    wl.Mandatory_White = Convert.ToInt32((Color_Balance < -1) || (Color_Balance_Last_Two < -1));
                    wl.Mandatory_Black = Convert.ToInt32((Color_Balance > 1) || (Color_Balance_Last_Two > 1));
                    wl.Color_Balance = Color_Balance;
                    wl.Current_Rating = wl.Start_Rating + ELO_Result;
                    wl.Competition_points = Restart_CompetitionPoints;
                    wl.Gain_Loss = CompetitionResult;
                    wl.Rating_Gain = ELO_Result;
                    wl.Aantal_Afmeldingen = Absent;
                    wl.Aantal_Partijen = Games;
                    wl.Aantal_Punten = Points;
                    if (Games > 0)
                    {
                        wl.Percentage = ((float)Points * (float)100.0) / (float)Games;
                    }
                    else
                    {
                        wl.Percentage = (float)0.0;
                    }
                    //
                    // Add all information to the database table temp_list
                    //
                    AddWorklistRecord(wl);
                    //
                    Restart_CompetitionPoints = Restart_CompetitionPoints - 1.0f;
                    wl.Speler_Id = System.Convert.ToInt32(Single_Game["ID_Zwartspeler"]);
                    wl.Round_Free = 0;
                    //
                    // assemble all relevant data for print lists and new pairing
                    //
                    wl.Start_Rating = GetPlayerStartRating(wl.Speler_Id, CID);
                    wl.Competition_Id = CID;
                    wl.Start_Competition_Points = GetPlayerStartCompetitionPoints(wl.Speler_Id, CID);
                    wl.Round_Number = RNR;
                    GetResultsData_Simple(wl.Speler_Id, CID, RNR, ref Color_Balance, ref Color_Balance_Last_Two, ref ELO_Result, ref CompetitionResult, ref Absent, ref Games, ref Points);
                    wl.Mandatory_White = Convert.ToInt32((Color_Balance < -1) || (Color_Balance_Last_Two < -1));
                    wl.Mandatory_Black = Convert.ToInt32((Color_Balance > 1) || (Color_Balance_Last_Two > 1));
                    wl.Color_Balance = Color_Balance;
                    wl.Current_Rating = wl.Start_Rating + ELO_Result;
                    wl.Competition_points = Restart_CompetitionPoints;
                    wl.Gain_Loss = CompetitionResult;
                    wl.Rating_Gain = ELO_Result;
                    wl.Aantal_Afmeldingen = Absent;
                    wl.Aantal_Partijen = Games;
                    wl.Aantal_Punten = Points;
                    if (Games > 0)
                    {
                        wl.Percentage = ((float)Points * (float)100.0) / (float)Games;
                    }
                    else
                    {
                        wl.Percentage = (float)0.0;
                    }
                    //
                    // Add all information to the database table temp_list
                    //
                    AddWorklistRecord(wl);
                }
            }
        }

        private void SetUp_Competition_Games(int CID, int RNR)
        {
            //
            // This function calculates the next round for all still available players
            //
            int kf = Convert.ToInt16(GetKFactor(CID));
            float Acceleration = GetAcceleration(CID);
            const int C_SCD = 1;
            int Skip_Players = System.Math.Max((5 - RNR), 0);
            int Follower_of = 0;
            int Games_Paired = 0;
            bool Game_Found = false;
            bool Color_Conflict = false;
            int Color_Last_Game_Player_1 = 0;

            PairingWorklist wl1 = new PairingWorklist();
            PairingWorklist wl2 = new PairingWorklist();
            GamesData game = new GamesData();
            //
            // Start searching for players until no candidates are left:
            //
            while (Count_Candidates(CID, RNR) > 0)
            {
                Game_Found = false;
                wl1 = GetHighestPlayerLeft(CID, RNR);
                WriteLogLine("C#", CID, "Setup_Competition_Games", "Info", 4, "Speler 1:" + GetPlayerName(wl1.Speler_Id) + "(" + wl1.Speler_Id.ToString() + ")");
                Follower_of = wl1.Speler_Id;
                //
                // Skip the strongest games to get interesting games later on in the competition
                //
                if (Skip_Players > 0)
                {
                    if (Games_Paired < Skip_Players)
                    {
                        for (int j = 0; j < Skip_Players; j++)
                        {
                            wl2 = GetFollowerPlayerLeft(Follower_of, CID, RNR);
                            Follower_of = wl2.Speler_Id;
                        }
                    }
                }
                //
                // Now find the follower until a game is found
                //
                while (Game_Found == false)
                {
                    wl2 = GetFollowerPlayerLeft(Follower_of, CID, RNR);
                    if (wl2 != null)
                    {
                        WriteLogLine("C#", CID, "Setup_Competition_Games", "Info", 4, "Speler 2:" + GetPlayerName(wl2.Speler_Id) + "(" + wl2.Speler_Id.ToString() + ")");
                        if (!GamePlayed(wl1.Speler_Id, wl2.Speler_Id, CID, RNR, GetUniqueRounds(CID)))
                        {
                            Color_Conflict = ((wl1.Mandatory_White == 1) && (wl2.Mandatory_White == 1)) || ((wl1.Mandatory_Black == 1) && (wl2.Mandatory_Black == 1));
                            Color_Last_Game_Player_1 = GetGameLastColor(wl1.Speler_Id, wl2.Speler_Id, CID);
                            if (!Color_Conflict)
                            {
                                Color_Conflict = 
                                    (((wl1.Mandatory_White == 1) && (Color_Last_Game_Player_1 == 1)) ||
                                    ((wl1.Mandatory_Black == 1) && (Color_Last_Game_Player_1 == -1)) ||
                                    ((wl2.Mandatory_White == 1) && (Color_Last_Game_Player_1 == -1)) ||
                                    ((wl2.Mandatory_Black == 1) && (Color_Last_Game_Player_1 == 1)));
                            } 
                            if (!Color_Conflict)
                            {
                                Game_Found = true;
                                Games_Paired++;
                                if (SwitchGame(wl1, wl2, CID, RNR, Color_Last_Game_Player_1))
                                {
                                    game.Id_Witspeler = wl2.Speler_Id;
                                    game.Id_Zwartspeler = wl1.Speler_Id;
                                    game.Competitie_Id = CID;
                                    game.Rondernr = RNR;
                                    game.Wedstrijdresultaat = 0;
                                    game.Wedstrijdtype = 4;
                                    game.Sorteerwaarde = (int)System.Math.Max(wl1.Current_Rating, wl2.Current_Rating);
                                    game.Wit_Winst = Calculate_Competitionpoints_One_Game(wl2.Current_Rating, wl1.Current_Rating, kf, 1, Acceleration);
                                    game.Wit_Remise = Calculate_Competitionpoints_One_Game(wl2.Current_Rating, wl1.Current_Rating, kf, 2, Acceleration);
                                    game.Wit_Verlies = Calculate_Competitionpoints_One_Game(wl2.Current_Rating, wl1.Current_Rating, kf, 3, Acceleration);
                                    game.Zwart_Winst = 0 - game.Wit_Verlies;
                                    game.Zwart_Remise = 0 - game.Wit_Remise;
                                    game.Zwart_Verlies = 0 - game.Wit_Winst;
                                    game.Matchpoints_White = 0;
                                    game.Matchpoints_Black = 0;
                                    game.Pairing_Status_Level = 0;
                                    AddGame(game);
                                    Remove_Player_From_List(wl1.Speler_Id, CID, RNR, C_SCD);
                                    Remove_Player_From_List(wl2.Speler_Id, CID, RNR, C_SCD);
                                    WriteLogLine("C#", CID, "Setup_Competition_Games", "Info", 4, "Geaccepteerd: Switched; partijen:" + Games_Paired.ToString() + ", spelers over: " + Count_Candidates(CID, RNR).ToString());
                                }
                                else
                                {
                                    game.Id_Witspeler = wl1.Speler_Id;
                                    game.Id_Zwartspeler = wl2.Speler_Id;
                                    game.Competitie_Id = CID;
                                    game.Rondernr = RNR;
                                    game.Wedstrijdresultaat = 0;
                                    game.Wedstrijdtype = 4;
                                    game.Sorteerwaarde = (int)System.Math.Max(wl1.Current_Rating, wl2.Current_Rating);
                                    game.Wit_Winst = Calculate_Competitionpoints_One_Game(wl1.Current_Rating, wl2.Current_Rating, kf, 1, Acceleration);
                                    game.Wit_Remise = Calculate_Competitionpoints_One_Game(wl1.Current_Rating, wl2.Current_Rating, kf, 2, Acceleration);
                                    game.Wit_Verlies = Calculate_Competitionpoints_One_Game(wl1.Current_Rating, wl2.Current_Rating, kf, 3, Acceleration);
                                    game.Zwart_Winst = 0 - game.Wit_Verlies;
                                    game.Zwart_Remise = 0 - game.Wit_Remise;
                                    game.Zwart_Verlies = 0 - game.Wit_Winst;
                                    game.Matchpoints_White = 0;
                                    game.Matchpoints_Black = 0;
                                    game.Pairing_Status_Level = 0;
                                    AddGame(game);
                                    Remove_Player_From_List(wl1.Speler_Id, CID, RNR, C_SCD);
                                    Remove_Player_From_List(wl2.Speler_Id, CID, RNR, C_SCD);
                                    WriteLogLine("C#", CID, "Setup_Competition_Games", "Info", 4, "Geaccepteerd, partijen:" + Games_Paired.ToString() + ", spelers over: " + Count_Candidates(CID, RNR).ToString());
                                }

                            }
                            else
                            {
                                WriteLogLine("C#", CID, "Setup_Competition_Games", "Info", 4, "Afgewezen:Verplichte Kleur van spelers of partij");
                                Follower_of = wl2.Speler_Id;
                            }
                        }
                        else
                        {
                            WriteLogLine("C#", CID, "Setup_Competition_Games", "Info", 4, "Afgewezen:Gespeeld");
                            Follower_of = wl2.Speler_Id;
                        }
                    }
                    else
                    {
                        //
                        // There hase no player been found for the first player, so the worklist is build back but reverted and in the order of the games 
                        //
                        WriteLogLine("C#", CID, "Setup_Competition_Games", "Info", 4, "Onderaan vastgelopen, opnieuw");
                        Skip_Players = 0;
                        ResetWorklist(CID, RNR);
                        Cleanup_Before_Pairing(CID, RNR, 4);
                        Game_Found = true;
                        Games_Paired = 0;
                    }
                }
            }
        }



        public bool Calculate_New_Pairing(int CID, int RNR)
        {
            //
            // This function creates the records that describe the games to be played in a specific round of a specific competition
            //
            const int C_SCD = 1;
            bool error_occurred = false;
            Cleanup_Before_Pairing(CID, RNR, 0);
            Create_Worklist(CID, RNR);
            SetUp_Non_Competition_Games(CID, RNR);
            SetUp_ChampionsGroup_Games(CID, RNR);
            Remove_NietIntern_From_List(CID, RNR, C_SCD);
            int Number_of_Candidates = Count_Candidates(CID, RNR);
            if (Number_of_Candidates % 2 != 0)
            {
                int Free_Player = Find_Free_Round_Player(CID, RNR);
                SetUp_Free_Game(Free_Player, CID, RNR);
                Remove_Player_From_List(Free_Player, CID, RNR, C_SCD);
            }

            SetUp_Competition_Games(CID, RNR);
            //
            // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
            //
            return error_occurred;
        }
        /* next the module to read the gamelist dataset */

        public DataSet GetGameList(int CID, int RNR, int GameType, int Language)
        {
            //
            // This function results in a dataset with all previously registered absentees and externals from the NietIntern table of a specific round of a 
            // specific competition
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetGamesList";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    cmd.Parameters.Add("@GameType", SqlDbType.Int).Value = GameType;
                    cmd.Parameters.Add("@LAN", SqlDbType.Int).Value = Language;
                    da.SelectCommand = cmd;
                    con.Open();
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetGameList", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return ds;
        }

        public DataSet GetOneGame(int PID, int CID, int RNR)
        {
            //
            // This function gets all player ids with mail adresses for sending pairing information
            // multiple situations
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetOneGame";
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                da.SelectCommand = cmd;
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetOneGame", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public DataSet GetMailAddresses(int CID)
        {
            //
            // This function gets all player ids with mail adresses for sending pairing information
            // multiple situations
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetPlayersWithMail";
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                da.SelectCommand = cmd;
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetPlayersWithMail", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public string FTP_Basis_Upload_File(string Display_Target, string App_Source, int CID, string File_Name)
        {
            string aux = "";
            FTP ftplib = new FTP();

            try
            {
                // there are server, user and password properties
                // that can be set within the ftplib object as well
                // those properties are actually set when
                // you call the Connect(server, user, pass) function
                string Webserver_host = GetStringFromAlgemeneInfo(CID, "Client_FTP_Host").Trim();
                string user = GetStringFromAlgemeneInfo(CID, "Client_FTP_UN").Trim();
                string pass = GetStringFromAlgemeneInfo(CID, "Client_FTP_PW").Trim();
                ftplib.Connect(Webserver_host, user, pass);
                ftplib.ChangeDir(Display_Target);
            }
            catch (Exception ex)
            {
                string lit_Error = ex.Message;
                WriteLogLine("C#", CID, "FTP_Basis_Upload_File-1", "Error", 1, lit_Error + ", " + Display_Target);
                aux = "-1-" + lit_Error + ", " + Display_Target;
            }
            //
            // Upload file itself
            //

            try
            {
                int perc = 0;

                // open the file with resume support if it already exists, the last 
                // peram should be false for no resume
                string source = App_Source + "\\" + File_Name;
                ftplib.OpenUpload(source, File_Name);
                while (ftplib.DoUpload() > 0)
                {
                    perc = (int)((ftplib.BytesTotal * 100) / ftplib.FileSize);
                }
            }
            catch (Exception ex)
            {
                string lit_Error = ex.Message;
                WriteLogLine("C#", CID, "FTP_Basis_Upload_File-2", "Error", 1, lit_Error + ", " + Display_Target);
                aux += "-2-" + lit_Error + ", " + Display_Target;
            }
            //
            // Close connection
            //
            ftplib.Disconnect();

            return aux;
        }

        public DataSet GetGamesUpdateList(int CID, int RNR)
        {
            //
            // This function results in a dataset with all previously registered absentees and externals from the NietIntern table of a specific round of a 
            // specific competition
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetGamesUpdateList";
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                da.SelectCommand = cmd;
                con.Open();
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetGamesUpdateList", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return ds;
        }

        public void AddManualGame(int PIDWhite, int PIDBlack, int CID, int RNR, int ChampionsgroupGame)
        {
            //
            // This function calculates the next round for all still available players
            //
            float kf = Convert.ToSingle(GetKFactor(CID));
            float Acceleration = GetAcceleration(CID);
            float RatingWhite = Convert.ToSingle(GetPlayerRatingInCompetition(PIDWhite, RNR, CID));
            float RatingBlack = Convert.ToSingle(GetPlayerRatingInCompetition(PIDBlack, RNR, CID));

            GamesData game = new GamesData();

            game.Id_Witspeler = PIDWhite;
            game.Id_Zwartspeler = PIDBlack;
            game.Competitie_Id = CID;
            game.Rondernr = RNR;
            switch (PIDBlack)
            {
                case -5:
                    game.Wedstrijdresultaat = 5;
                    game.Wedstrijdtype = 5;
                    game.Sorteerwaarde = (int)RatingWhite + 4500;
                    game.Wit_Winst = Get_Reward_External(CID);
                    game.Wit_Remise = game.Wit_Winst;
                    game.Wit_Verlies = game.Wit_Winst;
                    game.Zwart_Winst = 0;
                    game.Zwart_Remise = 0;
                    game.Zwart_Verlies = 0;
                    game.NumberChampionsgroupGame = 0;
                    break;
                case -3:
                    game.Wedstrijdresultaat = 13;
                    game.Wedstrijdtype = 3;
                    game.Sorteerwaarde = 4500;
                    game.Wit_Winst = GetKFactor(CID);
                    game.Wit_Remise = 0;
                    game.Wit_Verlies = 0;
                    game.Zwart_Winst = 0;
                    game.Zwart_Remise = 0;
                    game.Zwart_Verlies = 0;
                    game.NumberChampionsgroupGame = 0;
                    Update_Free_Round_Date(PIDWhite);
                    break;
                case -1:
                    int aux = Count_Player_Absent(PIDWhite, CID, RNR);
                    if (aux >= Get_Free_Absent(CID))
                    {
                        game.Wit_Winst = 0 - Get_Penalty_Absent(CID);
                    }
                    game.Wedstrijdresultaat = 12;
                    game.Wedstrijdtype = 1;
                    game.Sorteerwaarde = (int)RatingWhite + 3000;
                    game.Wit_Remise = game.Wit_Winst;
                    game.Wit_Verlies = game.Wit_Winst;
                    game.Zwart_Winst = 0;
                    game.Zwart_Remise = 0;
                    game.Zwart_Verlies = 0;
                    game.NumberChampionsgroupGame = 0;
                    break;
                default:
                    game.Wedstrijdresultaat = 0;
                    game.Sorteerwaarde = (int)System.Math.Max(RatingWhite, RatingBlack);
                    if (ChampionsgroupGame > 0)
                    {
                        game.Wedstrijdtype = 8;
                        game.Sorteerwaarde = game.Sorteerwaarde + 3000;
                    }
                    else
                    {
                        game.Wedstrijdtype = 4;
                    }
                    game.Wit_Winst = Calculate_Competitionpoints_One_Game(RatingWhite, RatingBlack, kf, 1, Acceleration);
                    game.Wit_Remise = Calculate_Competitionpoints_One_Game(RatingWhite, RatingBlack, kf, 2, Acceleration);
                    game.Wit_Verlies = Calculate_Competitionpoints_One_Game(RatingWhite, RatingBlack, kf, 3, Acceleration);
                    game.Zwart_Winst = 0 - game.Wit_Verlies;
                    game.Zwart_Remise = 0 - game.Wit_Remise;
                    game.Zwart_Verlies = 0 - game.Wit_Winst;
                    game.NumberChampionsgroupGame = ChampionsgroupGame;
                    break;
            }
            game.Matchpoints_White = 0;
            game.Matchpoints_Black = 0;
            game.Pairing_Status_Level = 0;
            AddGame(game);
        }
        //
        // Modules for results processing
        //
        public DataSet GetResultsGameList(int CID, int RNR)
        {
            //
            // This function results in a dataset with all games of a specific round of a 
            // specific competition
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetResultsGamesList";
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                da.SelectCommand = cmd;
                con.Open();
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetResultsGamesList", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return ds;
        }

        public bool UpdateResult(int PID_White, int PID_Black, int CID, int RNR, int auxR)
        {
            //
            // This function updates the game result record 
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spUpdateGameResult", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    cmd.Parameters.Add("@PIDW", SqlDbType.NVarChar).Value = PID_White;
                    cmd.Parameters.Add("@PIDB", SqlDbType.Int).Value = PID_Black;
                    cmd.Parameters.Add("@Result", SqlDbType.Int).Value = auxR;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "UpdateGameResult", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        public bool AllResultsEntered(int CID, int RNR)
        {
            //
            // This module checks if all game-results are entered
            //
            bool aux = false;
            int LR_Value = 0;

            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spCountResults", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    SqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            LR_Value = Convert.ToInt16(reader["Results"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        string lit_Error = ex.Message;
                        WriteLogLine("C#", CID, "CountResults", "Error", 1, lit_Error);
                        //
                        // If no color was found, the last color is 0
                        //
                        LR_Value = 0;
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "CountResults", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            aux = (LR_Value == 0);
            //
            // This function returns a Boolean value
            //
            return aux;
        }

        private DataSet GetGamesCompleteData(int CID, int RNR)
        {
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetGamesFullData";
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                da.SelectCommand = cmd;
                con.Open();
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetGamesFullData", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return ds;
        }

        private DataSet GetResultsPlayerIDData(int CID, int RNR)
        {
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetResultsPlayerIdData";
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                da.SelectCommand = cmd;
                con.Open();
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetResultsPlayerIdData", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return ds;
        }

        private int InverseResult(int ResultCode)
        {
            int aux = -1;
            switch (ResultCode)
            {
                case 0: return 0;
                case 1: return 3;
                case 2: return 2;
                case 3: return 1;
                case 4: return 4;
                case 5: return -1;
                case 6: return -1;
                case 7: return -1;
                case 8: return -1;
                case 9: return 11;
                case 10: return 10;
                case 11: return 9;
                case 12: return -1;
                case 13: return -1;
                case 14: return 14;
                case 15: return 15;
            }
            return aux;
        }

        public void Create_Result_Records(int CID, int RNR)
        {
            //
            // Creates new datarecords in CompetitieResultaat and deleting old ones for this record
            //
            DataSet ds = GetGamesCompleteData(CID, RNR);
            ResultData ORW = new ResultData();
            ResultData ORB = new ResultData();
            int PID_White;
            int PID_Black;
            float kf = GetKFactor(CID);
            float ELO_White;
            float ELO_Black;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                PID_Black = (int)row["ID_Zwartspeler"];
                ORW.Competitie_Id = CID;
                ORW.Rondernr = RNR;
                ORW.Partijnaam = "";
                PID_White = (int)row["ID_Witspeler"];
                ORW.Deelnemer_ID = PID_White;
                ORW.Tegenstander = PID_Black;
                ORW.Resultaat = (byte)row["Resultaat_Wedstrijd"];
                ORW.Plaats_Op_Ranglijst = 0;
                ORW.CorrectieOpElo = 0;
                ORW.CorrectieOpCompetitie = 0;
                ORW.ChampionsgroupGameNumber = (int)row["NumberChampionsgroupGame"];
                //
                // Kleur value will be overruled if it turns out this was a game between two players and not a "special" game
                ORW.Kleur = 0;
                switch (ORW.Resultaat)
                {
                    case 1:
                        ORW.Competitie_Resultaat = Convert.ToSingle(row["Wit_Winst"]);
                        break;
                    case 2:
                        ORW.Competitie_Resultaat = Convert.ToSingle(row["Wit_Remise"]);
                        break;
                    case 3:
                        ORW.Competitie_Resultaat = Convert.ToSingle(row["Wit_Verlies"]);
                        break;
                    case 9:
                        ORW.Competitie_Resultaat = Convert.ToSingle(row["Wit_Winst"]);
                        break;
                    case 10:
                        ORW.Competitie_Resultaat = Convert.ToSingle(row["Wit_Remise"]);
                        break;
                    case 11:
                        ORW.Competitie_Resultaat = Convert.ToSingle(row["Wit_Verlies"]);
                        break;
                    default:
                        ORW.Competitie_Resultaat = Convert.ToSingle(row["Wit_Winst"]);
                        break;
                }
                if (PID_Black > 0)
                {
                    ELO_White = GetPlayerRatingInCompetition(PID_White, RNR, CID);
                    ELO_Black = GetPlayerRatingInCompetition(PID_Black, RNR, CID);
                    switch (ORW.Resultaat)
                    {
                        case 1:
                            ORW.ELO_Resultaat = Calculate_Rating_One_Game(ELO_White, ELO_Black, kf, ORW.Resultaat);
                            break;
                        case 2:
                            ORW.ELO_Resultaat = Calculate_Rating_One_Game(ELO_White, ELO_Black, kf, ORW.Resultaat);
                            break;
                        case 3:
                            ORW.ELO_Resultaat = Calculate_Rating_One_Game(ELO_White, ELO_Black, kf, ORW.Resultaat);
                            break;
                        default:
                            ORW.ELO_Resultaat = 0;
                            break;
                    }
                }
                else
                {
                    ORW.ELO_Resultaat = 0;
                }
                //
                // create Black result record
                //
                if (PID_Black > 0)
                {
                    ORB.Competitie_Id = CID;
                    ORB.Rondernr = RNR;
                    ORB.Partijnaam = "";
                    ORB.Deelnemer_ID = PID_Black;
                    ORB.Tegenstander = PID_White;
                    ORW.Kleur = 1;
                    ORB.Kleur = -1;
                    ORB.Resultaat = InverseResult((byte)row["Resultaat_Wedstrijd"]);
                    ORB.Plaats_Op_Ranglijst = 0;
                    ORB.CorrectieOpElo = 0;
                    ORB.CorrectieOpCompetitie = 0;
                    ORB.ChampionsgroupGameNumber = (int)row["NumberChampionsgroupGame"];
                    ELO_White = GetPlayerRatingInCompetition(PID_White, RNR, CID);
                    ELO_Black = GetPlayerRatingInCompetition(PID_Black, RNR, CID);
                    switch (ORB.Resultaat)
                    {
                        case 1:
                            ORB.Competitie_Resultaat = Convert.ToSingle(row["Zwart_Winst"]);
                            ORB.ELO_Resultaat = 0 - ORW.ELO_Resultaat;
                            break;
                        case 2:
                            ORB.Competitie_Resultaat = Convert.ToSingle(row["Zwart_Remise"]);
                            ORB.ELO_Resultaat = 0 - ORW.ELO_Resultaat;
                            break;
                        case 3:
                            ORB.Competitie_Resultaat = Convert.ToSingle(row["Zwart_Verlies"]);
                            ORB.ELO_Resultaat = 0 - ORW.ELO_Resultaat;
                            break;
                        case 9:
                            ORB.Competitie_Resultaat = Convert.ToSingle(row["Zwart_Winst"]);
                            ORB.ELO_Resultaat = 0;
                            break;
                        case 10:
                            ORB.Competitie_Resultaat = Convert.ToSingle(row["Zwart_Remise"]);
                            ORB.ELO_Resultaat = 0;
                            break;
                        case 11:
                            ORB.Competitie_Resultaat = Convert.ToSingle(row["Zwart_Verlies"]);
                            ORB.ELO_Resultaat = 0;
                            break;
                        default:
                            ORB.Competitie_Resultaat = 0;
                            ORB.ELO_Resultaat = 0;
                            break;
                    }
                    ORB.ELO_Resultaat = 0 - ORW.ELO_Resultaat;
                }
                else
                {
                    ORW.Kleur = 0;
                    ORB.Kleur = 0;
                }
                ORW.Was_Downfloat = 0;
                ORW.Was_Upfloat = 0;

                AddOneResult(ORW);
                if (PID_Black > 0)
                {
                    ORB.Was_Downfloat = 0;
                    ORB.Was_Upfloat = 0;
                    AddOneResult(ORB);
                }
            }
        }

        private bool AddOneResult(ResultData OneResult)
        {
            //
            // This module adds one game to the table "Competitie_Resultaten"
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spSaveOneResult", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = OneResult.Rondernr;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = OneResult.Deelnemer_ID;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = OneResult.Competitie_Id;
                    cmd.Parameters.Add("@PRL", SqlDbType.Int).Value = OneResult.Plaats_Op_Ranglijst;
                    cmd.Parameters.Add("@TID", SqlDbType.Int).Value = OneResult.Tegenstander;
                    cmd.Parameters.Add("@COL", SqlDbType.Int).Value = OneResult.Kleur;
                    cmd.Parameters.Add("@RES", SqlDbType.Int).Value = OneResult.Resultaat;
                    cmd.Parameters.Add("@ERES", SqlDbType.Float).Value = OneResult.ELO_Resultaat;
                    cmd.Parameters.Add("@CRES", SqlDbType.Float).Value = OneResult.Competitie_Resultaat;
                    cmd.Parameters.Add("@COE", SqlDbType.Float).Value = OneResult.CorrectieOpElo;
                    cmd.Parameters.Add("@COC", SqlDbType.Float).Value = OneResult.CorrectieOpCompetitie;
                    cmd.Parameters.Add("@PAN", SqlDbType.Text).Value = OneResult.Partijnaam;
                    cmd.Parameters.Add("@CGN", SqlDbType.Int).Value = OneResult.ChampionsgroupGameNumber;
                    cmd.Parameters.Add("@WDF", SqlDbType.Int).Value = OneResult.Was_Downfloat;
                    cmd.Parameters.Add("@WUF", SqlDbType.Int).Value = OneResult.Was_Upfloat;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", OneResult.Competitie_Id, "SaveOneResult", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }

        public bool UpdateOneResult(ResultData OneResult)
        {
            //
            // This module Updates one game to the table "Competitie_Resultaten"
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spUpdateOneResult", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = OneResult.Rondernr;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = OneResult.Deelnemer_ID;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = OneResult.Competitie_Id;
                    cmd.Parameters.Add("@COL", SqlDbType.Int).Value = OneResult.Kleur;
                    cmd.Parameters.Add("@RES", SqlDbType.Int).Value = OneResult.Resultaat;
                    cmd.Parameters.Add("@ERES", SqlDbType.Float).Value = OneResult.ELO_Resultaat;
                    cmd.Parameters.Add("@CRES", SqlDbType.Float).Value = OneResult.Competitie_Resultaat;
                    cmd.Parameters.Add("@CGGN", SqlDbType.Int).Value = OneResult.ChampionsgroupGameNumber;
                    cmd.Parameters.Add("@ADV", SqlDbType.Int).Value = OneResult.Tegenstander;
                    cmd.Parameters.Add("@MP", SqlDbType.Float).Value = OneResult.Matchpunten;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", OneResult.Competitie_Id, "UpdateOneResult", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }
        public bool DeleteResults(int CompetitieID, int RondeNummer)
        {
            //
            // Clean up table "Competitie Resultaten" before new cycle starts
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spDeleteResults", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CompetitieID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RondeNummer;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CompetitieID, "DeleteResults", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // Return value with error status. At the moment, the calling routine does not handle any exception status because 
                // there is no correct way to continue when the exception occurs.
                //
                return error_occurred;
            }
        }

        private bool AddNewRatingInfoAfterCompetitionRound(int CID, int RNR, int PID)
        {
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spCalculateAndSaveOneRating", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "CalculateAndSaveOneRating", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }

        public void AdministrationRatingData(int CID, int RNR)
        {

            DataSet ds = GetResultsPlayerIDData(CID, RNR);
            ResultData ORW = new ResultData();
            ResultData ORB = new ResultData();
            int PID;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                PID = (int)row["Deelnemer_Id"];
                AddNewRatingInfoAfterCompetitionRound(CID, RNR, PID);
            }

        }
        public bool PlayerOutOfCompetition(int CID, int RNR, int PID)
        {
            bool aux = false;
            int Count_Absentees = 0;
            aux = Count_Absentees > 5;
            return aux;
        }

        public DataSet GetCompetitionRankingList(int CID, int RNR)
        {
            //
            // This function returns a list of players sorted on Competition points
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetCompetitionRanking";
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                da.SelectCommand = cmd;
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetCompetitionRanking", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }


        public DataSet GetCompetitionGainRankingList(int CID, int RNR)
        {
            //
            // This function returns a list of players sorted on Competition Gain/Loss numbers
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetCompetitionGainRanking";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetCompetitionGainRanking", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public DataSet GetELORankingList(int CID, int RNR)
        {
            //
            // This function returns a list of players sorted on Competition Gain/Loss numbers
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetELORanking";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetELORanking", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public int ButtonAllowed(string FunctionDescription, int Status, int Privileges)
        {
            //
            // This function returns the state of validity of the function at this state
            //
            int LR_Value = 0;

            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetFunctionStatus";
                    cmd.Parameters.Add("@Func", SqlDbType.Text).Value = FunctionDescription;
                    cmd.Parameters.Add("@Stat", SqlDbType.Int).Value = Status;
                    cmd.Parameters.Add("@Priv", SqlDbType.Int).Value = Privileges;
                    SqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            LR_Value = Convert.ToInt16(reader["New_State"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        //
                        // If no color was found, the last color is 0
                        LR_Value = 0;
                        string lit_Error = ex.Message;
                        WriteLogLine("C#", 0, "GetFunctionStatus", "Error", 1, "First catch" + lit_Error);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetFunctionStatus", "Error", 1, "Second catch" + lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }

            //
            // Return value is a dataset
            //
            return LR_Value;
        }

        public void SetIntInAlgemeneInfo(string Item_Name, int CID, int Item_Value)
        {
            //
            // This function updates a value in a Algemene Info record  
            //
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spSetGeneralInfoAttribute", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@AttributeName", SqlDbType.NVarChar).Value = Item_Name;
                    cmd.Parameters.Add("@AttributeValue", SqlDbType.Int).Value = Item_Value;
                    cmd.Parameters.Add("@CID", SqlDbType.NVarChar).Value = CID;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "SetGeneralInfoAttribute", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
            }
        }

        public void RemovePlayerStatusCalculated(int CID, int RNR)
        {
            //
            // This function removes Status information of a player after a specific action: Go to next round and roll-back current round 
            //
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spRemovePlayerStatusCalculated", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "spRemovePlayerStatusCalculated", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
            }
        }
        public int GetIntFromAlgemeneInfo(int CID, string AttributeName)
        {
            //
            // Get value of an attribute of the general info record belonging to this competition
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetGeneralInfoAttribute", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@AttributeName", SqlDbType.NVarChar).Value = AttributeName;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = (int)(reader["AttributeValue"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetGeneralInfoAttribute", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // return value is an integer
            //
            return aux;
        }

        public DataSet GetPlayerListAlphabetical(int CompetitionId)
        {
            //
            // This function gets all names (combined into readable names) of the players of a specific competition plus some additional player data for use in
            // multiple situations
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetPlayersAlphabetical";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CompetitionId;

                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CompetitionId, "GetPlayersAlphabetical", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public bool GetPlayerListAlphabeticalOverviewInit(int CompetitionId)
        {
            bool error_occurred = false;
            //
            // This function gets all names (combined into readable names) of the players of a specific competition plus some additional player data for use in
            // multiple situations
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetPlayersAlphabeticalOverviewInit";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CompetitionId;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetPlayersAlphabeticalOverviewInit", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
            }
            return error_occurred;

        }


        public DataSet GetPlayerListAlphabeticalOverview(int CompetitionId)
        {
            //
            // This function gets all names (combined into readable names) of the players of a specific competition plus some additional player data for use in
            // multiple situations
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetPlayersAlphabeticalOverview";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CompetitionId;

                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CompetitionId, "GetPlayersAlphabeticalOverview", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public bool DeletePlayerListAlphabeticalOverview(int CompetitionId)
        {
            bool error_occurred = false;
            //
            // This function removes all names from the temporary upload file
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spRemoveAlphabeticalOverview";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CompetitionId;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "DeletePlayersAlphabeticalOverview", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
            }
            return error_occurred;
        }

        public bool UpdatePlayerListAlphabeticalOverview(int CompetitionId, int PID, String Mode, int YesNo)
        {
            bool error_occurred = false;
            //
            // This function updates the created and uploaded flags
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdatePlayersAlphabeticalOverview";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CompetitionId;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@MOD", SqlDbType.Text).Value = Mode;
                    cmd.Parameters.Add("@OOZ", SqlDbType.Int).Value = YesNo;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "UpdatePlayersAlphabeticalOverview", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
            }
            return error_occurred;
        }

        public DataSet GetResults(int PID, int CID, int TC)
        {
            //
            // This function gets all data of the rounds of player PID
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetResults";
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@TC", SqlDbType.Int).Value = TC;

                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetResults", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public GeneralInfo GetGeneralInfo(int CID)
        {
            //
            // This function returns an instance of a class of all data related to a single player. Sources are multiple database tables
            //
            GeneralInfo gi = new GeneralInfo();
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    //
                    // Get algemene info data
                    //
                    SqlCommand cmd = new SqlCommand("spGetAlgemeneInfoRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    SqlDataReader readerGI = cmd.ExecuteReader();

                    while (readerGI.Read())
                    {
                        gi.Vereniging = Convert.ToString(readerGI["Vereniging"]);
                        gi.Naam_competitie = Convert.ToString(readerGI["Naam_competitie"]);
                        gi.Aanmaakdatum = Convert.ToDateTime(readerGI["Aanmaakdatum"]);
                        gi.Aantal_groepen = Convert.ToInt32(readerGI["Aantal_groepen"]);
                        gi.Bonus_externe_wedstrijden = Convert.ToInt32(readerGI["Bonus_externe_wedstrijden"]);
                        gi.Vrij_afmelden = Convert.ToInt32(readerGI["Vrij_afmelden"]);
                        gi.Strafpunten_afmelden = Convert.ToInt32(readerGI["Strafpunten_afmelden"]);
                        gi.Strafpunten_wegblijven = Convert.ToInt32(readerGI["Strafpunten_wegblijven"]);
                        gi.Aantal_ronden = Convert.ToInt32(readerGI["Aantal_ronden"]);
                        gi.Laatste_ronde = Convert.ToInt16(readerGI["Laatste_Ronde"]);
                        gi.KFactor = Convert.ToInt32(readerGI["KFactor"]);
                        gi.Standaarddeviatie = Convert.ToInt32(readerGI["Standaarddeviatie"]);
                        gi.Aantal_Unieke_Ronden = Convert.ToInt32(readerGI["Aantal_Unieke_Ronden"]);
                        gi.Intern_Basis = Convert.ToString(readerGI["Intern_Basis"]);
                        gi.Intern_Template = Convert.ToString(readerGI["Intern_Template"]);
                        gi.Intern_Images = Convert.ToString(readerGI["Intern_Images"]);
                        gi.Intern_Competitie = Convert.ToString(readerGI["Intern_Competitie"]);
                        gi.Intern_Competitie_Images = Convert.ToString(readerGI["Intern_Competitie_Images"]);
                        gi.Website_Basis = Convert.ToString(readerGI["Website_Basis"]);
                        gi.Website_Template = Convert.ToString(readerGI["Website_Template"]);
                        gi.Website_Competitie = Convert.ToString(readerGI["Website_Competitie"]);
                        gi.Website_Competitie_Images = Convert.ToString(readerGI["Website_Competitie_Images"]);
                        gi.Client_FTP_Host = Convert.ToString(readerGI["Client_FTP_Host"]);
                        gi.Client_FTP_UN = Convert.ToString(readerGI["Client_FTP_UN"]);
                        gi.Client_FTP_PW = Convert.ToString(readerGI["Client_FTP_PW"]);
                        gi.Competitie_Type = Convert.ToInt32(readerGI["Competitie_Type"]);
                        gi.CurrentState = Convert.ToInt32(readerGI["CurrentState"]);
                        gi.IntegrateWith = GetIntegrateWithCompetition(CID);
                        gi.PartijenPerRonde = Convert.ToInt32(readerGI["PartijenPerRonde"]);
                        gi.Seizoen = Convert.ToInt32(readerGI["Seizoen"]);
                        gi.Acceleration = Convert.ToSingle(readerGI["Acceleration"]);
                        if (DBNull.Value != readerGI["NoPicture"])
                        {
                            gi.ProfilePicture = (byte[])readerGI["NoPicture"];
                        }

                    }
                    readerGI.Close();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetAlgemeneInfoRecord", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
            //
            // This function returns an instance of a class of  player data
            //
            return gi;
        }

        public int UpdateGeneralInfo(GeneralInfo gi, int ManagerId)
        {
            //
            // This module Updates one game to the table "Competitie_Resultaten"
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spUpdateAlgemeneInfoRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = gi.Competitie_Id;
                    cmd.Parameters.Add("@VER", SqlDbType.Text).Value = gi.Vereniging;
                    cmd.Parameters.Add("@NCO", SqlDbType.Text).Value = gi.Naam_competitie;
                    cmd.Parameters.Add("@NOG", SqlDbType.Int).Value = gi.Aantal_groepen;
                    cmd.Parameters.Add("@BEX", SqlDbType.Int).Value = gi.Bonus_externe_wedstrijden;
                    cmd.Parameters.Add("@VAF", SqlDbType.Int).Value = gi.Vrij_afmelden;
                    cmd.Parameters.Add("@PAF", SqlDbType.Int).Value = gi.Strafpunten_afmelden;
                    cmd.Parameters.Add("@PAWOL", SqlDbType.Int).Value = gi.Strafpunten_wegblijven;
                    cmd.Parameters.Add("@MRO", SqlDbType.Int).Value = gi.Aantal_ronden;
                    cmd.Parameters.Add("@CRO", SqlDbType.Int).Value = gi.Laatste_ronde;
                    cmd.Parameters.Add("@KFAC", SqlDbType.Int).Value = gi.KFactor;
                    cmd.Parameters.Add("@STD", SqlDbType.Int).Value = gi.Standaarddeviatie;
                    cmd.Parameters.Add("@NUR", SqlDbType.Int).Value = gi.Aantal_Unieke_Ronden;
                    cmd.Parameters.Add("@CIB", SqlDbType.Text).Value = gi.Intern_Basis;
                    cmd.Parameters.Add("@CIT", SqlDbType.Text).Value = gi.Intern_Template;
                    cmd.Parameters.Add("@CII", SqlDbType.Text).Value = gi.Intern_Images;
                    cmd.Parameters.Add("@CIC", SqlDbType.Text).Value = gi.Intern_Competitie;
                    cmd.Parameters.Add("@CICI", SqlDbType.Text).Value = gi.Intern_Competitie_Images;
                    cmd.Parameters.Add("@CEB", SqlDbType.Text).Value = gi.Website_Basis;
                    cmd.Parameters.Add("@CET", SqlDbType.Text).Value = gi.Website_Template;
                    cmd.Parameters.Add("@CEC", SqlDbType.Text).Value = gi.Website_Competitie;
                    cmd.Parameters.Add("@CECI", SqlDbType.Text).Value = gi.Website_Competitie_Images;
                    cmd.Parameters.Add("@CCFH", SqlDbType.Text).Value = gi.Client_FTP_Host;
                    cmd.Parameters.Add("@CCFU", SqlDbType.Text).Value = gi.Client_FTP_UN;
                    cmd.Parameters.Add("@CCFP", SqlDbType.Text).Value = gi.Client_FTP_PW;
                    cmd.Parameters.Add("@CTY", SqlDbType.Int).Value = gi.Competitie_Type;
                    cmd.Parameters.Add("@CST", SqlDbType.Int).Value = gi.CurrentState;
                    cmd.Parameters.Add("@INT", SqlDbType.Int).Value = gi.IntegrateWith;
                    cmd.Parameters.Add("@MID", SqlDbType.Int).Value = ManagerId;
                    cmd.Parameters.Add("@GPR", SqlDbType.Int).Value = gi.PartijenPerRonde;
                    cmd.Parameters.Add("@CIDNEW", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@NoPicture", SqlDbType.Image).Value = gi.ProfilePicture;
                    cmd.Parameters.Add("@ACC", SqlDbType.Float).Value = gi.Acceleration;
                    cmd.Parameters.Add("@SEI", SqlDbType.Int).Value = gi.Seizoen;


                    int LR_Value = (int)cmd.ExecuteNonQuery();
                    aux = Convert.ToInt32(cmd.Parameters["@CIDNEW"].Value);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", gi.Competitie_Id, "UpdateAlgemeneInfoRecord", "Error", 1,  lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                int nr_groups = gi.Aantal_groepen;
                if (nr_groups > 1)
                {
                    DataSet ds_Participants = GetPlayerListCompetitionOnly(gi.Competitie_Id);
                    if (ds_Participants.Tables.Count != 0)
                    {
                        if (ds_Participants.Tables[0].Rows.Count != 0)
                        {
                            int per_group = ds_Participants.Tables[0].Rows.Count / nr_groups;
                            int surplus = ds_Participants.Tables[0].Rows.Count - per_group * nr_groups;
                            int groupsize = 0;
                            float lowerboundary = 0;
                            float upperboundary = 0;
                            for (int i=0; i<nr_groups; i++)
                            {
                                if (i+1 > surplus)
                                {
                                    groupsize = groupsize + per_group;
                                }
                                else
                                {
                                    groupsize = groupsize + per_group + 1;
                                }
                                if (i + 1 == nr_groups)
                                {
                                    upperboundary = 3000;
                                }
                                else
                                {
                                    upperboundary = (Convert.ToSingle(ds_Participants.Tables[0].Rows[groupsize][1]) + Convert.ToSingle(ds_Participants.Tables[0].Rows[groupsize + 1][1])) / 2;
                                }

                                AddCompetitionGroupBoundaries(gi.Competitie_Id, nr_groups-i, upperboundary, lowerboundary);
                                lowerboundary = upperboundary;
                            }
                        }
                    }
                }

            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return aux;
        }

        private bool AddCompetitionGroupBoundaries(int CID, int GroupNr, float Upper, float Lower)
        {
            //
            // This module adds one game to the table "Wedstrijden"
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spSaveOneGroupBoundaryRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@GNR", SqlDbType.Int).Value = GroupNr;
                    cmd.Parameters.Add("@UPP", SqlDbType.Float).Value = Upper;
                    cmd.Parameters.Add("@LOW", SqlDbType.Float).Value = Lower;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "AddCompetitionGroupBoundaries", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }

        public bool AddPlayerCompetitionRecord(int CID, int PID, float CPS, float ELO, int COT)
        {
            //
            // This module adds one game to the table "Wedstrijden"
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spSaveOneDeelnemerCompetitionRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@CPS", SqlDbType.Float).Value = CPS;
                    cmd.Parameters.Add("@ELO", SqlDbType.Float).Value = ELO;
                    cmd.Parameters.Add("@COT", SqlDbType.Float).Value = COT;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "SaveOneDeelnemerCompetitionRecord", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }

        public bool DeleteDeelnemerCompetitieRecords(int CID, int CIDOld)
        {
            //
            // Clean up table "Competitie Resultaten" before new cycle starts
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spRemoveAllfromDeelnemerCompetition", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@CIDOld", SqlDbType.Int).Value = CIDOld;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "RemoveAllfromDeelnemerCompetition", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // Return value with error status. At the moment, the calling routine does not handle any exception status because 
                // there is no correct way to continue when the exception occurs.
                //
                return error_occurred;
            }
        }

        public bool UpdatePlayerTeam(int PID, int Team, string Association_ID)
        {
            //
            // This function executes an update of player data in the different database tables
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spUpdatePlayerTeam", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@Team", SqlDbType.SmallInt).Value = Team;
                    cmd.Parameters.Add("@VID", SqlDbType.Text).Value = Association_ID;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "UpdatePlayerTeam", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
            //
            return error_occurred;
        }

        public DataSet GetChampionsgroupPlayerList(int CID)
        {
            //
            // This function gets all names (combined into readable names) of the players of a specific competition plus some additional player data for use in
            // multiple situations
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetChampionsGroupParticipants";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetChampionsGroupPar", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public bool UpdateChampionsGroupPlayer(ChampionsgroupData OnePlayer)
        {
            //
            // This module Updates one game to the table "Competitie_Resultaten"
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spUpdateOneChampionsgroupPlayer", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = OnePlayer.Player_Id;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = OnePlayer.Competition_Id;
                    cmd.Parameters.Add("@LOT", SqlDbType.Int).Value = OnePlayer.LotNumber;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", OnePlayer.Competition_Id, "UpdateOneChampionsgroupPlayer", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }

        public bool AddPlayerRoundRobinRecord(int CID, int RNR, int GNR, int PID, int ADV, int COL)
        {
            //
            // This module adds one game to the table "Round Robin"
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spSaveOneRoundRobinRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    cmd.Parameters.Add("@GNR", SqlDbType.Int).Value = GNR;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@ADV", SqlDbType.Int).Value = ADV;
                    cmd.Parameters.Add("@COL", SqlDbType.Int).Value = COL;
                    cmd.Parameters.Add("@CTK", SqlDbType.Int).Value = COL; // Crosstable X
                    cmd.Parameters.Add("@CTR", SqlDbType.Int).Value = COL; // Crosstable Y
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "SaveOneRoundRobinRecord", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }

        public bool Remove_RoundRobin(int CID)
        {
            //
            // This function adds a new record to the workflow table
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spRemoveRoundRobin", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "RemoveRoundRobinRecord", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        public float ResultCodeToMatchPoints(int ResultCode)
        {
            float aux = (float)0.0;

            switch (ResultCode)
            {
                case 1:
                    aux = (float)1.0;
                    break;
                case 9:
                    aux = (float)1.0;
                    break;
                case 13:
                    aux = (float)1.0;
                    break;
                case 2:
                    aux = (float)0.5;
                    break;
                case 10:
                    aux = (float)0.5;
                    break;
            }

            return aux;
        }

        public void Upgrade_ChampionsgroupPoints(int CID)
        {

            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetChampionsgroupResults";
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                da.SelectCommand = cmd;
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetChampionsgroupResults", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            if (ds != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int Player_Id = (int)ds.Tables[0].Rows[i]["Deelnemer_Id"];
                    int ResultCode = (int)ds.Tables[0].Rows[i]["Resultaat"];
                    int GameNumber = (int)ds.Tables[0].Rows[i]["KroongroepPartijNummer"];

                    float MP = ResultCodeToMatchPoints(ResultCode);

                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        try
                        {
                            SqlCommand cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "spUpdateRoundRobinResult";
                            cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                            cmd.Parameters.Add("@PID", SqlDbType.Int).Value = Player_Id;
                            cmd.Parameters.Add("@CGN", SqlDbType.Int).Value = GameNumber;
                            cmd.Parameters.Add("@MPT", SqlDbType.Float).Value = MP;
                            int LR_Value = (int)cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            string lit_Error = ex.Message;
                            WriteLogLine("C#", CID, "UpdateRoundRobinResult", "Error", 1, lit_Error);
                        }
                        finally
                        {
                            con.Close();
                            con.Dispose();
                        }
                    }
                }
            }
        }

        public DataSet GetRoundRobinData(int CID)
        {
            //
            // This function gets all data of the rounds of competition CID
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetRoundRobinData";
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                da.SelectCommand = cmd;
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetRoundRobinData", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public float GetChampionsgroupMatchpoint(int PID, int ADV, int CID)
        {
            //
            // Get the number of players within the current competition at the start of the competition 
            // withdrawn themselves.
            // The information comes from the database table "Deelnemers" which registers all players
            //
            float Aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetRoundRobinMPts", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                cmd.Parameters.Add("@ADV", SqlDbType.Int).Value = ADV;

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Aux = Convert.ToSingle(reader["Matchpoints"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetRoundRobinMPts", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is an real number
            //
            return Aux;
        }

        public DataSet GetAllOtherPlayersAlphabetical(int CID)
        {
            //
            // This function gets all data of the rounds of competition CID
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetAllPlayersAlphabetical";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    da.SelectCommand = cmd;
                    da.Fill(ds);

                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetAllPlayersAlphabetical", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public void Remove_Player_From_CompetitionRating(int PID, int CID)
        {
            //
            // remove a player from the deelnemer_competition and a competition
            //
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spRemoveOnePlayerFromCompetitionRating", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "RemoveOnePlayerFromCom", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public float GetCompetitionPoints(int PID)
        {
            float CompetitionPoints = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetCompetitiePunten", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PlayerId", SqlDbType.Int).Value = PID;

                    SqlDataReader readerValue = cmd.ExecuteReader();
                    while (readerValue.Read())
                    {
                        CompetitionPoints = Convert.ToSingle(readerValue["RatingPoints"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetCompetitiePunten", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                return CompetitionPoints;
            }
        }

        public float GetClubRating(int PID)
        {
            float ClubRating = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetClubRating", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                    SqlDataReader readerValue = cmd.ExecuteReader();
                    while (readerValue.Read())
                    {
                        ClubRating = Convert.ToSingle(readerValue["RatingPoints"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetClubRating", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                return ClubRating;
            }
        }

        public float GetKNSBRating(int PID)
        {
            float KNSBRating = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetKNSBrating", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                    SqlDataReader readerValue = cmd.ExecuteReader();
                    while (readerValue.Read())
                    {
                        KNSBRating = Convert.ToSingle(readerValue["RatingPoints"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetKNSBrating", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return KNSBRating;
            }
        }

        public float GetFIDERating(int PID)
        {
            float FIDERating = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetFIDErating", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                    SqlDataReader readerValue = cmd.ExecuteReader();
                    while (readerValue.Read())
                    {
                        FIDERating = Convert.ToSingle(readerValue["RatingPoints"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetGetFIDErating", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                return FIDERating;
            }
        }

        public void WriteLogLine(string User, int CID, string Module, string Level, int LevelValue, string LogLine)
        {
            //
            // This function updates a value in a Algemene Info record  
            //
            int MaxLogLevel = Convert.ToInt16(GetConfigValue("MaxLogLevel"));
            if (LevelValue <= MaxLogLevel)
            {
                string cs = Connection_String_CS();
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    try
                    {
                        SqlCommand cmd = new SqlCommand("spSaveLogRecord", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@User", SqlDbType.VarChar).Value = User + "-";
                        cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                        cmd.Parameters.Add("@Module", SqlDbType.VarChar).Value = Module;
                        cmd.Parameters.Add("@Level", SqlDbType.VarChar).Value = Level;
                        cmd.Parameters.Add("@LogLine", SqlDbType.VarChar).Value = LogLine;
                        cmd.Parameters.Add("@LVLVAL", SqlDbType.Int).Value = LevelValue;

                        int LR_Value = (int)cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string lit_Error = ex.Message;
                        WriteLogLine("C#", CID, "SaveLogRecord", "Error", 1, lit_Error);
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }
                    //
                    // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                    //
                }
            }
        }

        public void WriteLogExceptionTrace(string User, int CID, string Module, string Level, int LevelValue, string TraceLog)
        {
            //
            // This function updates a value in a Algemene Info record  
            //
            int MaxLogLevel = Convert.ToInt16(GetConfigValue("MaxLogLevel"));
            if (LevelValue <= MaxLogLevel)
            {
                string cs = Connection_String_CS();
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    try
                    {
                        SqlCommand cmd = new SqlCommand("spSaveTraceRecord", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@User", SqlDbType.VarChar).Value = User + "-";
                        cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                        cmd.Parameters.Add("@Module", SqlDbType.VarChar).Value = Module;
                        cmd.Parameters.Add("@Level", SqlDbType.VarChar).Value = Level;
                        cmd.Parameters.Add("@TraceLog", SqlDbType.VarChar).Value = TraceLog;
                        cmd.Parameters.Add("@LVLVAL", SqlDbType.Int).Value = LevelValue;

                        int LR_Value = (int)cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string lit_Error = ex.Message;
                        WriteLogLine("C#", CID, "SaveLogRecord", "Error", 1, lit_Error);
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }
                    //
                    // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                    //
                }
            }
        }


        public bool PlayerHasResults(int CID, int PID)
        {
            //
            // Get number of the last ound of a specific competition
            //
            bool aux = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spPlayerHasResults", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                cmd.Parameters.Add("@RES", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                    aux = (Convert.ToInt32(cmd.Parameters["@RES"].Value) == 0);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "PlayerHasResults", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // return value is a boolean
            //
            return aux;
        }

        public bool DeletePlayerCompetitionRecord(int CID, int PID)
        {
            //
            // Clean up table "Competitie Resultaten" before new cycle starts
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spRemoveOnefromDeelnemerCompetition", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "RemoveOnefromDeeln", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // Return value with error status. At the moment, the calling routine does not handle any exception status because 
                // there is no correct way to continue when the exception occurs.
                //
                return error_occurred;
            }
        }

        public bool Remove_BlitzResults(int CID, int RNR)
        {
            //
            // This function adds a new record to the workflow table
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spRemoveBlitzResults", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "RemoveBlitzResults", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        public bool AddPlayerBlitzResultInit(int CID, int RNR, int PID)
        {
            //
            // This module adds one game to the table "Round Robin"
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spSaveOneBlitzResultRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "SaveOneBlitzResultRecord", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }

        public bool UpdateOneBlitzResult(BlitzResultData OneBlitzResult)
        {
            //
            // This module Updates one game to the table "Competitie_Resultaten"
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spUpdateOneBlitzResult", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = OneBlitzResult.Rondernr;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = OneBlitzResult.Deelnemer_ID;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = OneBlitzResult.Competitie_Id;
                    cmd.Parameters.Add("@SPT", SqlDbType.Float).Value = OneBlitzResult.Strafpunten;
                    cmd.Parameters.Add("@MPT", SqlDbType.Float).Value = OneBlitzResult.Matchpunten;
                    cmd.Parameters.Add("@GNR", SqlDbType.Int).Value = OneBlitzResult.Groepnummer;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", OneBlitzResult.Competitie_Id, "UpdateOneBlitzResult", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }

        public DataSet GetPlayersBlitz(int CID, int RNR)
        {
            //
            // This function gets all data of the rounds of competition CID
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetPlayersBlitz";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                    da.SelectCommand = cmd;
                    da.Fill(ds);

                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetPlayersBlitz", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public float GetClubBlitzRating(int PID)
        {
            float ClubRating = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetClubBlitzrating", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                    SqlDataReader readerValue = cmd.ExecuteReader();
                    while (readerValue.Read())
                    {
                        ClubRating = Convert.ToSingle(readerValue["RatingPoints"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetClubBlitzrating", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                return ClubRating;
            }
        }

        public bool SaveBlitzRating(int PID, float Rating, int CID, int RNR)
        {
            //
            // This module adds one game to the table "Wedstrijden"
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spSaveBlitzRating", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    cmd.Parameters.Add("@Rating", SqlDbType.Float).Value = Rating;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "SaveBlitzRating", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }

        public bool Remove_RatingRound(int CID, int RNR)
        {
            //
            // This function adds a new record to the workflow table
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spRemoveRatingRound", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "RemoveRatingRound", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        public DataSet GetBlitzRatingRanking()
        {
            //
            // This function gets all data of the rounds of competition CID
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetBlitzRatingRanking";

                    da.SelectCommand = cmd;
                    da.Fill(ds);

                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "RemoveRatingRound", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public DataSet GetPlayersUniqueBlitz(int CID)
        {
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetPlayersUniqueBlitz", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    da.SelectCommand = cmd;
                    da.Fill(ds);

                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetPlayersUniqueBlitz", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public float GetBlitzPenaltyPoints(int CID, int PID)
        {
            float PenaltyPoints = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetBlitzPenaltyPoints", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;


                    SqlDataReader readerValue = cmd.ExecuteReader();
                    while (readerValue.Read())
                    {
                        PenaltyPoints = Convert.ToSingle(readerValue["TotalPenaltyPoints"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetBlitzPenaltyPoints", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                return PenaltyPoints;
            }
        }

        public float GetBlitzPenaltyPointsCleaned(int CID, int PID)
        {
            float PenaltyPoints = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetBlitzPenaltyPointsCleaned", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    SqlDataReader readerValue = cmd.ExecuteReader();
                    while (readerValue.Read())
                    {
                        PenaltyPoints = Convert.ToSingle(readerValue["TotalRelevantPoints"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetBlitzPenaltyPoints", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return PenaltyPoints;
            }
        }

        public DataSet GetBlitzDisplayPoints(int CID, int PID)
        {
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetBlitzDisplayPoints", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                    da.SelectCommand = cmd;
                    da.Fill(ds);

                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetBlitzDisplayPoints", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public string MakeImageSourceData(byte[] bytes)
        {
            string base64String = "data:image/png;base64," + Convert.ToBase64String(bytes, 0, bytes.Length);

            return base64String;
        }

        public byte[] GetPlayerProfilePictureFile(int PID)
        {
            //
            // Get a bytestring of the current profile picture
            //
            byte[] aux;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmdFindName = new SqlCommand("spGetPlayerProfilePicture", con);
                cmdFindName.CommandType = CommandType.StoredProcedure;
                cmdFindName.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                con.Open();

                SqlDataReader readPlayerName = cmdFindName.ExecuteReader();
                //
                // format of the name is: Roepnaam
                //
                try
                {
                    readPlayerName.Read();
                    aux = readPlayerName.GetValue(0) as Byte[];
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetPlayerProfilePicture", "Error", 1, lit_Error);
                    aux = null;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Result is a binary representation of a picture
            //
            return aux;
        }

        public byte[] GetNoPictureFile(int CID)
        {
            //
            // Get a readable name of a specific player
            //
            byte[] aux;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmdFindName = new SqlCommand("spGetNoProfilePicture", con);
                cmdFindName.CommandType = CommandType.StoredProcedure;
                cmdFindName.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                con.Open();

                SqlDataReader readFile = cmdFindName.ExecuteReader();
                try
                {
                    readFile.Read();
                    aux = readFile.GetValue(0) as Byte[];
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetNoProfilePicture", "Error", 1, lit_Error);
                    aux = null;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Result is a string with the combined name
            //
            return aux;
        }

        public string StringImage(int PID, int CID)
        {
            string aux;

            byte[] file = GetPlayerProfilePictureFile(PID);
            if (file == null)
            {
                file = GetNoPictureFile(CID);
            }
            aux = MakeImageSourceData((byte[])file);
            return aux;
        }

        public DataSet GetTeamData(string Association)
        {
            //
            // This function gets URLs fro the teams of a specific chess association
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetTeamData";
                cmd.Parameters.Add("@VER", SqlDbType.Text).Value = Association;

                try
                {
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetTeamData", "Error", 1, lit_Error);
                    ds = null;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }
        public DataSet GetTeamPlayers(string Association)
        {
            //
            // This function gets URLs fro the teams of a specific chess association
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetTeamPlayers";
                cmd.Parameters.Add("@VID", SqlDbType.Text).Value = Association.Trim();

                try
                {
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetTeamPlayers", "Error", 1, lit_Error);
                    ds = null;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public bool RemoveTeam(String VER, int Team_Record_Nummer)
        {
            //
            // This function adds a new record to the workflow table
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spRemoveOneTeam", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@VER", SqlDbType.NVarChar).Value = VER;
                    cmd.Parameters.Add("@TRN", SqlDbType.Int).Value = Team_Record_Nummer;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", 0, "RemoveTeam", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        public bool UpdateTeam(string VER, int Team_nr, string URL, string Teamnaam, int Team_Record_Nummer)
        {
            //
            // This function updates any value in a workflow record  in the Workflow Table
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spUpdateTeamData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@VER", SqlDbType.NVarChar).Value = VER;
                    cmd.Parameters.Add("@TNR", SqlDbType.Int).Value = Team_nr;
                    cmd.Parameters.Add("@TRN", SqlDbType.Int).Value = Team_Record_Nummer;
                    cmd.Parameters.Add("@TeamNaam", SqlDbType.NVarChar).Value = Teamnaam;
                    cmd.Parameters.Add("@URL", SqlDbType.NVarChar).Value = URL;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", 0, "spUpdateTeamData", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }
        public bool AddTeam(string VER, string URL, string Teamnaam, int Team_nr)
        {
            //
            // This function updates any value in a workflow record  in the Workflow Table
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spSaveOneTeam", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@VER", SqlDbType.NVarChar).Value = VER;
                    cmd.Parameters.Add("@TeamNaam", SqlDbType.NVarChar).Value = Teamnaam;
                    cmd.Parameters.Add("@URL", SqlDbType.NVarChar).Value = URL;
                    cmd.Parameters.Add("@TNR", SqlDbType.Int).Value = Team_nr;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", 0, "spSaveOneTeam", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        public int ValidateInteger(string Inputfield, bool emptyAllowed, int Default, bool Bounded, int Minimum, int Maximum, ref int InputValue)
        {
            int ErrorCode = 0;
            //
            // check empty allowed
            //
            if ((emptyAllowed == false) && (Inputfield == ""))
            {
                ErrorCode = 2;
                InputValue = Default;
                WriteLogLine("C#", 0, "Validate error", "Info", 4, "Field Required");
            }
            //
            // check syntax
            //
            if (ErrorCode == 0)
            {
                if (emptyAllowed && (Inputfield == ""))
                {
                    InputValue = Default;
                }
                else
                {
                    try
                    {
                        InputValue = Convert.ToInt32(Inputfield.Trim());
                    }
                    catch (Exception ex)
                    {
                        string lit_Error = ex.Message;
                        ErrorCode = 1;
                        InputValue = Default;
                        WriteLogLine("C#", 0, "Validate error", "Info", 4, lit_Error);
                    }
                }
            }
            //
            // check boundaries
            //
            if (ErrorCode == 0)
            {
                if (Bounded)
                {
                    if (InputValue < Minimum || InputValue > Maximum)
                    {
                        ErrorCode = 3;
                        InputValue = Default;
                        WriteLogLine("C#", 0, "Validate error", "Info", 4, "Field outside acceptable values");
                    }
                }
            }
            return ErrorCode;
        }

        public int ValidateReal(string Inputfield, bool emptyAllowed, double Default, bool Bounded, double Minimum, double Maximum, ref double InputValue)
        {
            int ErrorCode = 0;
            //
            // check empty allowed
            //
            if ((emptyAllowed == false) && (Inputfield == ""))
            {
                ErrorCode = 2;
                InputValue = Default;
                WriteLogLine("C#", 0, "Validate error", "Info", 4, "Field Required");
            }
            //
            // check syntax
            //
            if (ErrorCode == 0)
            {
                if (emptyAllowed && (Inputfield == ""))
                {
                    InputValue = Default;
                }
                else
                {
                    try
                    {
                        InputValue = Convert.ToDouble(Inputfield.Trim(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {
                        string lit_Error = ex.Message;
                        ErrorCode = 1;
                        InputValue = Default;
                        WriteLogLine("C#", 0, "Validate error", "Info", 4, lit_Error);
                    }
                }
            }
            //
            // check boundaries
            //
            if (ErrorCode == 0)
            {
                if (Bounded)
                {
                    if (InputValue < Minimum || InputValue > Maximum)
                    {
                        ErrorCode = 3;
                        InputValue = Default;
                        WriteLogLine("C#", 0, "Validate error", "Info", 4, "Field outside acceptable values");
                    }
                }
            }
            return ErrorCode;
        }

        public int ValidateString(string Inputfield, bool emptyAllowed, string Default, ref string InputValue)
        {
            int ErrorCode = 0;
            //
            // check empty allowed
            //
            InputValue = Inputfield;
            if ((emptyAllowed == false) && (Inputfield == ""))
            {
                ErrorCode = 2;
                InputValue = Default;
                WriteLogLine("C#", 0, "Validate error", "Info", 4, "Field Required");
            }
            return ErrorCode;
        }

        public bool Remove_Round_Information(int CID, int RNR)
        {
            //
            // This function adds a new record to the workflow table
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spRemoveLastRound", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "Remove_Last_Round", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        public int Get_Workflow_Item(string Item_Name, int CID, int RNR)
        {
            int Value = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetWorkflowItem", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ItemNaam", SqlDbType.VarChar).Value = Item_Name;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                    SqlDataReader readerValue = cmd.ExecuteReader();
                    while (readerValue.Read())
                    {
                        Value = Convert.ToInt16(readerValue["ItemValue"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 30, "GetWorkflowItem", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                return Value;
            }
        }

        public DataSet GetPlayersWithByeList(int CID)
        {
            //
            // This function results in a dataset with 50 last players that have gotten a Bye 
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetPlayersWithBye";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    da.SelectCommand = cmd;
                    con.Open();
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetPlayersWithBye", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return ds;
        }

        public DataSet GetRetentionStatistics(int CID, int RNR)
        {
            //
            // This function results in a dataset with 50 last players that have gotten a Bye 
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetRetentionStatistics";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    da.SelectCommand = cmd;
                    con.Open();
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetRetentionStatistics", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return ds;
        }

        public bool Update_National_Ratingtable(string Filename)
        {
            //
            // Get new national rating tables
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spRenewKNSBRatings", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Filename", SqlDbType.Text).Value = Filename;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", 0, "spRenewKNSBRatings", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // Return value with error status. At the moment, the calling routine does not handle any exception status because 
                // there is no correct way to continue when the exception occurs.
                //
                return error_occurred;
            }
        }

        public bool Update_FIDE_Ratingtable(string Filename)
        {
            //
            // Get new national rating tables
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spRenewFIDERatings", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Filename", SqlDbType.Text).Value = Filename;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", 0, "spRenewFIDERatings", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // Return value with error status. At the moment, the calling routine does not handle any exception status because 
                // there is no correct way to continue when the exception occurs.
                //
                return error_occurred;
            }
        }
        public DataSet GetUniquePlayers()
        {
            //
            // This function results in a dataset with 50 last players that have gotten a Bye 
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetUniquePlayers";
                    da.SelectCommand = cmd;
                    con.Open();
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetUniquePlayers", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return ds;
        }

        public bool Update_KNSB_and_FIDE_Ratings(int PID, int KNSB, int FIDE)
        {
            //
            // Get new national rating tables
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            if (KNSB > 0)
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    try
                    {
                        error_occurred = false;
                        SqlCommand cmd = new SqlCommand("spUpdateKNSBRating", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PID", SqlDbType.Int).Value = @PID;
                        cmd.Parameters.Add("@KNSB", SqlDbType.Int).Value = @KNSB;
                        int LR_Value = (int)cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string lit_Error = ex.Message;
                        error_occurred = true;
                        WriteLogLine("C#", 0, "spUpdateKNSBRating", "Error", 1, lit_Error);
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }

                }
            }
            if (FIDE > 0)
            {

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    try
                    {
                        error_occurred = false;
                        SqlCommand cmd = new SqlCommand("spUpdateFIDERating", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PID", SqlDbType.Int).Value = @PID;
                        cmd.Parameters.Add("@FIDE", SqlDbType.Int).Value = @FIDE;

                        int LR_Value = (int)cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string lit_Error = ex.Message;
                        error_occurred = true;
                        WriteLogLine("C#", 0, "spUpdateFIDERating", "Error", 1, lit_Error);
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }
                    //
                    // Return value with error status. At the moment, the calling routine does not handle any exception status because 
                    // there is no correct way to continue when the exception occurs.
                    //
                }
            }
            return error_occurred;
        }

        public bool PrepareBlitzRanking(int CID)
        {
            //
            // This function updates any value in a workflow record  in the Workflow Table
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spPrepareBlitzCompetitionRanking", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", 0, "spPrepareBlitzCompetitionRanking", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        public DataSet GetChampionsgroupSchedule(int CID)
        {
            //
            // This function gets all data of the rounds of competition CID
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetChampionsgroupSchedule";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    da.SelectCommand = cmd;
                    da.Fill(ds);

                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetChampionsgroupSchedule", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public int GetLastPlayerID()
        {
            //
            // This function gets the number of unique rounds within a competition from the tabel Algemene_Info
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetLastPlayerId", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["PID"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetHighestPlayerID", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
            //
            // The return value is a floating number
            //
            return aux;
        }

        public int GetCompetitionType(int CID)
        {
            //
            // This function gets Competition Type of Competition CID
            //
            int aux = 0;
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetCompetitionType";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["CTY"]);
                    }

                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    aux = 0;
                    WriteLogLine("C#", 0, "GetCompetitionType", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a type number
            //
            return aux;
        }

        public DataSet GetCompetitionTypes(int TC)
        {
            //
            // This function gets all data of the rounds of player PID
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetCompetitionTypes";
                    cmd.Parameters.Add("@TC", SqlDbType.Int).Value = TC;

                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetCompetitionTypes", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public DataSet GetStateDescriptions(int TC)
        {
            //
            // This function gets all data of the rounds of player PID
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetStateDescriptions";
                    cmd.Parameters.Add("@TC", SqlDbType.Int).Value = TC;

                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetStateDescriptions", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public bool Remove_One_Competition(int CID)
        {
            //
            // This function removes one competition
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spRemoveOneCompetition", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "RemoveOneCompetition", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        public bool Remove_Work_Images(string workmap)
        {
            //
            // This function removes one competition
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            //
            // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
            //
            System.IO.DirectoryInfo di = new DirectoryInfo(workmap);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            return error_occurred;
        }

        public bool Remove_Obsolete_Players()
        {
            //
            // This function removes one competition
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spRemovePlayers", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "RemoveOneCompetition", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        public bool Create_Backup(string BackUpString)
        {
            //
            // This function removes one competition
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spCreateBackup", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@BUS", SqlDbType.Text).Value = BackUpString;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "Create_Backup", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        public DataSet GetAllOccurrencesOfOneGame(int PID, int AID)
        {
            //
            // This function results in a dataset with all previously registered absentees and externals from the NietIntern table of a specific round of a 
            // specific competition
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetAllResultsOfOnePlayer";
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@AID", SqlDbType.Int).Value = AID;
                    da.SelectCommand = cmd;
                    con.Open();
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetAllResultsOfOnePlayer", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return ds;
        }

        public int UpdateAccessUser(string Session_Identification, string User)
        {
            //
            // This module Updates one game to the table "Competitie_Resultaten"
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spUpdateAccessUser", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Session", SqlDbType.Int).Value = Session_Identification;
                    cmd.Parameters.Add("@User", SqlDbType.Text).Value = User;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "UpdateAccessUser", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return aux;
        }

        public int UpdateAccessNumber(string Session_Identification, string Manager_Identification, int UpdateNumber)
        {
            //
            // This module Updates one game to the table "Competitie_Resultaten"
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spUpdateAccessNumber", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Session", SqlDbType.Text).Value = Session_Identification;
                    cmd.Parameters.Add("@User", SqlDbType.Text).Value = Manager_Identification;
                    cmd.Parameters.Add("@UAN", SqlDbType.Int).Value = UpdateNumber;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "UpdateAccessNumber", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return aux;
        }

        public int UpdateAccessTimeStamp(string Session_Identification)
        {
            //
            // This module Updates one game to the table "Competitie_Resultaten"
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spUpdateAccessTimeStamp", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Session", SqlDbType.Text).Value = Session_Identification;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "UpdateAccessTimeStamp", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return aux;
        }

        public AccessData GetAccesData(string Session_Identification)
        {
            //
            // Get number of the last ound of a specific competition
            //
            AccessData aux_Access_Data = new AccessData();
            string aux_User = "-";
            DateTime aux_Last_Activity = System.DateTime.Now;
            string aux_SessionID = Session_Identification;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetAccessData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Session", SqlDbType.Int).Value = Session_Identification;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    aux_SessionID = Convert.ToString(reader["SessionID"]);
                    aux_User = Convert.ToString(reader["Huidige_Gebruiker"]);
                    aux_Last_Activity = Convert.ToDateTime(reader["TimeStamp_Laatste_Load"]);
                }
                aux_Access_Data.Huidige_Gebruiker = aux_User;
                aux_Access_Data.TimeStamp_Laatste_Load = aux_Last_Activity;
                aux_Access_Data.SessionID = aux_SessionID;
            }
            //
            // return value is an integer
            //
            return aux_Access_Data;
        }

        public int Count_Sessions()
        {
            //
            // This function gets the number of player available for a normal club evening
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {

                    SqlCommand cmd = new SqlCommand("spCountSessions", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Aantal"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "CountSessions", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is a floating number
            //
            return aux;
        }

        public bool Update_Fontsize(int MID, int Fontsize)
        {
            //
            // This function updates any value in a workflow record  in the Workflow Table
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spUpdateFontsize", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@MID", SqlDbType.Int).Value = MID;
                    cmd.Parameters.Add("@Fontsize", SqlDbType.Int).Value = Fontsize;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", 0, "Update_Fontsize", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        public int PurgeTableLogging()
        {
            //
            // This module Updates one game to the table "Competitie_Resultaten"
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spPurgeTableLogging", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "PurgeTableLogging", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return aux;
        }

        public General_Swiss_Info GetGeneralSwissInfo(int CID)
        {
            //
            // This function returns an instance of a class of all data related to a single player. Sources are multiple database tables
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            General_Swiss_Info gsi = new General_Swiss_Info();
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    //
                    // Get data from Deelnemer table, some of the rating data will be overridden by new data from rating history table
                    // In a future update this information need to be removed from the table
                    //
                    SqlCommand cmd = new SqlCommand("spGetAlgemeneInfoRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    SqlDataReader readerGI = cmd.ExecuteReader();

                    while (readerGI.Read())
                    {
                        gsi.Toernooi_Id = Convert.ToInt32(readerGI["Competitie_Id"]);
                        gsi.Toernooi_Naam = Convert.ToString(readerGI["Toernooi_Naam"]);
                        gsi.Hoofdsponsor = Convert.ToString(readerGI["Hoofdsponsor"]);
                        gsi.Subsponsors = Convert.ToString(readerGI["Subsponsors"]);
                        gsi.Aanmaakdatum = Convert.ToDateTime(readerGI["Aanmaakdatum"]);
                        if (DBNull.Value != readerGI["Toernooi_Logo"])
                        {
                            gsi.Toernooi_Logo = (byte[])readerGI["Toernooi_Logo"];
                        }
                        gsi.Aantal_Ronden = Convert.ToInt32(readerGI["Aantal_Ronden"]);
                        gsi.Aantal_Partijen_Per_Uitslag = Convert.ToInt32(readerGI["Aantal_Partijen_Per_Uitslag"]);
                        gsi.Indelings_Modus = Convert.ToInt16(readerGI["Indelings_Modus"]);
                        gsi.Aantal_Rating_Groepen = Convert.ToInt32(readerGI["Aantal_Rating_Groepen"]);
                        if (DBNull.Value != readerGI["NoPicture"])
                        {
                            gsi.ProfilePicture = (byte[])readerGI["NoPicture"];
                        }
                        gsi.Laatste_Ronde = Convert.ToInt16(readerGI["Laatste_ronde"]);
                        gsi.KFactor = Convert.ToInt32(readerGI["KFactor"]);
                        gsi.Aantal_Invoerpunten = Convert.ToInt32(readerGI["Aantal_Invoerpunten"]);
                        gsi.Decentrale_Invoer_Spread = Convert.ToInt16(readerGI["Decentrale_Invoer_Spread"]);
                        gsi.Decentrale_Invoer_Maximaal = Convert.ToInt32(readerGI["Decentrale_Invoer_Maximaal"]);
                        gsi.Restrictie_Ronden = Convert.ToInt32(readerGI["Restrictie_Ronden"]);
                        gsi.Restrictie_Rating_Grens = Convert.ToInt32(readerGI["Restrictie_Rating_Grens"]);
                        gsi.Website_Basis = Convert.ToString(readerGI["Website_Basis"]);
                        gsi.Website_Template = Convert.ToString(readerGI["Website_Template"]);
                        gsi.Website_Competitie = Convert.ToString(readerGI["Website_Competitie"]);
                        gsi.Client_FTP_Host = Convert.ToString(readerGI["Client_FTP_Host"]);
                        gsi.Client_FTP_UN = Convert.ToString(readerGI["Client_FTP_UN"]);
                        gsi.Client_FTP_PW = Convert.ToString(readerGI["Client_FTP_PW"]);
                        gsi.CurrentState = Convert.ToInt32(readerGI["CurrentState"]);
                        gsi.Competitie_Type = Convert.ToInt32(readerGI["Competitie_Type"]);
                    }
                    readerGI.Close();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetAlgemeneSwissInfoRecord", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
            //
            // This function returns an instance of a class of  player data
            //
            return gsi;
        }

        public int UpdateGeneralSwissInfo(General_Swiss_Info gsi, int ManagerId)
        {
            //
            // This module Updates one game to the table "Competitie_Resultaten"
            //
            WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int aux = 0;

            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spUpdateAlgemeneSwissInfoRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = gsi.Toernooi_Id;
                    cmd.Parameters.Add("@TNA", SqlDbType.Text).Value = gsi.Toernooi_Naam;
                    cmd.Parameters.Add("@HSP", SqlDbType.Text).Value = gsi.Hoofdsponsor;
                    cmd.Parameters.Add("@SSP", SqlDbType.Text).Value = gsi.Subsponsors;
                    cmd.Parameters.Add("@NOR", SqlDbType.Int).Value = gsi.Aantal_Ronden;
                    cmd.Parameters.Add("@PPU", SqlDbType.Int).Value = gsi.Aantal_Partijen_Per_Uitslag;
                    cmd.Parameters.Add("@IMO", SqlDbType.Int).Value = gsi.Indelings_Modus;
                    cmd.Parameters.Add("@ARG", SqlDbType.Int).Value = gsi.Aantal_Rating_Groepen;
                    cmd.Parameters.Add("@AIP", SqlDbType.Int).Value = gsi.Aantal_Invoerpunten;
                    cmd.Parameters.Add("@DIS", SqlDbType.Int).Value = gsi.Decentrale_Invoer_Spread;
                    cmd.Parameters.Add("@DIM", SqlDbType.Int).Value = gsi.Decentrale_Invoer_Maximaal;
                    cmd.Parameters.Add("@KFA", SqlDbType.Int).Value = gsi.KFactor;
                    cmd.Parameters.Add("@RRO", SqlDbType.Int).Value = gsi.Restrictie_Ronden;
                    cmd.Parameters.Add("@CTY", SqlDbType.Int).Value = gsi.Competitie_Type;
                    cmd.Parameters.Add("@RRG", SqlDbType.Int).Value = gsi.Restrictie_Rating_Grens;
                    cmd.Parameters.Add("@WET", SqlDbType.Text).Value = gsi.Website_Template;
                    cmd.Parameters.Add("@WEC", SqlDbType.Text).Value = gsi.Website_Competitie;
                    cmd.Parameters.Add("@CFH", SqlDbType.Text).Value = gsi.Client_FTP_Host;
                    cmd.Parameters.Add("@CFU", SqlDbType.Text).Value = gsi.Client_FTP_UN;
                    cmd.Parameters.Add("@CFP", SqlDbType.Text).Value = gsi.Client_FTP_PW;
                    cmd.Parameters.Add("@CST", SqlDbType.Int).Value = gsi.CurrentState;
                    cmd.Parameters.Add("@MID", SqlDbType.Int).Value = ManagerId;
                    cmd.Parameters.Add("@CIDNEW", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@NoPicture", SqlDbType.Image).Value = gsi.ProfilePicture;
                    cmd.Parameters.Add("@Toernooi_Logo", SqlDbType.Image).Value = gsi.Toernooi_Logo;
                    cmd.Parameters.Add("@LRO", SqlDbType.Int).Value = gsi.Laatste_Ronde;
                    cmd.Parameters.Add("@WEB", SqlDbType.Text).Value = gsi.Website_Basis;
                    cmd.Parameters.Add("@GPR", SqlDbType.Text).Value = gsi.PartijenPerRonde;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                    aux = Convert.ToInt32(cmd.Parameters["@CIDNEW"].Value);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", gsi.Toernooi_Id, "UpdateAlgemeneInfoRecord", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }

            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return aux;
        }

        public int CTYtoRatingtype(int CTY)
        {
            int aux = 0;
            switch (CTY)
            {
                case 1:
                    aux = 1;
                    break;
                case 2:
                    aux = 1;
                    break;
                case 3:
                    aux = 1;
                    break;
                case 4:
                    aux = 1;
                    break;
                case 5:
                    aux = 4;
                    break;
                case 6:
                    aux = 5;
                    break;

            }
            return aux;
        }
        public DataSet GetAbsentRounds(int PID, int CompetitionId)
        {
            //
            // This function gets all rounds, past and future this players is or was absent in this competition
            //
            WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetAbsentRounds";
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CompetitionId;

                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CompetitionId, "GetAbsentRounds", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public int GetIntegrateWithCompetition(int CID)
        {
            int CIDInt = 0;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    //
                    // Get algemene info data
                    //
                    SqlCommand cmd = new SqlCommand("spGetCIDInt", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    SqlDataReader readerCIDInt = cmd.ExecuteReader();

                    if (readerCIDInt.HasRows)
                    {
                        while (readerCIDInt.Read())
                        {
                            CIDInt = Convert.ToInt32(readerCIDInt["Basis_Toernooi"]);
                        }
                        readerCIDInt.Close();

                    }
                    else
                    {
                        CIDInt = CID;
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetCIDInt", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
            //
            // This function returns an instance of a class of  player data
            //
            return CIDInt;

        }

        public void Calculate_New_Pairing_Swiss(int CID, int RNR, int GPR)
        {
            //
            // This function creates the records that describe the games to be played in a specific round of a specific competition
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");

            const int C_Swiss_Game_Type = 9;
            const int C_Swiss_Competition_Type = 3;

            Cleanup_Before_Pairing_Swiss(CID, RNR, C_Swiss_Game_Type);
            Create_Worklist_Swiss(CID, RNR, GPR);
            //
            // handle Byes
            //
            SetUp_Bye_Games(CID, RNR, GPR);
            Remove_NietIntern_From_List(CID, RNR, C_Swiss_Competition_Type);
            //
            // handle Manual fixed games
            //
            SetUp_Manual_Games(CID, RNR, GPR);
            Remove_Manual_From_List(CID, RNR);
            //
            // If there is an odd number of players, determine the free player
            //
            int Number_of_Candidates = Count_Candidates(CID, RNR);
            if (Number_of_Candidates % 2 != 0)
            {
                //
                // Odd number of players left, so find one that will get a free round
                //
                int Free_Player = Find_Free_Round_Player_Swiss(CID, RNR);
                SetUp_Free_Swiss_Game(Free_Player, CID, RNR, GPR);
                Remove_Player_From_List(Free_Player, CID, RNR, C_Swiss_Competition_Type);
            }
            //
            // Even number left, create a swiss pairing for those players available
            //
            //
            // Setup groupsizes for handling brackets
            //
            SetUp_Groups_List(CID, RNR, GPR);
            //
            // After all the preparations, start doing the business !!
            //
            SetUp_Swiss_Games(CID, RNR);
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");

        }

        private bool SetUp_Groups_List(int CID, int RNR, int GPR)
        {
            //
            // create list of group-counts for each point-group in a swiss pairing
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");

            bool error_occurred = false;
            float Matchpoints = (float)0.0;
            int Max_Groups = (RNR-1) * GPR * 2 + 1;
            //
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    //
                    // for each group, get a count of the numbers of players within that group and add that to the database table tbl_Swiss_List_Groups
                    //
                    for (int i = Max_Groups; i > 0; i--)
                    {
                        Matchpoints = (i - 1) / (float)2.0;
                        error_occurred = false;
                        SqlCommand cmd = new SqlCommand("spAddGroupsListRecord", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                        cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                        cmd.Parameters.Add("@GID", SqlDbType.Int).Value = i;
                        cmd.Parameters.Add("@MPO", SqlDbType.Float).Value = Matchpoints;
                        int LR_Value = (int)cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "End");
            //
            return error_occurred;
        }

        private bool Cleanup_Before_Pairing_Swiss(int CID, int RNR, int GameType)
        {
            //
            // At the start of a pairing sequence, clean up the database tables required in the pairing process for the relevant tournament and roundnumber
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");

            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spPreparationsNewPairingSwiss", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    cmd.Parameters.Add("@GT", SqlDbType.Int).Value = GameType;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "Cleanup_Before_Pairing", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        private bool Set_Top_50_Percent(int CID, int RNR)
        {
            //
            // in the last round of a tournament, the absolute color rules are a little bit relaxed to prevent top-players missing each other while competing for top-spots.
            // This is rule B3 of the Swiss Pairing manual
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");

            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spSetTop50Percent", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "Set_Top_50_Percent", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        public void Create_Worklist_Swiss(int CID, int RNR, int GPR)
        {
            //
            // This module creates a list of entries into the temp_table to facilitate printing lists and generating a new round
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            //
            // Remove old templist
            // 
            Remove_Templist(CID, RNR);
            //
            // Get all players from current competition
            //
            PairingWorklistSwiss wl = new PairingWorklistSwiss();
            DataSet ds = new DataSet();
            ds = GetPlayerList(CID);
            DataTable dt = ds.Tables[0];
            //
            // For all participants, collect relevant data for the ranking list, used for pairing and information distribution on papar or on the site
            //
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DataRow Single_Player = dt.Rows[j];
                wl.Speler_Id = System.Convert.ToInt32(Single_Player["Speler_ID"]);
                wl.Had_Pair_Allocated_Bye = GetPairAllocatedBye(wl.Speler_Id, CID, RNR);
                //
                // assemble all relevant data for print lists and new pairing
                //
                Calculate_Player_Data_Swiss(wl, CID, RNR, GPR);
                //
                // Add all information to the database table temp_list
                //
                AddWorklistRecordSwiss(wl);
            }
            if (RNR == GetIntFromAlgemeneInfo(CID, "Aantal_Ronden"))
            {
                //
                // In the last round, mark top half for use in rule B3 of the handbook
                //
                Set_Top_50_Percent(CID, RNR);
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
        }

        private bool GetPairAllocatedBye(int PID, int CID, int RNR)
        {
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            bool aux = false;
            int Count_Byes = 0;

            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetPairAllocatedBye", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                con.Open();

                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Paired"] != DBNull.Value)
                    {
                        Count_Byes = Convert.ToInt16(ds.Tables[0].Rows[0]["Paired"]);
                    }
                    else
                    {
                        Count_Byes = 0;
                    }
                }
                else
                {
                    Count_Byes = 0;
                }

            }
            aux = (Count_Byes != 0);

            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return aux;
        }

        private void Calculate_Player_Data_Swiss(PairingWorklistSwiss wl, int CID, int RNR, int GPR)
        {
            //
            // This module prepares several information items for the Worklist record to be added to the temp_list table
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int Color_Balance = 0;
            int Color_Balance_Last_Two = 0;
            float ELO_Result = (float)0.0;
            int Last_Color = 0;
            int Games = 0;
            float Points = (float)0.0;
            int PID = wl.Speler_Id;
            bool LastRoundUpfloated = false;
            bool LastRoundDownfloated = false;
            int Number_Ups = 0;
            int Number_Downs = 0;

            //
            // Filling the data
            //

            wl.Competition_Id = CID;
            wl.Round_Number = RNR;
            wl.Start_Rating = GetPlayerStartRating(PID, CID);
            wl.Is_In_Top_50_Percent = false;
            wl.Absolute_Color_Preference_White = false;
            wl.Absolute_Color_Preference_Black = false;

            wl.Strong_Color_Preference_White = false;
            wl.Strong_Color_Preference_Black = false;

            wl.Mild_Color_Preference_White = false;
            wl.Mild_Color_Preference_Black = false;

            //
            // compile result data for this player such as color data, elo-rating data, arbiter-ruled-games, normal games and up- and downfloat info
            //
            GetResultsData_Simple_Swiss(wl.Speler_Id, CID, RNR, GPR, ref Color_Balance, ref Color_Balance_Last_Two, ref Last_Color, ref ELO_Result, ref Games, ref Points, ref LastRoundUpfloated, ref LastRoundDownfloated, ref Number_Ups, ref Number_Downs);
            wl.Last_Pairing_Downfloated = LastRoundDownfloated;
            wl.Last_Pairing_Upfloated = LastRoundUpfloated;
            wl.Number_Upfloats = Number_Ups;
            wl.Number_Downfloats = Number_Downs;

            wl.Absolute_Color_Preference_White = (Color_Balance <= -2) || (Color_Balance_Last_Two <= -2);
            wl.Absolute_Color_Preference_Black = (Color_Balance >= 2) || (Color_Balance_Last_Two >= 2);

            if ((wl.Absolute_Color_Preference_White == false) && (wl.Absolute_Color_Preference_Black == false))
            {

                wl.Strong_Color_Preference_White = (Color_Balance == -1);
                wl.Strong_Color_Preference_Black = (Color_Balance == 1);

                if ((wl.Strong_Color_Preference_White == false) && (wl.Strong_Color_Preference_Black == false))
                {
                    wl.Mild_Color_Preference_White = (Last_Color == -1);
                    wl.Mild_Color_Preference_Black = (Last_Color == 1);
                }
            }

            wl.Color_Balance = Color_Balance;
            wl.Rating_Gain = ELO_Result;
            wl.MatchPoints = Points;
            wl.Number_of_Games = Games;

            wl.WP = 0;
            wl.SB = 0;
            wl.Is_In_Top_50_Percent = false;
            //
            // creation completed
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");

        }

        private bool AddWorklistRecordSwiss(PairingWorklistSwiss wl)
        {
            //
            // This function adds a record to the temp_list table
            //
            WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spSaveWorkListDataSwiss", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = wl.Competition_Id;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = wl.Round_Number;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = wl.Speler_Id;
                    cmd.Parameters.Add("@SRA", SqlDbType.Float).Value = wl.Start_Rating;
                    cmd.Parameters.Add("@NGA", SqlDbType.Int).Value = wl.Number_of_Games;
                    cmd.Parameters.Add("@MPT", SqlDbType.Float).Value = wl.MatchPoints;
                    cmd.Parameters.Add("@WPU", SqlDbType.Float).Value = wl.WP;
                    cmd.Parameters.Add("@SBP", SqlDbType.Float).Value = wl.SB;

                    cmd.Parameters.Add("@CBA", SqlDbType.Int).Value = wl.Color_Balance;
                    cmd.Parameters.Add("@ACW", SqlDbType.Bit).Value = wl.Absolute_Color_Preference_White;
                    cmd.Parameters.Add("@ACB", SqlDbType.Bit).Value = wl.Absolute_Color_Preference_Black;
                    cmd.Parameters.Add("@SCW", SqlDbType.Bit).Value = wl.Strong_Color_Preference_White;
                    cmd.Parameters.Add("@SCB", SqlDbType.Bit).Value = wl.Strong_Color_Preference_Black;
                    cmd.Parameters.Add("@MCW", SqlDbType.Bit).Value = wl.Mild_Color_Preference_White;
                    cmd.Parameters.Add("@MCB", SqlDbType.Bit).Value = wl.Mild_Color_Preference_Black;

                    cmd.Parameters.Add("@ABY", SqlDbType.Bit).Value = wl.Had_Pair_Allocated_Bye;
                    cmd.Parameters.Add("@T50", SqlDbType.Bit).Value = wl.Is_In_Top_50_Percent;

                    cmd.Parameters.Add("@LPU", SqlDbType.Bit).Value = wl.Last_Pairing_Upfloated;
                    cmd.Parameters.Add("@NUF", SqlDbType.Bit).Value = wl.Number_Upfloats;
                    cmd.Parameters.Add("@LPD", SqlDbType.Bit).Value = wl.Last_Pairing_Downfloated;
                    cmd.Parameters.Add("@NDF", SqlDbType.Bit).Value = wl.Number_Downfloats;

                    cmd.Parameters.Add("@RGA", SqlDbType.Float).Value = wl.Rating_Gain;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", wl.Competition_Id, "AddWorklistRecordSwiss", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
            //
            WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return error_occurred;
        }


        private void GetColorBalanceData(int PID, int CID, int RNR, ref int Color_Balance, ref int Color_Balance_Last_Two, ref int Last_Color)
        {
            //
            // This function collects color information
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            Color_Balance = 0;
            Color_Balance_Last_Two = 0;

            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetColorBalance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                con.Open();

                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                //
                // Table 1 contains colorbalance over all previous rounds
                // Table 2 contains colorbalance of last two, non-arbiter-ruled-games
                //
                DataTable Color_full = ds.Tables[0];
                DataTable Color_Last = ds.Tables[1];

                if (Color_full.Rows.Count > 0)
                {
                    DataRow cfRow = Color_full.Rows[0];
                    if (cfRow["Color_Balance"] != DBNull.Value)
                    {
                        Color_Balance = Convert.ToInt16(cfRow["Color_Balance"]);
                    }
                    else
                    {
                        //
                        // no records found, so start of the tournament
                        //
                        Color_Balance = 0;
                    }
                }
                else
                {
                    //
                    // Something wrong, continue with colorbalance = 0;
                    //
                    Color_Balance = 0;
                }

                if (Color_Last.Rows.Count > 0)
                {
                    DataRow clRow = Color_Last.Rows[0];
                    if (clRow["Color_Balance_Last_Two"] != DBNull.Value)
                    {

                        Color_Balance_Last_Two = Convert.ToInt16(clRow["Color_Balance_Last_Two"]);
                    }
                    else
                    {
                        //
                        // No two valid occerences found, so  very early in the tournament
                        //
                        Color_Balance_Last_Two = 0;
                    }
                }
                else
                {
                    //
                    // Something wrong, continue with colorbalance = 0;
                    //
                    Color_Balance_Last_Two = 0;
                }
            }

            Last_Color = GetLastColor(PID, CID, RNR);
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");

        }

        private int GetGameLastColor(int PID, int AID, int CID)
        {
            int aux = 0;
            //
            // Get last occurence of a game to be able to switch the colors
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetGameLastColor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                cmd.Parameters.Add("@AID", SqlDbType.Int).Value = AID;
                con.Open();

                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                //
                // Table 1 contains color of PID in the last occurrence
                //
                DataTable Color_Last = ds.Tables[0];

                if (Color_Last.Rows.Count > 0)
                {
                    DataRow cfRow = Color_Last.Rows[0];
                    if (cfRow["Game_Last_Color"] != DBNull.Value)
                    {
                        aux = Convert.ToInt16(cfRow["Game_Last_Color"]);
                    }
                    else
                    {
                        //
                        // no records found, so first occurrence and value = 0
                        //
                        aux = 0;
                    }
                }
                else
                {
                    //
                    // Something wrong, just set value to 0
                    //
                    aux = 0;
                }
            }

            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            return aux;
        }
        /*
                private void GetGameColorBalanceData(int PID, int AID, int CID, ref int Game_Color_Balance, ref int Game_Color_Balance_Last_Two)
                {
                    //
                    // This function collects color information
                    //
                    WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
                    Game_Color_Balance = 0;
                    Game_Color_Balance_Last_Two = 0;

                    string cs = Connection_String_CS();
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        SqlCommand cmd = new SqlCommand("spGameGetColorBalance", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                        cmd.Parameters.Add("@AID", SqlDbType.Int).Value = AID;
                        con.Open();

                        SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        //
                        // Table 1 contains colorbalance over all previous rounds
                        // Table 2 contains colorbalance of last two, non-arbiter-ruled-games
                        //
                        DataTable Color_full = ds.Tables[0];
                        DataTable Color_Last = ds.Tables[1];

                        if (Color_full.Rows.Count > 0)
                        {
                            DataRow cfRow = Color_full.Rows[0];
                            if (cfRow["Game_Color_Balance"] != DBNull.Value)
                            {
                                Game_Color_Balance = Convert.ToInt16(cfRow["Game_Color_Balance"]);
                            }
                            else
                            {
                                //
                                // no records found, so start of the tournament
                                //
                                Game_Color_Balance = 0;
                            }
                        }
                        else
                        {
                            //
                            // Something wrong, continue with colorbalance = 0;
                            //
                            Game_Color_Balance = 0;
                        }

                        if (Color_Last.Rows.Count > 0)
                        {
                            DataRow clRow = Color_Last.Rows[0];
                            if (clRow["Game_Color_Balance_Last_Two"] != DBNull.Value)
                            {

                                Game_Color_Balance_Last_Two = Convert.ToInt16(clRow["Game_Color_Balance_Last_Two"]);
                            }
                            else
                            {
                                //
                                // No two valid occerences found, so  very early in the tournament
                                //
                                Game_Color_Balance_Last_Two = 0;
                            }
                        }
                        else
                        {
                            //
                            // Something wrong, continue with colorbalance = 0;
                            //
                            Game_Color_Balance_Last_Two = 0;
                        }
                    }
                    WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
                }
        */

        private void GetResultsData_Simple_Swiss(int PID, int CID, int RNR, int GPR, ref int Color_Balance, ref int Color_Balance_Last_Two, ref int Last_Color, ref float ELO_Result, ref int Games, ref float Points, ref bool LastRoundUpfloated, ref bool LastRoundDownfloated, ref int Number_Ups, ref int Number_Downs)
        {
            //
            // this function calculates the colorbalance, the mandatory color assignments, the elo and competition gains and the score parameters
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            GetColorBalanceData(PID, CID, RNR, ref Color_Balance, ref Color_Balance_Last_Two, ref Last_Color);

            Number_Downs = 0;
            Number_Ups = 0;

            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetScores", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                con.Open();

                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                DataTable Ratings = ds.Tables[0];
                DataTable Scores = ds.Tables[1];
                if (Ratings.Rows.Count > 0)
                {
                    DataRow raRow = Ratings.Rows[0];
                    if (raRow["ELOResultaat"] != DBNull.Value)
                    {

                        ELO_Result = Convert.ToSingle(raRow["ELOResultaat"]);
                    }
                    else
                    {
                        ELO_Result = (float)0.0;
                    }
                }
                else
                {
                    ELO_Result = (float)0.0;
                }
                //
                // Calculate scores and percentages
                //
                Games = 0;
                Points = (float)0.0;
                int aux = 0;
                bool auxbool = false;
                //
                if (Scores.Rows.Count > 0)
                {
                    for (int j = 0; j < Scores.Rows.Count; j++)
                    {
                        DataRow Single_Score = Scores.Rows[j];
                        aux = Convert.ToInt16(Single_Score["Resultaat"]);
                        Points = Points + Calculate_Swiss_Points(aux, GPR);
                        auxbool = Convert.ToBoolean(Single_Score["Was_Downfloat"]);
                        if (auxbool) Number_Downs++;
                        auxbool = Convert.ToBoolean(Single_Score["Was_Upfloat"]);
                        if (auxbool) Number_Ups++;
                        Games++;
                    }

                    DataRow Last_Score = Scores.Rows[Scores.Rows.Count - 1];
                    auxbool = Convert.ToBoolean(Last_Score["Was_Downfloat"]);
                    LastRoundDownfloated = auxbool;
                    auxbool = Convert.ToBoolean(Last_Score["Was_Upfloat"]);
                    LastRoundUpfloated = auxbool;

                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
        }

        private void SetUp_Bye_Games(int CID, int RNR, int GPR)
        {
            //
            // This function creates "games" for all players that are not playing in the regular clubcompetition (external, absent and Championsgroup
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int Afwezigheidscode = 0;
            int PID = 0;
            float Rating = (float)0.0;
            DataSet ds = new DataSet();
            ds = GetAbsenteeList(CID, RNR);
            SwissGamesData game = new SwissGamesData();
            DataTable tbl = ds.Tables[0];
            for (int j = 0; j < tbl.Rows.Count; j++)
            {
                // Calculate for each participant all relevant information and add it to the dataset
                DataRow myRow = tbl.Rows[j];
                if ((int)myRow["Afwezigheidscode"] == 1)
                {
                    Afwezigheidscode = (int)myRow["Afwezigheidscode"];
                    PID = (int)myRow["Speler_ID"];
                    Rating = GetPlayerStartRating(PID, CID);
                    game.Competition_Id = (short)CID;
                    game.Round_Number = (short)RNR;
                    game.Id_Player_White = PID;
                    game.Matchpoints_Player_White = 0;
                    game.Id_Player_Black = -1;
                    game.Matchpoints_Player_Black = 0;
                    game.Game_Type = 10;
                    game.Game_Result = (GPR * 2 + 1) % 2 + 1 + (GPR * 2 + 1);
                    game.Pairing_Status_Level = 1000;
                    game.Sorting_Key = (int)Rating;
                    AddSwissGame(game);
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
        }

        private void SetUp_Manual_Games(int CID, int RNR, int GPR)
        {
            //
            // This function creates "games" for all players that are not playing in the regular clubcompetition (external, absent and Championsgroup
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            /*
             *  to be defined: for now: set them to bye, generate pairing, start up correct pairing en reset byes to manual sets
             *
                        int Afwezigheidscode = 0;
                        int PID = 0;
                        float Rating = (float)0.0;
                        DataSet ds = new DataSet();

                        ds = GetManualList(CID, RNR);
                        SwissGamesData game = new SwissGamesData();
                        DataTable tbl = ds.Tables[0];
                        for (int j = 0; j < tbl.Rows.Count; j++)
                        {
                            // Calculate for each participant all relevant information and add it to the dataset
                            DataRow myRow = tbl.Rows[j];
                            if ((int)myRow["Afwezigheidscode"] == 1)
                            {
                                Afwezigheidscode = (int)myRow["Afwezigheidscode"];
                                PID = (int)myRow["Speler_ID"];
                                Rating = GetPlayerStartRating(PID, CID);
                                game.Competition_Id = (short)CID;
                                game.Round_Number = (short)RNR;
                                game.Id_Player_White = PID;
                                game.Matchpoints_Player_White = 0;
                                game.Id_Player_Black = -1;
                                game.Matchpoints_Player_Black = 0;
                                game.Game_Type = 10;
                                game.Game_Result = (GPR * 2 + 1) % 2 + 1 + (GPR * 2 + 1);
                                game.Pairing_Status_Level = 1000;
                                game.Sorting_Key = (int)Rating;
                                AddSwissGame(game);
                            }
                        }
             */
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
        }

        public bool AddSwissGame(SwissGamesData OneGame)
        {
            //
            // This module adds one game to the table "Wedstrijden"
            //
            WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spSaveOneGame", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Competitie_Id", SqlDbType.Int).Value = OneGame.Competition_Id;
                    cmd.Parameters.Add("@Id_Witspeler", SqlDbType.Int).Value = OneGame.Id_Player_White;
                    cmd.Parameters.Add("@Id_Zwartspeler", SqlDbType.Int).Value = OneGame.Id_Player_Black;
                    cmd.Parameters.Add("@Rondenr", SqlDbType.Int).Value = OneGame.Round_Number;
                    cmd.Parameters.Add("@Sorteerwaarde", SqlDbType.Int).Value = OneGame.Sorting_Key;
                    cmd.Parameters.Add("@Wedstrijdresultaat", SqlDbType.Int).Value = OneGame.Game_Result;
                    cmd.Parameters.Add("@Wedstrijdtype", SqlDbType.Int).Value = OneGame.Game_Type;
                    cmd.Parameters.Add("@Wit_Remise", SqlDbType.Float).Value = 0.0;
                    cmd.Parameters.Add("@Wit_Verlies", SqlDbType.Float).Value = 0.0;
                    cmd.Parameters.Add("@Wit_Winst", SqlDbType.Float).Value = 0.0;
                    cmd.Parameters.Add("@Zwart_Remise", SqlDbType.Float).Value = 0.0;
                    cmd.Parameters.Add("@Zwart_Verlies", SqlDbType.Float).Value = 0.0;
                    cmd.Parameters.Add("@Zwart_Winst", SqlDbType.Float).Value = 0.0;
                    cmd.Parameters.Add("@NumberChampionsgroupGame", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@Matchpoints_White", SqlDbType.Float).Value = OneGame.Matchpoints_Player_White;
                    cmd.Parameters.Add("@Matchpoints_Black", SqlDbType.Float).Value = OneGame.Matchpoints_Player_Black;
                    cmd.Parameters.Add("@Pairing_Status_Level", SqlDbType.Int).Value = OneGame.Pairing_Status_Level;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", OneGame.Competition_Id, "AddSwissGame", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }

        private int Find_Free_Round_Player_Swiss(int CID, int RNR)
        {
            //
            // This function gets the id of the player that will receive a system paired bye, for now: select the lowest on the list with
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spFreeRoundPlayersSwiss", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["FreeGame"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "FreeRoundPlayers", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is a floating number
            //
            return aux;
        }

        private void SetUp_Free_Swiss_Game(int Free_Player, int CID, int RNR, int GPR)
        {
            //
            // This function create the game for the Free_Game
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            SwissGamesData game = new SwissGamesData();
            int PID = Free_Player;
            float Rating = GetPlayerStartRating(PID, CID);
            game.Competition_Id = (short)CID;
            game.Round_Number = (short)RNR;
            game.Id_Player_White = PID;
            game.Matchpoints_Player_White = 0;
            game.Id_Player_Black = -3;
            game.Matchpoints_Player_Black = 0;
            game.Game_Type = 11;
            game.Game_Result = GPR * 2 + 1 + 1;
            game.Pairing_Status_Level = 100;
            game.Sorting_Key = (int)Rating;
            AddSwissGame(game);
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");

        }
        private void SetUp_Swiss_Games(int CID, int RNR)
        {
            //
            // This function calculates the next round for all still available players
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            const int C_Swiss_Competition_Type = 3;
            string logline = "";
            int Games_Paired = 0;
            //float kf = (float)15.0;
            float Bracket = -1;
            bool Game_Found = false;
            bool Game_Played = false;
            bool Color_Conflict = false;
            bool Continue_Checking = true;
            bool Remember_Switch_Outcome = false;
            int P0 = 0;
            int M0 = 0;
            int X1 = 0;
            int Z1 = 0;
            int P1 = 0;
            int M1 = 0;
            int Optimal_Pairings = 0;
            bool Bracket_is_Homogeneous = true;
            //
            // define relaxing rules
            //
            bool relax_B2 = false;
            bool relax_B6_for_upfloaters = false;
            bool relax_B5_for_upfloaters = false;
            bool relax_B6_for_downfloaters = false;
            bool relax_B5_for_downfloaters = false;

            //
            // Rule A11:
            // P = Number of pairings acceptable at certain stage. First value = P0 or M0
            // X = Number of pairings not fulfilling all color preferences at a certain stage. First value = X1 and increasing
            //
            int P = 0;
            int X = 0;
            int Z = 0;
            int Balance_Color_Preference = 0; // is used for criterium B4
            int Games_in_One_Bracket = 0;
            bool Bracket_Pairing_Complete = false;
            //
            bool S1_Not_Empty = true;
            bool Transposition_Possible = true;
            bool Exchange_possible = true;
            bool relax_next = true;

            PairingWorklistSwiss wl1 = new PairingWorklistSwiss();
            PairingWorklistSwiss wl2 = new PairingWorklistSwiss();

            GamesData game = new GamesData();
            //
            // Start searching for players until no candidates are left:
            //
            while (Count_Swiss_Pairable_Candidates(CID, RNR) > 0)
            {
                //
                // preparations for each bracket:
                //
                S1_Not_Empty = true;
                Game_Found = false;
                Games_in_One_Bracket = 0;
                Balance_Color_Preference = 0;
                //
                // fill up each new bracket
                //
                Bracket = GetHighestSwissBracket(CID, RNR);
                DebugFlag = (Bracket == 2.5);
                WriteLogLine("C#", CID, "Setup_Swiss_Games", "Info", 4, "Pairing of Bracket :" + Bracket.ToString());
                Fill_Bracket(CID, RNR, Bracket);
                //
                // Prepare bracket depending parameters
                //
                P0 = Calculate_P0(CID, RNR); // C2.a - A6.b
                P1 = P0;
                M0 = Calculate_M0(CID, RNR); // C2.a - A6.c
                M1 = M0;
                Calculate_X1_Z1_and_B4(P0, CID, RNR, ref X1, ref Z1, ref Balance_Color_Preference); // C2.b - A8.a and C2.b - A8.b and B4
                Bracket_is_Homogeneous = (M0 == 0);
                if (Bracket_is_Homogeneous)
                {
                    P = P1; // C3.a
                }
                else
                {
                    P = M1; // C3.a
                }
                X = X1; // C3.d
                Z = Z1; // C3.d
                //
                // Rule A11, quality of pairings
                //
                Optimal_Pairings = P0 - X1;
                //
                // create startpositions for permutations
                //
                Prepare_S2_Permutations(CID, RNR);
                Resequence_S2(CID, RNR, 1);
                Bracket_Pairing_Complete = false;
                //
                // Now find
                //
                while (S1_Not_Empty)
                {
                    //
                    // Start with highest S1 player
                    //
                    wl1 = Get_Highest_S1(CID, RNR);
                    S1_Not_Empty = (wl1.Speler_Id != 0);
                    Game_Found = !S1_Not_Empty;
                    int Nr_Downfloat_B5_Violations = 0;
                    int Nr_Upfloat_B5_Violations = 0;
                    int Nr_Downfloat_B6_Violations = 0;
                    int Nr_Upfloat_B6_Violations = 0;
                    int Nr_Color_Violations = 0;
                    //
                    while (Game_Found == false)
                    {
                        //
                        // Search for fitting S2 player
                        //
                        wl2 = Get_Highest_S2(CID, RNR);
                        if (wl2.Speler_Id != 0)
                        {
                            logline = "Speler 1:" + GetPlayerName(wl1.Speler_Id) + "(" + wl1.Speler_Id.ToString() + ") - " + "Speler 2:" + GetPlayerName(wl2.Speler_Id) + "(" + wl2.Speler_Id.ToString() + ")";
                            WriteLogLine("C#", CID, "Setup_Swiss_Games", "Info", 4, logline);
                            //
                            // Check first requirement: B1
                            //
                            Game_Played = GamePlayed(wl1.Speler_Id, wl2.Speler_Id, CID, RNR, GetUniqueRounds(CID));// B1
                            if (DebugFlag)
                            {
                                Game_Played = true;
                            }
                            Continue_Checking = !Game_Played;
                            if (Continue_Checking)
                            {
                                //
                                // Check absolute color requirement B2
                                //
                                if (!relax_B2)
                                {
                                    Color_Conflict = ((wl1.Absolute_Color_Preference_White) && (wl2.Absolute_Color_Preference_White)) || ((wl1.Absolute_Color_Preference_Black) && (wl2.Absolute_Color_Preference_Black));
                                    Continue_Checking = !Color_Conflict;
                                    if (!Continue_Checking)
                                    {
                                        Nr_Color_Violations++;
                                        WriteLogLine("C#", CID, "Setup_Swiss_Games", "Info", 4, "Afgewezen: Een van twee heeft een verplichte kleur");
                                    }
                                    else
                                    {
                                        Remember_Switch_Outcome = SwitchGameSwiss(wl1, wl2, CID, RNR, Games_Paired);
                                    }
                                }
                            }
                            else
                            {
                                WriteLogLine("C#", CID, "Setup_Swiss_Games", "Info", 4, "Afgewezen: Partij al gespeeld");
                            }

                            if (Continue_Checking)
                            {
                                if (!relax_B6_for_downfloaters)
                                {
                                    Continue_Checking = No_B6_Downfloat_Violation(wl1, wl2, CID, RNR);
                                }
                                if (!Continue_Checking)
                                {
                                    Nr_Downfloat_B6_Violations++;
                                    WriteLogLine("C#", CID, "Setup_Swiss_Games", "Info", 4, "Afgewezen:B6 for Downfloaters");
                                }
                            }

                            if (Continue_Checking)
                            {
                                if (!relax_B6_for_upfloaters)
                                {
                                    Continue_Checking = No_B6_Upfloat_Violation(wl1, wl2, CID, RNR);
                                }
                                if (!Continue_Checking)
                                {
                                    Nr_Upfloat_B6_Violations++;
                                    WriteLogLine("C#", CID, "Setup_Swiss_Games", "Info", 4, "Afgewezen:B6 for upfloaters");
                                }
                            }

                            if (Continue_Checking)
                            {
                                if (!relax_B5_for_downfloaters)
                                {
                                    Continue_Checking = No_B5_Downfloat_Violation(wl1, wl2, CID, RNR);
                                }
                                if (!Continue_Checking)
                                {
                                    Nr_Downfloat_B5_Violations++;
                                    WriteLogLine("C#", CID, "Setup_Swiss_Games", "Info", 4, "Afgewezen:B5 for downfloaters");
                                }
                            }

                            if (Continue_Checking)
                            {
                                if (!relax_B5_for_upfloaters)
                                {
                                    Continue_Checking = No_B5_Upfloat_Violation(wl1, wl2, CID, RNR);
                                }
                                if (!Continue_Checking)
                                {
                                    Nr_Upfloat_B5_Violations++;
                                    WriteLogLine("C#", CID, "Setup_Swiss_Games", "Info", 4, "Afgewezen:B5 for upfloaters");
                                }
                            }

                            Game_Found = Continue_Checking;
                            if (Game_Found)
                            {
                                Games_Paired++;
                                Games_in_One_Bracket++;
                                if (Remember_Switch_Outcome)
                                {
                                    game.Id_Witspeler = wl2.Speler_Id;
                                    game.Id_Zwartspeler = wl1.Speler_Id;
                                    game.Competitie_Id = CID;
                                    game.Rondernr = RNR;
                                    game.Wedstrijdresultaat = 0;
                                    game.Wedstrijdtype = 9;
                                    game.Sorteerwaarde = (int)System.Math.Max(wl1.MatchPoints * 10000 + wl1.Start_Rating, wl1.MatchPoints * 10000 + wl2.Start_Rating);
                                    game.Wit_Winst = 0;
                                    game.Wit_Remise = 0;
                                    game.Wit_Verlies = 0;
                                    game.Zwart_Winst = 0;
                                    game.Zwart_Remise = 0;
                                    game.Zwart_Verlies = 0;
                                    game.Matchpoints_White = wl2.MatchPoints;
                                    game.Matchpoints_Black = wl1.MatchPoints;
                                    game.Pairing_Status_Level = 500;
                                    AddGame(game);
                                    Remove_Player_From_List(wl1.Speler_Id, CID, RNR, C_Swiss_Competition_Type);
                                    Remove_Player_From_List(wl2.Speler_Id, CID, RNR, C_Swiss_Competition_Type);
                                    Update_Swiss_Paired_Indicator(CID, RNR, wl1.Speler_Id);
                                    Update_Swiss_Paired_Indicator(CID, RNR, wl2.Speler_Id);
                                    Game_Found = true;
                                    WriteLogLine("C#", CID, "Setup_Swiss_Games", "Info", 4, "Geaccepteerd: Switched; partijen:" + Games_Paired.ToString() + ", spelers over: " + Count_Swiss_Pairable_Candidates(CID, RNR).ToString());
                                }
                                else
                                {
                                    game.Id_Witspeler = wl1.Speler_Id;
                                    game.Id_Zwartspeler = wl2.Speler_Id;
                                    game.Competitie_Id = CID;
                                    game.Rondernr = RNR;
                                    game.Wedstrijdresultaat = 0;
                                    game.Wedstrijdtype = 9;
                                    game.Sorteerwaarde = (int)System.Math.Max(wl1.MatchPoints * 10000 + wl1.Start_Rating, wl1.MatchPoints * 10000 + wl2.Start_Rating);
                                    game.Wit_Winst = 0;
                                    game.Wit_Remise = 0;
                                    game.Wit_Verlies = 0;
                                    game.Zwart_Winst = 0;
                                    game.Zwart_Remise = 0;
                                    game.Zwart_Verlies = 0;
                                    game.Matchpoints_White = wl1.MatchPoints;
                                    game.Matchpoints_Black = wl2.MatchPoints;
                                    game.Pairing_Status_Level = 500;
                                    AddGame(game);
                                    Remove_Player_From_List(wl1.Speler_Id, CID, RNR, C_Swiss_Competition_Type);
                                    Remove_Player_From_List(wl2.Speler_Id, CID, RNR, C_Swiss_Competition_Type);
                                    Update_Swiss_Paired_Indicator(CID, RNR, wl1.Speler_Id);
                                    Update_Swiss_Paired_Indicator(CID, RNR, wl2.Speler_Id);
                                    Game_Found = true;
                                    WriteLogLine("C#", CID, "Setup_Swiss_Games", "Info", 4, "Geaccepteerd, partijen:" + Games_Paired.ToString() + ", spelers over: " + Count_Swiss_Pairable_Candidates(CID, RNR).ToString());
                                }
                                //
                                // Determine if the bracket-pairing is complete
                                //
                                Bracket_Pairing_Complete = (Games_in_One_Bracket == P);
                            }
                            else
                            {
                                // Step 1: try different order in S2 by transposition
                                // 
                                Transposition_Possible = Transpose(CID, RNR, wl2.Speler_Id);
                                if (!Transposition_Possible)
                                {
                                    //
                                    // Step 2 if no transposition is possible, try exchanging players between S1 and S2
                                    //

                                    Exchange_possible = Exchange(CID, RNR, Bracket);
                                    if (Exchange_possible)
                                    {

                                        Prepare_S2_Permutations(CID, RNR);
                                        Transposition_Possible = true;
                                        wl1 = Get_Highest_S1(CID, RNR);
                                        Nr_Downfloat_B5_Violations = 0;
                                        Nr_Upfloat_B5_Violations = 0;
                                        Nr_Downfloat_B6_Violations = 0;
                                        Nr_Upfloat_B6_Violations = 0;
                                        Nr_Color_Violations = 0;
                                    }
                                    else
                                    {
                                        //
                                        // before backtracking, relax the criteria and start again
                                        //
                                        if (relax_next)
                                        {
                                            if (!relax_B5_for_upfloaters)
                                            {
                                                relax_B5_for_upfloaters = true;
                                                relax_next = (Nr_Upfloat_B5_Violations == 0);
                                            }
                                        }
                                        if (relax_next)
                                        {
                                            if (!relax_B5_for_downfloaters)
                                            {
                                                relax_B5_for_downfloaters = true;
                                                relax_next = (Nr_Downfloat_B5_Violations == 0);
                                            }
                                        }
                                        if (relax_next)
                                        {
                                            if (!relax_B6_for_upfloaters)
                                            {
                                                relax_B6_for_upfloaters = true;
                                                relax_next = (Nr_Upfloat_B6_Violations == 0);
                                            }
                                        }
                                        if (relax_next)
                                        {
                                            if (!relax_B5_for_downfloaters)
                                            {
                                                relax_B5_for_upfloaters = true;
                                                relax_next = (Nr_Upfloat_B5_Violations == 0);
                                            }
                                        }
                                        if (relax_next)
                                        {
                                            if (!relax_B5_for_downfloaters)
                                            {
                                                relax_B5_for_upfloaters = true;
                                                relax_next = (Nr_Upfloat_B5_Violations == 0);
                                            }
                                        }
                                        if (!relax_next)
                                        {
                                            //
                                            // restart searching in this bracket from the level of the initial transposition,  but after an exchange
                                            // that were already paired in this bracket
                                            //
                                            if (Games_in_One_Bracket > 0)
                                            {
                                                for (int i = 1; i <= Games_in_One_Bracket; i++)
                                                {
                                                    RemoveLastGame(CID, RNR);
                                                    Games_Paired--;
                                                }
                                                Games_in_One_Bracket = 0;
                                            }
                                            //
                                            // and search for the highest in the newly formed bracket to start again
                                            //
                                            wl1 = Get_Highest_S1(CID, RNR);                                            // Reset_Current_Pairing(CID, RNR, Bracket);
                                        }
                                        // If no exchange is possible, then try backtracking if there are S1 players downfloated, else move rest over to next group 
                                        // implement Rule C12: Backtracking started
                                        // C1: no resolution found for player from higher bracket and player was moved down, then break up pairings previous bracket en fin
                                        // the next downfloater
                                        // If this happens in the last bracket, than always break open previous bracket
                                        // Undo_Current_and_Previous_Bracket();
                                    }
                                }
                                else
                                {
                                    //
                                    // restart searching in this bracket from the level of the transposition, start by removing the games
                                    // that were already paired in this bracket
                                    //
                                    if (Games_in_One_Bracket > 0 )
                                    { 
                                        for (int i = 1; i <= Games_in_One_Bracket;i++ )
                                        {
                                            RemoveLastGame(CID, RNR);
                                            Games_Paired--;
                                        }
                                        Games_in_One_Bracket = 0;
                                    }
                                    //
                                    // and search for the highest in the newly formed bracket to start again
                                    //
                                    wl1 = Get_Highest_S1(CID, RNR);
                                    //
                                    WriteLogLine("C#", CID, "Setup_Swiss_Games", "Info", 4, "Bracket vastgelopen, na transpose opnieuw pairen");
                                }
                            }

                            if (Bracket_Pairing_Complete)
                            {
                                // If bracket paring complete, then start a new bracket to pair
                                S1_Not_Empty = false;
                                // set players over in next bracket
                                if (Bracket_is_Homogeneous)
                                {
                                    Set_Left_Over_Players_To_Next_Bracket(CID, RNR);
                                }

                            }
                        }
                        else
                        {
                            //
                            // There has no player been found for the first player, so the worklist is build back but reverted and in the order of the games 
                            //
                            WriteLogLine("C#", CID, "Setup_Competition_Games", "Info", 4, "Onderaan vastgelopen, opnieuw");
                            ResetWorklist(CID, RNR);
                            Cleanup_Before_Pairing(CID, RNR, 4);
                            Game_Found = true;
                            Games_Paired = 0;
                        }
                    }
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
        }

        private bool No_B5_Upfloat_Violation(PairingWorklistSwiss wl1, PairingWorklistSwiss wl2, int CID, int RNR)
        {
            bool aux = true;
            //
            // check if there is an upfloat here
            //

            aux = (wl1.MatchPoints == wl2.MatchPoints);
            //
            // Check if eiter one has the same float as two rounds ago (why 2????)
            //
            if (!aux)
            {
                if (RNR > 2)
                {
                    int Float_Previous_Round_PID1 = Get_Float_Previous_Round(wl1.Speler_Id, CID, RNR);
                    int Float_Previous_Round_PID2 = Get_Float_Previous_Round(wl2.Speler_Id, CID, RNR);
                    if (wl1.MatchPoints > wl2.MatchPoints)
                    {
                        aux = (Float_Previous_Round_PID1 != 1);
                    }
                    if (wl1.MatchPoints < wl2.MatchPoints)
                    {
                        aux = (Float_Previous_Round_PID2 != 1);
                    }
                }
            }

            return aux;
        }

        private bool No_B5_Downfloat_Violation(PairingWorklistSwiss wl1, PairingWorklistSwiss wl2, int CID, int RNR)
        {
            bool aux = true;
            //
            // check if there is an upfloat here
            //

            aux = (wl1.MatchPoints == wl2.MatchPoints);
            //
            // Check if eiter one has the same float as two rounds ago (why 2????)
            //
            if (!aux)
            {
                if (RNR > 2)
                {
                    int Float_Previous_Round_PID1 = Get_Float_Previous_Round(wl1.Speler_Id, CID, RNR);
                    int Float_Previous_Round_PID2 = Get_Float_Previous_Round(wl2.Speler_Id, CID, RNR);
                    if (wl1.MatchPoints > wl2.MatchPoints)
                    {
                        aux = (Float_Previous_Round_PID1 != 1);
                    }
                    if (wl1.MatchPoints < wl2.MatchPoints)
                    {
                        aux = (Float_Previous_Round_PID2 != 1);
                    }
                }
            }

            return aux;
        }


        private bool No_B6_Downfloat_Violation(PairingWorklistSwiss wl1, PairingWorklistSwiss wl2, int CID, int RNR)
        {
            bool aux = true;
            //
            // check if there is an upfloat here
            //

            aux = (wl1.MatchPoints == wl2.MatchPoints);
            //
            // Check if eiter one has the same float as two rounds ago (why 2????)
            //
            if (!aux)
            {
                if (RNR > 3)
                {
                    int Float_2_Rounds_Ago_PID1 = Get_Float_2_Rounds_Ago(wl1.Speler_Id, CID, RNR);
                    int Float_2_Rounds_Ago_PID2 = Get_Float_2_Rounds_Ago(wl2.Speler_Id, CID, RNR);
                    if (wl1.MatchPoints > wl2.MatchPoints)
                    {
                        aux = (Float_2_Rounds_Ago_PID1 != -1);
                    }
                    if (wl1.MatchPoints < wl2.MatchPoints)
                    {
                        aux = (Float_2_Rounds_Ago_PID2 != -1);
                    }
                }
            }

            return aux;
        }

        private bool No_B6_Upfloat_Violation(PairingWorklistSwiss wl1, PairingWorklistSwiss wl2, int CID, int RNR)
        {
            bool aux = true;
            //
            // check if there is an upfloat here
            //

            aux = (wl1.MatchPoints == wl2.MatchPoints);
            //
            // Check if eiter one has the same float as two rounds ago (why 2????)
            //
            if (!aux)
            {
                if (RNR > 3)
                {
                    int Float_2_Rounds_Ago_PID1 = Get_Float_2_Rounds_Ago(wl1.Speler_Id, CID, RNR);
                    int Float_2_Rounds_Ago_PID2 = Get_Float_2_Rounds_Ago(wl2.Speler_Id, CID, RNR);
                    if (wl1.MatchPoints > wl2.MatchPoints)
                    {
                        aux = (Float_2_Rounds_Ago_PID1 != 1);
                    }
                    if (wl1.MatchPoints < wl2.MatchPoints)
                    {
                        aux = (Float_2_Rounds_Ago_PID2 != 1);
                    }
                }
            }

            return aux;
        }

        private int Get_Float_2_Rounds_Ago(int PID, int CID, int RNR)
        {
            //
            // This function returns the color code of the last game for this player
            //
            int aux = 0;
            int Upfloat = 0;
            int Downfloat = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spSWGetFloatRoundX", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR - 2;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Downfloat = Convert.ToInt16(reader["Was_Downfloat"]);
                        Upfloat = Convert.ToInt16(reader["Was_Upfloat"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "spSWGetFloatRoundX", "Info", 4, lit_Error);
                    //
                    // If no color was found, the last color is 0
                    aux = 0;
                }
            }
            //
            // The return value is either -1, for was downfloat or 1 was upfloat or 0 if no up or downfloat
            //
            aux = Downfloat * -1 + Upfloat;
            //
            return aux;
        }

        private int Get_Float_Previous_Round(int PID, int CID, int RNR)
        {
            //
            // This function returns the color code of the last game for this player
            //
            int aux = 0;
            int Upfloat = 0;
            int Downfloat = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spSWGetFloatRoundX", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR - 1;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Downfloat = Convert.ToInt16(reader["Was_Downfloat"]);
                        Upfloat = Convert.ToInt16(reader["Was_Upfloat"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "spSWGetFloatRoundX", "Info", 4, lit_Error);
                    //
                    // If no color was found, the last color is 0
                    aux = 0;
                }
            }
            //
            // The return value is either -1, for was downfloat or 1 was upfloat or 0 if no up or downfloat
            //
            aux = Downfloat * -1 + Upfloat;
            //
            return aux;
        }


        private void Set_Left_Over_Players_To_Next_Bracket(int CID, int RNR)
        {
            //
            // This function returns the color code of the last game for this player
            //
             WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
             string cs = Connection_String_CS();
             using (SqlConnection con = new SqlConnection(cs))
             {
                 con.Open();

                 SqlCommand cmd = new SqlCommand("spSetLeftOverPlayers", con);
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                 cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                 try
                 {
                     cmd.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {
                     string lit_Error = ex.Message;
                     WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Error", 1, lit_Error);
                 }
                 finally
                 {
                     con.Close();
                     con.Dispose();
                 }
             }
             
        }

        private bool Exchange(int CID, int RNR, float Bracket)
        {
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            bool aux = true;
            int Exchange_Combinations = 0;
            int Max_S1 = 0;
            int Max_S2 = 0;
            const int C_S1 = 1;
            const int C_S2 = 2;
            //int PID1 = 0;
            //int PID2 = 0;
            int Exchanges_Executed = 0;
            //
            // reset original subbracket
            //
            Prepare_Exchange(CID, RNR, Bracket);
            //
            // check if exchange is possible
            //
            Exchanges_Executed = Get_Exchange_Passes(CID, RNR, Bracket);
            Max_S1 = CountNumberSx(CID, RNR, C_S1);
            Max_S2 = CountNumberSx(CID, RNR, C_S2);
            //
            // maximum number of exchanges is Max_s1 * Max_s2
            //
            aux = Exchange_Combinations < Max_S1 * Max_S2;
            if (aux)
            {
                //
                // Now create a list with exchange possibilities
                //
                List<Exchange_Combination> Exchanges = new List<Exchange_Combination>();
                for (int i = Max_S1 + 1; i <= Max_S1 + Max_S2; i++)
                {
                    for (int j = Max_S1; j <= 1; j--)
                    {
                        Exchanges.Add(new Exchange_Combination(j, i, i - j));
                    }
                }
                //
                // Sort the list to the required exchange order
                //
                Exchanges.OrderBy(o => o.Exchange_Value).ThenBy(o => o.S2);
                //
                // And exchange the two two players between the subgroups in the bracket
                //
                Exchange_In_Bracket(Exchanges[Exchanges_Executed + 1].S1, Exchanges[Exchanges_Executed + 1].S2, CID, RNR);
            }
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return aux;
        }

        private void Exchange_In_Bracket(int S1, int S2, int CID, int RNR)
        {
            //
            // This function returns switches the two players between the groups S1 or S2
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spExchangeInBracket", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@S1", SqlDbType.Int).Value = S1;
                cmd.Parameters.Add("@S2", SqlDbType.Int).Value = S2;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is the highest unpaired number of points
            //
            // public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
            //{
            //    return k == 0 ? new[] { new T[0] } :
            //      elements.SelectMany((e, i) =>
            //        elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
            //}

            //      Usage:
            //
            // var result = Combinations(new[] { 1, 2, 3, 4, 5 }, 3);
        }

        private int CountNumberSx(int CID, int RNR, int S1_or_S2)
        {
            //
            // This function returns number of players in either S1 or S2
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spCountNumberSx", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@SNR", SqlDbType.Int).Value = S1_or_S2;
                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt32(cmd.Parameters["@Aantal"].Value);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Error", 1, lit_Error);
                    //
                    // when an error occurred, the number is 0
                    //
                    aux = 0;
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is the number of players in a sub-group of a bracket
            //
            return aux;
        }


        private int Get_Exchange_Passes(int CID, int RNR, float Bracket)
        {
            //
            // This function returns number of exchanges that have been executed within a bracket
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetExchangePasses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@BRK", SqlDbType.Float).Value = Bracket;
                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt32(cmd.Parameters["@Passes"].Value);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Error", 1, lit_Error);
                    //
                    // If no color was found, the last color is 0
                    aux = 0;
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is the highest unpaired number of points
            //
            return aux;
        }

        protected bool Prepare_Exchange(int CID, int RNR, float Bracket)
        {
            //
            // resetting earlier exchanged players
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spPrepareExchange", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    cmd.Parameters.Add("@BRK", SqlDbType.Float).Value = Bracket;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "PrepareExchange", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }

        protected void Log_New_Transposition(int CID, int RNR, int PNR)
        {
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            DataSet ds = new DataSet();
            ds = GetPermutationList(CID, RNR, PNR);
            string Logline = "Permutation nr"+ PNR.ToString() + ": ";
            for (int i=0;i<ds.Tables[0].Rows.Count;i++)
            {
                Logline += System.Convert.ToInt16(ds.Tables[0].Rows[i]["PID"]) + "-";
            }
            WriteLogExceptionTrace("ZS", 1, "Dumplogging", "Info", 4, Logline);
        }
        private bool Transpose(int CID, int RNR, int PID)
        {
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            bool aux = false;
            int New_Permutation_Nr = Transpose_Prepare(CID, RNR, PID);
            int Pivot = find_first_item(CID, RNR, New_Permutation_Nr);
            aux = Pivot > -1;
            if (aux)
            {
                int Ceiling = find_ceiling(CID, RNR, Pivot, New_Permutation_Nr);
                aux = Ceiling > 0;
                if (aux)
                {
                    aux = !swap(Pivot, Ceiling, CID, RNR);
                    Resequence_S2_After_Swap(CID, RNR, New_Permutation_Nr, Pivot);
                    WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
                    Log_New_Transposition(CID, RNR, New_Permutation_Nr);
                }
            }
                //
            // Swap completed
            //
            return aux;
        }

        private int Transpose_Prepare(int CID, int RNR, int PID)
        {
            int aux;
            // 
            // First prepare list before transposition
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int Last = 0;
            int Last_Ascending = 0;
            int First_Descending = 0;
            int Sequence_Number = 0;
            int Sequence_Search_Number = 0;
            int i = 0;
            int Permutation_Number = 0;
            PermutationData Permutation_Record = new PermutationData();

            Last = Highest_Original_Number(CID, RNR);
            Sequence_Search_Number = Last + 1;
            Permutation_Number = Get_Permutation_Number(CID, RNR);
            aux = Permutation_Number + 1;
            Last_Ascending = Get_Sequence_Number(CID, RNR, PID);
            First_Descending = Last_Ascending + 1;


            for (i = 1; i <= Last_Ascending; i++)
            {
                Permutation_Record = Get_Permutation_Record(CID, RNR, i, Permutation_Number);
                Permutation_Record.Permutation_Number = aux;
                Add_Permutation_Data(CID, RNR, Permutation_Record.Sequence_Number, Permutation_Record.PID, Permutation_Record.Original_Sequence_Number, Permutation_Record.Permutation_Number, Permutation_Record.Paired);
                //                Reset_Paired_Indicators(CID, RNR, i);
            }

            for (i = First_Descending; i <= Last; i++)
            {
                Sequence_Search_Number = Sequence_Search_Number - 1;
                Permutation_Record = Get_Permutation_Record(CID, RNR, Sequence_Search_Number, Permutation_Number);
                Permutation_Record.Permutation_Number = aux;
                Sequence_Number = i;
                Add_Permutation_Data(CID, RNR, Sequence_Number, Permutation_Record.PID, Permutation_Record.Original_Sequence_Number, Permutation_Record.Permutation_Number, 0);
            }
            //
            //
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return aux;
        }

        private void Reset_Paired_Indicators(int CID, int RNR, int Rownr)
        {
            //
            // This function resets the paired indicator for a specific row 
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spResetPairedIndicators", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@ROW", SqlDbType.Int).Value = Rownr;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "ResetPairedIndicators", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
        }

        private PermutationData Get_Permutation_Record(int CID, int RNR, int SEQ, int PNR)
        {
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            PermutationData aux = new PermutationData();

            DataSet ds = new DataSet();

            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetPermutationRecord", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@SEQ", SqlDbType.Int).Value = SEQ;
                cmd.Parameters.Add("@PNR", SqlDbType.Int).Value = PNR;

                da.SelectCommand = cmd;
                con.Open();
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetOnePermutation", "Error", 1, lit_Error);
                    ds = null;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            if (ds != null)
            {
                aux.CID = (int)ds.Tables[0].Rows[0]["CID"];
                aux.RNR = (int)ds.Tables[0].Rows[0]["RNR"];
                aux.PID = (int)ds.Tables[0].Rows[0]["PID"];
                aux.Sequence_Number = (int)ds.Tables[0].Rows[0]["Sequence_Number"];
                aux.Original_Sequence_Number = (int)ds.Tables[0].Rows[0]["Original_Sequence_Number"];
                aux.Permutation_Number = (int)ds.Tables[0].Rows[0]["Permutation_Number"];
                aux.Paired = Convert.ToInt16((bool)ds.Tables[0].Rows[0]["Paired"]);
            }
            else
            {
                aux.CID = 0;
                aux.RNR = 0;
                aux.PID = 0;
                aux.Sequence_Number = 0;
                aux.Original_Sequence_Number = 0;
                aux.Permutation_Number = 0;
                aux.Paired = 0;
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // Result is a dataset to be used in a gridview control
            //
            return aux;
        }

        private void Add_Permutation_Data(int CID, int RNR, int SNR, int PID, int OSN, int PNR, int Paired)
        {
            //
            // This function returns the color code of the last game for this player
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spSaveOnePermutation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@SNR", SqlDbType.Int).Value = SNR;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                cmd.Parameters.Add("@OSN", SqlDbType.Int).Value = OSN;
                cmd.Parameters.Add("@PNR", SqlDbType.Int).Value = PNR;
                cmd.Parameters.Add("@PAI", SqlDbType.Int).Value = Paired;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "SaveOnePermutation", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
        }


        private int Highest_Original_Number(int CID, int RNR)
        {
            //
            // This function returns the color code of the last game for this player
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetOriginalNumber", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        aux = (int)reader["Highest"];
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetOriginalNumber", "Error", 1, lit_Error);
                    //
                    // If no color was found, the last color is 0
                    aux = 0;
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is the highest unpaired number of points
            //
            return aux;
        }

        private int Get_Permutation_Number(int CID, int RNR)
        {
            //
            // This function returns the color code of the last game for this player
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetPermutationNumber", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        aux = (int)reader["PNR"];
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetOriginalNumber", "Error", 1, lit_Error);
                    //
                    // If no color was found, the last color is 0
                    aux = 0;
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is the highest unpaired number of points
            //
            return aux;
        }

        private int Get_Sequence_Number(int CID, int RNR, int PID)
        {
            //
            // This function returns the color code of the last game for this player
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetSequenceNumber", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        aux = (int)reader["Seqnr"];
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetSequenceNumber", "Error", 1, lit_Error);
                    //
                    // If no color was found, the last color is 0
                    aux = 0;
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is the highest unpaired number of points
            //
            return aux;
        }

        private PairingWorklistSwiss Get_Highest_S1(int CID, int RNR)
        {
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            PairingWorklistSwiss wl = new PairingWorklistSwiss();
            wl.Speler_Id = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spGetHighestS1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@PME", SqlDbType.Int).Value = 1;
                //
                // This function gets the highest player on the worklist that has to be paired
                //

                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        wl.Speler_Id = Convert.ToInt16(reader["Speler_Id"]);
                        wl.Color_Balance = Convert.ToInt16(reader["Color_Balance"]);
                        wl.Absolute_Color_Preference_White = (Convert.ToByte(reader["Absolute_Color_Preference_White"]) == 1);
                        wl.Absolute_Color_Preference_Black = (Convert.ToByte(reader["Absolute_Color_Preference_Black"]) == 1);
                        wl.Strong_Color_Preference_White = (Convert.ToByte(reader["Strong_Color_Preference_White"]) == 1);
                        wl.Strong_Color_Preference_Black = (Convert.ToByte(reader["Strong_Color_Preference_Black"]) == 1);
                        wl.Mild_Color_Preference_White = (Convert.ToByte(reader["Mild_Color_Preference_White"]) == 1);
                        wl.Mild_Color_Preference_Black = (Convert.ToByte(reader["Mild_Color_Preference_Black"]) == 1);
                        wl.Start_Rating = Convert.ToSingle(reader["Start_Rating"]);
                        wl.MatchPoints = Convert.ToSingle(reader["Aantal_Punten"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetHighestS1", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            return wl;
        }
        private PairingWorklistSwiss Get_Highest_S2(int CID, int RNR)
        {
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            PairingWorklistSwiss wl = new PairingWorklistSwiss();
            wl.Speler_Id = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetHighestS2", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@PME", SqlDbType.Int).Value = 1;
                //
                // This function gets the highest player on the worklist that has to be paired
                //

                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        wl.Speler_Id = Convert.ToInt16(reader["Speler_Id"]);
                        wl.Color_Balance = Convert.ToInt16(reader["Color_Balance"]);
                        wl.Absolute_Color_Preference_White = (Convert.ToByte(reader["Absolute_Color_Preference_White"]) == 1);
                        wl.Absolute_Color_Preference_Black = (Convert.ToByte(reader["Absolute_Color_Preference_Black"]) == 1);
                        wl.Strong_Color_Preference_Black = (Convert.ToByte(reader["Strong_Color_Preference_White"]) == 1);
                        wl.Strong_Color_Preference_White = (Convert.ToByte(reader["Strong_Color_Preference_Black"]) == 1);
                        wl.Mild_Color_Preference_White = (Convert.ToByte(reader["Mild_Color_Preference_White"]) == 1);
                        wl.Mild_Color_Preference_White = (Convert.ToByte(reader["Mild_Color_Preference_Black"]) == 1);
                        wl.Start_Rating = Convert.ToSingle(reader["Start_Rating"]);
                        wl.MatchPoints = Convert.ToSingle(reader["Aantal_Punten"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetHighestS2", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return wl;
        }

        private void Prepare_S2_Permutations(int CID, int RNR)
        {
            //
            // This function fills the permutation data table starting with the players from the current bracket
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spPreparePermutation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "PreparePermutation", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is the highest unpaired number of points
            //
        }

        private void Update_Swiss_Paired_Indicator(int CID, int RNR, int PID)
        {
            //
            // This function returns the color code of the last game for this player
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spUpdateSwissPairedIndicator", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "Update_Swiss_Paired_Indicator", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is the highest unpaired number of points
            //
        }

        private float GetHighestSwissBracket(int CID, int RNR)
        {
            //
            // This function returns the unpaired number of points
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            float aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetHighestSwissBracket", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        aux = (Convert.ToSingle(reader["Aantal_Punten"]));
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetHighestSwissBracket", "Error", 1, lit_Error);
                    //
                    // If no color was found, the last color is 0
                    aux = 0;
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended with points: "+aux.ToString());
            //
            // The return value is the highest unpaired number of points
            //
            return aux;
        }
        private bool Fill_Bracket(int CID, int RNR, float Bracket)
        {
            //
            // This module adds players into the Bracket-table
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spSwissFillBracketRating", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    cmd.Parameters.Add("@BRK", SqlDbType.Float).Value = Bracket;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "Fill_Bracket", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }

        private int Calculate_P0(int CID, int RNR)
        {
            //
            // This function returns the color code of the last game for this player
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetP0", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["P0"]);
                        aux = aux / 2;
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetP0", "Error", 1, lit_Error);
                    //
                    // If no color was found, the last color is 0
                    aux = 0;
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is the highest unpaired number of points
            //
            return aux;
        }
        private int Calculate_M0(int CID, int RNR)
        {
            //
            // This function returns the color code of the last game for this player
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetM0", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["M0"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetM0", "Error", 1, lit_Error);
                    //
                    // If no color was found, the last color is 0
                    aux = 0;
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is the highest unpaired number of points
            //
            return aux;
        }

        private void Calculate_X1_Z1_and_B4(int P0, int CID, int RNR, ref int X1, ref int Z1, ref int B4)
        {
            //
            // This function returns the color code of the last game for this player
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int A8b_w = 0;
            int A8b_b = 0;
            int A8b_W = 0;
            int A8b_B = 0;
            int A8b_a = 0;

            DataSet ds = new DataSet();
            ds = GetPlayerListInBracket(CID, RNR);

            A8b_w = GetSwiss_w(ds, CID, RNR);
            A8b_b = GetSwiss_b(ds, CID, RNR);
            A8b_W = GetSwiss_W(CID, RNR);
            A8b_B = GetSwiss_B(CID, RNR);
            A8b_a = GetSwiss_a(ds, CID, RNR);
            //
            // X1: A8.b
            //
            if ((A8b_B + A8b_b) > (A8b_W + A8b_w))
            {
                X1 = System.Math.Max(0, (P0 - A8b_W - A8b_w - A8b_a));
            }
            else
            {
                X1 = System.Math.Max(0, (P0 - A8b_B - A8b_b - A8b_a));
            }
            //
            // Z1: A8.b
            //
            Z1 = 0;
            if ((RNR % 2) == 0)
            {
                if (A8b_B > A8b_W)
                {
                    Z1 = System.Math.Max(0, (P0 - A8b_W - A8b_w - A8b_b - A8b_a));
                }
                else
                {
                    Z1 = System.Math.Max(0, (P0 - A8b_B - A8b_b - A8b_w - A8b_a));
                }

            }
            //
            // B4
            //
            B4 = GetSwiss_B4(ds, CID, RNR);
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended, X1, Z1 and b4:"+X1.ToString()+", "+ Z1.ToString() + ", "+B4.ToString());
        }

        private int GetSwiss_B4(DataSet ds, int CID, int RNR)
        {
            int aux = 0;
            int White_Preference = 0;
            int Black_Preference = 0;
            //
            // This function returns the color-preference balance in a bracket
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow PlayerRow = ds.Tables[0].Rows[i];
                White_Preference += Convert.ToInt16(PlayerRow["AW"]) + Convert.ToInt16(PlayerRow["SW"]) + Convert.ToInt16(PlayerRow["MW"]);
                Black_Preference += Convert.ToInt16(PlayerRow["AB"]) + Convert.ToInt16(PlayerRow["SB"]) + Convert.ToInt16(PlayerRow["MB"]);
            }

            aux = White_Preference - Black_Preference;
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended: B4="+aux.ToString());
            //
            // The return value is 'B4' required in B.4 from Fide Handbook
            //
            return aux;
        }


        private int GetSwiss_w(DataSet ds, int CID, int RNR)
        {
            //
            // This function returns the color code of the last game for this player
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int aux = 0;
            int i = 0;
            int player_games_count = 0;
            int total_player_count = 0;
            int unplayed_games = 0;
            int PID = 0;

            if (((RNR - 1) % 2) == 1)
            {
                aux = 0;
            }
            else
            {
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow PlayerRow = ds.Tables[0].Rows[i];
                    PID = Convert.ToInt16(PlayerRow["Player_Id"]);
                    player_games_count = GetPlayerGamesCount(PID, CID, RNR);
                    unplayed_games = RNR - 1 - player_games_count;
                    if ((unplayed_games % 2) == 1)
                    {
                        if (PlayerHasMildPreferenceForWhite(PID, CID, RNR))
                        {
                            total_player_count = total_player_count + 1;
                        }
                    }
                }
            }
            aux = total_player_count;
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is 'w' from Definitions A.8 Fide Handbook
            //
            return aux;
        }

        private int GetPlayerGamesCount(int PID, int CID, int RNR)
        {
            int aux = 0;
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started with : "+PID.ToString());

            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetPlayerGamesCount", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Number_Games"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetPlayerGamesCount", "Error", 1, lit_Error);
                    aux = 0;
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return aux;
        }

        private bool PlayerHasMildPreferenceForWhite(int PID, int CID, int RNR)
        {
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            bool aux = false;

            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spSWGetPlayerMildPreferenceWhite", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        aux = (Convert.ToInt16(reader["Mild_Color_PreferenceWhite"]) == 1);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetPlayerMildReferenceWhite", "Error", 1, lit_Error);
                    aux = false;
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return aux;
        }

        private bool PlayerHasMildPreferenceForBlack(int PID, int CID, int RNR)
        {
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            bool aux = false;

            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spSWGetPlayerMildPreferenceBlack", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        aux = (Convert.ToInt16(reader["Mild_Color_Preference_Black"]) == 1);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetPlayerMildReferenceBlack", "Error", 1, lit_Error);
                    aux = false;
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return aux;
        }


        private int GetSwiss_b(DataSet ds, int CID, int RNR)
        {
            //
            // This function returns the color code of the last game for this player
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int aux = 0;
            int i = 0;
            int player_games_count = 0;
            int total_player_count = 0;
            int unplayed_games = 0;
            int PID = 0;

            if (((RNR - 1) % 2) == 1)
            {
                aux = 0;
            }
            else
            {
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow PlayerRow = ds.Tables[0].Rows[i];
                    PID = Convert.ToInt16(PlayerRow["Player_Id"]);
                    player_games_count = GetPlayerGamesCount(PID, CID, RNR);
                    unplayed_games = RNR - 1 - player_games_count;
                    if ((unplayed_games % 2) == 1)
                    {
                        if (PlayerHasMildPreferenceForBlack(PID, CID, RNR))
                        {
                            total_player_count = total_player_count + 1;
                        }
                    }
                }
            }
            aux = total_player_count;
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is 'b' from Definitions A.8 Fide Handbook
            //
            return aux;
        }

        private int GetSwiss_W(int CID, int RNR)
        {
            //
            // This function returns the color code of the last game for this player
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetSwiss_W", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["W"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetSwiss_W", "Error", 1, lit_Error);
                    aux = 0;
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is the highest unpaired number of points
            //
            return aux;
        }

        private int GetSwiss_B(int CID, int RNR)
        {
            //
            // This function returns the color code of the last game for this player
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetSwiss_B", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["B"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetSwiss_B", "Error", 1, lit_Error);
                    aux = 0;
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is the highest unpaired number of points
            //
            return aux;
        }

        private int GetSwiss_a(DataSet ds, int CID, int RNR)
        {
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int PID = 0;
            int aux = 0;
            int player_games_count = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow PlayerRow = ds.Tables[0].Rows[i];
                PID = Convert.ToInt16(PlayerRow["Player_Id"]);
                player_games_count = GetPlayerGamesCount(PID, CID, RNR);
                if (player_games_count == 0)
                {
                    aux = aux + 1;
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is the highest unpaired number of points
            //
            return aux;
        }

        private DataSet GetPlayerListInBracket(int CID, int RNR)
        {
            //
            // This function gets all names (combined into readable names) of the players of a specific competition plus some additional player data for use in
            // multiple situations
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetPlayerListInBracket";
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                da.SelectCommand = cmd;
                da.Fill(ds);
                con.Close();
            }
            //
            // Return value is a dataset
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return ds;
        }

        protected bool swap(int Rownr_Pivot, int Rownr_Ceiling, int CID, int RNR)
        {
            //
            // This module swaps two entries in the permutation datatable
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spSwapping", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                    cmd.Parameters.Add("@SNR1", SqlDbType.Int).Value = Rownr_Pivot + 1;
                    cmd.Parameters.Add("@SNR2", SqlDbType.Int).Value = Rownr_Ceiling + 1;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "Swapping", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }


        protected int find_ceiling(int CID, int RNR, int Pivot, int PNR)
        {
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int Ceiling = 0;
            int Possible_Ceiling = Pivot + 1;
            DataSet ds = new DataSet();
            ds = GetPermutationList(CID, RNR, PNR);
            int NrEntries = ds.Tables[0].Rows.Count;
            int Last_Possible_Ceiling = NrEntries -1;
            int Smallest_Bigger_Than_Pivot = NrEntries+1;
            int Value_Pivot = System.Convert.ToInt16(ds.Tables[0].Rows[Pivot]["Original_Sequence_Number"]);
            int Value_Next = 0;

            while (Possible_Ceiling <= Last_Possible_Ceiling) 
            {
                Value_Next = System.Convert.ToInt16(ds.Tables[0].Rows[Possible_Ceiling]["Original_Sequence_Number"]);
                if (Value_Next > Value_Pivot)
                {
                    if (Value_Next < Smallest_Bigger_Than_Pivot)
                    {  
                        Smallest_Bigger_Than_Pivot = Value_Next;
                        Ceiling = Possible_Ceiling;
                    }
                }
                Possible_Ceiling++;
            }
            if (Ceiling == 0)
            {
                WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended- no more ceiling available ");
            }
            else
            {
                WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended- ceiling : " + System.Convert.ToInt16(ds.Tables[0].Rows[Ceiling]["Original_Sequence_Number"]).ToString());
            }
            return Ceiling;
        }

        protected int find_first_item(int CID, int RNR, int PNR)
        {
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int aux = -1;
            DataSet ds = new DataSet();
            ds = GetPermutationList(CID, RNR, PNR);
            int NrEntries = ds.Tables[0].Rows.Count;
            bool StopSearching = false;

            int searchindex = NrEntries-1;
            while (!StopSearching)
            {
                if (System.Convert.ToInt16(ds.Tables[0].Rows[searchindex - 1]["Original_Sequence_Number"]) > System.Convert.ToInt16(ds.Tables[0].Rows[searchindex]["Original_Sequence_Number"]))
                {
                    searchindex -= 1;
                    StopSearching = searchindex == 0;
                }
                else
                {
                    StopSearching = true;
                    aux = searchindex - 1;
                }
            }

            if (searchindex == 0 && !StopSearching)
            {
                // 
                // last permutation found so first Item = -1
                //
                aux = - 1;
                WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended - no more permutations");
            }
            if (aux > -1)
            {
                WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended - first item :" + System.Convert.ToInt16(ds.Tables[0].Rows[aux]["Original_Sequence_Number"]).ToString());
            }
            return aux;
        }

        protected DataSet GetPermutationList(int CID, int RNR, int PNR)
        {
            //
            // This function gets all names (combined into readable names) of the players of a specific competition plus some additional player data for use in
            // multiple situations
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetPermutationList";
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@PNR", SqlDbType.Int).Value = PNR;


                da.SelectCommand = cmd;
                da.Fill(ds);
                con.Close();
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // Return value is a dataset
            //
            return ds;
        }

        protected int Preference_to_Number(PairingWorklistSwiss wl)
        {
            WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int aux = 0;
            if (wl.Absolute_Color_Preference_White)
            {
                aux = 1;
            }
            if (wl.Strong_Color_Preference_White)
            {
                aux = 2;
            }
            if (wl.Mild_Color_Preference_White)
            {
                aux = 3;
            }
            if (wl.Mild_Color_Preference_Black)
            {
                aux = 4;
            }
            if (wl.Strong_Color_Preference_Black)
            {
                aux = 5;
            }
            if (wl.Absolute_Color_Preference_Black)
            {
                aux = 6;
            }
            WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return aux;
        }

        private bool SwitchGameSwiss(PairingWorklistSwiss wl1, PairingWorklistSwiss wl2, int CID, int RNR, int GNR)
        {

            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            bool aux = false;
            bool finished = false;

            if ((wl1.Absolute_Color_Preference_White) || (wl2.Absolute_Color_Preference_Black))
            {
                aux = false;
                finished = true;
            }
            if ((wl2.Absolute_Color_Preference_White) || (wl1.Absolute_Color_Preference_Black))
            {
                aux = true;
                finished = true;
            }

            if (!finished)
            {
                if (Preference_to_Number(wl1) > Preference_to_Number(wl2))
                {
                    aux = false;
                }
                else if (Preference_to_Number(wl1) < Preference_to_Number(wl2))
                {
                    aux = true;
                }
                else
                {
                    if (RNR == 1)
                    {
                        aux = (GNR % 2) == 0;
                    }
                    else
                    {
                        aux = (RandomNumber(1, 100) % 2) == 0;
                    }
                    /*
                    Or re-evaluate
                    */

                }

            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return aux;
        }
        public bool Remove_NoShow_From_Deelnemer_Competition(int CID, int RNR)
        {
            //
            // This module adds one game to the table "Wedstrijden"
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spRemoveNoShows", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "Swapping", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;

        }
        //
        // Modules for results processing
        //
        public DataSet GetResultsSwissGameList(int CID, int RNR)
        {
            //
            // This function results in a dataset with all games of a specific round of a 
            // specific competition
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetResultsSwissGamesList";
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                da.SelectCommand = cmd;
                con.Open();
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetResultsGamesList", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return ds;
        }

        public void Create_Swiss_Result_Records(int CID, int RNR, int Games_Per_Round)
        {
            //
            // Creates new datarecords in CompetitieResultaat and deleting old ones for this record
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            DataSet ds = GetGamesCompleteData(CID, RNR);
            ResultData ORW = new ResultData();
            ResultData ORB = new ResultData();
            int PID_White;
            int PID_Black;
            float kf = GetKFactor(CID);
            float ELO_White;
            float ELO_Black;
            float ScoreFraction;
            double StepSize = 1.0 / ((float)Games_Per_Round * 2.0);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                PID_Black = (int)row["ID_Zwartspeler"];
                ORW.Competitie_Id = CID;
                ORW.Rondernr = RNR;
                ORW.Partijnaam = "";
                PID_White = (int)row["ID_Witspeler"];
                ORW.Deelnemer_ID = PID_White;
                ORW.Tegenstander = PID_Black;
                ORW.Resultaat = (byte)row["Resultaat_Wedstrijd"];
                ORW.Plaats_Op_Ranglijst = 0;
                ORW.CorrectieOpElo = 0;
                ORW.CorrectieOpCompetitie = 0;
                ORW.Competitie_Resultaat = 0;
                ORW.ChampionsgroupGameNumber = (int)row["NumberChampionsgroupGame"];
                //
                // Kleur value will be overruled if it turns out this was a game between two players and not a "special" game
                ORW.Kleur = 0;
                if (PID_Black > 0)
                {
                    ELO_White = GetPlayerStartRating(PID_White, CID);
                    ELO_Black = GetPlayerStartRating(PID_Black, CID);
                    if (ORW.Resultaat <= (Games_Per_Round * 2 + 1))
                    {
                        ScoreFraction = Convert.ToSingle(1.0 - (ORW.Resultaat - 1) * StepSize);
                        ORW.ELO_Resultaat = Calculate_Rating_One_Swiss_Game(ELO_White, ELO_Black, kf, ScoreFraction);
                    }
                    else
                    {
                        ORW.ELO_Resultaat = 0;
                    }
                }
                else
                {
                    ORW.ELO_Resultaat = 0;
                }
                //
                // create Black result record
                //
                if (PID_Black > 0)
                {
                    ORB.Competitie_Id = CID;
                    ORB.Rondernr = RNR;
                    ORB.Partijnaam = "";
                    ORB.Deelnemer_ID = PID_Black;
                    ORB.Tegenstander = PID_White;
                    ORW.Kleur = 1;
                    ORB.Kleur = -1;
                    ORB.Resultaat = InverseSwissResult((byte)row["Resultaat_Wedstrijd"], Games_Per_Round);
                    ORB.Plaats_Op_Ranglijst = 0;
                    ORB.CorrectieOpElo = 0;
                    ORB.CorrectieOpCompetitie = 0;
                    ORB.ChampionsgroupGameNumber = (int)row["NumberChampionsgroupGame"];
                    ORB.ELO_Resultaat = 0 - ORW.ELO_Resultaat;
                    ORB.Competitie_Resultaat = 0;
                    if (Convert.ToSingle(row["Matchpoints_White"]) > Convert.ToSingle(row["Matchpoints_Black"]))
                    {
                        ORW.Was_Downfloat = 1;
                        ORB.Was_Upfloat = 1;
                    }
                    else
                    {
                        if (Convert.ToSingle(row["Matchpoints_White"]) < Convert.ToSingle(row["Matchpoints_Black"]))
                        {
                            ORW.Was_Upfloat = 1;
                            ORB.Was_Downfloat = 1;
                        }
                        else
                        {
                            ORW.Was_Downfloat = 0;
                            ORB.Was_Upfloat = 0;
                        }
                    }
                }
                else
                {
                    ORW.Kleur = 0;
                    ORB.Kleur = 0;
                }

                AddOneResult(ORW);
                if (PID_Black > 0)
                {
                    AddOneResult(ORB);
                }
                WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            }
        }

        private float Calculate_Rating_One_Swiss_Game(float Elo_Wit, float Elo_Zwart, float K_Factor, float ScoreFraction)
        {
            //
            // This function returns the value of the gain for the whiteplayer
            //
            WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");

            float aux = (float)0.0;
            float K_Factor_Calc = (float)0.0;
            //
            // K factor formule: K_Factor * (1.0-Max(0, min(0.834,(gem-rating-2000)/(500/0.834))))
            // Bij K_factor van 30 levert dit een K_Factor van 30 op beneden gem rating van 2000,
            // een aflopende K_Factor to 5 bij gemiddelde rating van 2500 of hoger
            // 
            float Avg_Rating = (Elo_Wit + Elo_Zwart) / (float)2.0;
            if (Avg_Rating < 2000)
            {
                K_Factor_Calc = K_Factor;
            }
            else
            {
                K_Factor_Calc = K_Factor * ((float)1.0 - System.Math.Max(0, System.Math.Min((float)0.833333, (Avg_Rating - (float)2000.0) / ((float)500.0 / (float)0.833333))));
            }
            //
            // Kansberekenings benadering gebruikt door KNSB:
            //
            float TEN = (float)10.0;
            float Fourhundred = (float)400.0;
            float One = (float)1.0;

            float P_normal = One / (float)(System.Math.Pow(TEN, ((Elo_Wit - Elo_Zwart) / Fourhundred)) + One);
            aux = K_Factor_Calc * (ScoreFraction - (1 - P_normal));
            //
            //
            //
            WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return aux;
        }

        private float Calculate_Swiss_Points(int ResultCode, int GamesPerRound)
        {
            WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            float aux = 0;
            if (ResultCode == 0)
            {
                aux = 0;
            }
            else if (ResultCode <= GamesPerRound * 2 + 1)
            {

                //
                // calculation to compensate for number of games per round.  
                // If games per round = 1: normal results are 1 for win, 2 for draw, 3 for loss
                // below line gives 1 results in 1 point, 2 results in 0.5 points and 3 results in 0 points
                // If games per round = 2 (one with white and one with black: normal results are 1 for 2-0, 2 for 1.5-0.5, 3 for 1-1, 4 for 0.5-1.5 and 5 for 0-2
                // below line gives 1 results in 2 points, 2 results in 1.5 points, 3 results in 1 points, 4 results in 0.5 points anf 5 results in 0 points
                // Same goes for teammatches with 4 results, etc. etc.
                //
                aux = (float)GamesPerRound - (float)(ResultCode - 1) / (float)2.0;
            }
            else if (ResultCode == (((float)GamesPerRound * 2 + 1) * (float)2.0 + 1)) // result after normal results (GPR*2+1) + arbiter results (GPR*2+1)
            {
                aux = (float)GamesPerRound;
            }
            else
            {
                //
                // However, there are results by arbiter rule (player requested-bye, no show and pairing forced byes).These results immediately follow the normal results and their points can be calculated from the line below.
                // If games per round = 1: arbitary results are 4 for R-win, 5 for R-draw, 6 for R-loss
                // below line gives 4 results in 1 point, 5 results in 0.5 points and 6 results in 0 points
                // If games per round = 2 (one with white and one with black: normal results are 6 for 2-0, 7 for 1.5-0.5, 8 for 1-1, 9 for 0.5-1.5 and 10 for 0-2
                // below line gives 1 results in 2 points, 2 results in 1.5 points, 3 results in 1 points, 4 results in 0.5 points anf 5 results in 0 points
                // Same goes for teammatches with 4 results, etc. etc.
                //
                aux = (float)GamesPerRound - (float)(ResultCode - (GamesPerRound * 2 + 1) - 1) / (float)2.0;
            }
            WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return aux;
        }

        private int InverseSwissResult(int ResultCode, int GamesPerRound)
        {
            WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int aux = -1;
            if (ResultCode == 0)
            {
                aux = 0;
            }
            else if (ResultCode <= GamesPerRound * 2 + 1)
            {
                aux = GamesPerRound * 2 + 2 - ResultCode;
            }
            else
            {
                aux = GamesPerRound * 2 * 2 + 2 - (ResultCode - (GamesPerRound * 2 + 2));
            }
            WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return aux;
        }

        private void Resequence_S2(int CID, int RNR, int StartSequencing)
        {
            //
            // set the sequence numbers, starting at 1 in a newpermutation list
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            int Sequence_Number = 0;
            int Record_Id = 0;
            DataSet ds = GetS2List(CID, RNR);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Sequence_Number = Sequence_Number + 1;
                    if ((i + 1) >= StartSequencing)
                    {
                        Record_Id = Convert.ToInt16(ds.Tables[0].Rows[i]["Recordnr"]);
                        UpdateS2SequenceNumber(Sequence_Number, Record_Id);
                        UpdateS2OriginalSequenceNumber(Sequence_Number, Record_Id);
                    }
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
        }

        private void Resequence_S2_After_Swap(int CID, int RNR, int PNR, int Pivot)
        {
            //
            // set the sequence numbers, starting at Pivot+1 in a newpermutation list
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            DataSet ds = GetS2ListAfterSwap(CID, RNR, PNR, Pivot+1);

            int New_Sequence_Number = Pivot+1;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 1; i <= ds.Tables[0].Rows.Count; i++)
                {
                    New_Sequence_Number++;
                    UpdateS2SequenceNumber(New_Sequence_Number, Convert.ToInt32(ds.Tables[0].Rows[i-1]["Recordnr"]));
                }
            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
        }

        private DataSet GetS2List(int CID, int RNR)
        {
            //
            // This function results in a dataset with all Permutation records
            // specific competition
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spS2List";
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                da.SelectCommand = cmd;
                con.Open();
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "S2List", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return ds;
        }

        private DataSet GetS2ListAfterSwap(int CID, int RNR, int PNR, int Pivot)
        {
            //
            // This function results in a dataset with all Permutation records after the swap position
            // specific competition
            //
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetS2ListAfterSwap";
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;
                cmd.Parameters.Add("@PNR", SqlDbType.Int).Value = PNR;
                cmd.Parameters.Add("@PIV", SqlDbType.Int).Value = Pivot;

                da.SelectCommand = cmd;
                con.Open();
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "S2List", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
            WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
            return ds;
        }

        private bool UpdateS2OriginalSequenceNumber(int SNR, int RID)
        {
            //
            // This function updates any value in a workflow record  in the Workflow Table
            //
            WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");

            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spUpdateS2OriginalSequenceNumber", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SNR", SqlDbType.Int).Value = SNR;
                    cmd.Parameters.Add("@RID", SqlDbType.Int).Value = RID;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", 0, "Original Sequence number", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
                return error_occurred;
            }
        }
        private bool UpdateS2SequenceNumber(int SNR, int RID)
        {
            //
            // This function updates any value in a workflow record  in the Workflow Table
            //
            WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Started");

            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spUpdateS2SequenceNumber", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SNR", SqlDbType.Int).Value = SNR;
                    cmd.Parameters.Add("@RID", SqlDbType.Int).Value = RID;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", 0, "Sequence number", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                WriteLogLine("C#", 0, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info", 4, "Ended");
                return error_occurred;
            }
        }

        public DataSet GetAllAdversariesStatistics(int PID)
        {
            //
            // This function gets all data of the rounds of player PID
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetAllAdversariesStatistics";
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetAllAdversariesStatistics", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public void GenerateHeaderFile(DateTime Next_Evening, string textpart, string Name, int CID, int RNR, string RootPath)
        {
            //
            // This function updates and uploads the header file, it is called automatically after processing the results and also by the manual header form
            // That form will also let you select the next club night
            //
            int iLr;
            int iId;
            //
            // Start at 20:00 hours of the next club-evening
            //
            Next_Evening.AddHours(20.0);
            string IDay = Next_Evening.Day.ToString();
            string IMonth = Next_Evening.Month.ToString();
            string IYear = Next_Evening.Year.ToString();
            //
            // Calculate last processed round, if the results have been processed, the last round is also the current round, if the next round is starten but not yet processed
            // The porcessed round is 1 lower than the current round
            int iLr_Processed;
            iLr_Processed = Get_Workflow_Item("[Resultaten Verwerken]", CID, RNR);
            if (iLr_Processed == 1)
            {
                iLr = RNR;
            }
            else
            {
                iLr = RNR - 1;
            }
            iId = RNR;

            //
            // Prepare the .js file with the data for the website
            //
            string DisplayBody = "header_body.js";
            StreamWriter Content = new StreamWriter(RootPath + @"\" + DisplayBody);

            string content_line;
            content_line = "<div align='CENTER'>" + string.Format(textpart, Name, iId, iLr) + "</div><br>";
            Content.WriteLine(content_line);
            Content.Flush();
            Content.Close();
            //
            // Upload the data for the header in the website
            //
            string Display_Competition = GetStringFromAlgemeneInfo(CID, "Website_basis").Trim() + "/" +
                                         GetStringFromAlgemeneInfo(CID, "Website_Competitie").Trim();
            FTP_Basis_Upload_File(Display_Competition, RootPath, CID, DisplayBody);
        }

        public DataSet GetOpenChampionsgroupGames(int WID, int CID)
        {
            //
            // This function returns a list of open games from the Championsgroup for this player
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetOpenChampionsGroupGames";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@WID", SqlDbType.Int).Value = WID;
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "spGetOpenChampionsGroupGames", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }
        public int GetCGGamenrInThisRound(int PID, int CID, int RNR)
        {
            //
            // This function returns the reason why a player is absent from the normal competition
            //
            int GameNr = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetCGGamenrInThisRound", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PlayerId", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@CompetitieId", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@RoundNumber", SqlDbType.Int).Value = RNR;

                    SqlDataReader readerGameNr = cmd.ExecuteReader();
                    while (readerGameNr.Read())
                    {
                        GameNr = Convert.ToInt16(readerGameNr["Kroongroep_partijnummer"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetCGGamenrInThisRound", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                return GameNr;
            }
        }
        public int GetCGPartnerInThisRound(int PID, int CID, int GameNr)
        {
            //
            // This function returns the reason why a player is absent from the normal competition
            //
            int Partner_Id = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetCGPartnerInThisRound", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@GNR", SqlDbType.Int).Value = GameNr;

                    SqlDataReader readerGameNr = cmd.ExecuteReader();
                    while (readerGameNr.Read())
                    {
                        Partner_Id = Convert.ToInt16(readerGameNr["Player_Adv"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetCGPartnerInThisRound", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                return Partner_Id;
            }
        }

        public DataSet GetPlayerListCompetitionOnly(int CID)
        {
            //
            // This function gets all names (combined into readable names) of the players of a specific competition plus some additional player data for use in
            // multiple situations
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetPlayerListCompetitionOnly";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    con.Close();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetPlayerListCompetitionOnly", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public bool UpdateAlarmPanelIndicator(int Status, int Recordnr)
        {
            //
            // This module Updates one status in the logrecord
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spUpdateCriticalErrorStatus", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@STAT", SqlDbType.Int).Value = Status;
                    cmd.Parameters.Add("@REC", SqlDbType.Int).Value = Recordnr;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "UpdateUpdateCriticalErrorStatus", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }

        public bool UpdateChampionsGroupIndicator(int PID, int Indicator)
        {
            //
            // This module Updates one CG indicator to the table "Deelnemers"
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spUpdateChampionsgroupIndicator", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;
                    cmd.Parameters.Add("@CGI", SqlDbType.Int).Value = Indicator;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "UpdateChampionsgroupIndicator", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }

        public bool PlayerIsCG(int PID)
        {
            //
            // This function gets the number of player available for a normal club evening
            //
            bool aux = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {

                    SqlCommand cmd = new SqlCommand("spPlayerIsCG", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Yes"]) == 1;
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "PlayerIsCG", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is a floating number
            //
            return aux;
        }

        private DataSet GetPlayedChampionsGroupGames(int CID)
        {
            //
            // This function gets all names (combined into readable names) of the players of a specific competition plus some additional player data for use in
            // multiple situations
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetPlayedChampionsGroupGames";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    con.Close();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "GetPlayedChampionsGroupGames", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Return value is a dataset
            //
            return ds;
        }

        public DataSet GetAlarmPanelData(string Manager)
        {
            //
            // This function results in a dataset with all previously registered absentees and externals from the NietIntern table of a specific round of a 
            // specific competition
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetAlarmPanelData";
                    cmd.Parameters.Add("@MAN", SqlDbType.Text).Value = Manager.Trim();
                    da.SelectCommand = cmd;
                    con.Open();
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetAlarmPanelData", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return ds;
        }

        private bool Add_Matrix_Value(int CID, int Row, int Column, int Level, float Matrix_Value)
        {
            //
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                        SqlCommand cmd = new SqlCommand("spAddValueToMatrix", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                        cmd.Parameters.Add("@LNR", SqlDbType.Int).Value = Level;
                        cmd.Parameters.Add("@MRW", SqlDbType.Int).Value = Row;
                        cmd.Parameters.Add("@MCO", SqlDbType.Float).Value = Column;
                        cmd.Parameters.Add("@MVA", SqlDbType.Float).Value = Matrix_Value;
                        int Err_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            return true;
        }

        private bool Add_Matrix_RowName(int CID, int Row, int Level, string Row_Label)
        {
            //
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spAddRowNameToMatrix", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@LNR", SqlDbType.Int).Value = Level;
                    cmd.Parameters.Add("@MRW", SqlDbType.Int).Value = Row;
                    cmd.Parameters.Add("@RNA", SqlDbType.Text).Value = Row_Label;
                    int Err_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            return true;
        }

        private bool Add_Matrix_ColumnName(int CID, int Column, int Level, string Column_Label)
        {
            //
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spAddColumnNameToMatrix", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@LNR", SqlDbType.Int).Value = Level;
                    cmd.Parameters.Add("@MCO", SqlDbType.Int).Value = Column;
                    cmd.Parameters.Add("@CNA", SqlDbType.Text).Value = Column_Label;
                    int Err_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, System.Reflection.MethodBase.GetCurrentMethod().Name, "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            return true;
        }

        public int GetCGTournamentIdFrom(int CID)
        {
            //
            // This function gets the number of ID of the championsgroupcompetition that is being played during this competition
            //
            int aux = CID;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                try
                {

                    SqlCommand cmd = new SqlCommand("spGetCGTournamentIDFromCID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = Convert.ToInt16(reader["Basis_Toernooi"]);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetCGTournamentId", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is a number
            //
            return aux;
        }

        private bool Clean_Up_Sort_Matrix (int CID)
        {
            //
            // This function removes all records from a previous calculation for this competition
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spRemoveSortMatrix", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    int error = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "Remove_Sort_Matrix", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        private int Find_Position_In_List(int Value, int MaxNumbers, int[] Array)
        {
            int aux = 0;
            for (int i = 1; i <= MaxNumbers;i++ )
            {
                if (Array[i-1] == Value)
                {
                    aux = i;
                }
            }
            return aux;
        }
        
        private bool Handle_Crosstable_Sorting(int CID)
        {
            //
            // This function removes all records from a previous calculation for this competition
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spHandleCrosstableSorting", con);
                    //SqlCommand cmd = new SqlCommand("spUpdateTotalPerRow", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    int error = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "Handle_Crosstable_Sorting", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        private bool Update_Total_Per_Row(int CID)
        {
            //
            // This function removes all records from a previous calculation for this competition
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spUpdateTotalPerRow", con);
                    //SqlCommand cmd = new SqlCommand("spUpdateTotalPerRow", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    int error = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "UpdateTotalPerRow", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }


        private bool Calculate_Order(int CID)
        {
            //
            // This function removes all records from a previous calculation for this competition
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spCalculateOrder", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    int error = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "CalculateOrder", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        private bool Set_Up_Sorting_Matrices(int CID)
        {
            //
            // This function removes all records from a previous calculation for this competition
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spSetUpSortingMatrices", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    int error = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "SetUpSortingMatrices", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        private bool Step_One_In_Sorting(int CID)
        {
            //
            // This function removes all records from a previous calculation for this competition
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spStepOneInSorting", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    int error = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "StepOneInSorting", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        private bool Step_Two_In_Sorting(int CID)
        {
            //
            // This function removes all records from a previous calculation for this competition
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    error_occurred = false;
                    SqlCommand cmd = new SqlCommand("spStepTwoInSorting", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;

                    int error = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "UpdateTotalPerRow", "Error", 1, lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                //
                // This function returns a error value, but in the calling function, this value is not tested because there is no way to recover from a problem(yet)
                //
                return error_occurred;
            }
        }

        public DataSet GetCrossTableRow (int CID, int Row)
        {
            string cs = Connection_String_CS();
            SqlDataAdapter da = new SqlDataAdapter();

            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetCrossTableRow";
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@ROW", SqlDbType.Int).Value = Row;

                da.SelectCommand = cmd;

                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetCrossTableRow", "Error", 1, lit_Error);
                    ds = null;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Result is a dataset
            //
            return ds;

        }
        public bool SortMatrix(int CID)
        {
            //
            // 0. Matrix starts at (0,0) in top left corner and is assigned as follows: (Column, Row)
            // 0.a Level 1: result matrix, unordered
            //     Level 2: Sorting matrix
            //     Level 3: Inverse of sorting matrix
            //     Level 4: Sorted Result matrix = Sorting_Matrix*Result_Matrix*Inverse_Sorting_Matrix
            //     level 5: intermediate result of Sorting_Matrix*Result_Matrix
            //     Use sparsely filled matrices, so use only record for fields that are filled. So level 2 and 3 are only 2xgroup_size records
            // 0.b Delete all Matrix records from this competition
            //
            int Basic_Tournament = GetCGTournamentIdFrom(CID);
            Clean_Up_Sort_Matrix(Basic_Tournament);
            //
            // 1a. Read from Round Robin first round to assign Player Id's to Player start numbers in Matrix
            // 1b. Determine groups_size
            //
            int Group_size = 0;
            DataSet ds = GetPlayerList(Basic_Tournament);
            if (ds.Tables.Count != 0) 
            {
                Group_size = ds.Tables[0].Rows.Count;
                if ( Group_size != 0)
                {
                    ds.Tables[0].DefaultView.Sort = "Speler_Id ASC";
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            //ds.Dispose();
            //
            // 2a. Populate Row_names (0, 1 through group_size) with Player ID's name
            // 2b. Populate Row_Coded_Names (Groupsize + 3, 1 through group_size with Players ID
            //
            string Player_Name;
            int Player_Number;
            int[] Player_List = new int[Group_size];
            //
            for (int i = 1; i <= Group_size; i++)
            {
                Player_Number = (int)ds.Tables[0].Rows[i-1]["Speler_Id"];
                Add_Matrix_Value(Basic_Tournament, i, Group_size + 3, 1, (float)Player_Number);
                Player_Name = (string)ds.Tables[0].Rows[i-1]["SpelerNaam"];
                Add_Matrix_RowName(Basic_Tournament, i, 1, Player_Name);
                Player_List[i - 1] = Player_Number;
            }
            //
            // 3a. Populate Column_name (1 trhough group_size, 0) with Player startnumbers in ASCI
            //
            for (int i = 1; i <= Group_size; i++)
            {
                Add_Matrix_ColumnName(Basic_Tournament, i, 1, i.ToString());
            }
            //
            // 3b. Populate Column_name (group_size + 1, 0) = 'Total'
            //
            Add_Matrix_ColumnName(Basic_Tournament, Group_size + 1, 1, "Total");
            //
            // 3c. Populate Column_name (group_size + 2, 0) = 'Ranking'
            //
            Add_Matrix_ColumnName(Basic_Tournament, Group_size + 2, 1, "Ranking");
            //
            // 3d. Populate Column_name (0, 0) = 'Name'
            //
            Add_Matrix_ColumnName(Basic_Tournament, 0, 1, "Name");
            //
            // 3e. Populate Column_name (group_size + 3, 0) = 'Player_Id'
            //
            Add_Matrix_ColumnName(Basic_Tournament, Group_size + 3, 1, "Player_Id");            //
            // 4 Fill in Matrix values where Column != Row with value from Round Robin result (where row = Player_2 Id.Startnumber and Column = Player_1_Id.Startnumber)\
            //
            DataSet dsG = GetPlayedChampionsGroupGames(Basic_Tournament);
            if  (dsG.Tables[0].Rows.Count != 0)
            {
                int PID = 0;
                int AID = 0;
                float MPN = 0;
                int Level = 1;

                foreach (DataRow dgItem in dsG.Tables[0].Rows)
                {
                    PID = Convert.ToInt16(dgItem["Player"]);
                    AID = Convert.ToInt16(dgItem["Adversary"]);
                    MPN = Convert.ToSingle(dgItem["MatchPoints"]);
                    Add_Matrix_Value(Basic_Tournament, Find_Position_In_List(PID, Group_size, Player_List), Find_Position_In_List(AID, Group_size, Player_List), Level, MPN);
                }
            }
            //
            // 5. Calculate sum of points per player and fill in sum in column nr group_size + 1
            //
            Update_Total_Per_Row(Basic_Tournament);
            //
            // 6. Calculate order, based on total and fill in Order in column nr group_size + 2
            //
            Calculate_Order(Basic_Tournament);
            //
            // 7a. Setup sorting Matrix, based on order in column group_size+2
            // 7b. Setup inverse of Matrix, based on order in column Group_size+2
            //
            Set_Up_Sorting_Matrices(Basic_Tournament);
            //
            //
            // 8. Calculate intermediate result matrix (Level 5) = Sorting_Matrix*Result_Matrix
            //    first row in intermediate is row in original with number=column-number of 1 in first row of Sorting Matrix
            //    second row in intermedite is row in original with number= column-number of 1 in second row of Sorting Matrix
            //    ....
            //    last row in intermediate is row in original with number=column-number of 1 in last row of Sorting Matrix
            //
            Step_One_In_Sorting(Basic_Tournament);
            //
            // 9. Calculate Sorted result matrix (Level 4) = Level 5 * Inverse_Sorting_Matrix (Level3)
            //    first column in final matrix is column in intermediate matrix with number=row-number of 1 in first column of Inverted Sorting Matrix
            //    second column in final matrix is column in intermediate matrix with number=row-number of 1 in second column of Inverted Sorting Matrix
            //    ....
            //    last column in final matrix is column in intermediate matrix with number=row-number of 1 in last column of Inverted Sorting Matrix
            //
            Step_Two_In_Sorting(Basic_Tournament);
            Add_Matrix_ColumnName(Basic_Tournament, 0, 5, "Name");
            Add_Matrix_ColumnName(Basic_Tournament, Group_size + 3, 5, "Player_Id");
            //
            // 10a. Add names to column 0 
            // 10b. Add totals to column group-size + 3 
            //
            //Handle_Crosstable_Sorting(Basic_Tournament);
            return true;
        }

        public DataSet List_Free_Round_Players(int CID, int RNR)
        {
            //
            // This function returns the list of players, ordered by date of last round with a bye
            // Highest on the list is the person that will be assigned a bye, at the first round with an odd number of players
            //
            string cs = Connection_String_CS();
            SqlDataAdapter da = new SqlDataAdapter();

            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spListFreeRoundPlayers";
                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                cmd.Parameters.Add("@RNR", SqlDbType.Int).Value = RNR;

                da.SelectCommand = cmd;

                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "ListFreeRoundPlayers", "Error", 1, lit_Error);
                    ds = null;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Result is a dataset
            //
            return ds;
            
        }

        public DataSet GetMobileMessageData(int CID)
        {
            //
            // This function results in a dataset with all previously registered absentees and externals from the NietIntern table of a specific round of a 
            // specific competition
            //
            string cs = Connection_String_CS();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spGetMobileMessages";
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    da.SelectCommand = cmd;
                    con.Open();
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetMobileMessages", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return ds;
        }

        public bool UpdateMobileMessageIndicator(int Status, int Recordnr)
        {
            //
            // This module Updates one status in the logrecord
            //
            bool error_occurred = false;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spUpdateMobileMessage", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@STAT", SqlDbType.Int).Value = Status;
                    cmd.Parameters.Add("@REC", SqlDbType.Int).Value = Recordnr;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "UpdateMobileMessageStatus", "Error", 1, lit_Error);
                    error_occurred = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return error_occurred;
        }

        public int CountUnhandledMobileMessages(int CID)
        {
            //
            // Get the number of times a player has been absent within the current competition until now.
            // The information comes from the database table "Competitie resultaten"
            //
            int aux = 0;
            string cs = Connection_String_CS();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spCountUnhandledMobileMessages", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        aux = 0;
                        if (!reader.IsDBNull(0))
                        {
                            aux = Convert.ToInt16(reader["UnhandledMobileMessages"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "CountUnhandledMobileMessages", "Error", 1, lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // The return value is an integer number
            //

            return aux;
        }

    }
}