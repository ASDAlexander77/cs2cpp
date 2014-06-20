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
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;

    using Il2Native.Logic.CodeParts;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class TypeGen
    {
        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, int> SystemTypeSizes = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, string> SystemTypesToCTypes = new SortedDictionary<string, string>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, int> sizeByType = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        static TypeGen()
        {
            // to be removed
            SystemTypesToCTypes["String"] = "i8";
            SystemTypesToCTypes["String&"] = "i8*";

            SystemTypesToCTypes["Void"] = "void";
            SystemTypesToCTypes["Byte"] = "i8";
            SystemTypesToCTypes["SByte"] = "i8";
            SystemTypesToCTypes["Char"] = "i16";
            SystemTypesToCTypes["Int16"] = "i16";
            SystemTypesToCTypes["Int32"] = "i32";
            SystemTypesToCTypes["Int64"] = "i64";
            SystemTypesToCTypes["UInt16"] = "i16";
            SystemTypesToCTypes["UInt32"] = "i32";
            SystemTypesToCTypes["UInt64"] = "i64";
            SystemTypesToCTypes["Float"] = "float";
            SystemTypesToCTypes["Single"] = "float";
            SystemTypesToCTypes["Double"] = "double";
            SystemTypesToCTypes["Boolean"] = "i1";
            SystemTypesToCTypes["Byte&"] = "i8*";
            SystemTypesToCTypes["SByte&"] = "i8*";
            SystemTypesToCTypes["Char&"] = "i8*";
            SystemTypesToCTypes["Int16&"] = "i16*";
            SystemTypesToCTypes["Int32&"] = "i32*";
            SystemTypesToCTypes["Int64&"] = "i64*";
            SystemTypesToCTypes["IntPtr"] = "i32*";
            SystemTypesToCTypes["UIntPtr"] = "i32*";
            SystemTypesToCTypes["UInt16&"] = "i16**";
            SystemTypesToCTypes["UInt32&"] = "i32**";
            SystemTypesToCTypes["UInt64&"] = "i64*";
            SystemTypesToCTypes["Float&"] = "float*";
            SystemTypesToCTypes["Single&"] = "float*";
            SystemTypesToCTypes["Double&"] = "double*";
            SystemTypesToCTypes["Boolean&"] = "i1*";

            SystemTypeSizes["Void"] = 0;
            SystemTypeSizes["Byte"] = 1;
            SystemTypeSizes["SByte"] = 1;
            SystemTypeSizes["Char"] = 2;
            SystemTypeSizes["Int16"] = 2;
            SystemTypeSizes["Int32"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Int64"] = 8;
            SystemTypeSizes["UInt16"] = 2;
            SystemTypeSizes["UInt32"] = LlvmWriter.pointerSize;
            SystemTypeSizes["UInt64"] = 8;
            SystemTypeSizes["Float"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Single"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Double"] = 8;
            SystemTypeSizes["Boolean"] = 1;
            SystemTypeSizes["Byte&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["SByte&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Char&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Int16&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Int32&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Int64&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["IntPtr"] = LlvmWriter.pointerSize;
            SystemTypeSizes["UIntPtr"] = LlvmWriter.pointerSize;
            SystemTypeSizes["UInt16&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["UInt32&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["UInt64&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Float&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Single&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Double&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Boolean&"] = LlvmWriter.pointerSize;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static int CalculateSize(this IType type)
        {
            if (type.IsInterface)
            {
                // i8** (...)
                return LlvmWriter.pointerSize;
            }

            if (type.IsEnum)
            {
                return type.GetEnumUnderlyingType().GetTypeSize();
            }

            var size = 0;

            // add shift for virtual table
            if (type.IsRootOfVirtualTable())
            {
                size += LlvmWriter.pointerSize;
            }

            if (type.BaseType != null)
            {
                size += type.BaseType.GetTypeSize();
            }

            // add shift for interfaces
            if (type.BaseType == null)
            {
                size += type.GetInterfaces().Count() * LlvmWriter.pointerSize;
            }
            else
            {
                var baseInterfaces = type.BaseType.GetInterfaces();
                size += type.GetInterfaces().Count(i => !baseInterfaces.Contains(i)) * LlvmWriter.pointerSize;
            }

            foreach (var field in IlReader.Fields(type).Where(t => !t.IsStatic).ToList())
            {
                if (field.FieldType.IsStructureType())
                {
                    size += field.FieldType.GetTypeSize();
                }

                var fieldSize = 0;
                if (field.FieldType.IsClass)
                {
                    // pointer size
                    size += LlvmWriter.pointerSize;
                }
                else if (field.FieldType.Namespace == "System" && SystemTypeSizes.TryGetValue(field.FieldType.Name, out fieldSize))
                {
                    size += fieldSize;
                }
                else
                {
                    size += field.FieldType.GetTypeSize();
                }
            }

            sizeByType[type.FullName] = size;

            return size;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static int GetTypeSize(this IType type)
        {
            // find index
            int size;
            if (!sizeByType.TryGetValue(type.FullName, out size))
            {
                size = type.CalculateSize();
            }

            return size;
        }

        /// <summary>
        /// </summary>
        /// <param name="requiredType">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsClassCastRequired(this IType requiredType, OpCodePart opCodePart)
        {
            return opCodePart.Result != null && requiredType != opCodePart.Result.Type && requiredType.IsAssignableFrom(opCodePart.Result.Type);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="doNotConvert">
        /// </param>
        /// <returns>
        /// </returns>
        public static string TypeToCType(this IType type, bool doNotConvert = false)
        {
            var effectiveType = type;

            if (type.IsArray)
            {
                effectiveType = type.GetElementType();
            }

            if (!doNotConvert)
            {
                if (effectiveType.Namespace == "System")
                {
                    string ctype;
                    if (SystemTypesToCTypes.TryGetValue(effectiveType.Name, out ctype))
                    {
                        return ctype;
                    }
                }

                if (type.IsEnum)
                {
                    switch (type.GetTypeSize())
                    {
                        case 1:
                            return "i8";
                        case 2:
                            return "i16";
                        case 4:
                            return "i32";
                        case 8:
                            return "i64";
                    }
                }

                if (type.IsValueType && type.IsPrimitive)
                {
                    return type.Name.ToLowerInvariant();
                }
            }

            return string.Concat('"', type.FullName, '"');
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        /// <param name="asReference">
        /// </param>
        /// <param name="refChar">
        /// </param>
        public static void WriteTypeModifiers(this IType type, IndentedTextWriter writer, bool asReference, char refChar)
        {
            var effectiveType = type;

            do
            {
                var isReference = !effectiveType.IsPrimitive && !effectiveType.IsValueType;
                if ((isReference || asReference) && !effectiveType.IsGenericParameter && !effectiveType.IsArray && !effectiveType.IsByRef)
                {
                    writer.Write(refChar);
                }

                if (effectiveType.IsArray)
                {
                    writer.Write(refChar);
                }

                if (effectiveType.IsByRef)
                {
                    writer.Write(refChar);
                }

                if (effectiveType.HasElementType)
                {
                    effectiveType = effectiveType.GetElementType();
                }
                else
                {
                    break;
                }
            }
            while (effectiveType != null);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        /// <param name="doNotConvert">
        /// </param>
        public static void WriteTypeName(this IType type, LlvmIndentedTextWriter writer, bool doNotConvert = false)
        {
            var typeBaseName = type.TypeToCType(doNotConvert);

            // clean name
            if (typeBaseName.EndsWith("&"))
            {
                typeBaseName = typeBaseName.Substring(0, typeBaseName.Length - 1);
            }

            var index = typeBaseName.IndexOf('`');
            if (index >= 0)
            {
                var nameWithoutGeneric = typeBaseName.Substring(0, index);
                writer.Write(nameWithoutGeneric);
            }
            else
            {
                writer.Write(typeBaseName);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        /// <param name="asReference">
        /// </param>
        /// <param name="doNotIncludeTypePrefixId">
        /// </param>
        /// <param name="refChar">
        /// </param>
        public static void WriteTypePrefix(
            this IType type, LlvmIndentedTextWriter writer, bool asReference = false, bool doNotIncludeTypePrefixId = false, char refChar = '*')
        {
            type.WriteTypeWithoutModifiers(writer, doNotIncludeTypePrefixId);
            type.WriteTypeModifiers(writer, asReference, refChar);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        /// <param name="doNotIncludeTypePrefixId">
        /// </param>
        public static void WriteTypeWithoutModifiers(this IType type, LlvmIndentedTextWriter writer, bool doNotIncludeTypePrefixId = false)
        {
            var effectiveType = type;

            while (effectiveType.HasElementType)
            {
                effectiveType = effectiveType.GetElementType();
            }

            // TODO: remove String test when you use real string class
            if (!doNotIncludeTypePrefixId && !effectiveType.IsPrimitiveType() && !effectiveType.IsVoid() && !effectiveType.IsEnum
                && !(effectiveType.Namespace == "System" && effectiveType.Name == "String"))
            {
                writer.Write('%');
            }

            // write base name
            effectiveType.WriteTypeName(writer);
        }
    }
}