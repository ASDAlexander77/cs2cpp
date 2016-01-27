namespace Il2Native.Logic.DOM2
{
    using System;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class Conversion : Expression
    {
        private TypeSymbol typeSource;

        private TypeSymbol typeDestination;

        private Expression operand;

        private ConversionKind conversionKind;

        internal void Parse(BoundConversion boundConversion)
        {
            if (boundConversion == null)
            {
                throw new ArgumentNullException();
            }

            this.typeSource = boundConversion.Operand.Type;
            this.typeDestination = boundConversion.Type;
            this.operand = Deserialize(boundConversion.Operand) as Expression;
            this.conversionKind = boundConversion.ConversionKind;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            switch (this.conversionKind)
            {
                case ConversionKind.MethodGroup:
                    throw new NotImplementedException();
                case ConversionKind.NullToPointer:
                    // The null pointer is represented as 0u.
                    c.TextSpan("(uintptr_t)0");
                    return;
                case ConversionKind.Boxing:
                    c.TextSpan("__box<");
                    c.WriteTypeFullName((INamedTypeSymbol)this.typeSource);
                    c.TextSpan(">");
                    break;
                case ConversionKind.Unboxing:
                    c.TextSpan("__unbox<");
                    c.WriteTypeFullName((INamedTypeSymbol)this.typeDestination);
                    c.TextSpan(">");
                    break;
                default:
                    c.TextSpan("static_cast<");
                    c.WriteType(this.typeDestination);
                    c.TextSpan(">");
                    break;
            }

            c.TextSpan("(");
            this.operand.WriteTo(c);
            c.TextSpan(")");
        }
    }
}
