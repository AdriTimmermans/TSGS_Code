using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Master : System.Web.UI.MasterPage
    {
        public void SetErrorMessage(string Message)
        {
            Label2.Text = Message;
        }

        public void ErrorMessageVisibility(bool Visibility)
        {
            Label2.Visible = Visibility;
        }

        public void SetErrorMessageRed()
        {
            Label2.BackColor = System.Drawing.Color.Red;
        }

        public void SetErrorMessageGreen()
        {
            Label2.BackColor = System.Drawing.Color.Green;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient(); 
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            if ((int)Session["ForceExit"] == 2)
            {
                System.Environment.Exit(1);
            }

            if ((int)Session["ForceExit"] == 1)
            {
                Label2.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 5).Trim();
                Label2.Visible = true;
                Session.Timeout = 1;
                Timer1.Interval = 60000;
                Session["ForceExit"] = 2;
                Client_WCF.UpdateAccessNumber((string)Session.SessionID, (string)Session["Manager"], -1);
            }
            else
            {
                Client_WCF.UpdateAccessTimeStamp((string)Session.SessionID);
            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (!this.IsPostBack)
            {
                Session["Reset"] = true;
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~/Web.Config");
                SessionStateSection section = (SessionStateSection)config.GetSection("system.web/sessionState");
                int timeout = (int)section.Timeout.TotalMinutes * 1000 * 60;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "SessionExpireAlert(" + timeout + ");", true);
            }

            if (!IsPostBack)
            {
                AssemblyInfoNamespace.AssemblyInfo info = new AssemblyInfoNamespace.AssemblyInfo();
                Fill_Texts();
                Label8.Text = "Version: " + info.AssemblyVersion + " - " + info.Copyright + " " + info.Company;
                Label13.Text = info.Product;
                DataSet ds = Client_MLC.GetLanguageList();

                DDL_Languages.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 10).Trim(), "--")); 
                foreach (DataRow dsItem in ds.Tables[0].Rows)
                {
                    DDL_Languages.Items.Add(new ListItem((string)dsItem["Required_Language"], (string)dsItem["Language_abbreviation"])); 
                }
            }
            Client_MLC.Close();
            Client_WCF.Close();
        }

        public ImageButton DisplayAlarmPanelOnMasterPage
        {
            get
            {
                return this.ImageButton3;
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            //Session["Language"] = 2;
            //Session["Project"] = "CSen";
            //Label10.Text = "English";
            //Fill_Texts();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 2, "Closed from homebutton");
            Client_WCF.Close();
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["Language"] = 1;
            Session["Project"] = "CSnl";
            Label10.Text = "Nederlands";
            Fill_Texts();
        }

        protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 2, "Display Alarm Panel");
            Client_WCF.Close();
            Response.Redirect("TSGS_CS_Display_Alarm_Panel.aspx");

        }

        protected void ImageButton5_Click(object sender, ImageClickEventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 2, "Display Alarm Panel");
            Client_WCF.Close();
            Response.Redirect("TSGS_CS_Display_MobileMessages.aspx");

        }
        protected void ImageButton4_Click(object sender, ImageClickEventArgs e)
        {

            Response.Redirect("TSGS_CS_Create_email_to_application_Manager.aspx");
        }



        public void Reset_Header ()
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            lbl_csc_Status_Line_Ronde.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 4).Trim(), Convert.ToString(0));
            Label5.Text = Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 6).Trim();
            lbl_csc_Status_Line_Resultaten.Text = Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 5).Trim();
            string HTML_Images = "~/images/";
            Image1.ImageUrl = HTML_Images + "\\INDRED.bmp";
            Image2.ImageUrl = HTML_Images + "\\RESRED.bmp";
            Image3.ImageUrl = HTML_Images + "\\OVZRED.bmp";
            Image4.ImageUrl = HTML_Images + "\\WWWRED.bmp";
            Client_MLC.Close();
        }
        protected void Fill_Texts()
        {

            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            controls.FormatControlsLocal(form1.Controls, (int)Session["FontSize"]);
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            Label1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 1).Trim();
            Label1.Font.Size = (int)Session["Fontsize"] + 4;
            Label2.Font.Size = (int)Session["Fontsize"] + 2;

            Label3.Text = Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 2).Trim();
            Label4.Text = Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 1).Trim();
            Label6.Text = Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 3).Trim();
            Label7.Text = (string)Session["Manager"];
            Label9.Text = Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 7).Trim();

            lbl_csc_Status_Line_Ronde.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 4).Trim(), Session["Round_Number"].ToString());

            if ((string)Session["Club"] == "-")
            {
                Label5.Text = Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 6).Trim();
            }
            else
            {
                Label5.Text = Session["Club"] + ", " + Session["Competition"];
            }
            lbl_csc_Status_Line_Resultaten.Text = Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 5).Trim();

            Client_MLC.Close();
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            Timer1.Interval = 60000;


            bool NR = false;
            bool RV = false;
            bool AF = false;
            bool PU = false;
            bool ERR = false;

            if ((int)Session["Competition_Identification"] > 0)
            {
                DataSet ds = Client_WCF.GetWorkFlowRecord((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                DataTable tbl = ds.Tables[0];
                DataRow myRow = tbl.Rows[0];
                NR = (bool)myRow["Nieuwe Ronde"];
                RV = (bool)myRow["Resultaten Verwerken"];
                AF = (bool)myRow["Afdrukken Indeling"];
                PU = (bool)myRow["Publiceren"];
            }
            if ((string)Session["Club"] == "-")
            {
                Label5.Text = Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 6).Trim();
            }
            else
            {
                Label5.Text = Session["Club"] + ", " + Session["Competition"];
            }
            lbl_csc_Status_Line_Ronde.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 4).Trim(), Session["Round_Number"].ToString()); 
            string HTML_Images = "~/images/";
            Label7.Text = (string)Session["Manager"];

            if (NR)
            {
                Image1.ImageUrl = HTML_Images + "\\INDGREEN.bmp";
            }
            else
            {
                Image1.ImageUrl = HTML_Images + "\\INDRED.bmp";
            }
            if (RV)
            {
                Image2.ImageUrl = HTML_Images + "\\RESGREEN.bmp";
            }
            else
            {
                Image2.ImageUrl = HTML_Images + "\\RESRED.bmp";
            }
            if (AF)
            {
                Image4.ImageUrl = HTML_Images + "\\OVZGREEN.bmp";
            }
            else
            {
                Image4.ImageUrl = HTML_Images + "\\OVZRED.bmp";
            }
            if (PU)
            {
                Image3.ImageUrl = HTML_Images + "\\WWWGREEN.bmp";
            }
            else
            {
                Image3.ImageUrl = HTML_Images + "\\WWWRED.bmp";
            }

            ERR = (Client_WCF.CountUnhandledMobileMessages((int)Session["Competition_Identification"]) >= 1);

            if (ERR)
            {
                ImageButton5.ImageUrl = HTML_Images + "\\newmessages.jpg";
            }
            else
            {
                ImageButton5.ImageUrl = HTML_Images + "\\nomessages.jpg";
            }

            ERR = (Client_WCF.CountUnhandledCriticalAlarms() >= 1);

            if (ERR)
            {
                ImageButton3.ImageUrl = HTML_Images + "\\DIY_worried_smiley.jpg";
            }
            else
            {
                ImageButton3.ImageUrl = HTML_Images + "\\smiley-vector-happy-face.jpg";
            }
            Client_MLC.Close();
            Client_WCF.Close();
        }

        protected void On_Language_Selected(object sender, EventArgs e)
        {
            if (new[] { 8, 21, 41, 11, 62, 43, 25, 26, 29, 12, 14, 39, 42, 45, 49, 52, 60, 65 }.Contains(DDL_Languages.SelectedIndex))
            {
                Session["Language"] = 2;
            }
            else
            {
                Session["Language"] = DDL_Languages.SelectedIndex;
            }
            Session["Project"] = "CS" + DDL_Languages.Items[DDL_Languages.SelectedIndex].Value;
            Label10.Text = DDL_Languages.Items[DDL_Languages.SelectedIndex].Text;
            Fill_Texts();
        }
    }
}