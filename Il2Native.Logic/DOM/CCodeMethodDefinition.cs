namespace Il2Native.Logic.DOM
{
    using System.CodeDom.Compiler;
    using Microsoft.CodeAnalysis;

    public class CCodeMethodDefinition : CCodeDefinition
    {
        public CCodeMethodDefinition(IMethodSymbol method)
        {
            this.Method = method;
        }

        public IMethodSymbol Method { get; set; }

        public override void WriteTo(IndentedTextWriter itw)
        {
            itw.WriteLine();

            itw.WriteLine("// Method: {0}", this.Method.ToDisplayString());
        }
    }
}
