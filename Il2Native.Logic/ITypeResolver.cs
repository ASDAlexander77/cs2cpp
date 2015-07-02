namespace Il2Native.Logic
{
    using PEAssemblyReader;

    public interface ITypeResolver
    {
        SystemTypes System { get; }

        // TODO: should be in ICodeWriter
        bool GetGcSupport();

        // TODO: should be in ICodeWriter
        bool GetMultiThreadingSupport();

        // TODO: should be in ICodeWriter
        string GetAllocator(bool isAtomicAllocation, bool isBigObj);

        IType ResolveType(string fullTypeName, IGenericContext genericContext = null);

        void RegisterType(IType type);
    }
}
