// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class IsOperator : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.IsOperator; }
        }

        public Expression Operand { get; set; }

        public TypeExpression TargetType { get; set; }

        internal ConversionKind ConversionKind { get; set; }

        internal void Parse(BoundIsOperator boundIsOperator)
        {
            base.Parse(boundIsOperator);
            this.ConversionKind = boundIsOperator.Conversion.Kind;
            this.TargetType = Deserialize(boundIsOperator.TargetType) as TypeExpression;
            this.Operand = Deserialize(boundIsOperator.Operand) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("is<");
            this.TargetType.IsReference = true;
            this.TargetType.WriteTo(c);
            c.TextSpan(">");
            c.TextSpan("(");
            c.WriteExpressionForWrappersIfNeeded(this.Operand);
            c.TextSpan(")");
        }
    }
}
