namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class FieldAccess : Expression
    {
        private FieldSymbol field;
        private Expression receiverOpt;

        internal void Parse(BoundFieldAccess boundFieldAccess)
        {
            base.Parse(boundFieldAccess);
            this.field = boundFieldAccess.FieldSymbol;
            if (boundFieldAccess.ReceiverOpt != null)
            {
                this.receiverOpt = Deserialize(boundFieldAccess.ReceiverOpt) as Expression;
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (field.IsStatic)
            {
                c.WriteTypeFullName(field.ContainingType);
                c.TextSpan("::");
                c.WriteName(field);
            }
            else
            {
                this.receiverOpt.WriteTo(c);
                c.WriteAccess(this.receiverOpt);
                c.WriteName(field);
            }
        }
    }
}
