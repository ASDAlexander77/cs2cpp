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

        internal bool Parse(BoundSequence boundSequence)
        {
            base.Parse(boundSequence);

            if (boundSequence.SideEffects.Length > 2 || !boundSequence.SideEffects.All(se => se is BoundAssignmentOperator))
            {
                return false;
            }

            var boundAssignmentOperator = boundSequence.SideEffects.First() as BoundAssignmentOperator;
            if (boundAssignmentOperator != null)
            {
                this.value = Deserialize(boundAssignmentOperator.Right) as Expression;
            }

            var prefixUnaryExpressionSyntax = boundSequence.Syntax.Green as PrefixUnaryExpressionSyntax;
            if (prefixUnaryExpressionSyntax != null)
            {
                this.operatorKind = prefixUnaryExpressionSyntax.OperatorToken.Kind;
            }

            var call = value as Call;
            if (call != null && (call.Method.Name == "op_Increment" || call.Method.Name == "op_Decrement"))
            {
                return false;
            }

            if (value == null)
            {
                return false;
            }

            return true;
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
