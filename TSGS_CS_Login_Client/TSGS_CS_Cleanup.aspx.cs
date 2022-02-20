using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Threading;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Cleanup : System.Web.UI.Page
    {
        protected static List<int> Competitions = new List<int>();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "Cleanup";
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
                other initialising stuff for function
                */
                Competitions.Clear();
                DDLCompetitions.Items.Clear();
                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                DataSet dsS = Client_WCF.GetCompetitionList((string)Session["Manager"], (int)Session["Manager_Id"]);
                foreach (DataRow dgItem in dsS.Tables[0].Rows)
                {
                    DDLCompetitions.Items.Add(new ListItem((string)dgItem["Naam_Competitie"]).Text.Trim());
                    Competitions.Add(Convert.ToInt16(dgItem["Competitie_Id"]));
                }
                Client_WCF.Close();
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

            Label1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            Label3.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            Label4.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();

            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();

            CheckBox1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            CheckBox2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            CheckBox3.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            CheckBox4.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            CheckBox6.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
            CheckBox4.Visible = ((int)Session["UserPrivileges"] == 1);

            Client_MLC.Close();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            /*
            trigger actions for main function of form
            */
            int Remove_CID = 0;
            int error = 0;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Label1.Visible = true;

            if (CheckBox1.Checked)
            {
                Client_WCF.Remove_Work_Images(Server.MapPath(@"~/workimages"));
            }

            if (CheckBox2.Checked)
            {
                error = Client_WCF.ValidateInteger(TextBox5.Text.Trim(), false, 0, true, 2, 99999, ref Remove_CID);
                if (error == 0)
                {
                    Master.ErrorMessageVisibility(false);
                    TextBox5.BackColor = System.Drawing.Color.White;
                    Client_WCF.Remove_One_Competition(Convert.ToInt32(TextBox5.Text.Trim()));
                }
                else
                {
                    Master.SetErrorMessageRed();
                    Master.ErrorMessageVisibility(true);
                    TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
                    Master.SetErrorMessage(TextBox5.Text.Trim() + ": " + Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], error + 1).Trim());
                    TextBox5.BackColor = System.Drawing.Color.Red;
                    Client_MLC.Close();
                }

            }

            if (CheckBox3.Checked)
            {
                Client_WCF.Remove_Obsolete_Players();
            }

            if (CheckBox4.Checked)
            {
                string BackUpString;
                BackUpString = Server.MapPath(@"~\workfiles\CSCD" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".bak");
                Client_WCF.Create_Backup(BackUpString);
            }

            if (CheckBox6.Checked)
            {
                TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
                //Thread workerThread = new Thread(Client_MLC.TranslateLanguages);
                //workerThread.Start();
                Client_MLC.TranslateLanguages();
                Client_MLC.Close();
            }
            Client_WCF.Close();
            if (error == 0)
            {
                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }
        }

        protected void DDL_SelectCompetition(object sender, EventArgs e)
        {

            TextBox5.Text = Competitions[DDLCompetitions.SelectedIndex].ToString();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

    }
}
