namespace Il2Native.Logic.CDOM
{
    using System.CodeDom.Compiler;
    using System.Linq;

    public class CBlockNode : CNodes
    {
        public override void WriteTo(IndentedTextWriter itw)
        {
            var header = this.Nodes.OfType<CStatementNode>().Where(s => s.Header).ToArray();
            if (header.Length > 0)
            {
                itw.Write(": ");
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

            itw.WriteLine("{");
            itw.Indent++;

            if (!IsEmpty)
            {
                foreach (var cNode in this.Nodes.Skip(countUsed))
                {
                    cNode.WriteTo(itw);
                }
            }

            itw.Indent--;
            itw.WriteLine("}");
        }
    }
}
