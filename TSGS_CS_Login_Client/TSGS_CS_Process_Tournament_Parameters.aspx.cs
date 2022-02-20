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

    public partial class TSGS_CS_Process_Tournament_Parameters : System.Web.UI.Page
    {
        protected static List<int> State_Ref = new List<int>();
        protected static List<int> Comp_Ref = new List<int>();
        protected static List<int> Integrate_Ref = new List<int>();
        protected static List<int> BasedOn_Ref = new List<int>();

        protected static bool Update_In_Postback_Required = false;

        protected void Page_Load(object sender, EventArgs e)
        {

            Session["Functionality"] = "Params";
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
                FileUpload1.Attributes.Add("onchange", "UploadFile(this);");

                Master.SetErrorMessageGreen();
                Master.ErrorMessageVisibility(false);
                Label1.Visible = false;
                /*
                other initialising stuff for function <xxxx>
                */

                Fill_Texts();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Update_In_Postback_Required)
            {
                Fill_Texts();
                Update_In_Postback_Required = false;
            }
        }

        protected void Fill_Texts()
        {

            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            Label1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 31).Trim();
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 30).Trim();
            Label3.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            Label4.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Label5.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            Label6.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            Label7.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            Label8.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            Label9.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            Label10.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            Label11.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
            Label12.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim();
            Label13.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 13).Trim();
            Label14.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 14).Trim();
            Label15.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim();
            Label16.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 16).Trim();
            Label17.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 17).Trim();
            Label18.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 18).Trim();
            Label19.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 19).Trim();
            Label21.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 21).Trim();
            Label22.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 22).Trim();
            Label23.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 23).Trim();
            Label25.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 25).Trim();
            Label26.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 26).Trim();
            Label27.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 27).Trim();
            Label28.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 28).Trim();
            Label29.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 29).Trim();
            Label30.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 32).Trim();
            Label31.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 33).Trim();
            Label32.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 35).Trim();
            Label33.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 36).Trim();
            Label34.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 38).Trim();
            Label35.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 37).Trim(); 
            CheckBox1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 34).Trim();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            State_Ref.Clear();
            Comp_Ref.Clear();
            Integrate_Ref.Clear();
            DDLStatus.Items.Clear();
            DDLCompetitionType.Items.Clear();
            DDLToernooien.Items.Clear();
            DDLBasedOn.Items.Clear();

            DDLStatus.Items.Add("--");
            DDLCompetitionType.Items.Add("--");
            DDLToernooien.Items.Add("--");
            DDLBasedOn.Items.Add("--");
            State_Ref.Add(0);
            Comp_Ref.Add(0);
            Integrate_Ref.Add(0);
            BasedOn_Ref.Add(0);

            DataSet dsS = Client_WCF.GetStateDescriptions((int)Session["Language"]);
            foreach (DataRow dgItem in dsS.Tables[0].Rows)
            {
                DDLStatus.Items.Add(new ListItem((string)dgItem["State_Description"]).Text.Trim());
                State_Ref.Add(Convert.ToInt16(dgItem[0]));
            }

            DataSet dsC = Client_WCF.GetCompetitionTypes((int)Session["Language"]);
            foreach (DataRow dgItem in dsC.Tables[0].Rows)
            {
                DDLCompetitionType.Items.Add(new ListItem((string)dgItem["Omschrijving"]).Text.Trim());
                Comp_Ref.Add(Convert.ToInt16(dgItem[0]));
            }


            DataSet dsCL = Client_WCF.GetCompetitionList((string)Session["Manager"], (int)Session["Manager_Id"]);
            foreach (DataRow dgItem in dsCL.Tables[0].Rows)
            {
                DDLBasedOn.Items.Add(new ListItem((string)dgItem["Naam_competitie"]).Text.Trim());
                BasedOn_Ref.Add(Convert.ToInt16(dgItem[0])); 
                if ((int)dgItem["Competitie_Type"] == 4)
                {
                    DDLToernooien.Items.Add(new ListItem((string)dgItem["Naam_competitie"]).Text.Trim());
                    Integrate_Ref.Add(Convert.ToInt16(dgItem[0]));
                }
            }
            
            TSGS_CS_WCF_Service.GeneralInfo gi = new TSGS_CS_WCF_Service.GeneralInfo();
            if ((string)Session["CallingFunction"] == "NewEntry")
            {
                if ((int)Session["Competition_Identification"] == 0)
                {
                    gi = Client_WCF.GetGeneralInfo((int)Session["Competition_Selection"]);
                    TextBox4.Text = "* " + gi.Naam_competitie + " *";
                    TextBox5.Text = DateTime.Today.ToString();
                    TextBox12.Text = 0.ToString();
                    TextBox29.Text = 6.ToString();

                }
                else
                {
                    // new entry made, but without any players in the competition
                    Response.Redirect("TSGS_CS_Setup_New_Competition.aspx");
                }
            }
            else
            {
                
                gi = Client_WCF.GetGeneralInfo((int)Session["Competition_Identification"]);
                TextBox4.Text = gi.Naam_competitie;
                TextBox5.Text = gi.Aanmaakdatum.ToString();
                TextBox29.Text = gi.CurrentState.ToString();
                TextBox12.Text = gi.Laatste_ronde.ToString();
                DDLBasedOn.Visible = false;
                Label33.Visible = true;

            }
            TextBox3.Text = gi.Vereniging;
            TextBox5.ReadOnly = true;
            TextBox5.BackColor = System.Drawing.Color.LightGray;
            TextBox6.Text = gi.Aantal_groepen.ToString();
            TextBox7.Text = gi.Bonus_externe_wedstrijden.ToString();
            TextBox8.Text = gi.Vrij_afmelden.ToString();
            TextBox9.Text = gi.Strafpunten_afmelden.ToString();
            TextBox10.Text = gi.Strafpunten_wegblijven.ToString();
            TextBox11.Text = gi.Aantal_ronden.ToString();
            TextBox13.Text = gi.KFactor.ToString();
            TextBox14.Text = gi.Standaarddeviatie.ToString();
            TextBox15.Text = gi.Aantal_Unieke_Ronden.ToString();
            TextBox16.Text = gi.Intern_Basis;
            TextBox17.Text = gi.Intern_Template;
            TextBox18.Text = gi.Intern_Images;
            TextBox19.Text = gi.Intern_Competitie;
            TextBox21.Text = gi.Website_Basis;
            TextBox22.Text = gi.Website_Template;
            TextBox23.Text = gi.Website_Competitie;
            TextBox25.Text = gi.Client_FTP_Host;
            TextBox26.Text = gi.Client_FTP_UN;
            TextBox27.Text = gi.Client_FTP_PW;
            TextBox28.Text = gi.Competitie_Type.ToString();
            TextBox34.Text = gi.Acceleration.ToString();
            TextBox35.Text = gi.Seizoen.ToString();

            TextBox30.Text = gi.IntegrateWith.ToString(); 
            CheckBox1.Checked = ((int)Session["Competition_Identification"] != Convert.ToInt16(TextBox30.Text));
            
            if (gi.Competitie_Type == 1)
            {
                CheckBox1.Visible = true;
                DDLToernooien.Visible = CheckBox1.Checked;
            }
            else
            {
                CheckBox1.Visible = false;
                DDLToernooien.Visible = false;
            }

            int locate_index = 0;
            foreach (int element in State_Ref)
            {
                if (element == Convert.ToInt16(TextBox29.Text))
                {
                    DDLStatus.SelectedIndex = locate_index;
                }
                locate_index++;
            }
            DDLCompetitionType.SelectedIndex = Comp_Ref[gi.Competitie_Type];

            locate_index = 0;
            foreach (int element in Integrate_Ref)
            {
                if (element == gi.IntegrateWith)
                {
                    DDLToernooien.SelectedIndex = locate_index;
                }
                locate_index++;
            } 

            if (gi.ProfilePicture == null)
            {
                byte[] file = File.ReadAllBytes(Server.MapPath("~/images/nofoto.png"));
                Image2.ImageUrl = Client_WCF.MakeImageSourceData((byte[])file);
                File.WriteAllBytes(Server.MapPath("~/workimages/tempinputpicture.png"), file);
            }
            else
            {
                Image2.ImageUrl = Client_WCF.MakeImageSourceData((byte[])gi.ProfilePicture);
                File.WriteAllBytes(Server.MapPath("~/workimages/tempinputpicture.png"), gi.ProfilePicture);
            }
            Globals.FotoPath = "workimages/tempinputpicture.png";
   
            Client_WCF.Close();
            Client_MLC.Close();
        }

        protected void OnIntTextBoxChanged(object sender, EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            TextBox tb = (TextBox)sender;
            int Int = 0;
            int error = Client_WCF.ValidateInteger(tb.Text.Trim(), false, 0, false, -99999, 99999, ref Int);
            if (error == 0)
            {
                Master.ErrorMessageVisibility(false);
                tb.BackColor = System.Drawing.Color.White;
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


        protected void DDLBasedOnChange(object sender, EventArgs e)
        {
            Session["Competition_Selection"] = BasedOn_Ref[DDLBasedOn.SelectedIndex];
            Update_In_Postback_Required = true;
        }

        protected void DDL_CT_SelectedChange(object sender, EventArgs e)
        {

            TextBox28.Text = Comp_Ref[DDLCompetitionType.SelectedIndex].ToString();
        }

        protected void DDL_SelectedChange(object sender, EventArgs e)
        {

            TextBox29.Text = State_Ref[DDLStatus.SelectedIndex].ToString();
        }
        protected void DDL_SelectToernooiChange(object sender, EventArgs e)
        {

            TextBox30.Text = Integrate_Ref[DDLToernooien.SelectedIndex].ToString();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
/*
trigger actions for main function of form
*/
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_CS_WCF_Service.GeneralInfo gi = new TSGS_CS_WCF_Service.GeneralInfo();
            gi.Competitie_Id = (int)Session["Competition_Identification"];
            gi.Vereniging = TextBox3.Text.Trim();
            gi.Naam_competitie = TextBox4.Text.Trim();
            gi.Aanmaakdatum = Convert.ToDateTime(TextBox5.Text);
            gi.Aantal_groepen = Convert.ToInt16(TextBox6.Text);
            gi.Bonus_externe_wedstrijden = Convert.ToInt16(TextBox7.Text);;
            gi.Vrij_afmelden = Convert.ToInt16(TextBox8.Text);
            gi.Strafpunten_afmelden  = Convert.ToInt16(TextBox9.Text);
            gi.Strafpunten_wegblijven  = Convert.ToInt16(TextBox10.Text);
            gi.Aantal_ronden = Convert.ToInt16(TextBox11.Text);
            gi.Laatste_ronde = Convert.ToInt16(TextBox12.Text);
            gi.KFactor = Convert.ToInt16(TextBox13.Text);
            gi.Standaarddeviatie = Convert.ToInt16(TextBox14.Text);
            gi.Aantal_Unieke_Ronden = Convert.ToInt16(TextBox15.Text);
            gi.Intern_Basis = TextBox16.Text.Trim();
            gi.Intern_Template = TextBox17.Text.Trim();
            gi.Intern_Images = TextBox18.Text.Trim();
            gi.Intern_Competitie = TextBox19.Text.Trim();
            gi.Intern_Competitie_Images = "";
            gi.Website_Basis = TextBox21.Text.Trim();
            gi.Website_Template = TextBox22.Text.Trim();
            gi.Website_Competitie = TextBox23.Text.Trim();
            gi.Website_Competitie_Images = "";
            gi.Client_FTP_Host = TextBox25.Text.Trim();
            gi.Client_FTP_UN = TextBox26.Text.Trim();
            gi.Client_FTP_PW = TextBox27.Text.Trim();
            gi.Competitie_Type = Convert.ToInt16(TextBox28.Text);
            gi.CurrentState = Convert.ToInt16(TextBox29.Text);
            gi.IntegrateWith = Convert.ToInt16(TextBox30.Text);
            gi.Acceleration = Convert.ToSingle(TextBox34.Text);
            gi.Seizoen = Convert.ToInt16(TextBox35.Text);
            gi.PartijenPerRonde = 1;

            System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath("~/"+Globals.FotoPath));
            Bitmap thumbnailBitmap = new Bitmap(image.Size.Width, image.Size.Height);
            Graphics thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            var imageRectangle = new Rectangle(0, 0, image.Size.Width, image.Size.Height);
            thumbnailGraph.DrawImage(image, imageRectangle);
            MemoryStream ms = new MemoryStream();
            thumbnailBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] localProfilePicture = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(localProfilePicture, 0, localProfilePicture.Length);

            gi.ProfilePicture = localProfilePicture;

            int CID = Client_WCF.UpdateGeneralInfo(gi, (int)Session["Manager_Id"]);
            Session["Current_Status"] = gi.CurrentState;
            Client_WCF.Close();
            Label1.Visible = true;
            string RootPath = Server.MapPath("~/htmlfiles/" + CID.ToString().Trim());
            if ((string)Session["CallingFunction"] == "AdaptEntry")
            {
                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }
            else
            {
                Session["Competition_Identification"] = CID;
                Session["Club"] = gi.Vereniging;
                Session["Competition"] = gi.Naam_competitie;
                if (!System.IO.Directory.Exists(RootPath))
                {
                    System.IO.Directory.CreateDirectory(RootPath);
                }
                Response.Redirect("TSGS_CS_Setup_New_Competition.aspx");
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
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
                thumbnailGraph.Dispose();
                thumbnailBitmap.Dispose();
                image.Dispose();
                Image2.ImageUrl = "~/workimages/Resized_" + FileUpload1.FileName;
            }
            else
            {
                Globals.FotoPath = "images/nofoto.png";
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


        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

        protected void OnIntegrateChanged(object sender, EventArgs e)
        {
            DDLToernooien.Visible = CheckBox1.Checked;            
        }

    }
}