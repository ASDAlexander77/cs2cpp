// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeExtentions.cs" company="">
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection.Emit;
    using System.Text;
    using DebugInfo.DebugInfoSymbolWriter;
    using Gencode.SynthesizedMethods;
    using Il2Native.Logic.CodeParts;
    using Il2Native.Logic.Gencode;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class OpCodeExtensions
    {
        public static string CleanUpName(this string typeBaseName)
        {
            if (typeBaseName == null)
            {
                return null;
            }

            var s = new char[typeBaseName.Length];

            var n = ' ';
            for (var i = 0; i < typeBaseName.Length; i++)
            {
                var c = typeBaseName[i];
                switch (c)
                {
                    case ' ':
                        n = '_';
                        break;
                    case '.':
                        n = '_';
                        break;
                    case ':':
                        n = '_';
                        break;
                    case '<':
                        n = 'G';
                        break;
                    case '>':
                        n = 'C';
                        break;
                    case '-':
                        n = '_';
                        break;
                    case ',':
                        n = '_';
                        break;
                    case '*':
                        n = 'P';
                        break;
                    case '[':
                        n = 'A';
                        break;
                    case ']':
                        n = 'Y';
                        break;
                    case '&':
                        n = 'R';
                        break;
                    case '(':
                        n = 'F';
                        break;
                    case ')':
                        n = 'N';
                        break;
                    case '{':
                        n = 'C';
                        break;
                    case '}':
                        n = 'Y';
                        break;
                    case '$':
                        n = 'D';
                        break;
                    case '=':
                        n = 'E';
                        break;
                    default:
                        n = c;
                        break;
                }

                s[i] = n;
            }

            return new string(s);
        }

        public static bool IsIndirectMethodCall(this IMethod method, IType thisType)
        {
            return method.IsAbstract || method.IsVirtual || method.IsOverride;
        }

        public static int GetStaticArrayInitSize(this IType fieldType)
        {
            var pos = fieldType.Name.IndexOf("=");
            Debug.Assert(pos >= 0, "Could not find size");
            return Int32.Parse(fieldType.Name.Substring(pos + 1));
        }

        public static int Align(this int unalign, int alignSize)
        {
            var alignMinusOne = alignSize - 1;
            return (unalign + alignMinusOne) & ~alignMinusOne;
        }

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

        [Obsolete]
        public static int BranchShort(this List<object> codeList, Code op)
        {
            codeList.Add(op);
            codeList.Add((sbyte)0);
            return codeList.Count() - 1;
        }

        [Obsolete]
        public static void BranchShort(this List<object> codeList, Code op, int label)
        {
            Debug.Assert((codeList.Count() - label) < SByte.MaxValue);

            // +2, op code + address code, as backward jump start from first byte of the end of branch command with address
            var address = (byte)-(codeList.Count() - label + 2);
            codeList.Add(op);
            codeList.Add(address);
        }

        [Obsolete]
        public static int MarkLabel(this List<object> codeList)
        {
            return codeList.Count();
        }

        [Obsolete]
        public static void SetBranchShortLabel(this List<object> codeList, int jumpFrom)
        {
            codeList[jumpFrom] = codeList.Count() - 1 - jumpFrom;
        }

        [Obsolete]
        public static void AppendInt(this List<object> codeList, Code op, int valueInt)
        {
            var value = BitConverter.GetBytes(valueInt);
            codeList.Add(op);
            codeList.AddRange(value.Cast<object>());
        }

        [Obsolete]
        public static void AppendUInt(this List<object> codeList, Code op, uint valueInt)
        {
            var value = BitConverter.GetBytes(valueInt);
            codeList.Add(op);
            codeList.AddRange(value.Cast<object>());
        }

        [Obsolete]
        public static void AppendLong(this List<object> codeList, Code op, long valueLong)
        {
            var value = BitConverter.GetBytes(valueLong);
            codeList.Add(op);
            codeList.AddRange(value.Cast<object>());
        }

        [Obsolete]
        public static void AppendULong(this List<object> codeList, Code op, ulong valueLong)
        {
            var value = BitConverter.GetBytes(valueLong);
            codeList.Add(op);
            codeList.AddRange(value.Cast<object>());
        }

        [Obsolete]
        public static void AppendLoadArgument(this List<object> codeList, int argIndex)
        {
            switch (argIndex)
            {
                case 0:
                    codeList.Add(Code.Ldarg_0);
                    break;
                case 1:
                    codeList.Add(Code.Ldarg_1);
                    break;
                case 2:
                    codeList.Add(Code.Ldarg_2);
                    break;
                case 3:
                    codeList.Add(Code.Ldarg_3);
                    break;
                default:
                    codeList.AppendInt(Code.Ldarg, argIndex);
                    break;
            }
        }

        [Obsolete]
        public static void AppendLoadInt(this List<object> codeList, int value)
        {
            codeList.AppendInt(Code.Ldc_I4, value);
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="genericTypeSpecializations">
        /// </param>
        /// <param name="genericMethodSpecializations">
        /// </param>
        /// <param name="stackCall">
        /// </param>
        public static void DiscoverStructsArraysSpecializedTypesAndMethodsInMethodBody(
            this IMethod method,
            ISet<IType> genericTypeSpecializations,
            ISet<IMethod> genericMethodSpecializations,
            ISet<IType> arrayTypes,
            ISet<IType> usedTokenTypes,
            ISet<IType> usedTypes,
            ISet<MethodKey> calledMethods,
            ISet<IField> usedStaticFields,
            ISet<IType> usedVirtualTableImplementationTypes,
            Queue<IMethod> stackCall,
            ITypeResolver typeResolver)
        {
            if (Il2Converter.VerboseOutput)
            {
                Debug.WriteLine("Scanning method for types: {0}", method);
            }

            // read method body to extract all types
            var reader = new IlReader();

            reader.UsedArrayTypes = arrayTypes;
            reader.UsedGenericSpecialiazedTypes = genericTypeSpecializations;
            reader.UsedGenericSpecialiazedMethods = genericMethodSpecializations;
            reader.UsedTypeTokens = usedTokenTypes;
            reader.UsedTypes = usedTypes;
            reader.CalledMethods = calledMethods;
            reader.UsedStaticFields = usedStaticFields;
            reader.UsedVirtualTableImplementationTypes = usedVirtualTableImplementationTypes;
            reader.TypeResolver = typeResolver;

            var genericContext = MetadataGenericContext.DiscoverFrom(method, false); // true
            foreach (var op in reader.OpCodes(method, genericContext, stackCall))
            {
            }
        }

        public static int? FindBeginOfBasicBlock(this OpCodePart popCodePart)
        {
            // add alternative stack value to and address
            // 1) find jump address
            var current = popCodePart;

            // Varpop can pop 0
            var jumpOrLabel = false;
            while (current != null && !JumpOrLabelPoint(current, out jumpOrLabel))
            {
                current = current.Previous;
            }

            if (current != null && JumpOrLabelPoint(current, out jumpOrLabel))
            {
                var nextAlternateValues = current.Next != null ? current.Next.AlternativeValues : null;
                if (nextAlternateValues != null && nextAlternateValues.SelectMany(av => av.Values).Any(v => v.AddressStart == current.AddressStart))
                {
                    return current.Next.AddressStart;
                }

                var address = jumpOrLabel ? current.AddressStart : current.AddressEnd;
                return address;
            }

            return null;
        }

        public static IType FindInterfaceOwner(this IType type, IType @interface)
        {
            Debug.Assert(@interface.IsInterface, "Required interface type");

            while (!type.GetInterfaces().Any(i => i.TypeEquals(@interface) || i.GetAllInterfaces().Contains(@interface)))
            {
                type = type.BaseType;
                if (type == null)
                {
                    return null;
                }
            }

            return type;
        }

        public static IType FindInterfaceEntry(this IType type, IType @interface)
        {
            var currentType = type;

            var baseCount = 0;
            while (currentType.BaseType != null && currentType.BaseType.GetAllInterfaces().Contains(@interface))
            {
                baseCount++;
                currentType = currentType.BaseType;
            }

            while (currentType != null)
            {
                return currentType.FindInterfacePathForOneStep(@interface, out currentType);
            }

            return null;
        }


        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="interface">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        public static List<string> FindInterfacePath(this IType type, IType @interface)
        {
            var indexes = new List<string>();

            var currentType = type;

            while (currentType.BaseType != null && currentType.BaseType.GetAllInterfaces().Contains(@interface))
            {
                currentType = currentType.BaseType;
            }

            while (currentType != null)
            {
                var interfacePath = currentType.FindInterfacePathForOneStep(@interface, out currentType);
                indexes.Add(interfacePath.ToString().CleanUpName());
            }

            return indexes;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="interface">
        /// </param>
        /// <param name="nextCurrentType">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        private static int FindInterfaceIndexForOneStep(this IType type, IType @interface, out IType nextCurrentType)
        {
            nextCurrentType = type;
            var found = false;
            var interfaceIndex = -1;
            foreach (var subInterface in type.GetInterfaces().ToList())
            {
                interfaceIndex++;

                if (subInterface.TypeEquals(@interface))
                {
                    nextCurrentType = null;
                    found = true;
                    break;
                }

                if (subInterface.GetAllInterfaces().Contains(@interface))
                {
                    nextCurrentType = subInterface;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                throw new KeyNotFoundException("type can't be found");
            }

            return interfaceIndex;
        }

        private static IType FindInterfacePathForOneStep(this IType type, IType @interface, out IType nextCurrentType)
        {
            nextCurrentType = type;
            var found = false;
            IType interfacePath = null;
            foreach (var subInterface in type.GetInterfaces().ToList())
            {
                interfacePath = subInterface;

                if (subInterface.TypeEquals(@interface))
                {
                    nextCurrentType = null;
                    found = true;
                    break;
                }

                if (subInterface.GetAllInterfaces().Contains(@interface))
                {
                    nextCurrentType = subInterface;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                throw new KeyNotFoundException("type can't be found");
            }

            return interfacePath;
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
                index = Int32.Parse(asString.Substring(asString.Length - 1));
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

        public static IField GetFieldByName(this IType classType, string fieldName, ITypeResolver typeResolver, bool searchInBase = false)
        {
            var normalType = classType.ToNormal();
            if (!searchInBase)
            {
                var field = IlReader.Fields(normalType, typeResolver).FirstOrDefault(f => f.Name == fieldName);
                Debug.Assert(field != null, String.Format("Field {0} could not be found", fieldName));
                return field;
            }

            while (normalType != null)
            {
                var field = IlReader.Fields(normalType, typeResolver).FirstOrDefault(f => f.Name == fieldName);
                if (field != null)
                {
                    return field;
                }

                normalType = normalType.BaseType;
            }

            Debug.Assert(false, String.Format("Field {0} could not be found", fieldName));

            return null;
        }

        public static IMethod GetFirstMethodByName(this IType classType, string methodName, ITypeResolver typeResolver)
        {
            var normalType = classType.ToNormal();
            var method = IlReader.Methods(normalType, typeResolver).FirstOrDefault(f => f.Name == methodName);
            Debug.Assert(method != null, String.Format("Method {0} could not be found", methodName));
            return method;
        }

        public static IEnumerable<IMethod> GetMethodsByName(this IType classType, string methodName, ITypeResolver typeResolver)
        {
            var normalType = classType.ToNormal();
            return IlReader.Methods(normalType, typeResolver).Where(f => f.Name == methodName);
        }

        public static IEnumerable<IMethod> GetMethodsByMetadataName(this IType classType, string methodName, ITypeResolver typeResolver)
        {
            var normalType = classType.ToNormal();
            return IlReader.Methods(normalType, typeResolver, true).Where(f => f.MetadataName == methodName);
        }

        public static IField GetFieldByFieldNumber(this IType classType, int number, ITypeResolver typeResolver)
        {
            var normalType = classType.ToNormal();
            var field = IlReader.Fields(normalType, typeResolver).Where(t => !t.IsStatic).Skip(number).FirstOrDefault();
            if (field == null)
            {
                return null;
            }

            return field;
        }

        /// <summary>
        /// </summary>
        /// <param name="methodBase">
        /// </param>
        /// <param name="ownerOfExplicitInterface">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetFullMethodName(this IMethod methodBase, IType ownerOfExplicitInterface = null)
        {
            if (methodBase.IsUnmanaged || methodBase.IsUnmanagedDllImport)
            {
                return methodBase.Name;
            }

            return methodBase.ToString(ownerOfExplicitInterface).CleanUpName();
        }

        public static string GetMethodName(this IMethod methodBase, IType ownerOfExplicitInterface = null)
        {
            if (methodBase.IsUnmanaged || methodBase.IsUnmanagedDllImport)
            {
                return methodBase.Name;
            }

            return methodBase.ToString(ownerOfExplicitInterface, true).CleanUpName();
        }

        /// <summary>
        /// </summary>
        /// <param name="field">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetFullName(this IField field, bool cleanUp = false)
        {
            var sb = new StringBuilder();

            sb.Append(field.DeclaringType);
            sb.Append('.');
            sb.Append(field.Name);

            return sb.ToString();
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
                index = Int32.Parse(asString.Substring(asString.Length - 1));
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
        public static bool HasAnyVirtualMethod(this IType thisType, ITypeResolver typeResolver)
        {
            if (thisType.HasAnyVirtualMethodInCurrentType(typeResolver))
            {
                return true;
            }

            return thisType.BaseType != null && thisType.BaseType.HasAnyVirtualMethod(typeResolver);
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool HasAnyVirtualMethodInCurrentType(this IType thisType, ITypeResolver typeResolver)
        {
            if ((thisType.IsObject || thisType.IsInterface || thisType.BaseType != null)
                && IlReader.Methods(thisType, typeResolver).Any(m => m.IsMethodVirtual()))
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
        public static int IntTypeBitSize(this IType thisType)
        {
            // Do not include "System.IntPtr", "System.UIntPtr", they are structures not native types
            switch (thisType.FullName)
            {
                case "System.Boolean":
                    return 1;
                case "System.SByte":
                case "System.Byte":
                    return 8;
                case "System.Char":
                case "System.Int16":
                case "System.UInt16":
                    return 16;
                case "System.Int32":
                case "System.UInt32":
                    return 32;
                case "System.Int64":
                case "System.UInt64":
                    return 64;
            }

            return 0;
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

        public static bool IsReference(this IType type)
        {
            if (type == null)
            {
                return false;
            }

            if (type.IsPointer || type.IsByRef || type.UseAsClass)
            {
                return true;
            }

            if (type.IsValueType)
            {
                return false;
            }

            return true;
        }

        public static bool IsFirstInterfaceOf(this IType baseInterface, IType derivedInterface)
        {
            var firstInterface = derivedInterface.GetInterfaces().FirstOrDefault();
            if (firstInterface == null)
            {
                return false;
            }

            return firstInterface.TypeEquals(baseInterface) || baseInterface.IsFirstInterfaceOf(firstInterface);
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
            return method.IsUnmanaged;
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
            var thisTypeSize = thisType.IntTypeBitSize();
            var typeSize = type.IntTypeBitSize();

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
            var thisTypeSize = thisType.IntTypeBitSize();
            var typeSize = type.IntTypeBitSize();

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
            if (publicMethod.ExplicitInterface != null && interfaceMember.Name == publicMethod.Name &&
                interfaceMember.DeclaringType.TypeEquals(publicMethod.ExplicitInterface))
            {
                return interfaceMember.IsMatchingParamsAndReturnType(publicMethod);
            }

            if (interfaceMember.FullName == publicMethod.Name)
            {
                return interfaceMember.IsMatchingParamsAndReturnType(publicMethod);
            }

            if (interfaceMember.ExplicitName == publicMethod.Name)
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
        public static bool IsMatchingGeneric(this IMethod method, IMethod genericMethod, bool exactMatch = false)
        {
            if (method.MetadataName == genericMethod.MetadataName && method.DeclaringType.TypeEquals(genericMethod.DeclaringType))
            {
                return method.IsMatchingGenericParamsAndReturnType(genericMethod, exactMatch);
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
            if (interfaceMember.FullName == publicMethod.Name)
            {
                return interfaceMember.IsMatchingParamsAndReturnType(publicMethod);
            }

            if (interfaceMember.ExplicitName == publicMethod.Name)
            {
                return interfaceMember.IsMatchingParamsAndReturnType(publicMethod);
            }

            if (interfaceMember.Name == publicMethod.Name)
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

        public static bool IsMatchingOverrideOrExplicitInterface(this IMethod method, IMethod overridingMethod)
        {
            if (method.MetadataName == overridingMethod.MetadataName)
            {
                return method.IsMatchingParamsAndReturnType(overridingMethod);
            }

            if (method.IsExplicitInterfaceImplementation && method.MetadataName.EndsWith(String.Concat(".", overridingMethod.MetadataName)))
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
        public static bool IsRootOfVirtualTable(this IType type, ITypeResolver typeResolver)
        {
            return !type.IsInterface && type.HasAnyVirtualMethodInCurrentType(typeResolver)
                   && (type.BaseType == null || !type.BaseType.HasAnyVirtualMethod(typeResolver));
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
                case "System.IntPtr":
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

        public static bool IsSkipped(this IMethod method)
        {
            return false;
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

        public static bool IsUnsignedType(this IType thisType)
        {
            switch (thisType.FullName)
            {
                case "System.Byte":
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                case "System.UIntPtr":
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

        public static bool IsIntPtr(this IType thisType)
        {
            switch (thisType.FullName)
            {
                case "System.IntPtr":
                    return true;
            }

            return false;
        }

        public static bool IsUIntPtr(this IType thisType)
        {
            switch (thisType.FullName)
            {
                case "System.UIntPtr":
                    return true;
            }

            return false;
        }

        public static bool IsIntPtrOrUIntPtr(this IType thisType)
        {
            switch (thisType.FullName)
            {
                case "System.IntPtr":
                case "System.UIntPtr":
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
        public static bool IsVoid(this IType type)
        {
            return type != null && !type.IsPointer && !type.IsByRef && !type.UseAsClass && type.Name == "Void" && type.Namespace == "System";
        }

        public static bool IsVoidPointer(this IType type)
        {
            return type.IsPointer && type.GetElementType().IsVoid();
        }

        public static bool NotSpecialUsage(this IType type)
        {
            return type != null && !type.UseAsVirtualTableImplementation;
        }

        public static bool SpecialUsage(this IType type)
        {
            return type != null && (type.UseAsVirtualTableImplementation || type.UseAsVirtualTable);
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

        public static bool IsVirtual(this OpCodePart opCodePart)
        {
            return opCodePart.AddressEnd == 0;
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
        [Obsolete("New interface layout should be")]
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
        [Obsolete("New interface layout should be")]
        public static IEnumerable<IType> SelectAllTopAndAllNotFirstChildrenInterfaces(this IType type, Stack<IType> nesting)
        {
            if (type.BaseType != null)
            {
                foreach (var baseInterface in type.BaseType.SelectAllTopAndAllNotFirstChildrenInterfaces(nesting))
                {
                    yield return baseInterface;
                }
            }

            foreach (var topInterface in type.GetInterfacesExcludingBaseAllInterfaces())
            {
                yield return topInterface;

                if (nesting != null)
                {
                    nesting.Push(topInterface);
                }

                // enumerate all children except first
                foreach (var notFirstChild in topInterface.SelectAllNestedChildrenExceptFirstInterfaces())
                {
                    yield return notFirstChild;
                }

                if (nesting != null)
                {
                    nesting.Pop();
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

        public static IType NormalizeType(this IType typeSource)
        {
            var type = typeSource;
            while (type.IsPointer || type.UseAsClass || type.IsByRef)
            {
                type = type.UseAsClass ? type.ToNormal() : type.GetElementType();
            }

            return type;
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
            if (type == null && other == null)
            {
                return true;
            }

            if (type != null && other == null || type == null && other != null)
            {
                return false;
            }

            return type != null && other.CompareTo(type) == 0;
        }

        public static bool TypeEqualsOrDerived(this IType type, IType other)
        {
            if (type == null && other == null)
            {
                return true;
            }

            if (type != null && other == null || type == null && other != null)
            {
                return false;
            }

            return type != null && (other.CompareTo(type) == 0 || type.IsDerivedFrom(other));
        }

        public static bool TypeEqualsOrBaseOf(this IType type, IType other)
        {
            if (type == null && other == null)
            {
                return true;
            }

            if (type != null && other == null || type == null && other != null)
            {
                return false;
            }

            return type != null && (other.CompareTo(type) == 0 || other.IsDerivedFrom(type));
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
        private static bool CompareTypeWithGenericType(IType type, IType genType, bool testVoid = false, bool exactMatch = false)
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

            if (!exactMatch && (genType.IsGenericTypeDefinition || type.IsGenericTypeDefinition) && genType.MetadataFullName.Equals(type.MetadataFullName))
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
        private static bool IsMatchingGenericParamsAndReturnType(this IMethod method, IMethod genericMethod, bool exactMatch = false)
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
                    || !CompareTypeWithGenericType(params1[i].ParameterType, genParams2[i].ParameterType, exactMatch: exactMatch))
                {
                    return false;
                }
            }

            if (CompareTypeWithGenericType(method.ReturnType, genericMethod.ReturnType, true, exactMatch))
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
            var parameters = method.GetParameters();
            var otherParameters = overridingMethod.GetParameters();

            if (!CompareTypeParam(method.ReturnType, overridingMethod.ReturnType))
            {
                return false;
            }

            if ((parameters != null ? parameters.Count() : 0) == 0 &&
                (otherParameters != null ? otherParameters.Count() : 0) == 0)
            {
                return true;
            }

            if (parameters == null || otherParameters == null)
            {
                return false;
            }

            var params1 = parameters.ToArray();
            var params2 = otherParameters.ToArray();

            return IsMatchingParams(params1, params2);
        }

        public static bool IsMethodVirtual(this IMethod method)
        {
            return method.IsAbstract || method.IsVirtual || method.IsOverride;
        }

        public static bool IsMatchingParams(this IParameter[] params1, IParameter[] params2)
        {
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

            return true;
        }

        private static bool JumpOrLabelPoint(OpCodePart current, out bool startOrEnd)
        {
            if ((current.OpCode.FlowControl == FlowControl.Cond_Branch || current.OpCode.FlowControl == FlowControl.Branch)
                && !current.Any(Code.Leave, Code.Leave_S))
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

        public static IConstructor FindConstructor(this IType type, ITypeResolver typeResolver)
        {
            return IlReader.Constructors(type, typeResolver).FirstOrDefault(c => !c.GetParameters().Any());
        }

        public static IConstructor FindStaticConstructor(this IType type, ITypeResolver typeResolver)
        {
            return IlReader.Constructors(type, typeResolver).FirstOrDefault(c => c.IsStatic);
        }

        public static IConstructor FindConstructor(this IType type, IType firstParameterType, ITypeResolver typeResolver)
        {
            return IlReader.Constructors(type, typeResolver)
                     .FirstOrDefault(c => c.GetParameters().Count() == 1 && c.GetParameters().First().ParameterType.TypeEquals(firstParameterType));
        }

        public static IMethod FindFinalizer(this IType type, ITypeResolver typeResolver)
        {
            return type.GetMethods(IlReader.DefaultFlags).FirstOrDefault(m => m.IsDestructor);
        }

        public static IType GetThisTypeForMethod(this IMethod method)
        {
            var thisType = method.DeclaringType;
            if (thisType == null)
            {
                return null;
            }

            var methodExtraAttributes = method as IMethodExtraAttributes;
            if (!thisType.IsEnum && thisType.IsValueType() && (!method.Name.StartsWith(".") || method.Name == ".ctor")
                && !(methodExtraAttributes != null && methodExtraAttributes.IsStructObjectAdapter))
            {
                return thisType.ToPointerType();
            }

            return thisType.ToClass();
        }

        public static bool ShouldHaveStructToObjectAdapter(this IMethod m)
        {
            if (m.IsStatic)
            {
                return false;
            }

            return m.IsMethodVirtual() || m.IsExplicitInterfaceImplementation || m.IsPublic;
        }

        public static bool IsAssemblyNamespaceRequired(this IType type)
        {
            if (type.IsGenericOrArray() || type.IsPointer || type.IsModule)
            {
                return true;
            }

            if (type.IsAnyParentOrSelfInternal() && !type.Name.StartsWith("Runtime"))
            {
                return true;
            }

            return false;
        }

        public static bool IsGenericOrArray(this IType type)
        {
            return type.IsGenericType || type.IsGenericTypeDefinition || type.IsArray;
        }

        public static string GetAssemblyNamespace(this IType type, string currentAssemblyNamespace)
        {
            if (type.IsGenericOrArray() || type.IsPointer || type.IsModule)
            {
                return currentAssemblyNamespace;
            }

            if (type.IsAnyParentOrSelfInternal() && !type.Name.StartsWith("Runtime"))
            {
                return type.AssemblyQualifiedName;
            }

            return null;
        }

        public static bool IsAssemblyNamespaceRequired(this IMethod method, IType ownerOfExplicitInterface = null)
        {
            if (method.IsUnmanaged || method.IsUnmanagedMethodReference || method.IsUnmanagedDllImport)
            {
                return false;
            }

            if (method.DeclaringType != null && method.DeclaringType.IsAssemblyNamespaceRequired())
            {
                return true;
            }

            if (method.IsGenericMethod || method.IsGenericMethodDefinition)
            {
                return true;
            }

            if (ownerOfExplicitInterface != null && ownerOfExplicitInterface.IsGenericOrArray())
            {
                return true;
            }

            return false;
        }

        public static string GetAssemblyNamespace(this IMethod method, string currentAssemblyNamespace, IType ownerOfExplicitInterface = null)
        {
            if (method.IsGenericMethod || method.IsGenericMethodDefinition)
            {
                return currentAssemblyNamespace;
            }

            if (ownerOfExplicitInterface != null && (ownerOfExplicitInterface.IsGenericType || ownerOfExplicitInterface.IsGenericTypeDefinition || ownerOfExplicitInterface.IsArray))
            {
                return currentAssemblyNamespace;
            }

            if (method.DeclaringType != null && method.DeclaringType.IsAssemblyNamespaceRequired())
            {
                return GetAssemblyNamespace(method.DeclaringType, currentAssemblyNamespace);
            }

            return null;
        }

        public static bool IsAnyParentOrSelfInternal(this IType type)
        {
            if (type == null)
            {
                return false;
            }

            var effectiveType = type;
            do
            {
                if (effectiveType.IsInternal)
                {
                    return true;
                }

                if (!effectiveType.IsNested)
                {
                    break;
                }

                effectiveType = effectiveType.DeclaringType;
            } while (effectiveType != null);
             
            return false;
        }
    }
}