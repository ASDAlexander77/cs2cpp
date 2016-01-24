namespace Il2Native.Logic.CDOM
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;

    public class CNodes : CNode
    {
        private IList<CNode> _nodes;

        public IList<CNode> Nodes
        {
            get
            {
                if (_nodes == null)
                {
                    _nodes = new List<CNode>();
                }

                return _nodes;
            }
        }

        public override void WriteTo(IndentedTextWriter itw)
        {
            if (_nodes == null)
            {
                return;
            }

            foreach (var cNode in this.Nodes)
            {
                cNode.WriteTo(itw);
            }
        }
    }
}
