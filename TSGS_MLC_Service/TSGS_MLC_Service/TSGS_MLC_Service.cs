using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TSGS_MLC_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TSGS_MLC_Service" in both code and config file together.
    public class TSGS_MLC_Service : ITSGS_MLC_Service
    {

        protected string Connection_String_MLC()
        {
            string cs = "";
            string Status = ConfigurationManager.AppSettings["Phase"];
            if (Status == "DEV")
            {
                cs = ConfigurationManager.ConnectionStrings["DBMLCDEV"].ConnectionString;
            }
            else if (Status == "PROD")
            {
                cs = ConfigurationManager.ConnectionStrings["DBMLCPROD"].ConnectionString;
            }
            return cs;
        }

        public void TranslateLanguages()
        {

            //
            // First prepare the list of required languages
            // 
            List<string> Language_Codes = new List<string>();
            DataSet dsl = GetLanguageList();
            foreach (DataRow dsItem in dsl.Tables[0].Rows)
            {
                Language_Codes.Add((string)dsItem["Language_abbreviation"]);
            }
            //
            // select all dutch texts
            //
            string cs = Connection_String_MLC();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Get_all_texts_in_Dutch";
                da.SelectCommand = cmd;
                con.Open();
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    ds = null;
                }
                finally
                {
                }
                con.Close();
                con.Dispose();
            }

            //
            // Result is a dataset in Dutch
            //

            string Project;
            string Functionality;
            int Language_Code;
            int TextOrder;
            string TextPart_Original;
            string TextPart_Translated;
            string output_language_code;
            //
            // now for each individual line of Dutch text
            //
            foreach (DataRow dgItem in ds.Tables[0].Rows)
            {

                Project = (string)dgItem["Project"];
                Functionality = (string)dgItem["Functionality"];
                Language_Code = Convert.ToInt16(dgItem["Language_Code"]);
                TextOrder = Convert.ToInt16(dgItem["TextOrder"]);
                TextPart_Original = (string)dgItem["TextPart"];
                //
                //  cycle through all languages
                //
                for (int i = 1; i < 66; i++)
                {

                    //
                    // Only translate into "translatable"languages
                    //
                    output_language_code = Language_Codes[i].Trim();
                    //
                    // and only translate if there is no translation already available
                    // This way, when text is added, the whole translation process does not take that much time, 
                    // and manually corrected translations are not touched
                    //
                    if (!Translation_Exists(Project, Functionality, i + 1, TextOrder))
                    {
                        TextPart_Translated = TranslateText(TextPart_Original, "nl|" + output_language_code);
                        using (SqlConnection con = new SqlConnection(cs))
                        {
                            con.Open();
                            try
                            {
                                SqlCommand cmd = con.CreateCommand();
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "spInsertTranslation";
                                cmd.Parameters.Add("@PRO", SqlDbType.Text).Value = Project.Trim();
                                cmd.Parameters.Add("@FUN", SqlDbType.Text).Value = Functionality.Trim();
                                cmd.Parameters.Add("@LCO", SqlDbType.Int).Value = i + 1;
                                cmd.Parameters.Add("@TOR", SqlDbType.Int).Value = TextOrder;
                                cmd.Parameters.Add("@TXT", SqlDbType.Text).Value = TextPart_Translated.Trim();

                                int LR_Value = (int)cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                string lit_Error = ex.Message;
                            }
                            finally
                            {
                                con.Close();
                                con.Dispose();
                            }
                        }
                    }
                }
            }
        }

        protected bool Translation_Exists(string Project, string Functionality, int Language_Code, int TextOrder)
        {
            bool aux = true;
            string cs = Connection_String_MLC();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetText", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Project", SqlDbType.Text).Value = Project;
                cmd.Parameters.Add("@Functionality", SqlDbType.Text).Value = Functionality;
                cmd.Parameters.Add("@Language_Code", SqlDbType.Int).Value = Language_Code;
                cmd.Parameters.Add("@TextOrder", SqlDbType.Int).Value = TextOrder;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                aux = reader.HasRows;
            }
            return aux;
        }

        public static string TranslateText(string input, string languagePair)
        {
            string url = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", input, languagePair);
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();

            string result = httpClient.GetStringAsync(url).Result;
            result = result.Substring(result.IndexOf("<span title=\"") + "<span title=\"".Length);
            result = result.Substring(result.IndexOf(">") + 1);
            result = result.Substring(0, result.IndexOf("</span>"));
            return result.Trim();
        }

        public DataSet GetLanguageList()
        {
            //
            // Get a dataset with all competitions under control of a specific competition manager
            //
            string cs = Connection_String_MLC();

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetLanguages";

                da.SelectCommand = cmd;
                con.Open();
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string lit_Error = ex.Message;
                    ds = null;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            //
            // Result is a dataset to be used in a gridview control
            //
            return ds;
        }

        public string GetMLCText(string Project_Language, string Page, int Language_entered, int OrderNumber)
        {
            string MLCText = "No text found";
            int Language = Language_entered;
            string Translation_code = " ";
            string Project = Project_Language.Substring(0, 2);
            string cs = Connection_String_MLC();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetText", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Project", SqlDbType.Text).Value = Project;
                cmd.Parameters.Add("@Functionality", SqlDbType.Text).Value = Page;
                cmd.Parameters.Add("@Language_Code", SqlDbType.Int).Value = Language_entered;
                cmd.Parameters.Add("@TextOrder", SqlDbType.Int).Value = OrderNumber;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MLCText = Convert.ToString(reader["TextPart"]);
                    }
                }
                else
                {
                    con.Close();

                    SqlCommand cmd2 = new SqlCommand("spSetText", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    SqlParameter parameterProject2 = new SqlParameter();
                    parameterProject2.ParameterName = "@Project";
                    parameterProject2.Value = Project.Trim();
                    cmd2.Parameters.Add(parameterProject2);

                    SqlParameter parameterPage2 = new SqlParameter();
                    parameterPage2.ParameterName = "@Functionality";
                    parameterPage2.Value = Page.Trim();
                    cmd2.Parameters.Add(parameterPage2);

                    SqlParameter parameterLanguage2 = new SqlParameter();
                    parameterLanguage2.ParameterName = "@Language_Code";
                    parameterLanguage2.Value = Language;
                    cmd2.Parameters.Add(parameterLanguage2);

                    SqlParameter parameterTextOrder2 = new SqlParameter();
                    parameterTextOrder2.ParameterName = "@TextOrder";
                    parameterTextOrder2.Value = OrderNumber;
                    cmd2.Parameters.Add(parameterTextOrder2);

                    SqlParameter parameterText = new SqlParameter();
                    parameterText.ParameterName = "@Text";
                    parameterText.Value = Page + '-' + OrderNumber.ToString();
                    cmd2.Parameters.Add(parameterText);

                    MLCText = Page + '-' + OrderNumber.ToString();

                    con.Open();
                    cmd2.ExecuteNonQuery();
                    con.Close();
                }
            }
            string MLCText_Return = MLCText;

            return MLCText_Return;
        }
    }
}