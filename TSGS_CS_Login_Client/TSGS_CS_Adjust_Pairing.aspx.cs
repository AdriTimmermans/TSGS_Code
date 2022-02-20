using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Adjust_Pairing : System.Web.UI.Page
    {
        private DataSet ds_White = new DataSet();
        private DataSet ds_Black = new DataSet();
        private DataSet ds = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "AdjustPairing";
            Session["LastFunction"] = HttpContext.Current.Request.Url.AbsolutePath;
            if (IsPostBack)
            {
                int aux = TSGS_CS_Extention_Methods.GetLanguageCode(TSGS_CS_Extention_Methods.GetPostBackControlId(this));
                if (aux >= 1)
                {
                    Session["Language"] = aux;
                    Fill_Texts();
                    TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
                }
            }
            else
            {
                DataSet ds_White = new DataSet();
                DataSet ds_Black = new DataSet();
                Session["White_ID"] = 0;
                Session["Black_ID"] = 0;
                Fill_Texts();
                PrepareDatasets(ds_White, ds_Black);
                Master.SetErrorMessageGreen();
                Master.ErrorMessageVisibility(false);
            }

        }

        protected void Fill_Texts()
        {

            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            Label3.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            Label4.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim();
            Label5.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            Label6.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            Label7.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 16).Trim();
            Label7.Visible = false;
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 13).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 14).Trim();
            Button3.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim();

            if (ViewState["Games"] != null)
            {
                DataTable dtG = (DataTable)ViewState["Games"];
                GridView1.DataSource = dtG;
            }
            else
            {
                ds = Client_WCF.GetGamesUpdateList((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                GridView1.DataSource = ds;
                ViewState["Games"] = ds.Tables[0];
            }

            GridView1.AllowPaging = true;
            GridView1.PageSize = 15;
            GridView1.Font.Name = "Arial";
            GridView1.Font.Size = (int)Session["Fontsize"];
            GridView1.GridLines = GridLines.Horizontal;
            GridView1.DataBind();
            GridView1.SelectedIndex = -1;

            GridViewRow rowHeader = GridView1.HeaderRow;
            rowHeader.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            rowHeader.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            rowHeader.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();

            GridViewRow rowHeader2 = GridView2.HeaderRow;
            if (rowHeader2 != null)
            {
                rowHeader2.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            }
            GridViewRow rowHeader3 = GridView3.HeaderRow;
            if (rowHeader3 != null)
            {
                rowHeader3.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            }
            Client_MLC.Close();

        }

        protected void PrepareDatasets(DataSet ds_White, DataSet ds_Black)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            DataTable dt_White = new DataTable("White");
            dt_White.Columns.Add("ID");
            dt_White.Columns.Add("Name");
            DataTable dt_Black = new DataTable("Black");
            dt_Black.Columns.Add("ID");
            dt_Black.Columns.Add("Name");
            dt_Black.Rows.Add(-5, Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim());
            dt_Black.Rows.Add(-3, Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim());
            dt_Black.Rows.Add(-1, Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim());
            ds_White.Tables.Add(dt_White);
            ds_Black.Tables.Add(dt_Black);

            GridView2.DataSource = ds_White;
            GridView2.AllowPaging = true;

            GridView2.PageSize = 10;
            GridView2.DataBind();
            GridView2.Font.Name = "Arial";
            GridView2.Font.Size = (int)Session["Fontsize"];
            GridView2.GridLines = GridLines.Horizontal;
            GridView2.SelectedIndex = -1;

            GridView3.DataSource = ds_Black;
            GridView3.AllowPaging = true;

            GridView3.PageSize = 10;
            GridView3.DataBind();
            GridView3.Font.Name = "Arial";
            GridView3.Font.Size = (int)Session["Fontsize"];
            GridView3.GridLines = GridLines.Horizontal;
            GridView3.SelectedIndex = -1;

            GridViewRow rowHeader3 = GridView3.HeaderRow;
            if (rowHeader3 != null)
            {
                rowHeader3.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            }

            Client_MLC.Close();

            ViewState["OverWhite"] = ds_White.Tables[0];
            ViewState["OverBlack"] = ds_Black.Tables[0];

        }

        protected void GridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // When user moves mouse over the GridView row,First save original or previous color to new attribute,
                // and then change it by magenta color to highlight the gridview row.

                foreach (DataControlFieldCell cell in e.Row.Cells)
                {
                    // check all cells in one row
                    foreach (Control control in cell.Controls)
                    {
                        // Must use LinkButton here instead of ImageButton
                        // if you are having Links (not images) as the command button.
                        ImageButton button = control as ImageButton;
                        if (button != null)
                        {

                            button.Attributes.Add("onmouseover", "return SelectIcon(this);");
                            button.Attributes.Add("onmouseout", "return UnselectIcon(this);");
                        }
                    }
                }

            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
            // rebuild datasets from gridview, than add and substract values from dataset followed by databinding
            //
            DataTable dtG = (DataTable)ViewState["Games"];
            DataTable dtW = (DataTable)ViewState["OverWhite"];
            DataRow drW = null;
            DataTable dtB = (DataTable)ViewState["OverBlack"];
            DataRow drB = null;

            GridViewRow row = GridView1.Rows[GridView1.SelectedIndex];
            Label aux = (Label)row.FindControl("PID_Wit");
            int PIDWhite = Convert.ToInt16(aux.Text);
            aux = (Label)row.FindControl("PID_Zwart");
            int PIDBlack = Convert.ToInt16(aux.Text);
            aux = (Label)row.FindControl("SpelerNaamWit");
            string NameWhite = aux.Text;
            aux = (Label)row.FindControl("SpelerNaamZwart");
            string NameBlack = aux.Text;

            drW = dtW.NewRow();
            drW["ID"] = PIDWhite;
            drW["Name"] = NameWhite;
            dtW.Rows.Add(drW);

            drB = dtB.NewRow();
            drB["ID"] = PIDWhite;
            drB["Name"] = NameWhite;
            dtB.Rows.Add(drB);

            if (PIDBlack > 0)
            {
                drW = dtW.NewRow();
                drW["ID"] = PIDBlack;
                drW["Name"] = NameBlack;
                dtW.Rows.Add(drW);

                drB = dtB.NewRow();
                drB["ID"] = PIDBlack;
                drB["Name"] = NameBlack;
                dtB.Rows.Add(drB);
            }

            int GamesRow = GridView1.PageIndex * GridView1.PageSize + GridView1.SelectedIndex;
            dtG.Rows[GamesRow].Delete();
            dtG.AcceptChanges();
            GridView1.DataSource = dtG;
            GridView1.DataBind();
            GridView2.DataSource = dtW;
            GridView2.DataBind();
            GridView3.DataSource = dtB;
            GridView3.DataBind();
            ViewState["Games"] = dtG;
            ViewState["OverWhite"] = dtW;
            ViewState["OverBlack"] = dtB;
            GridView1.SelectedIndex = -1;

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Fill_Texts();
        }

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtW = (DataTable)ViewState["OverWhite"];
            GridViewRow row = GridView2.Rows[GridView2.SelectedIndex];
            Label aux = (Label)row.FindControl("Name");
            Label aux2 = (Label)row.FindControl("ID");
            Session["White_ID"] = Convert.ToInt32(aux2.Text);
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            if (Client_WCF.IsPlayerInChampiongroup((int)Session["White_ID"]) && Client_WCF.IsPlayerInChampiongroup((int)Session["Black_ID"]))
            {
                cb_CGGame.Visible = true;
                Label7.Visible = true;
            }
            Client_WCF.Close();
            Label5.Text = aux.Text;
            Master.ErrorMessageVisibility(false);
            GridView2.DataSource = dtW;
            GridView2.DataBind();
        }

        protected void CB_CGGame_Search_GameNr(object sender, EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            if (cb_CGGame.Checked)
            {
                CGGameNr.Text = Client_WCF.ChampionsGroupGameNumber((int)Session["White_ID"], (int)Session["Black_ID"], (int)Session["Competition_Identification"]).ToString();
            }
            Client_WCF.Close();
        }
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dtW = (DataTable)ViewState["OverWhite"];
            GridView2.PageIndex = e.NewPageIndex;
            Fill_Texts();
            GridView2.DataSource = dtW;
            GridView2.DataBind();
            ViewState["OverWhite"] = dtW;
        }

        protected void GridView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtB = (DataTable)ViewState["OverBlack"];
            GridViewRow row = GridView3.Rows[GridView3.SelectedIndex];
            Label aux = (Label)row.FindControl("Name");
            Label aux2 = (Label)row.FindControl("ID");
            Session["Black_ID"] = Convert.ToInt32(aux2.Text);
            Label6.Text = aux.Text;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            if (Client_WCF.IsPlayerInChampiongroup((int)Session["White_ID"]) && Client_WCF.IsPlayerInChampiongroup((int)Session["Black_ID"]))
            {
                cb_CGGame.Visible = true;
                Label7.Visible = true;
            }
            Client_WCF.Close();
            Master.ErrorMessageVisibility(false);
            GridView3.DataSource = dtB;
            GridView3.DataBind();
        }

        protected void GridView3_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dtB = (DataTable)ViewState["OverBlack"];
            GridView3.PageIndex = e.NewPageIndex;
            Fill_Texts();
            GridView3.DataSource = dtB;
            GridView3.DataBind();
            ViewState["OverBlack"] = dtB;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            GridView2.SelectedIndex = -1;
            GridView3.SelectedIndex = -1;
            Label6.Text = "-";
            Label5.Text = "-";
            cb_CGGame.Checked = false;
            CGGameNr.Text = "";
            Master.ErrorMessageVisibility(false);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            if (GridView2.SelectedIndex >= 0 && GridView3.SelectedIndex >= 0)
            {
                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

                DataTable dtW = (DataTable)ViewState["OverWhite"];
                DataTable dtB = (DataTable)ViewState["OverBlack"];
                GridViewRow rowW = GridView2.Rows[GridView2.SelectedIndex];

                Label auxW = (Label)rowW.FindControl("ID");
                int PIDWhite = Convert.ToInt16(auxW.Text);
                GridViewRow rowB = GridView3.Rows[GridView3.SelectedIndex];
                Label auxB = (Label)rowB.FindControl("ID");
                int PIDBlack = Convert.ToInt16(auxB.Text);

                if (PIDWhite != PIDBlack)
                {
                    int GamesRow1 = GridView2.PageIndex * GridView2.PageSize + GridView2.SelectedIndex;
                    int GamesRow2 = GridView3.PageIndex * GridView3.PageSize + GridView3.SelectedIndex;
                    dtW.Rows[GamesRow1].Delete();
                    dtB.Rows[GamesRow1 + 3].Delete();
                    dtW.AcceptChanges();
                    dtB.AcceptChanges();
                    if (PIDBlack >= 0)
                    {
                        if (GamesRow1 > GamesRow2 - 3)
                        {
                            dtW.Rows[GamesRow2 - 3].Delete();
                            dtB.Rows[GamesRow2].Delete();
                        }
                        else
                        {
                            dtW.Rows[GamesRow2 - 4].Delete();
                            dtB.Rows[GamesRow2 - 1].Delete();
                        }
                    }
                    dtW.AcceptChanges();
                    dtB.AcceptChanges();
                    GridView2.DataSource = dtW;
                    GridView2.DataBind();
                    GridView3.DataSource = dtB;
                    GridView3.DataBind();
                    ViewState["OverWhite"] = dtW;
                    ViewState["OverBlack"] = dtB;
                    Label5.Text = "-";
                    Label6.Text = "-";
                    GridView2.SelectedIndex = -1;
                    GridView3.SelectedIndex = -1;
                    int ChampionsgroupGame = 0;
                    if (cb_CGGame.Checked)
                    {
                        ChampionsgroupGame = Convert.ToInt16(CGGameNr.Text);
                    }
                    //
                    // Add game to gamedatabase
                    //
                    Client_WCF.Remove_Games_From_List(PIDWhite, PIDBlack, (int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                    Client_WCF.AddManualGame(PIDWhite, PIDBlack, (int)Session["Competition_Identification"], (int)Session["Round_Number"], ChampionsgroupGame);
                    //
                    // Add new game to game list
                    //
                    DataTable dtG = (DataTable)ViewState["Games"];
                    DataRow drG = null;
                    drG = dtG.NewRow();
                    drG["PID_Wit"] = PIDWhite;
                    drG["PID_Zwart"] = PIDBlack;
                    drG["SpelerNaamWit"] = Client_WCF.GetPlayerName(PIDWhite);
                    drG["SpelerNaamZwart"] = Client_WCF.GetPlayerName(PIDBlack);
                    dtG.Rows.Add(drG);

                    GridView1.DataSource = dtG;
                    GridView1.DataBind();
                    ViewState["Games"] = dtG;

                    Client_WCF.Close();

                }
                else
                {
                    Master.ErrorMessageVisibility(true);
                    Master.SetErrorMessageRed();
                    Master.SetErrorMessage(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim());
                }
                DataSet ds_Black = new DataSet();
                Session["White_ID"] = 0;
                Session["Black_ID"] = 0;
                cb_CGGame.Checked = false;
                CGGameNr.Text = "";
                cb_CGGame.Visible = false;
                Label7.Visible = false;
                Client_MLC.Close();
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            if ((GridView2.Rows.Count == 0) & (GridView3.Rows.Count == 3))
            {
                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                Client_WCF.Update_Workflow_Item("[Handindeling maken]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
                Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 4, "Closed");
                Client_WCF.Close();
                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }
            else
            {
                Master.ErrorMessageVisibility(true);
                Master.SetErrorMessageRed();
                Master.SetErrorMessage(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim());
            }
            Client_MLC.Close();
        }
    }
}