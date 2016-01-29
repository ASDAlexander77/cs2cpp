namespace Il2Native.Logic.DOM.Synthesized
{
    using Microsoft.CodeAnalysis;

    public class CCodeCopyConstructorDeclaration : CCodeDeclaration
    {
        private INamedTypeSymbol _type;

        public CCodeCopyConstructorDeclaration(INamedTypeSymbol type)
        {
            _type = type;
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.WriteTypeName(_type);
            c.TextSpan("(");
            c.WriteType(_type);
            c.WhiteSpace();
            c.TextSpan("__");
            c.TextSpan(_type.MetadataName.CleanUpName().ToLowerInvariant());
            c.TextSpan(")");
            c.WhiteSpace();
            c.TextSpan(":");
            c.WhiteSpace();
            c.WriteTypeName(_type);
            c.TextSpan("(");
            if (!_type.IsValueType)
            {
                c.TextSpan("*");
            }

            c.TextSpan("__");
            c.TextSpan(_type.MetadataName.CleanUpName().ToLowerInvariant());
            c.TextSpan(")");
            c.WhiteSpace();
            c.TextSpanNewLine("{}");
        }
    }
}
