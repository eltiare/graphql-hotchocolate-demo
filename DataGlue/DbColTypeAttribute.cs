using System;
using System.Data;

namespace DataGlue
{
    public class DbColTypeAttribute : Attribute
    {
        public DbType DbType { get; }

        public DbColTypeAttribute(DbType dbType)
        {
            DbType = dbType;
        }
    }
}