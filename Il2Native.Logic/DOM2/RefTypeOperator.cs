// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class RefTypeOperator : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.RefTypeOperator; }
        }

        public Expression Operand { get; set; }

        internal void Parse(BoundRefTypeOperator boundRefTypeOperator)
        {
            base.Parse(boundRefTypeOperator);
            this.Operand = Deserialize(boundRefTypeOperator.Operand) as Expression;
            Debug.Assert(this.Operand != null);
        }

        internal override void Visit(System.Action<Base> visitor)
        {
            this.Operand.Visit(visitor);
            base.Visit(visitor);
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
