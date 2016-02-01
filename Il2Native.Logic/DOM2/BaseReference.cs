namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class BaseReference : Expression
    {
        internal void Parse(BoundBaseReference boundBaseReference)
        {
            base.Parse(boundBaseReference);
            IsReference = true;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("this->base");
        }
    }
}
