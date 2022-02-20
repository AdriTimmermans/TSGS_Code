using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Display_Games : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "DisplayGames";
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
                Client_WCF.WriteLogLine("C#", (int)Session["Competition_Identification"], "TSGS_CS_Display_Games", "Info", 4, "Started");
                Client_WCF.Close(); 
                Fill_Texts();
                if ((bool)Session["AutoOverview"])
                {
                    Button1_Click_Actions(null, null);
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
            Label1.Visible = false;
            Label3.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 23).Trim(), (int)Session["Round_Number"]);
            Label4.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Label4.Visible = false;
            Label4.Font.Size = (int)Session["FontSize"] + 2;
            Label5.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            Label5.Font.Size = (int)Session["FontSize"] + 2;
            Label6.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            Label6.Font.Size = (int)Session["FontSize"] + 2;
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            Button3.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            GridView1.Font.Name = "Arial";
            GridView1.Font.Size= (int) Session["Fontsize"];
            GridView2.Font.Name = "Arial";
            GridView2.Font.Size= (int) Session["Fontsize"];
            GridView3.Font.Name = "Arial";
            GridView3.Font.Size= (int) Session["Fontsize"];
            GridView4.Font.Name = "Arial";
            GridView4.Font.Size= (int) Session["Fontsize"];

            DataSet ds5 = Client_WCF.GetGameList((int)Session["Competition_Identification"], (int)Session["Round_Number"], 5, (int)Session["Language"]);
            if (ds5.Tables[0].Rows.Count == 0)
            {
                GridView1.Visible = false;
            }
            else
            {
                GridView1.DataSource = ds5;
                GridView1.AllowPaging = true;
                GridView1.PageSize = 8;
                GridView1.DataBind();

                GridViewRow row1Header = GridView1.HeaderRow;
                row1Header.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
                row1Header.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                row1Header.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
                row1Header.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                row1Header.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim();
                row1Header.Cells[2].HorizontalAlign = HorizontalAlign.Center;

            }

            DataSet ds3 = Client_WCF.GetGameList((int)Session["Competition_Identification"], (int)Session["Round_Number"], 3, (int)Session["Language"]);
            if (ds3.Tables[0].Rows.Count == 0)
            {
                GridView2.Visible = false;
                Label4.Visible = false;
            }
            else
            {
                Label4.Visible = true;
                GridView2.DataSource = ds3;
                GridView2.AllowPaging = false;
                GridView2.DataBind();

                GridViewRow row2Header = GridView2.HeaderRow;
                row2Header.Cells[0].Text = Label4.Text;
                row2Header.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                row2Header.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
                row2Header.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            }

            DataSet ds8 = Client_WCF.GetGameList((int)Session["Competition_Identification"], (int)Session["Round_Number"], 8, (int)Session["Language"]);
            if (ds8.Tables[0].Rows.Count == 0)
            {
                GridView3.Visible = false;
                Label5.Visible = false;
            }
            else
            {
                Label5.Visible = true;
                GridView3.DataSource = ds8;
                GridView3.AllowPaging = false;
                GridView3.DataBind();

                GridViewRow row3Header = GridView3.HeaderRow;
                row3Header.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
                row3Header.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                row3Header.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
                row3Header.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                row3Header.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
                row3Header.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                row3Header.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim();
                row3Header.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                row3Header.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 13).Trim();
                row3Header.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                row3Header.Cells[5].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 14).Trim();
                row3Header.Cells[5].HorizontalAlign = HorizontalAlign.Left;
                row3Header.Cells[6].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
                row3Header.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                row3Header.Cells[7].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
                row3Header.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                row3Header.Cells[8].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim();
                row3Header.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                row3Header.Cells[9].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim();
                row3Header.Cells[9].HorizontalAlign = HorizontalAlign.Center;
            }

            DataSet ds4 = Client_WCF.GetGameList((int)Session["Competition_Identification"], (int)Session["Round_Number"], 4, (int)Session["Language"]);
            if (ds4.Tables[0].Rows.Count == 0)
            {
                Label6.Visible = false;
                GridView4.Visible = false;
            }
            else
            {
                Label6.Visible = true;
                GridView4.DataSource = ds4;
                GridView4.AllowPaging = true;
                GridView4.PageSize = 10;
                GridView4.DataBind();

                GridViewRow row4Header = GridView4.HeaderRow;
                row4Header.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
                row4Header.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                row4Header.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
                row4Header.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                row4Header.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
                row4Header.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                row4Header.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim();
                row4Header.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                row4Header.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 13).Trim();
                row4Header.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                row4Header.Cells[5].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 14).Trim();
                row4Header.Cells[5].HorizontalAlign = HorizontalAlign.Left;
                row4Header.Cells[6].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
                row4Header.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                row4Header.Cells[7].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
                row4Header.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                row4Header.Cells[8].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim();
                row4Header.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                row4Header.Cells[9].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim();
                row4Header.Cells[9].HorizontalAlign = HorizontalAlign.Center;
            }

            Client_MLC.Close();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                e.Row.Cells[0].Font.Size= (int) Session["Fontsize"];
                e.Row.Cells[1].Font.Size= (int) Session["Fontsize"];
                e.Row.Cells[2].Font.Size= (int) Session["Fontsize"];
            }
        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                e.Row.Cells[0].Font.Size= (int) Session["Fontsize"];
                e.Row.Cells[1].Font.Size= (int) Session["Fontsize"];
            }
        }
        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                e.Row.Cells[0].Font.Size= (int) Session["Fontsize"];
                e.Row.Cells[1].Font.Size= (int) Session["Fontsize"];
                e.Row.Cells[2].Font.Size= (int) Session["Fontsize"];
                e.Row.Cells[3].Font.Size= (int) Session["Fontsize"];
                e.Row.Cells[4].Font.Size= (int) Session["Fontsize"];
                e.Row.Cells[5].Font.Size= (int) Session["Fontsize"];
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[6].Font.Size= (int) Session["Fontsize"];
                e.Row.Cells[7].Font.Size= (int) Session["Fontsize"];
                e.Row.Cells[8].Font.Size= (int) Session["Fontsize"];
                e.Row.Cells[9].Font.Size= (int) Session["Fontsize"];
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Updatepanel1.Visible = true;
            Button1_Click_Actions(sender, e);
            Updatepanel1.Visible = false;
        }       
        protected void Button1_Click_Actions(object sender, EventArgs e)
        {
            int LineCount = 0;

            string RootPath =  Server.MapPath("~/htmlfiles/"+Convert.ToString((int)Session["Competition_Identification"]).Trim());
            string GamesDisplayHeader = "TSGS_CS_Pairing.html";
            string GamesDisplayBody = "Pairing_body.js";
            string GamesAppDisplay = "TSGS_CS_Pairing_App.html";

            StreamWriter Container = new StreamWriter(RootPath + @"\" + GamesDisplayHeader);
            StreamWriter Content = new StreamWriter(RootPath + @"\" + GamesDisplayBody);
            StreamWriter AppContent = new StreamWriter(RootPath + @"\" + GamesAppDisplay);

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            string doc_type = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";

            byte[] file = File.ReadAllBytes(Server.MapPath("~/images/target.jpg"));
            string TargetString = "<img src='" + Client_WCF.MakeImageSourceData((byte[])file) + "' width='20'>";

            Container.WriteLine(doc_type);
            Container.WriteLine("<html xmlns=\"http://www.w3.org/1999/shtml\">");
            Container.WriteLine("<head>");
            Container.WriteLine("<title>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim() + "</title>");
            Container.WriteLine("<meta http-equiv='content-type' CONTENT='text/html; charset=ISO-8859-1'>");
            Container.WriteLine("<meta http-equiv=\"expires\" content=\"0\">");
            string path_Template = Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_Template").Trim();
            Container.WriteLine("<link href='" + path_Template + "/modern.css' type=\"text/css\" rel=\"stylesheet\">");
            Container.WriteLine("</head>");
            Container.WriteLine("<base target='main'>");
            Container.WriteLine("<body>");

            Container.WriteLine("<hr size=4 Width='75%'>");

            AppContent.WriteLine(doc_type);
            AppContent.WriteLine("<html xmlns=\"http://www.w3.org/1999/shtml\">");
            AppContent.WriteLine("<head>");
            AppContent.WriteLine("<title>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim() + "</title>");
            AppContent.WriteLine("<meta http-equiv='content-type' CONTENT='text/html; charset=ISO-8859-1'>");
            AppContent.WriteLine("<meta http-equiv=\"expires\" content=\"0\">");
            AppContent.WriteLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, user-scalable=yes\">");
            AppContent.WriteLine("<link href='" + path_Template + "/modern.css' type=\"text/css\" rel=\"stylesheet\">");
            AppContent.WriteLine("<link media=\"all\" href='" + path_Template + "/base.min.css' type=\"text/css\" rel=\"stylesheet\">");
            AppContent.WriteLine("<link media=\"only screen and (max-width: 500px)\" href='" + path_Template + "/mobile.min.css' type=\"text/css\" rel=\"stylesheet\">");
            AppContent.WriteLine("</head>");
            AppContent.WriteLine("<base target='main'>");
            AppContent.WriteLine("<body>");

            Content.WriteLine("document.write(\"\\");
            Content.WriteLine("<div id='printable' class='PrintSettings'>\\");

            Content.WriteLine("<h1 align=left>" + (string)Session["Club"] + ", "+ (string)Session["Competition"] + "</h1>\\");
            Content.WriteLine("<h2 align=left>" + string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 23).Trim(), (int)Session["Round_Number"]) + "</h2>\\");
            Content.WriteLine("<br>\\");
            AppContent.WriteLine("<h2 align=left>" + (string)Session["Club"] + "</h2>");
            AppContent.WriteLine("<h2 align=left>" + string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 23).Trim(), (int)Session["Round_Number"]) + "</h2>");
            AppContent.WriteLine("<br>");

            DataSet ds5 = Client_WCF.GetGameList((int)Session["Competition_Identification"], (int)Session["Round_Number"], 5, (int)Session["Language"]);

            if (ds5.Tables[0].Rows.Count != 0)
            {
                Content.WriteLine("<h2>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim() + "</h2>\\");
                Content.WriteLine("<br>\\");
                Content.WriteLine("<table border='0\'>\\");
                AppContent.WriteLine("<table border='0\'>");
                GridViewRow row1Header = GridView1.HeaderRow;
                Content.Write("<tr>");
                Content.Write("<td width='2%' align='left'>&nbsp;</td>");
                Content.Write("<td width='35%' align='left'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 28).Trim() + "</td>");
                Content.Write("<td width='3%' align='center'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim() + "</td>");
                Content.Write("<td width='8%' align='center'>" + row1Header.Cells[2].Text + "</td>");
                Content.WriteLine("</tr>\\");
                Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");
                foreach (DataRow dgItem in ds5.Tables[0].Rows)
                {
                    LineCount++;
                    Content.Write("<tr>");
                    Content.Write("<td width='2%' align='left'>" + string.Format("{0:n0}", LineCount) + " </td>");
                    AppContent.WriteLine("<tr><td class=\"AppSize\" style=\"background-color:#D1DDF1\">" +
                        Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 24).Trim() +
                        string.Format("{0:n0}", LineCount) +
                        " </td></tr>");
                    Content.Write("<td width='35%' align='left'>" + dgItem["SpelernaamWit"].ToString() + "</td>");
                    AppContent.WriteLine("<tr><td class=\"AppSize\" style=\"background-color:#FFFFFF\">" + dgItem["SpelernaamWit"].ToString() + " </td></tr>");
                    Content.Write("<td width='3%' align='center'>" + dgItem["Wit_Winst"].ToString() + "</td>");
                    Content.Write("<td width='8%' align='center'>" + dgItem["Afkorting"].ToString() + "</td>");
                    Content.WriteLine("</tr>\\");
                    AppContent.WriteLine("<tr><td class=\"AppSize\" style=\"background-color:#000000\"><font color=\"White\">" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 25).Trim() + "</font></td></tr>");
                    AppContent.WriteLine("<tr><td>&nbsp;</td></tr>");
                    Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");
                }
                Content.WriteLine("</table>\\");
                Content.WriteLine("<br />\\");
                Content.WriteLine("<br />\\");
            }
            DataSet ds3 = Client_WCF.GetGameList((int)Session["Competition_Identification"], (int)Session["Round_Number"], 3, (int)Session["Language"]);

            if (ds3.Tables[0].Rows.Count != 0)
            {
                GridViewRow row2Header = GridView2.HeaderRow;
                Content.WriteLine("<table border='0\'>\\");
                Content.Write("<tr>");
                Content.Write("<td width='2%' align='left'>&nbsp;</td>");
                Content.Write("<td width='35%' align='left'><h2>" + row2Header.Cells[0].Text + "</h2></td>");
                Content.Write("<td width='8%' align='center'>" + row2Header.Cells[1].Text + "</td>");
                Content.WriteLine("</tr>\\");
                Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");
                foreach (DataRow dgItem in ds3.Tables[0].Rows)
                {
                    LineCount++;
                    Content.Write("<tr>");
                    Content.Write("<td width='2%' align='left'>" + string.Format("{0:n0}", LineCount) + " </td>");
                    AppContent.WriteLine("<tr><td class=\"AppSize\" style=\"background-color:#D1DDF1\">" +
                        Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 24).Trim() +
                        string.Format("{0:n0}", LineCount) + " -" +
                        " </td></tr>");
                    Content.Write("<td width='35%' align='left'>" + dgItem["SpelernaamWit"].ToString() + "</td>");
                    AppContent.WriteLine("<tr><td class=\"AppSize\" style=\"background-color:#FFFFFF\">" + dgItem["SpelernaamWit"].ToString() + " </td></tr>");
                    AppContent.WriteLine("<tr><td class=\"AppSize\" style=\"background-color:#000000\"><font color=\"White\">" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 26).Trim() + "</font></td></tr>");
                    AppContent.WriteLine("<tr><td>&nbsp;</td></tr>");
                    Content.Write("<td width='8%' align='center'>" + dgItem["Wit_Winst"].ToString() + "</td>");
                    Content.WriteLine("</tr>\\");
                    Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");
                }
                Content.WriteLine("</table>\\");
                Content.WriteLine("<br />\\");
                Content.WriteLine("<br />\\");

            }
            DataSet ds8 = Client_WCF.GetGameList((int)Session["Competition_Identification"], (int)Session["Round_Number"], 8, (int)Session["Language"]);
            if (ds8.Tables[0].Rows.Count != 0)
            {
                Content.WriteLine("<h2>" + Label5.Text + "</h2>\\");
                Content.WriteLine("<br />\\");
                Content.WriteLine("<table border='0\'>\\");
                GridViewRow row3Header = GridView3.HeaderRow;
                Content.Write("<tr>");
                Content.Write("<td width='2%' align='left'>&nbsp;</td>");
                Content.Write("<td width='35%' align='left'>" + row3Header.Cells[0].Text + "</td>");
                Content.Write("<td width='3%' align='center'>" + row3Header.Cells[1].Text + "</td>");
                Content.Write("<td width='3%' align='center'>" + row3Header.Cells[2].Text + "</td>");
                Content.Write("<td width='3%' align='center'>" + row3Header.Cells[3].Text + "</td>");
                Content.Write("<td width='2%' align='center'>" + row3Header.Cells[4].Text + "</td>");
                Content.Write("<td width='35%' align='left'>" + row3Header.Cells[5].Text + "</td>");
                Content.Write("<td width='3%' align='center'>" + row3Header.Cells[6].Text + "</td>");
                Content.Write("<td width='3%' align='center'>" + row3Header.Cells[7].Text + "</td>");
                Content.Write("<td width='3%' align='center'>" + row3Header.Cells[8].Text + "</td>");
                Content.Write("<td width='8%' align='center'>" + row3Header.Cells[9].Text + "</td>");
                Content.WriteLine("</tr>\\");
                Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");

                foreach (DataRow dgItem in ds8.Tables[0].Rows)
                {
                    LineCount++;
                    Content.Write("<tr>");
                    Content.Write("<td width='2%' align='left'>" + string.Format("{0:n0}", LineCount) + " </td>");
                    AppContent.WriteLine("<tr><td class=\"AppSize\" style=\"background-color:#D1DDF1\">" +
                        Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 24).Trim() +
                        string.Format("{0:n0}", LineCount) + " -" +
                        Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 27).Trim() +
                        " </td></tr>");
                    Content.Write("<td width='35%' align='left'>" + "<a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=" + string.Format("{0000}", dgItem["PID_Wit"]) + "&Player2=" + string.Format("{0000}", dgItem["PID_Zwart"]) + "'>" + TargetString + "</a>" + dgItem["SpelernaamWit"].ToString() + "</td>");
                    AppContent.WriteLine("<tr><td class=\"AppSize\" style=\"background-color:#FFFFFF\">" + dgItem["SpelernaamWit"].ToString() + " </td></tr>");
                    Content.Write("<td width='3%' align='center'>" + String.Format("{0:n1}", dgItem["Wit_Winst"]) + "</td>");
                    Content.Write("<td width='3%' align='center'>" + String.Format("{0:n1}", dgItem["Wit_Remise"]) + "</td>");
                    Content.Write("<td width='3%' align='center'>" + String.Format("{0:n1}", dgItem["Wit_Verlies"]) + "</td>");
                    Content.Write("<td width='3%' align='center'> - </td>");
                    Content.Write("<td width='35%' align='left'>" + dgItem["SpelernaamZwart"].ToString() + "<a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=" + string.Format("{0000}", dgItem["PID_Zwart"]) + "&Player2=" + string.Format("{0000}", dgItem["PID_Wit"]) + "'>" + TargetString + "</a>" + "</td>");
                    AppContent.WriteLine("<tr><td class=\"AppSize\" style=\"background-color:#000000\"><font color=\"White\">" + dgItem["SpelernaamZwart"].ToString() + "</font></td></tr>");
                    AppContent.WriteLine("<tr><td>&nbsp;</td></tr>");
                    Content.Write("<td width='3%' align='center'>" + String.Format("{0:n1}", dgItem["Zwart_Winst"]) + "</td>");
                    Content.Write("<td width='3%' align='center'>" + String.Format("{0:n1}", dgItem["Zwart_Remise"]) + "</td>");
                    Content.Write("<td width='3%' align='center'>" + String.Format("{0:n1}", dgItem["Zwart_Verlies"]) + "</td>");
                    Content.Write("<td width='8%' align='center'>" + dgItem["Afkorting"].ToString() + "</td>");
                    Content.WriteLine("</tr>\\");
                    Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");
                }
                Content.WriteLine("</table>\\");
                Content.WriteLine("<br />\\");
                Content.WriteLine("<br />\\");
            }
            DataSet ds4 = Client_WCF.GetGameList((int)Session["Competition_Identification"], (int)Session["Round_Number"], 4, (int)Session["Language"]);
            if (ds4.Tables[0].Rows.Count != 0)
            {
                GridViewRow row4Header = GridView4.HeaderRow;
                Content.WriteLine("<h2>" + Label6.Text + "</h2>\\");
                Content.WriteLine("<table border='0\'>\\");
                Content.WriteLine("<br />\\");
                Content.Write("<tr>");
                Content.Write("<td width='2%' align='left'>&nbsp;</td>");
                Content.Write("<td width='35%' align='left'>" + row4Header.Cells[0].Text + "</td>");
                Content.Write("<td width='3%' align='center'>" + row4Header.Cells[1].Text + "</td>");
                Content.Write("<td width='3%' align='center'>" + row4Header.Cells[2].Text + "</td>");
                Content.Write("<td width='3%' align='center'>" + row4Header.Cells[3].Text + "</td>");
                Content.Write("<td width='3%' align='center'>" + row4Header.Cells[4].Text + "</td>");
                Content.Write("<td width='35%' align='left'>" + row4Header.Cells[5].Text + "</td>");
                Content.Write("<td width='3%' align='center'>" + row4Header.Cells[6].Text + "</td>");
                Content.Write("<td width='3%' align='center'>" + row4Header.Cells[7].Text + "</td>");
                Content.Write("<td width='3%' align='center'>" + row4Header.Cells[8].Text + "</td>");
                Content.Write("<td width='8%' align='center'>" + row4Header.Cells[9].Text + "</td>");
                Content.WriteLine("</tr>\\");
                Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");

                foreach (DataRow dgItem in ds4.Tables[0].Rows)
                {
                    LineCount++;
                    Content.Write("<tr>");
                    Content.Write("<td width='2%' align='left'>" + string.Format("{0:n0}", LineCount) + "</td>");
                    AppContent.WriteLine("<tr><td class=\"AppSize\" style=\"background-color:#D1DDF1\">" +
                        Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 24).Trim() +
                        string.Format("{0:n0}", LineCount) +
                        " </td></tr>");
                    Content.Write("<td width='35%' align='left'>" + "<a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=" + string.Format("{0000}", dgItem["PID_Wit"]) + "&Player2=" + string.Format("{0000}", dgItem["PID_Zwart"]) + "'>" + TargetString + "</a>" + dgItem["SpelernaamWit"].ToString() + "</td>");
                    AppContent.WriteLine("<tr><td class=\"AppSize\" style=\"background-color:#FFFFFF\">" + dgItem["SpelernaamWit"].ToString() + " </td></tr>");
                    Content.Write("<td width='3%' align='center'>" + String.Format("{0:n1}", dgItem["Wit_Winst"]) + "</td>");
                    Content.Write("<td width='3%' align='center'>" + String.Format("{0:n1}", dgItem["Wit_Remise"]) + "</td>");
                    Content.Write("<td width='3%' align='center'>" + String.Format("{0:n1}", dgItem["Wit_Verlies"]) + "</td>");
                    Content.Write("<td width='2%' align='center'> - </td>");
                    Content.Write("<td width='35%' align='left'>" + dgItem["SpelernaamZwart"].ToString() + "<a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=" + string.Format("{0000}", dgItem["PID_Zwart"]) + "&Player2=" + string.Format("{0000}", dgItem["PID_Wit"]) + "'>" + TargetString + "</a>" + "</td>");
                    AppContent.WriteLine("<tr><td class=\"AppSize\" style=\"background-color:#000000\"><font color=\"White\">" + dgItem["SpelernaamZwart"].ToString() + "</font></td></tr>");
                    AppContent.WriteLine("<tr><td>&nbsp;</td></tr>");
                    Content.Write("<td width='3%' align='center'>" + String.Format("{0:n1}", dgItem["Zwart_Winst"]) + "</td>");
                    Content.Write("<td width='3%' align='center'>" + String.Format("{0:n1}", dgItem["Zwart_Remise"]) + "</td>");
                    Content.Write("<td width='3%' align='center'>" + String.Format("{0:n1}", dgItem["Zwart_Verlies"]) + "</td>");
                    Content.Write("<td width='8%' align='center'>" + dgItem["Afkorting"].ToString() + "</td>");
                    Content.WriteLine("</tr>\\");
                    Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");
                }
                Content.WriteLine("</table>\\");
                AppContent.WriteLine("</table>\\");
                Content.WriteLine("</div>\\");
                Content.WriteLine("<br />\\");
                Content.WriteLine("<br />\\");
            }

            Content.WriteLine("<input id=\"buttonprint\" type=\"button\" value=\"" + Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 11).Trim() + "\" onclick=\"PrintElem();\">\\");
            Content.WriteLine("\");");

            Container.WriteLine("    <script src=\"Pairing_body.js\"></script>");
            Container.WriteLine("</body>");
            Container.WriteLine("</html>");
            AppContent.WriteLine("</body>");
            AppContent.WriteLine("</html>");
            Container.Close();
            Content.Close();
            AppContent.Close();
            //
            // Upload files to website
            //
            string Display_Competition = Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_basis").Trim() + "/" +
                                         Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_Competitie").Trim();
            string error_text = "";
            Label1.Text = "";
            error_text = Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], GamesDisplayHeader);
            error_text += Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], GamesDisplayBody);
            error_text += Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], GamesAppDisplay);
            if (error_text.Trim() == "")
            {
                Client_WCF.Update_Workflow_Item("[Afdrukken Indeling]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
                Client_WCF.Update_Workflow_Item("[Publiceren]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
                if (ds3.Tables[0].Rows.Count != 0)
                {
                    //
                    // vrijgeloot bij indeling set the datum_vrijgeloot
                    //
                    foreach (DataRow dgItem in ds3.Tables[0].Rows)
                    {
                        Client_WCF.Update_Free_Round_Date(Convert.ToInt16(dgItem["PID_Wit"]));
                    }
                }

                Client_MLC.Close();
                Client_WCF.Close();
                if ((bool)Session["AutoOverview"])
                {
                    Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
                }
                else
                {
                    Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
                }
            }
            else
            {
                Label1.Text = error_text;
                Label1.Visible = true;
            }
        }
   		private void Send_Email (int PID, int CID, int RNR, string EmailAddress)
		{
			string lit_Error = "";
            string Subject = "";
            string aux = "";
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            DataSet ds = Client_WCF.GetOneGame(PID, CID, RNR);
			try 
			{
                SmtpClient client = new SmtpClient("mail.planet.nl");
//                SmtpClient client = new SmtpClient("smtp.mijnhostingpartner.nl");
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("Adri", "1403");
                MailAddress from = new MailAddress((String)Session["ManagerEmailAddress"]);
                MailAddress to = new MailAddress(EmailAddress);
                MailMessage message = new MailMessage(from, to);

                StringBuilder mailBody = new StringBuilder();
                
                aux = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 16).Trim(), Client_WCF.GetPlayerFirstName(PID));
                mailBody.AppendFormat("{0}", aux).AppendLine();
                aux = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 17).Trim();
                mailBody.AppendFormat("{0}", aux).AppendLine();
                try
				{
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int PID_1 = (int)dr[0];
                        int PID_2 = (int)dr[1];
                        aux = Client_WCF.GetPlayerName(PID_1) + " - " + Client_WCF.GetPlayerName(PID_2)+".";
                        mailBody.AppendFormat("{0}", aux).AppendLine();
                        aux = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 18).Trim();
                        mailBody.AppendFormat("{0}", aux).AppendLine();

                        if (Client_WCF.IsPlayerInChampiongroup(PID))
                        {
                            aux = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 29).Trim();
                            mailBody.AppendFormat("{0}", aux).AppendLine();
                            for (int i = 1; i <= 7; i++)
                            {
                                aux = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 29+i).Trim();
                                mailBody.AppendFormat("{0}", aux).AppendLine();
                            }
                        }
                        aux = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 19).Trim();
                        mailBody.AppendFormat("{0}", aux).AppendLine();
                        aux = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 20).Trim(), Session["Manager"]);
                        mailBody.AppendFormat("{0}", aux).AppendLine();
                        aux = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 21).Trim();
                        mailBody.AppendFormat("{0}", aux).AppendLine();
                    }
                    message.IsBodyHtml = false;
                    message.Body = mailBody.ToString();
                    Subject = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 1).Trim(), RNR);
                    message.Subject = Subject;
                
					// ADD AN ATTACHMENT.
					// TODO: Replace with path to attachment.
					// String sFile = @"C:\temp\Hello.txt";  
					// MailAttachment oAttch = new MailAttachment(sFile, MailEncoding.Base64);
  
					// oMsg.Attachments.Add(oAttch);

					client.Send(message);

					message.Dispose();
					// oAttch = null;
				}
				catch (Exception ex)
				{
					lit_Error = ex.Message;	
					//	error_occurred = true;
				}
				finally
				{
                    Client_WCF.Close();
                    Client_MLC.Close();
                }
			}
			catch (Exception ex)
			{
				lit_Error = ex.Message;	
			}
		}


        protected void Button2_Click(object sender, EventArgs e)
        {
            string lit_Error;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            DataSet ds = Client_WCF.GetMailAddresses((int)Session["Competition_Identification"]);
			try
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int PID = (int)dr[0];
					string Email_Address = (string)dr[1];
					Send_Email(PID, (int)Session["Competition_Identification"], (int)Session["Round_Number"], Email_Address.Trim());
				}
			}
			catch (Exception ex)
			{
				lit_Error = ex.Message;	
				//	error_occurred = true;
			}
			finally
			{
				Client_WCF.Close();
			}
        }


        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Fill_Texts();
        }
        protected void GridView4_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView4.PageIndex = e.NewPageIndex;
            Fill_Texts();
        }
    }

}