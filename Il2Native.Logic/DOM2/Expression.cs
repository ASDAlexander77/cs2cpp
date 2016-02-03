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

            if (boundExpression.Type == null)
            {
                return;
            }

            this.Type = boundExpression.Type;
            this.IsReference = this.Type.IsReferenceType;
        }

        public ITypeSymbol Type { get; protected set; }

        public bool IsReference { get; set; }
    }
}
