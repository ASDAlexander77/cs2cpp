namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class Parameter : Expression
    {
        private ParameterSymbol parameter;

        internal void Parse(BoundParameter boundParameter)
        {
            if (boundParameter == null)
            {
                throw new ArgumentNullException();
            }

            this.parameter = boundParameter.ParameterSymbol;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.WriteName(this.parameter);
        }
    }
}
