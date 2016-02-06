namespace Il2Native.Logic.DOM2
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class Conversion : Expression
    {
        public ITypeSymbol TypeSource { get; set; }

        public ITypeSymbol TypeDestination { get; set; }

        public Expression Operand { get; set; }

        private ConversionKind conversionKind;

        internal void Parse(BoundConversion boundConversion)
        {
            base.Parse(boundConversion);
            this.TypeSource = boundConversion.Operand.Type;
            this.TypeDestination = boundConversion.Type;
            this.Operand = Deserialize(boundConversion.Operand) as Expression;
            this.conversionKind = boundConversion.ConversionKind;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            switch (this.conversionKind)
            {
                case ConversionKind.MethodGroup:
                    Debug.Assert(false, "Not Implemented");
                    ////throw new NotImplementedException();
                    return;
                case ConversionKind.NullToPointer:
                    // The null pointer is represented as 0u.
                    c.TextSpan("nullptr");
                    return;
                case ConversionKind.Boxing:
                    c.TextSpan("__box<");
                    c.WriteTypeFullName(this.TypeSource);
                    c.TextSpan(">");
                    break;
                case ConversionKind.Unboxing:
                    c.TextSpan("__unbox<");
                    c.WriteType(this.TypeDestination);
                    c.TextSpan(",");
                    c.WhiteSpace();
                    c.WriteTypeFullName(this.TypeDestination);
                    c.TextSpan(">");
                    break;
                case ConversionKind.ExplicitReference:
                case ConversionKind.ImplicitReference:

                    if (this.TypeDestination.TypeKind != TypeKind.TypeParameter && this.TypeSource.IsDerivedFrom(this.TypeDestination))
                    {
                        c.TextSpan("static_cast<");
                        c.WriteType(this.TypeDestination);
                        c.TextSpan(">");
                    }
                    else
                    {
                        // TODO: finish dynamic cast
                        //c.TextSpan("dynamic_cast<");
                        if ((this.conversionKind == ConversionKind.ExplicitReference || this.conversionKind == ConversionKind.ImplicitReference)
                            && this.TypeDestination.TypeKind == TypeKind.Interface)
                        {
                            c.TextSpan("static_cast<");
                        }
                        else
                        {
                            c.TextSpan("reinterpret_cast<");
                        }
                        c.WriteType(this.TypeDestination);
                        c.TextSpan(">");
                    }

                    break;
                case ConversionKind.PointerToInteger:
                case ConversionKind.IntegerToPointer:
                        c.TextSpan("reinterpret_cast<");
                        c.WriteType(this.TypeDestination);
                        c.TextSpan(">");
                    break;
                default:
                    c.TextSpan("static_cast<");
                    c.WriteType(this.TypeDestination);
                    c.TextSpan(">");
                    break;
            }

            c.TextSpan("(");

            // TODO: temp hack for supporting cast to interface
            if ((this.conversionKind == ConversionKind.ExplicitReference || this.conversionKind == ConversionKind.ImplicitReference)
                && this.TypeDestination.TypeKind == TypeKind.Interface)
            {
                c.TextSpan("nullptr/*");
                this.Operand.WriteTo(c);
                c.TextSpan("*/");
            }
            else
            {
                this.Operand.WriteTo(c);
            }

            c.TextSpan(")");
        }
    }
}
