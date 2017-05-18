// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class FieldAccess : Expression
    {
        public IFieldSymbol Field { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.FieldAccess; }
        }

        public bool PointerAccess { get; set; }

        public Expression ReceiverOpt { get; set; }

        internal void Parse(BoundFieldAccess boundFieldAccess)
        {
            base.Parse(boundFieldAccess);
            this.Field = boundFieldAccess.FieldSymbol;
            if (boundFieldAccess.ReceiverOpt != null)
            {
                this.ReceiverOpt = Deserialize(boundFieldAccess.ReceiverOpt) as Expression;
            }

            var memberAccessExpressionSyntax = boundFieldAccess.Syntax as MemberAccessExpressionSyntax;
            if (memberAccessExpressionSyntax != null)
            {
                this.PointerAccess = memberAccessExpressionSyntax.Kind() == SyntaxKind.PointerMemberAccessExpression;
            }
        }

        internal override void Visit(Action<Base> visitor)
        {
            if (this.ReceiverOpt != null)
            {
                this.ReceiverOpt.Visit(visitor);
            }

            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.Field.IsStatic)
            {
                if (this.Field.ContainingType == null && this.Field.ContainingSymbol is ITypeSymbol)
                {
                    c.WriteTypeFullName((ITypeSymbol)this.Field.ContainingSymbol, containingNamespace: MethodOwner?.ContainingNamespace);
                }
                else
                {
                    c.WriteTypeFullName(this.Field.ContainingType, containingNamespace: MethodOwner?.ContainingNamespace);
                }

                c.TextSpan("::");
                c.WriteName(this.Field);
            }
            else
            {
                if (this.PointerAccess)
                {
                    var pointerIndirect = this.ReceiverOpt as PointerIndirectionOperator;
                    if (pointerIndirect != null)
                    {
                        c.WriteWrappedExpressionIfNeeded(pointerIndirect.Operand);
                        c.TextSpan("->");
                        c.WriteName(this.Field);
                        return;
                    }
                }

                if (this.ReceiverOpt != null && this.ReceiverOpt.Kind != Kinds.ConditionalReceiver)
                {
                    var primitiveValueAccess = this.Field.ContainingType != null &&
                                                               this.Field.ContainingType.IsPrimitiveValueType() &&
                                                               this.Field.Name == "m_value" && this.ReceiverOpt is Conversion &&
                                                               ((Conversion)this.ReceiverOpt).ConversionKind == ConversionKind.Unboxing;
                    if (primitiveValueAccess)
                    {
                        c.WriteWrappedExpressionIfNeeded(this.ReceiverOpt);
                        return;
                    }
                    else
                    {
                        var effectiveReceiverOpt = PrepareFieldReceiver(this.ReceiverOpt, this.Field);
                        c.WriteAccess(effectiveReceiverOpt);
                    }
                }

                c.WriteName(this.Field);
            }
        }

        private Expression PrepareFieldReceiver(Expression receiverOpt, IFieldSymbol fieldSymbol)
        {
            var effectiveReceiverOpt = receiverOpt;
            if (effectiveReceiverOpt.Type == null)
            {
                return effectiveReceiverOpt;
            }

            if (effectiveReceiverOpt.Type.TypeKind == TypeKind.TypeParameter && this.Field.ContainingType != effectiveReceiverOpt.Type)
            {
                effectiveReceiverOpt = new Cast
                {
                    Constrained = true,
                    Operand = effectiveReceiverOpt,
                    Type = this.Field.ContainingType
                };
            }

            return effectiveReceiverOpt;
        }
    }
}
