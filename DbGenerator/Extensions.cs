using System.Data;
using System.Data.SqlClient;

namespace DbGenerator
{
    public static class Extensions
    {
        public static DataTable GetData(this SqlConnection conn, string query)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            using var data = cmd.ExecuteReader();
            var table = new DataTable();
            table.Load(data);
            return table;
        }
        
        
    }
}