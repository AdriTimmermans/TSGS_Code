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
    public partial class TSGS_CS_Update_Header_Info : System.Web.UI.Page
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
                Session["Functionality"] = "HeaderUpdate";
                Session["LastFunction"] = HttpContext.Current.Request.Url.AbsolutePath;

                Master.SetErrorMessageGreen();
                Master.ErrorMessageVisibility(false);
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

            Label3.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();

            Client_MLC.Close();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

        protected void Calendar1_SelectionChanged(object sender, System.EventArgs e)
        {
            System.DateTime Next_Evening = Calendar1.SelectedDate;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            String TextPart = Client_MLC.GetMLCText((string)Session["Project"], "HeaderUpdate", (int)Session["Language"], 5).Trim();
            string RootPath = System.Web.HttpContext.Current.Server.MapPath("~/htmlfiles/" + Convert.ToString((int)Session["Competition_Identification"]).Trim());            
            Client_WCF.GenerateHeaderFile(Next_Evening, TextPart, (string)Session["Competition"], (int)Session["Competition_Identification"], (int)Session["Round_Number"], RootPath);
            Client_MLC.Close();
            Client_WCF.Close();
        }

        protected void Button1_Click(object sender, System.EventArgs e)
        {
            if ((bool)Session["AutoOverview"])
            {
                Response.Redirect("TSGS_CS_Display_ELO_List.aspx");
            }
            else
            {
                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }
        }

    }
}