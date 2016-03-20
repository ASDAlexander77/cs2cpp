// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class CatchBlock : BlockStatement
    {
        public Expression ExceptionFilterOpt { get; set; }

        public Expression ExceptionSourceOpt { get; set; }

        public ITypeSymbol ExceptionTypeOpt { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.CatchBlock; }
        }

        internal void Parse(BoundCatchBlock boundCatchBlock)
        {
            if (boundCatchBlock == null)
            {
                throw new ArgumentNullException();
            }

            if (boundCatchBlock.ExceptionTypeOpt != null)
            {
                this.ExceptionTypeOpt = boundCatchBlock.ExceptionTypeOpt;
            }

            if (boundCatchBlock.ExceptionSourceOpt != null)
            {
                this.ExceptionSourceOpt = Deserialize(boundCatchBlock.ExceptionSourceOpt) as Expression;
            }

            if (boundCatchBlock.ExceptionFilterOpt != null)
            {
                this.ExceptionFilterOpt = Deserialize(boundCatchBlock.ExceptionFilterOpt) as Expression;
            }

            Statements = Deserialize(boundCatchBlock.Body);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("catch");
            c.WhiteSpace();
            c.TextSpan("(");
            if (this.ExceptionTypeOpt != null)
            {
                c.WriteType(this.ExceptionTypeOpt);
                if (this.ExceptionSourceOpt != null)
                {
                    c.WhiteSpace();
                    this.ExceptionSourceOpt.WriteTo(c);
                }
            }
            else
            {
                c.TextSpan("...");
            }

            c.TextSpan(")");

            c.NewLine();
            c.WriteBlockOrStatementsAsBlock(Statements);
        }
    }
}
