// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class Parameter : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.Parameter; }
        }

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
