using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Go_To_Previous_Round : System.Web.UI.Page
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
                Session["Functionality"] = "PreviousRound";
                Session["LastFunction"] = HttpContext.Current.Request.Url.AbsolutePath;

                Master.SetErrorMessageGreen();
                Master.ErrorMessageVisibility(false);
                Button1.Visible = false;
                Label3.Visible = false;
                Label4.Visible = false; 
                Button3.Visible = true;
                Fill_Texts();
            }
        }

        protected void Fill_Texts()
        {

            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            Label3.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            Label4.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim(); 
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button3.Text = String.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim(), (int)(Session["Round_Number"]));

            Client_MLC.Close();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            if ((int)Session["Round_Number"] == 0)
            {
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
                Master.SetErrorMessage(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim());
                Client_MLC.Close();
            }
            else
            {
                Client_WCF.Remove_Round_Information((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                int Competition_Type = Client_WCF.GetIntFromAlgemeneInfo((int)Session["Competition_Identification"], "Competitie_Type");
                Client_WCF.RemovePlayerStatusCalculated((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                Session["Round_Number"] = (int)Session["Round_Number"] - 1;
                Client_WCF.SetIntInAlgemeneInfo("Laatste_ronde", (int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                Client_WCF.Update_Workflow_Item("Handindeling maken", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 0);
                Client_WCF.Update_Workflow_Item("Afdrukken indeling", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 0);
                Client_WCF.Update_Workflow_Item("Resultaten Verwerken", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 0);
                Client_WCF.Update_Workflow_Item("Correcties", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 0);
                Client_WCF.Update_Workflow_Item("Afdrukken Competitie Ranglijst", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 0);
                Client_WCF.Update_Workflow_Item("Afdrukken Winst Verlies lijst", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 0);
                Client_WCF.Update_Workflow_Item("Afdrukken ELO ranglijst", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 0);
                Client_WCF.Update_Workflow_Item("Publiceren", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 0);
                switch (Competition_Type)
                {
                    case 1:
                        {
                            Session["Current_Status"] = 4;
                            Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], 4);
                            break;
                        }
                    case 3:
                        {
                            Session["Current_Status"] = 33;
                            Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], 4);
                            break;
                        }
                    case 4:
                        {
                            Session["Current_Status"] = 21;
                            Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], 4);
                            break;
                        }
                }
                Label4.Visible = true;
                System.Threading.Thread.Sleep(5000);
                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }
            Client_WCF.Close();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Label3.Visible = true;
            Button1.Visible = true;
        }

    }
}