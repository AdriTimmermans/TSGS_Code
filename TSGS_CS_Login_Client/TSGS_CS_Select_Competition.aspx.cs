using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Select_Competition : System.Web.UI.Page
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
                foreach (var tb in Controls.OfType<TextBox>())
                {
                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                }
                Session["Functionality"] = "SelecteerCompetitie";
                Session["LastFunction"] = HttpContext.Current.Request.Url.AbsolutePath;

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
 
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            GridView1.Font.Name = "Arial";
            GridView1.Font.Size = (int)Session["Fontsize"];
            DataSet ds = Client_WCF.GetCompetitionList((string)Session["Manager"], (int)Session["Manager_Id"]);
            GridView1.DataSource = ds;
            GridView1.AllowPaging = true;

            GridView1.PageSize = 15;
            GridView1.DataBind();
            

            GridViewRow rowHeader = GridView1.HeaderRow;
            rowHeader.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            rowHeader.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            rowHeader.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            rowHeader.Cells[2].HorizontalAlign = HorizontalAlign.Left;
            rowHeader.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            rowHeader.Cells[3].HorizontalAlign = HorizontalAlign.Left;
            rowHeader.Cells[1].Visible = false;
            rowHeader.Cells[4].Visible = false;
            rowHeader.Cells[5].Visible = false;
            rowHeader.Cells[6].Visible = false;
            rowHeader.Cells[7].Visible = false;
            rowHeader.Cells[8].Visible = false;
            Client_MLC.Close();
            Client_WCF.Close();
        }
        
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Fill_Texts();
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridView1.SelectedRow;
            Session["Competition_Identification"] = System.Convert.ToInt32(row.Cells[1].Text);
            Session["Club_Id"] = row.Cells[5].Text;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Session["Round_Number"] = Client_WCF.GetRoundNumber((int)Session["Competition_Identification"]);
            Session["Club"] = row.Cells[2].Text;
            Session["Competition"] = row.Cells[3].Text;
            Session["Competitie_Type"] = System.Convert.ToInt32(row.Cells[7].Text);
            Session["GamesPerRound"] = System.Convert.ToInt32(row.Cells[8].Text);
            Session["Current_Status"] = Client_WCF.GetIntFromAlgemeneInfo((int)Session["Competition_Identification"], "CurrentState");
            Client_WCF.Close();
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[0].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[2].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[3].Font.Size = (int)Session["Fontsize"];
            }
        }
        protected void GridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (DataControlFieldCell cell in e.Row.Cells)
                {
                    foreach (Control control in cell.Controls)
                    {
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

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }
    }
}