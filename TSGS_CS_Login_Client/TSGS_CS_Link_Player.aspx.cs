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

    public partial class TSGS_CS_Link_Player : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "LinkPlayer";
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
                /*
                other initialising stuff for function <xxxx>
                */

                Fill_Texts();
            }
        }

        protected void Fill_Texts()
        {

            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataSet ds = Client_WCF.GetAllOtherPlayersAlphabetical((int)Session["Competition_Identification"]);
            if (ds.Tables[0].Rows.Count == 0)
            {
                Master.SetErrorMessageGreen();
                Master.SetErrorMessage(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim());
                Master.ErrorMessageVisibility(true);
            }
            else
            {
                Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
                Master.ErrorMessageVisibility(false);

                GridView1.DataSource = ds;
                GridView1.AllowPaging = true;

                GridView1.PageSize = 15;
                GridView1.DataBind();
                GridView1.Font.Name = "Arial";
                GridView1.Font.Size = (int)Session["Fontsize"];
                GridViewRow rowHeader = GridView1.HeaderRow;
                GridView1.GridLines = GridLines.Horizontal;

                GridView1.SelectedIndex = -1;

                GridViewRow row1Header = GridView1.HeaderRow;
                row1Header.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
                row1Header.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                row1Header.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
                row1Header.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                /*
                Other text fiilling actions
                */
            }
            DataSet dsP = Client_WCF.GetPlayerList((int)Session["Competition_Identification"]);
            if (dsP.Tables[0].Rows.Count == 0)
            {
                Master.SetErrorMessageGreen();
                Master.SetErrorMessage(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"],8).Trim());
                Master.ErrorMessageVisibility(true);
            }
            else
            {
                GridView2.DataSource = dsP;
                GridView2.AllowPaging = true;

                GridView2.PageSize = 15;
                GridView2.DataBind();
                GridView2.Font.Name = "Arial";
                GridView2.Font.Size = (int)Session["Fontsize"];
                GridViewRow rowHeader2 = GridView2.HeaderRow;
                GridView2.GridLines = GridLines.Horizontal;

                GridView2.SelectedIndex = -1;

                GridViewRow row2Header = GridView2.HeaderRow;
                row2Header.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
                row2Header.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                row2Header.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
                row2Header.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                /*
                Other text fiilling actions
                */
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Fill_Texts();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                e.Row.Cells[0].Text = Convert.ToString(dr["Speler_ID"]);
                e.Row.Cells[1].Text = Convert.ToString(dr["SpelerNaam"]);
                e.Row.Cells[1].Font.Size = 12;
                e.Row.Cells[2].Font.Size = 12;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
            }
        }

        protected void OnLinkPlayerChanged(object sender, EventArgs e)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            GridViewRow row = ((GridViewRow)((Control)sender).Parent.Parent);
            int i = row.RowIndex;
            GridView1.SelectedIndex = -1;
            CheckBox cbLinkedPlayer = (System.Web.UI.WebControls.CheckBox)GridView1.Rows[i].FindControl("cbLinkPlayer");

            Master.ErrorMessageVisibility(false);
            TSGS_CS_WCF_Service.AbsenteeData AbsenteeData = new TSGS_CS_WCF_Service.AbsenteeData();

            int PID = System.Convert.ToInt32(row.Cells[0].Text);
            int CID = (int)Session["Competition_Identification"];


            float CPS = Client_WCF.GetCompetitionPoints(PID);
            float ELO = Client_WCF.GetClubRating(PID);
            Client_WCF.AddPlayerCompetitionRecord(CID, PID, CPS, ELO, (int)Session["Competitie_Type"]);
            Fill_Texts();
            Client_MLC.Close();
            Client_WCF.Close();
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
            Fill_Texts();
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                e.Row.Cells[0].Text = Convert.ToString(dr["Speler_ID"]);
                e.Row.Cells[1].Text = Convert.ToString(dr["SpelerNaam"]);
                e.Row.Cells[1].Font.Size = 12;
                e.Row.Cells[2].Font.Size = 12;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                CheckBox cbUnLink = (CheckBox)e.Row.FindControl("cbUnLinkPlayer");
                cbUnLink.Checked = false;
                cbUnLink.Enabled = Client_WCF.PlayerHasResults((int)Session["Competition_Identification"], (int)dr["Speler_ID"]);
                if (!cbUnLink.Enabled)
                {
                    cbUnLink.Visible = false;
                }
            }
        }

        protected void OnUnLinkPlayerChanged(object sender, EventArgs e)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            GridViewRow row = ((GridViewRow)((Control)sender).Parent.Parent);
            int i = row.RowIndex;
            GridView2.SelectedIndex = i;
            CheckBox cbUnLinkedPlayer = (System.Web.UI.WebControls.CheckBox)GridView2.Rows[i].FindControl("cbUnLinkPlayer");

            Master.ErrorMessageVisibility(false);
            TSGS_CS_WCF_Service.AbsenteeData AbsenteeData = new TSGS_CS_WCF_Service.AbsenteeData();

            int PID = System.Convert.ToInt32(row.Cells[0].Text);
            int CID = (int)Session["Competition_Identification"];
            Client_WCF.DeletePlayerCompetitionRecord(CID, PID);

            Fill_Texts();
            Client_MLC.Close();
            Client_WCF.Close();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if ((string)Session["CallingFunction"] == "Blitz")
            {
                Response.Redirect("TSGS_CS_Non_Clubcompetition_Players.aspx");
            }
            else
            {
                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

    }
}