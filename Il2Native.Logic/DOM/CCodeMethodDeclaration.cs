namespace Il2Native.Logic.DOM
{
    using Microsoft.CodeAnalysis;

    public class CCodeMethodDeclaration : CCodeDeclaration
    {
        public CCodeMethodDeclaration(IMethodSymbol method)
        {
            this.Method = method;
        }

        public IMethodSymbol Method { get; set; }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.WriteMethodDeclaration(this.Method, true);
            c.EndStatement();
        }
    }
}
