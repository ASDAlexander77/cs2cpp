// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionHandlingGen.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Il2Native.Logic.CodeParts;
    using Il2Native.Logic.Exceptions;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    [Flags]
    public enum LandingPadOptions
    {
        /// <summary>
        /// </summary>
        None = 0, 

        /// <summary>
        /// </summary>
        Cleanup = 1, 

        /// <summary>
        /// </summary>
        EmptyFilter = 2
    }

    /// <summary>
    /// </summary>
    public static class ExceptionHandlingGen
    {
        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        public static LlvmResult WriteAllocateException(this LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine("; Allocate exception");
            var errorAllocationResultNumber = llvmWriter.WriteSetResultNumber(opCode, llvmWriter.ResolveType("System.Byte").ToPointerType());
            writer.Write("call i8* @__cxa_allocate_exception(i32 {0})", LlvmWriter.PointerSize);
            writer.WriteLine(string.Empty);

            var newExceptionResult = opCode.OpCodeOperands[0].Result;

            opCode.OpCodeOperands[0].Result = opCode.Result;

            llvmWriter.WriteBitcast(opCode, opCode.Result, newExceptionResult.Type);
            writer.WriteLine("*");

            opCode.OpCodeOperands[0].Result = newExceptionResult;

            llvmWriter.UnaryOper(writer, opCode, "store");
            writer.Write(", ");
            opCode.OpCodeOperands[0].Result.Type.WriteTypePrefix(writer);
            writer.Write("* ");
            llvmWriter.WriteResultNumber(opCode.Result);
            writer.WriteLine(string.Empty);

            return errorAllocationResultNumber;
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="exceptionHandlingClause">
        /// </param>
        public static void WriteCatchBegin(this LlvmWriter llvmWriter, CatchOfFinallyClause exceptionHandlingClause)
        {
            var writer = llvmWriter.Output;

            var isFinally = exceptionHandlingClause.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally);
            if (isFinally)
            {
                writer.WriteLine("; Begin of Finally");
            }
            else
            {
                writer.WriteLine("; Begin of Catch");
            }

            var catchType = exceptionHandlingClause.Catch;

            var opCodeNone = OpCodePart.CreateNop;
            var bytePointerType = llvmWriter.ResolveType("System.Byte").ToPointerType();
            var errorObjectOfCatchResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, bytePointerType);
            writer.WriteLine("load i8** %.error_object");
            var beginCatchResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, bytePointerType);
            writer.WriteLine("call i8* @__cxa_begin_catch(i8* {0})", errorObjectOfCatchResultNumber);
            if (catchType != null)
            {
                llvmWriter.WriteBitcast(opCodeNone, beginCatchResultNumber, catchType);
                writer.WriteLine(string.Empty);
            }

            if (isFinally)
            {
                // set default error handler jump to carry on try/catch execution
                writer.WriteLine("store i32 0, i32* %.finally_jump{0}", exceptionHandlingClause.Offset);
                writer.WriteLine("br label %.finally_no_error_entry{0}", exceptionHandlingClause.Offset);
                writer.Indent--;
                writer.WriteLine(".finally_no_error_entry{0}:", exceptionHandlingClause.Offset);
                writer.Indent++;
            }

            if (isFinally)
            {
                writer.WriteLine("; Begin of Finally Handler Body");
            }
            else
            {
                writer.WriteLine("; Begin of Catch Handler Body");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="exceptionHandlingClause">
        /// </param>
        /// <param name="upperLevelExceptionHandlingClause">
        /// </param>
        public static void WriteCatchEnd(
            this LlvmWriter llvmWriter, 
            OpCodePart opCode, 
            CatchOfFinallyClause exceptionHandlingClause,
            CatchOfFinallyClause upperLevelExceptionHandlingClause)
        {
            var writer = llvmWriter.Output;

            var isFinally = exceptionHandlingClause.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally);
            if (isFinally)
            {
                writer.WriteLine("; End of Finally Handler Body");
            }
            else
            {
                writer.WriteLine("; End of Catch Handler Body");
            }

            if (isFinally)
            {
                // process Leave jumps
                var index = 0;
                var opCodeNope = OpCodePart.CreateNop;

                var fullyDefinedRef = new FullyDefinedReference(
                    string.Concat("%.finally_jump", exceptionHandlingClause.Offset), llvmWriter.ResolveType("System.Int32"));

                llvmWriter.WriteLlvmLoad(opCodeNope, fullyDefinedRef);
                writer.WriteLine(string.Empty);
                writer.WriteLine(
                    "switch i32 {1}, label %.finally_exit{0} [", exceptionHandlingClause.Offset, opCodeNope.Result);
                writer.Indent++;
                writer.WriteLine("i32 {0}, label %.finally_exit{1}", index++, exceptionHandlingClause.Offset);
                foreach (var leave in exceptionHandlingClause.FinallyJumps)
                {
                    writer.WriteLine("i32 {0}, label %{1}", index++, leave);
                }

                writer.Indent--;
                writer.WriteLine("]");

                writer.Indent--;
                writer.WriteLine(".finally_exit{0}:", exceptionHandlingClause.Offset);
                writer.Indent++;
            }

            var endOfHandlerAddress = exceptionHandlingClause.Offset + exceptionHandlingClause.Length;

            if (exceptionHandlingClause.RethrowCatchWithCleanUpRequired)
            {
                writer.Indent--;
                writer.WriteLine(".catch_with_cleanup{0}:", endOfHandlerAddress);
                writer.Indent++;

                var opCodeNop = OpCodePart.CreateNop;
                llvmWriter.WriteLandingPad(opCodeNop, LandingPadOptions.Cleanup, new[] { upperLevelExceptionHandlingClause.Catch });
                writer.WriteLine(string.Empty);
            }
            else
            {
                writer.WriteLine("store i32 0, i32* %.error_typeid");
            }

            writer.WriteLine("call void @__cxa_end_catch()");

            if (!exceptionHandlingClause.RethrowCatchWithCleanUpRequired || upperLevelExceptionHandlingClause == null)
            {
                var nextOp = opCode.NextOpCode(llvmWriter);
                if (nextOp.JumpDestination == null || !nextOp.JumpDestination.Any() || nextOp.GroupAddressStart != endOfHandlerAddress)
                {
                    writer.WriteLine("br label %.exit{0}", endOfHandlerAddress);

                    writer.Indent--;
                    writer.Write(string.Concat(".exit", endOfHandlerAddress, ':'));
                    writer.Indent++;
                    writer.WriteLine(string.Empty);
                }
                else
                {
                    writer.WriteLine("br label %.a{0}", nextOp.GroupAddressStart);
                }
            }
            else
            {
                writer.WriteLine("br label %.exception_switch{0}", upperLevelExceptionHandlingClause.Offset);
            }

            if (isFinally)
            {
                writer.WriteLine("; End of Finally");
            }
            else
            {
                writer.WriteLine("; End of Catch");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteCatchProlog(this LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine("; Cacth Clauses - Prolog");

            var handlerOffset = opCode.ExceptionHandlers.First().Offset;
            writer.Indent--;
            writer.WriteLine(".catch{0}:", handlerOffset);
            writer.Indent++;

            llvmWriter.WriteLandingPad(
                opCode, 
                opCode.ExceptionHandlers.Any(eh => eh.Flags == ExceptionHandlingClauseOptions.Finally) ? LandingPadOptions.Cleanup : LandingPadOptions.None, 
                opCode.ExceptionHandlers.Where(eh => eh.Flags == ExceptionHandlingClauseOptions.Clause).Select(eh => eh.Catch).ToArray());

            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="exceptionHandlingClause">
        /// </param>
        /// <param name="nextExceptionHandlingClause">
        /// </param>
        public static void WriteCatchTest(
            this LlvmWriter llvmWriter, CatchOfFinallyClause exceptionHandlingClause, CatchOfFinallyClause nextExceptionHandlingClause)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine("br label %.exception_switch{0}", exceptionHandlingClause.Offset);

            writer.Indent--;
            writer.WriteLine(".exception_switch{0}:", exceptionHandlingClause.Offset);
            writer.Indent++;

            writer.WriteLine("; Test Exception type");

            var catchType = exceptionHandlingClause.Catch;

            var opCodeNone = OpCodePart.CreateNop;
            var errorTypeIdOfCatchResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, llvmWriter.ResolveType("System.Int32"));
            writer.WriteLine("load i32* %.error_typeid");
            var errorTypeIdOfExceptionResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, llvmWriter.ResolveType("System.Byte").ToPointerType());
            writer.Write("call i32 @llvm.eh.typeid.for(i8* bitcast (");
            catchType.WriteRttiPointerClassInfoDeclaration(writer);
            writer.WriteLine("* @\"{0}\" to i8*))", catchType.GetRttiPointerInfoName());
            var compareResultResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, llvmWriter.ResolveType("System.Boolean"));
            writer.WriteLine(
                "icmp eq i32 {0}, {1}",
                errorTypeIdOfCatchResultNumber,
                errorTypeIdOfExceptionResultNumber);
            writer.WriteLine(
                "br i1 {0}, label %.exception_handler{1}, label %.{2}",
                compareResultResultNumber, 
                exceptionHandlingClause.Offset,
                nextExceptionHandlingClause != null ? string.Concat("exception_switch", nextExceptionHandlingClause.Offset) : "resume");

            writer.Indent--;
            writer.WriteLine(".exception_handler{0}:", exceptionHandlingClause.Offset);
            writer.Indent++;
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="finallyClause">
        /// </param>
        public static void WriteFinallyLeave(this LlvmWriter llvmWriter, CatchOfFinallyClause finallyClause)
        {
            var writer = llvmWriter.Output;

            llvmWriter.WriteFinallyVariables(finallyClause);

            writer.WriteLine("store i32 {0}, i32* %.finally_jump{1}", finallyClause.FinallyJumps.Count, finallyClause.Offset);
            writer.WriteLine("br label %.finally_no_error_entry{0}", finallyClause.Offset);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="finallyClause">
        /// </param>
        public static void WriteFinallyVariables(this LlvmWriter llvmWriter, CatchOfFinallyClause finallyClause)
        {
            if (finallyClause.FinallyVariablesAreWritten)
            {
                return;
            }

            finallyClause.FinallyVariablesAreWritten = true;

            var writer = llvmWriter.Output;

            writer.Write("%.finally_jump{0} = ", finallyClause.Offset);
            writer.Write("alloca i32, align " + LlvmWriter.PointerSize);
            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="options">
        /// </param>
        /// <param name="catch">
        /// </param>
        /// <param name="filter">
        /// </param>
        /// <param name="exceptionAllocationResultNumber">
        /// </param>
        public static void WriteLandingPad(
            this LlvmWriter llvmWriter, 
            OpCodePart opCode, 
            LandingPadOptions options, 
            IType[] @catch = null, 
            int[] filter = null, 
            int? exceptionAllocationResultNumber = null)
        {
            var writer = llvmWriter.Output;

            llvmWriter.WriteLandingPadVariables();

            var landingPadResult = llvmWriter.WriteSetResultNumber(opCode, llvmWriter.ResolveType("System.Byte").ToPointerType());

            writer.WriteLine("landingpad { i8*, i32 } personality i8* bitcast (i32 (...)* @__gxx_personality_v0 to i8*)");
            if (options.HasFlag(LandingPadOptions.Cleanup))
            {
                writer.Indent++;
                writer.WriteLine("cleanup");
                writer.Indent--;
            }

            if (options.HasFlag(LandingPadOptions.EmptyFilter))
            {
                writer.Indent++;
                writer.WriteLine("filter [0 x i8*] zeroinitializer");
                writer.Indent--;
            }

            if (@catch != null)
            {
                foreach (var catchType in @catch)
                {
                    writer.Indent++;
                    writer.Write("catch i8* bitcast (");
                    catchType.WriteRttiPointerClassInfoDeclaration(writer);
                    writer.WriteLine("* @\"{0}\" to i8*)", catchType.GetRttiPointerInfoName());
                    writer.Indent--;

                    llvmWriter.typeRttiPointerDeclRequired.Add(catchType);
                    llvmWriter.CheckIfExternalDeclarationIsRequired(catchType);
                }
            }

            var getErrorObjectResultNumber = llvmWriter.WriteSetResultNumber(opCode, llvmWriter.ResolveType("System.Byte").ToPointerType());
            writer.WriteLine("extractvalue {1} {0}, 0", landingPadResult, "{ i8*, i32 }");
            writer.WriteLine("store i8* {0}, i8** %.error_object", getErrorObjectResultNumber);
            var getErrorTypeIdResultNumber = llvmWriter.WriteSetResultNumber(opCode, llvmWriter.ResolveType("System.Int32"));
            writer.WriteLine("extractvalue {1} {0}, 1", landingPadResult, "{ i8*, i32 }");
            writer.Write("store i32 {0}, i32* %.error_typeid", getErrorTypeIdResultNumber);

            if (exceptionAllocationResultNumber.HasValue)
            {
                writer.WriteLine(string.Empty);
                writer.Write("call void @__cxa_free_exception(i8* {0})", exceptionAllocationResultNumber.Value);
            }

            opCode.Result = landingPadResult;
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteLandingPadVariables(this LlvmWriter llvmWriter)
        {
            if (llvmWriter.landingPadVariablesAreWritten)
            {
                return;
            }

            llvmWriter.landingPadVariablesAreWritten = true;

            var writer = llvmWriter.Output;

            writer.Write("%.error_object = ");
            writer.Write("alloca i8*, align " + LlvmWriter.PointerSize);
            writer.WriteLine(string.Empty);

            writer.Write("%.error_typeid = ");
            writer.Write("alloca i32, align " + LlvmWriter.PointerSize);
            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteResume(this LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            writer.Indent--;
            writer.WriteLine(".resume:");
            writer.Indent++;

            writer.WriteLine("; Resume");

            var opCodeNone = OpCodePart.CreateNop;
            var getErrorObjectResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, llvmWriter.ResolveType("System.Byte").ToPointerType());
            writer.WriteLine("load i8** %.error_object");
            var getErrorTypeIdResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, llvmWriter.ResolveType("System.Int32"));
            writer.WriteLine("load i32* %.error_typeid");
            var insertedErrorObjectResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, llvmWriter.ResolveType("System.Byte").ToPointerType());
            writer.WriteLine("insertvalue {1} undef, i8* {0}, 0", getErrorObjectResultNumber, "{ i8*, i32 }");
            var insertedErrorTypeIdResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, llvmWriter.ResolveType("System.Byte").ToPointerType());
            writer.WriteLine(
                "insertvalue {2} {0}, i32 {1}, 1",
                insertedErrorObjectResultNumber,
                getErrorTypeIdResultNumber, 
                "{ i8*, i32 }");
            writer.WriteLine("resume {1} {0}", insertedErrorTypeIdResultNumber, "{ i8*, i32 }");
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="exceptionHandlingClause">
        /// </param>
        /// <param name="upperLevelExceptionHandlingClause">
        /// </param>
        public static void WriteRethrow(
            this LlvmWriter llvmWriter, 
            OpCodePart opCode,
            CatchOfFinallyClause exceptionHandlingClause,
            CatchOfFinallyClause upperLevelExceptionHandlingClause)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine("; Rethrow");
            WriteRethrowInvoke(llvmWriter, exceptionHandlingClause);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="exceptionHandlingClause">
        /// </param>
        public static void WriteThrow(this LlvmWriter llvmWriter, OpCodePart opCode, CatchOfFinallyClause exceptionHandlingClause)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine("; Throw");

            var exceptionPointerType = exceptionHandlingClause != null
                                           ? WriteThrowInvoke(llvmWriter, opCode, exceptionHandlingClause)
                                           : WriteThrowCall(llvmWriter, opCode);

            llvmWriter.typeRttiPointerDeclRequired.Add(exceptionPointerType);
            llvmWriter.CheckIfExternalDeclarationIsRequired(exceptionPointerType);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteUnexpectedCall(this LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            writer.Indent--;
            writer.WriteLine(".unexpected:");
            writer.Indent++;
            var result = llvmWriter.WriteSetResultNumber(null, llvmWriter.ResolveType("System.Byte").ToPointerType());
            writer.WriteLine("load i8** %.error_object");
            writer.WriteLine("call void @__cxa_call_unexpected(i8* {0})", result);
            writer.WriteLine("unreachable");
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteUnreachable(this LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            writer.Indent--;
            writer.WriteLine(".unreachable:");
            writer.Indent++;
            writer.WriteLine("unreachable");
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteUnwindException(this LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            writer.Indent--;
            writer.WriteLine(".unwind_exception:");
            writer.Indent++;
            llvmWriter.WriteLandingPad(OpCodePart.CreateNop, LandingPadOptions.EmptyFilter);
            writer.WriteLine(string.Empty);
            writer.WriteLine("br label %.unexpected");
            llvmWriter.WriteUnexpectedCall();
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="exceptionHandlingClause">
        /// </param>
        private static void WriteRethrowInvoke(LlvmWriter llvmWriter, CatchOfFinallyClause exceptionHandlingClause)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine("invoke void @__cxa_rethrow()");
            if (exceptionHandlingClause != null)
            {
                writer.Indent++;
                writer.WriteLine(
                    "to label %.unreachable unwind label %.catch_with_cleanup{0}", exceptionHandlingClause.Offset + exceptionHandlingClause.Length);
                writer.Indent--;
                exceptionHandlingClause.RethrowCatchWithCleanUpRequired = true;
            }
            else
            {
                writer.Indent++;
                writer.WriteLine("to label %.unreachable unwind label %.unwind_exception");
                writer.Indent--;
                llvmWriter.needToWriteUnwindException = true;
            }

            llvmWriter.needToWriteUnreachable = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        private static IType WriteThrowCall(LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var writer = llvmWriter.Output;

            var errorAllocationResultNumber = llvmWriter.WriteAllocateException(opCode);

            var exceptionPointerType = opCode.OpCodeOperands[0].Result.Type;
            writer.Write("call void @__cxa_throw(i8* {0}, i8* bitcast (", errorAllocationResultNumber);
            exceptionPointerType.WriteRttiPointerClassInfoDeclaration(writer);
            writer.WriteLine("* @\"{0}\" to i8*), i8* null)", exceptionPointerType.GetRttiPointerInfoName());
            writer.WriteLine("unreachable");
            return exceptionPointerType;
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="exceptionHandlingClause">
        /// </param>
        /// <returns>
        /// </returns>
        private static IType WriteThrowInvoke(LlvmWriter llvmWriter, OpCodePart opCode, CatchOfFinallyClause exceptionHandlingClause)
        {
            var writer = llvmWriter.Output;

            var errorAllocationResultNumber = llvmWriter.WriteAllocateException(opCode);

            var exceptionPointerType = opCode.OpCodeOperands[0].Result.Type;
            writer.Write("invoke void @__cxa_throw(i8* {0}, i8* bitcast (", errorAllocationResultNumber);
            exceptionPointerType.WriteRttiPointerClassInfoDeclaration(writer);
            writer.WriteLine("* @\"{0}\" to i8*), i8* null)", exceptionPointerType.GetRttiPointerInfoName());
            writer.Indent++;
            if (exceptionHandlingClause != null)
            {
                writer.WriteLine("to label %.unreachable unwind label %.catch{0}", exceptionHandlingClause.Offset);
            }
            else
            {
                writer.WriteLine("to label %.unreachable unwind label %.unwind_exception");
                llvmWriter.needToWriteUnwindException = true;
            }

            writer.Indent--;
            llvmWriter.needToWriteUnreachable = true;

            return exceptionPointerType;
        }
    }
}