// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class BaseReference : Expression
    {
        public bool ExplicitType { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.BaseReference; }
        }

        internal void Parse(BoundBaseReference boundBaseReference)
        {
            base.Parse(boundBaseReference);
            IsReference = true;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.ExplicitType)
            {
                c.WriteTypeFullName(Type);
            }
            else
            {
                c.TextSpan("base");
            }
        }
    }
}
