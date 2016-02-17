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

        public override Kinds Kind
        {
            get { return Kinds.AssignmentOperator; }
        }

        public Expression Left { get; set; }

        public Expression Right { get; set; }

        public bool ApplyAutoType { get; set; }

        public bool TypeDeclaration { get; set; }

        internal void Parse(BoundAssignmentOperator boundAssignmentOperator)
        {
            base.Parse(boundAssignmentOperator);

            var boundLocal = boundAssignmentOperator.Left as BoundLocal;
            if (boundLocal != null)
            {
                var localDeclarationStatementSyntax = boundLocal.Syntax.Parent.Parent.Green as LocalDeclarationStatementSyntax;
                if (localDeclarationStatementSyntax != null)
                {
                    this.TypeDeclaration = true;
                    this.ApplyAutoType = localDeclarationStatementSyntax.Declaration.Type.ToString() == "var";
                }

                var forStatementSyntax = boundLocal.Syntax.Parent.Parent.Green as ForStatementSyntax;
                if (forStatementSyntax != null)
                {
                    this.TypeDeclaration = true;
                    this.ApplyAutoType = forStatementSyntax.Declaration.Type.ToString() == "var";
                }
            }

            var forEachStatementSyntax = boundAssignmentOperator.Left.Syntax.Green as ForEachStatementSyntax;
            if (forEachStatementSyntax != null)
            {
                if (boundLocal != null && boundLocal.LocalSymbol.SynthesizedLocalKind == SynthesizedLocalKind.None)
                {
                    this.TypeDeclaration = true;
                    this.ApplyAutoType = true;
                }
            }

            this.Left = Deserialize(boundAssignmentOperator.Left) as Expression;
            this.Right = Deserialize(boundAssignmentOperator.Right) as Expression;

            if (boundLocal == null || boundLocal.LocalSymbol.IsFixed || boundLocal.LocalSymbol.IsUsing)
            {
                this.TypeDeclaration = false;
                this.ApplyAutoType = false;
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.TypeDeclaration)
            {
                if (this.ApplyAutoType)
                {
                    c.TextSpan("auto");
                }
                else if (Type != null)
                {
                    c.WriteType(Type);
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
