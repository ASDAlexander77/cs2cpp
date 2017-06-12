﻿// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;

    using Il2Native.Logic.DOM.Implementations;

    public class Cast : Expression
    {
        public bool CCast { get; set; }

        public bool ClassCast { get; set; }

        public bool Constrained { get; set; }
        public bool ValueTypeToClass { get; set; }

        public bool ClassToValueType { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.Cast; }
        }

        public bool MapPointerCast { get; set; }

        public bool BoxByRef { get; set; }

        public Expression MapPointerCastTypeParameter1 { get; set; }

        public Expression MapPointerCastTypeParameter2 { get; set; }

        public Expression Operand { get; set; }

        public bool Reference { get; set; }

        public bool UseEnumUnderlyingType { get; set; }

        internal override void Visit(Action<Base> visitor)
        {
            this.Operand.Visit(visitor);
            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            var effectiveType = Type;
            ////if (this.UseEnumUnderlyingType && effectiveType.TypeKind == TypeKind.Enum)
            ////{
            ////    effectiveType = ((INamedTypeSymbol)effectiveType).EnumUnderlyingType;
            ////}

            if (this.Constrained)
            {
                c.TextSpan("constrained");

                var refValue = false;
                switch (this.Operand)
                {
                    case Local localVar: refValue = localVar.IsRef || localVar.IsOut; break;

                    case Parameter parameter:
                        var refKind = parameter.ParameterSymbol.RefKind;
                        refValue = refKind.HasFlag(Microsoft.CodeAnalysis.RefKind.Ref) || refKind.HasFlag(Microsoft.CodeAnalysis.RefKind.Out);
                        break;

                    case Cast cast: refValue = cast.BoxByRef; break;
                }

                if (refValue)
                {
                    c.TextSpan("_ref");
                }

                c.TextSpan("<");

                if (this.ValueTypeToClass)
                {
                    c.TextSpan("typename valuetype_to_class<");
                }

                c.WriteType(effectiveType, this.ClassCast, valueTypeAsClass: this.ClassCast, containingNamespace: MethodOwner?.ContainingNamespace);
                if (this.ValueTypeToClass)
                {
                    c.TextSpan(">::type");
                }

                c.TextSpan(">(");
                this.Operand.WriteTo(c);
                c.TextSpan(")");
            }
            else if (this.ValueTypeToClass)
            {
                c.TextSpan("valuetype_to_class");
                c.TextSpan("<");
                c.WriteType(effectiveType, this.ClassCast, valueTypeAsClass: this.ClassCast, containingNamespace: MethodOwner?.ContainingNamespace);
                c.TextSpan(">(");
                this.Operand.WriteTo(c);
                c.TextSpan(")");
            }
            else if (this.ClassToValueType)
            {
                c.TextSpan("class_to_valuetype");
                c.TextSpan("<");
                c.WriteType(effectiveType, this.ClassCast, valueTypeAsClass: this.ClassCast, containingNamespace: MethodOwner?.ContainingNamespace);
                c.TextSpan(">(");
                this.Operand.WriteTo(c);
                c.TextSpan(")");
            }
            else if (this.Reference)
            {
                c.TextSpan("((");
                c.WriteType(effectiveType, this.ClassCast, valueTypeAsClass: this.ClassCast, containingNamespace: MethodOwner?.ContainingNamespace);
                c.TextSpan("&)");
                c.WriteExpressionInParenthesesIfNeeded(this.Operand);
                c.TextSpan(")");
            }
            else if (this.CCast)
            {
                c.TextSpan("((");
                c.WriteType(effectiveType, this.ClassCast, valueTypeAsClass: this.ClassCast, containingNamespace: MethodOwner?.ContainingNamespace);
                c.TextSpan(")");
                c.WriteExpressionInParenthesesIfNeeded(this.Operand);
                c.TextSpan(")");
            }
            else if (this.MapPointerCast)
            {
                c.TextSpan("map_pointer_cast<");
                this.MapPointerCastTypeParameter1.WriteTo(c);
                c.TextSpan(", ");
                this.MapPointerCastTypeParameter2.WriteTo(c);
                c.TextSpan(">(");
                c.WriteExpressionInParenthesesIfNeeded(this.Operand);
                c.TextSpan(")");
            }
            else if (this.BoxByRef)
            {
                c.TextSpan("__box_ref_t");
                c.TextSpan("(");
                new Cast { CCast = true, Type = new PointerTypeImpl { PointedAtType = Type }, Operand = Operand, MethodOwner = MethodOwner }.WriteTo(c);
                c.TextSpan(")");                   
            }
            else
            {
                c.WriteType(effectiveType, this.ClassCast, valueTypeAsClass: this.ClassCast, containingNamespace: MethodOwner?.ContainingNamespace);
                c.TextSpan("(");
                this.Operand.WriteTo(c);
                c.TextSpan(")");
            }
        }
    }
}
