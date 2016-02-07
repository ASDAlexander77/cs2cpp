namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class CatchBlock : Block
    {
        private TypeSymbol exceptionTypeOpt;
        private Expression exceptionSourceOpt;
        private Expression exceptionFilterOpt;
        private Base statements;

        internal void Parse(BoundCatchBlock boundCatchBlock)
        {
            if (boundCatchBlock == null)
            {
                throw new ArgumentNullException();
            }

            if (boundCatchBlock.ExceptionTypeOpt != null)
            {
                this.exceptionTypeOpt = boundCatchBlock.ExceptionTypeOpt;
            }

            if (boundCatchBlock.ExceptionSourceOpt != null)
            {
                this.exceptionSourceOpt = Deserialize(boundCatchBlock.ExceptionSourceOpt) as Expression;
            }

            if (boundCatchBlock.ExceptionFilterOpt != null)
            {
                this.exceptionFilterOpt = Deserialize(boundCatchBlock.ExceptionFilterOpt) as Expression;
            }

            this.statements = Deserialize(boundCatchBlock.Body);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("catch");
            c.WhiteSpace();
            c.TextSpan("(");
            if (this.exceptionTypeOpt != null)
            {
                c.WriteType(this.exceptionTypeOpt);
                if (this.exceptionSourceOpt != null)
                {
                    c.WhiteSpace();
                    this.exceptionSourceOpt.WriteTo(c);
                }
            }
            else
            {
                c.TextSpan("...");
            }

            c.TextSpan(")");

            c.NewLine();
            c.WriteBlockOrStatementsAsBlock(this.statements);
        }
    }
}
