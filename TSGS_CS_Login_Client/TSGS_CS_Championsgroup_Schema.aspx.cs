using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

namespace TSGS_CS_Login_Client
{

    public partial class TSGS_CS_Championsgroup_Schema : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "CGSchema";
            Session["LastFunction"] = HttpContext.Current.Request.Url.AbsolutePath;

            if (IsPostBack)
            {
                int aux = TSGS_CS_Extention_Methods.GetLanguageCode(TSGS_CS_Extention_Methods.GetPostBackControlId(this));
                if (aux >= 1)
                {
                    Session["Language"] = aux;
                    Fill_Texts();
                }
            }
            else
            {
                Master.SetErrorMessageGreen();
                Master.ErrorMessageVisibility(false);
                Label1.Visible = false;
                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                DataSet ds = Client_WCF.GetChampionsgroupPlayerList((int)Session["Competition_Identification"]);
                Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], "TSGS_CS_Championsgroup_Schema", "Workflow", 2, "Started");
                Client_WCF.Close();
                ViewState["Players"] = ds;
                Fill_Texts();
              
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox tbLotingnummer = (TextBox)e.Row.FindControl("tbLotingnummer");
                tbLotingnummer.Attributes.Add("onkeypress", "javascript:return clickButton(event);");
            }
        }

        protected void Fill_Texts()
        {

            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            Label1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();

            GridView1.Font.Name = "Arial";
            GridView1.Font.Size = (int)Session["Fontsize"];
            DataSet ds = (DataSet)ViewState["Players"];

            GridView1.DataSource = ds;
            GridView1.DataBind();


            GridViewRow rowHeader = GridView1.HeaderRow;
            rowHeader.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            rowHeader.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            rowHeader.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            rowHeader.Cells[2].HorizontalAlign = HorizontalAlign.Left;

            /*
            Other text fiilling actions
            */
            Client_WCF.Close();
            Client_MLC.Close();
        }

        protected void tbLotingnummer_TextChanged(object sender, EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            TextBox tb = (TextBox)sender;
            int Input_Lotingnummer = 0;
            GridViewRow gvr = (GridViewRow)tb.NamingContainer;
            int rowindex = gvr.RowIndex;
            int i = rowindex;
            int error = Client_WCF.ValidateInteger(tb.Text.Trim(), true, 0, true, 1, 999, ref Input_Lotingnummer);
            if (error == 0)
            {
                Master.ErrorMessageVisibility(false);
                tb.BackColor = System.Drawing.Color.White;
                GridViewRow row = GridView1.Rows[rowindex];
                DataSet ds = (DataSet)ViewState["Players"];
                ds.Tables[0].Rows[i]["Lotnumber"] = Input_Lotingnummer;
                ds.AcceptChanges();
                ViewState["Players"] = (DataSet)ds;
                Fill_Texts();
            }
            else
            {
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
                Master.SetErrorMessage(tb.Text.Trim() + ": " + Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], error + 1).Trim());
                tb.BackColor = System.Drawing.Color.Red;
                Client_MLC.Close();
            }
            Client_WCF.Close();

        }

        protected int GetPID (int PlayerNr)
        {
            int aux = 0;
            DataSet ds = (DataSet)ViewState["Players"];
            for (int i = 0; i < ds.Tables[0].Rows.Count;i++)
            {
                if ((int)ds.Tables[0].Rows[i]["Lotnumber"] == PlayerNr)
                {
                    aux = (int)ds.Tables[0].Rows[i]["Speler_ID"];
                }
            }
            return aux;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.Remove_RoundRobin((int)Session["Competition_Identification"]);
            TSGS_CS_WCF_Service.ChampionsgroupData OnePlayer = new TSGS_CS_WCF_Service.ChampionsgroupData();
            Label1.Visible = true;
            DataSet ds = (DataSet)ViewState["Players"];
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                OnePlayer.Player_Id = (int)ds.Tables[0].Rows[i]["Speler_ID"];
                OnePlayer.Competition_Id = (int)Session["Competition_Identification"];
                OnePlayer.LotNumber = (int)ds.Tables[0].Rows[i]["Lotnumber"];
                OnePlayer.StartCompetitionPoints = 0;
                OnePlayer.StartRating = 0;

                Client_WCF.UpdateChampionsGroupPlayer(OnePlayer);
            }

            int Game_Number = 0;
            int Max_Number = (int)ds.Tables[0].Rows.Count;
            int CID = (int)Session["Competition_Identification"];
            int StartNumber = 0;
            int SecondNumber = 0;
            int PID = 0;
            int ADV = 0;
            /*
             * 
             * Alternatively Berger tables,[13] named after the Austrian chess master Johann Berger, are widely used in the planning of tournaments. Berger published the pairing tables in his two Schachjahrbucher,[14][15] with due reference to its inventor Richard Schurig.[16][17]
            Round 1. 	1–14 	2–13 	3–12 	4–11 	5–10 	6–9 	7–8
            Round 2. 	14–8 	9–7 	10–6 	11–5 	12–4 	13–3 	1–2
            Round 3. 	2–14 	3–1 	4–13 	5–12 	6–11 	7–10 	8–9
            ... 	…
            Round 13. 	7–14 	8–6 	9–5 	10–4 	11–3 	12–2 	13–1

            This constitutes a schedule where player 14 has a fixed position, and all other players are rotated clockwise n 2 {\displaystyle {\begin{matrix}{\frac {n}{2}}\end{matrix}}} \begin{matrix} \frac{n}{2}\end{matrix} positions. This schedule alternates colours and is easily generated manually. To construct the next round, the last player, number 8 in the first round, moves to the head of the table, followed by player 9 against player 7, player 10 against 6, until player 1 against player 2. Arithmetically, this equates to adding n 2 {\displaystyle {\begin{matrix}{\frac {n}{2}}\end{matrix}}} \begin{matrix} \frac{n}{2}\end{matrix} to the previous row, with the exception of player n {\displaystyle n} n. When the result of the addition is greater than ( n − 1 ) {\displaystyle (n-1)} (n-1), then subtract ( n 2 − 1 ) {\displaystyle ({\begin{matrix}{\frac {n}{2}}\end{matrix}}-1)} ({\begin{matrix}{\frac {n}{2}}\end{matrix}}-1).
             */
            for (int R = 1; R <= Max_Number - 1; R++ )
            {
                Game_Number++;
                if ((R % 2) == 0)
                {
                    StartNumber = Max_Number / 2 + R / 2;
                    Client_WCF.AddPlayerRoundRobinRecord(CID, R, Game_Number, GetPID(Max_Number), GetPID(StartNumber), 1);
                    Client_WCF.AddPlayerRoundRobinRecord(CID, R, Game_Number, GetPID(StartNumber), GetPID(Max_Number), -1);
                }
                else
                {
                    StartNumber = (R+1)/2;
                    Client_WCF.AddPlayerRoundRobinRecord(CID, R, Game_Number, GetPID(StartNumber), GetPID(Max_Number), 1);
                    Client_WCF.AddPlayerRoundRobinRecord(CID, R, Game_Number, GetPID(Max_Number), GetPID(StartNumber), -1);
                }
                SecondNumber = StartNumber;
                for (int B = 2; B <= Max_Number/2; B++)
                {
                    Game_Number++;
                    StartNumber = StartNumber + 1; 
                    if (StartNumber == Max_Number)
                    {
                        StartNumber = 1;
                    }

                    SecondNumber = SecondNumber - 1;
                    if (SecondNumber == 0)
                    {
                        SecondNumber = Max_Number-1;
                    }


                    PID = StartNumber;
                    ADV = SecondNumber;
                    Client_WCF.AddPlayerRoundRobinRecord(CID, R, Game_Number, GetPID(PID), GetPID(ADV), 1);
                    Client_WCF.AddPlayerRoundRobinRecord(CID, R, Game_Number, GetPID(ADV), GetPID(PID), -1);
                }
            }


            Session["Current_Status"] = 22;
            Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], 22);
            Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 4, "Closed");
            Client_WCF.Close(); 
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 4, "Closed");
            Client_WCF.Close();
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

    }
}