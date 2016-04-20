// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class PointerIndirectionOperator : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.PointerIndirectionOperator; }
        }

        public Expression Operand { get; set; }

        internal void Parse(BoundPointerIndirectionOperator pointerIndirectionOperator)
        {
            base.Parse(pointerIndirectionOperator);

            this.Operand = Deserialize(pointerIndirectionOperator.Operand) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("*");
            c.WriteWrappedExpressionIfNeeded(this.Operand);
        }
    }
}
