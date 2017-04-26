// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class TypeOfOperator : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.TypeOfOperator; }
        }

        public Expression SourceType { get; set; }
        public bool RuntimeType { get; set; }

        internal void Parse(BoundTypeOfOperator boundTypeOfOperator)
        {
            base.Parse(boundTypeOfOperator);
            this.SourceType = Deserialize(boundTypeOfOperator.SourceType) as Expression;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.SourceType.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan(RuntimeType ? "_runtime_typeof<" : "_typeof<");
            this.SourceType.WriteTo(c);
            c.TextSpan(">()");
        }
    }
}
