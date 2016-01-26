namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class Local : Expression
    {
        private LocalSymbol local;

        internal void Parse(BoundLocal boundLocal)
        {
            if (boundLocal == null)
            {
                throw new ArgumentNullException();
            }

            this.local = boundLocal.LocalSymbol;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.local.SynthesizedLocalKind != SynthesizedLocalKind.None)
            {
                c.TextSpan(this.local.SynthesizedLocalKind.ToString());
            }
            else
            {
                c.WriteName(this.local);
            }
        }
    }
}
