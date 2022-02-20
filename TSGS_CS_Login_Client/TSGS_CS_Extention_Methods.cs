using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace TSGS_CS_Login_Client
{
    public static class TSGS_CS_Extention_Methods
    {
        public static string GetPostBackControlId(this Page page)
        {
            if (!page.IsPostBack)
                return string.Empty;

            Control control = null;
            // first we will check the "__EVENTTARGET" because if post back made by the controls
            // which used "_doPostBack" function also available in Request.Form collection.
            string controlName = page.Request.Params["__EVENTTARGET"];
            if (!String.IsNullOrEmpty(controlName))
            {
                control = page.FindControl(controlName);
            }
            else
            {
                // if __EVENTTARGET is null, the control is a button type and we need to
                // iterate over the form collection to find it

                // ReSharper disable TooWideLocalVariableScope
                string controlId;
                Control foundControl;
                // ReSharper restore TooWideLocalVariableScope

                foreach (string ctl in page.Request.Form)
                {
                    // handle ImageButton they having an additional "quasi-property" 
                    // in their Id which identifies mouse x and y coordinates
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        controlId = ctl.Substring(0, ctl.Length - 2);
                        foundControl = page.FindControl(controlId);
                    }
                    else
                    {
                        foundControl = page.FindControl(ctl);
                    }

                    if (!(foundControl is System.Web.UI.WebControls.Button || foundControl is System.Web.UI.WebControls.ImageButton)) continue;

                    control = foundControl;
                    break;
                }
            }

            return control == null ? String.Empty : control.ID;
        }

        public static int GetLanguageCode (string ButtonName)
        {
            int aux = 0;
            if (ButtonName == "ImageButton2")
            {
                aux = 1;
            }
            if (ButtonName == "ImageButton1")
            {
                aux = 2;
            }
            return aux;
        }
    }
}