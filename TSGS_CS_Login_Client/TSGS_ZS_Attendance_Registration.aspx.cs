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

    public partial class TSGS_ZS_Attendance_Registration : System.Web.UI.Page
    {

        int Ronde_nr;
        int Aantal_Aanwezig = 0;
        DataSet ds_all = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            string Class_Name = this.GetType().Name;
            int CutOff_Length = System.Math.Min(20, Class_Name.Length-8);
            Session["Functionality"] = Class_Name.Substring(8,  CutOff_Length);

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

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
            Button3.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 10).Trim();
            Label1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 2).Trim();
            Label4.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim(), Client_WCF.GetNumberTotal((int)Session["Competition_Identification"]), Client_WCF.GetNumberAbsentees((int)Session["Competition_Identification"], (int)Session["Round_Number"]), Client_WCF.GetNumberExternal((int)Session["Competition_Identification"], (int)Session["Round_Number"]));
            Ronde_nr = Client_WCF.GetRoundNumber((int)Session["Competition_Identification"]);
            Label3.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim(), Ronde_nr);
            Client_MLC.Close();
            Client_WCF.Close();
            RefreshDisplay(DataList1);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if ((int)Session["Current_Status"] > 2)
            {
                TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
                Master.ErrorMessageVisibility(true);
                Master.SetErrorMessage(Session["Club"] + ", " + Session["Competition"] + " - " +
                   Client_MLC.GetMLCText((string)Session["Project"], "StateDescription", (int)Session["Language"], (int)Session["Current_Status"]).Trim());
                Client_MLC.Close();
            }
            else
            {
                Master.ErrorMessageVisibility(false);
            }
            Fill_Texts();

        }

        protected void RefreshDisplay(DataList DL)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            ds_all.Clear();
            ds_all = Client_WCF.GetPlayerList((int)Session["Competition_Identification"]);
            DL.DataSource = ds_all;
            DL.DataBind();
            DL.Font.Name = "Arial";
            DL.Font.Size = (int)Session["Fontsize"];
            Aantal_Aanwezig = 0;
            DL.SelectedIndex = -1;
        }

        protected void Button2_Click(object sender, System.EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Session["Current_Status"] = 32;
            Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], (int)Session["Current_Status"]);
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            Client_WCF.Close();
        }


        protected void Button3_Click(object sender, System.EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            TSGS_CS_WCF_Service.AbsenteeData AbsenteeData = new TSGS_CS_WCF_Service.AbsenteeData();
            Master.ErrorMessageVisibility(false);

            for (int i = 0; i < DataList1.Items.Count; i++)
            {
                DataListItem Item = DataList1.Items[i];
                CheckBox cb_Aanwezig = (CheckBox)Item.FindControl("CheckBox1");
                TSGS_CS_WCF_Service.AbsenteeData PresentData = new TSGS_CS_WCF_Service.AbsenteeData();
                if (cb_Aanwezig.Checked == false)
                {
                    TextBox tb = (TextBox)Item.FindControl("tb_Speler_ID");
                    int Speler_Id_Local = System.Convert.ToInt32(tb.Text);
                    PresentData.Speler_ID = Speler_Id_Local;
                    PresentData.Rondenummer = (int)Session["Round_Number"];
                    PresentData.Competitie_ID = (int)Session["Competition_Identification"];
                    PresentData.Kroongroep_partijnummer = 0;
                    PresentData.Afwezigheidscode = 1;
                    Aantal_Aanwezig++;
                    Client_WCF.AddAbsentee(PresentData);
                }

            } 

            Fill_Texts();
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            Label4.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim(), Client_WCF.GetNumberTotal((int)Session["Competition_Identification"]), Client_WCF.GetNumberAbsentees((int)Session["Competition_Identification"], (int)Session["Round_Number"]) );

            Client_WCF.Close();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

        protected void DataList1_ItemBound(object sender, DataListItemEventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
             if (e.Item.ItemType == ListItemType.Item || 
                 e.Item.ItemType == ListItemType.AlternatingItem)
             {
                TextBox tb = (TextBox)e.Item.FindControl("tb_Speler_ID");
                int Speler_Id_Local = System.Convert.ToInt32(tb.Text);
                int PresentCode = Client_WCF.GetNoPlayCode(Speler_Id_Local, (int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                if (PresentCode == 0)
                {
                    CheckBox cb_Aanwezig = (CheckBox)e.Item.FindControl("CheckBox1");
                    cb_Aanwezig.Checked = true;
                    e.Item.BackColor = System.Drawing.Color.Pink;
                    Aantal_Aanwezig++;
                }
            }
        }

        protected void OnAfwezigChanged(object sender, EventArgs e)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            DataListItem Item = ((DataListItem)((Control)sender).Parent);
            int i = Item.ItemIndex;
            DataList1.SelectedIndex = i;
            CheckBox cb_Aanwezig = (CheckBox)Item.FindControl("CheckBox1");
            TSGS_CS_WCF_Service.AbsenteeData PresentData = new TSGS_CS_WCF_Service.AbsenteeData();
            TextBox tb = (TextBox)Item.FindControl("tb_Speler_ID");
            int Speler_Id_Local = System.Convert.ToInt32(tb.Text);
            PresentData.Speler_ID = Speler_Id_Local;
            PresentData.Rondenummer = (int)Session["Round_Number"];
            PresentData.Competitie_ID = (int)Session["Competition_Identification"];
            PresentData.Kroongroep_partijnummer = 0;
            if (cb_Aanwezig.Checked == true)
            {
                PresentData.Afwezigheidscode = 0;
                Aantal_Aanwezig++;
            }
            else
            {
                PresentData.Afwezigheidscode = 1;
            }
            Client_WCF.AddAbsentee(PresentData);

            RefreshDisplay(DataList1);
            Label4.Text = string.Format(Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim(), Client_WCF.GetNumberTotal((int)Session["Competition_Identification"]), Client_WCF.GetNumberAbsentees((int)Session["Competition_Identification"], (int)Session["Round_Number"]), Client_WCF.GetNumberExternal((int)Session["Competition_Identification"], (int)Session["Round_Number"]));
            Client_MLC.Close();
            Client_WCF.Close();
        }

    }
}
