using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Competition_System_System_Monitor : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "SystemMonitor";
            Session["LastFunction"] = HttpContext.Current.Request.Url.AbsolutePath;
            if (IsPostBack)
            {
                int aux = TSGS_CS_Extention_Methods.GetLanguageCode(TSGS_CS_Extention_Methods.GetPostBackControlId(this));
                if (aux >= 1)
                {
                    Session["Language"] = aux;
                }
            }
            else
            {
                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], "Competition Control Panel", "Workflow", 2, "Visible");
                Client_WCF.Close();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            if ((int)Session["Current_Status"] > 2)
            {
                Master.ErrorMessageVisibility(true);
                Master.SetErrorMessage(Session["Club"] + ", " + Session["Competition"] + " - " +
                        string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 101).Trim(), (int)Session["Round_Number"]) + ",  " +
                        Client_MLC.GetMLCText((string)Session["Project"], "StateDescription", (int)Session["Language"], (int)Session["Current_Status"]).Trim());
            }
            else
            {
                Master.ErrorMessageVisibility(false);
            }
            HttpBrowserCapabilities bc = Request.Browser;
            if (bc.Browser != "Firefox")
            {
                Master.SetErrorMessage(Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 12).Trim());
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                Client_WCF.WriteLogLine((string)Session["Manager"], 0, "Master", "General", 2, "Use Firefox, otherwise unpredictable results");
            }
            Fill_Texts();
            Client_MLC.Close();
            Client_WCF.Close();
        }

        protected void PrepareButton(string FunctionDescription, Button Btn, Label Lab)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Lab.Text = Btn.Text;
            if (Client_WCF.ButtonAllowed(FunctionDescription, (int)Session["Current_Status"], (int)Session["UserPrivileges"])== 0)
            {
                Btn.Enabled = false;
                Btn.Visible = false;
                Lab.Visible = true;
                if (FunctionDescription == "<xxxxx>")
                {
                    Lab.BackColor = System.Drawing.Color.PaleVioletRed;
                }
            }
            else
            {
                Btn.Enabled = true;
                Btn.BackColor = System.Drawing.Color.LightGray;
                Btn.Visible = true;
                Lab.Visible = false;
            }
            Client_WCF.Close();
        }

        public void Fill_Texts()
        {
            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            Label202.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 102).Trim();
            Label202.Font.Size = (int)Session["Fontsize"] + 2;
            Label202.Font.Bold = true;
            Label203.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 103).Trim();
            Label203.Font.Size = (int)Session["Fontsize"] + 2;
            Label203.Font.Bold = true;
            Label204.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 104).Trim();
            Label204.Font.Size = (int)Session["Fontsize"] + 2;
            Label204.Font.Bold = true;
            Label205.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 105).Trim();
            Label205.Font.Size = (int)Session["Fontsize"] + 2;
            Label205.Font.Bold = true;
            Label206.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 106).Trim();
            Label206.Font.Size = (int)Session["Fontsize"] + 2;
            Label206.Font.Bold = true;
            Label207.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 107).Trim();
            Label207.Font.Size = (int)Session["Fontsize"] + 2;
            Label207.Font.Bold = true;
            Label208.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 108).Trim();
            Label208.Font.Size = (int)Session["Fontsize"] + 2;
            Label208.Font.Bold = true;

            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 2).Trim();
            PrepareButton("Login", Button2, Label102);

            Button3.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            PrepareButton("OverallStatus", Button3, Label103);

            Button5.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            PrepareButton("SelectCompetition", Button5, Label105);

            Button6.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            PrepareButton("AddPlayer", Button6, Label106);

            Button7.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            PrepareButton("CorrectPlayer", Button7, Label107);

            Button8.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            PrepareButton("Absentees", Button8, Label108);

            Button9.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            PrepareButton("InitNextRound", Button9, Label109);

            Button10.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            PrepareButton("GeneratePairing", Button10, Label110);

            Button11.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
            PrepareButton("DisplayGames", Button11, Label111);

            Button12.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim();
            PrepareButton("CorrectPairing", Button12, Label112);

            Button13.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 13).Trim();
            PrepareButton("ReadResults", Button13, Label113);

            Button14.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 14).Trim();
            PrepareButton("DisplayCL", Button14, Label114);

            Button15.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim();
            PrepareButton("DisplayWVL", Button15, Label115);

            Button16.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 16).Trim();
            PrepareButton("DisplayELOL", Button16, Label116);

            Button17.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 17).Trim();
            PrepareButton("DisplayScores", Button17, Label117);

            Button18.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 18).Trim();
            PrepareButton("Logout", Button18, Label118);

            Button19.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 20).Trim();
            PrepareButton("CorrectRoundData", Button19, Label119);

            Button20.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 21).Trim();
            PrepareButton("InitCom", Button20, Label120);

            Button21.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 22).Trim();
            PrepareButton("Params", Button21, Label121);

            Button22.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 25).Trim();
            PrepareButton("CGOverview", Button22, Label122);

            Button23.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 24).Trim();
            PrepareButton("CGSchema", Button23, Label123);

            Button24.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 23).Trim();
            PrepareButton("<xxxxx>", Button24, Label124);

            Button25.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 26).Trim();
            PrepareButton("Blitz", Button25, Label125);

            Button26.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 27).Trim();
            PrepareButton("DisplayBlitzResults", Button26, Label126);

            Button27.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 28).Trim();
            PrepareButton("BlitzResults", Button27, Label127);

            Button28.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 29).Trim();
            PrepareButton("DisplayBlitzComp", Button28, Label128);

            Button29.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 30).Trim();
            PrepareButton("DisplayBlitzRanking", Button29, Label129);

            Button30.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 31).Trim();
            PrepareButton("Stats", Button30, Label130);

            Button31.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 32).Trim();
            PrepareButton("<xxxxx>", Button31, Label131);

            //Button32.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 33).Trim();
            //PrepareButton("<xxxxx>", Button32, Label132);

            Button33.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 34).Trim();
            PrepareButton("ManageTeams", Button33, Label133);

            Button34.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 35).Trim();
            PrepareButton("Byes", Button34, Label134);

            Button35.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 36).Trim();
            PrepareButton("HeaderUpdate", Button35, Label135);

            Button36.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 37).Trim();
            PrepareButton("PreviousRound", Button36, Label136);

            Button37.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 38).Trim();
            PrepareButton("LinkPlayer", Button37, Label137);

            Button38.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 39).Trim();
            PrepareButton("Auto", Button38, Label138);

            Button39.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 46).Trim();
            PrepareButton("Cleanup", Button39, Label139);

            Button40.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 41).Trim();
            PrepareButton("KNSBFIDE", Button40, Label140);

            Button42.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 43).Trim();
            PrepareButton("Close", Button42, Label142);

            Button43.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 44).Trim();
            PrepareButton("DisplayPlayers", Button43, Label143);

            Button44.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 45).Trim();
            PrepareButton("DisplayCGSchedule", Button44, Label144);

            Button45.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 73).Trim();
            PrepareButton("DefaultAbsence", Button45, Label145);

            Button51.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 51).Trim();
            PrepareButton("ZSInschr", Button51, Label51);

            Button52.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 52).Trim();
            PrepareButton("ZSBind", Button52, Label52);

            Button53.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 53).Trim();
            PrepareButton("ZSRegister", Button53, Label53);

            Button54.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 54).Trim();
            PrepareButton("ZSByes", Button54, Label54);

            Button55.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 55).Trim();
            PrepareButton("ZSIndelen", Button55, Label55);

            Button56.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 56).Trim();
            PrepareButton("ZSParams", Button56, Label56);

            Button57.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 57).Trim();
            PrepareButton("ZSWait", Button57, Label57);

            Button58.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 58).Trim();
            PrepareButton("ZSReadRes", Button58, Label58);

