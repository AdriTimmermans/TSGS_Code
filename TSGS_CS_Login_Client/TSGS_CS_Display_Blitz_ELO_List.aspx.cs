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
    public partial class TSGS_CS_Display_Blitz_ELO_List : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "DisplayBlitzRanking";
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
            Label3.Text = (string)Session["Club"];
            Label4.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataSet ds = Client_WCF.GetBlitzRatingRanking();
            ds.Tables[0].Columns.Add("PL", typeof(string));

            //            Cycle through DataSet to determine position

            int Position = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Position = Position + 1;
                ds.Tables[0].Rows[i]["PL"] = Position;
            }
            ViewState["DSList"] = ds.Tables[0];
            GridView1.DataSource = ds;
            GridView1.AllowPaging = true;
            GridView1.PageSize = 10;
            GridView1.DataBind();

            GridViewRow row1Header = GridView1.HeaderRow;
            row1Header.Cells[0].Text = "";
            row1Header.Cells[0].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            row1Header.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            row1Header.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            row1Header.Cells[2].HorizontalAlign = HorizontalAlign.Right;

            Client_WCF.Close();
            Client_MLC.Close();

            /*
            Other text fiilling actions
            */
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Fill_Texts();
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
                e.Row.Cells[1].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[2].Font.Size = (int)Session["Fontsize"];
                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                Label lbl = (Label)e.Row.FindControl("Spelernaam");
                lbl.Text = (string)Client_WCF.GetPlayerName((int)dt.Rows[Gridline]["PlayerID"]);
                Client_WCF.Close();
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
/*
trigger actions for main function of form
*/
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
/*            Client_WCF.<xxxz>((int)Session["Competition_Identification"], (int)Session["Round_Number"]);*/
            Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 4, "Closed");
            Client_WCF.Close();
            Label1.Visible = true;


/*            Session["Current_Status"] = <z>;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], <z>);
            Client_WCF.Update_Workflow_Item("[<XXXY>]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
            Client_WCF.Close();*/
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
