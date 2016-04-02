// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class ThisReference : Expression
    {
        public override bool IsReference
        {
            get
            {
                return true;
            }

            set
            {
            }
        }

        public bool ValueAsReference { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.ThisReference; }
        }

        internal void Parse(BoundThisReference boundThisReference)
        {
            base.Parse(boundThisReference);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("this");
        }
    }
}
