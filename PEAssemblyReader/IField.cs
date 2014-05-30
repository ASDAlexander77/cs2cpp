namespace PEAssemblyReader
{
    using System.Collections.Generic;
    using System.Reflection;

    public interface IField : IMember
    {
        IType FieldType { get; }

        bool IsLiteral { get; }
    }
}
