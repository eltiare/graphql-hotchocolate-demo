using System.Collections.Generic;

namespace DbGenerator.ClassBuilder
{
    public class Property
    {
        private static readonly Dictionary<string[], string> DbTypes = new Dictionary<string[], string>
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
        
        private static readonly Dictionary<string, string> TypeMap = new Dictionary<string, string>();

        static Property()
        {
            foreach (var (dbTypes, cValue) in DbTypes)
            foreach (var dbType in dbTypes)
                TypeMap[dbType] = cValue;
        }

        public Property(string name, string dbType)
        {
            Name = name;
            Type = TypeMap[dbType];
        }

        public string Name { get; }
        public string Type { get;  }
        public short? MaxLength { get; set; }
        public int? MinLength { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
        public bool IsNullable { get; set; }
        public bool IsComputed { get; set; }
        public bool IsRequired { get; set; }
        public bool IsFileStream { get; set; }
        
    }
}