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

    using Il2Native.Logic.CodeParts;
    using Il2Native.Logic.Gencode;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class OpCodeExtentions
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
            var code = opCode.ToCode();
            return codes.Any(item => item == code);
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="genericTypeSpecializations">
        /// </param>
        /// <param name="genericMethodSpecializations">
        /// </param>
        public static void DiscoverRequiredTypesAndMethods(
            this IMethod method, HashSet<IType> genericTypeSpecializations, HashSet<IMethod> genericMethodSpecializations, HashSet<IType> requiredTypes)
        {
            // read method body to extract all types
            var reader = new IlReader();

            var genericContext = MetadataGenericContext.DiscoverFrom(method);
            foreach (var op in reader.OpCodes(method, genericContext))
            {
                // dummy body we just need to read body of a method
            }

            if (genericTypeSpecializations != null)
            {
                foreach (var genericSpecializedType in reader.UsedGenericSpecialiazedTypes)
                {
                    genericTypeSpecializations.Add(genericSpecializedType);
                }
            }

            if (genericMethodSpecializations != null)
            {
                foreach (var genericSpecializedMethod in reader.UsedGenericSpecialiazedMethods)
                {
                    genericMethodSpecializations.Add(genericSpecializedMethod);
                }
            }

            if (requiredTypes != null)
            {
                foreach (var usedType in reader.UsedStructTypes)
                {
                    requiredTypes.Add(usedType);
                }
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
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        public static IType GetFieldTypeByIndex(this IType classType, int index)
        {
            var normalType = classType.ToNormal();

            if (normalType.IsEnum)
            {
                return normalType.GetEnumUnderlyingType();
            }

            var field = IlReader.Fields(normalType).Where(t => !t.IsStatic).Skip(index - 1).FirstOrDefault();
            if (field == null)
            {
                return null;
            }

            return field.FieldType;
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
        /// <returns>
        /// </returns>
        public static int IntTypeBitSize(this IType thisType, bool isPointer = false)
        {
            var thisTypeString = thisType.TypeToCType(isPointer);
            return GetIntSize(thisTypeString);
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
            return method.IsInternalCall && !method.Name.StartsWith("llvm_");
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
        public static bool IsIntValueTypeCastRequired(this IType thisType, IType type)
        {
            var thisTypeString = thisType.TypeToCType();
            var typeString = type.TypeToCType();

            var thisTypeSize = GetIntSize(thisTypeString);
            var typeSize = GetIntSize(typeString);

            if (thisTypeSize == 0 || typeSize == 0)
            {
                return false;
            }

            return thisTypeSize > typeSize;
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
            if (method.Name == genericMethod.Name)
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
            if (method.Name == overridingMethod.Name)
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
                   && (type.IsValueType && !type.IsEnum && !type.IsPrimitive && !type.IsVoid() && !type.IsPointer
                       || recurse && type.HasElementType && type.GetElementType().IsStructureType(recurse));
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

            if (opCode.CustomJumpAddress.HasValue)
            {
                return opCode.CustomJumpAddress.Value;
            }

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
        public static OpCodePart JumpOpCodeGroup(this OpCodePart opCode, BaseWriter baseWriter)
        {
            var jumpAddress = opCode.JumpAddress();
            OpCodePart stopForBranch;
            if (baseWriter.OpsByGroupAddressStart.TryGetValue(jumpAddress, out stopForBranch))
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
        public static OpCodePart NextOpCode(this OpCodePart opCode, BaseWriter baseWriter)
        {
            OpCodePart ret = null;
            baseWriter.OpsByAddressStart.TryGetValue(opCode.AddressEnd, out ret);
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
        public static OpCodePart PreviousOpCode(this OpCodePart opCode, BaseWriter baseWriter)
        {
            OpCodePart ret = null;
            baseWriter.OpsByAddressEnd.TryGetValue(opCode.AddressStart, out ret);
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
            foreach (var childInterface in type.GetInterfacesExcludingBaseAllInterfaces().Skip(1))
            {
                yield return childInterface;
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
        /// <param name="thisTypeString">
        /// </param>
        /// <returns>
        /// </returns>
        private static int GetIntSize(string thisTypeString)
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
                if (params1[i].IsOut != genParams2[i].IsOut
                    || params1[i].IsRef != genParams2[i].IsRef
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

            if (genType.MetadataFullName.Equals(type.MetadataFullName))
            {
                return true;
            }

            return genType.IsGenericParameter;
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
                if (!params1[index].ParameterType.Equals(params2[index].ParameterType))
                {
                    return false;
                }
            }

            if (!method.ReturnType.Equals(overridingMethod.ReturnType))
            {
                return false;
            }

            return true;
        }

        public static bool IsGenericDefinition(this IType type)
        {
            if (type.IsGenericTypeDefinition)
            {
                return true;
            }

            var current = type;
            while (current != null && current.IsNested)
            {
                current = current.DeclaringType;
            }

            return current != null && current.IsGenericTypeDefinition;
        }
    }
}