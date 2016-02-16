 namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class Conversion : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.Conversion; }
        }

        public Expression Operand { get; set; }

        // TODO: get rid of TypeSoure and use Operand.Type for it
        protected ITypeSymbol TypeSource { get; set; }

        internal ConversionKind ConversionKind { get; set; }

        internal void Parse(BoundConversion boundConversion)
        {
            base.Parse(boundConversion);
            this.TypeSource = boundConversion.Operand.Type;
            this.Operand = Deserialize(boundConversion.Operand) as Expression;
            this.ConversionKind = boundConversion.ConversionKind;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Operand.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            var interfaceCastRequired = this.ConversionKind == ConversionKind.Boxing && this.Type.TypeKind == TypeKind.Interface;
            if (interfaceCastRequired)
            {
                c.TextSpan("interface_cast<");
                c.WriteType(this.Type);
                c.TextSpan(">");
                c.TextSpan("(");
            }

            bool parenthesis = false;
            if (this.WriteCast(c, out parenthesis))
            {
                if (parenthesis)
                {
                    c.WriteExpressionInParenthesesIfNeeded(this.Operand);
                }
                else
                {
                    c.TextSpan("(");
                    this.Operand.WriteTo(c);
                    c.TextSpan(")");
                }
            }

            if (interfaceCastRequired)
            {
                c.TextSpan(")");
            }
        }

        private bool WriteCast(CCodeWriterBase c, out bool parenthesis)
        {
            parenthesis = false;

            switch (this.ConversionKind)
            {
                case ConversionKind.MethodGroup:
                    var newDelegate = new DelegateCreationExpression { Type = Type };
                    newDelegate.Arguments.Add(this.Operand);
                    newDelegate.WriteTo(c);
                    return false;
                case ConversionKind.NullToPointer:
                    // The null pointer is represented as 0u.
                    c.TextSpan("(");
                    c.WriteType(this.Type);
                    c.TextSpan(")");
                    c.TextSpan("nullptr");
                    return false;
                case ConversionKind.Boxing:
                    c.TextSpan("__box");
                    break;
                case ConversionKind.Unboxing:
                    c.TextSpan("__unbox<");
                    c.WriteType(this.Type, true, false, true);
                    c.TextSpan(">");
                    break;
                case ConversionKind.ExplicitReference:
                case ConversionKind.ImplicitReference:

                    if (this.Type.TypeKind != TypeKind.TypeParameter &&
                        TypeSource.IsDerivedFrom(this.Type))
                    {
                        c.TextSpan("static_cast<");
                        c.WriteType(this.Type);
                        c.TextSpan(">");
                    }
                    else
                    {
                        if ((this.ConversionKind == ConversionKind.ExplicitReference ||
                             this.ConversionKind == ConversionKind.ImplicitReference)
                            && this.Type.TypeKind == TypeKind.Interface)
                        {
                            c.TextSpan("interface_cast<");
                        }
                        else
                        {
                            c.TextSpan("as<");
                        }

                        c.WriteType(this.Type);
                        c.TextSpan(">");
                    }

                    break;
                case ConversionKind.PointerToInteger:
                case ConversionKind.IntegerToPointer:
                case ConversionKind.PointerToPointer:

                    if (!this.Type.IsIntPtrType())
                    {
                        c.TextSpan("(");
                        c.WriteType(this.Type);
                        c.TextSpan(")");

                        parenthesis = true;
                    }

                    break;
                case ConversionKind.Identity:
                    // for string
                    if (TypeSource.SpecialType == SpecialType.System_String &&
                        this.Type.TypeKind == TypeKind.PointerType)
                    {
                        c.TextSpan("&");
                        this.Operand.WriteTo(c);
                        c.TextSpan("->m_firstChar");
                        return false;
                    }

                    return true;
                default:
                    c.TextSpan("static_cast<");
                    c.WriteType(this.Type);
                    c.TextSpan(">");
                    break;
            }

            return true;
        }
    }
}
