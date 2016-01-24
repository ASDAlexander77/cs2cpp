namespace Il2Native.Logic.CDOM
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;

    public class CNodes : CNode
    {
        private IList<CNode> _nodes;

        public override bool IsEmpty
        {
            get { return this._nodes == null || this._nodes.Count == 0 || this._nodes.All(n => n.IsEmpty); }
        }

        public override CNodeType Type
        {
            get { return CNodeType.Container; }
        }

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

            foreach (var cNode in Nodes)
            {
                cNode.WriteTo(itw);
            }
        }
    }
}
