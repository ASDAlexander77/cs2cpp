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

    public static class ExceptionHandlingGen
    {
        public static void WriteThrow(
            this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer, OpCodePart opCode, IExceptionHandlingClause exceptionHandlingClause)
        {
            writer.WriteLine("; Throw");

            var exceptionPointerType = exceptionHandlingClause != null
                                           ? WriteThrowInvoke(llvmWriter, writer, opCode, exceptionHandlingClause)
                                           : WriteThrowCall(llvmWriter, writer, opCode, exceptionHandlingClause);

            llvmWriter.typeRttiPointerDeclRequired.Add(exceptionPointerType);
            llvmWriter.CheckIfExternalDeclarationIsRequired(exceptionPointerType);
        }

        public static void WriteRethrow(
            this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer, OpCodePart opCode, IExceptionHandlingClause exceptionHandlingClause, IExceptionHandlingClause upperLevelExceptionHandlingClause)
        {
            writer.WriteLine("; Rethrow");
            WriteRethrowInvoke(llvmWriter, writer, opCode, exceptionHandlingClause, upperLevelExceptionHandlingClause);
        }

        private static IType WriteThrowInvoke(LlvmWriter llvmWriter, LlvmIndentedTextWriter writer, OpCodePart opCode, IExceptionHandlingClause exceptionHandlingClause)
        {
            var errorAllocationResultNumber = llvmWriter.WriteAllocateException(writer, opCode);

            var exceptionPointerType = opCode.OpCodeOperands[0].ResultType;
            writer.Write("invoke void @__cxa_throw(i8* {0}, i8* bitcast (", llvmWriter.GetResultNumber(errorAllocationResultNumber));
            exceptionPointerType.WriteRttiPointerClassInfoDeclaration(writer);
            writer.WriteLine("* @\"{0}\" to i8*), i8* null)", exceptionPointerType.GetRttiPointerInfoName());
            if (exceptionHandlingClause != null)
            {
                writer.WriteLine("to label %.unreachable unwind label %.catch{0}", exceptionHandlingClause.HandlerOffset);
            }
            else
            {
                writer.WriteLine("to label %.unreachable unwind label %.unwind_exception");
                llvmWriter.needToWriteUnwindException = true;
            }

            llvmWriter.needToWriteUnreachable = true;

            return exceptionPointerType;
        }

        private static void WriteRethrowInvoke(LlvmWriter llvmWriter, LlvmIndentedTextWriter writer, OpCodePart opCode, IExceptionHandlingClause exceptionHandlingClause, IExceptionHandlingClause upperLevelExceptionHandlingClause)
        {
            writer.WriteLine("invoke void @__cxa_rethrow()");
            if (exceptionHandlingClause != null)
            {
                writer.Indent++;
                writer.WriteLine("to label %.unreachable unwind label %.catch_with_cleanup{0}", exceptionHandlingClause.HandlerOffset + exceptionHandlingClause.HandlerLength);
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

        private static IType WriteThrowCall(LlvmWriter llvmWriter, LlvmIndentedTextWriter writer, OpCodePart opCode, IExceptionHandlingClause exceptionHandlingClause)
        {
            var errorAllocationResultNumber = llvmWriter.WriteAllocateException(writer, opCode);

            var exceptionPointerType = opCode.OpCodeOperands[0].ResultType;
            writer.Write("call void @__cxa_throw(i8* {0}, i8* bitcast (", llvmWriter.GetResultNumber(errorAllocationResultNumber));
            exceptionPointerType.WriteRttiPointerClassInfoDeclaration(writer);
            writer.WriteLine("* @\"{0}\" to i8*), i8* null)", exceptionPointerType.GetRttiPointerInfoName());
            writer.WriteLine("unreachable");
            return exceptionPointerType;
        }

        public static void WriteCatchProlog(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            writer.WriteLine("; Cacth Clauses - Prolog");

            int handlerOffset = opCode.ExceptionHandlers.First().HandlerOffset;
            writer.Indent--;
            writer.WriteLine(".catch{0}:", handlerOffset);
            writer.Indent++;

            llvmWriter.WriteLandingPad(
                writer,
                opCode,
                LandingPadOptions.None,
                opCode.ExceptionHandlers.Where(eh => eh.Flags == ExceptionHandlingClauseOptions.Clause).Select(eh => eh.CatchType).ToArray());

            writer.WriteLine(string.Empty);

            writer.WriteLine("br label %.exceptions_switch{0}", handlerOffset);

            writer.Indent--;
            writer.WriteLine(".exceptions_switch{0}:", handlerOffset);
            writer.Indent++;
        }

        public static void WriteCatchTest(
            this LlvmWriter llvmWriter,
            LlvmIndentedTextWriter writer,
            IExceptionHandlingClause exceptionHandlingClause,
            IExceptionHandlingClause nextExceptionHandlingClause)
        {
            writer.WriteLine("; Test Exception type");

            var catchType = exceptionHandlingClause.CatchType;

            var opCodeNone = OpCodePart.CreateNop;
            var errorTypeIdOfCatchResultNumber = llvmWriter.WriteSetResultNumber(writer, opCodeNone);
            writer.WriteLine("load i32* %.error_typeid");
            var errorTypeIdOfExceptionResultNumber = llvmWriter.WriteSetResultNumber(writer, opCodeNone);
            writer.Write("call i32 @llvm.eh.typeid.for(i8* bitcast (");
            catchType.WriteRttiPointerClassInfoDeclaration(writer);
            writer.WriteLine("* @\"{0}\" to i8*))", catchType.GetRttiPointerInfoName());
            var compareResultResultNumber = llvmWriter.WriteSetResultNumber(writer, opCodeNone);
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

        public static void WriteCatchBegin(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer, IExceptionHandlingClause exceptionHandlingClause)
        {
            writer.WriteLine("; Begin of Catch or Finally");

            var catchType = exceptionHandlingClause.CatchType;

            var opCodeNone = OpCodePart.CreateNop;
            var errorObjectOfCatchResultNumber = llvmWriter.WriteSetResultNumber(writer, opCodeNone);
            writer.WriteLine("load i8** %.error_object");
            var beginCatchResultNumber = llvmWriter.WriteSetResultNumber(writer, opCodeNone);
            writer.WriteLine("call i8* @__cxa_begin_catch(i8* {0})", llvmWriter.GetResultNumber(errorObjectOfCatchResultNumber));
            llvmWriter.WriteBitcast(writer, opCodeNone, beginCatchResultNumber, catchType);
            writer.WriteLine(string.Empty);

            writer.WriteLine("; ==== ");
        }

        public static void WriteCatchEnd(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer, OpCodePart opCode, IExceptionHandlingClause exceptionHandlingClause, IExceptionHandlingClause upperLevelExceptionHandlingClause)
        {
            writer.WriteLine("; End of Catch or Finally");

            if (exceptionHandlingClause.RethrowCatchWithCleanUpRequired)
            {
                writer.Indent--;
                writer.WriteLine(".catch_with_cleanup{0}:", exceptionHandlingClause.HandlerOffset + exceptionHandlingClause.HandlerLength);
                writer.Indent++;

                var opCodeNop = OpCodePart.CreateNop;
                llvmWriter.WriteLandingPad(writer, opCodeNop, LandingPadOptions.Cleanup, new[] { upperLevelExceptionHandlingClause.CatchType });
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
                writer.WriteLine("br label %.exit{0}", exceptionHandlingClause.HandlerOffset + exceptionHandlingClause.HandlerLength);

                writer.Indent--;
                writer.Write(string.Concat(".exit", exceptionHandlingClause.HandlerOffset + exceptionHandlingClause.HandlerLength, ':'));
                writer.Indent++;
                writer.WriteLine(string.Empty);
            }
            else
            {
                writer.WriteLine("br label %.exceptions_switch{0}", upperLevelExceptionHandlingClause.HandlerOffset);
            }
        }

        public static void WriteResume(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer)
        {
            writer.Indent--;
            writer.WriteLine(".resume:");
            writer.Indent++;

            writer.WriteLine("; Resume");

            var opCodeNone = OpCodePart.CreateNop;
            var getErrorObjectResultNumber = llvmWriter.WriteSetResultNumber(writer, opCodeNone);
            writer.WriteLine("load i8** %.error_object");
            var getErrorTypeIdResultNumber = llvmWriter.WriteSetResultNumber(writer, opCodeNone);
            writer.WriteLine("load i32* %.error_typeid");
            var insertedErrorObjectResultNumber = llvmWriter.WriteSetResultNumber(writer, opCodeNone);
            writer.WriteLine("insertvalue {1} undef, i8* {0}, 0", llvmWriter.GetResultNumber(getErrorObjectResultNumber), "{ i8*, i32 }");
            var insertedErrorTypeIdResultNumber = llvmWriter.WriteSetResultNumber(writer, opCodeNone);
            writer.WriteLine(
                "insertvalue {2} {0}, i32 {1}, 1",
                llvmWriter.GetResultNumber(insertedErrorObjectResultNumber),
                llvmWriter.GetResultNumber(getErrorTypeIdResultNumber),
                "{ i8*, i32 }");
            writer.WriteLine("resume {1} {0}", llvmWriter.GetResultNumber(insertedErrorTypeIdResultNumber), "{ i8*, i32 }");
        }

        public static int WriteAllocateException(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            writer.WriteLine("; Allocate exception");
            var errorAllocationResultNumber = llvmWriter.WriteSetResultNumber(writer, opCode, TypeAdapter.FromType(typeof(byte*)));
            writer.Write("call i8* @__cxa_allocate_exception(i32 {0})", LlvmWriter.pointerSize);
            writer.WriteLine(string.Empty);

            var newExceptionResult = opCode.OpCodeOperands[0].ResultNumber;
            var newExceptionResultType = opCode.OpCodeOperands[0].ResultType;

            opCode.OpCodeOperands[0].ResultNumber = opCode.ResultNumber;
            opCode.OpCodeOperands[0].ResultType = opCode.ResultType;

            llvmWriter.WriteBitcast(writer, opCode, newExceptionResultType, options: LlvmWriter.OperandOptions.GenerateResult);
            writer.WriteLine("*");

            opCode.OpCodeOperands[0].ResultNumber = newExceptionResult;
            opCode.OpCodeOperands[0].ResultType = newExceptionResultType;

            llvmWriter.UnaryOper(writer, opCode, "store");
            writer.Write(", ");
            llvmWriter.WriteTypePrefix(writer, opCode.OpCodeOperands[0].ResultType);
            writer.Write("* ");
            llvmWriter.WriteResultNumber(opCode.ResultNumber ?? -1);
            writer.WriteLine(string.Empty);

            return errorAllocationResultNumber;
        }

        public static void WriteUnreachable(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer)
        {
            writer.Indent--;
            writer.WriteLine(".unreachable:");
            writer.Indent++;
            writer.WriteLine("unreachable");
        }

        public static void WriteUnwindException(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer)
        {
            writer.Indent--;
            writer.WriteLine(".unwind_exception:");
            writer.Indent++;
            llvmWriter.WriteLandingPad(writer, OpCodePart.CreateNop, LandingPadOptions.EmptyFilter);
            writer.WriteLine(string.Empty);
            writer.WriteLine("br label %.unexpected");
            llvmWriter.WriteUnexpectedCall(writer);
        }

        public static void WriteUnexpectedCall(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer)
        {
            writer.Indent--;
            writer.WriteLine(".unexpected:");
            writer.Indent++;
            var result = llvmWriter.WriteSetResultNumber(writer, null);
            writer.WriteLine("load i8** %.error_object");
            writer.WriteLine("call void @__cxa_call_unexpected(i8* {0})", llvmWriter.GetResultNumber(result));
            writer.WriteLine("unreachable");
        }

        public static void WriteLandingPadVariables(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer)
        {
            if (llvmWriter.landingPadVariablesAreWritten)
            {
                return;
            }

            llvmWriter.landingPadVariablesAreWritten = true;

            writer.Write("%.error_object = ");
            writer.Write("alloca i8*, align " + LlvmWriter.pointerSize);
            writer.WriteLine(string.Empty);

            writer.Write("%.error_typeid = ");
            writer.Write("alloca i32, align " + LlvmWriter.pointerSize);
            writer.WriteLine(string.Empty);
        }

        public static void WriteLandingPad(
            this LlvmWriter llvmWriter,
            LlvmIndentedTextWriter writer,
            OpCodePart opCode,
            LandingPadOptions options,
            IType[] @catch = null,
            int[] filter = null,
            int? exceptionAllocationResultNumber = null)
        {
            llvmWriter.WriteLandingPadVariables(writer);

            var landingPadResult = llvmWriter.WriteSetResultNumber(writer, opCode);

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

            var getErrorObjectResultNumber = llvmWriter.WriteSetResultNumber(writer, opCode);
            writer.WriteLine("extractvalue {1} {0}, 0", llvmWriter.GetResultNumber(landingPadResult), "{ i8*, i32 }");
            writer.WriteLine("store i8* {0}, i8** %.error_object", llvmWriter.GetResultNumber(getErrorObjectResultNumber));
            var getErrorTypeIdResultNumber = llvmWriter.WriteSetResultNumber(writer, opCode);
            writer.WriteLine("extractvalue {1} {0}, 1", llvmWriter.GetResultNumber(landingPadResult), "{ i8*, i32 }");
            writer.Write("store i32 {0}, i32* %.error_typeid", llvmWriter.GetResultNumber(getErrorTypeIdResultNumber));

            if (exceptionAllocationResultNumber.HasValue)
            {
                writer.WriteLine(string.Empty);
                writer.Write("call void @__cxa_free_exception(i8* {0})", exceptionAllocationResultNumber.Value);
            }

            opCode.ResultNumber = landingPadResult;
            opCode.ResultType = null;
        }
    }
}