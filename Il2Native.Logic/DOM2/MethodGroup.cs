namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class MethodGroup : Expression
    {
        private IList<MethodSymbol> methods = new List<MethodSymbol>();
        private Expression receiverOpt;

        internal void Parse(BoundMethodGroup boundMethodGroup)
        {
            base.Parse(boundMethodGroup);
            this.methods = boundMethodGroup.Methods;
            if (boundMethodGroup.ReceiverOpt != null)
            {
                this.receiverOpt = Deserialize(boundMethodGroup.ReceiverOpt) as Expression;
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            var method = methods.First();

            if (method.IsStatic)
            {
                c.WriteTypeFullName(method.ContainingType);
                c.TextSpan("::");
                c.WriteName(method);
            }
            else
            {
                this.receiverOpt.WriteTo(c);
                c.WriteAccess(this.receiverOpt);
                c.WriteName(method);
            }
        }
    }
}
