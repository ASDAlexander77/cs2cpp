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

        public bool MethodsTable { get; set; }

        internal void Parse(BoundTypeOfOperator boundTypeOfOperator)
        {
            base.Parse(boundTypeOfOperator);
            this.SourceType = Deserialize(boundTypeOfOperator.SourceType) as Expression;
        }

        internal override void Visit(Action<Base> visitor)
        {
            this.SourceType.Visit(visitor);
            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (!this.MethodsTable)
            {
                c.TextSpan(RuntimeType ? "_runtime_typeof<" : "_typeof<");
            }
            else
            {
                c.TextSpan("_typeMT<");
            }

            this.SourceType.WriteTo(c);
            c.TextSpan(">()");
        }
    }
}
