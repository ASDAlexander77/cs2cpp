namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class TypeExpression : Expression
    {
        internal void Parse(BoundTypeExpression boundTypeExpression)
        {
            base.Parse(boundTypeExpression);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.WriteType(Type, valueTypeAsClass: IsReference);
        }
    }
}
