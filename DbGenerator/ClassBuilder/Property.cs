using System.Collections.Generic;
using System.Data;

namespace DbGenerator.ClassBuilder
{
    public class Property
    {
        private static readonly Dictionary<string[], string> TypeMaps = new Dictionary<string[], string>
        {
            { new [] { "bigint" }, "long" },
            { new [] { "binary", "image", "rowversion", "timestamp", "varbinary" }, "byte[]" },
            { new [] { "bit" }, "bool" },
            { new [] { "date", "datetime", "datetime2", "smalldatetime" }, "DateTime" },
            { new [] { "char", "nchar", "ntext", "text", "varchar", "nvarchar", "sysname" }, "string" },
            { new [] { "datetimeoffset" }, "DateTimeOffset" },
            { new [] { "decimal", "money", "smallmoney", "numeric" }, "decimal" },
            { new [] { "float" }, "double" },
            { new [] { "int" }, "int" },
            { new [] { "real" }, "Single" },
            { new [] { "smallint" }, "short" },
            { new [] { "tinyint" }, "byte" },
            { new [] { "sql_variant" }, "object" },
            { new [] { "time" }, "TimeSpan" },
            { new [] { "uniqueidentifier" }, "Guid" },
        };
        
        private static readonly Dictionary<string[], DbType> ProviderTypes = new Dictionary<string[], DbType>
        {
            { new [] {  "varchar", "text" }, DbType.AnsiString },
            { new [] { "binary", "image", "rowversion", "varbinary" }, DbType.Binary },
            // Byte doesn't exist in SQL Server. Try SByte instead
            { new [] { "bit" }, DbType.Boolean },
            { new [] { "money", "smallmoney" }, DbType.Currency },
            { new [] { "date"  }, DbType.Date },
            { new [] { "datetime", "smalldatetime" }, DbType.DateTime },
            { new [] { "decimal", "numeric" }, DbType.Decimal },
            { new [] { "float" }, DbType.Double },
            { new [] { "uniqueidentifier" },  DbType.Guid },
            { new [] { "smallint" }, DbType.Int16 }, 
            { new [] { "int" }, DbType.Int32  },
            { new [] { "bigint" }, DbType.Int64 },
            { new [] { "sql_variant" }, DbType.Object },
            { new [] { "tinyint" }, DbType.SByte },
            { new [] { "float" }, DbType.Single },
            { new [] { "nvarchar", "ntext" }, DbType.String },
            { new [] { "time" }, DbType.Time },
            // UInts are not supported by SQL server
            // TODO: find out what VarNumeric is for
            { new [] { "char" }, DbType.AnsiStringFixedLength },
            { new [] { "nchar" }, DbType.StringFixedLength },
            // Implement XML type if needed
            { new [] { "datetime2" }, DbType.DateTime2 },
            { new [] { "datetimeoffset" }, DbType.DateTimeOffset }
        };
        
        private static readonly Dictionary<string, string> TypeMap = new Dictionary<string, string>();
        private static readonly Dictionary<string, DbType> DbTypeMap = new Dictionary<string, DbType>();

        static Property()
        {
            TypeMap = MapLoop(TypeMaps);
            DbTypeMap = MapLoop(ProviderTypes);
        }

        private static Dictionary<string, T> MapLoop<T>(Dictionary<string[], T> input)
        {
            var ret = new Dictionary<string, T>();
            foreach (var (keys, value) in input)
            foreach (var key in keys)
                ret[key] = value;
            return ret;
        }

        public Property(string name, string dbType)
        {
            ProperName = name.PascalCase();
            Name = name;
            Type = TypeMap[dbType];
            DbType = DbTypeMap[dbType];
        }

        public string Name { get; }
        public string ProperName { get; }

        public string Type { get;  }
        public short? MaxLength { get; set; }
        public int? MinLength { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
        public bool IsNullable { get; set; }
        public bool IsComputed { get; set; }
        public bool IsRequired { get; set; }
        public bool IsFileStream { get; set; }
        public bool IsOutput { get; set; }
        public DbType DbType { get; set; }
        
    }
}