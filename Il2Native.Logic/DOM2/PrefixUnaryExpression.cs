namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Linq;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax.InternalSyntax;

    public class PrefixUnaryExpression : Expression
    {
        private Expression value;

        private SyntaxKind operatorKind;

        internal void Parse(BoundSequence boundSequence)
        {
            if (boundSequence == null)
            {
                throw new ArgumentNullException();
            }

            var boundAssignmentOperator = boundSequence.SideEffects.First() as BoundAssignmentOperator;
            if (boundAssignmentOperator != null)
            {
                this.value = Deserialize(boundAssignmentOperator.Left) as Expression;
            }

            var prefixUnaryExpressionSyntax = boundSequence.Syntax.Green as PrefixUnaryExpressionSyntax;
            if (prefixUnaryExpressionSyntax != null)
            {
                this.operatorKind = prefixUnaryExpressionSyntax.OperatorToken.Kind;
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            switch (this.operatorKind)
            {
                case SyntaxKind.PlusPlusToken:
                    c.TextSpan("++");
                    break;
                case SyntaxKind.MinusMinusToken:
                    c.TextSpan("--");
                    break;
            }

            this.value.WriteTo(c);
        }
    }
}
