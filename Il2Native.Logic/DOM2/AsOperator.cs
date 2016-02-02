namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class AsOperator : Expression
    {
        public Expression Operand { get; set; }

        public TypeExpression TargetType { get; set; }

        internal ConversionKind ConversionKind { get; set; }

        internal void Parse(BoundAsOperator boundAsOperator)
        {
            base.Parse(boundAsOperator);
            this.ConversionKind = boundAsOperator.Conversion.Kind;
            this.TargetType = Deserialize(boundAsOperator.TargetType) as TypeExpression;
            this.Operand =  Deserialize(boundAsOperator.Operand) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            WriteCast(c, this.ConversionKind, this.TargetType, this.Operand);
        }

        internal static void WriteCast(CCodeWriterBase c, ConversionKind conversionKind, TypeExpression targetType, Expression operand)
        {
            // TODO: finish dynamic cast
            //c.TextSpan("dynamic_cast<");
            c.TextSpan("reinterpret_cast<");
            targetType.WriteTo(c);
            c.TextSpan(">");
            c.TextSpan("(");
            operand.WriteTo(c);
            c.TextSpan(")");
        }
    }
}
