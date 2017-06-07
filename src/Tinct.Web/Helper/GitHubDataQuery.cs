using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Net.MasterPointConsole
{
    public class GitHubDataQuery
    {
        public static string connectstring = ConfigurationManager.ConnectionStrings["GitHubNew"].ConnectionString;
        public static int GetDataCount(string RevelantTable)
        {
            int count = 0;
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    string query =string.Format("select count(0) totalcount from {0}",RevelantTable);             
                    con.Open();
                    SqlCommand comm = new SqlCommand(query, con);
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        int.TryParse(reader["totalcount"].ToString(), out count);
                    }

                }
                catch
                {
                    throw;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return count;
        }
    }
}
