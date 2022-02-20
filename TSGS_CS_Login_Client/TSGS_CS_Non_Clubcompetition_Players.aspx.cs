using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace TSGS_CS_Login_Client
{
    public partial class TSGS_CS_Non_Clubcompetition_Players : System.Web.UI.Page
    {

        int Ronde_nr;
        int Aantal_Afwezig = 0;
        int Aantal_Extern;
        DataSet ds_all = new DataSet();
        DataSet ds_absentees = new DataSet();
 
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "AttendanceRegister";
            Session["LastFunction"] = HttpContext.Current.Request.Url.AbsolutePath;

            if ((int)Session["Competition_Identification"] == 0)
            {
                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }

            if (IsPostBack)
            {
                int aux = TSGS_CS_Extention_Methods.GetLanguageCode(TSGS_CS_Extention_Methods.GetPostBackControlId(this));
                if (aux >= 1)
                {
                    Session["Language"] = aux;
                }
            }
            else
            {
                Master.SetErrorMessageGreen();
                Master.ErrorMessageVisibility(false);
                Label1.Visible = false;
                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                Ronde_nr = Client_WCF.GetRoundNumber((int)Session["Competition_Identification"]);

                TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
                Label4.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim(), Client_WCF.GetNumberTotal((int)Session["Competition_Identification"]), Client_WCF.GetNumberAbsentees((int)Session["Competition_Identification"], (int)Session["Round_Number"]), Client_WCF.GetNumberExternal((int)Session["Competition_Identification"], (int)Session["Round_Number"]));
                Client_MLC.Close();
                Client_WCF.Close();

            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
                Fill_Texts();
        }

        protected void Fill_Texts()
        {
            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            GridView1.Columns[1].HeaderText = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 11).Trim();
            GridView1.Columns[2].HeaderText = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            GridView1.Columns[3].HeaderText = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
            GridView1.Columns[5].HeaderText = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8).Trim();
            GridView1.Columns[6].HeaderText = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            GridView1.Columns[7].HeaderText = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 15).Trim();
            GridView1.Columns[8].HeaderText = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 16).Trim();
            GridView1.Columns[10].HeaderText = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 18).Trim();

            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 6).Trim();
            Button3.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            Label1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            Client_MLC.Close();
            RefreshDisplay(GridView1);

        }

		protected void RefreshDisplay (GridView GV)
		{
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

			ds_all.Clear();
            ds_all = Client_WCF.GetPlayerList((int)Session["Competition_Identification"]);
            GV.DataSource = ds_all;
            GridView1.AllowPaging = true;

            GridView1.PageSize = 15;
            GV.DataBind();
            GV.Font.Name = "Arial";
            GV.Font.Size = (int)Session["Fontsize"];
            GridViewRow rowHeader = GV.HeaderRow;
            GV.GridLines = GridLines.Horizontal;
            Aantal_Afwezig = 0;
			Aantal_Extern = 0;

            GV.SelectedIndex = -1;
  		}

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Fill_Texts();
        }

        protected void Button2_Click(object sender, System.EventArgs e)
		{
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            if ((string)Session["CallingFunction"] == "Blitz")
            {
                Session["Current_Status"] = 10;
                Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], (int)Session["Current_Status"]);
                Response.Redirect("TSGS_CS_Blitz_Capture_Results.aspx");
            }
            else
            {
                Session["Current_Status"] = 4;
                Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], (int)Session["Current_Status"]);
                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }
            Client_WCF.Close();
		}

        protected void Button3_Click(object sender, System.EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            TSGS_CS_WCF_Service.AbsenteeData AbsenteeData = new TSGS_CS_WCF_Service.AbsenteeData();
            Master.ErrorMessageVisibility(false);

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                GridView1.SelectedIndex = i;
                AbsenteeData.Speler_ID = System.Convert.ToInt32(GridView1.Rows[i].Cells[0].Text, 10);
                AbsenteeData.Rondenummer = (int)Session["Round_Number"];
                AbsenteeData.Competitie_ID = (int)Session["Competition_Identification"];
                AbsenteeData.Afwezigheidscode = 1;
                Client_WCF.AddAbsentee(AbsenteeData);
            }
            GridView1.PageIndex = 1;
            Fill_Texts();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            Label4.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim(), Client_WCF.GetNumberTotal((int)Session["Competition_Identification"]), Client_WCF.GetNumberAbsentees((int)Session["Competition_Identification"], (int)Session["Round_Number"]), Client_WCF.GetNumberExternal((int)Session["Competition_Identification"], (int)Session["Round_Number"]));

            Client_WCF.Close();
        }

        protected void GridView1_RowDataBound (object sender, GridViewRowEventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                e.Row.Cells[0].Text = Convert.ToString(dr["Speler_ID"]);
                e.Row.Cells[1].Text = Convert.ToString(dr["SpelerNaam"]);
                e.Row.Cells[1].Font.Size = 12;
                e.Row.Cells[2].Font.Size = 12;
                e.Row.Cells[3].Font.Size = 12;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Left; 
                int AbsenteeCode = Client_WCF.GetNoPlayCode((int)dr["Speler_ID"], (int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                if (AbsenteeCode == 1)
                {
                    CheckBox cb_afwezig = (CheckBox)e.Row.FindControl("cbAfwezig");
                    cb_afwezig.Checked = true;
                    e.Row.BackColor = System.Drawing.Color.Pink;
                    Aantal_Afwezig++;
                }
                if (AbsenteeCode == 5)
                {
                    CheckBox cb_extern = (CheckBox)e.Row.FindControl("cbExtern");
                    cb_extern.Checked = true;
                    e.Row.BackColor = System.Drawing.Color.LightBlue;
                    Aantal_Extern++;
                }
                CheckBox cb_kroongroep = (CheckBox)e.Row.FindControl("cbKroongroep");
                TextBox tb_Gamenumber = (TextBox)e.Row.FindControl("InputKroongroep");
                TextBox tb_Adversary_Id = (TextBox)e.Row.FindControl("tb_Adversary_Id");
                Label lbl_CG_Adversary_Name = (Label)e.Row.FindControl("lbl_CG_Adversary_Name");
                DropDownList ddl_CGGames = (DropDownList)e.Row.FindControl("DDLCGGames");
                ListBox lbox_Gamenr = (ListBox)e.Row.FindControl("lbox_Gamenr");
                ListBox lbox_Color = (ListBox)e.Row.FindControl("lbox_Color"); 
                int Aux_Gamenr = 0;

                lbl_CG_Adversary_Name.Visible = false;
                ddl_CGGames.Visible = false;
                
                if (Convert.ToByte(dr["Member_Premier_group"]) != 1)
                {
                    cb_kroongroep.Visible = false;
                    ddl_CGGames.Visible = false;
                }
                else
                {
                    DataSet ds_Games = Client_WCF.GetOpenChampionsgroupGames((int)dr["Speler_ID"], (int)Session["Competition_Identification"]);
                    ddl_CGGames.Items.Add(new ListItem("--"));
                    lbox_Gamenr.Items.Add(new ListItem("0"));
                    lbox_Color.Items.Add(new ListItem("0"));
                    foreach (DataRow dgItem in ds_Games.Tables[0].Rows)
                    {
                        ddl_CGGames.Items.Add(new ListItem((string)dgItem["Adversary_Name"]).Text.Trim());
                        lbox_Gamenr.Items.Add(new ListItem(dgItem["Game_Number"].ToString()));
                        lbox_Color.Items.Add(new ListItem(dgItem["Color"].ToString()));
                    }
                    ddl_CGGames.SelectedIndex = -1;
                    if (AbsenteeCode == 8)
                    {
                        cb_kroongroep.Checked = true;
                        Aux_Gamenr = Client_WCF.GetCGGamenrInThisRound((int)dr["Speler_ID"], (int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                        tb_Gamenumber.Text = Aux_Gamenr.ToString();
                        tb_Adversary_Id.Text = Client_WCF.GetCGPartnerInThisRound((int)dr["Speler_ID"], (int)Session["Competition_Identification"],  Aux_Gamenr).ToString();
                        int Pointer = 0;
                        if (Convert.ToInt16(tb_Gamenumber.Text) == 0)
                        {
                            lbl_CG_Adversary_Name.Visible = false;
                            ddl_CGGames.Visible = true;
                        }
                        else
                        {
                            foreach (ListItem s in lbox_Gamenr.Items)
                            {
                                string aux = System.Math.Abs(Convert.ToInt16(tb_Gamenumber.Text)).ToString();
                                if (s.Text == aux)
                                {
                                    lbl_CG_Adversary_Name.Text = ddl_CGGames.Items[Pointer].Text;
                                    lbl_CG_Adversary_Name.Visible = true;
                                    ddl_CGGames.Visible = false;
                                }
                                else
                                {
                                    Pointer++;
                                }
                            }
                            e.Row.BackColor = System.Drawing.Color.LemonChiffon;
                        }
                    }
                }
            }
        }

        protected void OnAfwezigChanged(object sender, EventArgs e)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            GridViewRow row = ((GridViewRow)((Control)sender).Parent.Parent);
            int i = row.RowIndex;
            GridView1.SelectedIndex = i;
            CheckBox cb_afwezig = (System.Web.UI.WebControls.CheckBox)GridView1.Rows[i].FindControl("cbAfwezig");
            CheckBox cb_extern = (System.Web.UI.WebControls.CheckBox)GridView1.Rows[i].FindControl("cbExtern");
            if (cb_afwezig.Checked && cb_extern.Checked)
            {
                Master.SetErrorMessage(string.Format(Client_MLC.GetMLCText((string)Session["Project"], "AttendanceRegister", (int)Session["Language"], 14).Trim(), GridView1.Rows[i].Cells[1].Text.Trim()));
                Master.ErrorMessageVisibility(true);
                Master.SetErrorMessageRed();
            }
            else
            {
                Master.ErrorMessageVisibility(false);
                TSGS_CS_WCF_Service.AbsenteeData AbsenteeData = new TSGS_CS_WCF_Service.AbsenteeData();

                AbsenteeData.Speler_ID = System.Convert.ToInt32(row.Cells[0].Text);
                AbsenteeData.Rondenummer = (int)Session["Round_Number"];
                AbsenteeData.Competitie_ID = (int)Session["Competition_Identification"];
                AbsenteeData.Kroongroep_partijnummer = 0;

                if (cb_extern.Checked == true)
                {
                    AbsenteeData.Afwezigheidscode = 5;
                    Aantal_Extern++;
                }
                else if (cb_afwezig.Checked == true)
                {
                    AbsenteeData.Afwezigheidscode = 1;
                    Aantal_Afwezig++;
                }
                else
                {
                    AbsenteeData.Afwezigheidscode = 0;
                }
                Client_WCF.AddAbsentee(AbsenteeData);
            }
//            RefreshDisplay(GridView1);
            Label4.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim(), Client_WCF.GetNumberTotal((int)Session["Competition_Identification"]), Client_WCF.GetNumberAbsentees((int)Session["Competition_Identification"], (int)Session["Round_Number"]), Client_WCF.GetNumberExternal((int)Session["Competition_Identification"], (int)Session["Round_Number"]));
            Client_MLC.Close();
            Client_WCF.Close();
        }

        protected void OnKroongroepChanged(object sender, EventArgs e)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            GridViewRow row = ((GridViewRow)((Control)sender).Parent.Parent);
            int i = row.RowIndex;
            CheckBox cbKroongroep = (System.Web.UI.WebControls.CheckBox)GridView1.Rows[i].FindControl("cbKroongroep");
            DropDownList ddl = (System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("DDLCGGames");
            Label lbl = (System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("lbl_CG_Adversary_Name");
            TextBox tb = (System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("InputKroongroep");
            TSGS_CS_WCF_Service.AbsenteeData AbsenteeData = new TSGS_CS_WCF_Service.AbsenteeData();
            AbsenteeData.Kroongroep_partijnummer = Convert.ToInt16(tb.Text);
            AbsenteeData.Speler_ID = System.Convert.ToInt32(row.Cells[0].Text);
            AbsenteeData.Rondenummer = (int)Session["Round_Number"];
            AbsenteeData.Competitie_ID = (int)Session["Competition_Identification"];

            GridView1.SelectedIndex = i;
            if (cbKroongroep.Checked == true)
            {
                ddl.Visible = true;
                AbsenteeData.Afwezigheidscode = 8;
                AbsenteeData.Kroongroep_partijnummer = 0;
                Client_WCF.AddAbsentee(AbsenteeData);
            }
            else 
            {
                ddl.Visible = false;
                lbl.Visible = false;
                lbl.Text = "";
                tb.Text = "";

                AbsenteeData.Afwezigheidscode = 0;
                int Adversary = Client_WCF.GetCGPartnerInThisRound(AbsenteeData.Speler_ID, AbsenteeData.Competitie_ID, AbsenteeData.Kroongroep_partijnummer);
                Client_WCF.AddAbsentee(AbsenteeData);

                if (AbsenteeData.Kroongroep_partijnummer != 0)
                {
                    AbsenteeData.Speler_ID = Adversary;
                    AbsenteeData.Kroongroep_partijnummer = 0 - AbsenteeData.Kroongroep_partijnummer;
                    Client_WCF.AddAbsentee(AbsenteeData);
                }
            }
            Client_WCF.AddAbsentee(AbsenteeData);

//            RefreshDisplay(GridView1);
            Client_MLC.Close();
            Client_WCF.Close();
        }

        protected void OnGameSelected(object sender, EventArgs e)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            GridViewRow row = ((GridViewRow)((Control)sender).Parent.Parent);
            int i = row.RowIndex;
            GridView1.SelectedIndex = i;
            CheckBox cb_kroongroep = (System.Web.UI.WebControls.CheckBox)GridView1.Rows[i].FindControl("cbKroongroep");
            ListBox lbox_Gamenr = (System.Web.UI.WebControls.ListBox)GridView1.Rows[i].FindControl("lbox_Gamenr");
            ListBox lbox_Color = (System.Web.UI.WebControls.ListBox)GridView1.Rows[i].FindControl("lbox_Color");
            DropDownList ddl_Adversary_Names = (System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("DDLCGGames");
            TextBox tb_Gamenummer = (System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("InputKroongroep");
            Label lbl_CG_Adversary_Name = (System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("lbl_CG_Adversary_Name");

            string aux_Gamenr = lbox_Gamenr.Items[ddl_Adversary_Names.SelectedIndex].Text;
            string aux_Color = lbox_Color.Items[ddl_Adversary_Names.SelectedIndex].Text;

            if (aux_Color == "-1")
            {
                tb_Gamenummer.Text = "-" + aux_Gamenr.Trim();
            }
            else
            {
                tb_Gamenummer.Text = aux_Gamenr.Trim();
            }


            TSGS_CS_WCF_Service.AbsenteeData AbsenteeData = new TSGS_CS_WCF_Service.AbsenteeData();

            AbsenteeData.Speler_ID = System.Convert.ToInt32(row.Cells[0].Text);
            AbsenteeData.Rondenummer = (int)Session["Round_Number"];
            AbsenteeData.Competitie_ID = (int)Session["Competition_Identification"];
            AbsenteeData.Afwezigheidscode = 8;
            AbsenteeData.Kroongroep_partijnummer = Convert.ToInt16(tb_Gamenummer.Text);
            Client_WCF.AddAbsentee(AbsenteeData);

            int Adversary = Client_WCF.GetCGPartnerInThisRound(AbsenteeData.Speler_ID, AbsenteeData.Competitie_ID, AbsenteeData.Kroongroep_partijnummer);
            AbsenteeData.Speler_ID = Adversary;
            AbsenteeData.Kroongroep_partijnummer = 0 - AbsenteeData.Kroongroep_partijnummer;
            Client_WCF.AddAbsentee(AbsenteeData);

            Label4.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim(), Client_WCF.GetNumberTotal((int)Session["Competition_Identification"]), Client_WCF.GetNumberAbsentees((int)Session["Competition_Identification"], (int)Session["Round_Number"]), Client_WCF.GetNumberExternal((int)Session["Competition_Identification"], (int)Session["Round_Number"]));

            RefreshDisplay(GridView1);

            Client_MLC.Close();
            Client_WCF.Close();
        }


        protected void OnExternChanged(object sender, EventArgs e)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            GridViewRow row = ((GridViewRow)((Control)sender).Parent.Parent);
            int i = row.RowIndex;
            GridView1.SelectedIndex = i;
            CheckBox cb_afwezig = (System.Web.UI.WebControls.CheckBox)GridView1.Rows[i].FindControl("cbAfwezig");
            CheckBox cb_extern = (System.Web.UI.WebControls.CheckBox)GridView1.Rows[i].FindControl("cbExtern");
            if (cb_afwezig.Checked && cb_extern.Checked)
            {
                Master.SetErrorMessage(string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 14).Trim(), GridView1.Rows[i].Cells[1].Text.Trim()));
                Master.ErrorMessageVisibility(true);
                Master.SetErrorMessageRed();
            }
            else
            {
                Master.ErrorMessageVisibility(false);
                TSGS_CS_WCF_Service.AbsenteeData AbsenteeData = new TSGS_CS_WCF_Service.AbsenteeData();
                AbsenteeData.Speler_ID = System.Convert.ToInt32(row.Cells[0].Text);
                AbsenteeData.Rondenummer = (int)Session["Round_Number"];
                AbsenteeData.Competitie_ID = (int)Session["Competition_Identification"];
                AbsenteeData.Kroongroep_partijnummer = 0;

                if (cb_extern.Checked == true)
                {
                    AbsenteeData.Afwezigheidscode = 5;
                    Aantal_Extern++;
                }
                else if (cb_afwezig.Checked == true)
                {
                    AbsenteeData.Afwezigheidscode = 1;
                    Aantal_Afwezig++;
                }
                else
                {
                    AbsenteeData.Afwezigheidscode = 0;
                }
                Client_WCF.AddAbsentee(AbsenteeData);
            }
            RefreshDisplay(GridView1);
            Label4.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim(), Client_WCF.GetNumberTotal((int)Session["Competition_Identification"]), Client_WCF.GetNumberAbsentees((int)Session["Competition_Identification"], (int)Session["Round_Number"]), Client_WCF.GetNumberExternal((int)Session["Competition_Identification"], (int)Session["Round_Number"]));
            Client_WCF.Close();
            Client_MLC.Close();
        }

    }
}