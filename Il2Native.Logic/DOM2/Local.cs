namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class Local : Expression
    {
        private LocalSymbol local;

        internal static void WriteLocal(LocalSymbol local, CCodeWriterBase c)
        {
            if (local.SynthesizedLocalKind != SynthesizedLocalKind.None)
            {
                c.TextSpan(local.SynthesizedLocalKind.ToString());
            }
            else
            {
                c.WriteName(local);
            }
        }

        internal void Parse(BoundLocal boundLocal)
        {
            base.Parse(boundLocal);
            this.local = boundLocal.LocalSymbol;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            WriteLocal(this.local, c);
        }
    }
}
