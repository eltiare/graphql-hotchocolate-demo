using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DbGenerator
{
    public static class Extensions
    {
        
        private static readonly Regex PascalSpaceMatcher = new Regex(@"[\W\p{P}\d]+");
        private static readonly Regex PascalUpperMatcher = new Regex(@"(\p{Ll})(\p{Lu})");
        
        public static DataTable GetData(this SqlConnection conn, string query)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            using var data = cmd.ExecuteReader();
            var table = new DataTable();
            table.Load(data);
            return table;
        }
        
        public static string PascalCase(this string orig)
        {
            var replace = PascalSpaceMatcher.Replace(orig, " ");
            replace = PascalUpperMatcher.Replace(replace, "$1 $2");
            var ti = new CultureInfo("en-US",false).TextInfo;
            return ti.ToTitleCase(replace).Replace(" ", string.Empty);
        }
    }
}