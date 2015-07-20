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
            bool hasThis,
            bool isCtor,
            bool isIndirectMethodCall,
            FullyDefinedReference resultNumberForThis,
            IType thisType,
            IType returnType,
            IType ownerOfExplicitInterface,
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
                    var castToInterfaceIsApplied = false;
                    if (isIndirectMethodCall && thisType.IsInterface)
                    {
                        var opCodeCastclass = used[0] as OpCodeTypePart;
                        castToInterfaceIsApplied = opCodeCastclass != null && opCodeCastclass.Any(Code.Castclass) && opCodeCastclass.Operand.TypeEquals(thisType);

                        writer.Write("");
                        cWriter.WriteCCastOnly(thisType);
                        writer.Write("(");
                    }

                    if (!isIndirectMethodCall && ownerOfExplicitInterface != null)
                    {
                        cWriter.WriteCCastOnly(ownerOfExplicitInterface);
                    }

                    // this expression
                    cWriter.WriteResultOrActualWrite(writer, !castToInterfaceIsApplied ? used[0] : used[0].OpCodeOperands[0]);

                    if (isIndirectMethodCall && thisType.IsInterface)
                    {
                        writer.Write("->__this)");
                    }
                }

                comaRequired = true;
            }

            var parameters = parameterInfos;
            var argsContainsThisArg = used != null ? (used.Length - (parameters != null ? parameters.Count() : 0)) > 0 : false;
            var argShift = hasThis && !isCtor && argsContainsThisArg ? 1 : 0;

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

                    cWriter.WriteFunctionCallParameterArgument(usedItem);

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
            out IType requiredType,
            out IMethod publicMethodInfo)
        {
            publicMethodInfo = null;
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

            bool isExplicit;
            if (isIndirectMethodCall && !methodDeclaringType.IsInterface && methodInfo.DeclaringType.IsInterface &&
                (methodDeclaringType.FindInterfaceOwner(methodInfo.DeclaringType).HasExplicitInterfaceMethodOrPublic(methodInfo, out isExplicit, out publicMethodInfo)))
            {
                // this is explicit call of interface
                isIndirectMethodCall = false;
                if (isExplicit)
                {
                    ownerOfExplicitInterface = methodDeclaringType.FindInterfaceOwner(methodInfo.DeclaringType);
                }
                else
                {
                    ownerOfExplicitInterface = null;
                }
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
            else if (methodInfo.IsUnmanaged)
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
    }
}