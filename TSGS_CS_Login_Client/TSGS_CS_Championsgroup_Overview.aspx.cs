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
    public partial class TSGS_CS_Championsgroup_Overview : System.Web.UI.Page 
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "CGOverview";
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
                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                Client_WCF.SortMatrix((int)Session["Competition_Identification"]);
                Client_WCF.Close();
                Label1.Visible = false;
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
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Create_CG_Overview();
            Client_MLC.Close();

        }

        protected void Create_CG_Overview()
        {
            //=============================================================================================
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            string RootPath = Server.MapPath("~/htmlfiles/" + Convert.ToString((int)Session["Competition_Identification"]).Trim());
            string DisplayHeader = "TSGS_CS_ChampionsgroupList.html";
            string DisplayBody = "ChampionsgroupList_body.js";

            string Output_Line = "";
            string Output_Line_Part2 = "";

            StreamWriter Container = new StreamWriter(RootPath + @"\" + DisplayHeader);
            StreamWriter CG = new StreamWriter(RootPath + @"\" + DisplayBody);

            string doc_type = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";
            string CG_String = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            Container.WriteLine(doc_type);
            Container.WriteLine("<html xmlns=\"http://www.w3.org/1999/shtml\">");
            Container.WriteLine("<head>");
            Container.WriteLine("<title>" + Label1.Text + "</title>");
            Container.WriteLine("<meta http-equiv='content-type' CONTENT='text/html; charset=ISO-8859-1'>");
            Container.WriteLine("<meta http-equiv=\"expires\" content=\"0\">");
            string path_Template = Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_Template").Trim();
            Container.WriteLine("<link href='" + path_Template + "/modern.css' type=\"text/css\" rel=\"stylesheet\">");
            Container.WriteLine("</head>");
            Container.WriteLine("<base target='main'>");
            Container.WriteLine("<body>");
            Container.WriteLine("<hr size=4 Width='75%'>");

            CG.WriteLine("document.write(\"\\");

            int CGCount = Client_WCF.GetNumberTotal((int)Session["Competition_Identification"]);
            if (CGCount != 0)
            {
                //
                // display Championsgroup overview overall
                //
                CG.WriteLine("<div id='printable' class='PrintSettings'>\\");
                CG.WriteLine("<h1 align=left>" + (string)Session["Club"] + ", " + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 16).Trim() + "</h1>\\");
                Output_Line = "<br><table border='1px'>";
                CG.WriteLine(Output_Line);
                //
                // first get headers
                //
                DataSet dsR = Client_WCF.GetCrossTableRow((int)Session["Competition_Identification"], 0);
                int ii = 0;
                foreach (DataRow dgItem in dsR.Tables[0].Rows)
                {
                    if (ii == 0)
                    {
                        Output_Line = "<tr><td align='center'>&nbsp;</td><td align='left'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 17).Trim() + "</td>";
                    }
                    else if (ii <= CGCount + 1)
                    {
                        Output_Line += "<td align='center'>" + Convert.ToString(dgItem["Column_Name"]).Trim() + "</td>";
                    }
                    ii++;
                }
                CG.WriteLine(Output_Line);
                //
                // Now same for all data rows
                //
                float Last_Total_Points = 0;
                float Current_Total_Points = 0;
                for (int j = 1; j <= CGCount; j++)
                {
                    dsR = Client_WCF.GetCrossTableRow((int)Session["Competition_Identification"], j);
                    ii = 0;
                    foreach (DataRow dgItem in dsR.Tables[0].Rows)
                    {
                        if (ii == 0)
                        {
                            Output_Line_Part2 = "</td><td align='left'>" + dgItem["Row_Name"] + "</td>";
                        }
                        else if (ii == CGCount + 1)
                        {
                            Current_Total_Points = Convert.ToSingle(dgItem["Matrix_Value"]);
                            Output_Line_Part2 += "<td align='center'>" + String.Format("{0:0.#}", dgItem["Matrix_Value"]) + "</td>";
                            if (Current_Total_Points != Last_Total_Points)
                            {
                                Output_Line = "<tr><td align='center'>" + j.ToString();
                                Last_Total_Points = Current_Total_Points;
                            }
                            else
                            {
                                Output_Line = "<tr><td align='center'>&nbsp;";
                            }
                        }
                        else if (ii <= CGCount)
                        {
                            if (Convert.ToInt16(dgItem["Matrix_Value"]) == -1)
                            {
                                if (ii == j)
                                {
                                    Output_Line_Part2 += "<td align='center' style='background-color:grey'><img src='../images/scdlogodoorzichtig.gif' height='16' ></td>";
                                }
                                else
                                {
                                    Output_Line_Part2 += "<td align='center'>.</td>";
                                }
                            }
                            else
                            {
                                Output_Line_Part2 += "<td align='center'>" + String.Format("{0:0.#}", dgItem["Matrix_Value"]) + "</td>";
                            }
                        }
                        ii++;
                    }
                    CG.WriteLine(Output_Line + Output_Line_Part2 + "</tr>");
                }
            }

            //=============================================================================================

            CG.WriteLine("</table>");
            CG.WriteLine("</div>");
            CG.WriteLine("<br />");
            CG.WriteLine("<br />");
            CG.WriteLine("<input id=\"buttonprint\" type=\"button\" value=\"" + Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 11).Trim() + "\" onclick=\"PrintElem();\">\\");

            //Content.WriteLine("\");");
            Container.WriteLine("<script src=\"" + DisplayBody + "\"></script>");
            Container.WriteLine("</body>");
            Container.WriteLine("</html>");
            Container.Close();
            CG.Close();

            StreamReader sr = new StreamReader(RootPath + @"\" + DisplayBody);
            ltrl_Html.Text = sr.ReadToEnd();

            Client_WCF.Close();
            Client_MLC.Close();

        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            Label1.Visible = true;
            string RootPath = Server.MapPath("~/htmlfiles/" + Convert.ToString((int)Session["Competition_Identification"]).Trim());
            string DisplayHeader = "TSGS_CS_ChampionsgroupList.html";
            string DisplayBody = "ChampionsgroupList_body.js";
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

                        //
            // Upload files to website
            //
            string Display_Competition = Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_basis").Trim() + "/" +
                                         Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_Competitie").Trim();
            Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], DisplayHeader);
            Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], DisplayBody);
            Client_WCF.Close();

            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

    }
}