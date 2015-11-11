namespace Il2Native.Logic
{
    using PEAssemblyReader;

    public interface ITypeResolver
    {
        SystemTypes System { get; }

        IType ResolveType(string fullTypeName, IGenericContext genericContext = null);
    }
}
