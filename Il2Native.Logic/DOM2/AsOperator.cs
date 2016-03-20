// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class AsOperator : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.AsOperator; }
        }

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
            c.TextSpan("as<");
            this.TargetType.IsReference = true;
            this.TargetType.WriteTo(c);
            c.TextSpan(">");
            c.TextSpan("(");
            this.Operand.WriteTo(c);
            c.TextSpan(")");
        }
    }
}