/*            Button59.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 59).Trim();
            PrepareButton("ZSPairing", Button59, Label59);
*/
            Button60.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 60).Trim();
            PrepareButton("ZSCorPL", Button60, Label60);

            Button61.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 61).Trim();
            PrepareButton("ZSCorRes", Button61, Label61);

            Button62.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 62).Trim();
            PrepareButton("ZSCorParms", Button62, Label62);

            Button63.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 63).Trim();
            PrepareButton("ZSBar", Button63, Label63);

            Button64.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 64).Trim();
            PrepareButton("ZSDPlayers", Button64, Label64);

            Button65.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 65).Trim();
            PrepareButton("ZSDPairing", Button65, Label65);

            Button66.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 66).Trim();
            PrepareButton("ZSDList", Button66, Label66);

            Button67.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 67).Trim();
            PrepareButton("ZSDWV", Button67, Label67);

            Button68.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 68).Trim();
            PrepareButton("ZSDPSc", Button68, Label68);

            Button69.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 69).Trim();
            PrepareButton("ZSDPrizeLet", Button69, Label69);

            Button70.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 70).Trim();
            PrepareButton("ZSManual", Button70, Label70);

            Button71.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 71).Trim();
            PrepareButton("ZSDNorms", Button71, Label71);

            Button72.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 72).Trim();
            PrepareButton("ZSFIDERap", Button72, Label72);

            Client_MLC.Close();

        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_CS_Login_Page.aspx");
        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Competition_Overall_Status.aspx"); 
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Status_Line.aspx");
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Select_Competition.aspx");
        }
        protected void Button6_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); Response.Redirect("TSGS_CS_Player_Entry_Form.aspx");
        }
        protected void Button7_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Session["CorrectionMode"] = "Player";
            Response.Redirect("TSGS_CS_Correction_Player_Data.aspx");
        }
        protected void Button8_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Non_Clubcompetition_Players.aspx");
        }
        protected void Button9_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Initialise_Next_Round.aspx");
        }
        protected void Button10_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Generate_New_Pairing.aspx");
        }
        protected void Button11_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Display_Games.aspx");
        }
        protected void Button12_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Adjust_Pairing.aspx");
        }
        protected void Button13_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Capture_Results.aspx");
        }
        protected void Button14_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); Response.Redirect("TSGS_CS_Display_Competition_List.aspx");
        }
        protected void Button15_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Display_Gain_List.aspx");
        }
        protected void Button16_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Display_ELO_List.aspx");
        }
        protected void Button17_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Display_Personal_Scores_Selection.aspx");
        }
        protected void Button18_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Session["Competition_Identification"] = 0;
            Session["Round_Number"] = 0;
            Session["Club"] = "-";
            Session["Competition"] = "-";
            Session["Competition_Selection"] = 0;
            Session["Project"] = "CS";
            Session["Functionality"] = "SystemMonitor";
            Session["PlayerToUpdate"] = 0;
            Session["Current_Status"] = 2;
            Master.ErrorMessageVisibility(true);
            Master.Reset_Header();
            Master.SetErrorMessage(Session["Club"] + ", " + Session["Competition"] + " - ");
        }

        protected void Button19_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Session["CorrectionMode"] = "Results";
            Response.Redirect("TSGS_CS_Correction_Player_Data.aspx");
        }

        protected void Button20_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Session["CallingFunction"] = "NewEntry";
            Response.Redirect("TSGS_CS_Process_Tournament_Parameters.aspx");
        }
        protected void Button21_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Session["CallingFunction"] = "AdaptEntry";
            Response.Redirect("TSGS_CS_Process_Tournament_Parameters.aspx");
        }
        protected void Button22_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Championsgroup_Overview.aspx");
        }
        protected void Button23_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Championsgroup_Schema.aspx");
        }
        protected void Button24_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();            
            //            Response.Redirect("TSGS_CS_<xxx>.aspx"); 
        }
        protected void Button25_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Session["CallingFunction"] = "Blitz";
            Response.Redirect("TSGS_CS_Player_Entry_Form.aspx");
        }
        protected void Button26_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Display_Blitz_Results.aspx");
        }
        protected void Button27_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Blitz_Capture_Results.aspx");
        }
        protected void Button28_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            
            Response.Redirect("TSGS_CS_Display_Blitz_Competition_Ranking.aspx");
        }
        protected void Button29_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); Response.Redirect("TSGS_CS_Display_Blitz_ELO_List.aspx");
        }
        protected void Button30_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); Response.Redirect("TSGS_CS_Stats.aspx");
        }
        protected void Button31_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();            
            
            //            Response.Redirect("TSGS_CS_<xxx>.aspx"); 
        }
        //protected void Button32_Click(object sender, EventArgs e)
        //{
