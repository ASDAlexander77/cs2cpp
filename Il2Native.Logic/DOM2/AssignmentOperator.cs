// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
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
        public bool ApplyAutoType { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.AssignmentOperator; }
        }

        public Expression Left { get; set; }

        public Expression Right { get; set; }

        public bool TypeDeclaration { get; set; }

        public bool TypeDeclarationSplit { get; set; }

        public bool IsRef { get; set; }

        public bool IsOut { get; set; }

        internal void Parse(BoundAssignmentOperator boundAssignmentOperator)
        {
            base.Parse(boundAssignmentOperator);

            var boundLocal = boundAssignmentOperator.Left as BoundLocal;
            if (boundLocal != null && boundLocal.Syntax.Parent != null)
            {
                var variableDeclarationSyntax = boundLocal.Syntax.Parent.Green as VariableDeclarationSyntax;
                if (variableDeclarationSyntax != null)
                {
                    this.TypeDeclaration = true;
                    this.ApplyAutoType = variableDeclarationSyntax.Type.ToString() == "var";
                }
            }

            var forEachStatementSyntax = boundAssignmentOperator.Left.Syntax.Green as ForEachStatementSyntax;
            if (forEachStatementSyntax != null)
            {
                if (boundLocal != null && boundLocal.LocalSymbol.SynthesizedKind == default(SynthesizedLocalKind))
                {
                    this.TypeDeclaration = true;
                    this.ApplyAutoType = true;
                }
            }

            this.Left = Deserialize(boundAssignmentOperator.Left) as Expression;
            this.Right = Deserialize(boundAssignmentOperator.Right) as Expression;

            if (boundLocal == null || boundLocal.LocalSymbol.IsFixed || boundLocal.LocalSymbol.IsUsing ||
                boundLocal.LocalSymbol.SynthesizedKind == SynthesizedLocalKind.LoweringTemp)
            {
                this.TypeDeclaration = false;
                this.ApplyAutoType = false;
            }

            this.IsRef = boundAssignmentOperator.RefKind.HasFlag(RefKind.Ref);
            this.IsOut = boundAssignmentOperator.RefKind.HasFlag(RefKind.Out);
            if (this.IsRef || this.IsOut)
            {
                this.TypeDeclaration = true;
            }
        }

        internal override void Visit(Action<Base> visitor)
        {
            this.Left.Visit(visitor);
            this.Right.Visit(visitor);
            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.TypeDeclaration)
            {
                if (this.ApplyAutoType && !this.TypeDeclarationSplit)
                {
                    c.TextSpan("auto");
                }
                else if (Type != null)
                {
                    c.WriteType(Type);
                }

                if (this.IsRef || this.IsOut)
                {
                    c.TextSpan("&");
                }

                c.WhiteSpace();
                if (this.TypeDeclarationSplit && !this.IsRef && !this.IsOut)
                {
                    ////Debug.Assert(!this.IsRef && !this.IsOut, "reference initialization");
                    this.Left.WriteTo(c);
                    c.EndStatement();
                }
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

            c.WriteWrappedExpressionIfNeeded(this.Right);
        }
    }
}
