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
    using System.Linq;

    using Il2Native.Logic.CodeParts;
    using Il2Native.Logic.Exceptions;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public static class LlvmHelpersGen
    {
        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="thisType">
        /// </param>
        /// <param name="opCodeFirstOperand">
        /// </param>
        /// <param name="resultOfirstOperand">
        /// </param>
        /// <param name="virtualMethodAddressResultNumber">
        /// </param>
        /// <param name="requiredType">
        /// </param>
        public static FullyDefinedReference GenerateVirtualCall(
            this LlvmWriter llvmWriter,
            OpCodePart opCodeMethodInfo,
            IMethod methodInfo,
            IType thisType,
            OpCodePart opCodeFirstOperand,
            BaseWriter.ReturnResult resultOfirstOperand,
            ref IType requiredType)
        {
            var writer = llvmWriter.Output;

            FullyDefinedReference virtualMethodAddressResultNumber = null;

            if (thisType.IsInterface && resultOfirstOperand.Type.TypeNotEquals(thisType))
            {
                // we need to extract interface from an object
                requiredType = thisType;
            }

            // get pointer to Virtual Table and call method
            // 1) get pointer to virtual table
            writer.WriteLine("; Get Virtual Table");

            IType requiredInterface;
            var effectiveType = requiredType ?? thisType;
            var methodIndex = effectiveType.GetVirtualMethodIndex(methodInfo, llvmWriter, out requiredInterface);

            if (requiredInterface != null)
            {
                llvmWriter.WriteInterfaceAccess(writer, opCodeMethodInfo.OpCodeOperands[0], effectiveType, requiredInterface);
                opCodeMethodInfo.Result = opCodeMethodInfo.OpCodeOperands[0].Result;
                requiredType = requiredInterface;
            }

            llvmWriter.UnaryOper(writer, opCodeMethodInfo, "bitcast", requiredType);
            writer.Write(" to ");
            llvmWriter.WriteMethodPointerType(writer, methodInfo, thisType);
            writer.WriteLine("**");

            var pointerToInterfaceVirtualTablePointersResultNumber = opCodeMethodInfo.Result;

            // load pointer
            llvmWriter.WriteSetResultNumber(opCodeMethodInfo, llvmWriter.ResolveType("System.Byte").ToPointerType().ToPointerType());
            writer.Write("load ");
            llvmWriter.WriteMethodPointerType(writer, methodInfo, thisType);
            writer.Write("** ");
            llvmWriter.WriteResult(pointerToInterfaceVirtualTablePointersResultNumber);
            writer.Write(", align {0}", LlvmWriter.PointerSize);
            writer.WriteLine(string.Empty);
            var virtualTableOfMethodPointersResultNumber = opCodeMethodInfo.Result;

            // get address of a function
            writer.WriteLine("; Get Virtual Index of Method: {0}", methodInfo.FullName);
            llvmWriter.WriteSetResultNumber(opCodeMethodInfo, llvmWriter.ResolveType("System.Byte").ToPointerType());
            writer.Write("getelementptr inbounds ");
            llvmWriter.WriteMethodPointerType(writer, methodInfo, thisType);
            writer.Write("* ");
            llvmWriter.WriteResult(virtualTableOfMethodPointersResultNumber);
            writer.WriteLine(", i64 {0}", methodIndex);
            var pointerToFunctionPointerResultNumber = opCodeMethodInfo.Result;

            // load method address
            llvmWriter.WriteSetResultNumber(opCodeMethodInfo, llvmWriter.ResolveType("System.Byte").ToPointerType());
            writer.Write("load ");
            llvmWriter.WriteMethodPointerType(writer, methodInfo, thisType);
            writer.Write("* ");
            llvmWriter.WriteResult(pointerToFunctionPointerResultNumber);
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

            return virtualMethodAddressResultNumber;
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="realConvert">
        /// </param>
        /// <param name="intConvert">
        /// </param>
        /// <param name="toType">
        /// </param>
        /// <param name="toAddress">
        /// </param>
        /// <param name="typesToExclude">
        /// </param>
        public static void LlvmConvert(
            this LlvmWriter llvmWriter, OpCodePart opCode, string realConvert, string intConvert, string toType, bool toAddress, params IType[] typesToExclude)
        {
            var writer = llvmWriter.Output;

            var resultOf = llvmWriter.ResultOf(opCode.OpCodeOperands[0]);
            var areBothPointers = (resultOf.Type.IsPointer || resultOf.Type.IsByRef) && toAddress;
            if (!typesToExclude.Any(t => resultOf.Type.TypeEquals(t)) && !areBothPointers)
            {
                if (resultOf.Type.IsReal())
                {
                    llvmWriter.UnaryOper(writer, opCode, realConvert, options: LlvmWriter.OperandOptions.GenerateResult);
                }
                else if (resultOf.Type.IsPointer || resultOf.Type.IsByRef)
                {
                    llvmWriter.UnaryOper(writer, opCode, "ptrtoint", options: LlvmWriter.OperandOptions.GenerateResult);
                }
                else if (toType.EndsWith("*") || toType.EndsWith("&"))
                {
                    llvmWriter.UnaryOper(writer, opCode, "inttoptr", options: LlvmWriter.OperandOptions.GenerateResult);
                }
                else
                {
                    llvmWriter.UnaryOper(writer, opCode, intConvert, options: LlvmWriter.OperandOptions.GenerateResult);
                }

                writer.Write(" to {0}", toType);
            }
            else
            {
                if (opCode.OpCodeOperands[0].Result != null)
                {
                    opCode.Result = opCode.OpCodeOperands[0].Result;
                }
                else
                {
                    llvmWriter.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    opCode.Result = opCode.OpCodeOperands[0].Result;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="intConvert">
        /// </param>
        /// <param name="toType">
        /// </param>
        public static void LlvmIntConvert(this LlvmWriter llvmWriter, OpCodePart opCode, string intConvert, string toType)
        {
            var writer = llvmWriter.Output;

            var incomingResult = opCode.Result;

            llvmWriter.PreProcess(writer, opCode);
            llvmWriter.ProcessOperator(writer, opCode, intConvert, opCode.Result.Type);

            var returnResult = opCode.Result;

            opCode.Result = incomingResult;

            llvmWriter.PostProcess(writer, opCode);

            writer.Write(" to {0}", toType);

            opCode.Result = returnResult;
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
            writer.Write(", align " + LlvmWriter.PointerSize);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="toType">
        /// </param>
        public static void WriteBitcast(this LlvmWriter llvmWriter, OpCodePart opCode, IType toType)
        {
            var writer = llvmWriter.Output;

            llvmWriter.WriteSetResultNumber(opCode, toType);
            writer.Write("bitcast ");
            opCode.Result.Type.WriteTypePrefix(writer, true);
            writer.Write(" ");
            llvmWriter.WriteResult(opCode.Result);
            writer.Write(" to ");
            toType.WriteTypePrefix(writer, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="source">
        /// </param>
        /// <param name="toType">
        /// </param>
        public static void WriteBitcast(this LlvmWriter llvmWriter, OpCodePart opCode, FullyDefinedReference source, IType toType)
        {
            var writer = llvmWriter.Output;

            llvmWriter.WriteSetResultNumber(opCode, toType);
            writer.Write("bitcast ");
            source.Type.WriteTypePrefix(writer, true);
            writer.Write(" ");
            llvmWriter.WriteResult(source);
            writer.Write(" to ");
            toType.WriteTypePrefix(writer, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="result">
        /// </param>
        public static void WriteBitcast(this LlvmWriter llvmWriter, OpCodePart opCode, FullyDefinedReference result)
        {
            var writer = llvmWriter.Output;

            llvmWriter.WriteSetResultNumber(opCode, llvmWriter.ResolveType("System.Byte").ToPointerType());
            writer.Write("bitcast ");
            result.Type.WriteTypePrefix(writer, true);
            writer.Write(" ");
            llvmWriter.WriteResult(result);
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
        public static void WriteBitcast(this LlvmWriter llvmWriter, OpCodePart opCode, IType fromType, IncrementalResult result, string custom)
        {
            var writer = llvmWriter.Output;

            llvmWriter.WriteSetResultNumber(opCode, null);
            writer.Write("bitcast ");
            fromType.WriteTypePrefix(writer, true);
            writer.Write(' ');
            llvmWriter.WriteResult(result);
            writer.Write(" to ");
            writer.Write(custom);

            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
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
        /// <param name="tryClause">
        /// </param>
        public static void WriteCall(
            this LlvmWriter llvmWriter,
            OpCodePart opCodeMethodInfo,
            IMethod methodInfo,
            bool isVirtual,
            bool hasThis,
            bool isCtor,
            FullyDefinedReference thisResultNumber,
            TryClause tryClause)
        {
            if (opCodeMethodInfo.HasResult)
            {
                return;
            }

            if (methodInfo != null
                && methodInfo.ReturnType.IsStructureType()
                && opCodeMethodInfo.UsedBy != null && !opCodeMethodInfo.UsedBy.Any(Code.Ldfld, Code.Ldflda, Code.Call, Code.Callvirt, Code.Newobj, Code.Box, Code.Unbox, Code.Unbox_Any, Code.Pop)
                && opCodeMethodInfo.Destination == null)
            {
                // You should not allocate it yourself, as result of function should be stored in DestinationName or in Return Value such as "agg.return"
                // so if Destination is null means that call is not ready and will be ready later when DestinationName is set
                // note: if method which returns structure used by "Field" when we need to generate temp result of a function in stack
                return;
            }

            var writer = llvmWriter.Output;

            llvmWriter.CheckIfExternalDeclarationIsRequired(methodInfo);
            llvmWriter.CheckIfExternalDeclarationIsRequired(methodInfo.DeclaringType);

            IType thisType;
            bool hasThisArgument;
            OpCodePart opCodeFirstOperand;
            BaseWriter.ReturnResult resultOfFirstOperand;
            bool isIndirectMethodCall;
            IType ownerOfExplicitInterface;
            IType requiredType;
            methodInfo.WriteFunctionCallProlog(opCodeMethodInfo, isVirtual, hasThis, llvmWriter, out thisType, out hasThisArgument, out opCodeFirstOperand, out resultOfFirstOperand, out isIndirectMethodCall, out ownerOfExplicitInterface, out requiredType);

            opCodeMethodInfo.WriteCallFunctionPreProcessOperands(llvmWriter);

            if (hasThisArgument)
            {
                opCodeMethodInfo.WriteFunctionCallPrepareThisExpression(thisType, opCodeFirstOperand, resultOfFirstOperand, llvmWriter);
            }

            FullyDefinedReference methodAddressResultNumber = null;
            if (isIndirectMethodCall)
            {
                methodAddressResultNumber = llvmWriter.GenerateVirtualCall(
                    opCodeMethodInfo, methodInfo, thisType, opCodeFirstOperand, resultOfFirstOperand, ref requiredType);
            }

            methodInfo.WriteFunctionCallLoadFunctionAddress(opCodeMethodInfo, thisType, ref methodAddressResultNumber, llvmWriter);

            methodInfo.PreProcessCallParameters(opCodeMethodInfo, llvmWriter);

            var returnFullyDefinedReference = methodInfo.WriteFunctionCallResult(opCodeMethodInfo, llvmWriter);

            writer.WriteFunctionCall(tryClause);

            methodInfo.WriteFunctionCallAttributes(writer);

            methodInfo.WriteFunctionCallReturnType(llvmWriter);

            writer.Write(' ');

            if (methodInfo.IsExternalLibraryMethod())
            {
                writer.Write("(...)* ");
            }

            methodInfo.WriteFunctionNameExpression(methodAddressResultNumber, ownerOfExplicitInterface, llvmWriter);

            methodInfo.GetParameters().WriteFunctionCallArguments(
                opCodeMethodInfo.OpCodeOperands,
                isVirtual,
                hasThis,
                isCtor,
                thisResultNumber,
                thisType,
                returnFullyDefinedReference,
                methodInfo != null ? methodInfo.ReturnType : null,
                llvmWriter);

            tryClause.WriteFunctionCallUnwind(opCodeMethodInfo, llvmWriter);
        }

        public static void SetCustomLabel(OpCodePart opCodePart, string label)
        {
            if (opCodePart.AddressStart == 0 && opCodePart.UsedBy != null)
            {
                opCodePart.UsedBy.OpCode.CreatedLabel = label;
            }
            else
            {
                opCodePart.CreatedLabel = label;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="fromResult">
        /// </param>
        /// <param name="effectiveToType">
        /// </param>
        /// <param name="appendReference">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool WriteCast(this LlvmWriter llvmWriter, OpCodePart opCode, FullyDefinedReference fromResult, IType toType, bool throwExceptionIfNull = false)
        {
            var writer = llvmWriter.Output;

            var bareType = fromResult.Type.ToBareType();
            if (toType.IsInterface)
            {
                if (bareType.GetAllInterfaces().Contains(toType))
                {
                    opCode.Result = fromResult;
                    llvmWriter.WriteInterfaceAccess(writer, opCode, bareType, toType);
                }
                else
                {
                    llvmWriter.WriteDynamicCast(writer, opCode, fromResult, toType, true, throwExceptionIfNull);
                }
            }
            else
            {
                if (fromResult.Type.IsArray || toType.IsArray || toType.IsPointer || bareType.IsDerivedFrom(toType))
                {
                    llvmWriter.WriteSetResultNumber(opCode, toType);
                    writer.Write("bitcast ");
                    fromResult.Type.WriteTypePrefix(writer, true);
                    writer.Write(' ');
                    llvmWriter.WriteResult(fromResult);
                    writer.Write(" to ");
                    toType.WriteTypePrefix(writer, true);
                }
                else
                {
                    llvmWriter.WriteDynamicCast(writer, opCode, fromResult, toType, true, throwExceptionIfNull);
                }
            }

            writer.WriteLine(string.Empty);

            return true;
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
        /// <param name="doNotConvert">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public static void WriteCast(
            this LlvmWriter llvmWriter,
            OpCodePart opCode,
            IType fromType,
            string custromName,
            IType toType,
            bool appendReference = false,
            bool doNotConvert = false)
        {
            // TODO: remove this one. use anather one
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

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="source">
        /// </param>
        /// <param name="toType">
        /// </param>
        public static void WriteIntToPtr(this LlvmWriter llvmWriter, OpCodePart opCode, FullyDefinedReference source, IType toType)
        {
            var writer = llvmWriter.Output;

            llvmWriter.WriteSetResultNumber(opCode, toType);
            writer.Write("inttoptr ");
            source.Type.WriteTypePrefix(writer);
            writer.Write(" ");
            llvmWriter.WriteResult(source);
            writer.Write(" to ");
            toType.WriteTypePrefix(writer, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="typeToLoad">
        /// </param>
        /// <param name="source">
        /// </param>
        /// <param name="appendReference">
        /// </param>
        /// <param name="structAsRef">
        /// </param>
        public static void WriteLlvmLoad(
            this LlvmWriter llvmWriter, OpCodePart opCode, IType typeToLoad, IncrementalResult source, bool appendReference = true, bool structAsRef = false)
        {
            llvmWriter.WriteLlvmLoad(opCode, typeToLoad, new FullyDefinedReference(source.ToString(), source.Type), appendReference, structAsRef);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="source">
        /// </param>
        /// <param name="appendReference">
        /// </param>
        /// <param name="structAsRef">
        /// </param>
        public static void WriteLlvmLoad(
            this LlvmWriter llvmWriter, OpCodePart opCode, FullyDefinedReference source, bool appendReference = true, bool structAsRef = false)
        {
            llvmWriter.WriteLlvmLoad(opCode, source.Type, source, appendReference, structAsRef);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="typeToLoad">
        /// </param>
        /// <param name="source">
        /// </param>
        /// <param name="appendReference">
        /// </param>
        /// <param name="structAsRef">
        /// </param>
        public static void WriteLlvmLoad(
            this LlvmWriter llvmWriter, OpCodePart opCode, IType typeToLoad, FullyDefinedReference source, bool appendReference = true, bool structAsRef = false)
        {
            // TODO: improve it, by checking if Source is Reference or Pointer

            var writer = llvmWriter.Output;

            Debug.Assert(!typeToLoad.IsStructureType() || typeToLoad.IsStructureType() && opCode.Destination != null);

            if (!typeToLoad.IsStructureType() || structAsRef || opCode.Destination == null)
            {
                if (opCode.HasResult)
                {
                    return;
                }

                ////Debug.Assert(source.Type.IsPointer);
                var dereferencedType = source.Type.IsPointer ? source.Type.GetElementType() : null;

                var effectiveSource = source;

                // check if you need bitcast pointer type
                if (!typeToLoad.IsPointer && dereferencedType != null && typeToLoad.TypeNotEquals(dereferencedType))
                {
                    // check if you need cast here
                    llvmWriter.WriteBitcast(opCode, source, typeToLoad);
                    writer.WriteLine(string.Empty);
                    effectiveSource = opCode.Result;
                }

                if (dereferencedType == null && !source.Type.IsPointer && !source.Type.IsByRef && typeToLoad.IntTypeBitSize() != source.Type.IntTypeBitSize()
                    && typeToLoad.IntTypeBitSize() > 0)
                {
                    // check if you need cast here
                    llvmWriter.WriteIntToPtr(opCode, source, typeToLoad);
                    writer.WriteLine(string.Empty);
                    effectiveSource = opCode.Result;
                }

                llvmWriter.WriteSetResultNumber(opCode, typeToLoad);

                // last part
                writer.Write("load ");
                typeToLoad.WriteTypePrefix(writer, structAsRef);
                if (appendReference)
                {
                    // add reference to type
                    writer.Write('*');
                }

                writer.Write(' ');
                writer.Write(effectiveSource.ToString());

                // TODO: optional do we need to calculate it propertly?
                writer.Write(", align " + LlvmWriter.PointerSize);
            }
            else
            {
                llvmWriter.WriteCopyStruct(writer, opCode, source, opCode.Destination);
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

            var localType = llvmWriter.LocalInfo[index].LocalType;

            localType.WriteTypePrefix(writer, false);
            if (asReference)
            {
                writer.Write('*');
            }

            writer.Write(' ');
            writer.Write(llvmWriter.GetLocalVarName(index));

            // TODO: optional do we need to calculate it propertly?
            writer.Write(", align " + LlvmWriter.PointerSize);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="op1">
        /// </param>
        /// <param name="op2">
        /// </param>
        public static void WriteMemCopy(this LlvmWriter llvmWriter, IType type, FullyDefinedReference op1, FullyDefinedReference op2)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine(
                "call void @llvm.memcpy.p0i8.p0i8.i32(i8* {0}, i8* {1}, i32 {2}, i32 {3}, i1 false)", op1, op2, type.GetTypeSize(), LlvmWriter.PointerSize

                /*Align*/);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="op1">
        /// </param>
        public static void WriteMemSet(this LlvmWriter llvmWriter, IType type, FullyDefinedReference op1)
        {
            var writer = llvmWriter.Output;

            writer.Write(
                "call void @llvm.memset.p0i8.i32(i8* {0}, i8 0, i32 {1}, i32 {2}, i1 false)", op1, type.GetTypeSize(), LlvmWriter.PointerSize /*Align*/);
        }
    }
}