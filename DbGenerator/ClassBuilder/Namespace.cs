using System.Collections.Generic;

namespace DbGenerator.ClassBuilder
{
    public class Namespace
    {
        public string Name { get; }
        public List<(string name, object[] args)>? Attributes { get; set; }
        public List<string>? Usings { get; set; }
        public List<Class>? Classes { get; set; }
        public Namespace(string name)
        {
            Name = name;
        }
    }
}