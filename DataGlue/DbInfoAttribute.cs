using System;

namespace DataGlue
{
    public enum DbSystemType {  Table, View, Procedure }
    
    public class DbInfoAttribute : Attribute
    {

        public string Name { get; }
        public DbSystemType DbSystemType { get; }
        
        public DbInfoAttribute(string name, DbSystemType type)
        {
            Name = name;
            DbSystemType = type;
        }
    }
}