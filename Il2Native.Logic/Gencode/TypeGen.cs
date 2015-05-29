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
        private static readonly IDictionary<string, int> SystemTypeSizes = new SortedDictionary<string, int>();

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
            var typeAlign = fieldSizes.Any() ? fieldSizes.Max(m => m.Size) : CWriter.PointerSize;
            if (type.BaseType != null)
            {
                typeAlign = Math.Max(typeAlign, CWriter.PointerSize);
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

            Debug.Assert(typeAlign != 0, "typeAlign can't be 0");

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
                membersLayout.FirstOrDefault(m => m.MemberType == MemberTypes.Field && field.Name == ((IField)m.Member).Name);
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
            foreach (var @interface in type.GetInterfacesExcludingBaseAllInterfaces())
            {
                yield return new MemberLocationInfo(@interface, CWriter.PointerSize);
            }

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
                                    field.FieldType.GetElementType().GetTypeSize(typeResolver, true) * field.FixedSize);
                        }
                        else
                        {
                            yield return
                                new MemberLocationInfo(field, field.FieldType.GetElementType().GetTypeSize(typeResolver, true));
                        }
                    }
                    else
                    {
                        yield return new MemberLocationInfo(field, CWriter.PointerSize);
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
                    return CWriter.PointerSize;
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

                yield break;
            }

            if (type.IsPointer)
            {
                // type*
                yield return new MemberLocationInfo(type, CWriter.PointerSize);
                yield break;
            }

            if (!firstLevel && type.IsArray)
            {
                // type*
                yield return new MemberLocationInfo(type, CWriter.PointerSize);
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
        /// <param name="type">
        /// </param>
        /// <param name="isPointerOpt">
        /// </param>
        /// <returns>
        /// </returns>
        public static string TypeToCType(this IType type, bool? isPointerOpt = null, bool enumAsName = false, bool shortName = false)
        {
            if (!type.UseAsClass)
            {
                if (type.IsEnum && !enumAsName)
                {
                    return TypeToCType(type.GetEnumUnderlyingType(), isPointerOpt);
                }

                if (type.IsValueType && type.IsPrimitive || type.IsVoid())
                {
                    return type.Name;
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
        public static void WriteTypePrefix(this IType type, CWriter codeWriter, bool asReference = false, bool enumAsName = false)
        {
            var writer = codeWriter.Output;

            Debug.Assert(type != null, "Type can't be null to write");

            type.WriteTypeWithoutModifiers(codeWriter, enumAsName: enumAsName);
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
            CWriter codeWriter,
            bool isPointer = false,
            bool enumAsName = false)
        {
            var writer = codeWriter.Output;

            var effectiveType = type;

            if (effectiveType.IsPointer)
            {
                effectiveType.GetElementType().WriteTypeWithoutModifiers(codeWriter, type.IsPointer, enumAsName);
                return;
            }

            // write base name
            effectiveType.WriteTypeName(writer, isPointer, enumAsName);
        }
    }
}