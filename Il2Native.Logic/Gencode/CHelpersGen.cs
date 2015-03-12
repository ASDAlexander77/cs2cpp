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
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using CodeParts;
    using Exceptions;
    using InternalMethods;
    using PEAssemblyReader;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public static class CHelpersGen
    {
        /// <summary>
        /// </summary>
        /// <param name="cWriter">
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
        /// <param name="requiredType">
        /// </param>
        /// <returns>
        /// </returns>
        public static void GenerateVirtualCall(
            this CWriter cWriter,
            OpCodePart opCodeMethodInfo,
            IMethod methodInfo,
            IType thisType,
            OpCodePart opCodeFirstOperand,
            BaseWriter.ReturnResult resultOfirstOperand,
            ref IType requiredType)
        {
            var writer = cWriter.Output;

            if (thisType.IsInterface && resultOfirstOperand.Type.TypeNotEquals(thisType))
            {
                // we need to extract interface from an object
                requiredType = thisType;
            }

            // get pointer to Virtual Table and call method
            // 1) get pointer to virtual table
            IType requiredInterface;
            var effectiveType = requiredType ?? thisType;
            effectiveType.GetVirtualMethodIndexAndRequiredInterface(methodInfo, cWriter, out requiredInterface);

            writer.Write("(*(");

            if (requiredInterface != null)
            {
                writer.Write("({0}*)", thisType.GetVirtualTableName());
                cWriter.WriteFieldAccess(writer, opCodeMethodInfo, requiredInterface.GetFieldByName("vtable", cWriter));
            }
            else
            {
                writer.Write("({0}*)", thisType.GetVirtualTableName());
                cWriter.WriteFieldAccess(writer, opCodeMethodInfo, cWriter.System.System_Object.GetFieldByName("vtable", cWriter));
            }

            writer.Write(")->");
            writer.Write(methodInfo.ToString(null, true).CleanUpName());

            //if (thisType.IsInterface)
            //{
            //    cWriter.WriteGetThisPointerFromInterfacePointer(
            //        writer,
            //        opCodeMethodInfo,
            //        methodInfo,
            //        thisType,
            //        null);

            //    var thisPointerResultNumber = opCodeMethodInfo.Result;

            //    // set ot for Call op code
            //    opCodeMethodInfo.OpCodeOperands[0].Result = thisPointerResultNumber;
            //}

            writer.Write(")");
        }

        public static IType GetIntTypeByBitSize(this BaseWriter llvmWriter, int bitSize)
        {
            IType toType = null;
            switch (bitSize)
            {
                case 1:
                    toType = llvmWriter.System.System_Boolean;
                    break;
                case 8:
                    toType = llvmWriter.System.System_SByte;
                    break;
                case 16:
                    toType = llvmWriter.System.System_Int16;
                    break;
                case 32:
                    toType = llvmWriter.System.System_Int32;
                    break;
                case 64:
                    toType = llvmWriter.System.System_Int64;
                    break;
            }

            return toType;
        }

        /// <summary>
        /// </summary>
        /// <param name="typeResolver">
        /// </param>
        /// <param name="byteSize">
        /// </param>
        /// <returns>
        /// </returns>
        public static IType GetIntTypeByByteSize(this ITypeResolver typeResolver, int byteSize)
        {
            IType toType = null;
            switch (byteSize)
            {
                case 1:
                    toType = typeResolver.System.System_SByte;
                    break;
                case 2:
                    toType = typeResolver.System.System_Int16;
                    break;
                case 4:
                    toType = typeResolver.System.System_Int32;
                    break;
                case 8:
                    toType = typeResolver.System.System_Int64;
                    break;
            }

            return toType;
        }

        public static IType GetUIntTypeByBitSize(this ITypeResolver llvmWriter, int bitSize)
        {
            IType toType = null;
            switch (bitSize)
            {
                case 1:
                    toType = llvmWriter.System.System_Boolean;
                    break;
                case 8:
                    toType = llvmWriter.System.System_Byte;
                    break;
                case 16:
                    toType = llvmWriter.System.System_UInt16;
                    break;
                case 32:
                    toType = llvmWriter.System.System_UInt32;
                    break;
                case 64:
                    toType = llvmWriter.System.System_UInt64;
                    break;
            }

            return toType;
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="byteSize">
        /// </param>
        /// <returns>
        /// </returns>
        public static IType GetUIntTypeByByteSize(this ITypeResolver llvmWriter, int byteSize)
        {
            IType toType = null;
            switch (byteSize)
            {
                case 1:
                    toType = llvmWriter.System.System_Byte;
                    break;
                case 2:
                    toType = llvmWriter.System.System_UInt16;
                    break;
                case 4:
                    toType = llvmWriter.System.System_UInt32;
                    break;
                case 8:
                    toType = llvmWriter.System.System_UInt64;
                    break;
            }

            return toType;
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="toType">
        /// </param>
        public static void WriteCCast(this CWriter cWriter, OpCodePart opCode, IType toType)
        {
            var writer = cWriter.Output;

            writer.Write("(");
            toType.WriteTypePrefix(cWriter);
            writer.Write(") ");

            writer.Write("(");
            cWriter.WriteResultOrActualWrite(writer, opCode);
            writer.Write(")");

            cWriter.SetResultNumber(opCode, toType);
        }

        public static void WriteCCastOperand(this CWriter cWriter, OpCodePart opCode, int operand, IType toType)
        {
            var writer = cWriter.Output;

            writer.Write("(");
            toType.WriteTypePrefix(cWriter);
            writer.Write(") ");

            writer.Write("(");
            cWriter.WriteOperandResultOrActualWrite(writer, opCode, operand);
            writer.Write(")");

            cWriter.SetResultNumber(opCode, toType);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="methodInfo">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool ProcessPluggableMethodCall(
            this CWriter cWriter,
            OpCodePart opCodeMethodInfo,
            IMethod methodInfo)
        {
            if (methodInfo.HasProceduralBody)
            {
                var customAction = methodInfo as IMethodBodyCustomAction;
                if (customAction != null)
                {
                    customAction.Execute(cWriter, opCodeMethodInfo);
                }

                return true;
            }

            // TODO: it seems, you can preprocess MSIL code and replace all functions with MSIL code blocks to stop writing the code manually.
            // for example call System.Activator.CreateInstance<X>() can be replace with "Code.NewObj x"
            // the same interlocked functions and the same for TypeOf operators
            if (methodInfo.IsTypeOfCallFunction() && opCodeMethodInfo.WriteTypeOfFunction(cWriter))
            {
                return true;
            }

            if (methodInfo.IsInterlockedFunction())
            {
                methodInfo.WriteInterlockedFunction(opCodeMethodInfo, cWriter);
                return true;
            }

            if (methodInfo.IsThreadingFunction())
            {
                methodInfo.WriteThreadingFunction(opCodeMethodInfo, cWriter);
                return true;
            }

            if (methodInfo.IsActivatorFunction())
            {
                methodInfo.WriteActivatorFunction(opCodeMethodInfo, cWriter);
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="label">
        /// </param>
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
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="source">
        /// </param>
        /// <param name="toType">
        /// </param>
        public static void WriteCCast(
            this CWriter cWriter,
            OpCodePart opCode,
            FullyDefinedReference source,
            IType toType,
            bool asReference = true)
        {
            cWriter.WriteStartCCast(opCode, toType, asReference);
            cWriter.WriteResult(source);
            cWriter.WriteEndCCast(opCode, toType);
        }

        public static void WriteEndCCast(this CWriter cWriter, OpCodePart opCode, IType toType)
        {
            cWriter.Output.Write(")");
            cWriter.SetResultNumber(opCode, toType);
        }

        public static void WriteStartCCast(this CWriter cWriter, OpCodePart opCode, IType toType, bool asReference = false)
        {
            var writer = cWriter.Output;

            writer.Write("(");
            toType.WriteTypePrefix(cWriter, asReference);
            writer.Write(") (");
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="result">
        /// </param>
        public static void WriteBitcast(this CWriter cWriter, OpCodePart opCode, FullyDefinedReference result)
        {
            var writer = cWriter.Output;

            cWriter.SetResultNumber(opCode, cWriter.System.System_Byte.ToPointerType());
            writer.Write("bitcast ");
            result.Type.WriteTypePrefix(cWriter, !result.Type.IsByRef && result.Type.IsValueType());
            writer.Write(" ");
            cWriter.WriteResult(result);
            writer.Write(" to i8*");
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="fromType">
        /// </param>
        /// <param name="result">
        /// </param>
        /// <param name="custom">
        /// </param>
        public static void WriteBitcast(
            this CWriter cWriter,
            OpCodePart opCode,
            IType fromType,
            IncrementalResult result,
            string custom)
        {
            var writer = cWriter.Output;

            cWriter.SetResultNumber(opCode, null);
            writer.Write("bitcast ");
            fromType.WriteTypePrefix(cWriter, true);
            writer.Write(' ');
            cWriter.WriteResult(result);
            writer.Write(" to ");
            writer.Write(custom);

            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="methodInfo">
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
            this CWriter cWriter,
            OpCodePart opCodeMethodInfo,
            IMethod methodInfo,
            bool isVirtual,
            bool hasThis,
            bool isCtor,
            FullyDefinedReference thisResultNumber,
            TryClause tryClause)
        {
            var writer = cWriter.Output;

            IType thisType;
            bool hasThisArgument;
            OpCodePart opCodeFirstOperand;
            BaseWriter.ReturnResult resultOfFirstOperand;
            bool isIndirectMethodCall;
            IType ownerOfExplicitInterface;
            IType requiredType;
            methodInfo.WriteFunctionCallProlog(
                opCodeMethodInfo,
                isVirtual,
                hasThis,
                cWriter,
                out thisType,
                out hasThisArgument,
                out opCodeFirstOperand,
                out resultOfFirstOperand,
                out isIndirectMethodCall,
                out ownerOfExplicitInterface,
                out requiredType);

            FullyDefinedReference methodAddressResultNumber = null;
            if (isIndirectMethodCall)
            {
                cWriter.GenerateVirtualCall(
                    opCodeMethodInfo,
                    methodInfo,
                    thisType,
                    opCodeFirstOperand,
                    resultOfFirstOperand,
                    ref requiredType);
            }

            methodInfo.WriteFunctionCallLoadFunctionAddress(
                opCodeMethodInfo,
                thisType,
                ref methodAddressResultNumber,
                cWriter);

            if (cWriter.ProcessPluggableMethodCall(opCodeMethodInfo, methodInfo))
            {
                return;
            }

            var returnFullyDefinedReference = methodInfo.WriteFunctionCallResult(opCodeMethodInfo, cWriter);

            if (methodInfo.CallingConvention.HasFlag(CallingConventions.VarArgs))
            {
                cWriter.WriteMethodPointerType(writer, methodInfo);
                writer.Write(" ");
            }
            else
            {
                if (methodInfo.IsExternalLibraryMethod())
                {
                    writer.Write("(...)* ");
                }
            }

            if (!isIndirectMethodCall)
            {
                methodInfo.WriteFunctionNameExpression(methodAddressResultNumber, ownerOfExplicitInterface, cWriter);
            }

            methodInfo.GetParameters()
                .WriteFunctionCallArguments(
                    opCodeMethodInfo,
                    resultOfFirstOperand,
                    isVirtual,
                    hasThis,
                    isCtor,
                    thisResultNumber,
                    thisType,
                    returnFullyDefinedReference,
                    methodInfo != null ? methodInfo.ReturnType : null,
                    cWriter,
                    methodInfo.CallingConvention.HasFlag(CallingConventions.VarArgs));

            tryClause.WriteFunctionCallUnwind(opCodeMethodInfo, cWriter);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="fromResult">
        /// </param>
        /// <param name="toType">
        /// </param>
        /// <param name="throwExceptionIfNull">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool WriteCast(
            this CWriter cWriter,
            OpCodePart opCode,
            OpCodePart opCodeOperand,
            IType toType,
            bool throwExceptionIfNull = false)
        {
            var writer = cWriter.Output;

            var estimatedOperandResultOf = cWriter.EstimatedResultOf(opCodeOperand);

            var bareType = !estimatedOperandResultOf.Type.IsArray
                ? estimatedOperandResultOf.Type.ToBareType()
                : estimatedOperandResultOf.Type;
            if (toType.IsInterface && !(estimatedOperandResultOf is ConstValue))
            {
                if (bareType.GetAllInterfaces().Contains(toType))
                {
                    cWriter.ActualWrite(writer, opCodeOperand);

                    opCode.Result = opCodeOperand.Result;
                    ////cWriter.WriteInterfaceAccess(writer, opCode, bareType, toType);
                }
                else
                {
                    cWriter.WriteDynamicCast(writer, opCode, opCodeOperand, toType, true, throwExceptionIfNull);
                }
            }
            else if (estimatedOperandResultOf.Type.IntTypeBitSize() == CWriter.PointerSize * 8 &&
                     (toType.IsPointer || toType.IsByRef))
            {
                WriteCCastOperand(cWriter, opCode, 0, toType);
                cWriter.SetResultNumber(opCode, toType);
            }
            else if (estimatedOperandResultOf.Type.IsArray
                     || (estimatedOperandResultOf.Type.IsPointer && bareType.TypeEquals(cWriter.System.System_Void))
                     || toType.IsArray 
                     || toType.IsPointer 
                     || toType.IsByRef 
                     || bareType.IsDerivedFrom(toType) 
                     || (estimatedOperandResultOf is ConstValue))
            {
                WriteCCastOperand(cWriter, opCode, 0, toType);
                cWriter.SetResultNumber(opCode, toType);
            }
            else
            {
                Debug.Assert(estimatedOperandResultOf.Type.IntTypeBitSize() == 0);
                cWriter.WriteDynamicCast(writer, opCode, opCodeOperand, toType, true, throwExceptionIfNull);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
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
            this CWriter cWriter,
            OpCodePart opCode,
            IType fromType,
            string custromName,
            IType toType,
            bool appendReference = false,
            bool doNotConvert = false)
        {
            // TODO: remove this one. use anather one
            var writer = cWriter.Output;

            if (!fromType.IsInterface && toType.IsInterface)
            {
                throw new NotImplementedException();

                ////opCode.Result = res;
                ////this.WriteInterfaceAccess(writer, opCode, fromType, toType);
            }
            cWriter.SetResultNumber(opCode, toType);
            writer.Write("bitcast ");
            fromType.WriteTypePrefix(cWriter, true);
            writer.Write(' ');
            writer.Write(custromName);
            writer.Write(" to ");
            toType.WriteTypePrefix(cWriter, true);
            if (appendReference)
            {
                // result should be array
                writer.Write('*');
            }

            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="source">
        /// </param>
        /// <param name="toType">
        /// </param>
        public static void WriteIntToPtr(
            this CWriter cWriter,
            OpCodePart opCode,
            FullyDefinedReference source,
            IType toType)
        {
            var writer = cWriter.Output;

            Debug.Assert(!source.Type.IsPointer && !source.Type.IsByRef);

            cWriter.SetResultNumber(opCode, toType);
            writer.Write("inttoptr ");
            source.Type.WriteTypePrefix(cWriter);
            writer.Write(" ");
            cWriter.WriteResult(source);
            writer.Write(" to ");
            toType.WriteTypePrefix(cWriter, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
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
            this CWriter cWriter,
            OpCodePart opCode,
            IType typeToLoad,
            IncrementalResult source,
            bool appendReference = true,
            bool structAsRef = false)
        {
            cWriter.WriteLlvmLoad(
                opCode,
                typeToLoad,
                new FullyDefinedReference(source.ToString(), source.Type),
                appendReference,
                structAsRef);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
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
        /// <param name="indirect">
        /// </param>
        public static void WriteLlvmLoad(
            this CWriter cWriter,
            OpCodePart opCode,
            IType typeToLoad,
            FullyDefinedReference source,
            bool appendReference = true,
            bool structAsRef = false,
            bool indirect = false)
        {
            // TODO: review the whole proc.
            var writer = cWriter.Output;

            ////Debug.Assert(source.Type.IsPointer);
            var dereferencedType = source.Type.IsPointer ? source.Type.GetElementType() : null;

            var effectiveSource = source;

            // check if you need bitcast pointer type
            if (!typeToLoad.IsPointer && dereferencedType != null && typeToLoad.TypeNotEquals(dereferencedType))
            {
                // check if you need cast here
                cWriter.WriteCCast(opCode, source, typeToLoad);
                writer.WriteLine(string.Empty);
                effectiveSource = opCode.Result;
            }

            if (indirect && !source.Type.IsPointer && !source.Type.IsByRef && source.Type.IntTypeBitSize() > 0)
            {
                // check if you need cast here
                cWriter.WriteIntToPtr(opCode, source, typeToLoad);
                writer.WriteLine(string.Empty);
                effectiveSource = opCode.Result;
            }

            opCode.Result = effectiveSource;
        }

        public static void WriteLlvmLoadPrimitiveFromStructure(
            this CWriter cWriter,
            OpCodePart opCode,
            FullyDefinedReference source)
        {
            var writer = cWriter.Output;

            // write access to a field
            if (!cWriter.WriteFieldAccess(opCode, source.Type, source.Type, 0, source))
            {
                writer.WriteLine("; No data");
                return;
            }

            writer.WriteLine(string.Empty);

            cWriter.WriteLlvmLoad(opCode, opCode.Result.Type, opCode.Result);

            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <param name="asReference">
        /// </param>
        public static void WriteLlvmLocalVarAccess(this CWriter cWriter, int index, bool asReference = false)
        {
            var writer = cWriter.Output;

            var localType = cWriter.LocalInfo[index].LocalType;

            localType.WriteTypePrefix(cWriter, false);
            if (asReference)
            {
                writer.Write('*');
            }

            writer.Write(' ');
            writer.Write(cWriter.GetLocalVarName(index));

            // TODO: optional do we need to calculate it propertly?
            writer.Write(", align " + CWriter.PointerSize);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="typeToSave">
        /// </param>
        /// <param name="operandIndex">
        /// </param>
        /// <param name="destination">
        /// </param>
        public static void WriteLlvmSave(
            this CWriter cWriter,
            OpCodePart opCode,
            IType typeToSave,
            int operandIndex,
            FullyDefinedReference destination,
            bool destinationIsIndirect = false)
        {
            var writer = cWriter.Output;

            if (destinationIsIndirect)
            {
                writer.Write("*(");
            }
            
            cWriter.WriteResult(destination);
            if (destinationIsIndirect)
            {
                writer.Write(")");
            }

            writer.Write(" = ");

            var estimatedResultOf = cWriter.EstimatedResultOf(opCode.OpCodeOperands[operandIndex]);
            if (typeToSave.TypeNotEquals(estimatedResultOf.Type))
            {
                writer.Write("(");
                typeToSave.WriteTypePrefix(cWriter);
                writer.Write(") ");
            }

            cWriter.WriteOperandResultOrActualWrite(writer, opCode, operandIndex);
        }

        public static void WriteLlvmSavePrimitiveIntoStructure(
            this CWriter cWriter,
            OpCodePart opCode,
            FullyDefinedReference source,
            FullyDefinedReference destination)
        {
            var writer = cWriter.Output;

            writer.WriteLine("; Copy primitive data into a structure");

            // write access to a field
            if (!cWriter.WriteFieldAccess(
                opCode,
                destination.Type.ToClass(),
                destination.Type.ToClass(),
                0,
                destination))
            {
                writer.WriteLine("; No data");
                return;
            }

            writer.WriteLine(string.Empty);

            cWriter.SaveToField(opCode, opCode.Result.Type, 0);

            writer.WriteLine(string.Empty);
            writer.WriteLine("; End of Copy primitive data");
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="op1">
        /// </param>
        /// <param name="op2">
        /// </param>
        public static void WriteMemCopy(
            this CWriter cWriter,
            IType type,
            FullyDefinedReference op1,
            FullyDefinedReference op2)
        {
            var writer = cWriter.Output;

            writer.WriteLine(
                "call void @llvm.memcpy.p0i8.p0i8.i32(i8* {0}, i8* {1}, i32 {2}, i32 {3}, i1 false)",
                op1,
                op2,
                type.GetTypeSize(cWriter),
                CWriter.PointerSize

                /*Align*/);
        }

        public static void WriteMemCopy(
            this CWriter cWriter,
            FullyDefinedReference op1,
            FullyDefinedReference op2,
            FullyDefinedReference size)
        {
            var writer = cWriter.Output;

            writer.WriteLine(
                "call void @llvm.memcpy.p0i8.p0i8.i32(i8* {0}, i8* {1}, i32 {2}, i32 {3}, i1 false)",
                op1,
                op2,
                size,
                CWriter.PointerSize
                /*Align*/);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="op1">
        /// </param>
        public static void WriteMemSet(this CWriter cWriter, IType type, FullyDefinedReference op1)
        {
            var writer = cWriter.Output;

            writer.Write("memset((i8*) ({0}), 0, sizeof(", op1);
            type.WriteTypePrefix(cWriter);
            writer.Write("));");
        }

        public static void WriteMemSet(this CWriter cWriter, FullyDefinedReference op1, FullyDefinedReference size)
        {
            var writer = cWriter.Output;

            writer.Write("memset((i8*) ({0}), 0, ({1}))", op1, size);
        }

        public static void WriteMemSet(
            this CWriter cWriter,
            OpCodePart reference,
            OpCodePart init,
            OpCodePart size)
        {
            var writer = cWriter.Output;
            writer.Write("memset((i8*) (");
            cWriter.WriteResultOrActualWrite(writer, reference);
            writer.Write("), ");
            cWriter.WriteResultOrActualWrite(writer, init);
            writer.Write(", (");
            cWriter.WriteResultOrActualWrite(writer, size);
            writer.Write("))");
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="source">
        /// </param>
        /// <param name="toType">
        /// </param>
        public static void WritePtrToInt(
            this CWriter cWriter,
            OpCodePart opCode,
            FullyDefinedReference source,
            IType toType)
        {
            var writer = cWriter.Output;

            Debug.Assert(!toType.IsPointer);

            cWriter.SetResultNumber(opCode, toType);
            writer.Write("ptrtoint ");
            source.Type.WriteTypePrefix(cWriter, true);
            writer.Write(" ");
            cWriter.WriteResult(source);
            writer.Write(" to ");
            toType.WriteTypePrefix(cWriter);
        }
    }
}