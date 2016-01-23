namespace Il2Native.Logic.DOM
{
    using System.CodeDom.Compiler;
    using Microsoft.CodeAnalysis;

    public class CCodeMethodDeclaration : CCodeDeclaration
    {
        public CCodeMethodDeclaration(IMethodSymbol method)
        {
            this.Method = method;
        }

        public IMethodSymbol Method { get; set; }

        public override void WriteTo(CCodeWriter c)
        {
            c.WriteMethodDeclaration(this.Method, true);
            c.WriteLine(";");
        }
    }
}
