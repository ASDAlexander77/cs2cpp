namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class FieldAccess : Expression
    {
        public IFieldSymbol Field { get; set; }

        public Expression ReceiverOpt { get; set; }

        internal void Parse(BoundFieldAccess boundFieldAccess)
        {
            base.Parse(boundFieldAccess);
            this.Field = boundFieldAccess.FieldSymbol;
            if (boundFieldAccess.ReceiverOpt != null)
            {
                this.ReceiverOpt = Deserialize(boundFieldAccess.ReceiverOpt) as Expression;
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.Field.IsStatic)
            {
                c.WriteTypeFullName(this.Field.ContainingType);
                c.TextSpan("::");
                c.WriteName(this.Field);
            }
            else
            {
                c.WriteAccess(this.ReceiverOpt);
                c.WriteName(this.Field);
            }
        }
    }
}
