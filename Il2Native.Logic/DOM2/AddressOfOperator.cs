namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class AddressOfOperator : Expression
    {
        internal void Parse(BoundAddressOfOperator boundAddressOfOperator)
        {
            base.Parse(boundAddressOfOperator);

            this.IsFixedStatementAddressOf = boundAddressOfOperator.IsFixedStatementAddressOf;
            this.Operand = Deserialize(boundAddressOfOperator.Operand) as Expression;
        }

        public Expression Operand { get; set; }

        public bool IsFixedStatementAddressOf { get; set; }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("&");
            this.Operand.WriteTo(c);
        }
    }
}
