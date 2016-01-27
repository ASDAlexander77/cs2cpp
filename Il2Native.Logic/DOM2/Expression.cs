namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public abstract class Expression : Base
    {
        internal void Parse(BoundExpression boundExpression)
        {
            if (boundExpression == null)
            {
                throw new ArgumentNullException();
            }

            this.Type = boundExpression.Type;
        }

        public ITypeSymbol Type { get; private set; }
    }
}
