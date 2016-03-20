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

        internal void Parse(BoundRefTypeOperator boundRefTypeOperator)
        {
            base.Parse(boundRefTypeOperator);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
        }
    }
}
