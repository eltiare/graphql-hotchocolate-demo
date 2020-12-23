using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace HotChocolateServer
{
    public static class ObjectExtensions
    {
        public static async Task<IEnumerable<TR>> CallProcedure<TR, T>(this SqlConnection conn, T args)
            where TR : new()
        {
            await using var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            var type = args.GetType();
            foreach (var prop in type.GetProperties())
            {
                var attr = prop.GetCustomAttribute<ColumnAttribute>();
                var paramName = attr?.Name ?? prop.Name;
                var param = cmd.Parameters.Add(paramName, SqlDbType.Binary);
                param.Value = prop.GetValue(args);
            }
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();
            
            // TODO: return real results
            return new List<TR>();
        }
    }
}