using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Data;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Correction_Player_Data_Details : System.Web.UI.Page
    {
 
        List<string> ControlList = new List<string>
        {
                "TextBox3", "TextBox4", "TextBox5", "TextBox6", "TextBox17", 
                "TextBox8", "TextBox7", "TextBox1", "TextBox11", "TextBox13", 
                "TextBox24", "TextBox16", "CheckBox17", "CheckBox18", "TextBox19", 
                "CheckBox26", "CheckBox27", "TextBox28", "CheckBox29", "CheckBox31"};

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "CorrectionPlayerData";
            Session["LastFunction"] = HttpContext.Current.Request.Url.AbsolutePath;
            Session["CallingControl"] = TSGS_CS_Extention_Methods.GetPostBackControlId(this);

            if (IsPostBack)
            {
                int aux = TSGS_CS_Extention_Methods.GetLanguageCode(TSGS_CS_Extention_Methods.GetPostBackControlId(this));
                if (aux >= 1)
                {
                    Session["Language"] = aux;
                    Fill_Texts();
                    Fill_Data();
                }
            }
            else
            {
                FileUpload1.Attributes.Add("onchange", "UploadFile(this);");
                Master.SetErrorMessageGreen();
                Master.ErrorMessageVisibility(false);
                Fill_Texts();
                Fill_Data();
            }
        }

        protected void selecteer_Click(object sender, EventArgs e)
        {
            GridViewRow row = GridView1.SelectedRow;

            Label auxlab;

            auxlab = (Label)row.FindControl("Label132");
            TextBox7.Text = auxlab.Text;
            auxlab = (Label)row.FindControl("Label136");
            TextBox18.Text = auxlab.Text;
            GridView1.Visible = false;
            divKNSB.Visible = false;
        }

        protected void GV2_selecteer_Click(object sender, EventArgs e)
        {
            GridViewRow row = GridView2.SelectedRow;

            Label auxlab;

            auxlab = (Label)row.FindControl("Label143");
            TextBox17.Text = auxlab.Text;
            auxlab = (Label)row.FindControl("Label142");
            TextBox1.Text = auxlab.Text;
            auxlab = (Label)row.FindControl("Label146");
            TextBox2.Text = auxlab.Text;
            GridView2.Visible = false;
            divFIDE.Visible = false;
        }

        protected void Button5_Click(object sender, System.EventArgs e)
        {

            if (TextBox3.Text.Length >= 3)
            {
                BindList();
            }
        }

        protected void Button4_Click(object sender, System.EventArgs e)
        {

            GridView2.Visible = false;
            divFIDE.Visible = false;
        }

        protected void BindList()
        {
            divKNSB.Visible = true;
            divFIDE.Visible = true;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            string select_Player = "%" + TextBox3.Text.Trim() + "%";
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
                rowHeader.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], "PlayerEntry", (int)Session["Language"], 18).Trim();
                rowHeader.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], "PlayerEntry", (int)Session["Language"], 19).Trim();
                rowHeader.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], "PlayerEntry", (int)Session["Language"], 17).Trim();
                rowHeader.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                rowHeader.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], "PlayerEntry", (int)Session["Language"], 15).Trim();
                rowHeader.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                rowHeader.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], "PlayerEntry", (int)Session["Language"], 3).Trim();
                rowHeader.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                rowHeader.Cells[5].Text = Client_MLC.GetMLCText((string)Session["Project"], "PlayerEntry", (int)Session["Language"], 28).Trim();
                rowHeader.Cells[6].Text = Client_MLC.GetMLCText((string)Session["Project"], "PlayerEntry", (int)Session["Language"], 8).Trim();
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
                rowHeader.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], "PlayerEntry", (int)Session["Language"], 20).Trim();
                rowHeader.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], "PlayerEntry", (int)Session["Language"], 21).Trim();
                rowHeader.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], "PlayerEntry", (int)Session["Language"], 23).Trim();
                rowHeader.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                rowHeader.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], "PlayerEntry", (int)Session["Language"], 22).Trim();
                rowHeader.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], "PlayerEntry", (int)Session["Language"], 24).Trim();
            }
            Client_MLC.Close();
            Client_WCF.Close();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            switch ((String)Session["CallingControl"])
            {
            case "TextBox3":
                {
                    TextBox4.Focus();
                    break;
                }
            case "TextBox4":
                {
                    TextBox5.Focus();
                    break;
                }
            case "TextBox5":
                {
                    TextBox6.Focus();
                    break;
                }
            case "TextBox6":
                {
                    TextBox17.Focus();
                    break;
                }
            case "TextBox17":
                {
                    TextBox8.Focus();
                    break;
                }
            case "TextBox8":
                {
                    TextBox7.Focus();
                    break;
                }
            case "TextBox7":
                {
                    TextBox1.Focus();
                    break;
                }
            case "TextBox1":
                {
                    TextBox11.Focus();
                    break;
                }
            case "TextBox11":
                {
                    TextBox13.Focus();
                    break;
                }
            case "TextBox13":
                {
                    TextBox24.Focus();
                    break;
                }
            case "TextBox24":
                {
                    TextBox16.Focus();
                    break;
                }
            case "TextBox16":
                {
                    CheckBox17.Focus();
                    break;
                }
            case "CheckBox17":
                {
                    TextBox18.Focus();
                    break;
                }
            case "CheckBox18":
                {
                    TextBox19.Focus();
                    break;
                }
            case "TextBox19":
                {
                    CheckBox26.Focus();
                    break;
                }
            case "CheckBox26":
                {
                    CheckBox27.Focus();
                    break;
                }
            case "CheckBox27":
                {
                    TextBox28.Focus();
                    break;
                }
            case "TextBox28":
                {
                    CheckBox31.Focus();
                    break;
                }
            }
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
        protected void Fill_Texts()
        {

            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            Label3.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            Label4.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            Label5.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            Label6.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            Label7.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            Label8.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
            Label10.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 13).Trim();
            Label11.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 14).Trim();
            Label13.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 16).Trim();
            Label16.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 19).Trim();
            Label17.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 20).Trim();
            Label18.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 21).Trim();
            Label19.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 22).Trim();
            Label20.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 23).Trim();
            Label21.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 24).Trim();
            Label24.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 28).Trim();
            Label26.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 30).Trim();
            Label27.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 31).Trim();
            Label28.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 32).Trim();
            Label29.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 33).Trim();
            Label30.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 34).Trim();
            Label31.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 37).Trim();
            Label32.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 38).Trim();
            Label33.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 35).Trim();
            Label34.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 36).Trim();
            Label35.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 41).Trim();
            Label41.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 39).Trim(); 
            Label42.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 40).Trim();
            Button2.Text = Client.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 6).Trim();
            Button3.Text = Client.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button5.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 42).Trim();
            Button4.Text = Client.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 43).Trim();
            Client.Close();
        }
        protected void Fill_Data()
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_CS_WCF_Service.PlayerBasicData Player = new TSGS_CS_WCF_Service.PlayerBasicData();
            Player = Client_WCF.GetPlayerFullData((int)Session["PlayerToUpdate"], (int)Session["Competition_identification"]);
   	        Label33a.Text = Convert.ToString(Player.Speler_ID);
            Label34a.Text = Convert.ToString(Player.Competitie_Id); 
	        TextBox3.Text = Player.Achternaam;
	        TextBox4.Text = Player.Tussenvoegsel;
	        TextBox5.Text = Player.Voorletters; 
	        TextBox6.Text = Player.Roepnaam; 
	        TextBox7.Text = Convert.ToString(Player.KNSBnummer); 
	        TextBox1.Text = Convert.ToString(Player.FIDEnummer);
            TextBox2.Text = Player.FIDErating.ToString("#000.0", System.Globalization.CultureInfo.InvariantCulture);
            TextBox8.Text = Player.Telefoonnummer;
            TextBox10.Text = Player.Startrating.ToString("#000.0", System.Globalization.CultureInfo.InvariantCulture);
            TextBox11.Text = Player.Rating.ToString("#000.0", System.Globalization.CultureInfo.InvariantCulture);
            TextBox13.Text = Player.Snelschaakrating.ToString("#000.0", System.Globalization.CultureInfo.InvariantCulture);
	        TextBox16.Text = Convert.ToString(Player.Team);
            TextBox17.Text = Player.Titel;
            TextBox18.Text = Player.KNSBrating.ToString("#000.0", System.Globalization.CultureInfo.InvariantCulture);
            CheckBox17.Checked = (Player.Clublid == 1);
            CheckBox18.Checked = (Player.Deelnemer_teruggetrokken == 1); 
	        TextBox19.Text = Convert.ToString(Player.Speelt_mee_sinds_ronde); 