//            var button = (Button)sender;
//            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
//            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
//            Client_WCF.Close();
            //            Response.Redirect("TSGS_CS_<xxx>.aspx"); 
        //}
        protected void Button33_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Manage_TEAM_References.aspx");
        }

        protected void Button34_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Players_With_Bye_List.aspx");
        }
        protected void Button35_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Update_Header_Info.aspx");
        }
        protected void Button36_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Go_To_Previous_Round.aspx");
        }
        protected void Button37_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Link_Player.aspx");
        }
        protected void Button38_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Session["AutoOverview"] = true;
            Response.Redirect("TSGS_CS_Update_Header_Info.aspx");
        }
        protected void Button39_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Cleanup.aspx"); 
        }
        protected void Button40_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Update_Rating_Bonden.aspx"); 
        }
        protected void Button41_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();            //            Response.Redirect("TSGS_CS_<xxx>.aspx"); 
        }
        protected void Button42_Click(object sender, EventArgs e)
        {

            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.UpdateAccessNumber((string)Session.SessionID, (string)Session["Manager"], -1);
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, "Exit started", "Info", 4, "Started the process of closing the application");
            Client_WCF.PurgeTableLogging();
            Client_WCF.Close();
            Session.Clear();
            Session.Abandon();
            Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-10);
            Response.Redirect("https://www.schaakclub-dordrecht.nl");
            Session.Timeout = 1; 
            System.Environment.Exit(0);
        }
        protected void Button43_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Display_Participant_List.aspx"); 
        }
        protected void Button44_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Display_Championsgroup_Schedule.aspx");
        }
        protected void Button45_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_CS_Default_Absence.aspx");
        }
        protected void Button51_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_CS_Player_Entry_Form.aspx");
        }
        protected void Button52_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_CS_Link_Player.aspx");
        }
        protected void Button53_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_ZS_Attendance_Registration.aspx");
        }
        protected void Button54_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();            // Response.Redirect("TSGS_ZS_Byes.aspx");
            Response.Redirect("TSGS_CS_Non_Clubcompetition_Players.aspx");
        }
        protected void Button55_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_ZS_Pairing.aspx");
        }
        protected void Button56_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Session["CallingFunction"] = "NewEntry";
            Response.Redirect("TSGS_ZS_Tournament_Parameters.aspx");
        }
        protected void Button57_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_ZS_Waiting_for_Results.aspx");
        }
        protected void Button58_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_ZS_Capture_Results.aspx");
        }
        protected void Button59_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_ZS_Correction_Withdraw_Round.aspx");
        }
        protected void Button60_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_ZS_Correct_Player_Data.aspx");
        }
        protected void Button61_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_ZS_Correct_Results.aspx");
        }
        protected void Button62_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Session["CallingFunction"] = "AdaptEntry";
            Response.Redirect("TSGS_ZS_Tournament_Parameters.aspx");
        }
        protected void Button63_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_ZS_Pubtournament_Module.aspx");
        }
        protected void Button64_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_CS_Display_Participant_List.aspx"); 
        }
        protected void Button65_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_ZS_Display_Pairing_Bord.aspx");
        }
        protected void Button66_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_ZS_Display_List_Game_Points.aspx");
        }
        protected void Button67_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_ZS_Display_List_Gain_Loss.aspx");
        }
        protected void Button68_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_ZS_Display_Personal_Scores.aspx");
        }
        protected void Button69_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_ZS_Prepare_Prize_Letters.aspx");
        }
        protected void Button70_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_ZS_Correct_Pairing.aspx");
        }
        protected void Button71_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_ZS_Prepare_Norm_Documentation.aspx");
        }
        protected void Button72_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, button.Text, "Workflow", 2, "Started");
            Client_WCF.Close();
            Response.Redirect("TSGS_ZS_FIDE_Tournament_Report.aspx");
        }
    }
}