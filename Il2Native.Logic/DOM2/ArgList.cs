// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class ArgList : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.ArgList; }
        }

        internal void Parse(BoundArgList boundArgList)
        {
            base.Parse(boundArgList);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            ////c.TextSpan("va_list");
            c.TextSpan("__init<CoreLib::System::IntPtr>(nullptr)");
        }
    }
}
