// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class Local : Expression
    {
        private ILocalSymbol localSymbol;

        public string CustomName { get; set; }

        public Local()
        {
            this.SynthesizedLocalKind = default(SynthesizedLocalKind);
        }

        public override Kinds Kind
        {
            get { return Kinds.Local; }
        }

        public ILocalSymbol LocalSymbol 
        {
            get
            {
                return this.localSymbol;
            }

            set
            {
                this.localSymbol = value;
                this.Parse(this.localSymbol);
            }
        }

        public string Name
        {
            get 
            {
                return this.CustomName ?? this.LocalSymbol.Name;
            }
        }

        public bool IsRef { get; set; }

        public bool IsOut { get; set; }

        internal SynthesizedLocalKind SynthesizedLocalKind { get; set; }

        internal static void WriteLocal(ILocalSymbol local, CCodeWriterBase c)
        {
            c.WriteNameEnsureCompatible(local);
        }

        internal void Parse(BoundLocal boundLocal)
        {
            base.Parse(boundLocal);
            this.Parse(boundLocal.LocalSymbol);
        }

        internal void Parse(LocalSymbol localSymbol)
        {
            Type = localSymbol.Type;
            IsReference = Type.IsReferenceType;

            this.ParseName(localSymbol);
            this.localSymbol = localSymbol;

            this.IsRef = localSymbol.RefKind.HasFlag(RefKind.Ref);
            this.IsOut = localSymbol.RefKind.HasFlag(RefKind.Out);
        }

        internal void Parse(ILocalSymbol localSymbol)
        {
            Type = localSymbol.Type;
            IsReference = Type.IsReferenceType;
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
            if (local.SynthesizedKind != default(SynthesizedLocalKind))
            {
                this.SynthesizedLocalKind = local.SynthesizedKind;

                var lbl = local.SynthesizedKind.ToString();
                var firstTime = false;
                lbl += string.Format("_{0}", CCodeWriterBase.GetIdLocal(local, out firstTime));

                this.CustomName = lbl;
            }
        }
    }
}
