using System;

namespace DAM.Core.Models.SolutionModels
{
    public class Property
    {
        public string Name { get; }
        public Type Type { get; }
        public string TypeName => Type.Name;

        public Property(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public override string ToString()
            => $"{Name} - {TypeName}";

        public override bool Equals(object obj)
        {
            return obj is Property property &&
                   property.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, TypeName, Type);
        }
    }
}
