namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class Local : Expression
    {
        private ILocalSymbol localSymbol;

        public override Kinds Kind
        {
            get { return Kinds.Local; }
        }

        public ILocalSymbol LocalSymbol 
        {
            get
            {
                return localSymbol;
            }

            set
            {
                localSymbol = value;
                Parse(localSymbol);
            }
        }

        public string Name
        {
            get 
            {
                return this.CustomName ?? this.LocalSymbol.Name;
            }
        }

        public string CustomName { get; set; }

        internal static void WriteLocal(ILocalSymbol local, CCodeWriterBase c)
        {
            c.WriteNameEnsureCompatible(local);
        }

        internal void Parse(BoundLocal boundLocal)
        {
            base.Parse(boundLocal);
            Parse(boundLocal.LocalSymbol);
        }

        internal void Parse(LocalSymbol localSymbol)
        {
            Type = localSymbol.Type;
            IsReference = this.Type.IsReferenceType;

            ParseName(localSymbol);
            this.localSymbol = localSymbol;
        }

        internal void Parse(ILocalSymbol localSymbol)
        {
            Type = localSymbol.Type;
            IsReference = this.Type.IsReferenceType;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.CustomName != null)
            {
                c.TextSpan(this.CustomName);
            }
            else
            {
                WriteLocal(this.LocalSymbol, c);
            }
        }

        private void ParseName(LocalSymbol local)
        {
            if (local.SynthesizedLocalKind != SynthesizedLocalKind.None)
            {
                var lbl = string.Empty;
                if (local.SynthesizedLocalKind > SynthesizedLocalKind.ForEachArrayIndex0 &&
                    local.SynthesizedLocalKind < SynthesizedLocalKind.ForEachArrayLimit0)
                {
                    lbl = string.Format("ForEachArrayIndex{0}", local.SynthesizedLocalKind - SynthesizedLocalKind.ForEachArrayIndex0);
                }
                else if (local.SynthesizedLocalKind > SynthesizedLocalKind.ForEachArrayLimit0 &&
                    local.SynthesizedLocalKind < SynthesizedLocalKind.FixedString)
                {
                    lbl = string.Format("ForEachArrayLimit{0}", local.SynthesizedLocalKind - SynthesizedLocalKind.ForEachArrayLimit0);
                }
                else
                {
                    lbl = local.SynthesizedLocalKind.ToString();
                    var firstTime = false;
                    lbl += string.Format("_{0}", CCodeWriterBase.GetIdLocal(local, out firstTime));
                }

                this.CustomName = lbl;
            }
        }
    }
}
