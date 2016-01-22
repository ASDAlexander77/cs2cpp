namespace Il2Native.Logic.DOM
{
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.Reflection.Metadata.Ecma335;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    public class CCodeMethodDefinition : CCodeDefinition
    {
        internal CCodeMethodDefinition(IMethodSymbol method, BoundStatement boundBody)
        {
            this.Method = method;
            this.BoundBody = boundBody;
        }

        public IMethodSymbol Method { get; set; }

        internal BoundStatement BoundBody { get; set; }

        public override void WriteTo(IndentedTextWriter itw)
        {
            itw.WriteLine();

            itw.WriteLine("// Method: {0}", this.Method.ToDisplayString());

            // pre attributes
            // TODO:

            CCodeSerializer.WriteMethodDeclaration(itw, this.Method, false);

            CCodeSerializer.WriteMethodBody(itw, this.BoundBody, this.Method);
        }
    }
}
