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

    public partial class TSGS_CS_Display_Participant_List : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "DisplayPlayers";
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

            Label1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Label3.Text = (String)Session["Club"] + ", " + (string)Session["Competition"];
            Label4.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 16).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();


            /*
            Other text filling actions
            */

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataSet ds = Client_WCF.GetPlayerListAlphabetical((int)Session["Competition_Identification"]);
            ViewState["Players"] = ds.Tables[0];
            Client_WCF.Close();

            GridView1.Font.Name = "Arial";
            GridView1.Font.Size = (int)Session["Fontsize"];
            GridView1.DataSource = ds;
            GridView1.AllowPaging = true;
            GridView1.PageSize = 10;
            GridView1.DataBind();
            GridViewRow row1Header = GridView1.HeaderRow;

            row1Header.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            row1Header.Cells[0].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            row1Header.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            row1Header.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim();
            row1Header.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            row1Header.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
            row1Header.Cells[4].HorizontalAlign = HorizontalAlign.Left;
            row1Header.Cells[5].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 13).Trim();
            row1Header.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[6].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 14).Trim();
            row1Header.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[7].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            row1Header.Cells[7].HorizontalAlign = HorizontalAlign.Left;
            row1Header.Cells[8].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim();
            row1Header.Cells[8].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[9].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
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

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                for (int i = 0; i < 10; i++)
                {
                    e.Row.Cells[i].Font.Size = (int)Session["Fontsize"];
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center; 
                }
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left; 

                Label lb = (Label)e.Row.FindControl("SpelerNaam");
                CheckBox cb_Kroongroep = (CheckBox)e.Row.FindControl("cbKroongroep");
                CheckBox cb_SpelendLid = (CheckBox)e.Row.FindControl("cbSpelend");
                TextBox tb_Team = (TextBox)e.Row.FindControl("tbTeam");
                cb_Kroongroep.Checked = Convert.ToBoolean(dr["Member_Premier_Group"]);
                cb_SpelendLid.Checked = !Convert.ToBoolean(dr["Deelnemer_teruggetrokken"]);
                tb_Team.Text = Convert.ToString(dr["Team"]);
                lb.Text = Convert.ToString(dr["Spelernaam"]);
                lb.Font.Size = (int)Session["Fontsize"];
                Label lbClubrating = (Label)e.Row.FindControl("Clubrating");
                lbClubrating.Text = string.Format("{0:#000.0}", Client_WCF.GetClubRating(Convert.ToInt16(dr["Speler_ID"])));
                
                Label lbKNSB_Id = (Label)e.Row.FindControl("KNSB_Id");
                Label lbKNSBrating = (Label)e.Row.FindControl("KNSB_Rating");
                if (dr.IsNull("KNSBnummer"))
                {
                    lbKNSB_Id.Text = "";
                    lbKNSBrating.Text = "";
                }
                else 
                {
                    lbKNSB_Id.Text = string.Format("{0:f0}", Convert.ToInt32(dr["KNSBnummer"]));
                    lbKNSBrating.Text = string.Format("{0:#000.0}", Client_WCF.GetKNSBRating(Convert.ToInt16(dr["Speler_ID"])));
                }

                Label lbFIDE_Id = (Label)e.Row.FindControl("FIDE_Id");
                Label lbFIDErating = (Label)e.Row.FindControl("FIDE_Rating");
                if (dr.IsNull("FIDEnummer"))
                {
                    lbFIDE_Id.Text = "";
                    lbFIDErating.Text = "";
                }
                else
                {
                    lbFIDE_Id.Text = string.Format("{0:f0}", Convert.ToInt32(dr["FIDEnummer"]));
                    lbFIDErating.Text = string.Format("{0:#000.0}", Client_WCF.GetFIDERating(Convert.ToInt16(dr["Speler_ID"])));
                }

                Image imgPicture = (Image)e.Row.FindControl("imgPicture");;
                imgPicture.ImageUrl = Client_WCF.StringImage(Convert.ToInt16(dr["Speler_ID"]), (int)Session["Competition_Identification"]);
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
/*
trigger actions for main function of form
*/
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
/*            Client_WCF.<xxxz>((int)Session["Competition_Identification"], (int)Session["Round_Number"]);*/
            Label1.Visible = true;


