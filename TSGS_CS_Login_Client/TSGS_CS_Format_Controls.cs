using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TSGS_CS_Login_Client
{
    public class TSGS_CS_Format_Controls
    {
        
        public void FormatControlsLocal(ControlCollection FormControls, int FontSize)
        {
            TextBox tb;
            Label lbl;
            Button btn;
            ImageButton Ibtn;

            foreach (Control ctl in FormControls)
            {
                if (ctl.GetType() == typeof(Label))
                {
                    lbl = (Label)ctl;
                    lbl.Font.Size = FontSize;
                    lbl.Font.Name = "Verdana";
                    lbl.Height = Convert.ToInt16(Convert.ToSingle(FontSize) * 2.5);
                }
                else if (ctl.GetType() == typeof(TextBox))
                {
                    tb = (TextBox)ctl;
                    tb.Font.Size = FontSize;
                    tb.Font.Name = "Verdana";
                    tb.Height = Convert.ToInt16(Convert.ToSingle(FontSize) * 2.5);
                    tb.Attributes.Add("onkeypress","javascript:return clickButton(event);" );
                }
                else if (ctl.GetType() == typeof(Button))
                {
                    btn = (Button)ctl;
                    btn.Font.Size = FontSize;
                    btn.Font.Name = "Verdana";
                    btn.Height = Convert.ToInt16(Convert.ToSingle(FontSize) * 2.5);
                    if ((int) HttpContext.Current.Session["ForceExit"] != 0)
                    {
                        btn.Enabled = false;
                    }
                }
                else if (ctl.GetType() == typeof(ImageButton))
                {
                    Ibtn = (ImageButton)ctl;
                    Ibtn.Height = Convert.ToInt16((Convert.ToSingle(FontSize)/14.0)*40);
                }
            }
        }

        protected void imageButton3_Click(object sender, ImageClickEventArgs e)
        {
//            HttpContext.Current.Response.Redirect("TSGS_CS_Competition_System_System_Monitor.aspx");
        }
        protected void imageButton2_Click(object sender, ImageClickEventArgs e)
        {
//           Global.Session["Language"] = 1;
        }
        protected void imageButton1_Click(object sender, ImageClickEventArgs e)
        {
//            HttpContext.Current.Session["Language"] = 2;
        }

        public void Create_Header(Panel Panel2)
        {

            int FontSize = (int) HttpContext.Current.Session["FontSize"];

            ImageButton imageButton3 = new ImageButton();
            imageButton3.ID = "imageButton3";
            imageButton3.Height = Unit.Pixel(Convert.ToInt16(Convert.ToSingle(FontSize) * 2.5));
            imageButton3.ImageUrl = "~/images/home.jpg";
            imageButton3.Click += new ImageClickEventHandler(imageButton3_Click);
            Panel2.Controls.Add(imageButton3);
            ImageButton imageButton2 = new ImageButton();
            imageButton2.ID = "imageButton2";
            imageButton2.Height = Unit.Pixel(Convert.ToInt16(Convert.ToSingle(FontSize) * 2.5));
            imageButton2.ImageUrl = "~/images/Nld.jpg";
            imageButton2.Click += new ImageClickEventHandler(imageButton2_Click);
            Panel2.Controls.Add(imageButton2);
            ImageButton imageButton1 = new ImageButton();
            imageButton1.ID = "imageButton1";
            imageButton1.Height = Unit.Pixel(Convert.ToInt16(Convert.ToSingle(FontSize) * 2.5));
            imageButton1.ImageUrl = "~/images/Eng.jpg";
            imageButton1.Click += new ImageClickEventHandler(imageButton1_Click);
            Panel2.Controls.Add(imageButton1);
        }
    }
}