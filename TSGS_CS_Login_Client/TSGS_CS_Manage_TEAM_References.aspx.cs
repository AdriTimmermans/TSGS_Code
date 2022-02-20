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

    public partial class TSGS_CS_Manage_TEAM_References : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            Session["Functionality"] = "ManageTeams";
            Session["LastFunction"] = HttpContext.Current.Request.Url.AbsolutePath;

            if (IsPostBack)
            {
                int aux = TSGS_CS_Extention_Methods.GetLanguageCode(TSGS_CS_Extention_Methods.GetPostBackControlId(this));
                if (aux >= 1)
                {
                    Session["Language"] = aux;
                    if (GridView1.Visible)
                    {
                        Master.SetErrorMessageGreen();
                        Master.ErrorMessageVisibility(false);
                        Label1.Visible = false;
                        Fill_Texts();
                    }
                    else
                    {
                        Fill_Gridview_TeamPlayers();
                    }
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
            Label3.Visible = false;
            Label4.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Button3.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 19).Trim();
            /*
            Other text filling actions
            */
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataSet ds = Client_WCF.GetTeamData((string)Session["Club_Id"]);
            ViewState["Teams"] = ds.Tables[0];
            GridView1.Font.Name = "Arial";
            GridView1.Font.Size = (int)Session["Fontsize"];

            GridView1.DataSource = ds;
            GridView1.AllowPaging = true;
            GridView1.PageSize = 8;
            GridView1.DataBind();

            GridViewRow row1Header = GridView1.HeaderRow;
            row1Header.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            row1Header.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            row1Header.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            row1Header.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            row1Header.Cells[3].HorizontalAlign = HorizontalAlign.Left;
            row1Header.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            row1Header.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[5].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
            row1Header.Cells[5].HorizontalAlign = HorizontalAlign.Left;
            Client_WCF.Close();
            Client_MLC.Close();
        }


        protected void Fill_Gridview_TeamPlayers()
        {

            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            /*
            Other text filling actions
            */
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataSet ds = Client_WCF.GetTeamPlayers((string)Session["Club_Id"]);
            ViewState["Players"] = ds.Tables[0];
            GridView2.Font.Name = "Arial";
            GridView2.Font.Size = (int)Session["Fontsize"];

            GridView2.DataSource = ds;
            GridView2.AllowPaging = true;
            GridView2.PageSize = 8;
            GridView2.DataBind();

            GridViewRow row1Header = GridView2.HeaderRow;
            row1Header.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim();
            row1Header.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 13).Trim();
            row1Header.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            row1Header.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 14).Trim();
            row1Header.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim();
            row1Header.Cells[3].HorizontalAlign = HorizontalAlign.Left;
            row1Header.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 16).Trim();
            row1Header.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            row1Header.Cells[5].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 17).Trim();
            row1Header.Cells[5].HorizontalAlign = HorizontalAlign.Left;
            row1Header.Cells[6].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 18).Trim();
            row1Header.Cells[6].HorizontalAlign = HorizontalAlign.Right;
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                for (int i = 0; i < 5; i++)
                {
                    e.Row.Cells[i].Font.Size = (int)Session["Fontsize"];
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Left;
                }
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                LinkButton lbDel = (LinkButton)e.Row.FindControl("lbDelete");
                if (lbDel != null)
                {
                    TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
                    lbDel.OnClientClick = "return confirm('" + Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 20).Trim() + "');";
                    Client_MLC.Close();
                }
            }
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
            Fill_Gridview_TeamPlayers();
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt;
            int Gridline = e.Row.RowIndex + GridView2.PageSize * GridView2.PageIndex;
            dt = (DataTable)ViewState["Players"];

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                for (int i = 0; i < 7; i++)
                {
                    e.Row.Cells[i].Font.Size = (int)Session["Fontsize"];
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Left;
                }
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                int PID = Convert.ToInt16(dt.Rows[Gridline]["Deelnemer"]);
                Image imgPicture;
                imgPicture = (Image)e.Row.FindControl("imgPicture");
                imgPicture.ImageUrl = Client_WCF.StringImage(PID, (int)Session["Competition_Identification"]);
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

            string RootPath =  Server.MapPath("~/htmlfiles/"+Convert.ToString((int)Session["Competition_Identification"]).Trim());
            string DisplayHeader = "TSGS_CS_Teams.html";
            string DisplayBody = "Teams_body.js";

            StreamWriter Container = new StreamWriter(RootPath + @"\" + DisplayHeader);
            StreamWriter Content = new StreamWriter(RootPath + @"\" + DisplayBody);

            DataTable dtP = (DataTable)ViewState["Players"];
            DataTable dtT = (DataTable)ViewState["Teams"];

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

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
            Content.WriteLine("<h1 align=left>" + Label4.Text + "</h1>\\");
            Content.WriteLine("<br>\\");
            Content.WriteLine("<table border='0\'>\\");
            int New_Team;
            string Foto_Line = "";
            string Name_Line = "";
            string Rating_Line = "";
            string FIDE_Rating_Line = "";
            string Team_Name_Line = "";
            string Team_Name;
            string Team_URL;
            string Player_Image;
            int PID;

            foreach (DataRow dgTeamItem in dtT.Rows)
            {
                New_Team = (int)dgTeamItem["Team_Nr"];
                Team_Name = (string)dgTeamItem["Team_Naam"];
                Team_URL = (string)dgTeamItem["URL"];
                Team_Name_Line = "<tr><td colspan='10'>&nbsp;</td></tr><tr><td colspan='10'><a href='" + Team_URL.Trim() + "'><H1>" + Team_Name + "</H1></a></td></tr>";
                Content.WriteLine(Team_Name_Line + "\\");
                Foto_Line = "<tr>";
                Content.WriteLine(Foto_Line + "\\");
                Name_Line = "<tr>";
                Rating_Line = "<tr>";
                FIDE_Rating_Line = "<tr>";

                foreach (DataRow dgItem in dtP.Rows)
                {
                    if((int)dgItem["Team_Nr"] == New_Team)
                    {
                        PID = (int)dgItem["Deelnemer"];
                        Player_Image = Client_WCF.StringImage(PID, (int)Session["Competition_Identification"]);
                        Foto_Line = "<td width='100' align='center'>" + "<img src='" + Player_Image + "' height='130' >  " + "</td>";
                        Content.WriteLine(Foto_Line + "\\");
                        Name_Line += "<td width='100' align='center'>" + (string)dgItem["SpelerNaam"] + "</td>";
                        object value = dgItem["KNSBRating"];
                        if (value != DBNull.Value)
                        {
                            Rating_Line += "<td width='100' align='center'>KNSB:" + string.Format("{0:#000.0}", (double)dgItem["KNSBRating"]) + "</td>";
                        }
                        else
                        {
                            Rating_Line += "<td width='100' align='center'>KNSB: 0</td>";
                        }
                        value = dgItem["FIDERating"];
                        if (value != DBNull.Value)
                        {
                            FIDE_Rating_Line += "<td width='100' align='center'>FIDE:" + string.Format("{0:#000.0}", (double)dgItem["FIDERating"]) + "</td>";
                        }
                        else
                        {
                            FIDE_Rating_Line += "<td width='100' align='center'>FIDE: 0</td>";
                        }
                    }
                }
                Foto_Line = "</tr>";
                Name_Line += "</tr>";
                Rating_Line += "</tr>";
                FIDE_Rating_Line += "</tr>";

                Content.WriteLine(Foto_Line + "\\");
                Content.WriteLine(Name_Line + "\\");
                Content.WriteLine(Rating_Line + "\\");
                Content.WriteLine(FIDE_Rating_Line + "\\");

            }

            Content.WriteLine("</table><br>\\");
            Content.WriteLine("\");");
            Container.WriteLine("    <script src=\"" + DisplayBody + "\"></script>");
            Container.WriteLine("<HR Size=4 Width='75%'>");
            Container.WriteLine("</body>");
            Container.WriteLine("</html>");
            Container.Close();
            Content.Close();

            string Display_Competition = Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_basis").Trim() + "/" +
                                         Client_WCF.GetStringFromAlgemeneInfo((int)Session["Competition_Identification"], "Website_Competitie").Trim();
            Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], DisplayHeader);
            Client_WCF.FTP_Basis_Upload_File(Display_Competition, RootPath, (int)Session["Competition_Identification"], DisplayBody);
            Client_WCF.Close();

            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            /*
            trigger actions for main function of form
            */
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            /*            Client_WCF.<xxxz>((int)Session["Competition_Identification"], (int)Session["Round_Number"]);*/
            Client_WCF.Close();
            Label1.Visible = true;
            Button2.Visible = false;
            Button3.Visible = true;
            GridView1.Visible = false;
            GridView2.Visible = true;
            Fill_Gridview_TeamPlayers();

            /*            Session["Current_Status"] = <z>;
                        TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                        Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], <z>);
                        Client_WCF.Update_Workflow_Item("[<XXXY>]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
                        Client_WCF.Close();*/
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string TeamNaam = "--";
            string URL = "--";
            int Team_Volg_Nummer = 0;

            if (e.CommandName == "EditRow")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                GridView1.EditIndex = rowIndex;
                Fill_Texts();
            }
            else if (e.CommandName == "DeleteRow")
            {
                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                Client_WCF.RemoveTeam((string)Session["Club_Id"], Convert.ToInt32(e.CommandArgument));
                Fill_Texts();
                Client_WCF.Close();
            }
            else if (e.CommandName == "CancelUpdate")
            {
                GridView1.EditIndex = -1;
                Fill_Texts();
            }
            else if (e.CommandName == "UpdateRow")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

                string CheckInput = ((TextBox)GridView1.Rows[rowIndex].FindControl("TextBox11")).Text;

                int error = Client_WCF.ValidateString(CheckInput, false, "-", ref TeamNaam);
                if (error == 0)
                {
                    CheckInput = ((TextBox)GridView1.Rows[rowIndex].FindControl("TextBox12")).Text;
                    error = Client_WCF.ValidateString(CheckInput, false, "-", ref URL);
                }
                if (error == 0)
                {
                    CheckInput = ((TextBox)GridView1.Rows[rowIndex].FindControl("TextBox15")).Text;
                    error = Client_WCF.ValidateInteger(CheckInput, false, 0, true, 0, 99, ref Team_Volg_Nummer);
                }
                if (error == 0)
                {
                    Client_WCF.UpdateTeam((string)Session["Club_Id"], Team_Volg_Nummer, URL, TeamNaam, Convert.ToInt32(e.CommandArgument));
                    GridView1.EditIndex = -1;
                    Fill_Texts();
                }
                else
                {
                    Master.SetErrorMessageRed();
                    Master.ErrorMessageVisibility(true);
                    Master.SetErrorMessage(CheckInput + ": " + Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], error + 1).Trim());
                }
                Client_MLC.Close();
                Client_WCF.Close();
            }
            else if (e.CommandName == "InsertRow")
            {

                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

                string CheckInput = ((TextBox)GridView1.FooterRow.FindControl("TextBox13")).Text;
                int error = Client_WCF.ValidateString(CheckInput, false, "-", ref TeamNaam);
                if (error == 0)
                {
                    CheckInput = ((TextBox)GridView1.FooterRow.FindControl("TextBox14")).Text;
                    error = Client_WCF.ValidateString(CheckInput, false, "-", ref URL);
                }
                if (error == 0)
                {
                    CheckInput = ((TextBox)GridView1.FooterRow.FindControl("TextBox16")).Text;
                    error = Client_WCF.ValidateInteger(CheckInput, false, 0, true, 1, 99, ref Team_Volg_Nummer);
                }
                if (error == 0)
                {
                    Client_WCF.AddTeam((string)Session["Club_Id"], URL, TeamNaam, Team_Volg_Nummer);
                    GridView1.EditIndex = -1;
                    Fill_Texts();
                }
                else
                {
                    Master.SetErrorMessageRed();
                    Master.ErrorMessageVisibility(true);
                    Master.SetErrorMessage(CheckInput + ":"+Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], error + 1).Trim());
                }
                Client_MLC.Close();
                Client_WCF.Close();
                GridView1.EditIndex = -1;
                Fill_Texts();
            }
        }
    }
}