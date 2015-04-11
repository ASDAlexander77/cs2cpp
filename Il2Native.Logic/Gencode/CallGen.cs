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
                    if (thisType.IsInterface)
                    {
                        writer.Write("(");
                        cWriter.WriteCCastOnly(thisType);
                        writer.Write("__interface_to_object(");
                    }

                    // this expression
                    opCodeMethodInfo.WriteFunctionCallThisExpression(
                        thisType,
                        isCtor,
                        used[0],
                        resultOfFirstOperand,
                        cWriter);

                    if (thisType.IsInterface)
                    {
                        writer.Write("))");
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

                    cWriter.WriteFunctionCallParameterArgument(usedItem);

                    comaRequired = true;
                    index++;
                }
            }

            writer.Write(")");
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
        public static void WriteFunctionCallThisExpression(
            this OpCodePart opCodeMethodInfo,
            IType thisType,
            bool isCtor,
            OpCodePart opCodeFirstOperand,
            BaseWriter.ReturnResult resultOfFirstOperand,
            CWriter cWriter)
        {
            var writer = cWriter.Output;

            // check if you need to cast this parameter
            var isPrimitive = resultOfFirstOperand.Type.IsPrimitiveTypeOrEnum();
            var isPrimitivePointer = resultOfFirstOperand.Type.IsPointer &&
                                     resultOfFirstOperand.Type.GetElementType().IsPrimitiveTypeOrEnum();

            var dynamicCastRequired = false;
            if (!isPrimitive && !isPrimitivePointer &&
                thisType.IsClassCastRequired(cWriter, opCodeFirstOperand, out dynamicCastRequired))
            {
                cWriter.WriteCCastOnly(thisType);
            }

            if (dynamicCastRequired)
            {
                cWriter.WriteDynamicCast(writer, opCodeFirstOperand, opCodeFirstOperand, thisType);
                return;
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

                // TODO: you need to review of this code (whole function), and check if you need to box it in case of Pointer operations on Array
                var @class = primitiveType.ToClass();
                if (@class.TypeNotEquals(thisType) && isPrimitivePointer && !thisType.IsPrimitive)
                {
                    // in case Void* used or Byte* used, you do not need to box it as it may be array operation.
                    cWriter.WriteCCastOnly(thisType);
                }
                else
                {
                    var opCodeNone = OpCodePart.CreateNop;
                    opCodeNone.OpCodeOperands = new[] { opCodeMethodInfo.OpCodeOperands[0] };
                    @class.WriteCallBoxObjectMethod(cWriter, opCodeNone);
                    return;
                }
            }

            cWriter.WriteResultOrActualWrite(writer, opCodeFirstOperand);
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

            isIndirectMethodCall = isVirtual && methodInfo.IsIndirectMethodCall(resultOfFirstOperand.Type);

            ownerOfExplicitInterface = isVirtual ? GetOwnerOfExplicitInterface(methodDeclaringType, resultOfFirstOperand.Type) : null;

            var rollbackType = false;
            requiredType = ownerOfExplicitInterface != null ? resultOfFirstOperand.Type : null;
            
            if (requiredType != null)
            {
                methodDeclaringType = requiredType;
                rollbackType = true;
            }

            if (isIndirectMethodCall && methodDeclaringType.IsInterface && methodDeclaringType.HasExplicitInterfaceMethodOverride(methodInfo))
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
            if (isIndirectMethodCall)
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
            OpCodePart operand)
        {
            cWriter.WriteResultOrActualWrite(cWriter.Output, operand);
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
    }
}