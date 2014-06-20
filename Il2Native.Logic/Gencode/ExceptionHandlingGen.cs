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
            var errorAllocationResultNumber = llvmWriter.WriteSetResultNumber(opCode, TypeAdapter.FromType(typeof(byte*)));
            writer.Write("call i8* @__cxa_allocate_exception(i32 {0})", LlvmWriter.pointerSize);
            writer.WriteLine(string.Empty);

            var newExceptionResult = opCode.OpCodeOperands[0].Result;

            opCode.OpCodeOperands[0].Result = opCode.Result;

            llvmWriter.WriteBitcast(opCode, newExceptionResult.Type, options: LlvmWriter.OperandOptions.GenerateResult);
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
        public static void WriteCatchBegin(this LlvmWriter llvmWriter, IExceptionHandlingClause exceptionHandlingClause)
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

            var catchType = exceptionHandlingClause.CatchType;

            var opCodeNone = OpCodePart.CreateNop;
            var errorObjectOfCatchResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, TypeAdapter.FromType(typeof(byte*)));
            writer.WriteLine("load i8** %.error_object");
            var beginCatchResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, TypeAdapter.FromType(typeof(byte*)));
            writer.WriteLine("call i8* @__cxa_begin_catch(i8* {0})", llvmWriter.GetResultNumber(errorObjectOfCatchResultNumber));
            if (catchType != null)
            {
                llvmWriter.WriteBitcast(opCodeNone, beginCatchResultNumber, catchType);
                writer.WriteLine(string.Empty);
            }

            if (isFinally)
            {
                // set default error handler jump to carry on try/catch execution
                writer.WriteLine("store i32 0, i32* %.finally_jump{0}", exceptionHandlingClause.HandlerOffset);
                writer.WriteLine("br label %.finally_no_error_entry{0}", exceptionHandlingClause.HandlerOffset);
                writer.Indent--;
                writer.WriteLine(".finally_no_error_entry{0}:", exceptionHandlingClause.HandlerOffset);
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
            IExceptionHandlingClause exceptionHandlingClause, 
            IExceptionHandlingClause upperLevelExceptionHandlingClause)
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
                llvmWriter.WriteLlvmLoad(opCodeNope, TypeAdapter.FromType(typeof(int)), string.Concat("%.finally_jump", exceptionHandlingClause.HandlerOffset));
                writer.WriteLine(string.Empty);
                writer.WriteLine(
                    "switch i32 {1}, label %.finally_exit{0} [", exceptionHandlingClause.HandlerOffset, llvmWriter.GetResultNumber(opCodeNope.Result));
                writer.Indent++;
                writer.WriteLine("i32 {0}, label %.finally_exit{1}", index++, exceptionHandlingClause.HandlerOffset);
                foreach (var leave in exceptionHandlingClause.FinallyJumps)
                {
                    writer.WriteLine("i32 {0}, label %{1}", index++, leave);
                }

                writer.Indent--;
                writer.WriteLine("]");

                writer.Indent--;
                writer.WriteLine(".finally_exit{0}:", exceptionHandlingClause.HandlerOffset);
                writer.Indent++;
            }

            var endOfHandlerAddress = exceptionHandlingClause.HandlerOffset + exceptionHandlingClause.HandlerLength;

            if (exceptionHandlingClause.RethrowCatchWithCleanUpRequired)
            {
                writer.Indent--;
                writer.WriteLine(".catch_with_cleanup{0}:", endOfHandlerAddress);
                writer.Indent++;

                var opCodeNop = OpCodePart.CreateNop;
                llvmWriter.WriteLandingPad(opCodeNop, LandingPadOptions.Cleanup, new[] { upperLevelExceptionHandlingClause.CatchType });
                writer.WriteLine(string.Empty);
            }
            else
            {
                writer.WriteLine("store i32 0, i32* %.error_typeid");
            }

            // TODO: I didn't find what is that
            ////writer.WriteLine("store i32 1, i32* %-1");
            writer.WriteLine("call void @__cxa_end_catch()");

            if (!exceptionHandlingClause.RethrowCatchWithCleanUpRequired || upperLevelExceptionHandlingClause == null)
            {
                var nextOp = opCode.NextOpCode(llvmWriter);
                if (!nextOp.JumpDestination.Any() || nextOp.GroupAddressStart != endOfHandlerAddress)
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
                writer.WriteLine("br label %.exceptions_switch{0}", upperLevelExceptionHandlingClause.HandlerOffset);
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

            var handlerOffset = opCode.ExceptionHandlers.First().HandlerOffset;
            writer.Indent--;
            writer.WriteLine(".catch{0}:", handlerOffset);
            writer.Indent++;

            llvmWriter.WriteLandingPad(
                opCode, 
                opCode.ExceptionHandlers.Any(eh => eh.Flags == ExceptionHandlingClauseOptions.Finally) ? LandingPadOptions.Cleanup : LandingPadOptions.None, 
                opCode.ExceptionHandlers.Where(eh => eh.Flags == ExceptionHandlingClauseOptions.Clause).Select(eh => eh.CatchType).ToArray());

            writer.WriteLine(string.Empty);

            writer.WriteLine("br label %.exceptions_switch{0}", handlerOffset);

            writer.Indent--;
            writer.WriteLine(".exceptions_switch{0}:", handlerOffset);
            writer.Indent++;
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
            this LlvmWriter llvmWriter, IExceptionHandlingClause exceptionHandlingClause, IExceptionHandlingClause nextExceptionHandlingClause)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine("; Test Exception type");

            var catchType = exceptionHandlingClause.CatchType;

            var opCodeNone = OpCodePart.CreateNop;
            var errorTypeIdOfCatchResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, TypeAdapter.FromType(typeof(int)));
            writer.WriteLine("load i32* %.error_typeid");
            var errorTypeIdOfExceptionResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, TypeAdapter.FromType(typeof(byte*)));
            writer.Write("call i32 @llvm.eh.typeid.for(i8* bitcast (");
            catchType.WriteRttiPointerClassInfoDeclaration(writer);
            writer.WriteLine("* @\"{0}\" to i8*))", catchType.GetRttiPointerInfoName());
            var compareResultResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, TypeAdapter.FromType(typeof(bool)));
            writer.WriteLine(
                "icmp eq i32 {0}, {1}", 
                llvmWriter.GetResultNumber(errorTypeIdOfCatchResultNumber), 
                llvmWriter.GetResultNumber(errorTypeIdOfExceptionResultNumber));
            writer.WriteLine(
                "br i1 {0}, label %.exception_handler{1}, label %.{2}", 
                llvmWriter.GetResultNumber(compareResultResultNumber), 
                exceptionHandlingClause.HandlerOffset, 
                nextExceptionHandlingClause != null ? string.Concat("exception_handler", nextExceptionHandlingClause.HandlerOffset) : "resume");

            writer.Indent--;
            writer.WriteLine(".exception_handler{0}:", exceptionHandlingClause.HandlerOffset);
            writer.Indent++;
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="finallyClause">
        /// </param>
        public static void WriteFinallyLeave(this LlvmWriter llvmWriter, IExceptionHandlingClause finallyClause)
        {
            var writer = llvmWriter.Output;

            llvmWriter.WriteFinallyVariables(finallyClause);

            writer.WriteLine("store i32 {0}, i32* %.finally_jump{1}", finallyClause.FinallyJumps.Count, finallyClause.HandlerOffset);
            writer.WriteLine("br label %.finally_no_error_entry{0}", finallyClause.HandlerOffset);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="finallyClause">
        /// </param>
        public static void WriteFinallyVariables(this LlvmWriter llvmWriter, IExceptionHandlingClause finallyClause)
        {
            if (finallyClause.FinallyVariablesAreWritten)
            {
                return;
            }

            finallyClause.FinallyVariablesAreWritten = true;

            var writer = llvmWriter.Output;

            writer.Write("%.finally_jump{0} = ", finallyClause.HandlerOffset);
            writer.Write("alloca i32, align " + LlvmWriter.pointerSize);
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

            var landingPadResult = llvmWriter.WriteSetResultNumber(opCode, TypeAdapter.FromType(typeof(byte*)));

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

            var getErrorObjectResultNumber = llvmWriter.WriteSetResultNumber(opCode, TypeAdapter.FromType(typeof(byte*)));
            writer.WriteLine("extractvalue {1} {0}, 0", llvmWriter.GetResultNumber(landingPadResult), "{ i8*, i32 }");
            writer.WriteLine("store i8* {0}, i8** %.error_object", llvmWriter.GetResultNumber(getErrorObjectResultNumber));
            var getErrorTypeIdResultNumber = llvmWriter.WriteSetResultNumber(opCode, TypeAdapter.FromType(typeof(int)));
            writer.WriteLine("extractvalue {1} {0}, 1", llvmWriter.GetResultNumber(landingPadResult), "{ i8*, i32 }");
            writer.Write("store i32 {0}, i32* %.error_typeid", llvmWriter.GetResultNumber(getErrorTypeIdResultNumber));

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
            writer.Write("alloca i8*, align " + LlvmWriter.pointerSize);
            writer.WriteLine(string.Empty);

            writer.Write("%.error_typeid = ");
            writer.Write("alloca i32, align " + LlvmWriter.pointerSize);
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
            var getErrorObjectResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, TypeAdapter.FromType(typeof(byte*)));
            writer.WriteLine("load i8** %.error_object");
            var getErrorTypeIdResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, TypeAdapter.FromType(typeof(int)));
            writer.WriteLine("load i32* %.error_typeid");
            var insertedErrorObjectResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, TypeAdapter.FromType(typeof(byte*)));
            writer.WriteLine("insertvalue {1} undef, i8* {0}, 0", llvmWriter.GetResultNumber(getErrorObjectResultNumber), "{ i8*, i32 }");
            var insertedErrorTypeIdResultNumber = llvmWriter.WriteSetResultNumber(opCodeNone, TypeAdapter.FromType(typeof(byte*)));
            writer.WriteLine(
                "insertvalue {2} {0}, i32 {1}, 1", 
                llvmWriter.GetResultNumber(insertedErrorObjectResultNumber), 
                llvmWriter.GetResultNumber(getErrorTypeIdResultNumber), 
                "{ i8*, i32 }");
            writer.WriteLine("resume {1} {0}", llvmWriter.GetResultNumber(insertedErrorTypeIdResultNumber), "{ i8*, i32 }");
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
            IExceptionHandlingClause exceptionHandlingClause, 
            IExceptionHandlingClause upperLevelExceptionHandlingClause)
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
        public static void WriteThrow(this LlvmWriter llvmWriter, OpCodePart opCode, IExceptionHandlingClause exceptionHandlingClause)
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
            var result = llvmWriter.WriteSetResultNumber(null, TypeAdapter.FromType(typeof(byte*)));
            writer.WriteLine("load i8** %.error_object");
            writer.WriteLine("call void @__cxa_call_unexpected(i8* {0})", llvmWriter.GetResultNumber(result));
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
        private static void WriteRethrowInvoke(LlvmWriter llvmWriter, IExceptionHandlingClause exceptionHandlingClause)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine("invoke void @__cxa_rethrow()");
            if (exceptionHandlingClause != null)
            {
                writer.Indent++;
                writer.WriteLine(
                    "to label %.unreachable unwind label %.catch_with_cleanup{0}", exceptionHandlingClause.HandlerOffset + exceptionHandlingClause.HandlerLength);
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
            writer.Write("call void @__cxa_throw(i8* {0}, i8* bitcast (", llvmWriter.GetResultNumber(errorAllocationResultNumber));
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
        private static IType WriteThrowInvoke(LlvmWriter llvmWriter, OpCodePart opCode, IExceptionHandlingClause exceptionHandlingClause)
        {
            var writer = llvmWriter.Output;

            var errorAllocationResultNumber = llvmWriter.WriteAllocateException(opCode);

            var exceptionPointerType = opCode.OpCodeOperands[0].Result.Type;
            writer.Write("invoke void @__cxa_throw(i8* {0}, i8* bitcast (", llvmWriter.GetResultNumber(errorAllocationResultNumber));
            exceptionPointerType.WriteRttiPointerClassInfoDeclaration(writer);
            writer.WriteLine("* @\"{0}\" to i8*), i8* null)", exceptionPointerType.GetRttiPointerInfoName());
            writer.Indent++;
            if (exceptionHandlingClause != null)
            {
                writer.WriteLine("to label %.unreachable unwind label %.catch{0}", exceptionHandlingClause.HandlerOffset);
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