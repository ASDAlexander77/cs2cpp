namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class AsOperator : Expression
    {
        private TypeSymbol typeSource;

        private TypeSymbol typeDestination;

        private Expression operand;

        private ConversionKind conversionKind;

        internal void Parse(BoundAsOperator boundAsOperator)
        {
            Parse(boundAsOperator, boundAsOperator.Operand);
        }

        internal void Parse(BoundExpression boundAsOperator, BoundExpression operand)
        {
            base.Parse(boundAsOperator);
            this.typeSource = operand.Type;
            this.typeDestination = boundAsOperator.Type;
            this.operand = Deserialize(operand) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            // TODO: finish dynamic cast
            //c.TextSpan("dynamic_cast<");
            if ((this.conversionKind == ConversionKind.ExplicitReference || this.conversionKind == ConversionKind.ImplicitReference)
                && this.typeDestination.IsInterfaceType())
            {
                c.TextSpan("static_cast<");
            }
            else
            {
                c.TextSpan("reinterpret_cast<");
            }
            c.WriteType(this.typeDestination);
            c.TextSpan(">");
            c.TextSpan("(");

            // TODO: temp hack for supporting cast to interface
            if ((this.conversionKind == ConversionKind.ExplicitReference || this.conversionKind == ConversionKind.ImplicitReference)
                && this.typeDestination.IsInterfaceType())
            {
                c.TextSpan("nullptr/*");
                this.operand.WriteTo(c);
                c.TextSpan("*/");
            }
            else
            {
                this.operand.WriteTo(c);
            }

            c.TextSpan(")");
        }
    }
}
