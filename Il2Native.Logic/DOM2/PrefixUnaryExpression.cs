namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class PrefixUnaryExpression : PrefixPostfixUnaryExpressionBase
    {
        internal override void WriteTo(CCodeWriterBase c)
        {
            switch (OperatorKind)
            {
                case SyntaxKind.PlusPlusToken:
                    c.TextSpan("++");
                    break;
                case SyntaxKind.MinusMinusToken:
                    c.TextSpan("--");
                    break;
            }

            Value.WriteTo(c);
        }
    }
}
