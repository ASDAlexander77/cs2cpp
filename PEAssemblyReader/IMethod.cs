namespace PEAssemblyReader
{
    using System.Collections.Generic;
    using System.Reflection;

    public interface IMethod : IMember, IMethodBody
    {
        IType ReturnType { get; }

        bool IsGenericMethod { get; }

        bool IsConstructor { get; }

        CallingConventions CallingConvention { get; }

        IEnumerable<IParameter> GetParameters();

        IMethodBody GetMethodBody();

        IEnumerable<IType> GetGenericArguments();
    }
}
