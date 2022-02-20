using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Competition_Overall_Status : System.Web.UI.Page
    {
        private int Withdrawn = 0;
        private int Total_Absentees = 0;
        private int External = 0;
        private int Total = 0;
        private int Absentees = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "StatusOverview";
            Session["LastFunction"] = HttpContext.Current.Request.Url.AbsolutePath;

            if (IsPostBack)
            {
                int aux = TSGS_CS_Extention_Methods.GetLanguageCode(TSGS_CS_Extention_Methods.GetPostBackControlId(this));
                if (aux >= 1)
                {
                    TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

                    Session["Language"] = aux;
                    Withdrawn = Client_WCF.GetNumberWithdrawn((int)Session["Competition_Identification"]);
                    Total_Absentees = Client_WCF.GetNumberAbsentees((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                    External = Client_WCF.GetNumberExternal((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                    Total = Client_WCF.GetNumberTotal((int)Session["Competition_Identification"]);
                    Client_WCF.Close();

                    Absentees = Total_Absentees - Withdrawn;
                    Total = Total - External - Total_Absentees;
                    Fill_Texts((int)Session["Round_Number"], Total, External, Absentees, Withdrawn);
                }
            }
            else
            {
                Master.SetErrorMessageGreen();
                Master.ErrorMessageVisibility(false);
                Label3.Visible = false;
                Label5.Visible = false;
                Label6.Visible = false;
                Label7.Visible = false;
                Label8.Visible = false;
                Label9.Visible = false;
                Label10.Visible = false;
                Label11.Visible = false;
                Label12.Visible = false;
                Label13.Visible = false;
                Label14.Visible = false;
                CheckBox1.Visible = false;
                CheckBox2.Visible = false;
                CheckBox12.Visible = false;
                CheckBox4.Visible = false;
                CheckBox5.Visible = false;
                CheckBox6.Visible = false;
                CheckBox7.Visible = false;
                CheckBox8.Visible = false;
                CheckBox9.Visible = false;
                CheckBox10.Visible = false;
                CheckBox11.Visible = false;
                int CID = (int)Session["Competition_Identification"];
                if (CID > 0)
                {

                    Label3.Visible = true;
                    Label5.Visible = true;
                    Label6.Visible = true;
                    Label7.Visible = true;
                    Label8.Visible = true;
                    Label9.Visible = true;
                    Label10.Visible = true;
                    Label11.Visible = true;
                    Label12.Visible = true;
                    Label13.Visible = true;
                    Label14.Visible = true;
                    CheckBox1.Visible = true;
                    CheckBox2.Visible = true;
                    CheckBox12.Visible = true;
                    CheckBox4.Visible = true;
                    CheckBox5.Visible = true;
                    CheckBox6.Visible = true;
                    CheckBox7.Visible = true;
                    CheckBox8.Visible = true;
                    CheckBox9.Visible = true;
                    CheckBox10.Visible = true;
                    CheckBox11.Visible = true;

                    TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

                    DataSet ds = Client_WCF.GetWorkFlowRecord((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                    DataTable tbl = ds.Tables[0];
                    DataRow myRow = tbl.Rows[0];
                    int RNR = (int)myRow["Rondenr"];
                    CheckBox12.Checked = (bool)myRow["Initialisatie"];
                    CheckBox1.Checked = (bool)myRow["Afmeldlijst"];
                    CheckBox2.Checked = (bool)myRow["Nieuwe Ronde"];
                    CheckBox4.Checked = (bool)myRow["Afdrukken Indeling"];
                    CheckBox5.Checked = (bool)myRow["Handindeling maken"];
                    CheckBox6.Checked = (bool)myRow["Resultaten Verwerken"];
                    CheckBox7.Checked = (bool)myRow["Correcties"];
                    CheckBox8.Checked = (bool)myRow["Afdrukken Competitie Ranglijst"];
                    CheckBox9.Checked = (bool)myRow["Afdrukken Winst Verlies lijst"];
                    CheckBox10.Checked = (bool)myRow["Afdrukken ELO ranglijst"];
                    CheckBox11.Checked = (bool)myRow["Publiceren"];
                    Withdrawn = Client_WCF.GetNumberWithdrawn((int)Session["Competition_Identification"]);
                    Total_Absentees = Client_WCF.GetNumberAbsentees((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                    External = Client_WCF.GetNumberExternal((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                    Total = Client_WCF.GetNumberTotal((int)Session["Competition_Identification"]);

                    Absentees = Total_Absentees - Withdrawn;
                    Total = Total - External - Total_Absentees;
                    Fill_Texts(RNR, Total, External, Absentees, Withdrawn);
                }
            }

        }

        protected void Fill_Texts(int RNR, int Total, int External, int Absentees, int Withdrawn)
        {

            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            Label3.Text = Client_MLC.GetMLCText((string)Session["Project"], "StatusOverview", (int)Session["Language"], 4).Trim();
            Label7.Text = Client_MLC.GetMLCText((string)Session["Project"], "StatusOverview", (int)Session["Language"], 7).Trim();
            Label8.Text = Client_MLC.GetMLCText((string)Session["Project"], "StatusOverview", (int)Session["Language"], 8).Trim();
            Label9.Text = Client_MLC.GetMLCText((string)Session["Project"], "StatusOverview", (int)Session["Language"], 9).Trim();
            Label10.Text = Client_MLC.GetMLCText((string)Session["Project"], "StatusOverview", (int)Session["Language"], 10).Trim();
            Label11.Text = Client_MLC.GetMLCText((string)Session["Project"], "StatusOverview", (int)Session["Language"], 11).Trim();
            Label12.Text = Client_MLC.GetMLCText((string)Session["Project"], "StatusOverview", (int)Session["Language"], 12).Trim();
            Label13.Text = Client_MLC.GetMLCText((string)Session["Project"], "StatusOverview", (int)Session["Language"], 13).Trim();
            Label14.Text = Client_MLC.GetMLCText((string)Session["Project"], "StatusOverview", (int)Session["Language"], 14).Trim();
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 6).Trim();
            if ((string)Session["Manager"] != "-")
            {
                string AuxText = Client_MLC.GetMLCText((string)Session["Project"], "StatusOverview", (int)Session["Language"], 15).Trim();
                Label4.Text = String.Format(AuxText, (string)Session["Manager"], Session["Loginmoment"]);
            }
            int CID = (int)Session["Competition_Identification"];
            if (CID > 0)
            {
                Label3.Text = Client_MLC.GetMLCText((string)Session["Project"], "StatusOverview", (int)Session["Language"], 4).Trim();
                string AuxText = Client_MLC.GetMLCText((string)Session["Project"], "StatusOverview", (int)Session["Language"], 6).Trim();
                Label6.Text = String.Format(AuxText, RNR);
                AuxText = Client_MLC.GetMLCText((string)Session["Project"], "StatusOverview", (int)Session["Language"], 5).Trim();
                Label5.Text = String.Format(AuxText, Total, External, Absentees, Withdrawn);
            }
            Client_MLC.Close();
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