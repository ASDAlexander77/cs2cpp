namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class StackAllocArrayCreation : Expression
    {
        public Expression Count { get; set; }

        internal void Parse(BoundStackAllocArrayCreation boundStackAllocArrayCreation)
        {
            base.Parse(boundStackAllocArrayCreation);
            this.Count = Deserialize(boundStackAllocArrayCreation.Count) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("alloca");
            c.TextSpan("("); 
            this.Count.WriteTo(c);
            c.TextSpan(")");
        }
    }
}
