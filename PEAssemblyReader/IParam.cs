namespace PEAssemblyReader
{
    using System.Collections.Generic;
    using System.Reflection;

    public interface IParam
    {
        string Name { get; }

        IType ParameterType { get; }
    }
}
