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
    public partial class TSGS_CS_Setup_New_Competition : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "InitCom";
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
                Button2.Visible = false;
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

            Label1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();


            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            if (GridView1.Visible)
            {
                GridView1.Font.Name = "Arial";
                GridView1.Font.Size = (int)Session["Fontsize"];
                DataSet ds = Client_WCF.GetCompetitionList((string)Session["Manager"], (int)Session["Manager_Id"]);
                GridView1.DataSource = ds;
                GridView1.AllowPaging = true;

                GridView1.PageSize = 15;
                GridView1.DataBind();


                GridViewRow rowHeader = GridView1.HeaderRow;
                rowHeader.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], "SelecteerCompetitie", (int)Session["Language"], 5).Trim();
                rowHeader.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                rowHeader.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], "SelecteerCompetitie", (int)Session["Language"], 3).Trim();
                rowHeader.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                rowHeader.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], "SelecteerCompetitie", (int)Session["Language"], 4).Trim();
                rowHeader.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                rowHeader.Cells[1].Visible = false;
                rowHeader.Cells[4].Visible = false;
            }

            if (GridView2.Visible)
            {
                GridView2.Font.Name = "Arial";
                GridView2.Font.Size = (int)Session["Fontsize"];

                DataSet ds = (DataSet)ViewState["Players"];
                GridView2.DataSource = ds;
                GridView2.AllowPaging = true;

                GridView2.PageSize = 15;
                GridView2.DataBind();

                GridViewRow rowHeader = GridView2.HeaderRow;
                rowHeader.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
                rowHeader.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                rowHeader.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
                rowHeader.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                rowHeader.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
                rowHeader.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                rowHeader.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
                rowHeader.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                rowHeader.Cells[5].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
                rowHeader.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            }

            Client_MLC.Close();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Fill_Texts();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridView1.SelectedRow;
            Session["Parent_CID"] = System.Convert.ToInt32(row.Cells[1].Text);
            if ((int)Session["Competition_Identification"] == (int)Session["Parent_CID"])
            {
                TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
                Master.SetErrorMessageRed();
                Master.SetErrorMessage(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim());
                Master.ErrorMessageVisibility(true);
                Client_MLC.Close();
            }
            else
            {
                Master.ErrorMessageVisibility(false);
                GridView1.Visible = false;
                GridView2.Visible = true;
                Button2.Visible = true;
                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                DataSet ds = Client_WCF.GetPlayerListAlphabetical((int)Session["Parent_CID"]);
                ds.Tables[0].Columns.Add("Mee", typeof(bool));
                ViewState["Players"] = (DataSet)ds;
                Client_WCF.Close();
                Fill_Texts();

            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[4].Visible = false; 
                e.Row.Cells[2].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[3].Font.Size = (int)Session["Fontsize"];
                if (Convert.ToInt32(e.Row.Cells[1].Text) == (int)Session["Competition_Identification"])
                {
                    e.Row.BackColor = System.Drawing.Color.Pink;
                }
            }
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
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
                e.Row.Cells[2].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[3].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[4].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[5].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                Label lb = (Label)e.Row.FindControl("SpelerNaam");
                CheckBox cb_Overnemen = (CheckBox)e.Row.FindControl("cbOvernemen");
                CheckBox cb_Kroongroep = (CheckBox)e.Row.FindControl("cbKroongroep");
                CheckBox cb_SpelendLid = (CheckBox)e.Row.FindControl("cbSpelend");
                TextBox tb_Team = (TextBox)e.Row.FindControl("tbTeam");
                tb_Team.Attributes.Add("onkeypress", "javascript:return clickButton(event);");
                object value = dr["Mee"];
                if (value == DBNull.Value)
                {
                    cb_Overnemen.Checked = true;
                }
                else
                {
                    cb_Overnemen.Checked = Convert.ToBoolean(dr["Mee"]);
                }
                cb_Kroongroep.Checked = Convert.ToBoolean(dr["Member_Premier_Group"]);
                cb_SpelendLid.Checked = !Convert.ToBoolean(dr["Deelnemer_teruggetrokken"]);
                tb_Team.Text = Convert.ToString(dr["Team"]);
                lb.Text = Convert.ToString(dr["Spelernaam"]);
                lb.Font.Size = (int)Session["Fontsize"];
            }
        }

        protected void tbTeam_TextChanged (object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.NamingContainer;
            int rowindex = gvr.RowIndex;

            int i = rowindex + GridView2.PageSize * GridView2.PageIndex;
            GridViewRow row = GridView2.Rows[rowindex];
//            TextBox aux = (TextBox)row.FindControl("tbTeam"); 
            DataSet ds = (DataSet)ViewState["Players"];
            ds.Tables[0].Rows[i]["Team"] = Convert.ToSByte("0"+tb.Text.Trim());
            ds.AcceptChanges();
            ViewState["Players"] = (DataSet)ds;
            Fill_Texts();        
        }

        protected void cbKroongroep_Changed (object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)cb.NamingContainer;
            int rowindex = gvr.RowIndex;

            int i = rowindex + GridView2.PageSize * GridView2.PageIndex;
            GridViewRow row = GridView2.Rows[rowindex];
            DataSet ds = (DataSet)ViewState["Players"];
            ds.Tables[0].Rows[i]["Member_Premier_Group"] = 0;
            if (cb.Checked)
            { 
                ds.Tables[0].Rows[i]["Member_Premier_Group"] = 1; 
            }
            ds.AcceptChanges();
            ViewState["Players"] = (DataSet)ds;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.UpdateChampionsGroupIndicator((int)ds.Tables[0].Rows[i]["Speler_ID"], System.Convert.ToInt16(ds.Tables[0].Rows[i]["Member_Premier_Group"]));
            Client_WCF.Close();
            Fill_Texts();   

        }

        protected void cbSpelend_Changed(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)cb.NamingContainer;
            int rowindex = gvr.RowIndex;

            int i = rowindex + GridView2.PageSize * GridView2.PageIndex;
            GridViewRow row = GridView2.Rows[rowindex];
 //           CheckBox aux = (CheckBox)row.FindControl("cbSpelend");
            DataSet ds = (DataSet)ViewState["Players"];
            ds.Tables[0].Rows[i]["Deelnemer_teruggetrokken"] = 1;
            if (cb.Checked)
            {
                ds.Tables[0].Rows[i]["Deelnemer_teruggetrokken"] = 0;
            }

            ds.AcceptChanges();
            ViewState["Players"] = (DataSet)ds;
            Fill_Texts();   

        }
        protected void cbOvernemen_Changed(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)cb.NamingContainer;
            int rowindex = gvr.RowIndex;

            int i = rowindex + GridView2.PageSize * GridView2.PageIndex;
            GridViewRow row = GridView2.Rows[rowindex];
