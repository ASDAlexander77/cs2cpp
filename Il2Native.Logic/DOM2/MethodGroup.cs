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
                new Parenthesis { Operand = new MethodPointer{ Method = this.Method } }.WriteTo(c);
                c.TextSpan("&");
                c.WriteTypeFullName(this.Method.ContainingType);
                c.TextSpan("::");
                c.WriteMethodNameNoTemplate(this.Method);
            }
            else
            {
                var receiverOpt = this.ReceiverOpt;
                if (receiverOpt is BaseReference)
                {
                    receiverOpt = new ThisReference();
                }

                if (this.ReceiverOpt.IsStaticWrapperCall())
                {
                    new Cast { Type = receiverOpt.Type, CCast = true, Operand = receiverOpt }.WriteTo(c);
                }
                else
                {
                    receiverOpt.WriteTo(c);
                }

                c.TextSpan(",");
                c.WhiteSpace();
                new Parenthesis { Operand = new MethodPointer { Method = this.Method } }.WriteTo(c);
                c.TextSpan("&");
                c.WriteAccess(receiverOpt);
                c.WriteMethodNameNoTemplate(this.Method);
            }

            if (this.TypeArgumentsOpt != null && this.TypeArgumentsOpt.Any())
            {
                c.WriteTypeArguments(this.TypeArgumentsOpt);
            }
        }
    }
}
