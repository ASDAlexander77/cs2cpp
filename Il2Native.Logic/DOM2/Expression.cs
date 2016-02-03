namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public abstract class Expression : Base
    {
        private ITypeSymbol _type;

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
        }

        public ITypeSymbol Type
        {
            get { return _type; }
            set
            {
                _type = value;
                this.IsReference = _type.IsReferenceType;
            }
        }

        public bool IsReference { get; set; }
    }
}
