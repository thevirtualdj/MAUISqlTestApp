using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
 

namespace MAUISqlApp
{
    public static class TringSQL
    {
        public static String GetStringFromDB()
        {
            //WORKS JUST FINE ON SAME SERVER THAT FORCES ENCRYPTION WHEN RUN ON WINDOWS
            //string connectionString = @"Server=YOURSERVER;Database=YOUR_DBHERE;User Id=sa;Password=YOUR_PASSWORD_HERE;integrated security=True;Encrypt=False;TrustServerCertificate=True;MultiSubnetFailover=true";
            string connectionString = @"Server=192.168.0.15,1433;Database=YOURDB;User Id=sa;Password=YOURPASS;Persist Security Info=True;Encrypt=True;TrustServerCertificate=True";
            string sql = "SELECT RAND()";
            string ret = "";
            SqlConnection Connection = new SqlConnection(connectionString);
            
            ConnectionState ps = Connection.State;
            var tbl = new DataTable();
            var cmd = new SqlDataAdapter(sql, Connection);
            cmd.SelectCommand.CommandTimeout = 0;
            try
            {
                if (ps != ConnectionState.Open)
                    //Here it will fail to open TLS connection on Android! 
                    Connection.Open();
                cmd.Fill(tbl);
            }
            catch (DbException ex)
            {
                //error message is:
                //A connection was successfully established with the server, but then an error
                //occurred during the pre-login handshake. (provider: TCP Provider, error: 35 - An internal exception was caught)
                ret = ex.Message;
            }
            finally
            {
                cmd = default;
                if (ps == ConnectionState.Closed)
                    Connection.Close();
            }
            if (tbl.Rows.Count > 0)
            {
                ret = tbl.Rows[0][0].ToString();
            }
            return ret;
        }
    }
}