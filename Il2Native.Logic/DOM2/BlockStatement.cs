namespace Il2Native.Logic.DOM2
{
    using System;

    public class BlockStatement : Statement
    {
        public override Kinds Kind
        {
            get { return Kinds.BlockStatement; }
        }

        public Base Statements { get; set; }

        public bool NoSeparation { get; set; }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            visitor(this.Statements);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.Statements != null)
            {
                c.WriteBlockOrStatementsAsBlock(this.Statements);
            }
            else
            {
                base.WriteTo(c);
            }

            if (!this.NoSeparation)
            {
                // No normal ending of Statement as we do not need extra ;
                c.Separate();
            }
        }
    }
}
