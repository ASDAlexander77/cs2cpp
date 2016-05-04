// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System.Linq;

    using DOM.Implementations;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class MakeRefOperator : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.MakeRefOperator; }
        }

        public Expression Operand { get; set; }

        internal void Parse(BoundMakeRefOperator boundMakeRefOperator)
        {
            base.Parse(boundMakeRefOperator);
            this.Operand = Deserialize(boundMakeRefOperator.Operand) as Expression;
        }

        internal override void Visit(System.Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Operand.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("__makeref");
            ////c.TextSpan("<");
            ////c.WriteType(this.Operand.Type);
            ////c.TextSpan(">");
            c.TextSpan("(");
            new AddressOfOperator { Operand = this.Operand }.WriteTo(c);
            c.TextSpan(")");
        }
    }
}
