namespace PEAssemblyReader
{
    using System.Collections.Generic;
    using System.Reflection;

    public interface IParameter
    {
        string Name { get; }

        IType ParameterType { get; }
    }
}
