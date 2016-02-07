namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Syntax.InternalSyntax;

    public class AssignmentOperator : Expression
    {
        private ITypeSymbol assignmentType;

        public Expression Left { get; set; }

        public Expression Right { get; set; }

        public bool ApplyAutoType { get; set; }

        internal void Parse(BoundAssignmentOperator boundAssignmentOperator)
        {
            base.Parse(boundAssignmentOperator);
            
            var boundLocal = boundAssignmentOperator.Left as BoundLocal;

            var variableDeclaratorSyntax = boundAssignmentOperator.Left.Syntax.Green as VariableDeclaratorSyntax;
            if (variableDeclaratorSyntax != null && variableDeclaratorSyntax.Initializer != null)
            {
                this.ApplyAutoType = true;
            }

            var forEachStatementSyntax = boundAssignmentOperator.Left.Syntax.Green as ForEachStatementSyntax;
            if (forEachStatementSyntax != null)
            {
                if (boundLocal != null && boundLocal.LocalSymbol.SynthesizedLocalKind == SynthesizedLocalKind.None)
                {
                    this.ApplyAutoType = true;
                }
            }

            this.Left = Deserialize(boundAssignmentOperator.Left) as Expression;
            Debug.Assert(this.Left != null);
            this.Right = Deserialize(boundAssignmentOperator.Right) as Expression;
            Debug.Assert(this.Right != null);

            if (this.Right is Literal || boundAssignmentOperator.Type.SpecialType == SpecialType.System_Object)
            {
                this.assignmentType = this.Right.Type;
            }

            if (boundLocal == null || boundLocal.LocalSymbol.IsFixed)
            {
                this.ApplyAutoType = false;
                this.assignmentType = null;
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.ApplyAutoType)
            {
                if (this.assignmentType != null)
                {
                    c.WriteType(this.assignmentType);
                }
                else
                {
                    c.TextSpan("auto");
                }

                c.WhiteSpace();
            }

            var rightType = this.Right.Type;
            if (rightType != null && rightType.IsValueType && this.Left is ThisReference)
            {
                c.TextSpan("*");
            }

            this.Left.WriteTo(c);
            c.WhiteSpace();
            c.TextSpan("=");
            c.WhiteSpace();

            var leftType = this.Left.Type;
            if (leftType != null && leftType.IsValueType && this.Right is ThisReference)
            {
                c.TextSpan("*");
            }

            this.Right.WriteTo(c);
        }
    }
}
