using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Show_One_Game_History : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "GameHistory";
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
                /*
                other initialising stuff for function
                */
                Fill_Texts();
            }
        }

        protected void Fill_Texts()
        {
            int PID = 0;
            int AID = 0;

            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);
            Master.DisplayAlarmPanelOnMasterPage.Visible = false;

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            Label1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();

            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();

            /*
            Other text filling actions
            */
            Label5.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            string ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], "TSGS_CS_Show_One_Game_History", "Info", 4, Request.QueryString.ToString() + " - " + ipAddress);

            if (Request.QueryString["Player1"] != null)
            {
                PID = Convert.ToInt16(Request.QueryString["Player1"]);
                AID = Convert.ToInt16(Request.QueryString["Player2"]);
            }
            Label3.Text = (string)Client_WCF.GetPlayerName(PID).Trim();
            Label4.Text = (string)Client_WCF.GetPlayerName(AID).Trim();
            Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], "TSGS_CS_Show_One_Game_History", "Info", 4, Label3.Text.Trim() + "-" + Label4.Text.Trim());
            DataSet ds = Client_WCF.GetAllOccurrencesOfOneGame(PID, AID);
            ViewState["DSList"] = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();

                GridViewRow row1Header = GridView1.HeaderRow;
                row1Header.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
                row1Header.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                row1Header.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
                row1Header.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                row1Header.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
                row1Header.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                row1Header.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
                row1Header.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                row1Header.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
                row1Header.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            }
            else
            {
                Master.SetErrorMessage(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim());
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
            }
            Client_WCF.Close();
            Client_MLC.Close();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt;
            int Gridline = e.Row.RowIndex + GridView1.PageSize * GridView1.PageIndex;
            dt = (DataTable)ViewState["DSList"];

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                e.Row.Cells[0].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[1].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[3].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.UpdateAccessNumber((string)Session.SessionID, (string)Session["Manager"], -1);
            Client_WCF.Close();
            Response.Redirect("https://www.schaakclub-dordrecht.nl");
            System.Environment.Exit(1);
        }

    }
}
