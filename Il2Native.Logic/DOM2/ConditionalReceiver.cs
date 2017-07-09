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

        public int Id { get; set; }

        internal void Parse(BoundConditionalReceiver boundConditionalReceiver)
        {
            base.Parse(boundConditionalReceiver);
            this.Id = boundConditionalReceiver.Id;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            new Local { LocalSymbol = new LocalImpl { Name = string.Concat("__ConditionalReceiver", this.Id), Type = Type } }.SetOwner(MethodOwner).WriteTo(c);
        }
    }
}
