using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Threading;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Display_Personal_Scores_Selection : System.Web.UI.Page
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
                else
                {
                    RefreshDisplay();
                }
            }
            else
            {
                Session["Functionality"] = "PersonalScores";
                Session["LastFunction"] = HttpContext.Current.Request.Url.AbsolutePath;
                Session["CurrentRow"] = 0;
                Session["MaximumRow"] = 0;
                Refresh.Interval = 1000;
                Master.SetErrorMessageGreen();
                Master.ErrorMessageVisibility(false);
                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                DataSet ds_all = new DataSet();

                ds_all.Clear();
                Client_WCF.DeletePlayerListAlphabeticalOverview((int)Session["Competition_Identification"]);
                Client_WCF.GetPlayerListAlphabeticalOverviewInit((int)Session["Competition_Identification"]);
                ds_all = Client_WCF.GetPlayerListAlphabeticalOverview((int)Session["Competition_Identification"]);
                Session["MaximumRow"] = ds_all.Tables[0].Rows.Count;
                Client_WCF.Close();
                ViewState["Players"] = ds_all;
                Fill_Texts();

                if ((bool)Session["AutoOverview"])
                {
                    GenerateOverview();
                    Button2_Click_Actions(null, null);
                }
            }
        }

        protected void Fill_Texts()
        {

            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            Button3.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button11.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Label1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            Label2.Text = String.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 23).Trim(), DateTime.Now.ToString("hh:mm:ss"));

            Client_MLC.Close();
            RefreshDisplay();

        }

        protected void RefreshDisplay()
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataSet ds = Client_WCF.GetPlayerListAlphabeticalOverview((int)Session["Competition_Identification"]);
            Client_WCF.Close();

            DataList1.DataSource = ds;
            DataList1.DataBind();
            DataList1.Font.Name = "Arial";
            DataList1.Font.Size = (int)Session["Fontsize"];
            DataList1.SelectedIndex = -1;
        }


        protected void GenerateOverview()
        {
            GenerateOverviewIndex();
            Session["RefreshMode"] = "Create";
            Refresh.Enabled = true;
        }

        protected void GenerateOverviewIndex()
        {
            int PictureCount = 0;
            string sublink = "";
            DataSet ds_all = new DataSet();

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            string RootPath = Server.MapPath("~/htmlfiles/" + Convert.ToString((int)Session["Competition_Identification"]).Trim());
            string Display_Competition =
                /*               Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_basis").Trim() + "/" + */
                Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_Competitie").Trim();

            string DisplayHeader = "TSGS_CS_PersonalScoresIndex.html";
            string DisplayBody = "PersonalScoresIndex_body.js";

            StreamWriter Container = new StreamWriter(RootPath + @"\" + DisplayHeader);
            StreamWriter Content = new StreamWriter(RootPath + @"\" + DisplayBody);

            string doc_type = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";

            Container.WriteLine(doc_type);
            Container.WriteLine("<html xmlns=\"http://www.w3.org/1999/shtml\">");
            Container.WriteLine("<head>");
            Container.WriteLine("<title>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 1).Trim() + "</title>");
            Container.WriteLine("<meta http-equiv='content-type' CONTENT='text/html; charset=ISO-8859-1'>");
            Container.WriteLine("<meta http-equiv=\"expires\" content=\"0\">");
            string path_Template = Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_Template").Trim();
            Container.WriteLine("<link href='" + path_Template + "/modern.css' type=\"text/css\" rel=\"stylesheet\">");
            Container.WriteLine("</head>");
            Container.WriteLine("<base target='main'>");
            Container.WriteLine("<body>");
            Container.WriteLine("<hr size=4 Width='75%'>");

            Content.WriteLine("document.write(\"\\");
            Content.WriteLine("<h1 align=left>" + (string)Session["Club"] + ", " + (string)Session["Competition"] + "</h1>\\");
            Content.WriteLine("<h1 align=left>" + string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim(), (int)Session["Round_Number"]) + "</h1>\\");
            Content.WriteLine("<br>\\");
            Content.WriteLine("<table border='0\'>\\");
            Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");

            ds_all = (DataSet)ViewState["Players"];
            foreach (DataRow dgItem in ds_all.Tables[0].Rows)
            {
                if ((int)dgItem["PlayerID"] > 0)
                {
                    if (PictureCount == 0)
                    {
                        Content.WriteLine("<tr>\\");
                    }
                    PictureCount++;
                    Content.Write("<td width='5%' align='center'>"/* + "<img src='" + Client_WCF.StringImage((int)dgItem[1], (int)Session["Competition_Identification"]) + "'>"*/+ "</td>");
                    sublink = "0000000000" + dgItem["PlayerID"].ToString();
                    sublink = "/" + Display_Competition + "/P" + sublink.Substring(sublink.Length - 10, 10) + ".html";
                    Content.WriteLine("<td width='20%' align='left'><a href='" + sublink + "' onclick='return !window.open(this.href);'>" + dgItem["PlayerName"].ToString() + "</a></td>\\");

                    if (PictureCount == 4)
                    {
                        PictureCount = 0;
                        Content.WriteLine("</tr>\\");
                    }
                }
            }

            if (PictureCount != 0)
            {
                Content.WriteLine("</tr>\\");
            }

            Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");

            Content.WriteLine("</table>\\");
            Content.WriteLine("<br />\\");
            Content.WriteLine("<br />\\");

            Content.WriteLine("\");");
            Container.WriteLine("    <script src=\"" + DisplayBody + "\"></script>");
            Container.WriteLine("</body>");
            Container.WriteLine("</html>");
            Container.Close();
            Content.Close();
            Client_WCF.Close();
        }
        protected void UploadOneIndividualOverview(object p_object)
        {
            string sublink;

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            int PlayerNr = 0;
            int.TryParse(p_object.ToString(), out PlayerNr); 
            
            sublink = "0000000000" + PlayerNr.ToString();
            sublink = "P" + sublink.Substring(sublink.Length - 10, 10);
            string RootPath = Server.MapPath("~/htmlfiles/" + Convert.ToString((int)Session["Competition_Identification"]).Trim());
            string Display_Competition =
                Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_basis").Trim() + "/" +
                Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_Competitie").Trim();
            string DisplayHeader = sublink + ".html";
            string DisplayBody = sublink + ".js";

            String UploadHeader = Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], DisplayHeader);
            if (UploadHeader != "")
            {
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                Label1.Text += UploadHeader;
                Label1.Visible = true;
            }
            String UploadBody = Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], DisplayBody);
            if (UploadHeader != "")
            {
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                Label1.Text += UploadBody;
                Label1.Visible = true;
            }


        }
        protected void GenerateOneIndividualOverview(object p_object)
        {
            string sublink;

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            int PlayerNr = 0;
            int.TryParse(p_object.ToString(), out PlayerNr);

            DataSet ds = (DataSet)ViewState["Players"];

            foreach (DataRow p_dr in ds.Tables[0].Rows)
            {
                if ((int)p_dr[1] == PlayerNr)
                {
                    int LineCount = 0;
                    sublink = "0000000000" + p_dr["PlayerID"].ToString();
                    sublink = "P" + sublink.Substring(sublink.Length - 10, 10);
                    string RootPath = Server.MapPath("~/htmlfiles/" + Convert.ToString((int)Session["Competition_Identification"]).Trim());
                    string Display_Competition =
                        Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_basis").Trim() + "/" +
                        Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_Competitie").Trim();
                    string DisplayHeader = sublink + ".html";
                    string DisplayBody = sublink + ".js";

                    StreamWriter Container = new StreamWriter(RootPath + @"\" + DisplayHeader);
                    StreamWriter Content = new StreamWriter(RootPath + @"\" + DisplayBody);

                    string doc_type = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";

                    Container.WriteLine(doc_type);
                    Container.WriteLine("<html xmlns=\"http://www.w3.org/1999/shtml\">");
                    Container.WriteLine("<head>");
                    Container.WriteLine("<title>" + sublink + "</title>");
                    Container.WriteLine("<meta http-equiv='content-type' CONTENT='text/html; charset=ISO-8859-1'>");
                    Container.WriteLine("<meta http-equiv=\"expires\" content=\"0\">");
                    string path_Template = Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_Template").Trim();
                    Container.WriteLine("<link href='" + path_Template + "/modern.css' type=\"text/css\" rel=\"stylesheet\">");
                    Container.WriteLine("</head>");
                    Container.WriteLine("<base target='main'>");
                    Container.WriteLine("<body>");
                    Container.WriteLine("<hr size=4 Width='75%'>");

                    Content.WriteLine("document.write(\"\\");
                    Content.WriteLine("<h1 align=left>" + (string)Session["Club"] + ", " + (string)Session["Competition"] + "</h1>\\");
                    Content.WriteLine("<img src='" + Client_WCF.StringImage((int)p_dr[1], (int)Session["Competition_Identification"]) + "'><br />\\");
                    Content.WriteLine("<h2 align=left>" + p_dr["PlayerName"] + "</h2><br /><br />\\");
                    string aux = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
                    string auxline = string.Format(aux,
                        (double)Client_WCF.GetPlayerRatingInCompetition((int)p_dr["PlayerID"], (int)Session["Round_Number"], (int)Session["Competition_Identification"]),
                        (double)Client_WCF.GetPlayerPointsInCompetition((int)p_dr["PlayerID"], (int)Session["Round_Number"], (int)Session["Competition_Identification"]),
                        (double)Client_WCF.GetKNSBRating((int)p_dr["PlayerID"]),
                        (double)Client_WCF.GetFIDERating((int)p_dr["PlayerID"]));
                    Content.WriteLine(auxline + "<br />\\");
                    Content.WriteLine("<table border='0\'>\\");
                    Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");

                    DataSet dsR = Client_WCF.GetResults((int)p_dr["PlayerID"], (int)Session["Competition_Identification"], (int)Session["Language"]);
                    LineCount++;

                    Content.WriteLine("<td style='width:  5%' >" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim() + "</td>\\");
                    Content.WriteLine("<td style='width: 15%' >" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 16).Trim() + "</td>\\");
                    Content.WriteLine("<td style='width: 30%' >" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim() + "</td>\\");
                    Content.WriteLine("<td style='width: 10%' >" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim() + "</td>\\");
                    Content.WriteLine("<td style='width: 10%' >" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim() + "</td>\\");
                    Content.WriteLine("<td style='width: 10%' >" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 13).Trim() + "</td>\\");
                    Content.WriteLine("<td style='width: 10%' >" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 14).Trim() + "</td>\\");
                    Content.WriteLine("</tr>\\");

                    Content.WriteLine("<tr><td height='8'></td></tr>\\");
                    foreach (DataRow dsRItem in dsR.Tables[0].Rows)
                    {
                        Content.WriteLine("<td style='width:  5%' >" + string.Format("{0:f0}", dsRItem["Rondenr"]) + "</td>\\");
                        Content.WriteLine("<td style='width: 15%' >" + string.Format("{0:dd/MM/yyyy}", (DateTime)dsRItem["Rondedatum"]) + "</td>\\");
                        Content.WriteLine("<td style='width: 30%' >" + (string)dsRItem["Spelernaam"] + "</td>\\");
                        Content.WriteLine("<td style='width: 10%' >" + (string)dsRItem["Afkorting"] + "</td>\\");
                        Content.WriteLine("<td style='width: 10%' >" + (string)dsRItem["Kleur"] + "</td>\\");
                        Content.WriteLine("<td style='width: 10%' >" + string.Format("{0:#0.0}", (double)dsRItem["ELOResultaat"]) + "</td>\\");
                        Content.WriteLine("<td style='width: 10%' >" + string.Format("{0:#0.0}", (double)dsRItem["CompetitieResultaat"]) + "</td>\\");
                        Content.WriteLine("</td></tr>\\");
                    }
                    Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");
                    Content.WriteLine("</table>\\");
                    Content.WriteLine("<br />\\");
                    Content.WriteLine("<br />\\");

                    Content.WriteLine("<h2 align=left>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 22).Trim() + "</h2><br /><br />\\");
                    Content.WriteLine("<table border='0\'>\\");

                    DataSet dsA = Client_WCF.GetAllAdversariesStatistics((int)p_dr["PlayerID"]);
                    LineCount = 0;

                    byte[] file = File.ReadAllBytes(Server.MapPath("~/images/target.jpg"));
                    string TargetString = "<img src='" + Client_WCF.MakeImageSourceData((byte[])file) + "' width='20'>";

                    Content.WriteLine("<tr>\\");
                    Content.WriteLine("<td style='width:  5%' >" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 21).Trim() + "</td>\\");
                    Content.WriteLine("<td style='width: 30%' >" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 17).Trim() + "</td>\\");
                    Content.WriteLine("<td style='width: 10%' >" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 18).Trim() + "</td>\\");
                    Content.WriteLine("<td style='width: 10%' >" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 19).Trim() + "</td>\\");
                    Content.WriteLine("<td style='width: 10%' >" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 20).Trim() + "</td>\\");
                    Content.WriteLine("</tr>\\");

                    Content.WriteLine("<tr><td height='8'></td></tr>\\");
                    foreach (DataRow dsRaItem in dsA.Tables[0].Rows)
                    {
                        LineCount++;

                        Content.WriteLine("<tr>\\");
                        Content.WriteLine("<td style='width:  5%' >" + string.Format("{0:f0}", LineCount) + "</td>\\");
                        Content.WriteLine("<td style='width: 30%; align:left'>" + "<a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=" + string.Format("{0000}", dsRaItem["Player_1"]) + "&Player2=" + string.Format("{0000}", dsRaItem["Player_2"]) + "'>" + (string)dsRaItem["Spelernaam"] + "</a></td>\\");
                        Content.WriteLine("<td style='width: 10%' >" + string.Format("{0:#0.00}", dsRaItem["Frequency"]) + "</td>\\");
                        Content.WriteLine("<td style='width: 10%' >" + string.Format("{0:#0}", dsRaItem["Games"]) + "</td>\\");
                        Content.WriteLine("<td style='width: 10%' >" + string.Format("{0:#0}", dsRaItem["Competitions"]) + "</td>\\");
                        Content.WriteLine("</tr>\\");
                    }

                    Content.WriteLine("</table>\\");
                    Content.WriteLine("<br />\\");
                    Content.WriteLine("<br />\\");
                    Content.WriteLine("\");");
                    Container.WriteLine("    <script src=\"" + DisplayBody + "\"></script>");
                    Container.WriteLine("</body>");
                    Container.WriteLine("</html>");
                    Container.Close();
                    Content.Close();

                    Client_MLC.Close();
                    Client_WCF.Close();
                    break;
                }
            }
        }

        protected void Button11_Click(object sender, EventArgs e)
        {
            Label2.Visible = true;
            RefreshDisplay();
            GenerateOverview();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            Session["LabelText"] = String.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 23).Trim(),DateTime.Now.ToString("hh:mm:ss")) + " - ";
            Session["LabelText"] += Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 24).Trim();
            Client_MLC.Close();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            Session["LabelText"] = String.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 25).Trim(), DateTime.Now.ToString("hh:mm:ss")) + " - ";
            Session["LabelText"] += Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 26).Trim();
            Client_MLC.Close();

            Button2_Click_Actions(sender, e);
        }

        protected void Button2_Click_Actions(object sender, EventArgs e)
        {
            Session["RefreshMode"] = "Upload";

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            string RootPath = Server.MapPath("~/htmlfiles/" + Convert.ToString((int)Session["Competition_Identification"]).Trim());

            string Display_Competition =
                Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_basis").Trim() + "/" +
                Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_Competitie").Trim();

            string DisplayHeader = "TSGS_CS_PersonalScoresIndex.html";
            string DisplayBody = "PersonalScoresIndex_body.js";

            string UploadHeader = Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], DisplayHeader);
            if (UploadHeader != "")
            {
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                Label1.Text += UploadHeader;
                Label1.Visible = true;
            }

            String UploadBody = Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], DisplayBody);
            if (UploadHeader != "")
            {
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                Label1.Text += UploadBody;
                Label1.Visible = true;
            }

            RefreshDisplay();
            Refresh.Enabled = true;
        }

        protected void RefreshTick(object sender, EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            if ((string)Session["RefreshMode"] == "Create")
            {
                for (int i = 1; i <= 5; i++)
                {
                    if ((int)Session["CurrentRow"] < (int)Session["MaximumRow"])
                    {
                        DataSet ds = (DataSet)ViewState["Players"];
                        DataRow dgItem = ds.Tables[0].Rows[(int)Session["CurrentRow"]];
                        Label2.Text = String.Format((string)Session["LabelText"], (int)Session["CurrentRow"] + 1, ds.Tables[0].Rows.Count, DateTime.Now.ToString("hh:mm:ss"));
                        GenerateOneIndividualOverview((object)dgItem[1]);
                        Client_WCF.UpdatePlayerListAlphabeticalOverview((int)Session["Competition_Identification"], (int)dgItem[1], "C", 1);
                        //
                        // Set up for next timer click
                        //
                        Session["CurrentRow"] = (int)Session["CurrentRow"] + 1;
                    }
                }
                if ((int)Session["CurrentRow"] == (int)Session["MaximumRow"])
                {
                    Button11.Visible = false;
                    Button2.Visible = true;
                    Refresh.Enabled = false;
                    RefreshDisplay();
                    Session["CurrentRow"] = 0;
                }
                else
                {
                    RefreshDisplay();
                }
            }
            else
            {
                for (int i = 1; i <= 1; i++)
                {
                    if ((int)Session["CurrentRow"] < (int)Session["MaximumRow"])
                    {
                        DataSet ds = (DataSet)ViewState["Players"];
                        DataRow dgItem = ds.Tables[0].Rows[(int)Session["CurrentRow"]];
                        Label2.Text = String.Format((string)Session["LabelText"], (int)Session["CurrentRow"] + 1, ds.Tables[0].Rows.Count, DateTime.Now.ToString("hh:mm:ss"));
                        UploadOneIndividualOverview((object)dgItem[1]);
                        Client_WCF.UpdatePlayerListAlphabeticalOverview((int)Session["Competition_Identification"], (int)dgItem[1], "U", 1);
                        //
                        // Set up for next timer click
                        //
                        Session["CurrentRow"] = (int)Session["CurrentRow"] + 1;
                    }
                }
                if ((int)Session["CurrentRow"] == (int)Session["MaximumRow"])
                {
                    Button11.Visible = true;
                    Button2.Visible = false;
                    Refresh.Enabled = false;
                    RefreshDisplay();
                    Client_WCF.Update_Workflow_Item("[Publiceren]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
                    Thread.Sleep(1000);
                    if ((bool)Session["AutoOverview"])
                    {
                        Response.Redirect("TSGS_CS_Players_With_Bye_List.aspx");
                    }
                    else
                    {
                        Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
                    }
                }
                else
                {
                    RefreshDisplay();
                }

            }
            Client_WCF.Close();
        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

    }
}