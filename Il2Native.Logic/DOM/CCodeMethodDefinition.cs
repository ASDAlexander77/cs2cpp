namespace Il2Native.Logic.DOM
{
    using Il2Native.Logic.DOM2;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class CCodeMethodDefinition : CCodeDefinition
    {
        internal CCodeMethodDefinition(IMethodSymbol method)
        {
            this.Method = method;
        }

        public IMethodSymbol Method { get; set; }

        public MethodBody MethodBodyOpt { get; set; }

        internal BoundStatement BoundBody { get; set; }

        public override bool IsGeneric
        {
            get { return this.Method.ContainingType.IsGenericType || (this.Method.IsGenericMethod && !this.Method.IsVirtualGenericMethod()); }
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.Separate();
            c.TextSpanNewLine(string.Format("// Method : {0}", this.Method.ToDisplayString()));

            c.WriteMethodDeclaration(this.Method, false);

            if (MethodBodyOpt != null)
            {
                MethodBodyOpt.WriteTo(c);
            }
            else
            {
                c.WriteMethodBody(this.BoundBody, this.Method);
            }
        }
    }
}
