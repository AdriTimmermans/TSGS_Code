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
    public partial class TSGS_CS_Default_Absence : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string Class_Name = this.GetType().Name;
            Class_Name = Class_Name.Substring(0, Class_Name.Length - 5);
            int CutOff_Length = System.Math.Min(20, Class_Name.Length - 8);
            Session["Functionality"] = Class_Name.Substring(Class_Name.Length - CutOff_Length, CutOff_Length);
            Fill_Texts();
            if (IsPostBack)
            {
                int aux = TSGS_CS_Extention_Methods.GetLanguageCode(TSGS_CS_Extention_Methods.GetPostBackControlId(this));
                if (aux >= 1)
                {
                    Session["Language"] = aux;
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
            Label3.Text = (string)Session["Club"];
            Label4.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim(), (int)Session["Round_number"]);

            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataSet ds = Client_WCF.GetPlayerListAlphabetical((int)Session["Competition_Identification"]);
            ViewState["Players"] = ds.Tables[0];
            Client_WCF.Close();
            GridView1.Font.Name = "Arial";
            GridView1.Font.Size = (int)Session["Fontsize"];
            GridView1.DataSource = ds;
            GridView1.AllowPaging = true;
            GridView1.PageSize = 10;
            GridView1.DataBind();
            GridViewRow row1Header = GridView1.HeaderRow;

            row1Header.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            row1Header.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            row1Header.Cells[2].Text = " ";
            row1Header.Cells[2].HorizontalAlign = HorizontalAlign.Center;

            Client_MLC.Close();
        }

        protected void GridView1_RowDataBound (object sender, GridViewRowEventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int Number_of_Rounds = Client_WCF.GetMaxRounds((int)Session["Competition_Identification"]);
                for (int i = 0; i < Number_of_Rounds; i++)
                {
                    CheckBox cb_Afwezig = new CheckBox();
                    cb_Afwezig.Text = "R"+(i+1).ToString();
                    cb_Afwezig.ID = (i + 1).ToString();
                    e.Row.Cells[2].Controls.Add(cb_Afwezig);
                }
               
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                e.Row.Cells[0].Text = Convert.ToString(dr["Speler_ID"]);
                e.Row.Cells[1].Text = Convert.ToString(dr["SpelerNaam"]);
                int PID = (int)dr["Speler_ID"];
                DataSet ds = Client_WCF.GetAbsentRounds(PID, (int)Session["Competition_Identification"]);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int Rondenr = (int)ds.Tables[0].Rows[i]["Rondenr"];
                        if (Rondenr != 0)
                        {
                            CheckBox cb_afwezig = (CheckBox)e.Row.FindControl(Rondenr.ToString());
                            cb_afwezig.Checked = true;
                        }
                    }
                }

                for (int i = 0; i < Number_of_Rounds; i++)
                {
                    CheckBox cb_Afwezig = (CheckBox)e.Row.FindControl((i+1).ToString());
                    if (i<(int)Session["Round_Number"])
                    {
                        cb_Afwezig.Enabled = false;
                    }
                    else
                    {
                        cb_Afwezig.CheckedChanged += new EventHandler(this.OnAfwezigChanged);
                        cb_Afwezig.AutoPostBack = true;
                    }
                }
            }
        }

        protected void OnAfwezigChanged(object sender, EventArgs e)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            GridViewRow row = ((GridViewRow)((Control)sender).Parent.Parent);
            int i = row.RowIndex;
            GridView1.SelectedIndex = i;
            CheckBox cb_afwezig = (System.Web.UI.WebControls.CheckBox)sender;
            
            TSGS_CS_WCF_Service.AbsenteeData AbsenteeData = new TSGS_CS_WCF_Service.AbsenteeData();

            AbsenteeData.Speler_ID = System.Convert.ToInt32(row.Cells[0].Text);
            AbsenteeData.Rondenummer = System.Convert.ToInt32(cb_afwezig.ID);
            AbsenteeData.Competitie_ID = (int)Session["Competition_Identification"];
            if (cb_afwezig.Checked)
                {
                    AbsenteeData.Afwezigheidscode = 1;
                }
            else
                {
                    AbsenteeData.Afwezigheidscode = 0;
                }

            AbsenteeData.Kroongroep_partijnummer = 0;
            Client_WCF.AddAbsentee(AbsenteeData);

            Client_WCF.Close();
            Client_MLC.Close();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Fill_Texts();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

    }
}
