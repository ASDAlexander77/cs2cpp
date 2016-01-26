namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Syntax.InternalSyntax;

    public class AssignmentOperator : Expression
    {
        private BinaryOperatorKind operatorKind;
        private Expression left;
        private Expression right;

        public bool ApplyAutoType { get; set; }

        internal void Parse(BoundAssignmentOperator boundAssignmentOperator)
        {
            if (boundAssignmentOperator == null)
            {
                throw new ArgumentNullException();
            }

            var variableDeclaratorSyntax = boundAssignmentOperator.Left.Syntax.Green as VariableDeclaratorSyntax;
            if (variableDeclaratorSyntax != null && variableDeclaratorSyntax.Initializer != null)
            {
                this.ApplyAutoType = true;
            }

            var boundLocal = boundAssignmentOperator.Left as BoundLocal;
            if (boundLocal != null && boundLocal.LocalSymbol.SynthesizedLocalKind != SynthesizedLocalKind.None)
            {
                this.ApplyAutoType = true;
            }

            this.left = Deserialize(boundAssignmentOperator.Left) as Expression;
            Debug.Assert(this.left != null);
            this.right = Deserialize(boundAssignmentOperator.Right) as Expression;
            Debug.Assert(this.right != null);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.ApplyAutoType)
            {
                c.TextSpan("auto");
                c.WhiteSpace();
            }

            this.left.WriteTo(c);
            c.WhiteSpace();
            c.TextSpan("=");
            c.WhiteSpace();
            this.right.WriteTo(c);
        }
    }
}
