using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Initialise_Next_Round : System.Web.UI.Page
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
                Session["Functionality"] = "NextRound";
                Session["LastFunction"] = HttpContext.Current.Request.Url.AbsolutePath;

                Master.SetErrorMessageGreen();
                Master.ErrorMessageVisibility(false);
                Button1.Visible = false;
                Label3.Visible = false;
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
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button3.Text = String.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim(), (int)(Session["Round_Number"]));

            Client_MLC.Close();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int COT = (int)Session["Competitie_Type"];
            int RNR = (int)Session["Round_Number"] + 1;
            Session["Round_Number"] = (int)Session["Round_Number"] + 1;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.Initialize_New_Round((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
            if (COT == 3 && RNR == 1 )
            {
                //
                // Remove no-shows from Competition_Deelnemers
                //
                Client_WCF.Remove_NoShow_From_Deelnemer_Competition((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
            }
            Client_WCF.Update_Workflow_Item("[Nieuwe Ronde]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
            int Competition_Type = Client_WCF.GetIntFromAlgemeneInfo((int)Session["Competition_Identification"], "Competitie_Type");
            
            switch (Competition_Type)
            { 
               case 1:
                {
                    Session["Current_Status"] = 3;
                    Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], 3);
                    break;
                }
               case 2:
                {
                    Session["Current_Status"] = 10;
                    Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], 10);
                    break;
                }
               case 3:
                {
                    Session["Current_Status"] = 33;
                    Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], 3);
                    break;
                }
               case 4:
                {
                    Session["Current_Status"] = 21;
                    Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], 21);
                    break;
                }
            }
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            String TextPart = Client_MLC.GetMLCText((string)Session["Project"], "HeaderUpdate", (int)Session["Language"], 5).Trim();
            string RootPath = System.Web.HttpContext.Current.Server.MapPath("~/htmlfiles/" + Convert.ToString((int)Session["Competition_Identification"]).Trim());
            Client_WCF.GenerateHeaderFile(DateTime.Now, TextPart, (string)Session["Competition"], (int)Session["Competition_Identification"], (int)Session["Round_Number"], RootPath);
            Client_MLC.Close();
            Client_WCF.Close();
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
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