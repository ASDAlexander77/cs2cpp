// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class ContinueStatement : Statement
    {
        public override Kinds Kind
        {
            get { return Kinds.ContinueStatement; }
        }

        internal bool Parse(BoundGotoStatement boundGotoStatement)
        {
            if (boundGotoStatement == null)
            {
                throw new ArgumentNullException();
            }

            if (boundGotoStatement.Label.Name.StartsWith("<continue"))
            {
                return true;
            }

            return false;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("continue");
            base.WriteTo(c);
        }
    }
}
