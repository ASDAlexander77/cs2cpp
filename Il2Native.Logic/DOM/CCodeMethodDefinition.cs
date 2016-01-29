namespace Il2Native.Logic.DOM
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

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
            c.TextSpanNewLine(string.Format("// MethodBodyOpt: {0}", this.Method.ToDisplayString()));

            c.WriteMethodDeclaration(this.Method, false);
            c.WriteMethodBody(this.BoundBody, this.Method);
        }
    }
}
