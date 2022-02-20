using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Globalization;

namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Correct_Player_Results : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "CorrectResults";
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
                Label3.Visible = false;
                Label4.Visible = false;
                Label5.Visible = false;
                Label6.Visible = false;
                Label7.Visible = false;
                Label8.Visible = false;
                Label9.Visible = false;
                Label10.Visible = false;
                Label11.Visible = false;
                Label12.Visible = false;
                Label13.Visible = false;
                Label14.Visible = false;
                Result.Visible = false;
                Kleur.Visible = false;
                TextBox3.Visible = false;
                TextBox4.Visible = false;
                CheckBox1.Visible = false;
                CheckBox2.Visible = false;
                Fill_Texts();
            }
        }

        protected void Fill_Texts()
        {
            string auxText = "";
            string auxName = "";
            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            auxText = (string)Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
            auxName = (string)Client_WCF.GetPlayerName((int)Session["PlayerToUpdate"]);
            Label2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            Label1.Text = auxText + " " + auxName;
            Label6.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            Label7.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Label8.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            Label9.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            Label10.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            Label11.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            Label12.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            Label13.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 27).Trim();
            Label14.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 28).Trim();

            CheckBox1.Checked = true;

            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 12).Trim();

            DataSet dsR = Client_WCF.GetResults((int)Session["PlayerToUpdate"], (int)Session["Competition_Identification"], (int)Session["Language"]);
            GridView1.Font.Size = (int)Session["Fontsize"];
            ViewState["PlayerResults"] = dsR.Tables[0];
            GridView1.DataSource = dsR;
            GridView1.AllowPaging = true;

            GridView1.PageSize = 15;
            GridView1.DataBind();


            GridViewRow rowHeader = GridView1.HeaderRow;
            rowHeader.Cells[0].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            rowHeader.Cells[0].HorizontalAlign = HorizontalAlign.Right;
            rowHeader.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            rowHeader.Cells[1].HorizontalAlign = HorizontalAlign.Right;
            rowHeader.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            rowHeader.Cells[2].HorizontalAlign = HorizontalAlign.Left;
            rowHeader.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            rowHeader.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            rowHeader.Cells[4].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            rowHeader.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            rowHeader.Cells[5].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            rowHeader.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            rowHeader.Cells[6].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            rowHeader.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            rowHeader.Cells[7].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 28).Trim();
            rowHeader.Cells[7].HorizontalAlign = HorizontalAlign.Right;


            Client_MLC.Close();
            Client_WCF.Close();

            Fill_DDL();
        }

        protected void OnInputTextBoxChanged(object sender, EventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Master.ErrorMessageVisibility(false);
            TextBox tb = (TextBox)sender;
            double Input_TB = 0.0;
            int error = Client_WCF.ValidateReal(tb.Text.Trim(), true, 0.0, true, -100.0, 100.0, ref Input_TB);
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

        protected void Fill_DDL()
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 13).Trim()));
            Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 14).Trim()));
            Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim()));
            Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 16).Trim()));
            Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 17).Trim()));
            Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 18).Trim()));
            Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 19).Trim()));
            Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 20).Trim()));
            Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 21).Trim()));
            Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 22).Trim()));
            Result.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 23).Trim()));

            Kleur.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 24).Trim()));
            Kleur.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 25).Trim()));
            Kleur.Items.Add(new ListItem(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 26).Trim()));

            Client_MLC.Close();
        }

        protected void DDLR_SelectedChange(object sender, EventArgs e)
        {
            int SelectedIndex = GridView1.SelectedIndex;
            DataTable dtR = (DataTable)ViewState["PlayerResults"];
            dtR.Rows[SelectedIndex][8] = ResultFromListToResult (Result.SelectedIndex);
            ViewState["PlayerResults"] = (DataTable)dtR;
            Fill_Texts();
        }

        protected void DDLK_SelectedChange(object sender, EventArgs e)
        {

        }

        protected int ResultFromListToResult(int ResultFromList)
        {
            switch (ResultFromList)
            {
                case 0: return 0;
                case 1: return 1;
                case 2: return 2;
                case 3: return 3;
                case 4: return 5;
                case 5: return 6;
                case 6: return 7;
                case 7: return 8;
                case 8: return 9;
                case 9: return 10;
                case 10: return 11;
            }
            return 0;
        }

        protected int ResultToResultFromList(int Result)
        {
            switch (Result)
            {
                case 0: return 0;
                case 1: return 1;
                case 2: return 2;
                case 3: return 3;
                case 5: return 4;
                case 6: return 5;
                case 7: return 6;
                case 8: return 7;
                case 9: return 8;
                case 10: return 9;
                case 11: return 10;
            }
            return 0;
        }

        protected int ComplementResult (int Resultcode)
        {
            switch (Resultcode)
            {
                case 0: return 0;
                case 1: return 3;
                case 2: return 2;
                case 3: return 1;
                case 4: return 4;
                case 5: return -1;
                case 6: return -1;
                case 7: return -1;
                case 8: return -1;
                case 9: return 11;
                case 10: return 10;
                case 11: return 9;
                case 12: return -1;
                case 13: return -1;
                case 14: return -1;
                case 15: return 15;
            }
            return -1;
        }


        protected float ResultCodeToMatchPoints(int Resultcode)
        {
            switch (Resultcode)
            {
                case 0: return Convert.ToSingle(0.0);
                case 1: return Convert.ToSingle(1.0);
                case 2: return Convert.ToSingle(0.5);
                case 3: return Convert.ToSingle(0.0);
                case 4: return Convert.ToSingle(0.0);
                case 5: return Convert.ToSingle(0.0);
                case 6: return Convert.ToSingle(0.0);
                case 7: return Convert.ToSingle(0.0);
                case 8: return Convert.ToSingle(0.0);
                case 9: return Convert.ToSingle(1.0);
                case 10: return Convert.ToSingle(0.5);
                case 11: return Convert.ToSingle(0.0);
                case 12: return Convert.ToSingle(0.0);
                case 13: return Convert.ToSingle(0.0);
                case 14: return Convert.ToSingle(0.0);
                case 15: return Convert.ToSingle(0.0);
            }
            return -1;
        }


        protected int KleurToKleurFromList (int KleurCode)
        {
            switch (KleurCode)
            {
                case -1: return 2;
                case 0: return 1;
                case 1: return 0;
            }
            return 0;
        }


        protected int KleurFromListToKleur(int DDLSelected)
        {
            switch (DDLSelected)
            {
                case 2: return -1;
                case 0: return 1;
                case 1: return 0;
            }
            return 0;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_CS_WCF_Service.ResultData ORD = new TSGS_CS_WCF_Service.ResultData();
            int SelectedIndex = GridView1.SelectedIndex;
            DataTable dtR = (DataTable)ViewState["PlayerResults"];
            DataRow row = dtR.Rows[SelectedIndex];
            int Championshipgame_Number = 0;
            int First_Player = (int)Session["PlayerToUpdate"];
            int Second_Player = (int)row[9];
            bool First_is_CG = Client_WCF.PlayerIsCG(First_Player);
            bool Second_is_CG =  Client_WCF.PlayerIsCG(Second_Player);
            CheckBox1.Visible = First_is_CG && Second_is_CG;
            if (CheckBox2.Checked)
            {
                CheckBox1.Checked = true;
                Championshipgame_Number = Client_WCF.ChampionsGroupGameNumber(First_Player, Second_Player, (int)Session["Competition_Identification"]);
            }

            ORD.Competitie_Id = (int)Session["Competition_Identification"];
            ORD.Rondernr = Convert.ToInt16(row[0]);
            ORD.Deelnemer_ID = (int)Session["PlayerToUpdate"];
            ORD.Kleur = KleurFromListToKleur(Kleur.SelectedIndex);
            ORD.Resultaat = ResultFromListToResult(Result.SelectedIndex);
            if (TextBox3.Text == "") TextBox3.Text = "0";
            ORD.ELO_Resultaat = Convert.ToSingle(TextBox3.Text.Trim(), CultureInfo.InvariantCulture);
            if (TextBox4.Text == "") TextBox4.Text = "0";
            ORD.Competitie_Resultaat = Convert.ToSingle(TextBox4.Text.Trim(), CultureInfo.InvariantCulture);
            ORD.ChampionsgroupGameNumber = Championshipgame_Number;
            ORD.Tegenstander = Convert.ToInt16(row[9]);
            ORD.Matchpunten = ResultCodeToMatchPoints(ORD.Resultaat);

            Client_WCF.UpdateOneResult(ORD);

            if (CheckBox1.Checked && (ComplementResult((int)ORD.Resultaat) != -1))
            {
                TSGS_CS_WCF_Service.ResultData ORA = new TSGS_CS_WCF_Service.ResultData();
                ORA.Competitie_Id = (int)Session["Competition_Identification"];
                ORA.Rondernr = Convert.ToInt16(row[0]);
                ORA.Deelnemer_ID = Convert.ToInt16(row[9]);
                ORA.Kleur = 0 - ORD.Kleur;
                ORA.Resultaat = ComplementResult(ORD.Resultaat);
                ORA.ELO_Resultaat = 0 - ORD.ELO_Resultaat;
                ORA.Competitie_Resultaat = 0 -ORD.Competitie_Resultaat;
                ORA.ChampionsgroupGameNumber = Championshipgame_Number;
                ORA.Matchpunten = ResultCodeToMatchPoints(ORA.Resultaat);
                ORA.Tegenstander = ORD.Deelnemer_ID;
                Client_WCF.UpdateOneResult(ORA);
            } 

            Client_WCF.Close();
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
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
                            button.Attributes.Add("onmouseout",  "return UnselectIcon(this);");
                        }
                    }
                }

            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedIndex = GridView1.SelectedIndex;
            DataTable dtR = (DataTable)ViewState["PlayerResults"];
            DataRow row = dtR.Rows[SelectedIndex];
            Label3.Visible = true;
            Label4.Visible = true;
            Label5.Visible = true;
            Label6.Visible = true;
            Label7.Visible = true;
            Label8.Visible = true;
            Label9.Visible = true;
            Label10.Visible = true;
            Label11.Visible = true;
            Label12.Visible = true;
            Label13.Visible = true;
            CheckBox1.Visible = true;

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            int First_Player = (int)Session["PlayerToUpdate"];
            int Second_Player = (int)row[9];
            bool First_is_CG = Client_WCF.PlayerIsCG(First_Player);
            bool Second_is_CG =  Client_WCF.PlayerIsCG(Second_Player);
            CheckBox2.Visible = First_is_CG && Second_is_CG;
            Label14.Visible = CheckBox2.Visible;

            Kleur.SelectedIndex = KleurToKleurFromList ((int)row[7]);
            Kleur.Visible = true;
            Result.SelectedIndex = ResultToResultFromList((int)row[8]);
            Result.Visible = true;
            TextBox3.Visible = true;
            TextBox4.Visible = true;
            Label3.Text = row[0].ToString();
            Label4.Text = row[1].ToString();
            Label5.Text = row[2].ToString();
            TextBox3.Text = String.Format(CultureInfo.InvariantCulture, "{0:F2}", row[5]);
            TextBox4.Text = String.Format(CultureInfo.InvariantCulture, "{0:F2}", row[6]);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }


        public float F0 { get; set; }
    }
}