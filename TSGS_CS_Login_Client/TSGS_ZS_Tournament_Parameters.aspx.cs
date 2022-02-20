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

    public partial class TSGS_ZS_Tournament_Registration : System.Web.UI.Page
    {
        protected static List<int> State_Ref = new List<int>();

        protected void Page_Load(object sender, EventArgs e)
        {
            string Class_Name = this.GetType().Name;
            int CutOff_Length = System.Math.Min(20, Class_Name.Length - 8);
            Session["Functionality"] = Class_Name.Substring(8, CutOff_Length);

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
                FileUpload2.Attributes.Add("onchange", "UploadFile(this);");

                Master.SetErrorMessageGreen();
                Master.ErrorMessageVisibility(false);
                Label1.Visible = false;
                /*
                other initialising stuff for function <xxxx>
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

            Label1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 31).Trim();
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 30).Trim();
            Toernooi_Id.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            Aanmaak_Datum.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Toernooi_Naam.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            Hoofdsponsor.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            Subsponsors.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            Aantal_Ronden.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            Aantal_Partijen_Per_Uitslag.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            Indelings_Modus.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            Aantal_Rating_Groepen.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
            Aantal_Invoerpunten.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim();
            Decentrale_Invoer_Spread.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 13).Trim();
            Decentrale_Invoer_Maximaal.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 14).Trim();
            KFactor.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim();
            Restrictie_Ronden.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 16).Trim();
            Restrictie_Rating_Grens.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 17).Trim();
            Website_Template.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 18).Trim();
            Website_Competitie.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 19).Trim();
            Client_FTP_Host.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 20).Trim();
            Client_FTP_UN.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 21).Trim();
            Client_FTP_PW.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 22).Trim();
            Current_State.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 23).Trim();
            ProfilePicture.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 24).Trim();
            Toernooi_Logo.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 25).Trim();
            Laatste_ronde.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 26).Trim();
            Website_Basis.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 27).Trim();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            State_Ref.Clear();
            DDLStatus.Items.Clear();

            DataSet dsS = Client_WCF.GetStateDescriptions((int)Session["Language"]);
            foreach (DataRow dgItem in dsS.Tables[0].Rows)
            {
                DDLStatus.Items.Add(new ListItem((string)dgItem["State_Description"]).Text.Trim());
                State_Ref.Add(Convert.ToInt16(dgItem[0]));
            }

            TSGS_CS_WCF_Service.General_Swiss_Info gsi = new TSGS_CS_WCF_Service.General_Swiss_Info();
            if ((string)Session["CallingFunction"] == "NewEntry")
            {
                if ((int)Session["Competition_Identification"] == 0)
                {
                    gsi = Client_WCF.GetGeneralSwissInfo(0);
                }
                else
                {
                    // new entry made, but without any players in the competition
                    Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
                }
            }
            else
            {
                gsi = Client_WCF.GetGeneralSwissInfo((int)Session["Competition_Identification"]);
            }

            TextBox3.Text = gsi.Toernooi_Id.ToString();
            TextBox3.ReadOnly = true;
            TextBox3.BackColor = System.Drawing.Color.LightGray; 
            TextBox4.Text = gsi.Aanmaakdatum.ToString(); 
            TextBox4.ReadOnly = true;
            TextBox4.BackColor = System.Drawing.Color.LightGray;
            TextBox5.Text = gsi.Toernooi_Naam; 
            TextBox6.Text = gsi.Hoofdsponsor.ToString();
            TextBox7.Text = gsi.Subsponsors.ToString();
            TextBox8.Text = gsi.Aantal_Ronden.ToString();
            TextBox9.Text = gsi.Aantal_Partijen_Per_Uitslag.ToString();
            TextBox10.Text = gsi.Indelings_Modus.ToString();
            TextBox11.Text = gsi.Aantal_Rating_Groepen.ToString();
            TextBox12.Text = gsi.Aantal_Invoerpunten.ToString();
            TextBox13.Text = gsi.Decentrale_Invoer_Spread.ToString();
            TextBox14.Text = gsi.Decentrale_Invoer_Maximaal.ToString();
            TextBox15.Text = gsi.KFactor.ToString();
            TextBox16.Text = gsi.Restrictie_Ronden.ToString();
            TextBox17.Text = gsi.Restrictie_Rating_Grens.ToString();
            TextBox18.Text = gsi.Website_Template;
            TextBox19.Text = gsi.Website_Competitie;
            TextBox20.Text = gsi.Client_FTP_Host;
            TextBox21.Text = gsi.Client_FTP_UN;
            TextBox22.Text = gsi.Client_FTP_PW;
            TextBox23.Text = gsi.CurrentState.ToString();
            TextBox24.Text = gsi.Laatste_Ronde.ToString();
            TextBox25.Text = gsi.Website_Basis;

            int locate_index = 0;
            foreach (int element in State_Ref)
            {
                if (element == gsi.CurrentState)
                {
                    DDLStatus.SelectedIndex = locate_index;
                }
                locate_index++;
            }

            if (gsi.ProfilePicture == null)
            {
                byte[] file1 = File.ReadAllBytes(Server.MapPath("~/images/nofoto.png"));
                Image2.ImageUrl = Client_WCF.MakeImageSourceData((byte[])file1);
                File.WriteAllBytes(Server.MapPath("~/workimages/tempinputpicture.png"), file1);
            }
            else
            {
                Image2.ImageUrl = Client_WCF.MakeImageSourceData((byte[])gsi.ProfilePicture);
                File.WriteAllBytes(Server.MapPath("~/workimages/tempinputpicture.png"), gsi.ProfilePicture);
            }
            if (gsi.Toernooi_Logo == null)
            {
                byte[] file2 = File.ReadAllBytes(Server.MapPath("~/images/TSGSLogo.png"));
                Image1.ImageUrl = Client_WCF.MakeImageSourceData((byte[])file2);
                File.WriteAllBytes(Server.MapPath("~/workimages/temptoernooilogo.png"), file2);
            }
            else
            {
                Image1.ImageUrl = Client_WCF.MakeImageSourceData((byte[])gsi.Toernooi_Logo);
                File.WriteAllBytes(Server.MapPath("~/workimages/temptoernooilogo.png"), gsi.Toernooi_Logo);
            } 
            Globals.FotoPath = "workimages/tempinputpicture.png";
            Globals.Toernooi_Logo_Path = "workimages/temptoernooilogo.png";
   
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

        protected void DDL_SelectedChange(object sender, EventArgs e)
        {

            TextBox23.Text = State_Ref[DDLStatus.SelectedIndex].ToString();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
/*
trigger actions for main function of form
*/
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_CS_WCF_Service.General_Swiss_Info gsi = new TSGS_CS_WCF_Service.General_Swiss_Info();
            gsi.Toernooi_Id = (int)Session["Competition_Identification"];
            gsi.Aanmaakdatum = Convert.ToDateTime(TextBox4.Text);
            gsi.Toernooi_Naam = TextBox5.Text.Trim();
            gsi.Hoofdsponsor = TextBox6.Text.Trim();
            gsi.Subsponsors = TextBox7.Text.Trim();
            gsi.Aantal_Ronden = Convert.ToInt16(TextBox8.Text);;
            gsi.Aantal_Partijen_Per_Uitslag = Convert.ToInt16(TextBox9.Text);
            gsi.Indelings_Modus  = Convert.ToInt16(TextBox10.Text);
            gsi.Aantal_Rating_Groepen  = Convert.ToInt16(TextBox11.Text);
            gsi.Aantal_Ronden = Convert.ToInt16(TextBox11.Text);
            gsi.Aantal_Invoerpunten = Convert.ToInt16(TextBox12.Text);
            gsi.Decentrale_Invoer_Spread = Convert.ToInt16(TextBox13.Text);
            gsi.Decentrale_Invoer_Maximaal = Convert.ToInt16(TextBox14.Text);
            gsi.KFactor = Convert.ToInt16(TextBox15.Text);
            gsi.Restrictie_Ronden = Convert.ToInt16(TextBox16.Text);
            gsi.Restrictie_Rating_Grens = Convert.ToInt16(TextBox17.Text);
            gsi.Website_Template = TextBox18.Text.Trim();
            gsi.Website_Competitie = TextBox19.Text.Trim();
            gsi.Client_FTP_Host = TextBox20.Text.Trim();
            gsi.Client_FTP_UN = TextBox21.Text.Trim();
            gsi.Client_FTP_PW = TextBox22.Text.Trim();
            gsi.CurrentState = Convert.ToInt16(TextBox23.Text);
            gsi.Laatste_Ronde = Convert.ToInt16(TextBox24.Text);
            gsi.Website_Basis = TextBox25.Text.Trim();
            gsi.Competitie_Type = 3;

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

            gsi.ProfilePicture = localProfilePicture;

            System.Drawing.Image imageLogo = System.Drawing.Image.FromFile(Server.MapPath("~/" + Globals.Toernooi_Logo_Path));
            Bitmap thumbnailBitmap2 = new Bitmap(imageLogo.Size.Width, imageLogo.Size.Height);
            Graphics thumbnailGraph2 = Graphics.FromImage(thumbnailBitmap2);
            var imageRectangle2 = new Rectangle(0, 0, imageLogo.Size.Width, imageLogo.Size.Height);
            thumbnailGraph2.DrawImage(imageLogo, imageRectangle2);
            MemoryStream ms2 = new MemoryStream();
            thumbnailBitmap2.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] localToernooiLogo = new byte[ms2.Length];
            ms2.Position = 0;
            ms2.Read(localToernooiLogo, 0, localToernooiLogo.Length);

            gsi.Toernooi_Logo = localToernooiLogo;

            int CID = Client_WCF.UpdateGeneralSwissInfo(gsi, (int)Session["Manager_Id"]);
           
            Client_WCF.Close();
            Label1.Visible = true;

            if ((string)Session["CallingFunction"] == "AdaptEntry")
            {
                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }
            else
            {
                Session["Competition_Identification"] = CID;
                Session["Club"] = gsi.Hoofdsponsor;
                Session["Competition"] = gsi.Toernooi_Naam;
                if (!System.IO.Directory.Exists(@"~\htmlfiles\"+CID.ToString().Trim()))
                {
                    System.IO.Directory.CreateDirectory(@"~\htmlfiles\"+CID.ToString().Trim());
                }
                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

            if (FileUpload1.HasFile)
            {
                Globals.Toernooi_Logo_Path = "workimages/Resized_" + FileUpload1.FileName;
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
                Globals.Toernooi_Logo_Path = "images/TSGSLogo.png";
            }

        }

        protected void Button4_Click(object sender, EventArgs e)
        {

            if (FileUpload2.HasFile)
            {
                Globals.FotoPath = "workimages/Resized_" + FileUpload2.FileName;
                FileUpload2.PostedFile.SaveAs(Server.MapPath("~/workimages/" + FileUpload2.FileName));

                System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath("~/workimages/" + FileUpload2.FileName));
                int newwidthimg = 80;
                float AspectRatio = (float)image.Size.Width / (float)image.Size.Height;
                int newHeight = Convert.ToInt32(newwidthimg / AspectRatio);
                Bitmap thumbnailBitmap = new Bitmap(newwidthimg, newHeight);
                Graphics thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                var imageRectangle = new Rectangle(0, 0, newwidthimg, newHeight);
                thumbnailGraph.DrawImage(image, imageRectangle);
                thumbnailBitmap.Save(Server.MapPath("~/workimages/Resized_" + FileUpload2.FileName));
                thumbnailGraph.Dispose();
                thumbnailBitmap.Dispose();
                image.Dispose();
                Image1.ImageUrl = "~/workimages/Resized_" + FileUpload2.FileName;
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
            while (File.Exists(Server.MapPath(Globals.Toernooi_Logo_Path)))
            {
                versionnr++;
                version = "V" + versionnr.ToString().Trim();
                Globals.Toernooi_Logo_Path = "~/workimages/" + version + "templogobig.jpg";
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
            string localPath = "~/workimages/" + version + "templogosmall.jpg";
            while (File.Exists(Server.MapPath(localPath)))
            {
                versionnr++;
                version = "V" + versionnr.ToString().Trim();
                localPath = "~/workimages/" + version + "templogosmall.jpg";
            }
            thumbnailBitmap.Save(Server.MapPath(localPath));
            thumbnailGraph.Dispose();
            thumbnailBitmap.Dispose();
            imageOrg.Dispose();
            image.Dispose();
            Image1.ImageUrl = localPath;
        }

        protected void RotateButton2_Click(object sender, ImageClickEventArgs e)
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

    }
}