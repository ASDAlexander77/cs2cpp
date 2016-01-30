namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class PostfixUnaryExpression : PrefixPostfixUnaryExpressionBase
    {
        internal override void WriteTo(CCodeWriterBase c)
        {
            Value.WriteTo(c);
            switch (OperatorKind)
            {
                case SyntaxKind.PlusPlusToken:
                    c.TextSpan("++");
                    break;
                case SyntaxKind.MinusMinusToken:
                    c.TextSpan("--");
                    break;
            }
        }
    }
}
