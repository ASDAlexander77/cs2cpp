namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class TypeOfOperator : Expression
    {
        private Expression sourceType;

        public override Kinds Kind
        {
            get { return Kinds.TypeOfOperator; }
        }

        internal void Parse(BoundTypeOfOperator boundTypeOfOperator)
        {
            base.Parse(boundTypeOfOperator);
            this.sourceType = Deserialize(boundTypeOfOperator.SourceType) as Expression;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.sourceType.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("_typeof<");
            sourceType.WriteTo(c);
            c.TextSpan(">()");
        }
    }
}
