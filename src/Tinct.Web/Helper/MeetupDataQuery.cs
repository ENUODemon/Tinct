using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Net.MasterPointConsole
{
    public class MeetupDataQuery
    {
        public static string connectstring = ConfigurationManager.ConnectionStrings["Meetup"].ConnectionString;
        public static bool SaveExecuteTime()
        {
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    string query = string.Format("update [dbo].[0127_Meetup_LastCrawledTime] set LastCrawledTime=getutcdate()");
                    con.Open();
                    SqlCommand comm = new SqlCommand(query, con);
                    int result = comm.ExecuteNonQuery();
                    if (result == -1)
                    {
                        return false;
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
            return true;
        }
        public static int GetDataCount(string RevelantTable)
        {
            int count = 0;
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    string query = string.Format("select count(0) totalcount from {0}", RevelantTable);
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
