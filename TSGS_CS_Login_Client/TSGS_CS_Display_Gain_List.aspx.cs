using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Display_Gain_List : System.Web.UI.Page
    {
        public enum FileSwitch
        {
            COMPETITION, OUTOFCOMPETITION, CHAMPIONSGROUP
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "DisplayGainList";
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
                Client_WCF.Remove_Templist((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                Client_WCF.Create_Worklist((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                Client_WCF.Close();
                Fill_Texts();
                if ((bool)Session["AutoOverview"])
                {
                    Response.Redirect("TSGS_CS_Display_Participant_List.aspx");
                }
                else
                {
                    // Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
                }
            }
        }

        protected void Fill_Texts()
        {

            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            Label3.Text = (string)Session["Competition"];
            Label4.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim(), (int)Session["Round_Number"]);
            Label5.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataSet ds = Client_WCF.GetCompetitionGainRankingList((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
            //
            // create a dataset to display on screen and add PL to both datasets
            //
            DataTable Table1 = new DataTable("Players");
            Table1.Columns.Add("PL");
            Table1.Columns.Add("imgPicture");
            Table1.Columns.Add("Spelernaam");
            Table1.Columns.Add("Gain_Loss");

            DataSet Set = new DataSet("DisplayScreen");

            ds.Tables[0].Columns.Add("PL", typeof(string));

            //  Cycle through DataSet to determine position
            int Position = 0;

            if (ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Position++;
                    ds.Tables[0].Rows[i]["PL"] = Position.ToString();
                    Table1.Rows.Add(Position.ToString(), "x", ds.Tables[0].Rows[i]["SpelerNaam"], String.Format("{0:#00.0}", ds.Tables[0].Rows[i]["Gain_Loss"]));
                }
            }
            //
            string CG_String = Client_MLC.GetMLCText((string)Session["Project"], "DisplayCPList", (int)Session["Language"], 10).Trim();
            int PID = 0;
            int Loop_Count = 1;

            int MaxAbsentsToCompete = Client_WCF.GetMaxRounds((int)Session["Competition_Identification"]) / 2;
            //
            // as long as we have groups, create lists
            //
            do
            {
                Position = 0;
                ds.Tables[Loop_Count].Columns.Add("PL", typeof(string));
                for (int i = 0; i < ds.Tables[Loop_Count].Rows.Count; i++) 
                {
                    PID = Convert.ToInt16(ds.Tables[Loop_Count].Rows[i]["Speler_ID"]);
                    if (Client_WCF.Count_Player_Absent(PID, (int)Session["Competition_Identification"], (int)Session["Round_Number"]) > MaxAbsentsToCompete)
                    {
                        ds.Tables[Loop_Count].Rows[i]["PL"] = "*";
                        Table1.Rows.Add("*", "x", ds.Tables[Loop_Count].Rows[i]["SpelerNaam"], String.Format("{0:#00.0}", ds.Tables[Loop_Count].Rows[i]["Gain_Loss"]));
                    }
                    else
                    {
                        Position++;
                        ds.Tables[Loop_Count].Rows[i]["PL"] = Position.ToString();
                        Table1.Rows.Add(Position.ToString(), "x", ds.Tables[Loop_Count].Rows[i]["SpelerNaam"], String.Format("{0:#00.0}", ds.Tables[Loop_Count].Rows[i]["Gain_Loss"]));
                    }
                }
                Loop_Count++;
            }
            while (Loop_Count < ds.Tables.Count);

            ViewState["DSList"] = ds;

            Set.Tables.Add(Table1);
            
            GridView1.DataSource = Set;
            GridView1.AllowPaging = true;
            GridView1.PageSize = 20;
            GridView1.DataBind();

            GridViewRow row1Header = GridView1.HeaderRow;
            row1Header.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            row1Header.Cells[0].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            row1Header.Cells[2].HorizontalAlign = HorizontalAlign.Left;
            row1Header.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            row1Header.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            
            Client_WCF.Close();
            Client_MLC.Close();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Fill_Texts();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Button1_Click_Actions(null, null);
        }

        protected void Button1_Click_Actions(object sender, EventArgs e)
        {
            int Position = 0;

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            FileSwitch Line_Selector = FileSwitch.COMPETITION;

            string RootPath =  Server.MapPath("~/htmlfiles/"+Convert.ToString((int)Session["Competition_Identification"]).Trim());
            string DisplayHeader = "TSGS_CS_CompetitionGain_Rankinglist.html";
            string DisplayBodyC = "CompetitionGain_Rankinglist_C_body.js";
            string DisplayBodyOOC = "CompetitionGain_Rankinglist_OOC_body.js";
            string DisplayBodyCG = "CompetitionGain_Rankinglist_CG_body.js";
            string DisplayBody = "CompetitionGain_Rankinglist_body.js";

            string Output_Line = "";

            StreamWriter Container = new StreamWriter(RootPath + @"\" + DisplayHeader);
            StreamWriter Content = new StreamWriter(RootPath + @"\" + DisplayBodyC);
            StreamWriter OOC = new StreamWriter(RootPath + @"\" + DisplayBodyOOC);
            StreamWriter CG = new StreamWriter(RootPath + @"\" + DisplayBodyCG);

            string doc_type = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";

            Container.WriteLine(doc_type);
            Container.WriteLine("<html xmlns=\"http://www.w3.org/1999/shtml\">");
            Container.WriteLine("<head>");
            Container.WriteLine("<title>" + Label4.Text + "</title>");
            Container.WriteLine("<meta http-equiv='content-type' CONTENT='text/html; charset=ISO-8859-1'>");
            Container.WriteLine("<meta http-equiv=\"expires\" content=\"0\">");
            string path_Template = Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_Template").Trim();
            Container.WriteLine("<link href='" + path_Template + "/modern.css' type=\"text/css\" rel=\"stylesheet\">");
            Container.WriteLine("</head>");
            Container.WriteLine("<base target='main'>");
            Container.WriteLine("<body>");
            Container.WriteLine("<hr size=4 Width='75%'>");

            CG.WriteLine("document.write(\"\\");

            CG.WriteLine("<div id='printable' class='PrintSettings'>\\");
            CG.WriteLine("<h1 align=left>" + (string)Session["Club"]+", "+Label3.Text+ "</h1>\\");
            CG.WriteLine("<h2 align=left>" + Label4.Text + "</h2>\\");
            OOC.WriteLine("<h1 align=left>" + (string)Session["Club"] + ", " + Label3.Text + "</h1>\\");
            OOC.WriteLine("<h2 align=left>Out of Competition</h2>\\");

            DataSet ds = (DataSet)ViewState["DSList"];
            GridViewRow row1Header = GridView1.HeaderRow;

            Output_Line = "<br><table border='0\'><tr>";
            Output_Line += "<td width='4%' align='right'>" + row1Header.Cells[0].Text + "</td>";
            Output_Line += "<td width='8%' align='center'>&nbsp;</td>";
            Output_Line += "<td width='50%' align='left'>" + row1Header.Cells[2].Text + "</td>";
            Output_Line += "<td width='4%' align='right'>" + row1Header.Cells[3].Text + "</td>";
            Output_Line += "<td width='34%' align='right'>&nbsp;</td>";
            Output_Line += "</tr><tr><td height='8'>&nbsp;<br></td></tr>\\";
            CG.WriteLine(Output_Line);
            OOC.WriteLine(Output_Line);


            int PID = 0;
            if (ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    PID = Convert.ToInt16(ds.Tables[0].Rows[i]["Speler_Id"]);
                    Output_Line = "<tr><td width='4%' align='right'>" + ds.Tables[0].Rows[i]["PL"] + "</td>";
                    if (i<3)
                    {
                        Output_Line += "<td width='8%' align='center'><img src='" + Client_WCF.StringImage(PID, (int)Session["Competition_Identification"]) + "'></td>";
                    }
                    else
                    {
                        Output_Line += "<td width='8%' align='center'>&nbsp</td>";
                    }
                    Output_Line += "<td width='50%' align='left'>" + ds.Tables[0].Rows[i]["Spelernaam"] + "</h2></td>";
                    Output_Line += "<td width='4%' align='right'>" + String.Format("{0:#00.0}", ds.Tables[0].Rows[i]["Gain_Loss"]) + "</td>";
                    Output_Line += "<td width='32%' align='right'>&nbsp;</td>";
                    Output_Line += "</tr>\\";
                    CG.WriteLine(Output_Line);
                }
            }
            //
            int Loop_Count = 1;
            string Header_GR_String = "";
            //
            // as long as we have groups, create lists
            //
            do
            {
                Position = 0;

                Content.WriteLine("<h1 align=left>" + (string)Session["Club"] + ", " + Label3.Text + "</h1>\\");
                if (ds.Tables.Count <= 2)
                {
                    Header_GR_String = "";
                }
                else
                {
                    Header_GR_String = ", " + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim() + " " + Loop_Count.ToString();
                }

                Content.WriteLine("<h2 align=left>" + Label4.Text + Header_GR_String + "</h2>\\");

                Output_Line = "<br><table border='0\'><tr>";
                Output_Line += "<td width='4%' align='right'>" + row1Header.Cells[0].Text + "</td>";
                Output_Line += "<td width='8%' align='center'>&nbsp;</td>";
                Output_Line += "<td width='50%' align='left'>" + row1Header.Cells[2].Text + "</td>";
                Output_Line += "<td width='4%' align='right'>"+row1Header.Cells[3].Text+"</td>";
                Output_Line += "<td width='34%' align='right'>&nbsp;</td>";
                Output_Line += "</tr><tr><td height='8'>&nbsp;<br></td></tr>\\";
                Content.WriteLine(Output_Line);

                for (int i = 0; i < ds.Tables[Loop_Count].Rows.Count; i++)
                {
                    if ((string)ds.Tables[Loop_Count].Rows[i]["PL"] == "*")
                    {
                        Line_Selector = FileSwitch.OUTOFCOMPETITION;
                        Output_Line = "<tr><td width='4%' align='right'>*</td>";
                    }
                    else
                    {
                        if (((int)ds.Tables[Loop_Count].Rows[i]["Aantal_Partijen"] == 0) && ((int)Session["Round_Number"] >= 2))
                        {
                            Line_Selector = FileSwitch.OUTOFCOMPETITION;
                            Output_Line = "<tr><td width='4%' align='right'>*</td>";                      
                        }
                        else
                        {
                            Position++;
                            Line_Selector = FileSwitch.COMPETITION;
                            Output_Line = "<tr><td width='4%' align='right'>"+Position.ToString()+"</td>";  
                        }
                    }
                    if (Position<4 && Line_Selector == FileSwitch.COMPETITION)
                    {
                        PID = Convert.ToInt16(ds.Tables[Loop_Count].Rows[i]["Speler_ID"]);
                        Output_Line += "<td width='8%' align='center'><img src='" + Client_WCF.StringImage(PID, (int)Session["Competition_Identification"]) + "'></td>";
                    }
                    else
                    {
                        Output_Line += "<td width='8%' align='center'>&nbsp</td>";
                    }
                    Output_Line += "<td width='50%' align='left'>" + ds.Tables[Loop_Count].Rows[i]["Spelernaam"] + "</td>";
                    Output_Line += "<td width='4%' align='right'>" + String.Format("{0:#00.0}", ds.Tables[Loop_Count].Rows[i]["Gain_Loss"]) + "</td>";
                    Output_Line += "<td width='32%' align='right'>&nbsp;</td>";
                    Output_Line += "</tr>\\";
                    //
                    // Line is created, now put it in the correct file
                    //
                    switch (Line_Selector)
                    {
                        case FileSwitch.OUTOFCOMPETITION:
                            {
                                OOC.WriteLine(Output_Line);
                                break;
                            }
                        case FileSwitch.COMPETITION:
                            {
                                Content.WriteLine(Output_Line);
                                break;
                            }
                    }
                }
                Content.WriteLine("</table>\\");
                Content.WriteLine("<br />\\");
                Content.WriteLine("<br />\\");
                Loop_Count++;
            }
            while (Loop_Count < ds.Tables.Count);
            
            CG.WriteLine("</table>\\");
            CG.WriteLine("<br />\\");
            CG.WriteLine("<br />\\");

            OOC.WriteLine("</table>\\");
            OOC.WriteLine("</div>\\");
            OOC.WriteLine("<br />\\");
            OOC.WriteLine("<br />\\");
            OOC.WriteLine("<input id=\"buttonprint\" type=\"button\" value=\"" + Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 11).Trim() + "\" onclick=\"PrintElem();\">\\");

            OOC.WriteLine("\");");

            Container.WriteLine("    <script src=\"" + DisplayBody + "\"></script>");
            Container.WriteLine("</body>");
            Container.WriteLine("</html>");
            Container.Close();
            Content.Close();
            CG.Close();
            OOC.Close();
            //
            // Concatenate files
            //
            string[] srcFileNames = { RootPath + "\\" + DisplayBodyCG, RootPath + "\\" + DisplayBodyC, RootPath + "\\" + DisplayBodyOOC };
            string destFileName = RootPath + @"\" + DisplayBody;

            if (System.IO.File.Exists(destFileName))
            {
                System.IO.File.Delete(destFileName);
            }

            using (Stream destStream = File.OpenWrite(destFileName))
            {
                foreach (string srcFileName in srcFileNames)
                {
                    using (Stream srcStream = File.OpenRead(srcFileName))
                    {
                        srcStream.CopyTo(destStream);
                    }
                }
            }
            //
            // Upload files to website
            //            
            string Display_Competition = Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_basis").Trim() + "/" +
                                         Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_Competitie").Trim();
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
            Client_WCF.Update_Workflow_Item("[Afdrukken Winst Verlies lijst]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
            Client_WCF.Update_Workflow_Item("[Publiceren]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);

            Client_WCF.Close();
            Client_MLC.Close();

            if ((bool)Session["AutoOverview"])
            {
                Response.Redirect("TSGS_CS_Display_Games.aspx");
            }
            else
            {
                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                e.Row.Cells[0].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[1].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[2].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[3].Font.Size = (int)Session["Fontsize"];
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }
    }
}
