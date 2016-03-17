namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class MethodGroup : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.MethodGroup; }
        }

        public Expression InstanceOpt { get; set; }

        public Expression ReceiverOpt { get; set; }

        public IMethodSymbol Method { get; set; }

        public IList<ITypeSymbol> TypeArgumentsOpt { get; set; }

        internal void Parse(BoundMethodGroup boundMethodGroup)
        {
            base.Parse(boundMethodGroup);
            if (boundMethodGroup.LookupSymbolOpt != null)
            {
                this.Method = boundMethodGroup.LookupSymbolOpt as IMethodSymbol;
            }

            if (boundMethodGroup.TypeArgumentsOpt != null)
            {
                this.TypeArgumentsOpt = boundMethodGroup.TypeArgumentsOpt.OfType<ITypeSymbol>().ToList();
            }

            if (boundMethodGroup.ReceiverOpt != null)
            {
                this.ReceiverOpt = Deserialize(boundMethodGroup.ReceiverOpt) as Expression;
            }

            if (boundMethodGroup.InstanceOpt != null)
            {
                this.InstanceOpt = Deserialize(boundMethodGroup.InstanceOpt) as Expression;
            }

            if (this.Method == null)
            {
                this.Method = boundMethodGroup.Methods.First();
            }
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            if (this.InstanceOpt != null)
            {
                this.InstanceOpt.Visit(visitor);
            }

            if (this.ReceiverOpt != null)
            {
                this.ReceiverOpt.Visit(visitor);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.Method.IsStatic)
            {
                c.TextSpan("&");
                c.WriteTypeFullName(this.Method.ContainingType);
                c.TextSpan("::");
                c.WriteMethodNameNoTemplate(this.Method);
            }
            else
            {
                if (this.ReceiverOpt is BaseReference)
                {
                    c.TextSpan("this");
                }
                else
                {
                    this.ReceiverOpt.WriteTo(c);
                }

                c.TextSpan(",");
                c.WhiteSpace();
                c.TextSpan("&");
                c.WriteAccess(this.ReceiverOpt);
                c.WriteMethodNameNoTemplate(this.Method);
            }

            if (this.TypeArgumentsOpt != null && this.TypeArgumentsOpt.Any())
            {
                c.WriteTypeArguments(this.TypeArgumentsOpt);
            }
        }
    }
}
