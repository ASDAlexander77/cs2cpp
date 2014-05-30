namespace PEAssemblyReader
{
    using Microsoft.CodeAnalysis;

    public interface IMember : IName
    {
        bool IsStatic { get; }

        bool IsAbstract { get; }

        bool IsVirtual { get; }

        IModule Module { get; }
    }
}
