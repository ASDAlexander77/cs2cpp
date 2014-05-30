namespace PEAssemblyReader
{
    using System;

    public interface IName : IComparable
    {
        string Name { get; }

        string Namespace { get; }

        string FullName { get; }

        string AssemblyQualifiedName { get; }

        IType DeclaringType { get; }
    }
}
