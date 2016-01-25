namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class Literal : Base
    {
        private ConstantValue constantValue;

        internal void Parse(BoundLiteral boundLiteral)
        {
            if (boundLiteral == null)
            {
                throw new ArgumentNullException();
            }

            this.constantValue = boundLiteral.ConstantValue;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            throw new System.NotImplementedException();
        }
    }
}
