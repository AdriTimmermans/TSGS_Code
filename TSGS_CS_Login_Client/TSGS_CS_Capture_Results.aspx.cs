using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Capture_Results : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
                Session["Functionality"] = "Results";
                Master.SetErrorMessageGreen();
                Master.ErrorMessageVisibility(false);
                Fill_Texts();
            }
        }

        protected void Fill_Texts()
        {

            DataSet ds = new DataSet();

            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;

            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            GridView1.Font.Name = "Arial";
            GridView1.Font.Size = (int)Session["Fontsize"];
            ds = Client_WCF.GetResultsGameList((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
            GridView1.DataSource = ds;

            GridView1.AllowPaging = true;
            GridView1.PageSize = 15;
            GridView1.DataBind();
            GridView1.SelectedIndex = -1;

            GridViewRow rowHeader = GridView1.HeaderRow;
            rowHeader.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            rowHeader.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();

            ViewState["Games"] = ds.Tables[0];

            Client_WCF.Close();
            Client_MLC.Close();

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int aux = 0;
            TextBox tb;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList Result = (DropDownList)e.Row.FindControl("Result");
                TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

                Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], "CorrectResults", (int)Session["Language"], 13).Trim()));
                Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], "CorrectResults", (int)Session["Language"], 14).Trim()));
                Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], "CorrectResults", (int)Session["Language"], 15).Trim()));
                Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], "CorrectResults", (int)Session["Language"], 16).Trim()));
                Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], "CorrectResults", (int)Session["Language"], 17).Trim()));
                Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], "CorrectResults", (int)Session["Language"], 18).Trim()));
                Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], "CorrectResults", (int)Session["Language"], 19).Trim()));
                Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], "CorrectResults", (int)Session["Language"], 20).Trim()));
                Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], "CorrectResults", (int)Session["Language"], 21).Trim()));
                Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], "CorrectResults", (int)Session["Language"], 22).Trim()));
                Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], "CorrectResults", (int)Session["Language"], 23).Trim()));

                tb = (TextBox)e.Row.FindControl("ResultValue");
                aux = ResultToResultFromList(Convert.ToInt16(tb.Text));
                Result.SelectedIndex = aux;

                Client_MLC.Close();
            }
        }

        protected bool ValidCombination(int Result, int GameType)
        {
            bool aux = true;
            switch (GameType)
            {
                case 5:
                    {
                        aux = (new[] { 0, 5, 6, 7, 8 }).Contains(Result);
                        break;
                    }
                case 4:
                    {
                        aux = (new[] { 0, 1, 2, 3, 9, 10, 11 }).Contains(Result);
                        break;
                    }
                case 8:
                    {
                        aux = (new[] { 0, 1, 2, 3, 9, 10, 11 }).Contains(Result);
                        break;
                    }
            }

            return aux;
        }

        protected int ResultFromListToResult(int ResultFromList)
        {
            switch (ResultFromList)
            {
                case 0: return 0;
                case 1: return 1;
                case 2: return 2;
                case 3: return 3;
                case 4: return 5;
                case 5: return 6;
                case 6: return 7;
                case 7: return 8;
                case 8: return 9;
                case 9: return 10;
                case 10: return 11;
            }
            return 0;
        }

        protected int ResultToResultFromList(int Result)
        {
            switch (Result)
            {
                case 0: return 0;
                case 1: return 1;
                case 2: return 2;
                case 3: return 3;
                case 5: return 4;
                case 6: return 5;
                case 7: return 6;
                case 8: return 7;
                case 9: return 8;
                case 10: return 9;
                case 11: return 10;
            }
            return 0;
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Fill_Texts();
        }

        protected void DDL_SelectedChange(object sender, EventArgs e)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataTable dtG = (DataTable)ViewState["Games"];

            GridViewRow row = ((GridViewRow)((Control)sender).Parent.Parent);
            int i = row.RowIndex;
            int GameNr = GridView1.PageIndex * GridView1.PageSize + i;

            DropDownList DDlR = (DropDownList)GridView1.Rows[i].FindControl("Result");


            int auxR = ResultFromListToResult(DDlR.SelectedIndex);
            int auxT = Convert.ToInt16(dtG.Rows[GameNr]["WedstrijdType"].ToString());
            int PID_White = Convert.ToInt16(dtG.Rows[GameNr]["PID_Wit"].ToString());
            int PID_Black = Convert.ToInt16(dtG.Rows[GameNr]["PID_Zwart"].ToString());

            if (ValidCombination(auxR, auxT))
            {
                Session["BreakCount"] = 0;
                Master.ErrorMessageVisibility(false);
                Client_WCF.UpdateResult(PID_White, PID_Black, (int)Session["Competition_Identification"], (int)Session["Round_Number"], auxR);
            }
            else
            {
                Master.SetErrorMessage(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim());
                Master.ErrorMessageVisibility(true);
                Master.SetErrorMessageRed();
            }
            Fill_Texts();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            if (Client_WCF.AllResultsEntered((int)Session["Competition_Identification"], (int)Session["Round_Number"]))
            {
                Client_WCF.DeleteResults((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                Client_WCF.Create_Result_Records((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                Client_WCF.AdministrationRatingData((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                Client_WCF.Upgrade_ChampionsgroupPoints((int)Session["Competition_Identification"]);
                Client_WCF.Update_Workflow_Item("[Resultaten Verwerken]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
                Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], 6);
                Session["Current_Status"] = 6;
                String TextPart = Client_MLC.GetMLCText((string)Session["Project"], "HeaderUpdate", (int)Session["Language"], 5).Trim();
                string RootPath = System.Web.HttpContext.Current.Server.MapPath("~/htmlfiles/" + Convert.ToString((int)Session["Competition_Identification"]).Trim());
                Client_WCF.GenerateHeaderFile(DateTime.Now, TextPart, (string)Session["Competition"], (int)Session["Competition_Identification"], (int)Session["Round_Number"], RootPath);
                Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 4, "Closed");
                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }
            else
            {

                if ((int)Session["BreakCount"] < 1)
                {
                    Master.SetErrorMessage(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim());
                    Master.ErrorMessageVisibility(true);
                    Master.SetErrorMessageRed();
                    Session["BreakCount"] = (int)Session["BreakCount"] + 1;
                }
                else
                {
                    Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 4, "Closed");
                    Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
                }
            }
            Client_WCF.Close();
            Client_MLC.Close();
        }
    }
}