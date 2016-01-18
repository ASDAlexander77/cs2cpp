namespace Il2Native.Logic.DOM
{
    using System;
    using System.CodeDom.Compiler;
    using Microsoft.CodeAnalysis;

    public class CCodeType : CCodeBase
    {
        public CCodeType(ITypeSymbol type)
        {
            this.Type = type;
        }

        public ITypeSymbol Type { get; set; }

        public override void WriteTo(IndentedTextWriter itw, WriteSettings settings)
        {
            if (this.Type.IsReferenceType)
            {
                itw.Write("void*");
                return;
            }

            if (this.Type.IsValueType)
            {
                switch (this.Type.SpecialType)
                {
                    case SpecialType.System_Enum:
                        // TODO: get enum type
                        break;
                    case SpecialType.System_Void:
                        itw.Write("void");
                        break;
                    case SpecialType.System_Boolean:
                        itw.Write("bool");
                        break;
                    case SpecialType.System_Char:
                        itw.Write("uint16_t");
                        break;
                    case SpecialType.System_SByte:
                        itw.Write("int8_t");
                        break;
                    case SpecialType.System_Byte:
                        itw.Write("uint8_t");
                        break;
                    case SpecialType.System_Int16:
                        itw.Write("int16_t");
                        break;
                    case SpecialType.System_UInt16:
                        itw.Write("uint16_t");
                        break;
                    case SpecialType.System_Int32:
                        itw.Write("int32_t");
                        break;
                    case SpecialType.System_UInt32:
                        itw.Write("uint32_t");
                        break;
                    case SpecialType.System_Int64:
                        itw.Write("int64_t");
                        break;
                    case SpecialType.System_UInt64:
                        itw.Write("uint64_t");
                        break;
                    case SpecialType.System_Single:
                        itw.Write("float");
                        break;
                    case SpecialType.System_Double:
                        itw.Write("double");
                        break;
                    case SpecialType.System_IntPtr:
                        itw.Write("intptr_t");
                        break;
                    case SpecialType.System_UIntPtr:
                        itw.Write("uintptr_t");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
