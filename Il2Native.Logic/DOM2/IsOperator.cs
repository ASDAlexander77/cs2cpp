namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class IsOperator : Expression
    {
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
            AsOperator.WriteCast(c, this.ConversionKind, this.TargetType, this.Operand);
            c.WhiteSpace();
            c.TextSpan("==");
            c.WhiteSpace();
            c.TextSpan("nullptr");
        }
    }
}
