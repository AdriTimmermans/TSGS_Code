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

    public partial class TSGS_CS_Display_MobileMessages : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "DisplayMessages";
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

            Label3.Text = (String)Session["Club"] + ", " + (string)Session["Competition"];
            //Label4.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 16).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();


            /*
            Other text filling actions
            */

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataSet ds = Client_WCF.GetMobileMessageData((int)Session["Competition_Identification"]);
            Client_WCF.Close();

            if (ds.Tables[0].Rows.Count >= 1)
            {
                GridView1.Font.Name = "Arial";
                GridView1.Font.Size = (int)Session["Fontsize"];
                GridView1.DataSource = ds;
                GridView1.AllowPaging = true;
                GridView1.PageSize = 15;
                GridView1.DataBind();
                GridViewRow row1Header = GridView1.HeaderRow;

                row1Header.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
                row1Header.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                row1Header.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
                row1Header.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                row1Header.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
                row1Header.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                row1Header.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
                row1Header.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            }
            Client_MLC.Close();
        }


        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Fill_Texts();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                TextBox tbRecordnr = (TextBox)e.Row.FindControl("tbRecordnr");
                tbRecordnr.Text = Convert.ToString(dr["record_id"]);
                tbRecordnr.ReadOnly = true;
                CheckBox cbHandled = (CheckBox)e.Row.FindControl("cbHandled");
                cbHandled.Checked = (Convert.ToInt16(dr["Handled"]) == 1);
                if (!cbHandled.Checked && cbHandled.Visible)
                {
                    e.Row.BackColor = System.Drawing.Color.LightPink;
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.LightGreen;
                }
             }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
/*
trigger actions for main function of form
*/
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 4, "Closed");
            Client_WCF.Close();
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

        protected void OnHandledChanged(object sender, EventArgs e)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            GridViewRow row = ((GridViewRow)((Control)sender).Parent.Parent);
            int i = row.RowIndex;
            GridView1.SelectedIndex = i;
            CheckBox cbHandled = (System.Web.UI.WebControls.CheckBox)GridView1.Rows[i].FindControl("cbHandled");
            TextBox tbRecord = (System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("tbRecordnr");
            Client_WCF.UpdateMobileMessageIndicator(System.Convert.ToInt16(cbHandled.Checked), System.Convert.ToInt32(tbRecord.Text));
            Client_MLC.Close();
            Client_WCF.Close();
        }

    }
}