namespace Il2Native.Logic
{
    using PEAssemblyReader;

    public interface ITypeResolver
    {
        /// <summary>
        /// </summary>
        IType ResolveType(string fullTypeName);
    }
}
