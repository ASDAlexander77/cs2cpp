namespace Il2Native.Logic.CDOM
{
    using System.CodeDom.Compiler;
    using System.Linq;

    public class CStatementNode : CNodes
    {
        public bool Header;

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

            foreach (var cNode in Nodes.Take(count - empty))
            {
                cNode.WriteTo(itw);
            }

            itw.WriteLine(!Header ? ";" : string.Empty);
        }
    }
}
