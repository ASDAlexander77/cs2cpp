namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Syntax.InternalSyntax;

    public abstract class PrefixPostfixUnaryExpressionBase : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.PrefixPostfixUnaryExpressionBase; }
        }

        public Expression Value { get; set; }

        public SyntaxKind OperatorKind { get; set; }

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
                var boundExpression = FindValue(boundAssignmentOperator.Left, boundAssignmentOperator.Right);
                if (boundExpression == null)
                {
                    return false;
                } 
                
                this.Value = Deserialize(boundExpression) as Expression;
            }

            var prefixUnaryExpressionSyntax = boundSequence.Syntax.Green as PrefixUnaryExpressionSyntax;
            if (prefixUnaryExpressionSyntax != null)
            {
                this.OperatorKind = prefixUnaryExpressionSyntax.OperatorToken.Kind;
            }
            else
            {
                var postfixUnaryExpressionSyntax = boundSequence.Syntax.Green as PostfixUnaryExpressionSyntax;
                if (postfixUnaryExpressionSyntax != null)
                {
                    this.OperatorKind = postfixUnaryExpressionSyntax.OperatorToken.Kind;
                }
            }

            Debug.Assert(this.OperatorKind != SyntaxKind.None);

            if (this.Value.Type.OriginalDefinition != null && this.Value.Type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
            {
                return false;
            }
            
            return true;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Value.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            bool changed;
            this.Value = AdjustEnumType(this.Value, out changed, true);
        }

        private static BoundExpression FindValue(BoundExpression left, BoundExpression right)
        {
            return CheckValue(left) ?? CheckValue(right);
        }

        private static BoundExpression CheckValue(BoundExpression left)
        {
            var boundLocal = left as BoundLocal;
            if (boundLocal != null && boundLocal.LocalSymbol.SynthesizedLocalKind == SynthesizedLocalKind.None)
            {
                return boundLocal;
            }

            var boundFieldAccess = left as BoundFieldAccess;
            if (boundFieldAccess != null)
            {
                return boundFieldAccess;
            }

            var boundParameter = left as BoundParameter;
            if (boundParameter != null)
            {
                return boundParameter;
            }

            var boundArrayAccess = left as BoundArrayAccess;
            if (boundArrayAccess != null)
            {
                return boundArrayAccess;
            }

            var boundBinaryOperator = left as BoundBinaryOperator;
            if (boundBinaryOperator != null)
            {
                return CheckValue(boundBinaryOperator.Left) ?? CheckValue(boundBinaryOperator.Right);
            }

            return null;
        }
    }
}
