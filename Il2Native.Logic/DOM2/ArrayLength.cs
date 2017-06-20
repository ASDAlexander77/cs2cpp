// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class ArrayLength : Expression
    {
        public Expression Expression1 { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.ArrayLength; }
        }

        internal void Parse(BoundArrayLength boundArrayLength)
        {
            base.Parse(boundArrayLength);
            this.Expression1 = Deserialize(boundArrayLength.Expression) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this.Expression1.WriteTo(c);
            c.TextSpan("->operator int32_t()");
        }
    }
}
