namespace Il2Native.Logic.DOM2
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class MethodGroup : Expression
    {
        private IList<IMethodSymbol> methods = new List<IMethodSymbol>();

        public override Kinds Kind
        {
            get { return Kinds.MethodGroup; }
        }

        public Expression ReceiverOpt { get; set; }

        public IList<IMethodSymbol> Methods
        {
            get { return this.methods; }
        }

        internal void Parse(BoundMethodGroup boundMethodGroup)
        {
            base.Parse(boundMethodGroup);
            foreach (var methodSymbol in boundMethodGroup.Methods.OfType<IMethodSymbol>())
            {
                this.Methods.Add(methodSymbol);
            }

            if (boundMethodGroup.ReceiverOpt != null)
            {
                this.ReceiverOpt = Deserialize(boundMethodGroup.ReceiverOpt) as Expression;
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            var method = methods.First();
            if (method.IsStatic)
            {
                c.TextSpan("&");
                c.WriteTypeFullName(method.ContainingType);
                c.TextSpan("::");
                c.WriteMethodName(method, true, true);
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
                c.WriteMethodName(method, true, true);
            }
        }
    }
}
