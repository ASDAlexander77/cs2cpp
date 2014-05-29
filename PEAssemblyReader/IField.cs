namespace PEAssemblyReader
{
    using System.Collections.Generic;
    using System.Reflection;

    public interface IField
    {
        string Name { get; }

        IType FieldType { get; }
    }
}
