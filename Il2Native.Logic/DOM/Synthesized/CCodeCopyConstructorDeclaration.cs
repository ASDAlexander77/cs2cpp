namespace Il2Native.Logic.DOM.Synthesized
{
    using Microsoft.CodeAnalysis;

    public class CCodeCopyConstructorDeclaration : CCodeMethodDeclaration
    {
        public CCodeCopyConstructorDeclaration(IMethodSymbol method)
            : base(method)
        {
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.WriteMethodPrefixesAndName(Method, true);
            c.TextSpan("(");
            c.WriteType(Method.ContainingType);
            c.WhiteSpace();
            c.TextSpan("cpy)");
            c.WriteMethodSuffixes(Method, true);
            c.WhiteSpace();
            c.TextSpan(":");
            c.WhiteSpace();
            c.WriteTypeName(Method.ContainingType);
            c.TextSpan("(");
            c.TextSpan("*cpy)");
            c.WhiteSpace();
            c.TextSpanNewLine("{}");
        }
    }
}
