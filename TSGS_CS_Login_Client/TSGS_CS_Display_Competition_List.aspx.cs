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
    public partial class TSGS_CS_Display_Competition_List : System.Web.UI.Page
    {
        public enum FileSwitch
        {
            COMPETITION, OUTOFCOMPETITION, CHAMPIONSGROUP
        }

        List<Class_PairingListdata> PairingList = new List<Class_PairingListdata>();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "DisplayCPList";
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
                Client_WCF.SortMatrix((int)Session["Competition_Identification"]);
                Fill_Texts();
                if ((bool)Session["AutoOverview"])
                {
                    Button1_Click_Actions(null, null);
                }
                Client_WCF.Close();
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
            DataSet ds = Client_WCF.GetCompetitionRankingList((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
            //
            // create a dataset to display on screen and add PL to both datasets
            //
            DataTable Table1 = new DataTable("Players");
            Table1.Columns.Add("PL");
            Table1.Columns.Add("imgPicture");
            Table1.Columns.Add("Spelernaam");
            Table1.Columns.Add("Competition_Points");
            Table1.Columns.Add("Aantal_Punten");
            Table1.Columns.Add("Aantal_Partijen");
            Table1.Columns.Add("Percentage");
            Table1.Columns.Add("Member_Premier_Group");

            DataSet Set = new DataSet("DisplayScreen");

            ds.Tables[0].Columns.Add("PL", typeof(string));

            string CG_String = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            //
            //            Cycle through DataSet to determine position
            //
            int Position = 0;
            int KG = 0;
            int MaxAbsentsToCompete = Client_WCF.GetMaxRounds((int)Session["Competition_Identification"]) / 2;
            int PID = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                PairingList.Add(new Class_PairingListdata()
                    {
                        PlayerName = Convert.ToString(ds.Tables[0].Rows[i]["SpelerNaam"]),
                        CompetitionPoints = Convert.ToDouble(ds.Tables[0].Rows[i]["Competition_Points"])
                    }
                    );
                KG = Convert.ToInt16(ds.Tables[0].Rows[i]["Member_Premier_Group"]);
                PID = Convert.ToInt16(ds.Tables[0].Rows[i]["Speler_ID"]);
                Position++;
                ds.Tables[0].Rows[i]["PL"] = Position;
                Table1.Rows.Add(Position.ToString(), 
                                "x", 
                                ds.Tables[0].Rows[i]["SpelerNaam"], 
                                String.Format("{0:#00.0}", ds.Tables[0].Rows[i]["Competition_Points"]),
                                String.Format("{0:#0.0}", ds.Tables[0].Rows[i]["Aantal_Punten"]),
                                String.Format("{0:#0}", ds.Tables[0].Rows[i]["Aantal_Partijen"]),
                                String.Format("{0:#00.0}", ds.Tables[0].Rows[i]["Percentage"]),
                                String.Format("{0:#00}", ds.Tables[0].Rows[i]["Member_Premier_Group"]));
            }
            int Loop_Count = 1;

            //
            // as long as we have groups, create lists
            //
            do
            {
                ds.Tables[Loop_Count].Columns.Add("PL", typeof(string));
                Position = 0;
                for (int i = 0; i < ds.Tables[Loop_Count].Rows.Count; i++) 
                {
                    PairingList.Add(new Class_PairingListdata()
                    {
                        PlayerName = Convert.ToString(ds.Tables[Loop_Count].Rows[i]["SpelerNaam"]),
                        CompetitionPoints = Convert.ToDouble(ds.Tables[Loop_Count].Rows[i]["Competition_Points"])
                    }
                    );
                    PID = Convert.ToInt16(ds.Tables[Loop_Count].Rows[i]["Speler_ID"]);
                    if (Client_WCF.Count_Player_Absent(PID, (int)Session["Competition_Identification"], (int)Session["Round_Number"]) > MaxAbsentsToCompete)
                    {
                        ds.Tables[Loop_Count].Rows[i]["PL"] = "*";
                        Table1.Rows.Add("*",
                                        "x",
                                        ds.Tables[Loop_Count].Rows[i]["SpelerNaam"],
                                        String.Format("{0:#00.0}", ds.Tables[Loop_Count].Rows[i]["Competition_Points"]),
                                        String.Format("{0:#0.0}", ds.Tables[Loop_Count].Rows[i]["Aantal_Punten"]),
                                        String.Format("{0:#0}", ds.Tables[Loop_Count].Rows[i]["Aantal_Partijen"]),
                                        String.Format("{0:#00.0}", ds.Tables[Loop_Count].Rows[i]["Percentage"]),
                                        String.Format("{0:#00}", ds.Tables[Loop_Count].Rows[i]["Member_Premier_Group"]));
                    }
                    else
                    {
                        Position++;
                        ds.Tables[Loop_Count].Rows[i]["PL"] = Position.ToString();
                        Table1.Rows.Add(Position.ToString(),
                                        "x",
                                        ds.Tables[Loop_Count].Rows[i]["SpelerNaam"],
                                        String.Format("{0:#00.0}", ds.Tables[Loop_Count].Rows[i]["Competition_Points"]),
                                        String.Format("{0:#0.0}", ds.Tables[Loop_Count].Rows[i]["Aantal_Punten"]),
                                        String.Format("{0:#0}", ds.Tables[Loop_Count].Rows[i]["Aantal_Partijen"]),
                                        String.Format("{0:#00.0}", ds.Tables[Loop_Count].Rows[i]["Percentage"]),
                                        String.Format("{0:#00}", ds.Tables[Loop_Count].Rows[i]["Member_Premier_Group"]));
                    }
                }
                Loop_Count++;
            }
            while (Loop_Count < ds.Tables.Count);

            PairingList.Sort((x, y) => y.CompetitionPoints.CompareTo(x.CompetitionPoints));

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
            row1Header.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            row1Header.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[5].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            row1Header.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[6].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            row1Header.Cells[6].HorizontalAlign = HorizontalAlign.Right;

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

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            int Position = 0;
            int CGGroup_Id = 0;

            FileSwitch Line_Selector = FileSwitch.COMPETITION;

            string RootPath =  Server.MapPath("~/htmlfiles/"+Convert.ToString((int)Session["Competition_Identification"]).Trim());
            string DisplayHeader = "TSGS_CS_Competition_Rankinglist.html";
            string DisplayBodyC = "Competition_Rankinglist_C_body.js";
            string DisplayBodyOOC = "Competition_Rankinglist_OOC_body.js";
            string DisplayBodyCG = "Competition_Rankinglist_CG_body.js";
            string DisplayBody = "Competition_Rankinglist_body.js";
            string DisplayHeaderFL = "TSGS_CS_Competition_Freelist.html";
            string DisplayBodyFL = "Competition_Freelist_body.js";
            string DisplayHeaderPL = "TSGS_CS_Competition_Pairinglist.html";
            string DisplayBodyPL = "Competition_Pairing_body.js"; 
            
            string Output_Line = "";
            string Output_Line_Part2 = "";

            StreamWriter Container = new StreamWriter(RootPath + @"\" + DisplayHeader);
            StreamWriter ContainerPL = new StreamWriter(RootPath + @"\" + DisplayHeaderPL);
            StreamWriter ContainerFL = new StreamWriter(RootPath + @"\" + DisplayHeaderFL);
            StreamWriter Content = new StreamWriter(RootPath + @"\" + DisplayBodyC);
            StreamWriter OOC = new StreamWriter(RootPath + @"\" + DisplayBodyOOC);
            StreamWriter CG = new StreamWriter(RootPath + @"\" + DisplayBodyCG);
            StreamWriter FL = new StreamWriter(RootPath + @"\" + DisplayBodyFL);
            StreamWriter PL = new StreamWriter(RootPath + @"\" + DisplayBodyPL);

            string doc_type = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";
            string CG_String = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            string Header_CG_String = ", " + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 14).Trim();
            string Header_GR_String = ", " + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim();

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
            //
            // free round list
            //
            ContainerFL.WriteLine(doc_type);
            ContainerFL.WriteLine("<html xmlns=\"http://www.w3.org/1999/shtml\">");
            ContainerFL.WriteLine("<head>");
            ContainerFL.WriteLine("<title>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 19).Trim() + "</title>");
            ContainerFL.WriteLine("<meta http-equiv='content-type' CONTENT='text/html; charset=ISO-8859-1'>");
            ContainerFL.WriteLine("<meta http-equiv=\"expires\" content=\"0\">");
            ContainerFL.WriteLine("<link href='" + path_Template + "/modern.css' type=\"text/css\" rel=\"stylesheet\">");
            ContainerFL.WriteLine("</head>");
            ContainerFL.WriteLine("<base target='main'>");
            ContainerFL.WriteLine("<body>");
            ContainerFL.WriteLine("<hr size=4 Width='75%'>");
            ContainerFL.WriteLine("    <script src=\"" + DisplayBodyFL + "\"></script>");
            ContainerFL.WriteLine("</body>");
            ContainerFL.WriteLine("</html>");
            ContainerFL.Close();

            //
            // Pairing list header
            //
            ContainerPL.WriteLine(doc_type);
            ContainerPL.WriteLine("<html xmlns=\"http://www.w3.org/1999/shtml\">");
            ContainerPL.WriteLine("<head>");
            ContainerPL.WriteLine("<title>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 21).Trim() + Session["Round_Number"].ToString() + "</title>");
            ContainerPL.WriteLine("<meta http-equiv='content-type' CONTENT='text/html; charset=ISO-8859-1'>");
            ContainerPL.WriteLine("<meta http-equiv=\"expires\" content=\"0\">");
            ContainerPL.WriteLine("<link href='" + path_Template + "/modern.css' type=\"text/css\" rel=\"stylesheet\">");
            ContainerPL.WriteLine("</head>");
            ContainerPL.WriteLine("<base target='main'>");
            ContainerPL.WriteLine("<body>");
            ContainerPL.WriteLine("<hr size=4 Width='75%'>");
            ContainerPL.WriteLine("    <script src=\"" + DisplayBodyFL + "\"></script>");
            ContainerPL.WriteLine("</body>");
            ContainerPL.WriteLine("</html>");
            ContainerPL.Close();

            DataSet ds = (DataSet)ViewState["DSList"];
            GridViewRow row1Header = GridView1.HeaderRow;

            CG.WriteLine("document.write(\"\\");
            FL.WriteLine("document.write(\"\\");
            PL.WriteLine("document.write(\"\\");
            //
            // Print Crown Group
            //
            int PID = 0;
            int CGCount = ds.Tables[0].Rows.Count;
            if (CGCount != 0)
            {
                CGGroup_Id = Client_WCF.GetCGTournamentIdFrom((int)Session["Competition_Identification"]);
                //
                // first get headers
                //
                DataSet dsR = Client_WCF.GetCrossTableRow(CGGroup_Id, 0);
                //
                // display Championsgroup overview overall
                //
                CG.WriteLine("<div id='printable' class='PrintSettings'>\\");
                CG.WriteLine("<h1 align=left>" + (string)Session["Club"] + ", " + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 16).Trim() + "</h1>\\");
                Output_Line = "<br><table border='1px'>";
                CG.WriteLine(Output_Line);

                int ii = 0;
                foreach (DataRow dgItem in dsR.Tables[0].Rows)
                {
                    if (ii == 0)
                    {
                        Output_Line = "<tr><td width='4%' align='center'>&nbsp;</td><td width='30%' align='left'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 17).Trim() + "</td>";
                    }
                    else if (ii <= CGCount + 1)
                    {
                        Output_Line += "<td width='4%' align='center'>" + Convert.ToString(dgItem["Column_Name"]).Trim() + "</td>";
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
                    dsR = Client_WCF.GetCrossTableRow(CGGroup_Id, j);
                    ii = 0;
                    foreach (DataRow dgItem in dsR.Tables[0].Rows)
                    {
                        if (ii == 0)
                        {
                            Output_Line_Part2 = "</td><td align='left' width='4%' >" + dgItem["Row_Name"].ToString().Trim() + "</td>";
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
                                    Output_Line_Part2 += "<td align='center' style='background-color:grey'><img src='../images\\scdlogodoorzichtig.gif' height='16' ></td>";
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

                OOC.WriteLine("<h1 align=left>" + (string)Session["Club"] + ", " + Label3.Text + "</h1>\\");
                OOC.WriteLine("<h2 align=left>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 13).Trim() + "</h2>\\");
                OOC.WriteLine("<br><table border='0\'>");
            }
            int Loop_Count = 0;
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
                    Header_GR_String =  ", " + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 18).Trim() + " " + Loop_Count.ToString();
                }

                Content.WriteLine("<h2 align=left>" + Label4.Text + Header_GR_String + "</h2>\\");

                Output_Line = "<br><table border='0\'>";
                Output_Line += "<tr>";
                Output_Line += "<td width='4%' align='right'>" + row1Header.Cells[0].Text + "</td>";
                Output_Line += "<td width='8%' align='center'>&nbsp;</td>";
                Output_Line += "<td width='30%' align='left'>" + row1Header.Cells[2].Text + "</td>";
                Output_Line += "<td width='4%' align='right'>" + row1Header.Cells[3].Text + "</td>";
                Output_Line += "<td width='4%' align='right'>" + row1Header.Cells[4].Text + "</td>";
                Output_Line += "<td width='4%' align='right'>" + row1Header.Cells[5].Text + "</td>";
                Output_Line += "<td width='4%' align='right'>" + row1Header.Cells[6].Text + "</td>";
                Output_Line += "<td width='42%' align='right'>&nbsp;</td></tr><tr><td height='8'>&nbsp;<br></td></tr>\\";
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
                    if (Position < 4 && Line_Selector == FileSwitch.COMPETITION)
                    {
                        PID = Convert.ToInt16(ds.Tables[Loop_Count].Rows[i]["Speler_ID"]);
                        Output_Line += "<td width='8%' align='center'><img src='" + Client_WCF.StringImage(PID, (int)Session["Competition_Identification"]) + "'></td>";
                    }
                    else
                    {
                        Output_Line += "<td width='8%' align='center'>&nbsp</td>";
                    }
                    Output_Line += "<td width='30%' align='left'>" + ds.Tables[Loop_Count].Rows[i]["SpelerNaam"] + "</td>";
                    Output_Line += "<td width='4%' align='right'>" + String.Format("{0:#000.0}", ds.Tables[Loop_Count].Rows[i]["Competition_Points"]) + "</td>";
                    Output_Line += "<td width='4%' align='right'>" + String.Format("{0:#0.0}", ds.Tables[Loop_Count].Rows[i]["Aantal_Punten"]) + "</td>";
                    Output_Line += "<td width='4%' align='right'>" + String.Format("{0:#}", ds.Tables[Loop_Count].Rows[i]["Aantal_Partijen"]) + "</td>";
                    Output_Line += "<td width='4%' align='right'>" + String.Format("{0:#0.0}", ds.Tables[Loop_Count].Rows[i]["Percentage"]) + "</td>";
                    Output_Line += "<td width='42%' align='right'>&nbsp;</td>";
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
            // Create the Player free list
            //
            Client_WCF.Create_Worklist((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
            DataSet dsFL = Client_WCF.List_Free_Round_Players((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
            //
            // display Player free list
            //
            FL.WriteLine("<div id='printable' class='PrintSettings'>\\");
            FL.WriteLine("<h1 align=left>" + (string)Session["Club"] + ", " + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 19).Trim() + "</h1>\\");
            Output_Line = "<br><table border='1px'>";
            FL.WriteLine(Output_Line);
            int loop = 0;
            Output_Line = "<tr>" + 
                "<td width='4%' align='center'>&nbsp;</td>" + 
                "<td width='30%' align='left'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 17).Trim() + "</td>" + 
                "<td>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 20).Trim() + "</td>" + 
                "</tr>";
            FL.WriteLine(Output_Line);
            //
            // Now same for all data rows
            //
            foreach (DataRow dgItem in dsFL.Tables[0].Rows)
            {
                loop++;
                Output_Line = "<tr>" +
                    "<td align='left' width='4%' >" + loop.ToString() + "</td>" +
                    "<td>" + Client_WCF.GetPlayerName((int)dgItem["Speler_Id"]) + "</td>" +
                    "<td>" + String.Format("{0:M/d/yyyy}", (DateTime)dgItem["Date_Last_Free_Round"]) + "</td>" +
                    "</tr>";
                FL.WriteLine(Output_Line);
            }
            FL.WriteLine("</table>\\");
            FL.WriteLine("<br />\\");
            FL.WriteLine("<br />\\");
            FL.WriteLine("\");");
            FL.Flush();
            FL.Close();

            //
            //  Create pairing list file
            //
            PL.WriteLine("<div id='printable' class='PrintSettings'>\\");
            PL.WriteLine("<h1 align=left>" + (string)Session["Club"] + ", " + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 21).Trim() + Session["Round_Number"].ToString() +"</h1>\\");
            Output_Line = "<br><table border='1px'>";
            PL.WriteLine(Output_Line);
            int PositionPL = 0;
            Output_Line = "<tr>" +
                "<td width='4%' align='center'>&nbsp;</td>" +
                "<td width='30%' align='left'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 21).Trim() + "</td>" +
                "<td>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim() + "</td>" +
                "</tr>";
            PL.WriteLine(Output_Line);
            //
            // Now same for all data rows
            //

            foreach (Class_PairingListdata oneEntry in PairingList)
            {
                PositionPL++;
                Output_Line = "<tr>" +
                    "<td align='left' width='4%' >" + loop.ToString() + "</td>" +
                    "<td>" + oneEntry.PlayerName + "</td>" +
                    "<td>" + String.Format("{0:#000.0}", oneEntry.CompetitionPoints)+ "</td>" +
                    "</tr>";
                PL.WriteLine(Output_Line);
                    ;
            }
            PL.WriteLine("</table>\\");
            PL.WriteLine("<br />\\");
            PL.WriteLine("<br />\\");
            PL.WriteLine("\");");
            PL.Flush();
            PL.Close();
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

            //
            // Upload files to website
            //
            string UploadHeaderFL = Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], DisplayHeaderFL);
            if (UploadHeader != "")
            {
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                Label1.Text += UploadHeader;
                Label1.Visible = true;
            }

            String UploadBodyFL = Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], DisplayBodyFL);
            if (UploadHeader != "")
            {
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                Label1.Text += UploadBody;
                Label1.Visible = true;
            }

            string UploadHeaderPL = Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], DisplayHeaderPL);
            if (UploadHeaderPL != "")
            {
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                Label1.Text += UploadHeaderPL;
                Label1.Visible = true;
            }

            String UploadBodyPL = Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], DisplayBodyPL);
            if (UploadHeaderPL != "")
            {
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                Label1.Text += UploadBodyPL;
                Label1.Visible = true;
            }


            Client_WCF.Update_Workflow_Item("[Afdrukken Competitie Ranglijst]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
            Client_WCF.Update_Workflow_Item("[Publiceren]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
            Client_WCF.Close();
            Client_MLC.Close();

            if ((bool)Session["AutoOverview"])
            {
                Response.Redirect("TSGS_CS_Display_Gain_List.aspx");
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
                e.Row.Cells[4].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[5].Font.Size = (int)Session["Fontsize"];
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }
    }
}
