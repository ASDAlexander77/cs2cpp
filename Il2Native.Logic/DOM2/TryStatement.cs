namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp;

    public class TryStatement : Statement
    {
        private IList<CatchBlock> catchBlocks = new List<CatchBlock>();

        public Base TryBlock { get; set; }

        public Base FinallyBlockOpt { get; set; }

        public IList<CatchBlock> CatchBlocks
        {
            get { return this.catchBlocks; }
            set { this.catchBlocks = value; }
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.TryBlock.Visit(visitor);
            foreach (var catchBlock in CatchBlocks)
            {
                catchBlock.Visit(visitor);
            }

            if (this.FinallyBlockOpt != null)
            {
                this.FinallyBlockOpt.Visit(visitor);
            }
        }

        internal void Parse(BoundTryStatement boundTryStatement)
        {
            if (boundTryStatement == null)
            {
                throw new ArgumentNullException();
            }

            this.TryBlock = Deserialize(boundTryStatement.TryBlock) as Block;

            foreach (var boundCatchBlock in boundTryStatement.CatchBlocks)
            {
                this.catchBlocks.Add(Deserialize(boundCatchBlock) as CatchBlock);
            }

            if (boundTryStatement.FinallyBlockOpt != null)
            {
                this.FinallyBlockOpt = Deserialize(boundTryStatement.FinallyBlockOpt) as Block;
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.FinallyBlockOpt != null)
            {
                var block = this.FinallyBlockOpt as Block;
                if (block != null)
                {
                    block.SuppressNewLineAtEnd = true;
                }

                c.OpenBlock();
                c.TextSpan("Finally");
                c.WhiteSpace();
                c.TextSpan("__finally_block");
                c.TextSpan("(");
                new LambdaExpression() { Statements = block }.WriteTo(c);
                c.TextSpan(");");
                c.NewLine();
            }

            if (this.catchBlocks.Any())
            {
                c.TextSpan("try");
            }

            c.NewLine();

            c.WriteBlockOrStatementsAsBlock(this.TryBlock);

            foreach (var catchBlock in this.catchBlocks)
            {
                catchBlock.WriteTo(c);
            }

            if (this.FinallyBlockOpt != null)
            {
                c.EndBlock();
            }

            c.Separate();
        }
    }
}