/*            Session["Current_Status"] = <z>;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], <z>);
            Client_WCF.Update_Workflow_Item("[<XXXY>]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
            Client_WCF.Close();*/
            Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 4, "Closed");
            Client_WCF.Close();
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Updatepanel1.Visible = true;
            Button1_Click_Actions(null, null);
            Updatepanel1.Visible = false;
        }

        protected void Button1_Click_Actions(object sender, EventArgs e)
        {
            Master.SetErrorMessageGreen();
            Master.ErrorMessageVisibility(false);
            Label1.Visible = false;
            string RootPath =  Server.MapPath("~/htmlfiles/"+Convert.ToString((int)Session["Competition_Identification"]).Trim());
            string DisplayHeader = "TSGS_CS_Display_Participant_List.html";
            string DisplayBody = "Display_Participant_List_body.js";

            StreamWriter Container = new StreamWriter(RootPath + @"\" + DisplayHeader);
            StreamWriter Content = new StreamWriter(RootPath + @"\" + DisplayBody);

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

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
            Content.WriteLine("<h1 align=left>" + Label3.Text + "</h1>\\");
            Content.WriteLine("<h2 align=left>" + Label4.Text + "</h2>\\");
            Content.WriteLine("<br>\\");
            Content.WriteLine("<table border='1\'>\\");
            DataTable dt = (DataTable)ViewState["Players"];
            GridViewRow row1Header = GridView1.HeaderRow;
            Content.Write("<tr>");
            Content.Write("<td width='2%' align='right'>&nbsp;</td>");
            Content.Write("<td width='5%' align='center'>&nbsp;</td>");
            Content.Write("<td width='40%' align='left'>" + row1Header.Cells[1].Text + "</td>");
            Content.Write("<td width='2%' align='right'>" + row1Header.Cells[2].Text + "</td>");
            Content.Write("<td width='4%' align='right'>" + row1Header.Cells[3].Text + "</td>");
            Content.Write("<td width='4%' align='right'>" + row1Header.Cells[4].Text + "</td>");
            Content.Write("<td width='4%' align='right'>" + row1Header.Cells[5].Text + "</td>");
            Content.Write("<td width='8%' align='right'>" + row1Header.Cells[6].Text + "</td>");
            Content.Write("<td width='4%' align='right'>" + row1Header.Cells[7].Text + "</td>");
            Content.Write("<td width='8%' align='right'>" + row1Header.Cells[8].Text + "</td>");
            Content.Write("<td width='4%' align='right'>" + row1Header.Cells[9].Text + "</td>");
            Content.Write("<td width='15%' align='right'>&nbsp;</td>");
            Content.WriteLine("</tr>\\");
            Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");
            int LineCount = 0;
            foreach (DataRow dgItem in dt.Rows)
            {
                LineCount++;
                Content.Write("<tr>");

                Content.Write("<td width='2%' align='right'>" + LineCount.ToString() + "</td>");

                int PID = Convert.ToInt16(dgItem["Speler_Id"]);
                Content.Write("<td width='5%' align='center'><img src='" + Client_WCF.StringImage(PID, (int)Session["Competition_Identification"]) + "'></td>");
                Content.Write("<td width='40%' align='left'>" + dgItem["Spelernaam"] + "</td>");
                Content.Write("<td width='2%' align='right'>" + string.Format("{0:N0}", dgItem["Team"]) + "</td>");
                Content.Write("<td width='4%' align='right'>" + Convert.ToBoolean(dgItem["Member_Premier_Group"]).ToString() + "</td>");
                Content.Write("<td width='4%' align='right'>" + (!Convert.ToBoolean(dgItem["Deelnemer_teruggetrokken"])).ToString() + "</td>");
                Content.Write("<td width='4%' align='right'>" + string.Format("{0:f0}", Client_WCF.GetClubRating(Convert.ToInt16(dgItem["Speler_ID"]))) + "</td>");
                string auxnr;
                string auxrating;
                if (dgItem.IsNull("KNSBnummer"))
                {
                    auxnr = "";
                    auxrating = "";
                }
                else
                {
                    auxnr = string.Format("{0:f0}", Convert.ToInt32(dgItem["KNSBnummer"]));
                    auxrating = string.Format("{0:#000.0}", Client_WCF.GetKNSBRating(Convert.ToInt16(dgItem["Speler_ID"])));
                }
                Content.Write("<td width='8%' align='right'>" + auxnr + "</td>");
                Content.Write("<td width='4%' align='right'>" + auxrating + "</td>");

                if (dgItem.IsNull("FIDEnummer"))
                {
                    auxnr = "";
                    auxrating = "";
                }
                else
                {
                    auxnr = string.Format("{0:f0}", Convert.ToInt32(dgItem["FIDEnummer"]));
                    auxrating = string.Format("{0:#000.0}", Client_WCF.GetFIDERating(Convert.ToInt16(dgItem["Speler_ID"])));
                }


                Content.Write("<td width='8%' align='right'>" + auxnr + "</td>");
                Content.Write("<td width='4%' align='right'>" + auxrating + "</td>");
                Content.Write("<td width='15%' align='right'>&nbsp;</td>");
                Content.WriteLine("</tr>\\");
            }

            Content.WriteLine("</table>\\");
            Content.WriteLine("</div>\\");
            Content.WriteLine("<br />\\");
            Content.WriteLine("<br />\\");
            Content.WriteLine("<input id=\"buttonprint\" type=\"button\" value=\"" + Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 11).Trim() + "\" onclick=\"PrintElem();\">\\");
            Content.WriteLine("\");");

            Container.WriteLine("    <script src=\"" + DisplayBody + "\"></script>");
            Container.WriteLine("</body>");
            Container.WriteLine("</html>");
            Container.Close();
            Content.Close();
            //
            // Upload files to website
            //
            Label1.Text = "";
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

            if ((bool)Session["AutoOverview"])
            {
                Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 4, "Closed");
                Response.Redirect("TSGS_CS_Stats.aspx");
            }
            else
            {
                Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 4, "Closed");
                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }
            Client_WCF.Close();
            Client_MLC.Close();
        }
    }
}