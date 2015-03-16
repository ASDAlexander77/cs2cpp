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
    using Exceptions;
    using PEAssemblyReader;

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
                else if (used != null && used.Length > 0)
                {
                    // this expression
                    opCodeMethodInfo.WriteFunctionCallThisExpression(
                        thisType,
                        used[0],
                        resultOfFirstOperand,
                        cWriter);

                    cWriter.WriteResultOrActualWrite(writer, used[0]);

                    if (thisType.IsInterface)
                    {
                        cWriter.WriteGetThisPointerFromInterfacePointer(used[0]);
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

            // if this is external method reference we need to load reference first
            // %4 = load i32 ()** @__glewCreateProgram, align 4
            // load pointer
            cWriter.SetResultNumber(
                opCodeMethodInfo,
                cWriter.System.System_Byte.ToPointerType().ToPointerType());
            writer.Write("load ");
            cWriter.WriteMethodPointerType(writer, methodInfo, thisType);
            writer.Write("* ");
            cWriter.WriteMethodDefinitionName(writer, methodInfo);
            writer.Write(", align {0}", CWriter.PointerSize);
            writer.WriteLine(string.Empty);
            methodAddressResultNumber = opCodeMethodInfo.Result;
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
                cWriter.WriteCCast(opCodeFirstOperand, thisType);
            }

            if (dynamicCastRequired)
            {
                cWriter.WriteDynamicCast(writer, opCodeFirstOperand, opCodeFirstOperand, thisType);
                writer.WriteLine(string.Empty);
            }

            if (isPrimitive || isPrimitivePointer)
            {
                var primitiveType = resultOfFirstOperand.Type;
                if (isPrimitivePointer)
                {
                    primitiveType = resultOfFirstOperand.Type.GetElementType();
                    cWriter.WriteResultOrActualWrite(writer, opCodeFirstOperand);
                    var firstOperandResult = opCodeFirstOperand.Result;
                    cWriter.WriteLoad(opCodeFirstOperand, primitiveType, firstOperandResult);
                }
                else
                {
                    var intType = cWriter.GetIntTypeByByteSize(CWriter.PointerSize);
                    var uintType = cWriter.GetUIntTypeByByteSize(CWriter.PointerSize);
                    if (intType.TypeEquals(primitiveType) || uintType.TypeEquals(primitiveType))
                    {
                        var declType = (opCodeMethodInfo as OpCodeMethodInfoPart).Operand.DeclaringType;
                        Debug.Assert(declType.IsStructureType(), "only Struct type can be used");
                        primitiveType = declType.ToClass();
                        cWriter.AdjustIntConvertableTypes(
                            writer,
                            opCodeMethodInfo.OpCodeOperands[0],
                            declType.ToPointerType());
                    }
                    else
                    {
                        Debug.Assert(false, "only Int type allowed");
                    }
                }

                // convert value to object
                opCodeMethodInfo.Result = null;
                var opCodeNone = OpCodePart.CreateNop;
                opCodeNone.OpCodeOperands = new[] { opCodeMethodInfo.OpCodeOperands[0] };
                primitiveType.ToClass().WriteCallBoxObjectMethod(cWriter, opCodeNone);
                opCodeFirstOperand.Result = opCodeNone.Result;

                if (thisType.IsClassCastRequired(cWriter, opCodeFirstOperand, out dynamicCastRequired))
                {
                    cWriter.WriteCast(opCodeFirstOperand, opCodeFirstOperand, thisType);
                }
            }
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
        /// <param name="thisType">
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
        public static void WriteFunctionCallProlog(
            this IMethod methodInfo,
            OpCodePart opCodeMethodInfo,
            bool isVirtual,
            bool hasThis,
            CWriter cWriter,
            out IType thisType,
            out bool hasThisArgument,
            out OpCodePart opCodeFirstOperand,
            out BaseWriter.ReturnResult resultOfFirstOperand,
            out bool isIndirectMethodCall,
            out IType ownerOfExplicitInterface,
            out IType requiredType)
        {
            thisType = methodInfo.DeclaringType != null ? methodInfo.DeclaringType.ToClass() : null;

            var parameters = methodInfo.GetParameters();
            hasThisArgument = hasThis && opCodeMethodInfo.OpCodeOperands != null
                && opCodeMethodInfo.OpCodeOperands.Length - (parameters != null ? parameters.Count() : 0) > 0;
            opCodeFirstOperand = opCodeMethodInfo.OpCodeOperands != null && opCodeMethodInfo.OpCodeOperands.Length > 0
                ? opCodeMethodInfo.OpCodeOperands[0]
                : null;
            resultOfFirstOperand = opCodeFirstOperand != null ? cWriter.EstimatedResultOf(opCodeFirstOperand) : null;

            isIndirectMethodCall = isVirtual
                                   &&
                                   (methodInfo.IsAbstract || methodInfo.IsVirtual ||
                                    (thisType.IsInterface && thisType.TypeEquals(resultOfFirstOperand.Type)));

            ownerOfExplicitInterface = isVirtual && thisType.IsInterface &&
                                       thisType.TypeNotEquals(resultOfFirstOperand.Type)
                ? resultOfFirstOperand.Type
                : null;

            var rollbackType = false;
            requiredType = ownerOfExplicitInterface != null ? resultOfFirstOperand.Type : null;
            if (requiredType != null)
            {
                thisType = requiredType;
                rollbackType = true;
            }

            if (isIndirectMethodCall && methodInfo.DeclaringType.TypeNotEquals(thisType) &&
                methodInfo.DeclaringType.IsInterface && !thisType.IsInterface
                && thisType.HasExplicitInterfaceMethodOverride(methodInfo))
            {
                // this is explicit call of interface
                isIndirectMethodCall = false;
            }
            else if (rollbackType)
            {
                thisType = methodInfo.DeclaringType;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        /// <returns>
        /// </returns>
        public static FullyDefinedReference WriteFunctionCallResult(
            this IMethod methodInfo,
            OpCodePart opCodeMethodInfo,
            CWriter cWriter)
        {
            var isReturnVoidType = methodInfo != null && methodInfo.ReturnType.IsVoid();
            if (!isReturnVoidType)
            {
                cWriter.SetResultNumber(opCodeMethodInfo, methodInfo.ReturnType);
            }

            return opCodeMethodInfo.Result;
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
            cWriter.AdjustToType(operand, parameter.ParameterType);
            cWriter.WriteResultOrActualWrite(cWriter.Output, operand);
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