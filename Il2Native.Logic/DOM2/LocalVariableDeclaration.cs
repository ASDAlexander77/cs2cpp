namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    internal class LocalVariableDeclaration : Statement
    {
        private LocalSymbol localSymbolOpt;

        public void Parse(LocalSymbol localSymbol)
        {
            this.localSymbolOpt = localSymbol;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.WriteType(this.localSymbolOpt.Type);
            c.WhiteSpace();
            c.WriteName(this.localSymbolOpt);
            base.WriteTo(c);
        }
    }
}
