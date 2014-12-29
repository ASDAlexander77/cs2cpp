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
    using System.Linq;
    using System.Runtime.InteropServices;

    using Il2Native.Logic.CodeParts;
    using Il2Native.Logic.Exceptions;

    using PEAssemblyReader;
    using System.Diagnostics;

    /// <summary>
    /// </summary>
    public static class CallGen
    {
        /// <summary>
        /// </summary>
        /// <param name="methodBase">
        /// </param>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void PreProcessCallParameters(this IMethod methodBase, OpCodePart opCodeMethodInfo, LlvmWriter llvmWriter)
        {
            // check if you need to cast parameter
            if (opCodeMethodInfo.OpCodeOperands == null)
            {
                return;
            }

            var writer = llvmWriter.Output;

            var parameters = methodBase.GetParameters();
            var index = opCodeMethodInfo.OpCodeOperands.Count() - parameters.Count();
            foreach (var parameter in parameters)
            {
                var operand = opCodeMethodInfo.OpCodeOperands[index];

                var dynamicCastRequired = false;
                if (parameter.ParameterType.IsClassCastRequired(operand, out dynamicCastRequired))
                {
                    writer.WriteLine("; Cast of '{0}' parameter", parameter.Name);
                    llvmWriter.WriteCast(operand, operand.Result, parameter.ParameterType);
                }

                operand.RequiredIncomingType = parameter.ParameterType;
                llvmWriter.AdjustOperandResultTypeToIncomingType(operand);

                index++;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="tryClause">
        /// </param>
        public static void WriteFunctionCall(this LlvmIndentedTextWriter writer, TryClause tryClause)
        {
            if (tryClause != null)
            {
                writer.Write("invoke ");
            }
            else
            {
                writer.Write("call ");
            }
        }

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
        /// <param name="resultNumberForReturn">
        /// </param>
        /// <param name="returnType">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteFunctionCallArguments(
            this IEnumerable<IParameter> parameterInfos, 
            OpCodePart[] used, 
            bool @isVirtual, 
            bool hasThis, 
            bool isCtor, 
            FullyDefinedReference resultNumberForThis, 
            IType thisType, 
            FullyDefinedReference resultNumberForReturn, 
            IType returnType, 
            LlvmWriter llvmWriter,
            bool varArg)
        {
            var writer = llvmWriter.Output;

            writer.Write("(");

            var index = 0;

            var returnIsStruct = returnType != null && returnType.IsStructureType();

            var comaRequired = false;

            // allocate space for structure if return type is structure
            if (returnIsStruct)
            {
                returnType.WriteTypePrefix(writer, returnType.IsStructureType());
                writer.Write(' ');
                if (resultNumberForReturn != null)
                {
                    llvmWriter.WriteResult(resultNumberForReturn);
                }

                comaRequired = true;
            }

            if (hasThis)
            {
                if (comaRequired)
                {
                    writer.Write(", ");
                }

                thisType.ToClass().WriteTypePrefix(writer);
                writer.Write(' ');
                if (resultNumberForThis != null)
                {
                    llvmWriter.WriteResult(resultNumberForThis);
                }
                else if (used != null && used.Length > 0)
                {
                    llvmWriter.WriteResult(used[0].Result);
                }

                comaRequired = true;
            }

            llvmWriter.CheckIfExternalDeclarationIsRequired(returnType);

            var argsContainsThisArg = used != null ? (used.Length - parameterInfos.Count()) > 0 : false;
            var argShift = (@isVirtual || (hasThis && !isCtor && argsContainsThisArg) ? 1 : 0);
            
            // add parameters
            foreach (var parameter in parameterInfos)
            {
                var effectiveIndex = index + argShift;
                var usedItem = used[effectiveIndex];

                if (comaRequired)
                {
                    writer.Write(", ");
                }

                llvmWriter.WriteFunctionCallParameterArgument(usedItem, parameter);

                comaRequired = true;
                index++;
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

                    llvmWriter.WriteFunctionCallVarArgument(usedItem, usedItem.Result.Type);

                    comaRequired = true;
                    index++;
                }
            }

            writer.Write(")");
        }

        private static void WriteFunctionCallParameterArgument(this LlvmWriter llvmWriter, OpCodePart opArg, IParameter parameter)
        {
            var writer = llvmWriter.Output;

            llvmWriter.CheckIfExternalDeclarationIsRequired(parameter.ParameterType);

            parameter.ParameterType.WriteTypePrefix(writer, parameter.ParameterType.IsStructureType());
            if (parameter.ParameterType.IsStructureType() && !parameter.IsOut && !parameter.IsRef)
            {
                writer.Write(" byval align " + LlvmWriter.PointerSize);
            }

            writer.Write(' ');
            llvmWriter.WriteResult(opArg);
        }

        private static void WriteFunctionCallVarArgument(this LlvmWriter llvmWriter, OpCodePart opArg, IType type)
        {
            var writer = llvmWriter.Output;

            llvmWriter.CheckIfExternalDeclarationIsRequired(type);

            type.WriteTypePrefix(writer, type.IsStructureType());
            if (type.IsStructureType())
            {
                writer.Write(" byval align " + LlvmWriter.PointerSize);
            }

            writer.Write(' ');
            llvmWriter.WriteResult(opArg);
        }

        /// <summary>
        /// </summary>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteFunctionCallAttributes(this IMethod methodInfo, LlvmIndentedTextWriter writer)
        {
            if (methodInfo.DllImportData != null && methodInfo.DllImportData.CallingConvention == CallingConvention.StdCall)
            {
                writer.Write("x86_stdcallcc ");
            }
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
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteFunctionCallLoadFunctionAddress(
            this IMethod methodInfo, OpCodePart opCodeMethodInfo, IType thisType, ref FullyDefinedReference methodAddressResultNumber, LlvmWriter llvmWriter)
        {
            if (!methodInfo.IsUnmanagedMethodReference)
            {
                return;
            }

            var writer = llvmWriter.Output;

            // if this is external method reference we need to load reference first
            // %4 = load i32 ()** @__glewCreateProgram, align 4
            // load pointer
            llvmWriter.WriteSetResultNumber(opCodeMethodInfo, llvmWriter.ResolveType("System.Byte").ToPointerType().ToPointerType());
            writer.Write("load ");
            llvmWriter.WriteMethodPointerType(writer, methodInfo, thisType);
            writer.Write("* ");
            llvmWriter.WriteMethodDefinitionName(writer, methodInfo);
            writer.Write(", align {0}", LlvmWriter.PointerSize);
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
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteFunctionCallPrepareThisExpression(
            this OpCodePart opCodeMethodInfo, IType thisType, OpCodePart opCodeFirstOperand, BaseWriter.ReturnResult resultOfFirstOperand, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            // check if you need to cast this parameter
            var isPrimitive = resultOfFirstOperand.Type.IsPrimitiveTypeOrEnum();
            var isPrimitivePointer = resultOfFirstOperand.Type.IsPointer && resultOfFirstOperand.Type.GetElementType().IsPrimitiveTypeOrEnum();

            var dynamicCastRequired = false;
            if (!isPrimitive && !isPrimitivePointer && thisType.IsClassCastRequired(opCodeFirstOperand, out dynamicCastRequired))
            {
                writer.WriteLine("; Cast of 'This' parameter");
                llvmWriter.WriteCast(opCodeFirstOperand, opCodeFirstOperand.Result, thisType);
                writer.WriteLine(string.Empty);
            }

            if (dynamicCastRequired)
            {
                writer.WriteLine("; Dynamic Cast of 'This' parameter");
                llvmWriter.WriteDynamicCast(writer, opCodeFirstOperand, opCodeFirstOperand.Result, thisType);
                writer.WriteLine(string.Empty);
            }

            if (isPrimitive || isPrimitivePointer)
            {
                var primitiveType = resultOfFirstOperand.Type;
                if (isPrimitivePointer)
                {
                    writer.WriteLine("; Box Primitive pointer type for 'This' parameter");

                    primitiveType = resultOfFirstOperand.Type.GetElementType();
                    var firstOperandResult = opCodeFirstOperand.Result;
                    opCodeFirstOperand.Result = null;
                    llvmWriter.WriteLlvmLoad(opCodeFirstOperand, firstOperandResult.Type.ToDereferencedType(), firstOperandResult);
                }
                else
                {
                    writer.WriteLine("; Box Primitive type(void*) for 'This' parameter");
                    var intType = llvmWriter.GetIntTypeByByteSize(LlvmWriter.PointerSize);
                    var uintType = llvmWriter.GetUIntTypeByByteSize(LlvmWriter.PointerSize);
                    if (intType.TypeEquals(primitiveType) || uintType.TypeEquals(primitiveType))
                    {
                        var declType = (opCodeMethodInfo as OpCodeMethodInfoPart).Operand.DeclaringType;
                        Debug.Assert(declType.IsStructureType(), "only Struct type can be used");
                        primitiveType = declType.ToClass();
                        llvmWriter.AdjustIntConvertableTypes(writer, opCodeMethodInfo.OpCodeOperands[0], declType.ToPointerType()); 
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
                primitiveType.ToClass().WriteCallBoxObjectMethod(llvmWriter, opCodeNone);
                opCodeFirstOperand.Result = opCodeNone.Result;
                writer.WriteLine(string.Empty);

                if (thisType.IsClassCastRequired(opCodeFirstOperand, out dynamicCastRequired))
                {
                    writer.WriteLine("; Cast of 'Boxed' 'This' parameter");
                    llvmWriter.WriteCast(opCodeFirstOperand, opCodeFirstOperand.Result, thisType);
                    writer.WriteLine(string.Empty);
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
        /// <param name="llvmWriter">
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
            LlvmWriter llvmWriter, 
            out IType thisType, 
            out bool hasThisArgument, 
            out OpCodePart opCodeFirstOperand, 
            out BaseWriter.ReturnResult resultOfFirstOperand, 
            out bool isIndirectMethodCall, 
            out IType ownerOfExplicitInterface, 
            out IType requiredType)
        {
            thisType = methodInfo.DeclaringType.ToClass();

            hasThisArgument = hasThis && opCodeMethodInfo.OpCodeOperands != null
                              && opCodeMethodInfo.OpCodeOperands.Length - methodInfo.GetParameters().Count() > 0;
            opCodeFirstOperand = opCodeMethodInfo.OpCodeOperands != null && opCodeMethodInfo.OpCodeOperands.Length > 0
                                     ? opCodeMethodInfo.OpCodeOperands[0]
                                     : null;
            resultOfFirstOperand = opCodeFirstOperand != null ? llvmWriter.ResultOf(opCodeFirstOperand) : null;

            isIndirectMethodCall = isVirtual
                                   && (methodInfo.IsAbstract || methodInfo.IsVirtual || (thisType.IsInterface && thisType.TypeEquals(resultOfFirstOperand.Type)));

            ownerOfExplicitInterface = isVirtual && thisType.IsInterface && thisType.TypeNotEquals(resultOfFirstOperand.Type) ? resultOfFirstOperand.Type : null;

            var rollbackType = false;
            requiredType = ownerOfExplicitInterface != null ? resultOfFirstOperand.Type : null;
            if (requiredType != null)
            {
                thisType = requiredType;
                rollbackType = true;
            }

            if (isIndirectMethodCall && methodInfo.DeclaringType.TypeNotEquals(thisType) && methodInfo.DeclaringType.IsInterface && !thisType.IsInterface
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
        /// <param name="llvmWriter">
        /// </param>
        /// <returns>
        /// </returns>
        public static FullyDefinedReference WriteFunctionCallResult(this IMethod methodInfo, OpCodePart opCodeMethodInfo, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var isReturnStructType = methodInfo != null && methodInfo.ReturnType.IsStructureType();
            var isReturnVoidType = methodInfo != null && methodInfo.ReturnType.IsVoid();

            // allocate space for structure if return type is structure
            if (isReturnStructType)
            {
                // TODO: for optimization, when it is used by Code.Stfld etc you can optimaze sending reference to a structure without allocating it in stack
                // todo so you need to request a destination reference as you did before

                // we need to store temp result of struct in stack to be used by "Ldfld, Ldflda"
                llvmWriter.WriteSetResultNumber(opCodeMethodInfo, methodInfo.ReturnType);
                llvmWriter.WriteAlloca(methodInfo.ReturnType);
                writer.WriteLine(string.Empty);
            }
            else if (!isReturnVoidType)
            {
                llvmWriter.WriteSetResultNumber(opCodeMethodInfo, methodInfo.ReturnType);
            }

            return opCodeMethodInfo.Result;
        }

        /// <summary>
        /// </summary>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteFunctionCallReturnType(this IMethod methodInfo, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            if (methodInfo != null && !methodInfo.ReturnType.IsVoid() && !methodInfo.ReturnType.IsStructureType())
            {
                methodInfo.ReturnType.WriteTypePrefix(writer, false);

                llvmWriter.CheckIfExternalDeclarationIsRequired(methodInfo.ReturnType);
            }
            else
            {
                // this is constructor
                writer.Write("void");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="tryClause">
        /// </param>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteFunctionCallUnwind(this TryClause tryClause, OpCodePart opCodeMethodInfo, LlvmWriter llvmWriter)
        {
            if (tryClause == null)
            {
                llvmWriter.WriteDbgLine(opCodeMethodInfo);
                return;
            }

            var writer = llvmWriter.Output;

            var nextAddress = llvmWriter.GetBlockJumpAddress();

            var label = string.Concat("next", nextAddress);

            writer.WriteLine(string.Empty);
            writer.Indent++;
            writer.Write("to label %.{0} unwind label %.catch{1}", label, tryClause.Catches.First().Offset);
            writer.Indent--;

            llvmWriter.WriteDbgLine(opCodeMethodInfo);
            writer.WriteLine(string.Empty);

            writer.Indent--;
            writer.WriteLine(".{0}:", label);
            writer.Indent++;

            LlvmHelpersGen.SetCustomLabel(opCodeMethodInfo, label);
        }

        /// <summary>
        /// </summary>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="methodAddressResultNumber">
        /// </param>
        /// <param name="ownerOfExplicitInterface">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteFunctionNameExpression(
            this IMethod methodInfo, FullyDefinedReference methodAddressResultNumber, IType ownerOfExplicitInterface, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var isIndirectMethodCall = methodAddressResultNumber != null;
            if (isIndirectMethodCall || methodInfo.IsUnmanagedMethodReference)
            {
                llvmWriter.WriteResult(methodAddressResultNumber);
            }
            else
            {
                // default method name
                llvmWriter.WriteMethodDefinitionName(writer, methodInfo, ownerOfExplicitInterface);
            }
        }
    }
}