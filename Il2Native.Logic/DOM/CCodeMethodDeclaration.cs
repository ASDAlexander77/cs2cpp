namespace Il2Native.Logic.DOM
{
    using Microsoft.CodeAnalysis;

    public class CCodeMethodDeclaration : CCodeDeclaration
    {
        public CCodeMethodDeclaration(IMethodSymbol metadataName)
        {
            this.MetadataName = metadataName;
        }

        public IMethodSymbol MetadataName { get; set; }
    }
}
