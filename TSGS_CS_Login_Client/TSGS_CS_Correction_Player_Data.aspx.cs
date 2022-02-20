using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Correction_Player_Data : System.Web.UI.Page
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
                Master.SetErrorMessageGreen();
                Master.ErrorMessageVisibility(false);
                Fill_Texts();
            }
        }


        protected void Fill_Texts()
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            Label3.Text = Client_MLC.GetMLCText((string)Session["Project"], "CorrectionPlayerData", (int)Session["Language"], 3).Trim();
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();

            DataSet ds_all = new DataSet();
			ds_all.Clear();
            ds_all = Client_WCF.GetPlayerList((int)Session["Competition_Identification"]);
            GridView1.DataSource = ds_all;
            GridView1.AllowPaging = true;

            GridView1.PageSize = 15;
            GridView1.DataBind();
            GridView1.Font.Name = "Arial";
            GridView1.Font.Size = (int)Session["Fontsize"];
            GridView1.GridLines = GridLines.Horizontal;
            GridView1.SelectedIndex = -1;

            GridViewRow rowHeader = GridView1.HeaderRow;
            rowHeader.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], "CorrectionPlayerData", (int)Session["Language"], 4).Trim();
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
            GridViewRow row = GridView1.Rows[GridView1.SelectedIndex];
            Label aux = (Label)row.FindControl("Speler_ID");
            Session["PlayerToUpdate"] = Convert.ToInt32(aux.Text); 
            if ((string)Session["CorrectionMode"] == "Player")
            {
                Response.Redirect("TSGS_CS_Correction_Player_Data_Details.aspx", false);
            }
            else
            {
                Response.Redirect("TSGS_CS_Correct_Player_Results.aspx", false);
            }
            
        }

        protected void GridView1_OnRowCreated(object sender, GridViewRowEventArgs e)
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

        protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }
    }
}