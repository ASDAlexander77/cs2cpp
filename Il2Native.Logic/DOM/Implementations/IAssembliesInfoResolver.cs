namespace Il2Native.Logic.DOM.Implementations
{
    using Microsoft.CodeAnalysis;

    public interface IAssembliesInfoResolver
    {
        ITypeSymbol GetType(string name);
    }
}
