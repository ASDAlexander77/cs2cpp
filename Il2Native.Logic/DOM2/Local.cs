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
                if (local.SynthesizedLocalKind > SynthesizedLocalKind.ForEachArrayIndex0 &&
                    local.SynthesizedLocalKind < SynthesizedLocalKind.ForEachArrayLimit0)
                {
                    c.TextSpan(string.Format("ForEachArrayIndex{0}", local.SynthesizedLocalKind - SynthesizedLocalKind.ForEachArrayIndex0));
                }
                else if (local.SynthesizedLocalKind > SynthesizedLocalKind.ForEachArrayLimit0 &&
                    local.SynthesizedLocalKind < SynthesizedLocalKind.FixedString)
                {
                    c.TextSpan(string.Format("ForEachArrayLimit{0}", local.SynthesizedLocalKind - SynthesizedLocalKind.ForEachArrayLimit0));
                }
                else
                {
                    c.TextSpan(local.SynthesizedLocalKind.ToString());
                }

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
