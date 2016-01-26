namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Linq;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax.InternalSyntax;

    public class PostfixUnaryExpression : Expression
    {
        private Expression value;

        private SyntaxKind operatorKind;

        internal void Parse(BoundSequence boundSequence)
        {
            if (boundSequence == null)
            {
                throw new ArgumentNullException();
            }

            var boundAssignmentOperator = boundSequence.SideEffects.Skip(1).First() as BoundAssignmentOperator;
            if (boundAssignmentOperator != null)
            {
                this.value = Deserialize(boundAssignmentOperator.Left) as Expression;
            }
            
            var postfixUnaryExpressionSyntax = boundSequence.Syntax.Green as PostfixUnaryExpressionSyntax;
            if (postfixUnaryExpressionSyntax != null)
            {
                this.operatorKind = postfixUnaryExpressionSyntax.OperatorToken.Kind;
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this.value.WriteTo(c);
            switch (this.operatorKind)
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
