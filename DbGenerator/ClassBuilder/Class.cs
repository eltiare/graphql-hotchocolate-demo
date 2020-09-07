using System.Collections.Generic;

namespace DbGenerator.ClassBuilder
{
    public class Class
    {
        public string Name { get; }
        public List<Property> Properties { get; set; } = new List<Property>();
        public List<(string name, object[] args)>? Attributes { get; set; }
        public bool IsView { get; set; }
        
        public Class(string name)
        {
            Name = name;
        }
    }
}