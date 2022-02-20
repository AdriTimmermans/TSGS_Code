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

    public partial class TSGS_CS_Display_Blitz_Results : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "DisplayBlitzResults";
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

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataSet ds = Client_WCF.GetPlayersBlitz((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
            ds.Tables[0].Columns.Add("PL", typeof(int));

            int Position = 0;
            int LastGroup = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (LastGroup != Convert.ToInt16(ds.Tables[0].Rows[i]["Groepnummer"]))
                {
                    Position = 0;
                }
                Position = Position + 1;
                LastGroup = Convert.ToInt16(ds.Tables[0].Rows[i]["Groepnummer"]);
                ds.Tables[0].Rows[i]["PL"] = Position;
            }
            ds.AcceptChanges();
            ViewState["BlitzResults"] = ds;
            GridView1.DataSource = ds;
            GridView1.AllowPaging = true;
            GridView1.PageSize = 5;
            GridView1.DataBind();

            GridViewRow row1Header = GridView1.HeaderRow;
            row1Header.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            row1Header.Cells[0].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            row1Header.Cells[1].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            row1Header.Cells[3].HorizontalAlign = HorizontalAlign.Left;
            row1Header.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            row1Header.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[5].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            row1Header.Cells[5].HorizontalAlign = HorizontalAlign.Right;

            Client_WCF.Close();
            Client_MLC.Close();

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Fill_Texts();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataSet ds = (DataSet)ViewState["BlitzResults"];

            DataTable dt = ds.Tables[0];
            int Gridline = e.Row.RowIndex + GridView1.PageSize * GridView1.PageIndex;

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                e.Row.Cells[0].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[1].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[2].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[3].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[4].Font.Size = (int)Session["Fontsize"]; 
                e.Row.Cells[5].Font.Size = (int)Session["Fontsize"];
                Image imgPicture = (Image)e.Row.FindControl("imgPicture");
                imgPicture.ImageUrl = Client_WCF.StringImage((int) dt.Rows[Gridline]["Deelnemer_Id"], (int) Session["Competition_Identification"]);
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            /*
            Displyfile for Blitz results
            */
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            
            DataSet ds = (DataSet)ViewState["BlitzResults"];
            //
            // Create overview
            //
            string RootPath =  Server.MapPath("~/htmlfiles/"+Convert.ToString((int)Session["Competition_Identification"]).Trim());
            string DisplayHeader = "TSGS_CS_Display_Blitz_Round_Results.html";
            string DisplayBody = "Display_Blitz_Round_Results_body.js";

            StreamWriter Container = new StreamWriter(RootPath + @"\" + DisplayHeader);
            StreamWriter Content = new StreamWriter(RootPath + @"\" + DisplayBody);

            string doc_type = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";

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

            Content.WriteLine("document.write(\"\\");
            Content.WriteLine("<div id='printable' class='PrintSettings'>\\");
            Content.WriteLine("<h1 align=left>" + (string)Session["Club"] + ", " + (string)Session["Competition"] + "</h1>\\");
            Content.WriteLine("<h2 align=left>" + string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim(), (int)Session["Round_Number"]) + "</h2>\\");
            Content.WriteLine("<br>\\");
            Content.WriteLine("<table border='0\'>\\");

            Content.Write("<tr>");
            Content.Write("<td width='2%' align='right'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim() + "</td>");
            Content.Write("<td width='2%' align='right'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim() + "</td>");
            Content.Write("<td width='10%'>&nbsp;</td>");
            Content.Write("<td width='30%' align='right'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim() + "</td>");
            Content.Write("<td width='10%' align='right'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim() + "</td>");
            Content.Write("<td width='10%' align='right'>" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim() + "</td>");
            Content.WriteLine("</tr>\\");
            Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");

            int Max_Players = (int)ds.Tables[0].Rows.Count;

            for (int i = 0; i < Max_Players; i++)
            {
                Content.Write("<td width='2%' align='right'>" + string.Format("{0:N0}", (int)ds.Tables[0].Rows[i]["Groepnummer"]) + "</td>");
                Content.Write("<td width='2%' align='right'>" + string.Format("{0:N0}", (int)ds.Tables[0].Rows[i]["PL"]) + "</td>");
                Content.Write("<td width='10%' align='center'><img src='" + Client_WCF.StringImage((int)ds.Tables[0].Rows[i]["Deelnemer_Id"], (int)Session["Competition_Identification"]) + "'></td>");
                Content.Write("<td width='30%' align='left'>" + (string)ds.Tables[0].Rows[i]["Spelernaam"] + "</td>");
                Content.Write("<td width='10%' align='right'>" + string.Format("{0:N1}", (double)ds.Tables[0].Rows[i]["Matchpunten"]) + "</td>");
                Content.Write("<td width='10%' align='right'>" + string.Format("{0:N1}", (double)ds.Tables[0].Rows[i]["Strafpunten"]) + "</td>");
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