using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace TSGS_CS_Login_Client
{

    public partial class TSGS_ZS_Display_List_Game_Points : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string Class_Name = this.GetType().Name;
            int CutOff_Length = System.Math.Min(20, Class_Name.Length - 8);
            Session["Functionality"] = Class_Name.Substring(8, CutOff_Length);

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
            /*            Client_WCF.<xxxz>((int)Session["Competition_Identification"], (int)Session["Round_Number"]);*/
            Client_WCF.Close();
            Label1.Visible = true;


            /*            Session["Current_Status"] = <z>;
                        TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                        Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], <z>);
                        Client_WCF.Update_Workflow_Item("[<XXXY>]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
                        Client_WCF.Close();*/
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

    }
}