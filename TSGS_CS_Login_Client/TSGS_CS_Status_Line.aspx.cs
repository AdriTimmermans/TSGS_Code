using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Status_Line : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();


            /* ls.Credentials = System.Net.CredentialCache.DefaultCredentials; */
            DataSet ds = Client_WCF.GetWorkFlowRecord((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
            DataTable tbl = ds.Tables[0];
            DataRow myRow = tbl.Rows[0];
            bool NR = (bool)myRow["Nieuwe Ronde"];
            bool RV = (bool)myRow["Resultaten Verwerken"];
            bool AF = (bool)myRow["Afdrukken Indeling"];
            bool PU = (bool)myRow["Publiceren"];
            Label3.Text = Session["Club"] + ", " + Session["Competition"];
            string HTML_Images = "~/images/";
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
            Client_WCF.Close();

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            Label2.Text = Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 2).Trim();
            Label1.Text = Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 1).Trim();
            lbl_csc_Status_Line_Indeling.Text = Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 3).Trim();
            lbl_csc_Status_Line_Ronde.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 4).Trim(), Session["Round_Number"].ToString());
            if ((string)Session["Club"] == "-")
            {
                Label3.Text = Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 6).Trim();
            }
            else
            {
                Label3.Text = Session["Club"] + ", " + Session["Competition"];
            }
            lbl_csc_Status_Line_Resultaten.Text = Client_MLC.GetMLCText((string)Session["Project"], "Statusline", (int)Session["Language"], 5).Trim();
            Client_MLC.Close();
        }

        protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }
    }
}