// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CallGen.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using CodeParts;
    using PEAssemblyReader;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public static class CallGen
    {
        /// <summary>
        /// </summary>
        /// <param name="parameterInfos">
        /// </param>
        /// <param name="used">
        /// </param>
        /// <param name="isVirtual">
        /// </param>
        /// <param name="hasThis">
        /// </param>
        /// <param name="isCtor">
        /// </param>
        /// <param name="resultNumberForThis">
        /// </param>
        /// <param name="thisType">
        /// </param>
        /// <param name="returnType">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void WriteFunctionCallArguments(
            this IEnumerable<IParameter> parameterInfos,
            OpCodePart opCodeMethodInfo,
            BaseWriter.ReturnResult resultOfFirstOperand,
            bool @isVirtual,
            bool hasThis,
            bool isCtor,
            FullyDefinedReference resultNumberForThis,
            IType thisType,
            IType returnType,
            CWriter cWriter,
            bool varArg)
        {
            var writer = cWriter.Output;

            OpCodePart[] used = opCodeMethodInfo.OpCodeOperands;

            writer.Write("(");

            var index = 0;

            var comaRequired = false;

            if (hasThis)
            {
                if (resultNumberForThis != null)
                {
                    cWriter.WriteResult(resultNumberForThis);
                }
                else if (!isCtor && used != null && used.Length > 0)
                {
                    // this expression
                    var closeRequired = opCodeMethodInfo.WriteFunctionCallThisExpression(
                        thisType,
                        isCtor,
                        used[0],
                        resultOfFirstOperand,
                        cWriter);

                    if (thisType.IsInterface)
                    {
                        cWriter.WriteGetThisPointerFromInterfacePointer(used[0]);
                        if (closeRequired)
                        {
                            writer.Write(")");
                        }
                    }
                }

                comaRequired = true;
            }

            var parameters = parameterInfos;
            var argsContainsThisArg = used != null ? (used.Length - (parameters != null ? parameters.Count() : 0)) > 0 : false;
            var argShift = @isVirtual || (hasThis && !isCtor && argsContainsThisArg) ? 1 : 0;

            // add parameters
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    var effectiveIndex = index + argShift;
                    var usedItem = used[effectiveIndex];

                    if (comaRequired)
                    {
                        writer.Write(", ");
                    }

                    cWriter.WriteFunctionCallParameterArgument(usedItem, parameter);

                    comaRequired = true;
                    index++;
                }
            }

            if (varArg)
            {
                // VarArgs
                while (index < used.Length)
                {
                    var effectiveIndex = index + argShift;
                    var usedItem = used[effectiveIndex];

                    if (comaRequired)
                    {
                        writer.Write(", ");
                    }

                    cWriter.WriteFunctionCallVarArgument(usedItem, usedItem.Result.Type);

                    comaRequired = true;
                    index++;
                }
            }

            writer.Write(")");
        }

        /// <summary>
        /// </summary>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="thisType">
        /// </param>
        /// <param name="methodAddressResultNumber">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void WriteFunctionCallLoadFunctionAddress(
            this IMethod methodInfo,
            OpCodePart opCodeMethodInfo,
            IType thisType,
            ref FullyDefinedReference methodAddressResultNumber,
            CWriter cWriter)
        {
            if (!methodInfo.IsUnmanagedMethodReference)
            {
                return;
            }

            var writer = cWriter.Output;

            // TODO: finish it
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="thisType">
        /// </param>
        /// <param name="opCodeFirstOperand">
        /// </param>
        /// <param name="resultOfFirstOperand">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static bool WriteFunctionCallThisExpression(
            this OpCodePart opCodeMethodInfo,
            IType thisType,
            bool isCtor,
            OpCodePart opCodeFirstOperand,
            BaseWriter.ReturnResult resultOfFirstOperand,
            CWriter cWriter)
        {
            var writer = cWriter.Output;

            var result = false;

            // check if you need to cast this parameter
            var isPrimitive = resultOfFirstOperand.Type.IsPrimitiveTypeOrEnum();
            var isPrimitivePointer = resultOfFirstOperand.Type.IsPointer &&
                                     resultOfFirstOperand.Type.GetElementType().IsPrimitiveTypeOrEnum();

            var dynamicCastRequired = false;
            if (!isPrimitive && !isPrimitivePointer &&
                thisType.IsClassCastRequired(cWriter, opCodeFirstOperand, out dynamicCastRequired))
            {
                cWriter.WriteCCastOnly(thisType);

                if (thisType.IsInterface)
                {
                    writer.Write("(");
                }

                result = true;
            }

            if (dynamicCastRequired)
            {
                cWriter.WriteDynamicCast(writer, opCodeFirstOperand, opCodeFirstOperand, thisType);
                return result;
            }

            var opCodeMethodInfoPart = opCodeMethodInfo as OpCodeMethodInfoPart;
            if (isPrimitive || isPrimitivePointer)
            {
                var primitiveType = resultOfFirstOperand.Type;
                if (!isPrimitivePointer)
                {
                    var intType = cWriter.GetIntTypeByByteSize(CWriter.PointerSize);
                    var uintType = cWriter.GetUIntTypeByByteSize(CWriter.PointerSize);
                    if (intType.TypeEquals(primitiveType) || uintType.TypeEquals(primitiveType))
                    {
                        var declType = opCodeMethodInfoPart.Operand.DeclaringType;
                        Debug.Assert(declType != null && declType.IsStructureType(), "only Struct type can be used");
                        primitiveType = declType.ToClass();
                        cWriter.AdjustIntConvertableTypes(writer, opCodeMethodInfo.OpCodeOperands[0], declType.ToPointerType());
                    }
                    else
                    {
                        Debug.Assert(false, "only Int type allowed");
                    }
                }
                else
                {
                    primitiveType = resultOfFirstOperand.Type.GetElementType();
                }

                // convert value to object
                var opCodeNone = OpCodePart.CreateNop;
                opCodeNone.OpCodeOperands = new[] { opCodeMethodInfo.OpCodeOperands[0] };
                primitiveType.ToClass().WriteCallBoxObjectMethod(cWriter, opCodeNone);

                return result;
            }

            cWriter.WriteResultOrActualWrite(writer, opCodeFirstOperand);

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="isVirtual">
        /// </param>
        /// <param name="hasThis">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        /// <param name="methodDeclaringType">
        /// </param>
        /// <param name="hasThisArgument">
        /// </param>
        /// <param name="opCodeFirstOperand">
        /// </param>
        /// <param name="resultOfFirstOperand">
        /// </param>
        /// <param name="isIndirectMethodCall">
        /// </param>
        /// <param name="ownerOfExplicitInterface">
        /// </param>
        /// <param name="requiredType">
        /// </param>
        public static void FunctionCallProlog(
            this IMethod methodInfo,
            OpCodePart opCodeMethodInfo,
            bool isVirtual,
            bool hasThis,
            CWriter cWriter,
            out IType methodDeclaringType,
            out bool hasThisArgument,
            out OpCodePart opCodeFirstOperand,
            out BaseWriter.ReturnResult resultOfFirstOperand,
            out bool isIndirectMethodCall,
            out IType ownerOfExplicitInterface,
            out IType requiredType)
        {
            methodDeclaringType = methodInfo.DeclaringType != null ? methodInfo.DeclaringType.ToClass() : null;

            var parameters = methodInfo.GetParameters();
            hasThisArgument = hasThis && opCodeMethodInfo.OpCodeOperands != null
                && opCodeMethodInfo.OpCodeOperands.Length - (parameters != null ? parameters.Count() : 0) > 0;
            opCodeFirstOperand = opCodeMethodInfo.OpCodeOperands != null && opCodeMethodInfo.OpCodeOperands.Length > 0
                ? opCodeMethodInfo.OpCodeOperands[0]
                : null;
            resultOfFirstOperand = opCodeFirstOperand != null ? cWriter.EstimatedResultOf(opCodeFirstOperand) : null;

            isIndirectMethodCall = isVirtual
                                   && (methodInfo.IsAbstract || methodInfo.IsVirtual || (methodDeclaringType.IsInterface && methodDeclaringType.TypeEquals(resultOfFirstOperand.Type)));

            ownerOfExplicitInterface = isVirtual ? GetOwnerOfExplicitInterface(methodDeclaringType, resultOfFirstOperand.Type) : null;

            var rollbackType = false;
            requiredType = ownerOfExplicitInterface != null ? resultOfFirstOperand.Type : null;
            if (requiredType != null)
            {
                methodDeclaringType = requiredType;
                rollbackType = true;
            }

            if (isIndirectMethodCall && methodInfo.DeclaringType.TypeNotEquals(methodDeclaringType) &&
                methodInfo.DeclaringType.IsInterface && !methodDeclaringType.IsInterface
                && methodDeclaringType.HasExplicitInterfaceMethodOverride(methodInfo))
            {
                // this is explicit call of interface
                isIndirectMethodCall = false;
            }
            else if (rollbackType)
            {
                methodDeclaringType = methodInfo.DeclaringType;
            }
        }

        public static IType GetOwnerOfExplicitInterface(IType methodDeclaringType, IType argument0)
        {
            return methodDeclaringType.IsInterface && methodDeclaringType.TypeNotEquals(argument0) ? argument0 : null;
        }

        /// <summary>
        /// </summary>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void WriteFunctionCallReturnType(this IMethod methodInfo, CWriter cWriter)
        {
            var writer = cWriter.Output;

            if (methodInfo != null && !methodInfo.ReturnType.IsVoid() && !methodInfo.ReturnType.IsStructureType())
            {
                methodInfo.ReturnType.WriteTypePrefix(cWriter, false);
            }
            else
            {
                // this is constructor
                writer.Write("void");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="methodAddressResultNumber">
        /// </param>
        /// <param name="ownerOfExplicitInterface">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void WriteFunctionNameExpression(
            this IMethod methodInfo,
            FullyDefinedReference methodAddressResultNumber,
            IType ownerOfExplicitInterface,
            CWriter cWriter)
        {
            var writer = cWriter.Output;

            var isIndirectMethodCall = methodAddressResultNumber != null;
            if (isIndirectMethodCall || methodInfo.IsUnmanagedMethodReference)
            {
                cWriter.WriteResult(methodAddressResultNumber);
            }
            else if (methodInfo.DeclaringType == null)
            {
                // just name (for example calloc)
                cWriter.Output.Write(methodInfo.Name);
            }
            else
            {
                // default method name
                cWriter.WriteMethodDefinitionName(writer, methodInfo, ownerOfExplicitInterface);
            }
        }

        private static void WriteFunctionCallParameterArgument(
            this CWriter cWriter,
            OpCodePart operand,
            IParameter parameter)
        {
            if (!cWriter.AdjustToType(operand, parameter.ParameterType))
            {
                cWriter.WriteResultOrActualWrite(cWriter.Output, operand);
            }
        }

        private static void WriteFunctionCallVarArgument(this CWriter cWriter, OpCodePart opArg, IType type)
        {
            var writer = cWriter.Output;

            type.WriteTypePrefix(cWriter, type.IsStructureType());
            if (type.IsStructureType())
            {
                writer.Write(" byval align " + cWriter.ByValAlign);
            }

            writer.Write(' ');
            cWriter.WriteResult(opArg);
        }
    }
}