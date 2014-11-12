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
    using System;

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
        private static readonly IDictionary<string, string> SystemPointerTypesToCTypes = new SortedDictionary<string, string>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, int> sizeByType = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, int> fieldsShiftByType = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        static TypeGen()
        {
            SystemPointerTypesToCTypes["Void"] = "i8";

            SystemTypesToCTypes["Void"] = "void";
            SystemTypesToCTypes["Void*"] = "i8";
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
            SystemTypesToCTypes["UInt16&"] = "i16**";
            SystemTypesToCTypes["UInt32&"] = "i32**";
            SystemTypesToCTypes["UInt64&"] = "i64*";
            SystemTypesToCTypes["Float&"] = "float*";
            SystemTypesToCTypes["Single&"] = "float*";
            SystemTypesToCTypes["Double&"] = "double*";
            SystemTypesToCTypes["Boolean&"] = "i1*";

            SystemTypeSizes["Void"] = 0;
            SystemTypeSizes["Void*"] = LlvmWriter.PointerSize;
            SystemTypeSizes["Byte"] = 1;
            SystemTypeSizes["SByte"] = 1;
            SystemTypeSizes["Char"] = 2;
            SystemTypeSizes["Int16"] = 2;
            SystemTypeSizes["Int32"] = LlvmWriter.PointerSize;
            SystemTypeSizes["Int64"] = 8;
            SystemTypeSizes["UInt16"] = 2;
            SystemTypeSizes["UInt32"] = LlvmWriter.PointerSize;
            SystemTypeSizes["UInt64"] = 8;
            SystemTypeSizes["Float"] = LlvmWriter.PointerSize;
            SystemTypeSizes["Single"] = LlvmWriter.PointerSize;
            SystemTypeSizes["Double"] = 8;
            SystemTypeSizes["Boolean"] = 1;
            SystemTypeSizes["Byte&"] = LlvmWriter.PointerSize;
            SystemTypeSizes["SByte&"] = LlvmWriter.PointerSize;
            SystemTypeSizes["Char&"] = LlvmWriter.PointerSize;
            SystemTypeSizes["Int16&"] = LlvmWriter.PointerSize;
            SystemTypeSizes["Int32&"] = LlvmWriter.PointerSize;
            SystemTypeSizes["Int64&"] = LlvmWriter.PointerSize;
            SystemTypeSizes["UInt16&"] = LlvmWriter.PointerSize;
            SystemTypeSizes["UInt32&"] = LlvmWriter.PointerSize;
            SystemTypeSizes["UInt64&"] = LlvmWriter.PointerSize;
            SystemTypeSizes["Float&"] = LlvmWriter.PointerSize;
            SystemTypeSizes["Single&"] = LlvmWriter.PointerSize;
            SystemTypeSizes["Double&"] = LlvmWriter.PointerSize;
            SystemTypeSizes["Boolean&"] = LlvmWriter.PointerSize;
        }

        public static int CalculateSize(this IType type)
        {
            var fieldSizes = type.GetFieldsSizesRecursive(true).ToList();
            var typeAlign = fieldSizes.Any() ? fieldSizes.Max() : LlvmWriter.PointerSize;
            if (type.BaseType != null)
            {
                typeAlign = Math.Max(typeAlign, LlvmWriter.PointerSize);
            }

            var offset = 0;
            foreach (var size in type.GetTypeSizes())
            {
                var effectiveSize = Math.Min(typeAlign, size);

                offset += size;
                while (offset % effectiveSize != 0)
                {
                    offset++;
                }
            }

            var alignToApply = offset % typeAlign;
            if (alignToApply > 0)
            {
                offset += typeAlign - alignToApply;
            }

            return offset;
        }

        public static IEnumerable<int> GetTypeSizes(this IType type)
        {
            if (type.IsInterface)
            {
                var any = false;
                foreach (var interfaceItem in type.GetInterfacesExcludingBaseAllInterfaces())
                {
                    foreach (var item in interfaceItem.GetTypeSizes())
                    {
                        any = true;
                        yield return item;
                    }
                }

                if (!any)
                {
                    yield return LlvmWriter.PointerSize;
                }

                yield break;
            }

            if (type.IsArray || type.IsPointer)
            {
                // type*
                yield return LlvmWriter.PointerSize;
                yield break;
            }

            if (type.IsEnum)
            {
                var enumUnderlyingType = type.GetEnumUnderlyingType();
                int enumUnderlyingTypeFieldSize;
                if (enumUnderlyingType.Namespace == "System" && SystemTypeSizes.TryGetValue(enumUnderlyingType.Name, out enumUnderlyingTypeFieldSize))
                {
                    yield return enumUnderlyingTypeFieldSize;
                }

                yield break;
            }

            // add shift for virtual table
            if (type.IsRootOfVirtualTable())
            {
                yield return LlvmWriter.PointerSize;
            }

            if (type.BaseType != null)
            {
                foreach (var item in type.BaseType.GetTypeSizes())
                {
                    yield return item;
                }
            }

            // add shift for interfaces
            foreach (var interfaceItem in type.GetInterfacesExcludingBaseAllInterfaces())
            {
                foreach (var item in interfaceItem.GetTypeSizes())
                {
                    yield return item;
                }
            }

            foreach (var item in type.GetFieldsSizes())
            {
                yield return item;
            }
        }

        public static IEnumerable<int> GetFieldsSizes(this IType type, bool excludingStructs = false)
        {
            foreach (var field in IlReader.Fields(type).Where(t => !t.IsStatic).ToList())
            {
                var fieldSize = 0;
                var fieldType = field.FieldType;
                if (fieldType.IsClass || fieldType.IsArray || fieldType.IsPointer || fieldType.IsDelegate)
                {
                    // pointer size
                    yield return LlvmWriter.PointerSize;
                }
                else if (!excludingStructs && fieldType.IsStructureType())
                {
                    yield return fieldType.GetTypeSize();
                }
                else if (fieldType.Namespace == "System" && SystemTypeSizes.TryGetValue(fieldType.Name, out fieldSize))
                {
                    yield return fieldSize;
                }
                else
                {
                    foreach (var item in fieldType.GetTypeSizes())
                    {
                        yield return item;
                    }
                }
            }
        }

        public static IEnumerable<int> GetFieldsSizesRecursive(this IType type, bool excludingStructs = false)
        {
            if (type.BaseType != null)
            {
                foreach (var item in type.BaseType.GetFieldsSizes(excludingStructs))
                {
                    yield return item;
                }
            }

            foreach (var item in type.GetFieldsSizes(excludingStructs))
            {
                yield return item;
            }
        }

        public static int CalculateFieldsShift(this IType type)
        {
            var fieldsShift = IlReader.Fields(type).Count(t => !t.IsStatic);
            if (type.BaseType != null)
            {
                fieldsShift += type.BaseType.GetFieldsShift();
            }

            fieldsShiftByType[type.FullName] = fieldsShift;

            return fieldsShift;
        }

        /// <summary>
        /// </summary>
        public static void Clear()
        {
            sizeByType.Clear();
        }


        public static int GetTypeSize(this IType type, bool asValueType = false)
        {
            if (asValueType && type.IsPrimitiveType())
            {
                return SystemTypeSizes[type.Name];
            }

            // find index
            int size;
            if (!sizeByType.TryGetValue(type.FullName, out size))
            {
                size = type.CalculateSize();
                sizeByType[type.FullName] = size;
            }

            return size;
        }

        public static int GetFieldsShift(this IType type)
        {
            // find index
            int fieldsShift;
            if (!fieldsShiftByType.TryGetValue(type.FullName, out fieldsShift))
            {
                fieldsShift = type.CalculateFieldsShift();
            }

            return fieldsShift;
        }

        /// <summary>
        /// </summary>
        /// <param name="requiredType">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="dynamicCastRequired">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsClassCastRequired(this IType requiredType, OpCodePart opCodePart, out bool dynamicCastRequired)
        {
            dynamicCastRequired = false;

            var other = opCodePart.Result.Type.ToDereferencedType();
            var constValue = opCodePart.Result as ConstValue;
            if (constValue != null && constValue.IsNull)
            {
                return false;
            }

            if (requiredType.TypeNotEquals(other))
            {
                if (requiredType.IsAssignableFrom(other) || other.IsArray && requiredType.FullName == "System.Array")
                {
                    return true;
                }

                dynamicCastRequired = true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static string TypeToCType(this IType type, bool? isPointerOpt = null)
        {
            var isPointer = isPointerOpt.HasValue ? isPointerOpt.Value : type.IsPointer;

            var effectiveType = type;

            if (type.IsArray)
            {
                effectiveType = type.GetElementType();
            }

            if (!type.UseAsClass)
            {
                if (effectiveType.Namespace == "System")
                {
                    string ctype;

                    if (isPointer && SystemPointerTypesToCTypes.TryGetValue(effectiveType.Name, out ctype))
                    {
                        return ctype;
                    }

                    if (SystemTypesToCTypes.TryGetValue(effectiveType.Name, out ctype))
                    {
                        return ctype;
                    }
                }

                if (type.IsEnum)
                {
                    switch (type.GetEnumUnderlyingType().FullName)
                    {
                        case "System.SByte":
                        case "System.Byte":
                            return "i8";
                        case "System.Int16":
                        case "System.UInt16":
                            return "i16";
                        case "System.Int32":
                        case "System.UInt32":
                            return "i32";
                        case "System.Int64":
                        case "System.UInt64":
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
        public static void WriteTypeModifiers(this IType type, IndentedTextWriter writer, bool asReference)
        {
            var refChar = '*';
            var effectiveType = type;

            if (type.IsArray)
            {
                writer.Write(refChar);

                if (type.IsByRef)
                {
                    writer.Write(refChar);
                }

                return;
            }

            var level = 0;
            do
            {
                var isReference = !effectiveType.IsValueType;
                if ((isReference || (!isReference && asReference && level == 0) || effectiveType.IsPointer) && !effectiveType.IsGenericParameter
                    && !effectiveType.IsArray && !effectiveType.IsByRef)
                {
                    writer.Write(refChar);
                }

                if (effectiveType.IsByRef || effectiveType.IsArray)
                {
                    writer.Write(refChar);
                }

                if (effectiveType.HasElementType)
                {
                    effectiveType = effectiveType.GetElementType();
                    level++;
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
        public static void WriteTypeName(this IType type, LlvmIndentedTextWriter writer, bool isPointer)
        {
            var typeBaseName = type.TypeToCType(isPointer);

            // clean name
            if (typeBaseName.EndsWith("&"))
            {
                typeBaseName = typeBaseName.Substring(0, typeBaseName.Length - 1);
            }

            writer.Write(typeBaseName);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        /// <param name="asReference">
        /// </param>
        public static void WriteTypePrefix(this IType type, LlvmIndentedTextWriter writer, bool asReference = false)
        {
            type.WriteTypeWithoutModifiers(writer);
            type.WriteTypeModifiers(writer, asReference);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteTypeWithoutModifiers(this IType type, LlvmIndentedTextWriter writer)
        {
            var effectiveType = type;

            while (effectiveType.HasElementType)
            {
                effectiveType = effectiveType.GetElementType();
            }

            if (!type.IsArray)
            {
                if (type.UseAsClass || !effectiveType.IsPrimitiveType() && !effectiveType.IsVoid() && !effectiveType.IsEnum)
                {
                    writer.Write('%');
                }

                // write base name
                effectiveType.WriteTypeName(writer, type.IsPointer);
            }
            else
            {
                writer.Write("{1} {2}, [ {0} x ", 0, "{", ArraySingleDimensionGen.GetArrayPrefixDataType());

                effectiveType = type;

                if (effectiveType.IsByRef)
                {
                    effectiveType = effectiveType.GetElementType();
                }

                effectiveType.GetElementType().WriteTypePrefix(writer);

                writer.Write(" ] }");
            }
        }
    }
}