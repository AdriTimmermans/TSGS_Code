using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Login_Page : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "Login";
            Session["LastFunction"] = HttpContext.Current.Request.Url.AbsolutePath;
            Session["CallingControl"] = TSGS_CS_Extention_Methods.GetPostBackControlId(this);

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
                Label11.Visible = true;
                TextBox11.Visible = true;
                Session["InitialFontSize"] = 10;
                Session["Manager_Password"] = "-";
                Session["Manager"] = "-";
                Session["Manager_Id"] = 0;
                Session["ManagerEmailAddress"] = "-";
                Session["Round_Number"] = 0;
                Session["Club"] = "-";
                Session["Competition"] = "-";
                Session["Competition_Selection"] = 0;

                Master.SetErrorMessageGreen();
                Master.ErrorMessageVisibility(false);
                Fill_Texts();
                TextBox12.Focus();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            
            switch ((String)Session["CallingControl"])
            {
                case "TextBox12":
                    {
                        TextBox13.Focus();
                        break;
                    }
                case "TextBox13":
                    {
                        TextBox11.Focus();
                        break;
                    }
            } 
        }
        protected void OnIntTextBoxChanged(object sender, EventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Master.ErrorMessageVisibility(false);
            TextBox tb = (TextBox)sender;
            int Input_TB = 0;
            int error = Client_WCF.ValidateInteger(tb.Text.Trim(), false, 10, true, 8, 16, ref Input_TB);
            if (error == 0)
            {
                Master.ErrorMessageVisibility(false);
                tb.BackColor = System.Drawing.Color.White;
                Fill_Texts();
            }
            else
            {
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
                Master.SetErrorMessage(tb.Text.Trim() + ": " + Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], error + 1).Trim());
                tb.BackColor = System.Drawing.Color.Red;
                Client_MLC.Close();
            }
            Client_WCF.Close();
        }

        protected void OnTextBoxMandatoryChanged(object sender, EventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Master.ErrorMessageVisibility(false);
            TextBox tb = (TextBox)sender;
            String Input_TB = "";
            int error = Client_WCF.ValidateString(tb.Text.Trim(), false, "-", ref Input_TB);
            if (error == 0)
            {
                Master.ErrorMessageVisibility(false);
                tb.BackColor = System.Drawing.Color.White;
                Fill_Texts();
            }
            else
            {
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
                Master.SetErrorMessage(tb.Text.Trim() + ": " + Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], error + 1).Trim());
                tb.BackColor = System.Drawing.Color.Red;
                Client_MLC.Close();
            }
            Client_WCF.Close();
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.CompetitionManager Competition_Manager = Client_WCF.GetCompetitionManager(TextBox12.Text);

            int error = CheckFontsize(TextBox11.Text);
            if (error > 0)
            {
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
                Master.SetErrorMessage(TextBox11.Text.Trim() + ": " + Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], error + 1).Trim());
                TextBox11.BackColor = System.Drawing.Color.Red;
                Client_MLC.Close();
            }
            else
            {
                if (Competition_Manager.CompetitionManagerId == 0)
                {
                    Master.SetErrorMessageRed();
                    Master.ErrorMessageVisibility(true);
                    Master.SetErrorMessage(Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim());
                }
                else
                {
                    if (Competition_Manager.Password.Trim() == TextBox13.Text.Trim())
                    {
                        Master.SetErrorMessageGreen();
                        Master.ErrorMessageVisibility(true);
                        Master.SetErrorMessage(Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim());
                        TextBox11.Text = Competition_Manager.Fontsize.ToString();
                        Session["Manager_Password"] = TextBox13.Text.Trim();
                        Session["Manager"] = TextBox12.Text.Trim();
                        Session["Manager_Id"] = Competition_Manager.CompetitionManagerId;
                        Session["UserPrivileges"] = Competition_Manager.UserPrivileges;
                        Session["Loginmoment"] = string.Format("  {0:d} - {0:t}", DateTime.Now);
                        Session["ManagerEmailAddress"] = Competition_Manager.EmailAddress;
                        Session["Current_Status"] = 2;

                        if ((int)Session["Fontsize"] != Competition_Manager.Fontsize)
                        {
                            Session["InitialFontSize"] = Competition_Manager.Fontsize;
                            Competition_Manager.Fontsize = (int)Session["Fontsize"];
                            Fill_Texts();
                        }
                        Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
                    }
                    else
                    {
                        Master.SetErrorMessageRed();
                        Master.ErrorMessageVisibility(true);
                        Master.SetErrorMessage(Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 2).Trim());
                    }
                }
            }
            Client.Close();
            Client_WCF.Close();
        }

        protected void Fill_Texts()
        {
            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            Button1.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Button2.Text = Client.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 9).Trim();
            Label11.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            Label4.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            Label3.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            TextBox11.Text = ((int)Session["InitialFontSize"]).ToString();
            Client.Close();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

        protected int CheckFontsize (string StringFontSize)
        {
            int aux;
            int Input_TB = 0;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            aux = Client_WCF.ValidateInteger(StringFontSize, true, 10, true, 8, 16, ref Input_TB);
            if (aux == 0)
            {
                Master.ErrorMessageVisibility(false);
                TextBox11.BackColor = System.Drawing.Color.White;
                if ((int)Session["InitialFontSize"] != Input_TB)
                {
                    Session["Fontsize"] = Input_TB;
                    Client_WCF.Update_Fontsize((int)Session["Manager_Id"], (int)Session["Fontsize"]);
                }
            }
            Client_WCF.Close();
            //
            //
            //
            return aux;
        }
    }
}