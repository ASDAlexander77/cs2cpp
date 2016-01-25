namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class TypeExpression : Expression
    {
        private TypeSymbol type;

        internal void Parse(BoundTypeExpression boundTypeExpression)
        {
            if (boundTypeExpression == null)
            {
                throw new ArgumentNullException();
            }

            this.type = boundTypeExpression.Type;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            throw new System.NotImplementedException();
        }
    }
}
