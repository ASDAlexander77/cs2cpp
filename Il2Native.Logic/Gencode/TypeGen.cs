// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeGen.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using CodeParts;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class TypeGen
    {
        /// <summary>
        /// </summary>
        public static readonly IDictionary<string, int> SystemTypeSizes = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        static TypeGen()
        {
            SystemTypeSizes["Void"] = 0;
            SystemTypeSizes["Byte"] = 1;
            SystemTypeSizes["SByte"] = 1;
            SystemTypeSizes["Char"] = 2;
            SystemTypeSizes["Int16"] = 2;
            SystemTypeSizes["Int32"] = 4;
            SystemTypeSizes["Int64"] = 8;
            SystemTypeSizes["UInt16"] = 2;
            SystemTypeSizes["UInt32"] = 4;
            SystemTypeSizes["UInt64"] = 8;
            SystemTypeSizes["Single"] = 4;
            SystemTypeSizes["Double"] = 8;
            SystemTypeSizes["Boolean"] = 1;
        }

        public static int GetKnownTypeSize(this IType type)
        {
            int size;
            if (TypeGen.SystemTypeSizes.TryGetValue(type.Name, out size))
            {
                return size;
            }

            return -1;
        }

        public static short GetTypeCode(this IType type)
        {
            switch (type.FullName)
            {
                case "System.Object":
                    return (short)TypeCode.Object;
                case "System.DBNull":
                    return (short)TypeCode.DBNull;
                case "System.Boolean":
                    return (short)TypeCode.Boolean;
                case "System.Char":
                    return (short)TypeCode.Char;
                case "System.SByte":
                    return (short)TypeCode.SByte;
                case "System.Byte":
                    return (short)TypeCode.Byte;
                case "System.Int16":
                    return (short)TypeCode.Int16;
                case "System.UInt16":
                    return (short)TypeCode.UInt16;
                case "System.Int32":
                    return (short)TypeCode.Int32;
                case "System.UInt32":
                    return (short)TypeCode.UInt32;
                case "System.Int64":
                    return (short)TypeCode.Int64;
                case "System.UInt64":
                    return (short)TypeCode.UInt64;
                case "System.Single":
                    return (short)TypeCode.Single;
                case "System.Double":
                    return (short)TypeCode.Double;
                case "System.Decimal":
                    return (short)TypeCode.Decimal;
                case "System.DateTime":
                    return (short)TypeCode.DateTime;
                case "System.String":
                    return (short)TypeCode.String;
                default:
                    if (type.IsStructureType())
                    {
                        return -1;
                    }
                    else
                    {
                        return (short)TypeCode.Object;
                    }
            }
        }

        public static Code GetLoadIndirectCode(this IType type)
        {
            switch (type.FullName)
            {
                case "System.Boolean":
                    return Code.Ldind_I1;
                case "System.Char":
                    return Code.Ldind_U1;
                case "System.SByte":
                    return Code.Ldind_I1;
                case "System.Byte":
                    return Code.Ldind_U1;
                case "System.Int16":
                    return Code.Ldind_I2;
                case "System.UInt16":
                    return Code.Ldind_U2;
                case "System.Int32":
                    return Code.Ldind_I4;
                case "System.UInt32":
                    return Code.Ldind_U4;
                case "System.Int64":
                    return Code.Ldind_I8;
                case "System.UInt64":
                    return Code.Ldind_I8;
                case "System.Single":
                    return Code.Ldind_R4;
                case "System.Double":
                    return Code.Ldind_R8;
                default:
                    return Code.Ldind_Ref;
            }
        }

        public static Code GetSaveIndirectCode(this IType type)
        {
            switch (type.FullName)
            {
                case "System.Boolean":
                case "System.Char":
                case "System.SByte":
                case "System.Byte":
                    return Code.Stind_I1;
                case "System.Int16":
                case "System.UInt16":
                    return Code.Stind_I2;
                case "System.Int32":
                case "System.UInt32":
                    return Code.Stind_I4;
                case "System.Int64":
                    return Code.Stind_I8;
                case "System.UInt64":
                    return Code.Stind_I8;
                case "System.Single":
                    return Code.Stind_R4;
                case "System.Double":
                    return Code.Stind_R8;
                default:
                    return Code.Stind_Ref;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="isPointerOpt">
        /// </param>
        /// <returns>
        /// </returns>
        public static string TypeToCType(this IType type, bool? isPointerOpt = null, bool enumAsName = false, bool shortName = false)
        {
            if (!type.UseAsClass && !type.SpecialUsage())
            {
                if (type.IsEnum && !enumAsName)
                {
                    return TypeToCType(type.GetEnumUnderlyingType(), isPointerOpt);
                }

                if (type.IsValueType && type.IsPrimitive || type.IsVoid())
                {
                    return type.Name;
                }

                if (type.IsStructureType())
                {
                    return string.Concat(shortName ? type.Name : type.FullName, "__struct_data");
                }
            }

            return shortName ? type.Name : type.FullName;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        /// <param name="asReference">
        /// </param>
        public static void WriteTypeModifiers(this IType type, IndentedTextWriter writer, bool asReference, bool asStruct = false)
        {
            var refChar = '*';
            var effectiveType = type;

            if (effectiveType.IsByRef)
            {
                effectiveType = effectiveType.GetElementType();
            }

            var level = 0;
            do
            {
                var isReference = !effectiveType.IsValueType;
                var valueTypeAsReference = (!isReference && asReference && level == 0 && !type.IsByRef);
                if ((isReference || valueTypeAsReference || effectiveType.IsPointer) && !effectiveType.IsGenericParameter && !(level == 0 && asStruct))
                {
                    writer.Write(refChar);
                }

                if (effectiveType.IsPointer)
                {
                    effectiveType = effectiveType.GetElementType();
                    level++;
                }
                else
                {
                    break;
                }
            } while (effectiveType != null);

            if (type.IsByRef)
            {
                writer.Write(refChar);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        /// <param name="isPointer">
        /// </param>
        public static void WriteTypeName(this IType type, CIndentedTextWriter writer, bool isPointer, bool enumAsName = false, bool shortName = false)
        {
            var typeBaseName = type.TypeToCType(isPointer, enumAsName, shortName);
            writer.Write(typeBaseName.CleanUpName());
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="codeWriter">
        /// </param>
        /// <param name="asReference">
        /// </param>
        public static void WriteTypePrefix(this IType type, CWriter codeWriter, bool asReference = false, bool enumAsName = false, bool asStruct = false)
        {
            var writer = codeWriter.Output;

            Debug.Assert(type != null, "Type can't be null to write");

            type.WriteTypeWithoutModifiers(codeWriter, enumAsName: enumAsName);
            type.WriteTypeModifiers(writer, asReference, asStruct);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteTypeWithoutModifiers(
            this IType type,
            CWriter codeWriter,
            bool isPointer = false,
            bool enumAsName = false)
        {
            var writer = codeWriter.Output;

            var effectiveType = type;

            if (effectiveType.IsPointer || effectiveType.IsByRef)
            {
                effectiveType.GetElementType().WriteTypeWithoutModifiers(codeWriter, type.IsPointer, enumAsName);
                return;
            }

            // write base name
            effectiveType.WriteTypeName(writer, isPointer, enumAsName);

            if (type.IsVirtualTable)
            {
                writer.Write(CWriter.VTable);
            }
        }
    }
}