namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class ElementAccessExpression : Expression
    {
        private Expression operand;
        private Expression index;

        internal bool Parse(BoundPointerIndirectionOperator boundPointerIndirectionOperator)
        {
            base.Parse(boundPointerIndirectionOperator);

            var boundBinaryOperator = boundPointerIndirectionOperator.Operand as BoundBinaryOperator;
            if (boundBinaryOperator != null)
            {
                this.operand = Deserialize(boundBinaryOperator.Left) as Expression;
                var boundBinaryOperatorWithIndex = boundBinaryOperator.Right as BoundBinaryOperator;
                if (boundBinaryOperatorWithIndex != null)
                {
                    this.index = Deserialize(boundBinaryOperatorWithIndex.Left) as Expression;
                }
                else
                {
                    var boundSizeOfOperator = boundBinaryOperator.Right as BoundSizeOfOperator;
                    if (boundSizeOfOperator != null)
                    {
                        this.index = new Literal { Value = ConstantValue.Create(1) };
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            var conversion = this.index as Conversion;
            if (conversion != null && conversion.Kind == ConversionKind.IntegerToPointer)
            {
                this.index = conversion.Operand;
            }

            return true;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.operand.Visit(visitor);
            this.index.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this.operand.WriteTo(c);
            c.TextSpan("[");
            this.index.WriteTo(c);
            c.TextSpan("]");
        }
    }
}
