using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;


namespace TSGS_CS_Login_Client
{

    public partial class TSGS_CS_Create_email_to_application_manager : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "SendEmail";

            Button2.Enabled = ((int)Session["Current_Status"] != 1);
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
                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], "TSGS_CS_Create_email_to_application_manager", "Functional", 2, "Started");
                Client_WCF.Close();
                Master.SetErrorMessageGreen();
                Master.ErrorMessageVisibility(false);
                Label1.Visible = false;
                /*
                other initialising stuff for function
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
            Label3.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            Label4.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();

            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();

            /*
            Other text filling actions
            */

            Client_MLC.Close();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
/*
trigger actions for main function of form
*/
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Button2.Enabled = false;
            Label1.Visible = true;
            string lit_Error = "";
            string aux = "";
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            string file = Server.MapPath("~/images/Person_CoffeeBreak_Male_Dark.png");

            SmtpClient client = new SmtpClient("mail.planet.nl");
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("Adri", "1403");
            MailAddress from = new MailAddress((String)Session["ManagerEmailAddress"]);
            MailAddress to = new MailAddress("administrator@administration.nl");
            MailMessage message = new MailMessage(from, to);

            StringBuilder mailBody = new StringBuilder();
            aux = (string)Session["Competition"];
            mailBody.AppendFormat("{0}", (string)Session["Competition"]).AppendLine();
            mailBody.AppendFormat("{0}", "From competition manager: "+ (string)Session["Manager"]).AppendLine();
            mailBody.AppendFormat("{0}", "Message: ").AppendLine();
            mailBody.AppendFormat("{0}", TextBox1.Text).AppendLine();
            message.IsBodyHtml = false;
            message.Body = mailBody.ToString();
            message.Subject = "TSGS_CS_Competitie_Systeem - Message from user";
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                lit_Error = ex.Message;
                Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], "TSGS_CS_Create_email_to_application_manager", "Error", 1, lit_Error);
                //	error_occurred = true;
            }
            finally
            {
                // data.Dispose();
                message.Dispose();
                Client_WCF.Close();
                Client_MLC.Close();
                Response.Redirect((string)Session["LastFunction"]);
            }
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect((string)Session["LastFunction"]);
        }

    }
}
