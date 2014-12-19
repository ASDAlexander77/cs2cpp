// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeExtentions.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Il2Native.Logic
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection.Emit;
    using System.Text;

    using Il2Native.Logic.CodeParts;
    using Il2Native.Logic.Gencode;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class OpCodeExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="codes">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool Any(this OpCodePart opCode, params Code[] codes)
        {
            if (opCode == null)
            {
                return false;
            }

            var code = opCode.ToCode();
            return codes.Any(item => item == code);
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="usedTypes">
        /// </param>
        /// <param name="calledMethods">
        /// </param>
        /// <param name="readStaticFields">
        /// </param>
        public static void DiscoverMethod(this IMethod method, ISet<IType> usedTypes, ISet<IMethod> calledMethods, ISet<IField> readStaticFields)
        {
            // read method body to extract all types
            var reader = new IlReader();

            reader.UsedTypes = usedTypes;
            reader.CalledMethods = calledMethods;
            reader.UsedStaticFieldsToRead = readStaticFields;

            var genericContext = MetadataGenericContext.DiscoverFrom(method);
            foreach (var op in reader.OpCodes(method, genericContext, new Queue<IMethod>()))
            {
                ;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="genericTypeSpecializations">
        /// </param>
        /// <param name="genericMethodSpecializations">
        /// </param>
        /// <param name="requiredTypes">
        /// </param>
        /// <param name="stackCall">
        /// </param>
        public static void DiscoverRequiredTypesAndMethodsInMethodBody(
            this IMethod method,
            ISet<IType> genericTypeSpecializations,
            ISet<IMethod> genericMethodSpecializations,
            ISet<IType> requiredTypes,
            Queue<IMethod> stackCall)
        {
            // read method body to extract all types
            var reader = new IlReader();

            reader.UsedStructTypes = requiredTypes;
            reader.UsedGenericSpecialiazedTypes = genericTypeSpecializations;
            reader.UsedGenericSpecialiazedMethods = genericMethodSpecializations;

            var genericContext = MetadataGenericContext.DiscoverFrom(method, false); // true
            foreach (var op in reader.OpCodes(method, genericContext, stackCall))
            {
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="baseWriter">
        /// </param>
        /// <returns>
        /// </returns>
        public static int GetArgCount(this BaseWriter baseWriter)
        {
            return baseWriter.Parameters.Count() - (baseWriter.HasMethodThis ? 1 : 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        public static int GetArgIndex(this OpCodePart opCode)
        {
            var asString = opCode.ToCode().ToString();
            var index = -1;
            if (opCode.Any(Code.Ldarg_S, Code.Ldarg, Code.Ldarga_S, Code.Ldarga, Code.Starg_S, Code.Starg))
            {
                var opCodeInt32 = opCode as OpCodeInt32Part;
                index = opCodeInt32.Operand;
            }
            else
            {
                index = int.Parse(asString.Substring(asString.Length - 1));
            }

            return index;
        }

        /// <summary>
        /// </summary>
        /// <param name="baseWriter">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetArgName(this BaseWriter baseWriter, int index)
        {
            var parameter = baseWriter.Parameters[index - (baseWriter.HasMethodThis ? 1 : 0)];
            return parameter.Name;
        }

        /// <summary>
        /// </summary>
        /// <param name="baseWriter">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        public static IType GetArgType(this BaseWriter baseWriter, int index)
        {
            var parameter = baseWriter.Parameters[index - (baseWriter.HasMethodThis ? 1 : 0)];
            var parameterType = parameter.ParameterType;
            return parameterType;
        }

        /// <summary>
        /// </summary>
        /// <param name="classType">
        /// </param>
        /// <param name="number">
        /// </param>
        /// <returns>
        /// </returns>
        public static IType GetFieldTypeByFieldNumber(this IType classType, int number)
        {
            var normalType = classType.ToNormal();

            if (normalType.IsEnum)
            {
                return normalType.GetEnumUnderlyingType();
            }

            var field = IlReader.Fields(normalType).Where(t => !t.IsStatic).Skip(number).FirstOrDefault();
            if (field == null)
            {
                return null;
            }

            return field.FieldType;
        }

        public static IField GetFieldByName(this IType classType, string fieldName)
        {
            var normalType = classType.ToNormal();
            var field = IlReader.Fields(normalType).FirstOrDefault(f => f.Name == fieldName);
            return field;
        }

        /// <summary>
        /// </summary>
        /// <param name="field">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetFullName(this IField field)
        {
            var sb = new StringBuilder();

            sb.Append(field.DeclaringType);
            sb.Append('.');
            sb.Append(field.Name);

            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="thisTypeString">
        /// </param>
        /// <returns>
        /// </returns>
        public static int GetIntSizeBits(this string thisTypeString)
        {
            if (string.IsNullOrWhiteSpace(thisTypeString) || thisTypeString[0] != 'i')
            {
                return 0;
            }

            int size;
            return int.TryParse(thisTypeString.Substring(1), out size) ? size : 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="baseWriter">
        /// </param>
        /// <returns>
        /// </returns>
        public static IType GetLocalType(this OpCodePart opCode, BaseWriter baseWriter)
        {
            var asString = opCode.ToCode().ToString();
            var index = -1;
            if (opCode.Any(Code.Ldloc_S, Code.Ldloc, Code.Ldloca_S, Code.Ldloca) || opCode.Any(Code.Stloc_S, Code.Stloc))
            {
                index = (opCode as OpCodeInt32Part).Operand;
            }
            else
            {
                index = int.Parse(asString.Substring(asString.Length - 1));
            }

            Debug.Assert(baseWriter.LocalInfo.Length > index);

            var localType = baseWriter.LocalInfo[index].LocalType;
            return localType;
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool HasAnyVirtualMethod(this IType thisType)
        {
            if (thisType.HasAnyVirtualMethodInCurrentType())
            {
                return true;
            }

            return thisType.BaseType != null && thisType.BaseType.HasAnyVirtualMethod();
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool HasAnyVirtualMethodInCurrentType(this IType thisType)
        {
            if (IlReader.Methods(thisType).Any(m => m.IsVirtual || m.IsAbstract))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <param name="isPointer">
        /// </param>
        /// <returns>
        /// </returns>
        public static int IntTypeBitSize(this IType thisType, bool isPointer = false)
        {
            var thisTypeString = thisType.TypeToCType(isPointer);
            return GetIntSizeBits(thisTypeString);
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsAnyBranch(this OpCodePart opCodePart)
        {
            return opCodePart.OpCode.FlowControl == FlowControl.Cond_Branch || opCodePart.OpCode.FlowControl == FlowControl.Branch;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsBranch(this OpCodePart opCodePart)
        {
            return opCodePart.OpCode.FlowControl == FlowControl.Branch && opCodePart.ToCode() != Code.Switch;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsCondBranch(this OpCodePart opCodePart)
        {
            return opCodePart.OpCode.FlowControl == FlowControl.Cond_Branch && opCodePart.ToCode() != Code.Switch && opCodePart.ToCode() != Code.Leave
                   && opCodePart.ToCode() != Code.Leave_S;
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsDerivedFrom(this IType thisType, IType type)
        {
            Debug.Assert(type != null);

            if (thisType == type)
            {
                return false;
            }

            var t = thisType.BaseType;
            while (t != null)
            {
                if (type.TypeEquals(t))
                {
                    return true;
                }

                t = t.BaseType;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsDouble(this IType type)
        {
            return type != null && type.Name == "Double" && type.Namespace == "System";
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsExternalLibraryMethod(this IMethod method)
        {
            return method.IsUnmanaged && !method.IsUnmanagedMethodReference && !method.Name.StartsWith("llvm_");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsFloat(this IType type)
        {
            return type != null && type.Name == "Float" && type.Namespace == "System";
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsIntValueTypeExtCastRequired(this IType thisType, IType type)
        {
            var thisTypeString = thisType.TypeToCType();
            var typeString = type.TypeToCType();

            var thisTypeSize = GetIntSizeBits(thisTypeString);
            var typeSize = GetIntSizeBits(typeString);

            if (thisTypeSize == 0 || typeSize == 0)
            {
                return false;
            }

            return thisTypeSize > typeSize;
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsIntValueTypeTruncCastRequired(this IType thisType, IType type)
        {
            var thisTypeString = thisType.TypeToCType();
            var typeString = type.TypeToCType();

            var thisTypeSize = GetIntSizeBits(thisTypeString);
            var typeSize = GetIntSizeBits(typeString);

            if (thisTypeSize == 0 || typeSize == 0)
            {
                return false;
            }

            return thisTypeSize < typeSize;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsJumpForward(this OpCodePart opCodePart)
        {
            var opCode = opCodePart as OpCodeInt32Part;
            if (opCode == null)
            {
                return false;
            }

            if (opCode.OpCode.OperandType == OperandType.ShortInlineBrTarget && (opCode.Operand & 0x80) == 0x80)
            {
                return false;
            }

            return opCode.Operand > 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="interfaceMember">
        /// </param>
        /// <param name="publicMethod">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsMatchingExplicitInterfaceOverride(this IMethod interfaceMember, IMethod publicMethod)
        {
            if (interfaceMember.ExplicitName == publicMethod.Name)
            {
                return interfaceMember.IsMatchingParamsAndReturnType(publicMethod);
            }

            if (interfaceMember.FullName == publicMethod.Name)
            {
                return interfaceMember.IsMatchingParamsAndReturnType(publicMethod);
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="genericMethod">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsMatchingGeneric(this IMethod method, IMethod genericMethod)
        {
            if (method.MetadataName == genericMethod.MetadataName && method.DeclaringType.TypeEquals(genericMethod.DeclaringType))
            {
                return method.IsMatchingGenericParamsAndReturnType(genericMethod);
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="interfaceMember">
        /// </param>
        /// <param name="publicMethod">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsMatchingInterfaceOverride(this IMethod interfaceMember, IMethod publicMethod)
        {
            if (interfaceMember.Name == publicMethod.Name)
            {
                return interfaceMember.IsMatchingParamsAndReturnType(publicMethod);
            }

            if (interfaceMember.ExplicitName == publicMethod.Name)
            {
                return interfaceMember.IsMatchingParamsAndReturnType(publicMethod);
            }

            if (interfaceMember.FullName == publicMethod.Name)
            {
                return interfaceMember.IsMatchingParamsAndReturnType(publicMethod);
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="overridingMethod">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsMatchingOverride(this IMethod method, IMethod overridingMethod)
        {
            if (method.MetadataName == overridingMethod.MetadataName)
            {
                return method.IsMatchingParamsAndReturnType(overridingMethod);
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsPrimitiveType(this IType type)
        {
            return type != null && type.IsValueType && type.IsPrimitive;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsPrimitiveTypeOrEnum(this IType type)
        {
            return type != null && type.IsValueType && (type.IsPrimitive || type.IsEnum);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsReal(this IType type)
        {
            return type.IsFloat() || type.IsDouble() || type.IsSingle();
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsReturn(this OpCodePart opCodePart)
        {
            return opCodePart.OpCode.FlowControl == FlowControl.Return && opCodePart.ToCode() != Code.Endfinally;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsRootInterface(this IType type)
        {
            return type.IsInterface && !type.GetInterfaces().Any();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsRootOfVirtualTable(this IType type)
        {
            return !type.IsInterface && type.HasAnyVirtualMethodInCurrentType() && (type.BaseType == null || !type.BaseType.HasAnyVirtualMethod());
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsSignedType(this IType thisType)
        {
            switch (thisType.FullName)
            {
                case "System.SByte":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.Single":
                case "System.Double":
                    return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsSingle(this IType type)
        {
            return type != null && type.Name == "Single" && type.Namespace == "System";
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="recurse">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsStructureType(this IType type, bool recurse = false)
        {
            return type != null
                   && (type.IsValueType && !type.IsEnum && !type.IsPrimitive && !type.IsVoid() && !type.IsPointer && !type.IsByRef
                       || recurse && type.HasElementType && type.GetElementType().IsStructureType(recurse));
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsUnsigned(this OpCodePart opCode)
        {
            if (opCode.OpCodeOperands == null || opCode.OpCodeOperands.Length == 0)
            {
                return false;
            }

            var result1 = opCode.OpCodeOperands[0].Result;
            var result2 = opCode.OpCodeOperands[1].Result;
            if (result2 is ConstValue)
            {
                return result1.Type.IsUnsignedType();
            }

            if (result1 is ConstValue)
            {
                return result2.Type.IsUnsignedType();
            }

            return result1.Type.IsUnsignedType() && result2.Type.IsUnsignedType();
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsUnsignedType(this IType thisType)
        {
            switch (thisType.FullName)
            {
                case "System.Byte":
                case "System.Char":
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                    return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsValueType(this IType type)
        {
            return type != null && type.IsValueType;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsVoid(this IType type)
        {
            return type != null && type.Name == "Void" && type.Namespace == "System";
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        public static int JumpAddress(this OpCodePart opCodePart)
        {
            var opCode = opCodePart as OpCodeInt32Part;

            if (opCode.OpCode.OperandType == OperandType.ShortInlineBrTarget && (opCode.Operand & 0x80) == 0x80)
            {
                return opCode.AddressEnd - (255 - (short)opCode.Operand + 1);
            }

            return opCode.Operand + opCode.AddressEnd;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodeLabelsPart">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        public static int JumpAddress(this OpCodeLabelsPart opCodeLabelsPart, int index)
        {
            return opCodeLabelsPart.Operand[index] + opCodeLabelsPart.AddressEnd;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="baseWriter">
        /// </param>
        /// <returns>
        /// </returns>
        public static OpCodePart JumpOpCode(this OpCodePart opCode, BaseWriter baseWriter)
        {
            var jumpAddress = opCode.JumpAddress();
            OpCodePart stopForBranch;
            if (baseWriter.OpsByAddressStart.TryGetValue(jumpAddress, out stopForBranch))
            {
                return stopForBranch;
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="other">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool NameEquals(this IName type, IName other)
        {
            return type != null && other.CompareTo(type) == 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="other">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool NameNotEquals(this IName type, IName other)
        {
            return !type.NameEquals(other);
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="baseWriter">
        /// </param>
        /// <returns>
        /// </returns>
        public static OpCodePart NextOpCodeGroup(this OpCodePart opCode, BaseWriter baseWriter)
        {
            OpCodePart ret = null;
            baseWriter.OpsByGroupAddressStart.TryGetValue(opCode.GroupAddressEnd, out ret);
            return ret;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="baseWriter">
        /// </param>
        /// <returns>
        /// </returns>
        public static OpCodePart PreviousOpCodeGroup(this OpCodePart opCode, BaseWriter baseWriter)
        {
            OpCodePart ret = null;
            baseWriter.OpsByGroupAddressEnd.TryGetValue(opCode.GroupAddressStart, out ret);
            return ret;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<IType> SelectAllNestedChildrenExceptFirstInterfaces(this IType type)
        {
            var index = 0;
            foreach (var childInterface in type.GetInterfacesExcludingBaseAllInterfaces())
            {
                if (index++ != 0)
                {
                    yield return childInterface;
                }

                foreach (var subChildInterface in childInterface.SelectAllNestedChildrenExceptFirstInterfaces())
                {
                    yield return subChildInterface;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<IType> SelectAllTopAndAllNotFirstChildrenInterfaces(this IType type)
        {
            foreach (var topInterface in type.GetInterfacesExcludingBaseAllInterfaces())
            {
                yield return topInterface;

                // enumerate all children except first
                foreach (var notFirstChild in topInterface.SelectAllNestedChildrenExceptFirstInterfaces())
                {
                    yield return notFirstChild;
                }
            }

            if (type.BaseType != null)
            {
                foreach (var baseInterface in type.BaseType.SelectAllTopAndAllNotFirstChildrenInterfaces())
                {
                    yield return baseInterface;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static IType ToBareType(this IType type)
        {
            var current = type;
            while (current.HasElementType)
            {
                current = current.GetElementType();
            }

            return current;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        public static Code ToCode(this OpCodePart opCode)
        {
            var val = opCode.OpCode.Value;
            if (val < 0xE1 && val >= 0)
            {
                return (Code)val;
            }

            return (Code)(val - (val >> 8 << 8) + 0xE1);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="other">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool TypeEquals(this IType type, IType other)
        {
            return type != null && other.CompareTo(type) == 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="other">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool TypeNotEquals(this IType type, IType other)
        {
            return !type.TypeEquals(other);
        }

        /// <summary>
        /// </summary>
        /// <param name="p1">
        /// </param>
        /// <param name="p2">
        /// </param>
        /// <returns>
        /// </returns>
        private static bool CompareTypeParam(IType p1, IType p2)
        {
            if (p1.IsGenericParameter && p2.IsGenericParameter && !p1.Equals(p2))
            {
                return false;
            }

            if (p1.IsGenericParameter || p2.IsGenericParameter)
            {
                return true;
            }

            if (!p1.Equals(p2))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="genType">
        /// </param>
        /// <param name="testVoid">
        /// </param>
        /// <returns>
        /// </returns>
        private static bool CompareTypeWithGenericType(IType type, IType genType, bool testVoid = false)
        {
            if (testVoid)
            {
                if (type.IsVoid() && genType.IsVoid())
                {
                    return true;
                }

                if (type.IsVoid() || genType.IsVoid())
                {
                    return false;
                }
            }

            if (genType.IsArray && type.IsArray)
            {
                return true;
            }

            if (genType.IsPointer && type.IsPointer)
            {
                return true;
            }

            if ((genType.IsGenericTypeDefinition || type.IsGenericTypeDefinition) && genType.MetadataFullName.Equals(type.MetadataFullName))
            {
                return true;
            }

            if (genType.FullName.Equals(type.FullName))
            {
                return true;
            }

            return genType.IsGenericParameter || type.IsGenericParameter;
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="genericMethod">
        /// </param>
        /// <returns>
        /// </returns>
        private static bool IsMatchingGenericParamsAndReturnType(this IMethod method, IMethod genericMethod)
        {
            var params1 = method.GetParameters().ToArray();
            var genParams2 = genericMethod.GetParameters().ToArray();

            if (params1.Length != genParams2.Length)
            {
                return false;
            }

            for (var i = 0; i < params1.Length; i++)
            {
                if (params1[i].Name != genParams2[i].Name)
                {
                    return false;
                }
            }

            for (var i = 0; i < params1.Length; i++)
            {
                if (params1[i].IsOut != genParams2[i].IsOut || params1[i].IsRef != genParams2[i].IsRef
                    || !CompareTypeWithGenericType(params1[i].ParameterType, genParams2[i].ParameterType))
                {
                    return false;
                }
            }

            if (CompareTypeWithGenericType(method.ReturnType, genericMethod.ReturnType, true))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="overridingMethod">
        /// </param>
        /// <returns>
        /// </returns>
        private static bool IsMatchingParamsAndReturnType(this IMethod method, IMethod overridingMethod)
        {
            var params1 = method.GetParameters().ToArray();
            var params2 = overridingMethod.GetParameters().ToArray();

            if (params1.Length != params2.Length)
            {
                return false;
            }

            var count = params1.Length;
            for (var index = 0; index < count; index++)
            {
                var p1 = params1[index].ParameterType;
                var p2 = params2[index].ParameterType;
                if (!CompareTypeParam(p1, p2))
                {
                    return false;
                }
            }

            return CompareTypeParam(method.ReturnType, overridingMethod.ReturnType);
        }

        public static int? FindBeginOfBasicBlock(this OpCodePart popCodePart)
        {
            // add alternative stack value to and address
            // 1) find jump address
            var current = popCodePart;

            // Varpop can pop 0
            var jumpOrLabel = false;
            while (current != null && !(JumpOrLabelPoint(current, out jumpOrLabel)))
            {
                current = current.Previous;
            }

            if (current != null && JumpOrLabelPoint(current, out jumpOrLabel))
            {
                var nextAlternateValues = current.Next != null ? current.Next.AlternativeValues : null;
                if (nextAlternateValues != null && nextAlternateValues.Values.Any(v => v.AddressStart == current.AddressStart))
                {
                    return current.Next.AddressStart;
                }

                var address = jumpOrLabel ? current.AddressStart : current.AddressEnd;
                return address;
            }

            return null;
        }

        private static bool JumpOrLabelPoint(OpCodePart current, out bool startOrEnd)
        {
            if ((current.OpCode.FlowControl == FlowControl.Cond_Branch || current.OpCode.FlowControl == FlowControl.Branch) && current.IsJumpForward() && !current.Any(Code.Leave, Code.Leave_S))
            {
                startOrEnd = false;
                return true;
            }

            if (current.JumpDestination != null && current.JumpDestination.Any())
            {
                startOrEnd = true;
                return true;
            }

            startOrEnd = false;
            return false;
        }
    }
}