using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Generate_New_Pairing : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "NewPairing";
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
                Label3.Visible = false;
                Button2.Visible = false;
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

            Label3.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            Button3.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();

            Client_MLC.Close();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Updatepanel1.Visible = true;
            Client_WCF.Calculate_New_Pairing((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
            Updatepanel1.Visible = false;
            Label3.Visible = true;
            Button2.Visible = true;
            Session["Current_Status"] = 5;
            Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], 5);
            Client_WCF.Update_Workflow_Item("[Nieuwe Ronde]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
            Client_WCF.Close();

            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
        }

        
        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

    }
}