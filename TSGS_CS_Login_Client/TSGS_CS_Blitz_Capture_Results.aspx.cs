using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Globalization;

namespace TSGS_CS_Login_Client
{

    public partial class TSGS_CS_Blitz_Capure_Results : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Functionality"] = "BlitzResults";
            Session["LastFunction"] = HttpContext.Current.Request.Url.AbsolutePath;
            Session["Groepnummer"] = 1;
            Session["GridLine"] = 0;

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
                DataSet dsP = Client_WCF.GetPlayerList((int)Session["Competition_Identification"]);
                dsP.Tables[0].Columns.Add("StringGroepnr", typeof(string));
                dsP.Tables[0].Columns.Add("StringMatchpunten", typeof(string)); 
                dsP.Tables[0].Columns.Add("Groepnr", typeof(int));
                dsP.Tables[0].Columns.Add("Matchpunten", typeof(float));
                //
                // Before anything else... Remove previous result records end then
                // create resultrecords for all players in total competition as default on absent, also for new players and for absent players
                // these results will be updated later in the process
                //
                Client_WCF.Remove_BlitzResults((int)Session["Competition_Identification"], (int)Session["Round_Number"]);

                for (int i = 0; i < dsP.Tables[0].Rows.Count; i++)
                {
                    Client_WCF.AddPlayerBlitzResultInit((int)Session["Competition_Identification"], (int)Session["Round_Number"], (int)dsP.Tables[0].Rows[i][0]);
                }

