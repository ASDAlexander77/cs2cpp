namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp;

    public class TryStatement : Statement
    {
        private Block tryBlock;
        private IList<CatchBlock> catchBlocks = new List<CatchBlock>();
        private Block finallyBlockOpt;

        internal void Parse(BoundTryStatement boundTryStatement)
        {
            if (boundTryStatement == null)
            {
                throw new ArgumentNullException();
            }

            this.tryBlock = Deserialize(boundTryStatement.TryBlock) as Block;

            foreach (var boundCatchBlock in boundTryStatement.CatchBlocks)
            {
                this.catchBlocks.Add(Deserialize(boundCatchBlock) as CatchBlock);
            }

            if (boundTryStatement.FinallyBlockOpt != null)
            {
                this.finallyBlockOpt = Deserialize(boundTryStatement.FinallyBlockOpt) as Block;
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("try");

            c.NewLine();
            PrintBlockOrStatementsAsBlock(c, this.tryBlock);

            foreach (var catchBlock in this.catchBlocks)
            {
                catchBlock.WriteTo(c);
            }

            // TODO: finish finally block
            if (this.finallyBlockOpt != null)
            {
                c.TextSpan("// Finally block");
                c.NewLine();
                c.TextSpan("/*");
                c.NewLine();

                this.finallyBlockOpt.WriteTo(c);

                c.TextSpan("*/");
                c.NewLine();
            }

            c.NewLine();
        }
    }
}
