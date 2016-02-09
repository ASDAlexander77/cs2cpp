namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class AddressOfOperator : Expression
    {
        public Expression Operand { get; set; }

        public bool IsFixedStatementAddressOf { get; set; }

        internal void Parse(BoundAddressOfOperator boundAddressOfOperator)
        {
            base.Parse(boundAddressOfOperator);

            this.IsFixedStatementAddressOf = boundAddressOfOperator.IsFixedStatementAddressOf;
            this.Operand = Deserialize(boundAddressOfOperator.Operand) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (!(this.Operand is ThisReference))
            {
                c.TextSpan("&");
            }

            this.Operand.WriteTo(c);
        }
    }
}
