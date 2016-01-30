namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Syntax.InternalSyntax;

    public abstract class PrefixPostfixUnaryExpressionBase : Expression
    {
        public Expression Value { get; protected set; }

        public SyntaxKind OperatorKind { get; protected set; }

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
                this.Value = Deserialize(FindValue(boundAssignmentOperator.Left, boundAssignmentOperator.Right)) as Expression;
            }

            var postfixUnaryExpressionSyntax = boundSequence.Syntax.Green as PostfixUnaryExpressionSyntax;
            if (postfixUnaryExpressionSyntax != null)
            {
                this.OperatorKind = postfixUnaryExpressionSyntax.OperatorToken.Kind;
            }

            var call = this.Value as Call;
            if (call != null && (call.Method.Name == "op_Increment" || call.Method.Name == "op_Decrement"))
            {
                return false;
            }

            if (this.Value == null)
            {
                return false;
            }

            return true;
        }

        private static BoundExpression FindValue(BoundExpression left, BoundExpression right)
        {
            var boundLocal = left as BoundLocal;
            if (boundLocal != null && boundLocal.LocalSymbol.SynthesizedLocalKind != SynthesizedLocalKind.None)
            {
                return right;
            }

            return left;
        }
    }
}
