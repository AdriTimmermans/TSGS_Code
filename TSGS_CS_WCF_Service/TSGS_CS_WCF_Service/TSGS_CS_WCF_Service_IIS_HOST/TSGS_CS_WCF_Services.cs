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
                    WriteLogLine("C#", 0, "GetCompetitionManager", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "SaveCompetitionManager", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "GetCompetitionList", "Error", 1,  lit_Error);
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

                // ?? cmd.ExecuteNonQuery();

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
                        pl.Rating = System.Convert.ToSingle(ClubRow["RatingPoints"]);
                    }
                    else
                    {
                        pl.Rating = (float)0.0;
                    }

                    DataTable Blitz = ds.Tables[1];
                    if (Blitz.Rows.Count > 0)
                    {
                        DataRow BlitzRow = Blitz.Rows[0];
                        pl.StartSnelschaakRating = System.Convert.ToSingle(BlitzRow["RatingPoints"]);
                    }
                    else
                    {
                        pl.StartSnelschaakRating = (float)0.0;
                    }

                    DataTable Blitz2 = ds.Tables[5];
                    if (Blitz2.Rows.Count > 0)
                    {
                        DataRow BlitzRow = Blitz2.Rows[0];
                        pl.Snelschaakrating = System.Convert.ToSingle(BlitzRow["RatingPoints"]);
                    }
                    else
                    {
                        pl.Snelschaakrating = (float)0.0;
                    }

                    DataTable Competition = ds.Tables[2];
                    if (Competition.Rows.Count > 0)
                    {
                        DataRow CompetitionRow = Competition.Rows[0];
                        pl.Competitiepunten = System.Convert.ToSingle(CompetitionRow["RatingPoints"]);
                    }
                    else
                    {
                        pl.Competitiepunten = (float)0.0;
                    }

                    DataTable Rapid = ds.Tables[3];
                    if (Rapid.Rows.Count > 0)
                    {
                        DataRow RapidRow = Rapid.Rows[0];
                        pl.StartRapidrating = System.Convert.ToInt16(RapidRow["RatingPoints"]);
                    }
                    else
                    {
                        pl.StartRapidrating = (int)0;
                    }

                    DataTable Rapid2 = ds.Tables[6];
                    if (Rapid2.Rows.Count > 0)
                    {
                        DataRow RapidRow = Rapid2.Rows[0];
                        pl.Rapidrating = System.Convert.ToInt16(RapidRow["RatingPoints"]);
                    }
                    else
                    {
                        pl.Rapidrating = (int)0;
                    }

                    DataTable StartCompetition = ds.Tables[4];
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
                        if (StartCompetitionRow["StartCompetitionPoints"] != System.DBNull.Value)
                        {
                            pl.StartCompetitiepunten = System.Convert.ToSingle(StartCompetitionRow["StartCompetitionPoints"]);
                        }
                        else
                        {
                            pl.StartCompetitiepunten = (float)0.0;
                        }
                    }
                    else
                    {
                        pl.Startrating = (float)0.0;
                        pl.StartCompetitiepunten = (float)0.0;
                    }

                    DataTable FIDERating = ds.Tables[7];
                    if (FIDERating.Rows.Count > 0)
                    {
                        DataRow FIDERow = FIDERating.Rows[0];
                        pl.FIDErating = System.Convert.ToSingle(FIDERow["RatingPoints"]);
                    }
                    else
                    {
                        pl.FIDErating = (float)0.0;
                    }

                    DataTable KNSBRating = ds.Tables[8];
                    if (KNSBRating.Rows.Count > 0)
                    {
                        DataRow KNSBRow = KNSBRating.Rows[0];
                        pl.KNSBrating = System.Convert.ToSingle(KNSBRow["RatingPoints"]);
                    }
                    else
                    {
                        pl.KNSBrating = (float)0.0;
                    }

                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
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
                    cmd.Parameters.Add("@StartSnelschaakRating", SqlDbType.Float).Value = Player.StartSnelschaakRating;
                    cmd.Parameters.Add("@Snelschaakrating", SqlDbType.Float).Value = Player.Snelschaakrating;
                    cmd.Parameters.Add("@Competitiepunten", SqlDbType.Float).Value = Player.Competitiepunten;
                    cmd.Parameters.Add("@StartRapidrating", SqlDbType.Float).Value = Player.StartRapidrating;
                    cmd.Parameters.Add("@Rapidrating", SqlDbType.Float).Value = Player.Rapidrating;

                    cmd.Parameters.Add("@Startrating", SqlDbType.Float).Value = Player.Startrating;
                    cmd.Parameters.Add("@StartCompetitiepunten", SqlDbType.Float).Value = Player.StartCompetitiepunten;

                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "UpdatePlayer", "Error", 1,  lit_Error);
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
                    cmd.Parameters.Add("@StartSnelschaakRating", SqlDbType.Float).Value = Player.StartSnelschaakRating;
                    cmd.Parameters.Add("@Snelschaakrating", SqlDbType.Float).Value = Player.Snelschaakrating;
                    cmd.Parameters.Add("@Competitiepunten", SqlDbType.Float).Value = Player.Competitiepunten;
                    cmd.Parameters.Add("@StartRapidrating", SqlDbType.Float).Value = Player.StartRapidrating;
                    cmd.Parameters.Add("@Rapidrating", SqlDbType.Float).Value = Player.Rapidrating;

                    cmd.Parameters.Add("@Startrating", SqlDbType.Float).Value = Player.Startrating;
                    cmd.Parameters.Add("@StartCompetitiepunten", SqlDbType.Float).Value = Player.StartCompetitiepunten;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", Player.Competitie_Id, "AddPlayer", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "Update_Workflow_Item", "Error", 1,  lit_Error);

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
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", CID, "Initialise_New_Round", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "Remove_Templist", "Error", 1,  lit_Error);

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
                wl.Round_Free = System.Convert.ToByte(Single_Player["Vrijgeloot"]);
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
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", wl.Competition_Id, "AddWorklistRecord", "Error", 1,  lit_Error);

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

                DataTable Color_full = ds.Tables[0];
                DataTable Color_Last = ds.Tables[1];
                DataTable Ratings = ds.Tables[2];
                DataTable Scores = ds.Tables[3];

                if (Color_full.Rows.Count > 0)
                {
                    DataRow cfRow = Color_full.Rows[0];
                    if (cfRow["Color_Balance"] != DBNull.Value)
                    {
                        Color_Balance = Convert.ToInt16(cfRow["Color_Balance"]);
                    }
                    else
                    {
                        Color_Balance = 0;
                    }
                }
                else
                {
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
                        Color_Balance_Last_Two = 0;
                    }
                }
                else
                {
                    Color_Balance_Last_Two = 0;
                }

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
                    WriteLogLine("C#", CID, "Cleanup_Before_Pairing", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "Count_Player_Absent", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetFreeAbsent", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetPenaltyAbsent", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "Get_Reward_External", "Error", 1,  lit_Error);
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
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", OneGame.Competitie_Id, "Add_Game", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "Get_ChampionsGroupList", "Error", 1,  lit_Error);

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
                    WriteLogLine("C#", CID, "Get_ChampionsGroupList", "Error", 1,  lit_Error);

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
                    WriteLogLine("C#", CID, "Get_ChampionsGroupList", "Error", 1,  lit_Error);

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

        private float Calculate_Competitionpoints_One_Game(float Elo_Wit, float Elo_Zwart, float K_Factor, int Result)
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
                        aux = K_Factor * P_normal * (float)2.0;
                        break;
                    }
                case 2:
                    {
                        aux = K_Factor * (P_normal - (float)0.5) * (float)2.0;
                        break;
                    }
                case 3:
                    {
                        aux = K_Factor * (P_normal - (float)1.0) * (float)2.0;
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
                    game.Wit_Winst = Calculate_Competitionpoints_One_Game(RatingWhite, RatingBlack, KFactor, 1);
                    game.Wit_Remise = Calculate_Competitionpoints_One_Game(RatingWhite, RatingBlack, KFactor, 2);
                    game.Wit_Verlies = Calculate_Competitionpoints_One_Game(RatingWhite, RatingBlack, KFactor, 3);
                    game.Zwart_Winst = 0 - game.Wit_Verlies;
                    game.Zwart_Remise = 0 - game.Wit_Remise;
                    game.Zwart_Verlies = 0 - game.Wit_Winst;
                    game.NumberChampionsgroupGame = Game_Number;
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
                    AddGame(game);
                }
            }
        }

        public void Remove_NietIntern_From_List(int CID, int RNR)
        {
            //
            // Save the new combination between the competition manager and a competition
            //
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
                    WriteLogLine("C#", CID, "RemoveNietInternFromList", "Error", 1,  lit_Error);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
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
                    WriteLogLine("C#", CID, "CountCandidates", "Error", 1,  lit_Error);

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
                    WriteLogLine("C#", CID, "FreeRoundPlayers", "Error", 1,  lit_Error);

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
            AddGame(game);

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
                    WriteLogLine("C#", CID, "RemoveOneGameFromList", "Error", 1,  "First -" + lit_Error);

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
                        WriteLogLine("C#", CID, "RemoveOneGameFromList", "Error", 1,  "Second -" + lit_Error);

                    }
                    finally
                    {
                    }

                }
                con.Close();
                con.Dispose();
            }
        }

        private void Remove_Player_From_List(int PID, int CID, int RNR)
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

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //
                    // no action (yet?) with this message, used during debugging
                    //
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "RemoveOneplayerFromList", "Error", 1,  lit_Error);

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
                    WriteLogLine("C#", CID, "GetUniqueRecords", "Error", 1,  lit_Error);

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
                    WriteLogLine("C#", CID, "GetMaxRounds", "Error", 1,  lit_Error);

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
                    WriteLogLine("C#", CID, "GetHighestWorklistRecord", "Error", 1,  lit_Error);

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
                    WriteLogLine("C#", CID, "GetFollowerWorklist", "Error", 1,  lit_Error);

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
                    WriteLogLine("C#", CID, "CheckGamePresence", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "CheckGamePresence", "Minor", 3,  lit_Error);
                    //
                    // If no color was found, the last color is 0
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
                    WriteLogLine("C#", 0, "CheckGamePresence", "Error", 1,  lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return KNSBRating;
            }
        }

        private bool SwitchGame(PairingWorklist wl1, PairingWorklist wl2, int CID, int RNR)
        {
            //
            // This function calculates the optimisation of the colorbalance after this game is played
            //
            // The return value  is a boolean, telling whether or not it is better to switch the game or not. Possible extention is to check the history over 
            // all available games between the same players
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
                                                aux = (RandomNumber(1, 100) % 2) == 0; break;
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
                    WriteLogLine("C#", CID, "ResetWorklistStep1", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "ResetWorklistStep2", "Error", 1,  lit_Error);
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
            int Skip_Players = System.Math.Max((5 - RNR), 0);
            int Follower_of = 0;
            int Games_Paired = 0;
            bool Game_Found = false;
            bool Color_Conflict = false;

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
                WriteLogLine("C#", CID, "Setup_Competition_Games", "Info", 4,  "Speler 1:" + wl1.Speler_Id.ToString());
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
                        WriteLogLine("C#", CID, "Setup_Competition_Games", "Info", 4,  "Speler 2:" + wl2.Speler_Id.ToString());
                        if (!GamePlayed(wl1.Speler_Id, wl2.Speler_Id, CID, RNR, GetUniqueRounds(CID)))
                        {
                            Color_Conflict = ((wl1.Mandatory_White == 1) && (wl2.Mandatory_White == 1)) || ((wl1.Mandatory_Black == 1) && (wl2.Mandatory_Black == 1));
                            if (!Color_Conflict)
                            {
                                Game_Found = true;
                                Games_Paired++;
                                if (SwitchGame(wl1, wl2, CID, RNR))
                                {
                                    game.Id_Witspeler = wl2.Speler_Id;
                                    game.Id_Zwartspeler = wl1.Speler_Id;
                                    game.Competitie_Id = CID;
                                    game.Rondernr = RNR;
                                    game.Wedstrijdresultaat = 0;
                                    game.Wedstrijdtype = 4;
                                    game.Sorteerwaarde = (int)System.Math.Max(wl1.Current_Rating, wl2.Current_Rating);
                                    game.Wit_Winst = Calculate_Competitionpoints_One_Game(wl2.Current_Rating, wl1.Current_Rating, kf, 1);
                                    game.Wit_Remise = Calculate_Competitionpoints_One_Game(wl2.Current_Rating, wl1.Current_Rating, kf, 2);
                                    game.Wit_Verlies = Calculate_Competitionpoints_One_Game(wl2.Current_Rating, wl1.Current_Rating, kf, 3);
                                    game.Zwart_Winst = 0 - game.Wit_Verlies;
                                    game.Zwart_Remise = 0 - game.Wit_Remise;
                                    game.Zwart_Verlies = 0 - game.Wit_Winst;
                                    AddGame(game);
                                    Remove_Player_From_List(wl1.Speler_Id, CID, RNR);
                                    Remove_Player_From_List(wl2.Speler_Id, CID, RNR);
                                    WriteLogLine("C#", CID, "Setup_Competition_Games", "Info", 4,  "Geaccepteerd: Switched; partijen:"+Games_Paired.ToString()+", spelers over: " + Count_Candidates(CID, RNR).ToString());
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
                                    game.Wit_Winst = Calculate_Competitionpoints_One_Game(wl1.Current_Rating, wl2.Current_Rating, kf, 1);
                                    game.Wit_Remise = Calculate_Competitionpoints_One_Game(wl1.Current_Rating, wl2.Current_Rating, kf, 2);
                                    game.Wit_Verlies = Calculate_Competitionpoints_One_Game(wl1.Current_Rating, wl2.Current_Rating, kf, 3);
                                    game.Zwart_Winst = 0 - game.Wit_Verlies;
                                    game.Zwart_Remise = 0 - game.Wit_Remise;
                                    game.Zwart_Verlies = 0 - game.Wit_Winst;
                                    AddGame(game);
                                    Remove_Player_From_List(wl1.Speler_Id, CID, RNR);
                                    Remove_Player_From_List(wl2.Speler_Id, CID, RNR);
                                    WriteLogLine("C#", CID, "Setup_Competition_Games", "Info", 4,  "Geaccepteerd, partijen:"+Games_Paired.ToString()+", spelers over: " + Count_Candidates(CID, RNR).ToString());
                                }

                            }
                            else
                            {
                                WriteLogLine("C#", CID, "Setup_Competition_Games", "Info", 4,  "Afgewezen:Verplichte Kleur" );
                                Follower_of = wl2.Speler_Id;
                            }
                        }
                        else
                        {
                            WriteLogLine("C#", CID, "Setup_Competition_Games", "Info", 4,  "Afgewezen:Gespeeld");
                            Follower_of = wl2.Speler_Id;
                        }
                    }
                    else
                    {
                        //
                        // There hase no player been found for the first player, so the worklist is build back but reverted and in the order of the games 
                        //
                        WriteLogLine("C#", CID, "Setup_Competition_Games", "Info", 4,  "Onderaan vastgelopen, opnieuw");
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
            bool error_occurred = false;
            Cleanup_Before_Pairing(CID, RNR, 0);
            Create_Worklist(CID, RNR);
            SetUp_Non_Competition_Games(CID, RNR);
            SetUp_ChampionsGroup_Games(CID, RNR);
            Remove_NietIntern_From_List(CID, RNR);
            int Number_of_Candidates = Count_Candidates(CID, RNR);
            if (Number_of_Candidates % 2 != 0)
            {
                int Free_Player = Find_Free_Round_Player(CID, RNR);
                SetUp_Free_Game(Free_Player, CID, RNR);
                Remove_Player_From_List(Free_Player, CID, RNR);
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
                    WriteLogLine("C#", CID, "GetGameList", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetOneGame", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetPlayersWithMail", "Error", 1,  lit_Error);
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

        public void FTP_Basis_Upload_File(string Display_Target, string App_Source, int CID, string File_Name)
        {
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
                Console.WriteLine("Uploading: " + File_Name);
                ftplib.Connect(Webserver_host, user, pass);
                ftplib.ChangeDir(Display_Target);
            }
            catch (Exception ex)
            {
                string lit_Error = ex.Message;
                Console.WriteLine(lit_Error);
                WriteLogLine("C#", CID, "FTP_Basis_Upload_File-1", "Error", 1,  lit_Error);
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
                    Console.Write("\rUploading: {0}/{1} {2}%",
                        ftplib.BytesTotal, ftplib.FileSize, perc);
                    Console.Out.Flush();
                }
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                string lit_Error = ex.Message;
                Console.WriteLine(lit_Error);
                WriteLogLine("C#", CID, "FTP_Basis_Upload_File-2", "Error", 1,  lit_Error);
            }
            //
            // Close connection
            //
            ftplib.Disconnect();

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
                    WriteLogLine("C#", CID, "GetGamesUpdateList", "Error", 1,  lit_Error);
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
                    if (ChampionsgroupGame > 0)
                    {
                        game.Wedstrijdtype = 8;
                    }
                    else
                    {
                        game.Wedstrijdtype = 4;
                    }
                    game.Sorteerwaarde = (int)System.Math.Max(RatingWhite, RatingBlack);
                    game.Wit_Winst = Calculate_Competitionpoints_One_Game(RatingWhite, RatingBlack, kf, 1);
                    game.Wit_Remise = Calculate_Competitionpoints_One_Game(RatingWhite, RatingBlack, kf, 2);
                    game.Wit_Verlies = Calculate_Competitionpoints_One_Game(RatingWhite, RatingBlack, kf, 3);
                    game.Zwart_Winst = 0 - game.Wit_Verlies;
                    game.Zwart_Remise = 0 - game.Wit_Remise;
                    game.Zwart_Verlies = 0 - game.Wit_Winst;
                    game.NumberChampionsgroupGame = ChampionsgroupGame;
                    break;
            }
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
                    WriteLogLine("C#", CID, "GetResultsGamesList", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "UpdateGameResult", "Error", 1,  lit_Error);
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
                        WriteLogLine("C#", CID, "CountResults", "Minor", 3,  lit_Error);
                        //
                        // If no color was found, the last color is 0
                        LR_Value = 0;
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "CountResults", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetGamesFullData", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetResultsPlayerIdData", "Error", 1,  lit_Error);
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
                AddOneResult(ORW);
                if (PID_Black > 0)
                {
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
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", OneResult.Competitie_Id, "SaveOneResult", "Error", 1,  lit_Error);
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
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", OneResult.Competitie_Id, "UpdateOneResult", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CompetitieID, "DeleteResults", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "CalculateAndSaveOneRating", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetCompetitionRanking", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetCompetitionGainRanking", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetELORanking", "Error", 1,  lit_Error);
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
                        WriteLogLine("C#", 0, "GetFunctionStatus", "Minor", 3,  lit_Error);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetFunctionStatus", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "SetGeneralInfoAttribute", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetGeneralInfoAttribute", "Error", 1,  lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // return value is a string
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
                    WriteLogLine("C#", CompetitionId, "GetPlayersAlphabetical", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetResults", "Error", 1,  lit_Error);
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
                    // Get data from Deelnemer table, some of the rating data will be overridden by new data from rating history table
                    // In a future update this information need to be removed from the table
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
                    WriteLogLine("C#", 0, "GetAlgemeneInfoRecord", "Error", 1,  lit_Error);
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
                    cmd.Parameters.Add("@MID", SqlDbType.Int).Value = ManagerId;
                    cmd.Parameters.Add("@CIDNEW", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@NoPicture", SqlDbType.Image).Value = gi.ProfilePicture;

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
            }
            //
            // The return value is boolean value indicating success or failure. The resultcode is not checked in the calling module.
            //
            return aux;
        }

        public bool AddPlayerCompetitionRecord(int CID, int PID, float CPS, float ELO)
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
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "SaveOneDeelnemerCompetitionRecord", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "RemoveAllfromDeelnemerCompetition", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "UpdatePlayerTeam", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetChampionsGroupPar", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", OnePlayer.Competition_Id, "UpdateOneChampionsgroupPlayer", "Error", 1,  lit_Error);
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
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "SaveOneRoundRobinRecord", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "RemoveRoundRobinRecord", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetChampionsgroupResults", "Error", 1,  lit_Error);
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
                            WriteLogLine("C#", CID, "UpdateRoundRobinResult", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetRoundRobinData", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetRoundRobinMPts", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetAllPlayersAlphabetical", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "RemoveOnePlayerFromCom", "Error", 1,  lit_Error);
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
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = PID;

                    SqlDataReader readerValue = cmd.ExecuteReader();
                    while (readerValue.Read())
                    {
                        CompetitionPoints = Convert.ToSingle(readerValue["RatingPoints"]);
                    }
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetCompetitiePunten", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "GetClubRating", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "GetKNSBrating", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "GetGetFIDErating", "Error", 1,  lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                return FIDERating;
            }
        }

        public void WriteLogLine(string User, int CID, string Module, string Level, string LogLine)
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
                    SqlCommand cmd = new SqlCommand("spSaveLogRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@User", SqlDbType.VarChar).Value = User;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CID;
                    cmd.Parameters.Add("@Module", SqlDbType.VarChar).Value = Module;
                    cmd.Parameters.Add("@Level", SqlDbType.VarChar).Value = Level;
                    cmd.Parameters.Add("@LogLine", SqlDbType.VarChar).Value = LogLine;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CID, "SaveLogRecord", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "PlayerHasResults", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "RemoveOnefromDeeln", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "RemoveBlitzResults", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "SaveOneBlitzResultRecord", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", OneBlitzResult.Competitie_Id, "UpdateOneBlitzResult", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetPlayersBlitz", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "GetClubBlitzrating", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "SaveBlitzRating", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "RemoveRatingRound", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "RemoveRatingRound", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetPlayersUniqueBlitz", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetBlitzPenaltyPoints", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetBlitzPenaltyPoints", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetBlitzDisplayPoints", "Error", 1,  lit_Error);
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
            // Get a readable name of a specific player
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
                    WriteLogLine("C#", 0, "GetPlayerProfilePicture", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetNoProfilePicture", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "GetTeamData", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "GetTeamPlayers", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "RemoveTeam", "Error", 1,  lit_Error);

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
                    WriteLogLine("C#", 0, "spUpdateTeamData", "Error", 1,  lit_Error);

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
                    WriteLogLine("C#", 0, "spSaveOneTeam", "Error", 1,  lit_Error);
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
                WriteLogLine("C#", 0, "Validate error", "Error", 1,  "Field Required");
            }
            //
            // check syntax
            //
            if (ErrorCode == 0)
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
                    WriteLogLine("C#", 0, "Validate error", "Error", 1,  lit_Error);
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
                        WriteLogLine("C#", 0, "Validate error", "Error", 1,  "Field outside acceptable values");
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
                WriteLogLine("C#", 0, "Validate error", "Error", 1,  "Field Required");
            }
            //
            // check syntax
            //
            if (ErrorCode == 0)
            {
                try
                {
                    InputValue = Convert.ToDouble(Inputfield.Trim(),System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    ErrorCode = 1;
                    InputValue = Default;
                    WriteLogLine("C#", 0, "Validate error", "Error", 1,  lit_Error);
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
                        WriteLogLine("C#", 0, "Validate error", "Error", 1,  "Field outside acceptable values");
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
                WriteLogLine("C#", 0, "Validate error", "Error", 1,  "Field Required");
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
                    WriteLogLine("C#", CID, "Remove_Last_Round", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 30, "GetWorkflowItem", "Error", 1,  lit_Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                return Value;
            }
        }

        public DataSet GetPlayersWithByeList()
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
                    da.SelectCommand = cmd;
                    con.Open();
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", 0, "GetPlayersWithBye", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "GetRetentionStatistics", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "spRenewKNSBRatings", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "spRenewFIDERatings", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "GetUniquePlayers", "Error", 1,  lit_Error);
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
                        WriteLogLine("C#", 0, "spUpdateKNSBRating", "Error", 1,  lit_Error);
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
                        WriteLogLine("C#", 0, "spUpdateFIDERating", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "spPrepareBlitzCompetitionRanking", "Error", 1,  lit_Error);

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
                    WriteLogLine("C#", CID, "GetChampionsgroupSchedule", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "GetHighestPlayerID", "Error", 1,  lit_Error);

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
                    WriteLogLine("C#", 0, "GetCompetitionTypes", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "GetStateDescriptions", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", CID, "RemoveOneCompetition", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "RemoveOneCompetition", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "Create_Backup", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "GetAllResultsOfOnePlayer", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "UpdateAccessUser", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "UpdateAccessNumber", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "UpdateAccessTimeStamp", "Error", 1,  lit_Error);
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
                    WriteLogLine("C#", 0, "CountSessions", "Error", 1,  lit_Error);

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
                    WriteLogLine("C#", 0, "Update_Fontsize", "Error", 1,  lit_Error);

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
                    WriteLogLine("C#", 0, "PurgeTableLogging", "Error", 1,  lit_Error);
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

        public DataSet GetAbsentRounds(int PID, int CompetitionId)
        {
            //
            // This function gets all rounds, past and future this players is or was absent in this competition
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
                    cmd.CommandText = "spGetAbsentRounds";
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = CompetitionId;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = CompetitionId;

                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    WriteLogLine("C#", CompetitionId, "GetAbsentRounds", "Error", 1,  lit_Error);
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

        public bool HandleOneMutationRecord(int OIL, int VIL)
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
                    SqlCommand cmd = new SqlCommand("spHandleOneMutationRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@OIL", SqlDbType.Int).Value = OIL;
                    cmd.Parameters.Add("@VIL", SqlDbType.Int).Value = VIL;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", 0, "HandleOneMutationRecord", "Error", 1,  lit_Error + OIL.ToString() + "-" + VIL.ToString());

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

        public bool CleanUpMutationRecords()
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
                    SqlCommand cmd = new SqlCommand("spCleanUpMutationRecords", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int LR_Value = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    error_occurred = true;
                    WriteLogLine("C#", 0, "CleanUpMutationRecords", "Error", 1,  lit_Error);

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

    }
}