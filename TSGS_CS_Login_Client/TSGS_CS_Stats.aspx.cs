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
    public partial class TSGS_CS_Stats : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "Stats";
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
                Build_Dataset();
                Fill_Texts();
                if ((bool)Session["AutoOverview"])
                {
                    Button1_Click_Actions(null, null);
                }
            }
        }

        protected void Build_Dataset()
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            DataSet ds = new DataSet();
            ds.Tables.Add();
            ds.Tables[0].Columns.Add("Ronde", typeof(int));
            ds.Tables[0].Columns.Add("Intern", typeof(int));
            ds.Tables[0].Columns.Add("Extern", typeof(int));
            ds.Tables[0].Columns.Add("Vrij", typeof(int));
            ds.Tables[0].Columns.Add("Afwezig", typeof(int));
            ds.Tables[0].Columns.Add("Totaal", typeof(int));
            int Aantal_ronden = (int)Session["Round_Number"];

            DataSet dsT = new DataSet();

            for (int i = 0; i < Aantal_ronden; i++)
            {
                dsT = Client_WCF.GetRetentionStatistics((int)Session["Competition_Identification"], i + 1);
                DataRow dr = dsT.Tables[0].Rows[0];
                if (Convert.ToInt16(dr["Totaal"]) != 0)
                {
                    ds.Tables[0].Rows.Add(i + 1,
                        Convert.ToInt16(dr["Intern"]),
                        Convert.ToInt16(dr["Extern"]),
                        Convert.ToInt16(dr["Vrij"]),
                        Convert.ToInt16(dr["Afwezig"]),
                        Convert.ToInt16(dr["Totaal"]));
                }
            }
            ds.AcceptChanges();
            ViewState["Stats"] = ds;

            Client_WCF.Close();
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
            Label3.Font.Size = (int)Session["Fontsize"] + 2;
            Label4.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();

            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataSet ds = (DataSet)ViewState["Stats"];
            GridView1.DataSource = ds;
            GridView1.AllowPaging = true;
            GridView1.PageSize = 10;
            for (int i = 0; i < 6; i++)
            {
                GridView1.Columns[i].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            }

            GridView1.DataBind();

            GridViewRow row1Header = GridView1.HeaderRow;
            row1Header.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim();
            row1Header.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            row1Header.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            row1Header.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            row1Header.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            row1Header.Cells[5].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();

            ViewState["StatsTable"] = ds.Tables[0];
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
            /*
            trigger actions for main function of form
            */
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            string RootPath = Server.MapPath("~/htmlfiles/" + Convert.ToString((int)Session["Competition_Identification"]).Trim());
            string DisplayHeader = "TSGS_CS_Stats.html";
            string DisplayBody = "Stats_body.js";

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
            Content.WriteLine("<h1 align=left>" + (string)Session["Club"] + ", " + Label3.Text + "</h1>\\");
            Content.WriteLine("<h2 align=left>" + System.DateTime.Now.ToString() + "</h2>\\");
            Content.WriteLine("<br>\\");
            Content.WriteLine("<table border='0\'>\\");
            DataTable dt = (DataTable)ViewState["StatsTable"];
            GridViewRow row1Header = GridView1.HeaderRow;
            Content.Write("<tr>");
            Content.Write("<td width='5%' align='right'>" + row1Header.Cells[0].Text + "</td>");
            Content.Write("<td width='5%' align='right'>" + row1Header.Cells[1].Text + "</td>");
            Content.Write("<td width='5%' align='right'>" + row1Header.Cells[2].Text + "</td>");
            Content.Write("<td width='5%' align='right'>" + row1Header.Cells[3].Text + "</td>");
            Content.Write("<td width='5%' align='right'>" + row1Header.Cells[4].Text + "</td>");
            Content.Write("<td width='5%' align='right'>" + row1Header.Cells[5].Text + "</td>");
            Content.Write("<td width='70%' align='right'>&nbsp;</td>");
            Content.WriteLine("</tr>\\");
            Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");
            foreach (DataRow dgItem in dt.Rows)
            {
                Content.Write("<tr>");
                Content.Write("<td width='5%' align='right'>" + String.Format("{0:f0}", dgItem["Ronde"]) + "</td>");
                Content.Write("<td width='5%' align='right'>" + String.Format("{0:f0}", dgItem["Intern"]) + "</h2></td>");
                Content.Write("<td width='5%' align='right'>" + String.Format("{0:f0}", dgItem["Extern"]) + "</td>");
                Content.Write("<td width='5%' align='right'>" + String.Format("{0:f0}", dgItem["Vrij"]) + "</td>");
                Content.Write("<td width='5%' align='right'>" + String.Format("{0:f0}", dgItem["Afwezig"]) + "</td>");
                Content.Write("<td width='5%' align='right'>" + String.Format("{0:f0}", dgItem["Totaal"]) + "</td>");
                Content.Write("<td width='70%' align='right'>&nbsp;</td>");
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
            string Display_Competition = Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_basis").Trim() + "/" +
                                         Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_Competitie").Trim();
            Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], DisplayHeader);
            Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], DisplayBody);
            Client_WCF.Close();
            Client_MLC.Close();
            Label1.Visible = true;

            if ((bool)Session["AutoOverview"])
            {
                Session["AutoOverview"] = "false";

                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }
        }


        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

    }
}
