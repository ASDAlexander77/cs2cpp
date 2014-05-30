namespace PEAssemblyReader
{
    using System;

    public interface IModule
    {
        string ResolveString(int token);

        IMember ResolveMember(int token, IType[] typeGenerics, IType[] methodGenerics);

        IMethod ResolveMethod(int token, IType[] typeGenerics, IType[] methodGenerics);

        IField ResolveField(int token, IType[] typeGenerics, IType[] methodGenerics);

        IType ResolveType(int token, IType[] typeGenerics, IType[] methodGenerics);
    }
}