//	        CheckBox20.Checked = (Player.Doet_mee_met_snelschaak == 1); 
//	        TextBox21.Text = Convert.ToString(Player.Speelt_blitz_sinds_ronde);
            TextBox24.Text = Player.Rapidrating.ToString("#000.0", System.Globalization.CultureInfo.InvariantCulture);
	        CheckBox26.Checked = (Player.Vrijgeloot == 1); 
	        CheckBox27.Checked = (Player.Wants_Email == 1); 
	        TextBox28.Text = Player.Email_Address;
            CheckBox29.Checked = false; // (Player.Wants_SMS == 1); 
//	        TextBox30.Text = Player.Mobile_Number; 
            CheckBox31.Checked = (Player.Member_Premier_Group == 1);
            if (Player.ProfilePicture == null)
            {
                Player.ProfilePicture = Client_WCF.GetNoPictureFile(Player.Competitie_Id);
            }
            Session["OldImage"] = Player.ProfilePicture; 
            Image1.ImageUrl = Client_WCF.StringImage(Player.Speler_ID, Player.Competitie_Id);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            int CTY = 1;
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_CS_WCF_Service.PlayerBasicData Player = new TSGS_CS_WCF_Service.PlayerBasicData();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            Globals.FotoPath = Image1.ImageUrl;
            if (Check_Error())
            {
                Label11.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim();
                Label11.Visible = true;
            }
            else
            {
                Player.Speler_ID = Convert.ToInt16(Label33a.Text.Trim());
                Player.Titel = TextBox17.Text;
                Player.Competitie_Id = Convert.ToInt16(Session["Competition_Identification"]);
                CTY = Client_WCF.GetCompetitionType((int)Session["Competition_Identification"]);
                Player.Achternaam =  TextBox3.Text;
                Player.Tussenvoegsel = TextBox4.Text;
                Player.Voorletters = TextBox5.Text;
                Player.Roepnaam = TextBox6.Text;
                Player.KNSBnummer = Convert.ToInt32(TextBox7.Text);
                Player.FIDEnummer = Convert.ToInt32("0" + TextBox1.Text);
                Player.Telefoonnummer = TextBox8.Text;
                Player.Team = Convert.ToSByte("0" + TextBox16.Text);
                Player.Clublid = Convert.ToSByte(CheckBox17.Checked);
                Player.Deelnemer_teruggetrokken = Convert.ToSByte(CheckBox18.Checked);
                Player.Speelt_mee_sinds_ronde = Convert.ToSByte("0" + TextBox19.Text);
                Player.Doet_mee_met_snelschaak = Convert.ToSByte(CheckBox20.Checked);
                Player.Speelt_blitz_sinds_ronde = Convert.ToSByte("0" + TextBox21.Text);
                Player.Vrijgeloot = Convert.ToSByte(CheckBox26.Checked);
                Player.Wants_Email = Convert.ToSByte(CheckBox27.Checked);
                Player.Email_Address = TextBox28.Text;
                Player.Wants_SMS = Convert.ToSByte(CheckBox29.Checked);
                Player.Mobile_Number = TextBox30.Text;
                Player.Member_Premier_Group = Convert.ToSByte(CheckBox31.Checked);
                Player.ProfilePicture = (byte[])Session["OldImage"];

                Player.Rating = (float)Convert.ToDouble("0" + TextBox11.Text, System.Globalization.CultureInfo.InvariantCulture);
                Player.Rapidrating = (float)Convert.ToDouble("0" + TextBox24.Text, System.Globalization.CultureInfo.InvariantCulture);
                Player.Snelschaakrating = (float)Convert.ToDouble("0" + TextBox13.Text, System.Globalization.CultureInfo.InvariantCulture);
                Player.KNSBrating = (float)Convert.ToDouble("0" + TextBox18.Text, System.Globalization.CultureInfo.InvariantCulture);
                Player.FIDErating = (float)Convert.ToDouble("0" + TextBox2.Text, System.Globalization.CultureInfo.InvariantCulture);
                Player.Startrating = (float)Convert.ToDouble("0" + TextBox10.Text, System.Globalization.CultureInfo.InvariantCulture);

                Client_WCF.UpdatePlayer(Player, (int)Session["Competition_Identification"]);
                Client_WCF.Update_Workflow_Item("[Correcties]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
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

        protected void OnTextBoxMandatoryChanged(object sender, EventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Master.ErrorMessageVisibility(false);
            TextBox tb = (TextBox)sender;
            String Input_TB = "";
            int error = Client_WCF.ValidateString(tb.Text.Trim(), false, "-", ref Input_TB);
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

        protected void OnTextBoxEmptyAllowedChanged(object sender, EventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Master.ErrorMessageVisibility(false);
            TextBox tb = (TextBox)sender;
            String Input_TB = " ";
            int error = Client_WCF.ValidateString(tb.Text.Trim(), true, " ", ref Input_TB);
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

        
        protected void OnIntTextBoxChanged(object sender, EventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Master.ErrorMessageVisibility(false);
            TextBox tb = (TextBox)sender;
            int Input_TB = 0;
            int error = Client_WCF.ValidateInteger(tb.Text.Trim(), true, 0, false, 0, 0, ref Input_TB);
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

        protected bool Check_Error()
        {
            bool aux = false;
            return aux;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            if (FileUpload1.HasFile)
            {
                Globals.FotoPath = "workimages/Resized_" + FileUpload1.FileName;
                FileUpload1.PostedFile.SaveAs(Server.MapPath("~/workimages/" + FileUpload1.FileName));
                System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath("~/workimages/" + FileUpload1.FileName));
                int newwidthimg = 80;
                float AspectRatio = (float)image.Size.Width / (float)image.Size.Height;
                int newHeight = Convert.ToInt32(newwidthimg / AspectRatio);
                Bitmap thumbnailBitmap = new Bitmap(newwidthimg, newHeight);
                Graphics thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                var imageRectangle = new Rectangle(0, 0, newwidthimg, newHeight);
                thumbnailGraph.DrawImage(image, imageRectangle);
                thumbnailBitmap.Save(Server.MapPath("~/workimages/Resized_" + FileUpload1.FileName));
                Session["OldImage"] = (byte[])File.ReadAllBytes(Server.MapPath("~/workimages/Resized_" + FileUpload1.FileName)); 
                thumbnailGraph.Dispose();
                thumbnailBitmap.Dispose();
                image.Dispose();
                Image1.ImageUrl = "~/workimages/Resized_" + FileUpload1.FileName;
            }
            else
            {
                Globals.FotoPath = "images/notdone2.jpg";
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
            Session["OldImage"] = (byte[])File.ReadAllBytes(Server.MapPath(localPath));
            thumbnailGraph.Dispose();
            thumbnailBitmap.Dispose();
            imageOrg.Dispose();
            image.Dispose();
            Image1.ImageUrl = localPath;
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }
    }
}