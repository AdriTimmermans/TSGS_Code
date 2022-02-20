using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;

namespace TSGS_CS_WCF_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITSGS_CS_WCF_Service" in both code and config file together.
    [ServiceContract]
    public interface ITSGS_CS_WCF_Service
    {
        [OperationContract]
        CompetitionManager GetCompetitionManager(string CompetitionManagerName);

        [OperationContract]
        void SaveCompetitionManager(CompetitionManager CompetitionManager);

        [OperationContract]
        DataSet GetCompetitionList(string CompetitionManagerName, int Manager_Id);

        [OperationContract]
        int GetRoundNumber(int CompetitieId);

        [OperationContract]
        DataSet GetWorkFlowRecord(int CompetitionManagerId, int RoundNumber);

        [OperationContract]
        int GetNumberWithdrawn(int CompetitieId);

        [OperationContract]
        int GetNumberExternal(int CID, int RNR);

        [OperationContract]
        int GetNumberAbsentees(int CID, int RNR);

        [OperationContract]
        int GetNumberTotal(int CID);

        [OperationContract]
        DataSet GetPlayerList(int CompetitionId);

        [OperationContract]
        DataSet GetAllAdversariesStatistics(int PID);

        [OperationContract]
        DataSet GetAbsenteeRegisterList(int CompetitionId, int RoundNumber);

        [OperationContract]
        DataSet GetAbsenteeList(int CompetitionId, int RoundNumber);

        [OperationContract]
        string GetPlayerName(int PID);

        [OperationContract]
        string GetPlayerFirstName(int PID);

        [OperationContract]
        bool AddAbsentee(AbsenteeData Absentee);

        [OperationContract]
        bool DeleteAbsentees(int CompetitieID, int RondeNummer);

        [OperationContract]
        bool Update_Workflow_Item(string Item_Name, int CID, int RNR, int Item_Value);

        [OperationContract]
        int GetNoPlayCode(int PID, int CID, int RNR);

        [OperationContract]
        DataSet GetKNSBPlayerList(string Playername);

        [OperationContract]
        DataSet GetFIDEPlayerList(string Playername);

        [OperationContract]
        bool AddPlayer(PlayerBasicData Player);

        [OperationContract]
        PlayerBasicData GetPlayerFullData(int Player, int CID);

        [OperationContract]
        bool UpdatePlayer(PlayerBasicData Player, int CID);

        [OperationContract]
        bool Initialize_New_Round(int CID, int RNR);

        [OperationContract]
        bool Calculate_New_Pairing(int CID, int RNR);

        [OperationContract]
        float GetPlayerStartCompetitionPoints(int PID, int CID);

        [OperationContract]
        float GetPlayerStartRating(int PID, int CID);

        [OperationContract]
        float GetPlayerRatingInCompetition(int PID, int RNR, int CID);

        [OperationContract]
        float GetPlayerPointsInCompetition(int PID, int RNR, int CID);

        [OperationContract]
        void Create_Worklist(int CID, int RNR);

        [OperationContract]
        int GetKroongroepPartijNummer(int PID, int CID, int RNR);

        [OperationContract]
        DataSet GetGameList(int CID, int RNR, int GameType, int Language);

        [OperationContract]
        string GetStringFromAlgemeneInfo(int CID, string AttributeName);

        [OperationContract]
        string FTP_Basis_Upload_File(string Display_Target, string App_Source, int CID, string File_Name);

        [OperationContract]
        DataSet GetMailAddresses(int CID);

        [OperationContract]
        DataSet GetOneGame(int PID, int CID, int RNR);

        [OperationContract]
        DataSet GetGamesUpdateList(int CID, int RNR);

        [OperationContract]
        void AddManualGame(int PIDWhite, int PIDBlack, int CID, int RNR, int ChampionsgroupGame);

        [OperationContract]
        void Remove_Games_From_List(int PIDWhite, int PIDBlack, int CID, int RNR);

        [OperationContract]
        bool IsPlayerInChampiongroup(int PID);

        [OperationContract]
        DataSet GetResultsGameList(int CID, int RNR);

        [OperationContract]
        bool UpdateResult(int PID_White, int PID_Black, int CID, int RNR, int auxR);

        [OperationContract]
        bool AllResultsEntered(int CID, int RNR);

        [OperationContract]
        bool DeleteResults(int CompetitieID, int RondeNummer);

        [OperationContract]
        void Create_Result_Records(int CID, int RNR);

        [OperationContract]
        void AdministrationRatingData(int CID, int RNR);

        [OperationContract]
        bool Remove_Templist(int CID, int RNR);

        [OperationContract]
        DataSet GetCompetitionRankingList(int CID, int RNR);

        [OperationContract]
        int GetMaxRounds(int CID);

        [OperationContract]
        int Count_Player_Absent(int PID, int CID, int RNR);

        [OperationContract]
        DataSet GetCompetitionGainRankingList(int CID, int RNR);

        [OperationContract]
        DataSet GetELORankingList(int CID, int RNR);

        [OperationContract]
        int ButtonAllowed(string FunctionDescription, int Status, int Privileges);

        [OperationContract]
        int GetIntFromAlgemeneInfo(int CID, string AttributeName);

        [OperationContract]
        void SetIntInAlgemeneInfo(string Item_Name, int CID, int Item_Value);

        [OperationContract]
        DataSet GetPlayerListAlphabetical(int CID);

        [OperationContract]
        DataSet GetResults(int PID, int CID, int TC);

        [OperationContract]
        bool UpdateOneResult(ResultData OneResult);

        [OperationContract]
        GeneralInfo GetGeneralInfo(int CID);

        [OperationContract]
        int UpdateGeneralInfo(GeneralInfo gi, int ManagerId);

        [OperationContract]
        General_Swiss_Info GetGeneralSwissInfo(int CID);

        [OperationContract]
        int UpdateGeneralSwissInfo(General_Swiss_Info gi, int ManagerId);

        [OperationContract]
        bool DeleteDeelnemerCompetitieRecords(int CID, int CIDOld);

        [OperationContract]
        bool AddPlayerCompetitionRecord(int CID, int PID, float CPS, float ELO, int COT);

        [OperationContract]
        bool UpdatePlayerTeam(int PID, int Team, string Association_ID);

        [OperationContract]
        DataSet GetChampionsgroupPlayerList(int CID);

        [OperationContract]
        bool UpdateChampionsGroupPlayer(ChampionsgroupData OnePlayer);

        [OperationContract]
        bool AddPlayerRoundRobinRecord(int CID, int RNR, int GNR, int PID, int ADV, int COL);

        [OperationContract]
        bool Remove_RoundRobin(int CID);

        [OperationContract]
        void Upgrade_ChampionsgroupPoints(int CID);

        [OperationContract]
        DataSet GetRoundRobinData(int CID);

        [OperationContract]
        float GetChampionsgroupMatchpoint(int PID, int ADV, int CID);

        [OperationContract]
        DataSet GetAllOtherPlayersAlphabetical(int CID);

        [OperationContract]
        void Remove_Player_From_CompetitionRating(int PID, int CID);

        [OperationContract]
        float GetClubRating(int PID);

        [OperationContract]
        float GetKNSBRating(int PID);

        [OperationContract]
        float GetFIDERating(int PID);

        [OperationContract]
        float GetCompetitionPoints(int PID);

        [OperationContract]
        void WriteLogLine(string User, int CID, string Module, string Level, int LevelValue, string LogLine);

        [OperationContract]
        bool PlayerHasResults(int CID, int PID);

        [OperationContract]
        bool DeletePlayerCompetitionRecord(int CID, int PID);

        [OperationContract]
        bool Remove_BlitzResults(int CID, int RNR);

        [OperationContract]
        bool AddPlayerBlitzResultInit(int CID, int RNR, int PID);

        [OperationContract]
        bool UpdateOneBlitzResult(BlitzResultData OneBlitzResult);

        [OperationContract]
        DataSet GetPlayersBlitz(int CID, int RNR);

        [OperationContract]
        float GetClubBlitzRating(int PID);

        [OperationContract]
        float GetKFactor(int CID);

        [OperationContract]
        float GetAcceleration(int CID);

        [OperationContract]
        bool SaveBlitzRating(int PID, float Rating, int CID, int RNR);

        [OperationContract]
        bool Remove_RatingRound(int CID, int RNR);

        [OperationContract]
        DataSet GetBlitzRatingRanking();

        [OperationContract]
        DataSet GetPlayersUniqueBlitz(int CID);

        [OperationContract]
        float GetBlitzPenaltyPointsCleaned(int CID, int PID);

        [OperationContract]
        float GetBlitzPenaltyPoints(int CID, int PID);

        [OperationContract]
        DataSet GetBlitzDisplayPoints(int CID, int PID);

        [OperationContract]
        int GetKNSBNumber(int PID);

        [OperationContractAttribute]
        string MakeImageSourceData(byte[] bytes);

        [OperationContractAttribute]
        string StringImage(int PID, int CID);

        [OperationContractAttribute]
        byte[] GetPlayerProfilePictureFile(int PID);

        [OperationContractAttribute]
        byte[] GetNoPictureFile(int CID);

        [OperationContractAttribute]
        DataSet GetTeamData(string Association);

        [OperationContractAttribute]
        DataSet GetTeamPlayers(string Association);

        [OperationContractAttribute]
        bool RemoveTeam(string VER, int Team_Record_Nummer);

        [OperationContractAttribute]
        bool UpdateTeam(string VER, int Team_nr, string URL, string Teamnaam, int Team_Record_Nummer);

        [OperationContractAttribute]
        bool AddTeam(string VER, string URL, string Teamnaam, int Team_nr);

        [OperationContractAttribute]
        int ValidateInteger(string Inputfield, bool emptyAllowed, int Default, bool Bounded, int Minimum, int Maximum, ref int InputValue);

        [OperationContractAttribute]
        int ValidateReal(string Inputfield, bool emptyAllowed, double Default, bool Bounded, double Minimum, double Maximum, ref double InputValue);

        [OperationContractAttribute]
        int ValidateString(string Inputfield, bool emptyAllowed, string Default, ref string InputValue);

        [OperationContractAttribute]
        bool Remove_Round_Information(int CID, int RNR);

        [OperationContractAttribute]
        int Get_Workflow_Item(string Item_Name, int CID, int RNR);

        [OperationContractAttribute]
        DataSet GetPlayersWithByeList(int CID);

        [OperationContractAttribute]
        DataSet GetRetentionStatistics(int CID, int RNR);

        [OperationContractAttribute]
        bool Update_National_Ratingtable(string Filename);

        [OperationContractAttribute]
        bool Update_FIDE_Ratingtable(string Filename);

        [OperationContractAttribute]
        DataSet GetUniquePlayers();

        [OperationContractAttribute]
        bool Update_KNSB_and_FIDE_Ratings(int PID, int KNSB, int FIDE);

        [OperationContractAttribute]
        bool PrepareBlitzRanking(int CID);

        [OperationContractAttribute]
        DataSet GetChampionsgroupSchedule(int CID);

        [OperationContractAttribute]
        int GetLastPlayerID();

        [OperationContractAttribute]
        bool AddGame(GamesData OneGame);

        [OperationContractAttribute]
        DataSet GetStateDescriptions(int TC);

        [OperationContractAttribute]
        DataSet GetCompetitionTypes(int TC);

        [OperationContractAttribute]
        int GetCompetitionType(int CID);

        [OperationContractAttribute]
        bool Remove_Work_Images(string workmap);

        [OperationContractAttribute]
        bool Remove_Obsolete_Players();

        [OperationContractAttribute]
        bool Remove_One_Competition(int CID);

        [OperationContractAttribute]
        bool Create_Backup(string BackUpString);

        [OperationContractAttribute]
        DataSet GetAllOccurrencesOfOneGame(int PID, int AID);

        [OperationContractAttribute]
        int UpdateAccessUser(string Session_Identification, string User);

        [OperationContractAttribute]
        int UpdateAccessNumber(string Session_Identification, string Manager_Identification, int UpdateNumber);

        [OperationContractAttribute]
        int UpdateAccessTimeStamp(string Session_Identification);

        [OperationContractAttribute]
        AccessData GetAccesData(string Session_Identification);

        [OperationContractAttribute]
        int Count_Sessions();

        [OperationContractAttribute]
        bool Update_Fontsize(int MID, int Fontsize);

        [OperationContractAttribute]
        int PurgeTableLogging();

        [OperationContractAttribute]
        int CTYtoRatingtype(int CTY);

        [OperationContractAttribute]
        DataSet GetAbsentRounds(int PID, int CompetitionId);

        [OperationContractAttribute]
        int GetIntegrateWithCompetition(int CID);

        [OperationContractAttribute]
        void Calculate_New_Pairing_Swiss(int CID, int RNR, int GPR);

        [OperationContractAttribute]
        bool Remove_NoShow_From_Deelnemer_Competition(int CID, int RNR);

        [OperationContractAttribute]
        DataSet GetResultsSwissGameList(int CID, int RNR);

        [OperationContractAttribute]
        void Create_Swiss_Result_Records(int CID, int RNR, int Games_Per_Round);

        [OperationContractAttribute]
        bool GetPlayerListAlphabeticalOverviewInit(int CompetitionId);

        [OperationContractAttribute]
        DataSet GetPlayerListAlphabeticalOverview(int CompetitionId);

        [OperationContractAttribute]
        bool DeletePlayerListAlphabeticalOverview(int CompetitionId);

        [OperationContractAttribute]
        bool UpdatePlayerListAlphabeticalOverview(int CompetitionId, int PID, String Mode, int YesNo);

        [OperationContractAttribute]
        void GenerateHeaderFile(DateTime Next_Evening, string textpart, string Name, int CID, int RNR, string RootPath);

        [OperationContractAttribute]
        int ChampionsGroupGameNumber(int WID, int BID, int CID);

        [OperationContractAttribute]
        DataSet GetOpenChampionsgroupGames(int WID, int CID);

        [OperationContractAttribute]
        int GetCGGamenrInThisRound(int PID, int CID, int RNR);

        [OperationContractAttribute]
        int GetCGPartnerInThisRound(int PID, int CID, int GameNr);

        [OperationContractAttribute]
        DataSet GetPlayerListCompetitionOnly(int CID);

        [OperationContractAttribute]
        bool UpdateChampionsGroupIndicator(int PID, int Indicator);

        [OperationContractAttribute]
        bool PlayerIsCG(int PID);

        [OperationContractAttribute]
        DataSet GetAlarmPanelData(string Manager);

        [OperationContractAttribute]
        int CountUnhandledCriticalAlarms();

        [OperationContractAttribute]
        bool UpdateAlarmPanelIndicator(int Status, int Recordnr);

        [OperationContractAttribute]
        bool SortMatrix(int CID);

        [OperationContractAttribute]
        DataSet GetCrossTableRow(int @CID, int @Row);

        [OperationContractAttribute]
        bool IPEntryDenied(string IPNumber, bool IsCrawler);

        [OperationContractAttribute]
        void WriteLogExceptionTrace(string User, int CID, string Module, string Level, int LevelValue, string TraceLog);

        [OperationContractAttribute]
        int GetCGTournamentIdFrom(int CID);

        [OperationContractAttribute]
        DataSet List_Free_Round_Players(int CID, int RNR);

        [OperationContractAttribute]
        void RemovePlayerStatusCalculated(int CID, int RNR);

        [OperationContractAttribute]
        void Update_Free_Round_Date(int PID);

        [OperationContractAttribute]
        bool UpdateMobileMessageIndicator(int Status, int Recordnr);

        [OperationContractAttribute]
        DataSet GetMobileMessageData(int CID);

        [OperationContractAttribute]
        int CountUnhandledMobileMessages(int CID);
   }
}