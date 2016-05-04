// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class RefValueOperator : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.RefValueOperator; }
        }

        internal void Parse(BoundRefValueOperator boundRefValueOperator)
        {
            base.Parse(boundRefValueOperator);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("__refvalue");
        }
    }
}
