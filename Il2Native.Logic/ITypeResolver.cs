namespace Il2Native.Logic
{
    using PEAssemblyReader;

    public interface ITypeResolver
    {
        /// <summary>
        /// </summary>
        IType ResolveType(string fullTypeName, IGenericContext genericContext = null);

        SystemTypes System { get; }
    }
}
