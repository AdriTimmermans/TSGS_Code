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
    public partial class TSGS_CS_Display_ELO_List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "DisplayELOList";
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
                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                Client_WCF.Remove_Templist((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                Client_WCF.Create_Worklist((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                Client_WCF.Close();
                Session["PreRenderFirstPass"] = true;
                Fill_Texts();
            }
        }

       protected void Page_PreRender(object sender, EventArgs e)
       {
           if ((bool)Session["PrerenderFirstPass"])
           {
               Session["PreRenderFirstPass"] = false;
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
            Label3.Text = (String)Session["Club"] + ", "+(string)Session["Competition"];
            Label4.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim(), (int)Session["Round_Number"]);
            Label5.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataSet ds = Client_WCF.GetELORankingList((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
            ds.Tables[0].Columns.Add("PL", typeof(string));

            //            Cycle through DataSet to determine position

            int Position = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Position = Position + 1;
                ds.Tables[0].Rows[i]["PL"] = Position;
            }
            ViewState["DSList"] = ds.Tables[0];
            GridView1.DataSource = ds;
            GridView1.AllowPaging = true;
            GridView1.PageSize = 15;
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
            bool PictureRequired = false;

            string RootPath =  Server.MapPath("~/htmlfiles/"+Convert.ToString((int)Session["Competition_Identification"]).Trim());
            string DisplayHeader = "TSGS_CS_ELO_Rankinglist.html";
            string DisplayBody = "ELO_Rankinglist_body.js";

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
            Content.WriteLine("<table border='0\'>\\");
            DataTable dt = (DataTable)ViewState["DSList"];
            GridViewRow row1Header = GridView1.HeaderRow;
            Content.Write("<tr>");
            Content.Write("<td width='4%' align='right'>" + row1Header.Cells[0].Text + "</td>");
            Content.Write("<td width='8%' align='center'>&nbsp;</td>");
            Content.Write("<td width='50%' align='left'>" + row1Header.Cells[2].Text + "</td>");
            Content.Write("<td width='4%' align='right'>" + row1Header.Cells[3].Text + "</td>");
            Content.Write("<td width='4%' align='right'>" + row1Header.Cells[4].Text + "</td>");
            Content.Write("<td width='30%' align='right'>&nbsp;</td>");
            Content.WriteLine("</tr>\\");
            Content.WriteLine("<tr><td height='8'>&nbsp;<br></td></tr>\\");
            foreach (DataRow dgItem in dt.Rows)
            {
                Content.Write("<tr>");

                Content.Write("<td width='4%' align='right'>" + dgItem["PL"].ToString() + "</td>");

                int PID = Convert.ToInt16(dgItem["Speler_Id"]);
                Position = Convert.ToInt16((string)dgItem["PL"]);
                PictureRequired = (Position <= 3);
                if (PictureRequired)
                {
                    Content.Write("<td width='8%' align='center'><img src='" + Client_WCF.StringImage(PID, (int)Session["Competition_Identification"]) + "'></td>");
                }
                else
                {
                    byte[] file = File.ReadAllBytes(Server.MapPath("~/images/transp.gif"));
                    Content.Write("<td width='8%' align='center'><img src='" + Client_WCF.MakeImageSourceData((byte[])file) + "'></td>");
                }

                Content.Write("<td width='40%' align='left'>" + dgItem["Spelernaam"] + "</td>");
                Content.Write("<td width='6%' align='right'>" + String.Format("{0:#000.0}", dgItem["ELO_Rating"]) + "</td>");
                Content.Write("<td width='6%' align='right'>" + String.Format("{0:#00.0}", dgItem["Rating_Gain"]) + "</td>");
                Content.Write("<td width='30%' align='right'>&nbsp;</td>");
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
            Label1.Visible = false;
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
            Client_WCF.Update_Workflow_Item("[Afdrukken ELO ranglijst]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
            Client_WCF.Update_Workflow_Item("[Publiceren]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);

            Client_WCF.Close();
            Client_MLC.Close();

            if (!Label1.Visible)
            {
                if ((bool)Session["AutoOverview"])
                {
                    Response.Redirect("TSGS_CS_Display_Competition_List.aspx");
                }
                else
                {
                    Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            bool PictureRequired = false;
            DataTable dt;
            int Gridline = e.Row.RowIndex + GridView1.PageSize * GridView1.PageIndex;
            dt = (DataTable)ViewState["DSList"];

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                e.Row.Cells[0].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[1].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[2].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[3].Font.Size = (int)Session["Fontsize"];
                int PID = Convert.ToInt16(dt.Rows[Gridline]["Speler_Id"]);
                int Position = Convert.ToInt16(dt.Rows[Gridline]["PL"]);
                if (Position <= 3)
                {
                    PictureRequired = true;
                }
                Image imgPicture;
                if (PictureRequired)
                {
                    imgPicture = (Image)e.Row.FindControl("imgPicture");
                    imgPicture.ImageUrl = Client_WCF.StringImage(PID, (int)Session["Competition_Identification"]);
                }
                else
                {
                    byte[] file = File.ReadAllBytes(Server.MapPath("~/images/transp.gif"));
                    imgPicture = (Image)e.Row.FindControl("imgPicture");
                    imgPicture.ImageUrl = Client_WCF.MakeImageSourceData((byte[])file);
                }

                Client_WCF.Close();

            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }
    }
}
