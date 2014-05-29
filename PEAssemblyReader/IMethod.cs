namespace PEAssemblyReader
{
    using System.Collections.Generic;
    using System.Reflection;

    public interface IMethod
    {
        string Name { get; }

        IType ReturnType { get; }

        IEnumerable<IParam> GetParameters();

        IMethodBody GetMethodBody();
    }
}
