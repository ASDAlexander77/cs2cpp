// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LlvmHelpersGen.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Il2Native.Logic.CodeParts;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class LlvmHelpersGen
    {
        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="op1">
        /// </param>
        /// <param name="op2">
        /// </param>
        public static void WriteMemCopy(this LlvmWriter llvmWriter, IType type, LlvmResult op1, LlvmResult op2)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine(
                "call void @llvm.memcpy.p0i8.p0i8.i32(i8* {0}, i8* {1}, i32 {2}, i32 {3}, i1 false)",
                llvmWriter.GetResultNumber(op1),
                llvmWriter.GetResultNumber(op2),
                type.GetTypeSize(),
                LlvmWriter.pointerSize /*Align*/);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="op1">
        /// </param>
        public static void WriteMemSet(this LlvmWriter llvmWriter, IType type, LlvmResult op1)
        {
            var writer = llvmWriter.Output;

            writer.Write(
                "call void @llvm.memset.p0i8.i32(i8* {0}, i8 0, i32 {1}, i32 {2}, i1 false)",
                llvmWriter.GetResultNumber(op1),
                type.GetTypeSize(),
                LlvmWriter.pointerSize /*Align*/);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="localVarName">
        /// </param>
        /// <param name="appendReference">
        /// </param>
        /// <param name="structAsRef">
        /// </param>
        public static void WriteLlvmLoad(
            this LlvmWriter llvmWriter, OpCodePart opCode, IType type, string localVarName, bool appendReference = true, bool structAsRef = false)
        {
            if (opCode.HasResult)
            {
                return;
            }

            var writer = llvmWriter.Output;

            if (!type.IsStructureType() || structAsRef)
            {
                llvmWriter.WriteSetResultNumber(opCode, type);

                // last part
                writer.Write("load ");
                type.WriteTypePrefix(writer, structAsRef);
                if (appendReference)
                {
                    // add reference to type
                    writer.Write('*');
                }

                writer.Write(' ');
                writer.Write(localVarName);

                // TODO: optional do we need to calculate it propertly?
                writer.Write(", align " + LlvmWriter.pointerSize);
            }
            else
            {
                Debug.Assert(opCode.DestinationName != null);
                llvmWriter.WriteCopyStruct(writer, opCode, type, localVarName, opCode.DestinationName);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <param name="asReference">
        /// </param>
        public static void WriteLlvmLocalVarAccess(this LlvmWriter llvmWriter, int index, bool asReference = false)
        {
            var writer = llvmWriter.Output;

            llvmWriter.LocalInfo[index].LocalType.WriteTypePrefix(writer, false);
            if (asReference)
            {
                writer.Write('*');
            }

            writer.Write(' ');
            writer.Write(llvmWriter.GetLocalVarName(index));

            // TODO: optional do we need to calculate it propertly?
            writer.Write(", align " + LlvmWriter.pointerSize);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="type">
        /// </param>
        public static void WriteAlloca(this LlvmWriter llvmWriter, IType type)
        {
            var writer = llvmWriter.Output;

            // for value types
            writer.Write("alloca ");
            type.WriteTypePrefix(writer);
            writer.Write(", align " + LlvmWriter.pointerSize);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="result">
        /// </param>
        /// <param name="toType">
        /// </param>
        public static void WriteBitcast(this LlvmWriter llvmWriter, OpCodePart opCode, LlvmResult result, IType toType)
        {
            var writer = llvmWriter.Output;

            llvmWriter.WriteSetResultNumber(opCode, TypeAdapter.FromType(typeof(byte*)));
            writer.Write("bitcast i8* ");
            llvmWriter.WriteResultNumber(result);
            writer.Write(" to ");
            toType.WriteTypePrefix(writer, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="toType">
        /// </param>
        /// <param name="options">
        /// </param>
        public static void WriteBitcast(
            this LlvmWriter llvmWriter, OpCodePart opCode, IType toType, LlvmWriter.OperandOptions options = LlvmWriter.OperandOptions.None)
        {
            var writer = llvmWriter.Output;
            llvmWriter.UnaryOper(writer, opCode, "bitcast", opCode.Result.Type, TypeAdapter.FromType(typeof(byte*)), options: options);
            writer.Write(" to ");
            toType.WriteTypePrefix(writer, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="fromType">
        /// </param>
        /// <param name="name">
        /// </param>
        public static void WriteBitcast(this LlvmWriter llvmWriter, OpCodePart opCode, IType fromType, string name)
        {
            var writer = llvmWriter.Output;

            llvmWriter.WriteSetResultNumber(opCode, TypeAdapter.FromType(typeof(byte*)));
            writer.Write("bitcast ");
            fromType.WriteTypePrefix(writer, true);
            writer.Write(" ");
            writer.Write(name);
            writer.Write(" to i8*");
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="fromType">
        /// </param>
        /// <param name="result">
        /// </param>
        /// <param name="custom">
        /// </param>
        public static void WriteBitcast(this LlvmWriter llvmWriter, OpCodePart opCode, IType fromType, LlvmResult result, string custom)
        {
            var writer = llvmWriter.Output;

            llvmWriter.WriteSetResultNumber(opCode, null);
            writer.Write("bitcast ");
            fromType.WriteTypePrefix(writer, true);
            writer.Write(' ');
            llvmWriter.WriteResultNumber(result);
            writer.Write(" to ");
            writer.Write(custom);

            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="methodBase">
        /// </param>
        /// <param name="isVirtual">
        /// </param>
        /// <param name="hasThis">
        /// </param>
        /// <param name="isCtor">
        /// </param>
        /// <param name="thisResultNumber">
        /// </param>
        public static void WriteCall(this LlvmWriter llvmWriter,
                OpCodePart opCodeMethodInfo, IMethod methodBase, bool isVirtual, bool hasThis, bool isCtor, LlvmResult thisResultNumber, IExceptionHandlingClause exceptionHandlingClause)
        {
            if (opCodeMethodInfo.HasResult)
            {
                return;
            }

            var writer = llvmWriter.Output;

            llvmWriter.CheckIfExternalDeclarationIsRequired(methodBase);

            var preProcessedOperandResults = new List<bool>();

            if (opCodeMethodInfo.OpCodeOperands != null)
            {
                var index = 0;
                foreach (var operand in opCodeMethodInfo.OpCodeOperands)
                {
                    preProcessedOperandResults.Add(llvmWriter.PreProcessOperand(writer, opCodeMethodInfo, index));
                    index++;
                }
            }

            var methodInfo = methodBase;
            var thisType = methodBase.DeclaringType;

            var hasThisArgument = hasThis && opCodeMethodInfo.OpCodeOperands != null && opCodeMethodInfo.OpCodeOperands.Length > 0;
            var opCodeFirstOperand = opCodeMethodInfo.OpCodeOperands != null && opCodeMethodInfo.OpCodeOperands.Length > 0 ? opCodeMethodInfo.OpCodeOperands[0] : null;
            var resultOfirstOperand = opCodeFirstOperand != null ? llvmWriter.ResultOf(opCodeFirstOperand) : null;

            var startsWithThis = hasThisArgument && opCodeFirstOperand.Any(Code.Ldarg_0);

            LlvmResult virtualMethodAddressResultNumber = null;
            var isInderectMethodCall = isVirtual && (methodBase.IsAbstract || methodBase.IsVirtual || (thisType.IsInterface && thisType.TypeEquals(resultOfirstOperand.IType)));

            var ownerOfExplicitInterface = isVirtual && thisType.IsInterface && thisType.TypeNotEquals(resultOfirstOperand.IType) ? resultOfirstOperand.IType : null;
            var requiredType = ownerOfExplicitInterface != null ? resultOfirstOperand.IType : null;
            if (requiredType != null)
            {
                thisType = requiredType;
            }

            if (isInderectMethodCall)
            {
                if (thisType.IsInterface && !resultOfirstOperand.IType.Equals(thisType))
                {
                    // we need to extract interface from an object
                    requiredType = thisType;
                }
                else if (!methodBase.DeclaringType.Equals(thisType) && methodBase.DeclaringType.IsInterface)
                {
                    // we need to extract interface from an object
                    requiredType = methodBase.DeclaringType;
                }

                // get pointer to Virtual Table and call method
                // 1) get pointer to virtual table
                writer.WriteLine("; Get Virtual Table");
                llvmWriter.UnaryOper(writer, opCodeMethodInfo, "bitcast", requiredType);
                writer.Write(" to ");
                llvmWriter.WriteMethodPointerType(writer, methodInfo);
                writer.WriteLine("**");
                var pointerToInterfaceVirtualTablePointersResultNumber = opCodeMethodInfo.Result;

                // load pointer
                llvmWriter.WriteSetResultNumber(opCodeMethodInfo, TypeAdapter.FromType(typeof(byte**)));
                writer.Write("load ");
                llvmWriter.WriteMethodPointerType(writer, methodInfo);
                writer.Write("** ");
                llvmWriter.WriteResultNumber(pointerToInterfaceVirtualTablePointersResultNumber);
                writer.WriteLine(string.Empty);
                var virtualTableOfMethodPointersResultNumber = opCodeMethodInfo.Result;

                // get address of a function
                llvmWriter.WriteSetResultNumber(opCodeMethodInfo, TypeAdapter.FromType(typeof(byte*)));
                writer.Write("getelementptr inbounds ");
                llvmWriter.WriteMethodPointerType(writer, methodInfo);
                writer.Write("* ");
                llvmWriter.WriteResultNumber(virtualTableOfMethodPointersResultNumber);
                writer.WriteLine(", i64 {0}", (requiredType ?? thisType).GetVirtualMethodIndex(methodInfo));
                var pointerToFunctionPointerResultNumber = opCodeMethodInfo.Result;

                // load method address
                llvmWriter.WriteSetResultNumber(opCodeMethodInfo, TypeAdapter.FromType(typeof(byte*)));
                writer.Write("load ");
                llvmWriter.WriteMethodPointerType(writer, methodInfo);
                writer.Write("* ");
                llvmWriter.WriteResultNumber(pointerToFunctionPointerResultNumber);
                writer.WriteLine(string.Empty);
                // remember virtual method address result
                virtualMethodAddressResultNumber = opCodeMethodInfo.Result;

                if (thisType.IsInterface)
                {
                    opCodeFirstOperand.Result = virtualTableOfMethodPointersResultNumber;

                    llvmWriter.WriteGetThisPointerFromInterfacePointer(
                        writer, opCodeMethodInfo, methodInfo, thisType, pointerToInterfaceVirtualTablePointersResultNumber);

                    var thisPointerResultNumber = opCodeMethodInfo.Result;
                    // set ot for Call op code
                    opCodeMethodInfo.OpCodeOperands[0].Result = thisPointerResultNumber;
                }
            }

            // check if you need to cast this parameter
            if (hasThisArgument && thisType.IsClassCastRequired(opCodeFirstOperand))
            {
                llvmWriter.WriteCast(opCodeFirstOperand, opCodeFirstOperand.Result, thisType);
                writer.WriteLine(string.Empty);
            }

            // check if you need to cast parameter
            if (opCodeMethodInfo.OpCodeOperands != null)
            {
                var index = startsWithThis && !isCtor ? 1 : 0;
                foreach (var parameter in methodBase.GetParameters())
                {
                    var operand = opCodeMethodInfo.OpCodeOperands[index];

                    if (parameter.ParameterType.IsClassCastRequired(operand))
                    {
                        llvmWriter.WriteCast(operand, operand.Result, parameter.ParameterType);
                    }

                    index++;
                }
            }

            if (methodInfo != null && !methodInfo.ReturnType.IsVoid())
            {
                llvmWriter.WriteSetResultNumber(opCodeMethodInfo, methodInfo.ReturnType);
            }

            // allocate space for structure if return type is structure
            if (methodInfo != null && methodInfo.ReturnType.IsStructureType())
            {
                llvmWriter.WriteAlloca(methodInfo.ReturnType);
                writer.WriteLine(string.Empty);
            }

            if (exceptionHandlingClause != null)
            {
                writer.Write("invoke ");
            }
            else
            {
                writer.Write("call ");
            }

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

            writer.Write(' ');

            if (isInderectMethodCall)
            {
                llvmWriter.WriteResultNumber(virtualMethodAddressResultNumber);
            }
            else
            {
                llvmWriter.WriteMethodDefinitionName(writer, methodBase, ownerOfExplicitInterface);
            }

            llvmWriter.ActualWrite(
                writer,
                opCodeMethodInfo.OpCodeOperands,
                methodBase.GetParameters(),
                isVirtual,
                hasThis,
                isCtor,
                preProcessedOperandResults,
                thisResultNumber,
                thisType,
                opCodeMethodInfo.Result,
                methodInfo != null ? methodInfo.ReturnType : null);

            if (exceptionHandlingClause != null)
            {
                var nextOpCode = opCodeMethodInfo.NextOpCode(llvmWriter);
                var nextIsBrunch = nextOpCode.Any(Code.Br, Code.Br_S, Code.Leave, Code.Leave_S);
                var nextAddress = nextIsBrunch ? nextOpCode.JumpAddress() : opCodeMethodInfo.AddressEnd;

                writer.WriteLine(string.Empty);
                writer.Indent++;
                writer.WriteLine("to label %.a{0} unwind label %.catch{1}", nextAddress, exceptionHandlingClause.HandlerOffset);
                writer.Indent--;
                if (!nextIsBrunch)
                {
                    writer.Indent--;
                    writer.WriteLine(".a{0}:", opCodeMethodInfo.GroupAddressEnd);
                    writer.Indent++;
                    opCodeMethodInfo.NextOpCode(llvmWriter).JumpProcessed = true;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="fromType">
        /// </param>
        /// <param name="fromResult">
        /// </param>
        /// <param name="toType">
        /// </param>
        /// <param name="appendReference">
        /// </param>
        public static void WriteCast(this LlvmWriter llvmWriter, OpCodePart opCode, LlvmResult fromResult, IType toType, bool appendReference = false)
        {
            var writer = llvmWriter.Output;

            if (!fromResult.Type.IsInterface && toType.IsInterface)
            {
                opCode.Result = fromResult;
                llvmWriter.WriteInterfaceAccess(writer, opCode, fromResult.Type, toType);
            }
            else
            {
                llvmWriter.WriteSetResultNumber(opCode, toType);
                writer.Write("bitcast ");
                fromResult.Type.WriteTypePrefix(writer, true);
                writer.Write(' ');
                llvmWriter.WriteResultNumber(fromResult);
                writer.Write(" to ");
                toType.WriteTypePrefix(writer, true);
                if (appendReference)
                {
                    // result should be array
                    writer.Write('*');
                }
            }

            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="fromType">
        /// </param>
        /// <param name="custromName">
        /// </param>
        /// <param name="toType">
        /// </param>
        /// <param name="appendReference">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public static void WriteCast(
            this LlvmWriter llvmWriter, OpCodePart opCode, IType fromType, string custromName, IType toType, bool appendReference = false)
        {
            var writer = llvmWriter.Output;

            if (!fromType.IsInterface && toType.IsInterface)
            {
                throw new NotImplementedException();

                ////opCode.Result = res;
                ////this.WriteInterfaceAccess(writer, opCode, fromType, toType);
            }
            else
            {
                llvmWriter.WriteSetResultNumber(opCode, toType);
                writer.Write("bitcast ");
                fromType.WriteTypePrefix(writer, true);
                writer.Write(' ');
                writer.Write(custromName);
                writer.Write(" to ");
                toType.WriteTypePrefix(writer, true);
                if (appendReference)
                {
                    // result should be array
                    writer.Write('*');
                }
            }

            writer.WriteLine(string.Empty);
        }
    }
}