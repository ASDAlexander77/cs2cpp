namespace Il2Native.Logic.DOM2
{
    using System;

    using Microsoft.CodeAnalysis.CSharp.Symbols;

    internal class LocalVariableDeclaration : Statement
    {
        private LocalSymbol local;

        public void Parse(LocalSymbol localSymbol)
        {
            if (localSymbol == null)
            {
                throw new ArgumentNullException();
            }

            this.local = localSymbol;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.WriteType(this.local.Type);
            c.WhiteSpace();
            Local.WriteLocal(this.local, c);
            base.WriteTo(c);
        }
    }
}
