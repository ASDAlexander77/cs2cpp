// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class NoOpStatement : Statement
    {
        public override Kinds Kind
        {
            get { return Kinds.NoOpStatement; }
        }

        internal void Parse(BoundNoOpStatement boundNoOpStatement)
        {
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
        }
    }
}
