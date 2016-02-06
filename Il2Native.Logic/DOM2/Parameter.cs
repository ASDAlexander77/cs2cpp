namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class Parameter : Expression
    {
        public IParameterSymbol ParameterSymbol { get; set; }

        internal void Parse(BoundParameter boundParameter)
        {
            base.Parse(boundParameter);
            this.ParameterSymbol = boundParameter.ParameterSymbol;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.WriteNameEnsureCompatible(this.ParameterSymbol);
        }
    }
}
