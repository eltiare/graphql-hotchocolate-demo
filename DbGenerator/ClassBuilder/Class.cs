using System.Collections.Generic;
using DataGlue;

namespace DbGenerator.ClassBuilder
{
    public class Class
    {
        public string Name { get; }
        public string ProperName { get; }

        public List<Property> Properties { get; set; } = new List<Property>();
        public List<(string name, object[] args)> Attributes { get; }
        public bool IsView { get; set; }
        
        public Class(string name, DbSystemType type)
        {
            ProperName = name.PascalCase();
            Name = name;
            Attributes = new List<(string name, object[] args)>
            {
                ("DbInfo", new object[] { name, type })
            };
        }
    }
}