                DataSet dsA = Client_WCF.GetAbsenteeList((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                if ((DataSet)dsA != null)
                {
                    for (int i = 0; i < dsA.Tables[0].Rows.Count; i++)
                    {
                        bool Continue_Synch = true;
                        bool Remove_From_Players = false;
                        int j = 0;
                        while (Continue_Synch)
                        {
                            if ((int)dsP.Tables[0].Rows[j]["Speler_ID"] != (int)dsA.Tables[0].Rows[i]["Speler_ID"])
                            {
                                dsP.Tables[0].Rows[j]["StringGroepnr"] = "0";
                                dsP.Tables[0].Rows[j]["StringMatchpunten"] = "0.0"; 
                                dsP.Tables[0].Rows[j]["Groepnr"] = 0;
                                dsP.Tables[0].Rows[j]["Matchpunten"] = 0.0;
                                j++;
                            }
                            else
                            {
                                Remove_From_Players = true;
                                dsP.Tables[0].Rows[j].Delete();
                                j = (int)dsP.Tables[0].Rows.Count + 1;
                            }
                            Continue_Synch = (j <= (int)dsP.Tables[0].Rows.Count);
                        }
                        if (Remove_From_Players)
                        {
                            dsP.AcceptChanges();
                        }
                    }
                }
                ViewState["Players"] = dsP;
                Client_WCF.Close();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Fill_Texts();
            if ((int)Session["Groepnummer"] == 0)
            {
                GridView1.Rows[(int)Session["GridLine"]].FindControl("StringMatchpunten").Focus();
            }
            else
            {
                GridView1.Rows[(int)Session["GridLine"]].FindControl("StringGroepnr").Focus();
            }
        }

        protected void Fill_Texts()
        {

            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            TSGS_CS_Format_Controls controls = new TSGS_CS_Format_Controls();
            controls.FormatControlsLocal(MainContent.Controls, (int)Session["FontSize"]);

            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();

            Label1.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 4).Trim();
            Button1.Text = Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], 1).Trim();
            Button2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 3).Trim();
            Label30.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 9).Trim();
            Label2.Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();

            DataSet dsP = (DataSet)ViewState["Players"];
            if (dsP.Tables[0].Rows.Count == 0)
            {
                TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
                Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 4, "Closed");
                Client_WCF.Close();
                Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
            }
            else
            {
                GridView1.DataSource = dsP;

                GridView1.AllowPaging = false;
//                GridView1.PageSize = 15; //
                GridView1.DataBind();
                GridView1.Font.Name = "Arial";
                GridView1.Font.Size = (int)Session["Fontsize"];
                GridView1.GridLines = GridLines.Horizontal;
                GridView1.SelectedIndex = -1;

                GridViewRow rowHeader = GridView1.HeaderRow;
                rowHeader.Cells[1].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 5).Trim();
                rowHeader.Cells[2].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 6).Trim();
                rowHeader.Cells[3].Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 7).Trim();
            }

            Client_MLC.Close();
        }

        protected void RefreshDisplay(GridView GV)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataSet ds_all = new DataSet();
            ds_all.Clear();
            ds_all = (DataSet) ViewState["Players"];
            GV.DataSource = ds_all;
            GridView1.AllowPaging = true;

            GridView1.PageSize = 15;
            GV.DataBind();
            GV.Font.Name = "Arial";
            GV.Font.Size = (int)Session["Fontsize"];
            GridViewRow rowHeader = GV.HeaderRow;
            GV.GridLines = GridLines.Horizontal;

            GV.SelectedIndex = -1;
        }


        protected void OnGroepnrInputChanged(object sender, EventArgs e)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            GridViewRow row = ((GridViewRow)((Control)sender).Parent.Parent);
            int i = row.RowIndex;
            GridView1.SelectedIndex = i;
            TextBox tb_Groepnr = (System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("StringGroepnr");

            int Input_TB = 0;
            int error = Client_WCF.ValidateInteger(tb_Groepnr.Text.Trim(), true, 0, true, 0, 100, ref Input_TB);
            if (error == 0)
            {
                Master.ErrorMessageVisibility(false);
                tb_Groepnr.BackColor = System.Drawing.Color.White;
                Update_Dataset();
            }
            else
            {
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                Master.SetErrorMessage(tb_Groepnr.Text.Trim() + ": " + Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], error + 1).Trim());
                tb_Groepnr.BackColor = System.Drawing.Color.Red;
            }
            Client_MLC.Close();
            Client_WCF.Close();

            Session["GridLine"] = i ;
            Session["Groepnummer"] = 0;
        }

        protected void OnMatchpuntenInputChanged(object sender, EventArgs e)
        {
            TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();

            GridViewRow row = ((GridViewRow)((Control)sender).Parent.Parent);
            int i = row.RowIndex;
            GridView1.SelectedIndex = i;
            TextBox tb_Matchpunten = (System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("StringMatchpunten");

            Double Input_TB = 0;
            int error = Client_WCF.ValidateReal(tb_Matchpunten.Text.Trim(), true, 0.0, true, 0.0, 100.0, ref Input_TB);
            if (error == 0)
            {
                Master.ErrorMessageVisibility(false);
                tb_Matchpunten.BackColor = System.Drawing.Color.White;
                Update_Dataset();
            }
            else
            {
                Master.SetErrorMessageRed();
                Master.ErrorMessageVisibility(true);
                Master.SetErrorMessage(tb_Matchpunten.Text.Trim() + ": " + Client_MLC.GetMLCText((string)Session["Project"], "General", (int)Session["Language"], error + 1).Trim());
                tb_Matchpunten.BackColor = System.Drawing.Color.Red;
            }
            Client_MLC.Close();
            Client_WCF.Close();

            Session["GridLine"] = System.Math.Min(i + 1, GridView1.Rows.Count);
            Session["Groepnummer"] = 1;

        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox tb_Matchpunten = (System.Web.UI.WebControls.TextBox)e.Row.FindControl("StringMatchpunten");
                tb_Matchpunten.Attributes.Add("onkeypress", "javascript:return clickButton(event);");
                TextBox tb_Groepnr = (System.Web.UI.WebControls.TextBox)e.Row.FindControl("StringGroupnr");
                tb_Groepnr.Attributes.Add("onkeypress", "javascript:return clickButton(event);"); 
            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                DataRow dr = dv.Row;
                e.Row.Cells[0].Text = Convert.ToString(dr["Speler_ID"]);
                e.Row.Cells[1].Text = Convert.ToString(dr["SpelerNaam"]);
                e.Row.Cells[1].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[2].Font.Size = (int)Session["Fontsize"];
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;

                if (dr["Matchpunten"] == DBNull.Value)
                {
                    dr["Matchpunten"] = 0.0;
                }
                if (dr["Groepnr"] == DBNull.Value)
                {
                    dr["Groepnr"] = 0;
                }

            }
        }


        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Fill_Texts();
        }

        protected void Update_Dataset()
        {
            int i = GridView1.PageSize * GridView1.PageIndex;
            DataSet ds = (DataSet)ViewState["Players"];
            foreach (GridViewRow row in GridView1.Rows)
            {
                TextBox tb1 = (TextBox)row.FindControl("StringGroepnr");
                TextBox tb2 = (TextBox)row.FindControl("StringMatchpunten");
                ds.Tables[0].Rows[i]["Groepnr"] = Convert.ToInt32(tb1.Text);
                ds.Tables[0].Rows[i]["Matchpunten"] = Convert.ToDouble(tb2.Text, CultureInfo.InvariantCulture);
                ds.Tables[0].Rows[i]["StringGroepnr"] = tb1.Text;
                ds.Tables[0].Rows[i]["StringMatchpunten"] = tb2.Text;

                i++;
            }
            ds.AcceptChanges();
            ViewState["Players"] = (DataSet)ds;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            DataSet dsP = (DataSet)ViewState["Players"];
            DataView dv = new DataView(dsP.Tables[0]);
            dv.Sort = "Matchpunten DESC";
            double Total_Points_Required = 0.0;
            double Max_Points = Convert.ToDouble(dsP.Tables[0].Rows.Count * (dsP.Tables[0].Rows.Count - 1) / 2);
            if (Client_WCF.ValidateReal(TextBox11.Text, false, 0.0, true, 1.0, Max_Points, ref Total_Points_Required) == 0)
            {

                float TotalMatchpoints = 0;
                for (int i = 0; i < dsP.Tables[0].Rows.Count; i++)
                {
                    TotalMatchpoints = TotalMatchpoints + (float)dv[i]["Matchpunten"];
                }
                if (Math.Abs(TotalMatchpoints - Total_Points_Required) < 0.01)
                {
                    float PenaltyPoints = -1;
                    float Growth = 1;
                    float LastMatchPoints = -1;
                    for (int i = 0; i < dv.Count; i++)
                    {
                        TSGS_CS_WCF_Service.BlitzResultData OnePlayer = new TSGS_CS_WCF_Service.BlitzResultData();
                        OnePlayer.Competitie_Id = (int)Session["Competition_Identification"];
                        OnePlayer.Rondernr = (int)Session["Round_Number"];
                        OnePlayer.Deelnemer_ID = (int)dv[i]["Speler_Id"];
                        OnePlayer.Matchpunten = (float)dv[i]["Matchpunten"];
                        if (LastMatchPoints != OnePlayer.Matchpunten)
                        {
                            PenaltyPoints = (float)PenaltyPoints + Growth;
                            Growth = 1;
                        }
                        else
                        {
                            Growth = Growth + 1;
                        }
                        OnePlayer.Strafpunten = PenaltyPoints;
                        LastMatchPoints = OnePlayer.Matchpunten;
                        OnePlayer.Groepnummer = (int)dv[i]["Groepnr"];
                        Client_WCF.UpdateOneBlitzResult(OnePlayer);
                    }

                    dsP.Tables[0].Columns.Add("ELORating", typeof(float));
                    dsP.Tables[0].Columns.Add("ELOAdversaries", typeof(float));
                    dsP.Tables[0].Columns.Add("ELOGain", typeof(float));
                    dsP.AcceptChanges();
                    Client_WCF.Remove_RatingRound((int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                    Label1.Visible = true;
                    int Max_Players = (int)dsP.Tables[0].Rows.Count;
                    float TotalRating = 0;
                    int PlayersWithoutRating = 0;
                    float TEN = (float)10.0;
                    float Fourhundred = (float)400.0;
                    float One = (float)1.0;
                    //
                    // using Px approximation used by KNSB
                    //
                    for (int i = 0; i < Max_Players; i++)
                    {
                        int PID = (int)dsP.Tables[0].Rows[i][0];
                        dsP.Tables[0].Rows[i]["ELORating"] = Client_WCF.GetClubBlitzRating(PID);
                        if (Convert.ToSingle(dsP.Tables[0].Rows[i]["ELORating"]) < 700)
                        {
                            PlayersWithoutRating++;
                        }
                        else
                        {
                            TotalRating = TotalRating + Convert.ToSingle(dsP.Tables[0].Rows[i]["ELORating"]);
                        }
                    }
                    float AverageRating = TotalRating / (Max_Players - PlayersWithoutRating);
                    for (int i = 0; i < Max_Players; i++)
                    {
                        if (Convert.ToSingle(dsP.Tables[0].Rows[i]["ELORating"]) < 700.0)
                        {
                            dsP.Tables[0].Rows[i]["ELORating"] = AverageRating;
                            TotalRating = TotalRating + Convert.ToSingle(dsP.Tables[0].Rows[i]["ELORating"]);
                        }
                    }
                    float tempnewrating;
                    for (int i = 0; i < Max_Players; i++)
                    {
                        dsP.Tables[0].Rows[i]["ELOAdversaries"] = (TotalRating - (float)dsP.Tables[0].Rows[i]["ELORating"]) / (Max_Players - 1);
                        double W = Convert.ToSingle(dsP.Tables[0].Rows[i]["Matchpunten"], CultureInfo.InvariantCulture);
                        float P_normal = One / (float)(System.Math.Pow(TEN, ((Convert.ToSingle(dsP.Tables[0].Rows[i]["ELOAdversaries"]) - Convert.ToSingle(dsP.Tables[0].Rows[i]["ELORating"])) / Fourhundred)) + One);
                        double We = P_normal * Convert.ToSingle(Max_Players - 1);
                        dsP.Tables[0].Rows[i]["ELOGain"] = (W - We) * Client_WCF.GetKFactor((int)Session["Competition_Identification"]);
                        tempnewrating = (float)dsP.Tables[0].Rows[i]["ELORating"] + (float)dsP.Tables[0].Rows[i]["ELOGain"];
                        Client_WCF.SaveBlitzRating((int)dsP.Tables[0].Rows[i][0], tempnewrating, (int)Session["Competition_Identification"], (int)Session["Round_Number"]);
                    }
                    Label1.Visible = true;
                    Session["Current_Status"] = 11;
                    Client_WCF.SetIntInAlgemeneInfo("CurrentState", (int)Session["Competition_Identification"], 11);
                    Client_WCF.Update_Workflow_Item("[Resultaten Verwerken]", (int)Session["Competition_Identification"], (int)Session["Round_Number"], 1);
                    Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 4, "Closed");
                    Client_WCF.Close();
                    Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
                }
                else
                {
                    Master.SetErrorMessageRed();
                    Master.ErrorMessageVisibility(true);
                    TSGS_MLC_Service.TSGS_MLC_ServiceClient Client_MLC = new TSGS_MLC_Service.TSGS_MLC_ServiceClient();
                    string Error_Text = Client_MLC.GetMLCText((string)Session["Project"], (string)Session["Functionality"], (int)Session["Language"], 8);
                    String.Format(Error_Text, TotalMatchpoints, (dsP.Tables[0].Rows.Count * (dsP.Tables[0].Rows.Count - 1) / 2));
                    Master.SetErrorMessage(Error_Text);
                    Client_MLC.Close();
                    Label1.Visible = false;
                }
            }
            else
            {
                Master.ErrorMessageVisibility(true);
                Master.SetErrorMessageRed();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], (int)Session["Competition_Identification"], this.GetType().Name, "Info", 4, "Closed");
            Client_WCF.Close();
            Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}