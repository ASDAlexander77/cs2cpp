namespace Il2Native.Logic.DOM2
{
    using System;

    using Microsoft.CodeAnalysis;

    public class TypeDef : Statement
    {
        public override Kinds Kind
        {
            get { return Kinds.TypeDef; }
        }

        public TypeExpression TypeExpressionOpt { get; set; }

        public IMethodSymbol PointerToMemberOpt { get; set; }

        public Expression Identifier { get; set; }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            if (this.TypeExpressionOpt != null)
            {
                this.TypeExpressionOpt.Visit(visitor);
            }

            this.Identifier.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("typedef");
            c.WhiteSpace();
            if (this.TypeExpressionOpt != null)
            {
                this.TypeExpressionOpt.WriteTo(c);
                c.WhiteSpace();
                this.Identifier.WriteTo(c);
            }
            else if (this.PointerToMemberOpt != null)
            {
                c.WriteType(this.PointerToMemberOpt.ReturnType);
                c.WhiteSpace();
                c.TextSpan("(");
                if (!this.PointerToMemberOpt.IsStatic)
                {
                    c.WriteType(this.PointerToMemberOpt.ContainingType, true, true, true);
                    c.TextSpan("::");
                }

                c.TextSpan("*");
                c.WhiteSpace();
                this.Identifier.WriteTo(c);
                c.TextSpan(")");
                c.WriteMethodParameters(this.PointerToMemberOpt, true, false);
            }

            base.WriteTo(c);
        }
    }
}
