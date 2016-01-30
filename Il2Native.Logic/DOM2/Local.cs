namespace Il2Native.Logic.DOM2
{
    using System.Runtime.Serialization;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class Local : Expression
    {
        private LocalSymbol local;

        private static ObjectIDGenerator objectIDGenerator = new ObjectIDGenerator();

        internal static void WriteLocal(LocalSymbol local, CCodeWriterBase c)
        {
            if (local.SynthesizedLocalKind != SynthesizedLocalKind.None)
            {
                c.TextSpan(local.SynthesizedLocalKind.ToString());
                c.TextSpan("_");

                var firstTime = false;
                c.TextSpan(objectIDGenerator.GetId(local, out firstTime).ToString());
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
