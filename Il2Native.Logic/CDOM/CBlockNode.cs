namespace Il2Native.Logic.CDOM
{
    using System.CodeDom.Compiler;
    using System.Linq;

    public class CBlockNode : CNodes
    {
        public bool MethodBlock { get; set; }

        public override void WriteTo(IndentedTextWriter itw)
        {
            var header = this.Nodes.OfType<CStatementNode>().Where(s => s.Header).ToArray();
            if (header.Length > 0)
            {
                itw.Write(" : ");
            }

            var countUsed = 0;
            var any = false;
            foreach (var cNode in header)
            {
                if (any)
                {
                    itw.WriteLine(", ");
                }

                cNode.WriteTo(itw);
                any = true;
                countUsed++;
            }

            if (MethodBlock && !any)
            {
                itw.WriteLine();
            }

            itw.WriteLine("{");
            itw.Indent++;

            if (!IsEmpty)
            {
                var newLineAfterBlockRequired = false;
                foreach (var cNode in Nodes.Skip(countUsed))
                {
                    if (newLineAfterBlockRequired)
                    {
                        itw.WriteLine();
                        newLineAfterBlockRequired = false;
                    }

                    cNode.WriteTo(itw);

                    var statement = cNode as CStatementNode;
                    if (statement != null)
                    {
                        newLineAfterBlockRequired = statement.FinishedByBlock && !statement.ContinuationOfStatement;
                    }
                }
            }

            itw.Indent--;
            itw.WriteLine("}");
        }
    }
}
