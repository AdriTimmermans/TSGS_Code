using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.SessionState;
using System.Text.RegularExpressions;

namespace TSGS_CS_Login_Client
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            /*
             * System.Environment.Exit(1)
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, "Application_Start", "Info", 4, "Started");
            Client_WCF.Close();                
             */

        }

        protected void Session_Start(object sender, EventArgs e)
        {

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            string ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            bool iscrawler = Regex.IsMatch(Request.UserAgent, @"bot|crawler|baiduspider|80legs|ia_archiver|voyager|curl|wget|yahoo! slurp|mediapartners-google", RegexOptions.IgnoreCase);
            if (Client_WCF.IPEntryDenied(ipAddress, iscrawler))
            {
                Client_WCF.WriteLogLine("No log in", 0, "Break in attempt/crawler", "Serious", 2, "Denied for IP:" + ipAddress);
                Session.Clear();
                Session.Abandon();
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-10);
                Session.Timeout = 1; 
                System.Environment.Exit(0);
            }
            else
            {
                Client_WCF.WriteLogLine((string)Session["Manager"], 0, "Comp. process", "Workflow", 2, "Session: " + (string)Session.SessionID + " started");
                Session["Manager"] = "No log in";
                Session["AutoOverview"] = false;
                Session["Fontsize"] = 10;
                Session["Manager_Id"] = 0;
                Session["Competition_Identification"] = 0;
                Session["Round_Number"] = 0;
                Session["Club"] = "-";
                Session["Club_ID"] = "140010";
                Session["Competition"] = "-";
                Session["Competition_Selection"] = 0;
                Session["Language"] = 2;
                Session["Project"] = "CS";
                Session["Functionality"] = "SystemMonitor";
                Session["Loginmoment"] = string.Format("{0:d} - {0:t}", DateTime.Now);
                Session["PlayerToUpdate"] = 0;
                Session["Parent_CID"] = -1;
                Session["ManagerEmailAddress"] = "-";
                Session["Current_Status"] = 1;
                Session["CallingFunction"] = "-";
                Session["UserPrivileges"] = 2;
                Session["ForceExit"] = 0;
                Session["GamesPerRound"] = 1;
                Session["BreakCount"] = 0;
                Session["Competitie_Type"] = 0;
                Session["LastFunction"] = "TSGS_CS_Competition_System_System_Monitor.aspx";


                Client_WCF.UpdateAccessNumber((string)Session.SessionID, (string)Session["Manager"], 1);
                if (Client_WCF.Count_Sessions() >= 3)
                {
                    Client_WCF.WriteLogLine((string)Session["Manager"], 1, "Comp. process", "Error", 2, "too many open sessions");
                    Session["ForceExit"] = 1;
                }
                Client_WCF.Close();
            }

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
/*            
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine("(string)Session["Manager"], 0, "Application_BeginRequest", "Info", 4, "Started");
            Client_WCF.Close();                
      */      
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
/*            
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 0, "Application_AuthenticateRequest", "Info", 4, "Started");
            Client_WCF.Close();                
  */           
        }

        protected void Application_Error(object sender, EventArgs e)
        {

            // Code that runs when an unhandled error occurs

            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            // Get the exception object.
            Exception exc = Server.GetLastError();
            // Handle HTTP errors
            if (exc.GetType() == typeof(HttpException))
            {
                // The Complete Error Handling Example generates
                // some errors using URLs with "NoCatch" in them;
                // ignore these here to simulate what would happen
                // if a global.asax handler were not implemented.
                if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
                    return;

                //Redirect HTTP errors to HttpError page
                Server.Transfer("HttpErrorPage.aspx");
            }

            // For other kinds of errors give the user some information
            // but stay on the default page
            Response.Write("<h2>Global Page Error</h2>\n");
            Response.Write("<p>" + exc.Message + "</p>\n");
            Response.Write("Return to the <a href='https://www.schaakclub-dordrecht.nl'>" +"Default Page</a>\n");

            Client_WCF.WriteLogExceptionTrace((string)Session["Manager"], 1, "Application_Error", "Error", 1, "Catch: " + exc.StackTrace);
            Client_WCF.Close();

            // Log the exception and notify system operators
//            ExceptionUtility.LogException(exc, "DefaultPage");
//            ExceptionUtility.NotifySystemOps(exc);

            // Clear the error from the server
            Server.ClearError();            
        }

        protected void Session_End(object sender, EventArgs e)
        {
            TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient Client_WCF = new TSGS_CS_WCF_Service.TSGS_CS_WCF_ServiceClient();
            Client_WCF.WriteLogLine((string)Session["Manager"], 1, "Session_End", "Workflow", 2, "Session: " + (string)Session.SessionID + " completed");
            Client_WCF.Close();                
        }

        protected void Application_End(object sender, EventArgs e)
        {
            
            
        }

    }
}