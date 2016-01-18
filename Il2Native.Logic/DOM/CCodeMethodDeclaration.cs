namespace Il2Native.Logic.DOM
{
    using System.CodeDom.Compiler;
    using Microsoft.CodeAnalysis;

    public class CCodeMethodDeclaration : CCodeDeclaration
    {
        public CCodeMethodDeclaration(IMethodSymbol metadataName)
        {
            this.MetadataName = metadataName;
        }

        public IMethodSymbol MetadataName { get; set; }

        public override void WriteTo(IndentedTextWriter itw, WriteSettings settings)
        {
            throw new System.NotImplementedException();
        }
    }
}
