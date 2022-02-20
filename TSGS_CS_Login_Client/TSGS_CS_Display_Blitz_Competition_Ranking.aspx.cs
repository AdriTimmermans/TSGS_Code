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
    public partial class TSGS_CS_Display_Blitz_Competition_Ranking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "DisplayBlitzComp";
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
                Label1.Visible = false;
                /*
                other initialising stuff for function <xxxx>
                */

                Build_Dataset(); 
                Fill_Texts();
            }
        }

        protected void Build_Dataset()
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            bool SQLError = Client_WCF.PrepareBlitzRanking((int)Session["Competition_Identification"]);
            DataSet dsP = new DataSet();
            dsP.Tables.Add();
            dsP.Tables[0].Columns.Add("PL", typeof(int));
            dsP.Tables[0].Columns.Add("Foto", typeof(string));
            dsP.Tables[0].Columns.Add("Speler", typeof(string));
            dsP.Tables[0].Columns.Add("PuntenClean", typeof(float));
            dsP.Tables[0].Columns.Add("PuntenRaw", typeof(float));
            int Aantal_ronden = Client_WCF.GetIntFromAlgemeneInfo((int)Session["Competition_Identification"], "Aantal_ronden");
            for (int i = 0; i<Aantal_ronden; i++)
            {
                dsP.Tables[0].Columns.Add("R"+(i+1).ToString(), typeof(string));
            }

            DataSet ds = Client_WCF.GetPlayersUniqueBlitz((int)Session["Competition_Identification"]);
            DataTable dt = ds.Tables[0];
            int nextrow = 0;
            for (int i=0;i<dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                int PID = Convert.ToInt32(dr[0]);
                TSGS_CS_WCF_Service.PlayerBasicData Player = new TSGS_CS_WCF_Service.PlayerBasicData();
                Player = Client_WCF.GetPlayerFullData(PID, (int)Session["Competition_Identification"]);
                string Name = Client_WCF.GetPlayerName(PID);
                string Picture = "<img src='" + Client_WCF.StringImage(PID, (int)Session["Competition_Identification"])+ "'>'";
                float CleanPoints = Client_WCF.GetBlitzPenaltyPointsCleaned((int)Session["Competition_Identification"], PID);
                float RawPoints = Client_WCF.GetBlitzPenaltyPoints((int)Session["Competition_Identification"], PID);
                if (RawPoints < ((int)Session["Round_number"] * 1000.0 - 1.0))
                {
                    dsP.Tables[0].Rows.Add(i + 1, Picture, Name, CleanPoints, RawPoints);
                    DataSet dsDPts = Client_WCF.GetBlitzDisplayPoints((int)Session["Competition_Identification"], PID);
                    for (int j = 0; j < dsDPts.Tables[0].Rows.Count; j++)
                    {
                        dsP.Tables[0].Rows[nextrow][j + 5] = dsDPts.Tables[0].Rows[j][0].ToString().Trim();
                    }
                    nextrow++;
                }
            }
            dsP.AcceptChanges();
            ViewState["DSList"] = dsP;

            Client_WCF.Close();
        }

        protected void Fill_Texts()
        {

            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            Label1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            Label3.Text = (string)Session["Club"];
            Label4.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim(),(int)Session["Round_number"]);

            DataSet ds = new DataSet();
            ds = (DataSet)ViewState["DSList"];
            DataView dv = new DataView(ds.Tables[0]);
            dv.Sort = " PuntenClean ASC, PuntenRaw ASC";
            GridView1.DataSource = dv;
            GridView1.AllowPaging = true;
            GridView1.PageSize = 10;
            GridView1.DataBind();

            GridViewRow row1Header = GridView1.HeaderRow;
            row1Header.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            row1Header.Cells[0].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            row1Header.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            row1Header.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            row1Header.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            row1Header.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            row1Header.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[5].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
            row1Header.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[6].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim();
            row1Header.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[7].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 13).Trim();
            row1Header.Cells[7].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[8].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 14).Trim();
            row1Header.Cells[8].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[9].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim();
            row1Header.Cells[9].HorizontalAlign = HorizontalAlign.Right;

            Client_MLC.Close();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Fill_Texts();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int Gridline = e.Row.RowIndex + GridView1.PageSize * GridView1.PageIndex;
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                for (int i = 0; i < 10; i++)
                {
                    e.Row.Cells[i].Font.Size = (int)Session["Fontsize"];
                }
                e.Row.Cells[0].Text = (Gridline + 1).ToString();
            }
        }


        protected void Button2_Click(object sender, EventArgs e)
        {
            /*
            trigger actions for main function of form
            */
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataSet ds = (DataSet)ViewState["DSList"];
            DataView dv = new DataView(ds.Tables[0]);
            dv.Sort = "PuntenClean ASC, PuntenRaw ASC";
            //
            // Create overview
            //
            string RootPath =  Server.MapPath("~/htmlfiles/"+Convert.ToString((int)Session["Competition_Identification"]).Trim());
            string DisplayHeader = "TSGS_CS_Display_Blitz_Competition_Ranking.html";
            string DisplayBody = "Display_Blitz_Competition_Ranking_body.js";

            StreamWriter Container = new StreamWriter(RootPath + @"\" + DisplayHeader);
            StreamWriter Content = new StreamWriter(RootPath + @"\" + DisplayBody);

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
            Content.WriteLine("document.write(\"\\");
            Content.WriteLine("<div id='printable' class='PrintSettings'>\\");
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            Content.WriteLine("<h1 align=left>" + (string)Session["Club"] + ", " + (string)Session["Competition"] + "</h1>\\");
            Content.WriteLine("<h2 align=left>" + string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 16).Trim(), (int)Session["Round_Number"]) + "</h2>\\");
            Content.WriteLine("<br>\\");
            Content.WriteLine("<table border='0\'>\\");

            Content.Write("<tr>");

            Content.Write("<td width='2%' align='right'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim() + "</td>");
            Content.Write("<td width='8%' align='right'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim() + "</td>");
            Content.Write("<td width='30%'>&nbsp;</td>");
            Content.Write("<td width='5%' align='right'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim() + "</td>");
            Content.Write("<td width='5%' align='right'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim() + "</td>");
            Content.Write("<td width='5%' align='right'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim() + "</td>");
            Content.Write("<td width='5%' align='right'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim() + "</td>");
            Content.Write("<td width='5%' align='right'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 13).Trim() + "</td>");
            Content.Write("<td width='5%' align='right'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 14).Trim() + "</td>");
            Content.Write("<td width='10%' align='right'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim() + "</td>");
            Content.WriteLine("</tr>\\");
            Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");

            int Max_Players = (int)ds.Tables[0].Rows.Count;
            int LineNumber = 0;
            foreach (DataRowView drv in dv)
            {

                LineNumber = LineNumber + 1;
                Content.Write("<td width='2%' align='right'>" + string.Format("{0:N0}", LineNumber) + "</td>");
                Content.Write("<td width='8%' align='right'>" + (string)drv["Foto"] + "</td>");
                Content.Write("<td width='30%' align='left'>" + (string)drv["Speler"] + "</td>");
                if ((object)drv["R1"] != DBNull.Value)
                {
                    Content.Write("<td width='5%' align='right'>" + (string)drv["R1"] + "</td>");
                }
                else
                {
                    Content.Write("<td width='5%' align='right'>-</td>");
                }
                if ((object)drv["R2"] != DBNull.Value)
                {
                    Content.Write("<td width='5%' align='right'>" + (string)drv["R2"] + "</td>");
                }
                else
                {
                    Content.Write("<td width='5%' align='right'>-</td>");
                }
                if ((object)drv["R3"] != DBNull.Value)
                {
                    Content.Write("<td width='5%' align='right'>" + (string)drv["R3"] + "</td>");
                }
                else
                {
                    Content.Write("<td width='5%' align='right'>-</td>");
                }
                if ((object)drv["R4"] != DBNull.Value)
                {
                    Content.Write("<td width='5%' align='right'>" + (string)drv["R4"] + "</td>");
                }
                else
                {
                    Content.Write("<td width='5%' align='right'>-</td>");
                }
                if ((object)drv["R5"] != DBNull.Value)
                {
                    Content.Write("<td width='5%' align='right'>" + (string)drv["R5"] + "</td>");
                }
                else
                {
                    Content.Write("<td width='5%' align='right'>-</td>");
                }
                if ((object)drv["R6"] != DBNull.Value)
                {
                    Content.Write("<td width='5%' align='right'>" + (string)drv["R6"] + "</td>");
                }
                else
                {
                    Content.Write("<td width='5%' align='right'>-</td>");
                }
                Content.Write("<td width='10%' align='right'>" + string.Format("{0:0.0}",(float)drv["PuntenClean"]) + "</td>");
                Content.WriteLine("</tr>\\");
            }

            Content.WriteLine("</table>\\");
            Content.WriteLine("</div>\\");

            Content.WriteLine("<br />\\");
            Content.WriteLine("<br />\\");
            Content.WriteLine("<input id=\"buttonprint\" type=\"button\" value=\"" + Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 11).Trim() + "\" onclick=\"PrintElem();\">\\");

            Content.WriteLine("\");");
            Container.WriteLine("<script src=\"" + DisplayBody + "\"></script>");
            Container.WriteLine("</body>");
            Container.WriteLine("</html>");
            Container.Close();
            Content.Close();
            Client_MLC.Close();
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

            Client_WCF.Close();
            Label1.Visible = true;

            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

    }
}