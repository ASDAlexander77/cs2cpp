namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class LambdaExpression : Expression
    {
        public Block Block
        {
            get; set;
        }

        internal void Parse(BoundStatementList boundStatementList)
        {
            var block = new Block();
            block.Parse(boundStatementList);
            this.Block = block;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("[&]()");
            this.Block.WriteTo(c);
        }
    }
}
