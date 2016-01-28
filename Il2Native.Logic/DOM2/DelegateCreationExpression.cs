namespace Il2Native.Logic.DOM2
{
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class DelegateCreationExpression : Call
    {
        private MethodSymbol methodOpt;

        internal void Parse(BoundDelegateCreationExpression boundDelegateCreationExpression)
        {
            base.Parse(boundDelegateCreationExpression);
            var argument = Deserialize(boundDelegateCreationExpression.Argument) as Expression;
            this.methodOpt = boundDelegateCreationExpression.MethodOpt;
            Debug.Assert(argument != null);
            Arguments.Add(argument);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("new");
            c.WhiteSpace();
            base.WriteTo(c);
        }
    }
}
