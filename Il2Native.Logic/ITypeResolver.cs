namespace Il2Native.Logic
{
    using PEAssemblyReader;

    public interface ITypeResolver
    {
        SystemTypes System { get; }

        string GetAllocator(bool isAtomicAllocation, bool isBigObj);

        IType ResolveType(string fullTypeName, IGenericContext genericContext = null);
    }
}
