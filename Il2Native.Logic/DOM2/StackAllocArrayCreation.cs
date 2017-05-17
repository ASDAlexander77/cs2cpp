// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class StackAllocArrayCreation : Expression
    {
        public Expression Count { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.StackAllocArrayCreation; }
        }

        internal void Parse(BoundStackAllocArrayCreation boundStackAllocArrayCreation)
        {
            base.Parse(boundStackAllocArrayCreation);
            this.Count = Deserialize(boundStackAllocArrayCreation.Count) as Expression;
        }

        internal override void Visit(Action<Base> visitor)
        {
            this.Count.Visit(visitor);
            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("reinterpret_cast<");
            c.WriteType(Type, containingNamespace: MethodOwner?.ContainingNamespace);
            c.TextSpan(">(std::memset(alloca");
            c.TextSpan("("); 
            this.Count.WriteTo(c);
            c.TextSpan("), 0, ");
            this.Count.WriteTo(c);
            c.TextSpan("))");
        }
    }
}
