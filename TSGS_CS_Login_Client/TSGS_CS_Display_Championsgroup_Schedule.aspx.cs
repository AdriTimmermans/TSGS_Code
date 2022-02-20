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

    public partial class TSGS_CS_Display_Championsgroup_Schedule : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "DisplayCGSchedule";
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
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataSet ds = Client_WCF.GetChampionsgroupSchedule((int)Session["Competition_Identification"]);
            ViewState["Games"] = ds.Tables[0];
            Client_WCF.Close();

            GridView1.Font.Name = "Arial";
            GridView1.Font.Size = (int)Session["Fontsize"];
            GridView1.DataSource = ds;
            GridView1.AllowPaging = true;
            GridView1.PageSize = 15;
            GridView1.DataBind();
            GridViewRow row1Header = GridView1.HeaderRow;

            row1Header.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            row1Header.Cells[0].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            row1Header.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            row1Header.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            row1Header.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            row1Header.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
            row1Header.Cells[4].HorizontalAlign = HorizontalAlign.Left;

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
                string Aux = Convert.ToString(dr["Uitslag"]);
                for (int i = 0; i < 5; i++)
                {
                    e.Row.Cells[i].Font.Size = (int)Session["Fontsize"];
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                }
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                if (Convert.ToSingle(Aux) == -1 )
                {
                    e.Row.BackColor = System.Drawing.Color.LightPink;
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.LightGreen;
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            /*
            trigger actions for main function of form
            */
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            /*            Client_WCF.<xxxz>((int)Session["Competition_Identification"], (int)Session["Round_Number"]);*/
            Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 4, "Closed");
            Client_WCF.Close(); Label1.Visible = true;


            /*            Session["Current_Status"] = <z>;
                        TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                        Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], <z>);
                        Client_WCF.Update_Workflow_Item("[<XXXY>]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
                        Client_WCF.Close();*/
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Button2_Click_Actions(null, null);
        }

        protected void Button2_Click_Actions(object sender, EventArgs e)
        {
            string RootPath =  Server.MapPath("~/htmlfiles/"+Convert.ToString((int)Session["Competition_Identification"]).Trim());
            string DisplayHeader = "TSGS_CS_Display_Championsgroup_Schedule.html";
            string DisplayBody = "Display_Championsgroup_Schedule_body.js";

            StreamWriter Container = new StreamWriter(RootPath + @"\" + DisplayHeader);
            StreamWriter Content = new StreamWriter(RootPath + @"\" + DisplayBody);

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            string doc_type = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";

            Container.WriteLine(doc_type);
            Container.WriteLine("<html xmlns=\"http://www.w3.org/1999/shtml\">");
            Container.WriteLine("<head>");
            Container.WriteLine("<title>" + Label3.Text + "</title>");
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
            Content.WriteLine("<h1 align=left>" + (string)Session["Club"] + ", " + Label3.Text + "</h1>\\");
            Content.WriteLine("<h2 align=left>" + Label4.Text + "</h2>\\");
            Content.WriteLine("<br>\\");
            Content.WriteLine("<table border='0\'>\\");
            DataTable dt = (DataTable)ViewState["Games"];
            GridViewRow row1Header = GridView1.HeaderRow;
            Content.Write("<tr>");
            Content.Write("<td width='4%' align='center'>" + row1Header.Cells[0].Text + "</td>");
            Content.Write("<td width='4%' align='center'>" + row1Header.Cells[1].Text + "</td>");
            Content.Write("<td width='30%' align='left'>" + row1Header.Cells[2].Text + "</td>");
            Content.Write("<td width='30%' align='left'>" + row1Header.Cells[3].Text + "</td>");
            Content.Write("<td width='20%' align='center'>" + row1Header.Cells[4].Text + "</td>");
            Content.WriteLine("</tr>\\");
            Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");
            int LineCount = 0;

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            foreach (DataRow dgItem in dt.Rows)
            {
                LineCount++;
                Content.Write("<tr>");
                Content.Write("<td width='4%' align='center'>" + string.Format("{0:N0}", dgItem["Rondenummer"]) + "</td>");
                Content.Write("<td width='4%' align='center'>" + string.Format("{0:N0}", dgItem["Partijnummer"]) + "</td>");
                Content.Write("<td width='30%' align='left'>" + dgItem["SpelernaamWit"] + "</td>");
                Content.Write("<td width='30%' align='left'>" + dgItem["SpelernaamZwart"] + "</td>");
                if ((double)dgItem["Uitslag"] > -1.0)
                {
                    Content.Write("<td width='20%' align='center'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 13).Trim() + "</td>");
                }
                else
                {
                    Content.Write("<td width='20%' align='center'>"+Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim()+"</td>");
                }
                Content.WriteLine("</tr>\\");
            }
            Client_MLC.Close();
            Content.WriteLine("</table>\\");
            Content.WriteLine("</div>\\");
            Content.WriteLine("<br />\\");
            Content.WriteLine("<br />\\");

            Content.WriteLine("\");");
            Container.WriteLine("    <script src=\"" + DisplayBody + "\"></script>");
            Container.WriteLine("</body>");
            Container.WriteLine("</html>");
            Container.Close();
            Content.Close();
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

            Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 4, "Closed");
            Client_WCF.Close();
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            
        }

    }
}