//            CheckBox aux = (CheckBox)row.FindControl("cbOvernemen");
            DataSet ds = (DataSet)ViewState["Players"];
            ds.Tables[0].Rows[i]["Mee"] = 0;
            if (cb.Checked)
            {
                ds.Tables[0].Rows[i]["Mee"] = 1;
            }
            ds.AcceptChanges();
            ViewState["Players"] = (DataSet)ds;
            Fill_Texts();   

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            bool savedata = false;

            if (!GridView1.Visible && !GridView2.Visible)
            {
                GridView1.Visible = true;
                Fill_Texts();
            }
            else
            {
                float aux = (float)0.0;
                Label1.Visible = true;
                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                Client_WCF.DeleteDeelnemerCompetitieRecords((int)Session["Competition_Identification"], (int)Session["Parent_CID"]);
                DataSet ds = (DataSet)ViewState["Players"];
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    object value = ds.Tables[0].Rows[i]["Mee"];
                    if (value == DBNull.Value)
                    {
                        savedata = true;
                    }
                    else
                    {
                        savedata = (bool)ds.Tables[0].Rows[i]["Mee"];
                    }
                    if (savedata)
                    {
                        aux = Client_WCF.GetPlayerRatingInCompetition((int)ds.Tables[0].Rows[i]["Speler_ID"], 1000, (int)Session["Parent_CID"]);
                        Client_WCF.AddPlayerCompetitionRecord((int)Session["Competition_Identification"], (int)ds.Tables[0].Rows[i]["Speler_ID"], aux, aux, (int)Session["Competitie_Type"]);
                        Client_WCF.UpdatePlayerTeam((int)ds.Tables[0].Rows[i]["Speler_ID"], (byte)ds.Tables[0].Rows[i]["Team"], "140010");
                    }
                }

                switch ((int)Session["Competitie_Type"])
                {
                    case 1: 
                    {
                        Session["Current_Status"] = 8;
                        break;
                    }
                    case 3:
                    {
                        Session["Current_Status"] = 31;
                        break;
                    }
                    case 4:
                    {
                        Session["Current_Status"] = 21;
                        break;
                    }
                }

                Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], (int)Session["Current_Status"]);
                Client_WCF.Update_Workflow_Item("Initialisatie", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
                Client_WCF.Close();

                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }

        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

    }
}