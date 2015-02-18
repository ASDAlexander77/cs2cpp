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
        private static readonly IDictionary<string, string> SystemPointerTypesToCTypes =
            new SortedDictionary<string, string>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, int> SystemTypeSizes = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, string> SystemTypesToCTypes = new SortedDictionary<string, string>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, int> fieldsShiftByType = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, int> sizeByType = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, IList<MemberLocationInfo>> membersLayoutByType =
            new SortedDictionary<string, IList<MemberLocationInfo>>();

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
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static int CalculateFieldsShift(this IType type, ITypeResolver typeResolver)
        {
            var fieldsShift = IlReader.Fields(type, typeResolver).Count(t => !t.IsStatic);
            if (type.BaseType != null)
            {
                fieldsShift += type.BaseType.GetFieldsShift(typeResolver);
            }

            fieldsShiftByType[type.FullName] = fieldsShift;

            return fieldsShift;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static int CalculateSize(this IType type, ITypeResolver typeResolver, out IList<MemberLocationInfo> membersLayout)
        {
            var fieldSizes = type.GetFieldsSizesRecursive(typeResolver, true).ToList();
            var typeAlign = fieldSizes.Any() ? fieldSizes.Max(m => m.Size) : LlvmWriter.PointerSize;
            if (type.BaseType != null)
            {
                typeAlign = Math.Max(typeAlign, LlvmWriter.PointerSize);
            }

            var offset = 0;
            membersLayout = type.GetTypeSizes(typeResolver).ToList();
            foreach (var member in membersLayout)
            {
                member.Offset = offset;
                var size = member.Size;
                var effectiveSize = Math.Min(typeAlign, size);

                offset += size;
                while (effectiveSize > 0 && offset % effectiveSize != 0)
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

        /// <summary>
        /// </summary>
        public static void Clear()
        {
            fieldsShiftByType.Clear();
            sizeByType.Clear();
            membersLayoutByType.Clear();
        }

        public static int GetFieldOffset(this IField field, ITypeResolver typeResolver)
        {
            IList<MemberLocationInfo> membersLayout;
            while (!membersLayoutByType.TryGetValue(field.DeclaringType.ToString(), out membersLayout))
            {
                GetTypeSize(field.DeclaringType, typeResolver);
            }

            var memberLocationInfo =
                membersLayout.FirstOrDefault(m => m.MemberType == MemberTypes.Field && field.Equals((IField)m.Member));
            if (memberLocationInfo == null)
            {
                throw new MissingMemberException(field.FullName);
            }

            return memberLocationInfo.Offset;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static int GetFieldsShift(this IType type, ITypeResolver typeResolver)
        {
            // find index
            int fieldsShift;
            if (!fieldsShiftByType.TryGetValue(type.FullName, out fieldsShift))
            {
                fieldsShift = type.CalculateFieldsShift(typeResolver);
            }

            return fieldsShift;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="excludingStructs">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<MemberLocationInfo> GetFieldsSizes(this IType type, ITypeResolver typeResolver, bool excludingStructs = false)
        {
            foreach (var field in IlReader.Fields(type, typeResolver).Where(t => !t.IsStatic).ToList())
            {
                var fieldSize = 0;
                var fieldType = field.FieldType;
                if (fieldType.IsClass || fieldType.IsArray || fieldType.IsPointer || fieldType.IsDelegate || field.IsFixed)
                {
                    // pointer size
                    if (field.IsFixed)
                    {
                        if (!excludingStructs)
                        {
                            yield return
                                new MemberLocationInfo(
                                    field,
                                    field.FieldType.ToDereferencedType().GetTypeSize(typeResolver, true) * field.FixedSize);
                        }
                        else
                        {
                            yield return
                                new MemberLocationInfo(field, field.FieldType.ToDereferencedType().GetTypeSize(typeResolver, true));
                        }
                    }
                    else
                    {
                        yield return new MemberLocationInfo(field, LlvmWriter.PointerSize);
                    }
                }
                else if (!excludingStructs && fieldType.IsStructureType())
                {
                    yield return new MemberLocationInfo(field, fieldType.GetTypeSize(typeResolver));
                }
                else if (fieldType.Namespace == "System" && SystemTypeSizes.TryGetValue(fieldType.Name, out fieldSize))
                {
                    yield return new MemberLocationInfo(field, fieldSize);
                }
                else
                {
                    foreach (var item in fieldType.GetTypeSizes(typeResolver))
                    {
                        item.SetMainMember(field);
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="excludingStructs">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<MemberLocationInfo> GetFieldsSizesRecursive(
            this IType type,
            ITypeResolver typeResolver,
            bool excludingStructs = false)
        {
            if (type.BaseType != null)
            {
                foreach (var item in type.BaseType.GetFieldsSizes(typeResolver, excludingStructs))
                {
                    yield return item;
                }
            }

            foreach (var item in type.GetFieldsSizes(typeResolver, excludingStructs))
            {
                yield return item;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="asValueType">
        /// </param>
        /// <returns>
        /// </returns>
        public static int GetTypeSize(this IType type, ITypeResolver typeResolver, bool asValueType = false)
        {
            if (asValueType)
            {
                // TODO: do I need to return plain size of structure?

                if (type.IsPrimitiveType())
                {
                    return SystemTypeSizes[type.Name];
                }

                if (!type.IsStructureType() && !type.IsEnum)
                {
                    return LlvmWriter.PointerSize;
                }
            }

            // find index
            int size;
            if (!sizeByType.TryGetValue(type.ToString(), out size))
            {
                IList<MemberLocationInfo> membersLayout;
                size = type.CalculateSize(typeResolver, out membersLayout);
                sizeByType[type.ToString()] = size;
                membersLayoutByType[type.ToString()] = membersLayout;
            }

            return size;
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
        /// <returns>
        /// </returns>
        public static IEnumerable<MemberLocationInfo> GetTypeSizes(this IType type, ITypeResolver typeResolver, bool firstLevel = true)
        {
            if (type.IsInterface)
            {
                var any = false;
                foreach (
                    var item in
                        type.GetInterfacesExcludingBaseAllInterfaces()
                            .SelectMany(interfaceItem => interfaceItem.GetTypeSizes(typeResolver, false)))
                {
                    any = true;
                    yield return item;
                }

                if (!any)
                {
                    yield return new MemberLocationInfo(type, LlvmWriter.PointerSize);
                }

                yield break;
            }

            if (type.IsPointer)
            {
                // type*
                yield return new MemberLocationInfo(type, LlvmWriter.PointerSize);
                yield break;
            }

            if (!firstLevel && type.IsArray)
            {
                // type*
                yield return new MemberLocationInfo(type, LlvmWriter.PointerSize);
                yield break;
            }

            if (!firstLevel && type.IsEnum)
            {
                var enumUnderlyingType = type.GetEnumUnderlyingType();
                int enumUnderlyingTypeFieldSize;
                if (enumUnderlyingType.Namespace == "System" &&
                    SystemTypeSizes.TryGetValue(enumUnderlyingType.Name, out enumUnderlyingTypeFieldSize))
                {
                    yield return new MemberLocationInfo(type, enumUnderlyingTypeFieldSize);
                }

                yield break;
            }

            // add shift for virtual table
            if (type.IsRootOfVirtualTable(typeResolver))
            {
                yield return new MemberLocationInfo(LlvmWriter.PointerSize);
            }

            if (type.BaseType != null)
            {
                foreach (var item in type.BaseType.GetTypeSizes(typeResolver, false))
                {
                    yield return item;
                }
            }

            // add shift for interfaces
            foreach (
                var item in
                    type.GetInterfacesExcludingBaseAllInterfaces()
                        .SelectMany(interfaceItem => interfaceItem.GetTypeSizes(typeResolver, false)))
            {
                yield return item;
            }

            foreach (var item in type.GetFieldsSizes(typeResolver))
            {
                yield return item;
            }
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
        public static bool IsClassCastRequired(
            this IType requiredType,
            OpCodePart opCodePart,
            out bool dynamicCastRequired)
        {
            dynamicCastRequired = false;

            var resultType = opCodePart.Result.Type;
            var other = resultType.ToDereferencedType();
            var constValue = opCodePart.Result as ConstValue;
            if (constValue != null && constValue.IsNull)
            {
                return false;
            }

            if ((resultType.IsClass || resultType.IsPointer || resultType.IsByRef) && requiredType.IsByRef)
            {
                return requiredType.GetElementType().TypeNotEquals(other);
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
        /// <param name="isPointerOpt">
        /// </param>
        /// <returns>
        /// </returns>
        public static string TypeToCType(this IType type, bool? isPointerOpt = null)
        {
            var isPointer = isPointerOpt.HasValue ? isPointerOpt.Value : type.IsPointer;

            var effectiveType = type;

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

            if (effectiveType.IsByRef)
            {
                effectiveType = effectiveType.GetElementType();
            }

            var level = 0;
            do
            {
                var isReference = !effectiveType.IsValueType;
                if ((isReference || (!isReference && asReference && level == 0 && !type.IsByRef) || effectiveType.IsPointer) && !effectiveType.IsGenericParameter)
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
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="asReference">
        /// </param>
        public static void WriteTypePrefix(this IType type, LlvmWriter llvmWriter, bool asReference = false)
        {
            LlvmIndentedTextWriter writer = llvmWriter.Output;

            type.WriteTypeWithoutModifiers(llvmWriter);
            type.WriteTypeModifiers(writer, asReference);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteTypeWithoutModifiers(
            this IType type,
            LlvmWriter llvmWriter,
            bool isPointer = false)
        {
            LlvmIndentedTextWriter writer = llvmWriter.Output;

            var effectiveType = type;

            if (effectiveType.IsPointer)
            {
                effectiveType.GetElementType().WriteTypeWithoutModifiers(llvmWriter, type.IsPointer);
                return;
            }

            if (type.UseAsClass ||
                !effectiveType.IsPrimitiveType() && !effectiveType.IsVoid() && !effectiveType.IsEnum)
            {
                writer.Write('%');
            }

            // write base name
            effectiveType.WriteTypeName(writer, isPointer);
        }
    }
}