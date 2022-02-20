using System;
using System.Collections.Generic;
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

        public string GetMLCText(string Project, string Page, int TextId, int Language)
        {
            string MLCText = "No text found";

            string cs = ConfigurationManager.ConnectionStrings["DBMLC"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {

                SqlCommand cmd = new SqlCommand("spGetText", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parameterProject = new SqlParameter();
                parameterProject.ParameterName = "@Project";
                parameterProject.Value = Project.Trim();
                cmd.Parameters.Add(parameterProject);

                SqlParameter parameterPage = new SqlParameter();
                parameterPage.ParameterName = "@Functionality";
                parameterPage.Value = Page.Trim();
                cmd.Parameters.Add(parameterPage);

                SqlParameter parameterLanguage = new SqlParameter();
                parameterLanguage.ParameterName = "@Language_Code";
                parameterLanguage.Value = Language;
                cmd.Parameters.Add(parameterLanguage);

                SqlParameter parameterTextOrder = new SqlParameter();
                parameterTextOrder.ParameterName = "@TextOrder";
                parameterTextOrder.Value = TextId;
                cmd.Parameters.Add(parameterTextOrder);

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
                    SqlCommand cmd2 = new SqlCommand("spSetText", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter parameterProject2 = new SqlParameter();
                    parameterProject.ParameterName = "@Project";
                    parameterProject.Value = Project.Trim();
                    cmd.Parameters.Add(parameterProject);

                    SqlParameter parameterPage2 = new SqlParameter();
                    parameterPage.ParameterName = "@Functionality";
                    parameterPage.Value = Page.Trim();
                    cmd.Parameters.Add(parameterPage);

                    SqlParameter parameterLanguage2 = new SqlParameter();
                    parameterLanguage.ParameterName = "@Language_Code";
                    parameterLanguage.Value = Language;
                    cmd.Parameters.Add(parameterLanguage);

                    SqlParameter parameterTextOrder2 = new SqlParameter();
                    parameterTextOrder.ParameterName = "@TextOrder";
                    parameterTextOrder.Value = TextId;
                    cmd.Parameters.Add(parameterTextOrder);

                    SqlParameter parameterText = new SqlParameter();
                    parameterText.ParameterName = "@TextOrder";
                    parameterText.Value = Page + '-' + TextId.ToString();
                    cmd.Parameters.Add(parameterText);

                    MLCText = Page + '-' + TextId.ToString();

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return MLCText;
        }
    }
}