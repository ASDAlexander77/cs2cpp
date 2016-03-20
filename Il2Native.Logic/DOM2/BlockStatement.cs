// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;

    public class BlockStatement : Statement
    {
        public override Kinds Kind
        {
            get { return Kinds.BlockStatement; }
        }

        public bool NoSeparation { get; set; }

        public Base Statements { get; set; }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            if (this.Statements != null)
            {
                this.Statements.Visit(visitor);
            }
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
