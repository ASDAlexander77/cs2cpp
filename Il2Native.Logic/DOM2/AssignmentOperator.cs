namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class AssignmentOperator : Expression
    {
        private BinaryOperatorKind operatorKind;
        private Expression left;
        private Expression right;

        internal void Parse(BoundAssignmentOperator boundAssignmentOperator)
        {
            if (boundAssignmentOperator == null)
            {
                throw new ArgumentNullException();
            }

            this.left = Deserialize(boundAssignmentOperator.Left) as Expression;
            Debug.Assert(this.left != null);
            this.right = Deserialize(boundAssignmentOperator.Right) as Expression;
            Debug.Assert(this.right != null);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this.left.WriteTo(c);
            c.WhiteSpace();
            c.TextSpan("=");
            c.WhiteSpace();
            this.right.WriteTo(c);
        }
    }
}
