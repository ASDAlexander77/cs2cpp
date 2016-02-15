namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class BaseReference : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.BaseReference; }
        }

        public bool ExplicitType { get; set; }

        internal void Parse(BoundBaseReference boundBaseReference)
        {
            base.Parse(boundBaseReference);
            IsReference = true;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (ExplicitType)
            {
                c.WriteTypeFullName(Type);
            }
            else
            {
                c.TextSpan("base");
            }
        }
    }
}
