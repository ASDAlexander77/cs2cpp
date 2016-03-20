// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public abstract class Expression : Base
    {
        private ITypeSymbol _type;

        public virtual bool IsReference { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.Expression; }
        }

        public ITypeSymbol Type
        {
            get
            {
                return this._type;
            }

            set
            {
                this._type = value;
                this.IsReference = this._type != null ? this._type.IsReferenceType : true;
            }
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
