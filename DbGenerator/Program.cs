using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DataGlue;
using DbGenerator.ClassBuilder;

namespace DbGenerator
{
    class Program
    {
        
        private static readonly Regex SkipTables = new Regex(@"^(spt|MSreplication)_");
        private static readonly string[] SkipTablesArr = { "SYSTEM_TABLE", "INTERNAL_TABLE" };
        private static readonly Regex SkipProcs = new Regex(@"^sp_MS");

        private const string DataDir = "DataModels";

        private const string ColumnQuery = @"
                    SELECT schema_name(o.schema_id) as schema_name,
                           object_name(c.object_id) as table_name,
                           o.type_desc as table_type,
                           c.name as column_name,
                           type_name(user_type_id) as data_type,
                           dc.definition as default_value,
                           is_output = CAST(0 as bit),
                           is_readonly = CAST(0 as bit),
                           c.*
                    FROM sys.columns c
                    INNER JOIN sys.objects o ON o.object_id = c.object_id
                    LEFT JOIN sys.default_constraints dc ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
                    ORDER BY schema_name,
                             table_name,
                             column_id;";

        private const string ProcQuery = @"
                    SELECT name AS column_name,
                           proc_name = object_name(object_id),
                           data_type = type_name(user_type_id),
                           default_value,
                           max_length,
                           precision,
                           scale,
                           is_output,
                           is_readonly,
                           is_nullable,
                           is_filestream = CAST(0 as bit),
                           is_computed = CAST(0 as bit),
                           parameter_id,
                           system_type_id
                    FROM sys.parameters
                    ORDER BY object_id,
                             parameter_id;";
        
        private static string _baseDir = null!;
        private static string _baseNamespace = null!;
        
        static void Main(string[] args)
        {
            
            // Set up variables
            if (args.Length != 3)
                throw new ArgumentException("Must pass in connection string, project directory, and base namespace");
            
            var builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = args[0]; // parses string and checks for errors 
            
            _baseDir = Path.Join(args[1], DataDir);
            _baseNamespace = $"{args[2]}.DataModels";
            
            // Empty data model directory
            var di = new DirectoryInfo(_baseDir);
            if (di.Exists)
            {
                foreach (var file in di.GetFiles())
                    file.Delete();
                foreach (var dir in di.GetDirectories())
                    dir.Delete(true);    
            }
            else
            {
                Directory.CreateDirectory(_baseDir);
            }
            
            // Set up SQL connection
            using var conn = new SqlConnection(builder.ConnectionString);
            conn.Open();
            
            // Get column information for tables + views
            var columnInfo = conn.GetData(ColumnQuery);
            var tables = new Dictionary<string, Class>();
            foreach (DataRow row in columnInfo.Rows)
            {
                var tableName = row.Field<string>("table_name");
                var tableType = row.Field<string>("table_type");
                if (SkipTablesArr.Contains(tableType) || SkipTables.IsMatch(tableName))
                    continue;
                var isView = tableType == "VIEW";
                if (!tables.ContainsKey(tableName))
                    tables.Add(tableName, new Class(tableName, isView ? DbSystemType.View : DbSystemType.Table)
                    {
                        IsView = isView
                    });
                tables[tableName].Properties.Add(MakeProperty(row));
            }
            
            // Get parameter information for procs
            var procInfo = conn.GetData(ProcQuery);
            var procs = new Dictionary<string, Class>();
            foreach (DataRow row in procInfo.Rows)
            {
                var procName = row.Field<string>("proc_name");
                if (SkipProcs.IsMatch(procName))
                    continue;
                if (!procs.ContainsKey(procName))
                    procs.Add(procName, new Class(procName, DbSystemType.Procedure));
                procs[procName].Properties.Add(MakeProperty(row));
            }
            
            var @namespace = new Namespace(string.Empty)
            {
                Usings = new List<string> { "System", "System.Data", "System.Collections.Generic", "System.ComponentModel.DataAnnotations", "System.ComponentModel.DataAnnotations.Schema", "DataGlue" }
            };
            foreach (var @class in tables.Values)
            {   
                var prefix = @class.IsView ? "Views" : "Tables";
                @namespace.Classes = new List<Class> { @class };
                WriteNamespace(@namespace, prefix, $"{ @class.ProperName }.cs");
            }

            const string procPrefix = "Procedures";
            foreach (var proc in procs.Values)
            {
                @namespace.Classes = new List<Class> {proc};
                WriteNamespace(@namespace, procPrefix, $"{ proc.ProperName }.cs");
            }
        }

        private static Property MakeProperty(DataRow row)
        {
            var isNullable = row.Field<bool>("is_nullable");;
            var isComputed = row.Field<bool?>("is_computed") ?? false;
            var hasDefault = row.Field<string?>("default_value") != null;
            return new Property(row.Field<string>("column_name"), row.Field<string>("data_type"))
            {
                IsFileStream = row.Field<bool>("is_filestream"),
                IsRequired = !isNullable && !hasDefault && !isComputed,
                IsNullable = isNullable,
                MaxLength = row.Field<short>("max_length"),
                IsOutput = row.Field<bool>("is_output")
            };   
        }

        private static void WriteNamespace(Namespace @namespace, string prefix, string fileName)
        {
            @namespace.Name = $"{_baseNamespace}.{prefix}";
            var code = ClassFactory.CreateCode(@namespace);
            WriteCode(Path.Join(prefix, fileName), code);
        }

        private static void WriteCode(string path, string code)
        {
            var fullPath = Path.Join(_baseDir, path);
            var dirName = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);
            File.WriteAllText(fullPath, code);
        }
        
    }
}