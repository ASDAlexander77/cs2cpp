namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public abstract class Expression : Base
    {
        private ITypeSymbol _type;

        public ITypeSymbol Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
                this.IsReference = _type.IsReferenceType;
            }
        }

        public virtual bool IsReference { get; set; }

        internal override void Visit(Action<Base> visitor)
        {
        }

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
    }
}
