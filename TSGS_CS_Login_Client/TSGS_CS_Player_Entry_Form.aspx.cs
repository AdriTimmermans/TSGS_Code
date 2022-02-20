using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.IO;
using System.Globalization;

namespace TSGS_CS_Login_Client
{
    public static class Globals
    {
        public static string FotoPath;
        public static byte[] ProfilePicture;
        public static string Toernooi_Logo_Path;
        public static byte[] Toernooi_Logo;
    }

    public partial class TSGS_CS_Player_Entry_Form : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
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
                Session["Functionality"] = "PlayerEntry";
                Session["LastFunction"] = HttpContext.Current.Request.Url.AbsolutePath;

                FileUpload1.Attributes.Add("onchange", "UploadFile(this);");
                Master.SetErrorMessageGreen();
                Master.ErrorMessageVisibility(false);
                Fill_Texts();
                Image2.ImageUrl = "~/images/nofoto.png";
                Globals.FotoPath = "~/images/nofoto.png";
                Button6.Enabled = true;
                if ((string)Session["CallingFunction"] == "Blitz")
                {
                    Label23.Visible = true;
                    Button3.Visible = true;
                    Button4.Visible = true;
                    Set_Visibility_To(false);
                }
                else
                {
                    Label23.Visible = false;
                    Button3.Visible = false;
                    Button4.Visible = false;
                    Set_Visibility_To(true);
                    divFIDE.Visible = false;
                    divKNSB.Visible = false;
                }
            }
        }

        private void Set_Visibility_To (Boolean Visible)
        {
            Label2.Visible = Visible;
            Label12.Visible = Visible;
            Label13.Visible = Visible;
            Label14.Visible = Visible;
            Label16.Visible = Visible;
            Label17.Visible = Visible;
            Label18.Visible = Visible;
            Label19.Visible = Visible;
            Label20.Visible = Visible;
            Label21.Visible = Visible;
            Label22.Visible = Visible;
            Label24.Visible = Visible;
            Label25.Visible = Visible;
            Label26.Visible = Visible;
            Label27.Visible = Visible;
            Label47.Visible = Visible;
            TextBox15.Visible = Visible;
            TextBox14.Visible = Visible;
            TextBox13.Visible = Visible;
            TextBox12.Visible = Visible;
            TextBox3.Visible = Visible;
            TextBox10.Visible = Visible;
            TextBox7.Visible = Visible;
            TextBox11.Visible = Visible;
            TextBox1.Visible = Visible;
            TextBox2.Visible = Visible;
            TextBox16.Visible = Visible;
            TextBox25.Visible = Visible;
            TextBox26.Visible = Visible;
            TextBox27.Visible = Visible;
            CheckBox1.Visible = Visible;
            Button2.Visible = Visible;
            Button5.Visible = Visible;
            Button6.Visible = Visible;
            Button7.Visible = Visible;
            UpdatePanel1.Visible = Visible;
            Image2.Visible = Visible;
            divFIDE.Visible = Visible;
            divKNSB.Visible = Visible;
            RotateButton.Visible = Visible;
            TextBox24.Visible = Visible;
        }

        private string Convert_Bool(bool Boolean_Value)
        {
            string One_Zero;
            if (Boolean_Value)
            {
                One_Zero = "1";
            }
            else
            {
                One_Zero = "0";
            }
            return One_Zero;
        }

        protected void selecteer_Click(object sender, EventArgs e)
        {
            GridViewRow row = GridView1.SelectedRow;

            Label auxlab;
            auxlab = (Label)row.FindControl("Label33");
            TextBox15.Text = auxlab.Text;
            auxlab = (Label)row.FindControl("Label34");
            TextBox14.Text = auxlab.Text;
            auxlab = (Label)row.FindControl("Label35");
            TextBox13.Text = auxlab.Text;
            if (TextBox15.Text == "&nbsp;")
            {
                TextBox15.Text = "";
            }
            auxlab = (Label)row.FindControl("Label32");
            TextBox11.Text = auxlab.Text;
            auxlab = (Label)row.FindControl("Label36");
            TextBox10.Text = auxlab.Text;
            TextBox25.Text = auxlab.Text;
            TextBox26.Text = auxlab.Text;
            TextBox27.Text = auxlab.Text;
            auxlab = (Label)row.FindControl("Label37");
            TextBox12.Text = auxlab.Text;
            GridView1.Visible = false;
            divKNSB.Visible = false;
        }

        protected void GV2_selecteer_Click(object sender, EventArgs e)
        {
            GridViewRow row = GridView2.SelectedRow;

            Label auxlab;

            auxlab = (Label)row.FindControl("Label43");
            TextBox3.Text = auxlab.Text;
            auxlab = (Label)row.FindControl("Label42");
            TextBox1.Text = auxlab.Text;
            auxlab = (Label)row.FindControl("Label46");
            TextBox2.Text = auxlab.Text;
            GridView2.Visible = false;
            divFIDE.Visible = false;
        }

        protected void Button5_Click(object sender, System.EventArgs e)
        {

            if (TextBox15.Text.Length >= 3)
            {
                divKNSB.Visible = true;
                divFIDE.Visible = true;
                BindList();
            }
        }

        protected void Button3_Click(object sender, System.EventArgs e)
        {
            Label23.Visible = false;
            Button3.Visible = false;
            Button4.Visible = false;
            Set_Visibility_To(true);
        }

        protected void Button4_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("TSGS_CS_Non_Clubcompetition_Players.aspx");
        }

        protected void BindList()
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            string select_Player = "%" + TextBox15.Text.Trim() + "%";
            GridView1.Visible = true;
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            GridView1.Font.Name = "Arial";
            GridView1.Font.Size = (int)Session["Fontsize"];
            DataSet ds = Client_WCF.GetKNSBPlayerList(select_Player);
            GridView1.DataSource = ds;
            GridView1.AllowPaging = true;

            GridView1.PageSize = 10;
            GridView1.DataBind();

            if (GridView1.Rows.Count > 0)
            {
                GridViewRow rowHeader = GridView1.HeaderRow;
                rowHeader.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 18).Trim();
                rowHeader.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 19).Trim();
                rowHeader.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 17).Trim();
                rowHeader.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                rowHeader.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim();
                rowHeader.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                rowHeader.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
                rowHeader.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                rowHeader.Cells[5].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 28).Trim();
                rowHeader.Cells[6].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            }

            GridView2.Visible = true;
            GridView2.Font.Name = "Arial";
            GridView2.Font.Size = (int)Session["Fontsize"];
            DataSet dsFIDE = Client_WCF.GetFIDEPlayerList(select_Player);
            GridView2.DataSource = dsFIDE;
            GridView2.AllowPaging = true;

            GridView2.PageSize = 10;
            GridView2.DataBind();

            if (GridView2.Rows.Count > 0)
            {
                GridViewRow rowHeader = GridView2.HeaderRow;
                rowHeader.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 20).Trim();
                rowHeader.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 21).Trim();
                rowHeader.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 23).Trim();
                rowHeader.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                rowHeader.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 22).Trim();
                rowHeader.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 24).Trim();
            }


            Client_MLC.Close();
            Client_WCF.Close();

        }

        protected void OnRatingTextBoxChanged(object sender, EventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Master.ErrorMessageVisibility(false);
            TextBox tb = (TextBox)sender;
            double Input_TB = 0.0;
            int error = Client_WCF.ValidateReal(tb.Text.Trim(), true, 1000.0, true, 0.0, 3000.0, ref Input_TB);
            if (error == 0)
            {
                Master.ErrorMessageVisibility(false);
                tb.BackColor = System.Drawing.Color.White;
                Fill_Texts();
            }
            else
            {
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
                Master.SetErrorMessage(tb.Text.Trim() + ": " + Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], error + 1).Trim());
                tb.BackColor = System.Drawing.Color.Red;
                Client_MLC.Close();
            }
            Client_WCF.Close();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindList();
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
            BindList();
        }

        protected void GridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // When user moves mouse over the GridView row,First save original or previous color to new attribute,
                // and then change it by magenta color to highlight the gridview row.

                foreach (DataControlFieldCell cell in e.Row.Cells)
                {
                    // check all cells in one row
                    foreach (Control control in cell.Controls)
                    {
                        // Must use LinkButton here instead of ImageButton
                        // if you are having Links (not images) as the command button.
                        ImageButton button = control as ImageButton;
                        if (button != null)
                        {

                            button.Attributes.Add("onmouseover", "return SelectIcon(this);");
                            button.Attributes.Add("onmouseout", "return UnselectIcon(this);");
                        }
                    }
                }

            }
        }

        protected void Button6_Click(object sender, System.EventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_CS_WCF_Service.PlayerBasicData Player = new TSGS_CS_WCF_Service.PlayerBasicData();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            if (Check_Error())
            {
                Master.SetErrorMessage(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 2).Trim());
                Master.ErrorMessageVisibility(true);
                Master.SetErrorMessageRed();
            }
            else
            {
                Player.Competitie_Id = Convert.ToInt16(Session["Competition_Identification"]);
                Player.Titel = TextBox3.Text;
                Player.Achternaam = TextBox15.Text;
                Player.Tussenvoegsel = TextBox13.Text;
                Player.Voorletters = TextBox12.Text;
                Player.Roepnaam = TextBox14.Text;
                Player.KNSBnummer = Convert.ToInt32(TextBox11.Text);
                Player.FIDEnummer = Convert.ToInt32(TextBox1.Text);
                Player.Telefoonnummer = TextBox7.Text;
                Player.Team = 0;
                Player.Clublid = 1;
                Player.Deelnemer_teruggetrokken = 0;
                Player.Speelt_mee_sinds_ronde = Convert.ToSByte(Session["Round_Number"]);
                Player.Doet_mee_met_snelschaak = 0;
                Player.Speelt_blitz_sinds_ronde = 0;
                Player.Vrijgeloot = 0;
                Player.Wants_Email = Convert.ToSByte(CheckBox1.Checked);
                Player.Email_Address = TextBox16.Text;
                Player.Wants_SMS = 0;
                Player.Mobile_Number = "--";
                Player.Member_Premier_Group = 0;
//
// Get rating numbers
//
                Player.Rating = (float)Convert.ToDouble(TextBox10.Text, System.Globalization.CultureInfo.InvariantCulture);
                Player.KNSBrating = (float)Convert.ToDouble(TextBox27.Text, System.Globalization.CultureInfo.InvariantCulture);
                Player.FIDErating = (float)Convert.ToDouble(TextBox2.Text, System.Globalization.CultureInfo.InvariantCulture);
                Player.Snelschaakrating = (float)Convert.ToDouble(TextBox24.Text, System.Globalization.CultureInfo.InvariantCulture);
                Player.Rapidrating = (float)Convert.ToDouble(TextBox25.Text, System.Globalization.CultureInfo.InvariantCulture);
                Player.Startrating = Player.Rating;

                System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(Globals.FotoPath));
                Bitmap thumbnailBitmap = new Bitmap(image.Size.Width, image.Size.Height);
                Graphics thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                var imageRectangle = new Rectangle(0, 0, image.Size.Width, image.Size.Height);
                thumbnailGraph.DrawImage(image, imageRectangle);
                MemoryStream ms = new MemoryStream();
                thumbnailBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] localProfilePicture = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(localProfilePicture, 0, localProfilePicture.Length);
                Player.ProfilePicture = localProfilePicture;
                Client_WCF.AddPlayer(Player);
                int NewPID = Client_WCF.GetLastPlayerID();
                if (((int)Session["Competition_Identification"] > 0) && ((int)Session["Current_Status"] > 3))
                {
                    TSGS_CS_WCF_Service.AbsenteeData AbsenteeData = new TSGS_CS_WCF_Service.AbsenteeData();
                    AbsenteeData.Speler_ID = NewPID;
                    AbsenteeData.Rondenummer = (int)Session["Round_Number"];
                    AbsenteeData.Competitie_ID = (int)Session["Competition_Identification"];
                    AbsenteeData.Kroongroep_partijnummer = 0;
                    AbsenteeData.Afwezigheidscode = 1;
                    Client_WCF.AddAbsentee(AbsenteeData);
                }
                if (((int)Session["Competition_Identification"] > 0) && ((int)Session["Current_Status"] > 4))
                {
                    TSGS_CS_WCF_Service.GamesData game = new TSGS_CS_WCF_Service.GamesData();

                    float Rating = Client_WCF.GetPlayerRatingInCompetition(NewPID, (int)Session["Round_Number"], (int)Session["Competition_Identification"]);
                    game.Rondernr = (int)Session["Round_Number"];
                    game.Competitie_Id = (int)Session["Competition_Identification"];
                    game.Id_Witspeler = NewPID;
                    game.Zwart_Winst = 0;
                    game.Zwart_Remise = 0;
                    game.Zwart_Verlies = 0;
                    game.Wedstrijdtype = (byte)1;
                    game.Sorteerwaarde = 5000;
                    game.NumberChampionsgroupGame = 0;
                    game.Id_Zwartspeler = -1;
                    game.Wedstrijdresultaat = 12;
                    game.Wit_Winst = 0;
                    game.Wit_Remise = 0;
                    game.Wit_Verlies = 0;
                    Client_WCF.AddGame(game);
                }
            }
            Client_MLC.Close();
            Client_WCF.Close();
            Button6.Enabled = false;
        }

        private bool Check_Error()
        {
            bool ErrorStatus = false;

            if (TextBox15.Text.Trim() == "")
            {
                ErrorStatus = true;
                TextBox15.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                TextBox15.BackColor = System.Drawing.Color.White;
            }
            if (TextBox12.Text.Trim() == "")
            {
                ErrorStatus = true;
                TextBox12.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                TextBox12.BackColor = System.Drawing.Color.White;
            }
            if (TextBox14.Text.Trim() == "")
            {
                ErrorStatus = true;
                TextBox14.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                TextBox14.BackColor = System.Drawing.Color.White;
            }
            if (TextBox11.Text.Trim() == "")
            {
                TextBox11.Text = "0000";
            }
            if (TextBox10.Text.Trim() == "")
            {
                TextBox10.Text = "0.0";
            }
            if (TextBox27.Text.Trim() == "")
            {
                TextBox27.Text = "0000";
            } 
            if (TextBox1.Text.Trim() == "")
            {
                TextBox1.Text = "0000";
            }
            if (TextBox2.Text.Trim() == "")
            {
                TextBox2.Text = "0.0";
            }
            try
            {
                double startrating = (float)Convert.ToDouble(TextBox10.Text, System.Globalization.CultureInfo.InvariantCulture);
                if (startrating < 800.0 || startrating > 3000)
                {
                    ErrorStatus = true;
                    TextBox10.BackColor = System.Drawing.Color.Red;
                }
                else
                {
                    TextBox10.BackColor = System.Drawing.Color.White;
                    if (TextBox24.Text.Trim() == "")
                    {
                        TextBox24.Text = TextBox10.Text;
                    }
                    if (TextBox25.Text.Trim() == "")
                    {
                        TextBox25.Text = TextBox10.Text;
                    }
                }
            }
            catch
            {
                ErrorStatus = true;
                TextBox10.BackColor = System.Drawing.Color.Red;
            }
            return ErrorStatus;

        }

        private void Fill_Texts()
        {
            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);


            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            Label18.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 17).Trim();
            Label17.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim();
            Label19.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            Label16.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Label12.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            Label14.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            Label13.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            Label21.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            Label20.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            Label22.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            Label23.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 30).Trim();
            Label24.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 31).Trim();
            Label25.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 34).Trim();
            Label26.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 35).Trim();
            Label27.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 36).Trim();
            CVTB.ErrorMessage = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 32).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button3.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 7).Trim();
            Button4.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 8).Trim();
            Button5.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 13).Trim();
            Button6.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 14).Trim();
            Button7.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 29).Trim();

            CheckBox1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 16).Trim();

            Client_MLC.Close();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            if (FileUpload1.HasFile)
            {
                string version = "";
                int versionnr = 1;

                while (File.Exists(Server.MapPath("~/workimages/" + version + FileUpload1.FileName)))
                {
                    versionnr++;
                    version = "V" + versionnr.ToString().Trim();
                }
                Globals.FotoPath = "~/workimages/" + version + FileUpload1.FileName;

                FileUpload1.PostedFile.SaveAs(Server.MapPath("~/workimages/" + version + FileUpload1.FileName));

                System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath("~/workimages/" + version + FileUpload1.FileName));
                int newwidthimg = 80;
                float AspectRatio = (float)image.Size.Width / (float)image.Size.Height;
                int newHeight = Convert.ToInt32(newwidthimg / AspectRatio);
                Bitmap thumbnailBitmap = new Bitmap(newwidthimg, newHeight);
                Graphics thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                var imageRectangle = new Rectangle(0, 0, newwidthimg, newHeight);
                thumbnailGraph.DrawImage(image, imageRectangle);
                thumbnailBitmap.Save(Server.MapPath("~/workimages/Resized_" + version + FileUpload1.FileName));
                thumbnailGraph.Dispose();
                thumbnailBitmap.Dispose();
                image.Dispose();
                Globals.FotoPath = "~/workimages/" + version + FileUpload1.FileName;
                Image2.ImageUrl = "~/workimages/Resized_" + version + FileUpload1.FileName;
            }
            else
            {
                Globals.FotoPath = "images/notdone2.jpg";
            }

        }

        protected void Button7_Click(object sender, EventArgs e)
        {
                Response.Redirect(Request.RawUrl);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if ((string)Session["CallingFunction"] == "Blitz")
            {
                Response.Redirect("TSGS_CS_Link_Player.aspx");
            }
            else
            {
                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }
        }

        protected void RotateButton_Click(object sender, ImageClickEventArgs e)
        {
            System.Drawing.Image imageOrg = System.Drawing.Image.FromFile(Server.MapPath(Globals.FotoPath));
            System.Drawing.Image image = TSGS_CS_Utilities.RotateImage(imageOrg, 90.0f);
            string version = "";
            int versionnr = 1;
            while (File.Exists(Server.MapPath(Globals.FotoPath)))
            {
                versionnr++;
                version = "V" + versionnr.ToString().Trim();
                Globals.FotoPath = "~/workimages/" + version + "tempbig.jpg";
            }
            image.Save(Server.MapPath(Globals.FotoPath));
            int newwidthimg = 80;
            float AspectRatio = (float)imageOrg.Size.Width / (float)imageOrg.Size.Height;
            int newHeight = Convert.ToInt32(newwidthimg / AspectRatio);
            Bitmap thumbnailBitmap = new Bitmap(newwidthimg, newHeight);
            Graphics thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            var imageRectangle = new Rectangle(0, 0, newwidthimg, newHeight);
            thumbnailGraph.DrawImage(image, imageRectangle);
            version = "";
            versionnr = 1;
            string localPath = "~/workimages/" + version + "tempsmall.jpg";
            while (File.Exists(Server.MapPath(localPath)))
            {
                versionnr++;
                version = "V" + versionnr.ToString().Trim();
                localPath = "~/workimages/" + version + "tempsmall.jpg";
            }
            thumbnailBitmap.Save(Server.MapPath(localPath));
            thumbnailGraph.Dispose();
            thumbnailBitmap.Dispose();
            imageOrg.Dispose();
            image.Dispose();
            Image2.ImageUrl = localPath;
        }

        protected void CustomValidator1_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            DateTime d;
            e.IsValid = DateTime.TryParseExact(e.Value, "MM/dd/YYYY", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        }
    }
}