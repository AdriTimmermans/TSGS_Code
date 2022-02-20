using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TSGS_CS_Login_Client
{

    public partial class TSGS_ZS_Capture_Results : System.Web.UI.Page
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
                Session["Functionality"] = "ZSResults";
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
            ds = Client_WCF.GetResultsSwissGameList((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
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
            int GamesPerRound = (int)Session["GamesPerRound"];
            string resultstring = "";
            TextBox tb;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList Result = (DropDownList)e.Row.FindControl("Result");
                TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

                Result.Items.Add(new ListItem(".. - .."));
                resultstring = String.Format("{0:##0.0}", GamesPerRound) + " -  0.0";
                Result.Items.Add(new ListItem(resultstring));
                for (int i = 1; i <= GamesPerRound * 2; i++)
                {
                    resultstring = String.Format("{0:##0.0}", (GamesPerRound - i * 0.5)) + " - " + String.Format("{0:##0.0}", i * 0.5);
                    Result.Items.Add(new ListItem(resultstring));
                }
                resultstring = "R " + String.Format("{0:##0.0}", GamesPerRound) + " -  0.0";
                Result.Items.Add(new ListItem(resultstring));
                for (int i = 1; i <= GamesPerRound * 2; i++)
                {
                    resultstring = "R " + String.Format("{0:##0.0}", (GamesPerRound - i * 0.5)) + " - " + String.Format("{0:##0.0}", i * 0.5);
                    Result.Items.Add(new ListItem(resultstring));
                }

                tb = (TextBox)e.Row.FindControl("ResultValue");
                aux = ResultToResultFromList(Convert.ToInt16(tb.Text));
                Result.SelectedIndex = aux;

                Client_MLC.Close();
            }
        }

        protected int ResultFromListToResult(int ResultFromList)
        {
            return ResultFromList;
        }

        protected int ResultToResultFromList(int Result)
        {
            return Result;
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
            int PID_White = (int) dtG.Rows[GameNr]["PID_Wit"];
            int PID_Black = (int) dtG.Rows[GameNr]["PID_Zwart"];

            Session["BreakCount"] = 0;
            Master.ErrorMessageVisibility(false);
            Client_WCF.UpdateResult(PID_White, PID_Black, (int)Session["Competition_Identification"], (int)Session["Round_Number"], auxR);

            Fill_Texts();
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            if (Client_WCF.AllResultsEntered((int)Session["Competition_Identification"], (int)Session["Round_Number"]))
            {
                Client_WCF.DeleteResults((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                Client_WCF.Create_Swiss_Result_Records((int)Session["Competition_Identification"], (int)Session["Round_Number"], (int)Session["GamesPerRound"]);
/*                Client_WCF.AdministrationRatingData((int)Session["Competition_Identification"], (int)Session["Round_Number"]); */
                Client_WCF.Update_Workflow_Item("[Resultaten Verwerken]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
                Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], 6);
//                Session["Current_Status"] = 6;
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