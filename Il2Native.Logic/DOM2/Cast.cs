namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;

    public class Cast : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.Cast; }
        }

        public Expression Operand { get; set; }

        public bool ClassCast { get; set; }

        public bool Reference { get; set; }

        public bool Constrained { get; set; }

        public bool CCast { get; set; }

        public bool UseEnumUnderlyingType { get; set; }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Operand.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            var effectiveType = Type;
            if (UseEnumUnderlyingType && effectiveType.TypeKind == TypeKind.Enum)
            {
                effectiveType = ((INamedTypeSymbol)effectiveType).EnumUnderlyingType;
            }

            if (Constrained)
            {
                c.TextSpan("constrained<");
                c.WriteType(effectiveType, ClassCast, valueTypeAsClass: ClassCast);
                c.TextSpan(">(");
                this.Operand.WriteTo(c);
                c.TextSpan(")");                
            }
            else if (Reference)
            {
                c.TextSpan("((");
                c.WriteType(effectiveType, ClassCast, valueTypeAsClass: ClassCast);
                c.TextSpan("&)");
                c.WriteExpressionInParenthesesIfNeeded(this.Operand);
                c.TextSpan(")");
            }
            else if (this.CCast)
            {
                c.TextSpan("((");
                c.WriteType(effectiveType, ClassCast, valueTypeAsClass: ClassCast);
                c.TextSpan(")");
                c.WriteExpressionInParenthesesIfNeeded(this.Operand);
                c.TextSpan(")");
            }
            else
            {
                c.WriteType(effectiveType, this.ClassCast, valueTypeAsClass: ClassCast);
                c.TextSpan("(");
                this.Operand.WriteTo(c);
                c.TextSpan(")");
            }
        }
    }
}
