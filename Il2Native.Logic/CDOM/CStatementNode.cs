namespace Il2Native.Logic.CDOM
{
    using System.CodeDom.Compiler;
    using System.Linq;

    public class CStatementNode : CNodes
    {
        public bool Header;

        public bool FinishedByBlock;

        public bool ContinuationOfStatement;

        public override void WriteTo(IndentedTextWriter itw)
        {
            if (IsEmpty)
            {
                return;
            }

            var count = this.Nodes.Count;
            var empty = 0;
            for (var i = count - 1; i >= 0; i--)
            {
                if (this.Nodes[i].IsEmpty)
                {
                    empty++;
                }

                break;
            }

            var lastNodeIsBlock = false;
            foreach (var cNode in Nodes.Take(count - empty))
            {
                if (lastNodeIsBlock)
                {
                    itw.WriteLine();
                }

                lastNodeIsBlock = cNode is CBlockNode;
                var nestedBlock = lastNodeIsBlock;
                if (nestedBlock)
                {
                    itw.WriteLine();
                }

                cNode.WriteTo(itw);
            }

            FinishedByBlock = lastNodeIsBlock;
            if (!lastNodeIsBlock)
            {
                itw.WriteLine(!Header ? ";" : string.Empty);
            }
        }
    }
}
