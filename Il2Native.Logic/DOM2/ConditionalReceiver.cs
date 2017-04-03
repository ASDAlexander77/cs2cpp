// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using DOM.Implementations;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class ConditionalReceiver : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.ConditionalReceiver; }
        }

        internal void Parse(BoundConditionalReceiver boundConditionalReceiver)
        {
            base.Parse(boundConditionalReceiver);
            throw new NotSupportedException("This should not be called.");
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            // Finish it properly
            c.TextSpan("TEST");
        }
    }
}
