// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class RefTypeOperator : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.RefTypeOperator; }
        }

        public Expression Operand { get; set; }

        internal void Parse(BoundRefValueOperator boundRefValueOperator)
        {
            base.Parse(boundRefValueOperator);
            this.Operand = Deserialize(boundRefValueOperator.Operand) as Expression;
        }

        internal override void Visit(System.Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Operand.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("__reftype");
            c.TextSpan("(");
            this.Operand.WriteTo(c);
            c.TextSpan(")");
        }
    }
}
