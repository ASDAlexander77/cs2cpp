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

        public override bool IsGeneric
        {
            get { return this.Method.ContainingType.IsGenericType || this.Method.IsGenericMethod; }
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.Separate();
            c.TextSpanNewLine(string.Format("// Method: {0}", this.Method.ToDisplayString()));

            c.WriteMethodDeclaration(this.Method, false);
            c.WriteMethodBody(this.BoundBody, this.Method);
        }
    }
}
