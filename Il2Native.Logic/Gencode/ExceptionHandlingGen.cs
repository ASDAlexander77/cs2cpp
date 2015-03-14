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
    using CodeParts;
    using Exceptions;
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
        /// <param name="cWriter">
        /// </param>
        /// <param name="exceptionHandlingClause">
        /// </param>
        public static void WriteCatchBegin(this CWriter cWriter, CatchOfFinallyClause exceptionHandlingClause)
        {
            var writer = cWriter.Output;

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
            var bytePointerType = cWriter.System.System_Byte.ToPointerType();
            var errorObjectOfCatchResultNumber = cWriter.SetResultNumber(opCodeNone, bytePointerType);
            writer.WriteLine("load i8** %.error_object");
            var beginCatchResultNumber = cWriter.SetResultNumber(opCodeNone, bytePointerType);
            writer.WriteLine("call i8* @__cxa_begin_catch(i8* {0})", errorObjectOfCatchResultNumber);
            if (catchType != null)
            {
                cWriter.WriteCCast(opCodeNone, beginCatchResultNumber, catchType);
                writer.WriteLine(string.Empty);

                exceptionHandlingClause.ExceptionResult = opCodeNone.Result;
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
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="exceptionHandlingClause">
        /// </param>
        /// <param name="upperLevelExceptionHandlingClause">
        /// </param>
        public static void WriteCatchEnd(
            this CWriter cWriter,
            OpCodePart opCode,
            CatchOfFinallyClause exceptionHandlingClause,
            CatchOfFinallyClause upperLevelExceptionHandlingClause)
        {
            var writer = cWriter.Output;

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
                    string.Concat("%.finally_jump", exceptionHandlingClause.Offset),
                    cWriter.System.System_Int32);

                cWriter.WriteLoad(opCodeNope, cWriter.System.System_Int32, fullyDefinedRef);
                writer.WriteLine(string.Empty);
                writer.WriteLine(
                    "switch i32 {1}, label %.finally_exit{0} [",
                    exceptionHandlingClause.Offset,
                    opCodeNope.Result);
                writer.Indent++;
                writer.WriteLine("i32 {0}, label %.finally_exit{1}", index++, exceptionHandlingClause.Offset);
                foreach (var leave in exceptionHandlingClause.FinallyJumps)
                {
                    writer.WriteLine("i32 {0}, label %{1}", index++, leave);
                }

                writer.Indent--;
                writer.WriteLine("]");

                cWriter.WriteLabel(writer, string.Concat(".finally_exit", exceptionHandlingClause.Offset));

                if (exceptionHandlingClause.EmptyFinallyRethrowRequired)
                {
                    // rethrow exception in empty finally block
                    var opCodeNop = OpCodePart.CreateNop;
                    cWriter.WriteRethrow(
                        opCodeNop,
                        upperLevelExceptionHandlingClause,
                        cWriter.tryScopes.Count > 0 ? cWriter.tryScopes.Peek().Catches.First() : null);
                }
            }

            var startOfHandlerAddress = exceptionHandlingClause.Offset;
            var endOfHandlerAddress = exceptionHandlingClause.Offset + exceptionHandlingClause.Length;

            if (exceptionHandlingClause.RethrowCatchWithCleanUpRequired)
            {
                cWriter.WriteLabel(
                    writer,
                    string.Format(".catch_with_cleanup_{0}_{1}", startOfHandlerAddress, endOfHandlerAddress));

                var opCodeNop = OpCodePart.CreateNop;
                cWriter.WriteLandingPad(
                    opCodeNop,
                    LandingPadOptions.Cleanup,
                    null,
                    new[]
                    {
                        upperLevelExceptionHandlingClause != null
                            ? upperLevelExceptionHandlingClause.Catch
                            : cWriter.System.System_Exception
                    });
                writer.WriteLine(string.Empty);
            }
            else
            {
                writer.WriteLine("store i32 0, i32* %.error_typeid");
            }

            writer.WriteLine("call void @__cxa_end_catch()");

            if (!exceptionHandlingClause.RethrowCatchWithCleanUpRequired || upperLevelExceptionHandlingClause == null)
            {
                var isLeave = opCode.Any(Code.Leave, Code.Leave_S);
                var nextOp = opCode.Next;
                if (!isLeave &&
                    (nextOp == null || nextOp.JumpDestination == null || !nextOp.JumpDestination.Any() ||
                     nextOp.GroupAddressStart != endOfHandlerAddress))
                {
                    var noNext = nextOp == null;
                    var isNextCatchBlock = nextOp != null && nextOp.CatchOrFinallyBegin != null;
                    var hasExit = cWriter.OpsByAddressStart.Values.Any(op => op.ToCode() == Code.Ret);
                    if (!isNextCatchBlock && !noNext && (isLeave || hasExit))
                    {
                        writer.WriteLine("br label %.exit{0}", endOfHandlerAddress);
                        cWriter.WriteLabel(writer, string.Concat(".exit", endOfHandlerAddress));
                        writer.WriteLine(string.Empty);
                    }
                    else
                    {
                        writer.WriteLine("unreachable");
                    }
                }
                else
                {
                    if (isLeave)
                    {
                        writer.WriteLine("br label %.a{0}", opCode.JumpAddress());
                    }
                    else
                    {
                        writer.WriteLine("br label %.a{0}", nextOp.GroupAddressStart);
                    }
                }
            }
            else
            {
                if (!upperLevelExceptionHandlingClause.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally))
                {
                    writer.WriteLine("br label %.exception_switch{0}", upperLevelExceptionHandlingClause.Offset);
                }
                else
                {
                    writer.WriteLine("br label %.finally_no_error_entry{0}", upperLevelExceptionHandlingClause.Offset);
                }
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
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteCatchProlog(this CWriter cWriter, OpCodePart opCode)
        {
            var writer = cWriter.Output;

            writer.WriteLine("; Cacth Clauses - Prolog");

            var handlerOffset = opCode.ExceptionHandlers.First().Offset;
            writer.Indent--;
            writer.WriteLine(".catch{0}:", handlerOffset);
            writer.Indent++;

            var catchTypes =
                opCode.ExceptionHandlers.Where(eh => eh.Flags == ExceptionHandlingClauseOptions.Clause)
                    .Select(eh => eh.Catch)
                    .ToArray();
            var finallyOrFault =
                opCode.ExceptionHandlers.FirstOrDefault(
                    eh =>
                        eh.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally) ||
                        eh.Flags.HasFlag(ExceptionHandlingClauseOptions.Fault));
            cWriter.WriteLandingPad(
                opCode,
                finallyOrFault != null ? LandingPadOptions.Cleanup : LandingPadOptions.None,
                finallyOrFault,
                catchTypes);

            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="exceptionHandlingClause">
        /// </param>
        /// <param name="nextExceptionHandlingClause">
        /// </param>
        public static void WriteCatchTest(
            this CWriter cWriter,
            CatchOfFinallyClause exceptionHandlingClause,
            CatchOfFinallyClause nextExceptionHandlingClause)
        {
            var writer = cWriter.Output;

            writer.WriteLine("br label %.exception_switch{0}", exceptionHandlingClause.Offset);

            writer.Indent--;
            writer.WriteLine(".exception_switch{0}:", exceptionHandlingClause.Offset);
            writer.Indent++;

            writer.WriteLine("; Test Exception type");

            var catchType = exceptionHandlingClause.Catch;

            var opCodeNone = OpCodePart.CreateNop;
            var errorTypeIdOfCatchResultNumber = cWriter.SetResultNumber(
                opCodeNone,
                cWriter.System.System_Int32);
            writer.WriteLine("load i32* %.error_typeid");
            var errorTypeIdOfExceptionResultNumber = cWriter.SetResultNumber(
                opCodeNone,
                cWriter.System.System_Byte.ToPointerType());
            writer.Write("call i32 @llvm.eh.typeid.for(i8* bitcast (");
            catchType.WriteRttiPointerClassInfoDeclaration(writer);
            writer.WriteLine("* @\"{0}\" to i8*))", catchType.GetRttiPointerInfoName());
            var compareResultResultNumber = cWriter.SetResultNumber(
                opCodeNone,
                cWriter.System.System_Boolean);
            writer.WriteLine("icmp eq i32 {0}, {1}", errorTypeIdOfCatchResultNumber, errorTypeIdOfExceptionResultNumber);
            writer.WriteLine(
                "br i1 {0}, label %.exception_handler{1}, label %.{2}",
                compareResultResultNumber,
                exceptionHandlingClause.Offset,
                nextExceptionHandlingClause != null
                    ? string.Concat("exception_switch", nextExceptionHandlingClause.Offset)
                    : "resume");

            writer.Indent--;
            writer.WriteLine(".exception_handler{0}:", exceptionHandlingClause.Offset);
            writer.Indent++;
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="finallyClause">
        /// </param>
        public static void WriteEndFinally(this CWriter cWriter, CatchOfFinallyClause finallyClause)
        {
            cWriter.WriteFinallyVariables(finallyClause);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="finallyClause">
        /// </param>
        public static void WriteFinallyLeave(this CWriter cWriter, CatchOfFinallyClause finallyClause)
        {
            var writer = cWriter.Output;

            cWriter.WriteFinallyVariables(finallyClause);

            writer.WriteLine(
                "store i32 {0}, i32* %.finally_jump{1}",
                finallyClause.FinallyJumps.Count,
                finallyClause.Offset);
            writer.WriteLine("br label %.finally_no_error_entry{0}", finallyClause.Offset);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="finallyClause">
        /// </param>
        public static void WriteFinallyVariables(this CWriter cWriter, CatchOfFinallyClause finallyClause)
        {
            if (finallyClause.FinallyVariablesAreWritten)
            {
                return;
            }

            finallyClause.FinallyVariablesAreWritten = true;

            var writer = cWriter.Output;

            writer.Write("%.finally_jump{0} = ", finallyClause.Offset);
            writer.Write("alloca i32, align " + CWriter.PointerSize);
            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="options">
        /// </param>
        /// <param name="finallyOrFaultClause">
        /// </param>
        /// <param name="catch">
        /// </param>
        /// <param name="filter">
        /// </param>
        /// <param name="exceptionAllocationResultNumber">
        /// </param>
        public static void WriteLandingPad(
            this CWriter cWriter,
            OpCodePart opCode,
            LandingPadOptions options,
            CatchOfFinallyClause finallyOrFaultClause,
            IType[] @catch = null,
            int[] filter = null,
            int? exceptionAllocationResultNumber = null)
        {
            var writer = cWriter.Output;

            cWriter.WriteLandingPadVariables();

            var landingPadResult = cWriter.SetResultNumber(
                opCode,
                cWriter.System.System_Byte.ToPointerType());

            writer.WriteLine(
                "landingpad { i8*, i32 } personality i8* bitcast (i32 (...)* @__gxx_personality_v0 to i8*)");
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

            if (@catch != null && @catch.Any())
            {
                foreach (var catchType in @catch)
                {
                    writer.Indent++;

                    if (catchType != null)
                    {
                        writer.Write("catch i8* bitcast (");
                        catchType.WriteRttiPointerClassInfoDeclaration(writer);
                        writer.WriteLine("* @\"{0}\" to i8*)", catchType.GetRttiPointerInfoName());
                    }
                    else
                    {
                        writer.Write("catch i8* null");
                    }

                    writer.Indent--;
                }
            }
            else if (finallyOrFaultClause != null)
            {
                // default catch with rethrowing it
                writer.Indent++;
                writer.WriteLine("catch i8* null");
                writer.Indent--;

                finallyOrFaultClause.EmptyFinallyRethrowRequired = true;
            }

            var getErrorObjectResultNumber = cWriter.SetResultNumber(
                opCode,
                cWriter.System.System_Byte.ToPointerType());
            writer.WriteLine("extractvalue {1} {0}, 0", landingPadResult, "{ i8*, i32 }");
            writer.WriteLine("store i8* {0}, i8** %.error_object", getErrorObjectResultNumber);
            var getErrorTypeIdResultNumber = cWriter.SetResultNumber(
                opCode,
                cWriter.System.System_Int32);
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
        /// <param name="cWriter">
        /// </param>
        public static void WriteLandingPadVariables(this CWriter cWriter)
        {
            if (cWriter.landingPadVariablesAreWritten)
            {
                return;
            }

            cWriter.landingPadVariablesAreWritten = true;

            var writer = cWriter.Output;

            writer.Write("%.error_object = ");
            writer.Write("alloca i8*, align " + CWriter.PointerSize);
            writer.WriteLine(string.Empty);

            writer.Write("%.error_typeid = ");
            writer.Write("alloca i32, align " + CWriter.PointerSize);
            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        public static void WriteResume(this CWriter cWriter)
        {
            var writer = cWriter.Output;

            writer.Indent--;
            writer.WriteLine(".resume:");
            writer.Indent++;

            writer.WriteLine("; Resume");

            var opCodeNone = OpCodePart.CreateNop;
            var getErrorObjectResultNumber = cWriter.SetResultNumber(
                opCodeNone,
                cWriter.System.System_Byte.ToPointerType());
            writer.WriteLine("load i8** %.error_object");
            var getErrorTypeIdResultNumber = cWriter.SetResultNumber(
                opCodeNone,
                cWriter.System.System_Int32);
            writer.WriteLine("load i32* %.error_typeid");
            var insertedErrorObjectResultNumber = cWriter.SetResultNumber(
                opCodeNone,
                cWriter.System.System_Byte.ToPointerType());
            writer.WriteLine("insertvalue {1} undef, i8* {0}, 0", getErrorObjectResultNumber, "{ i8*, i32 }");
            var insertedErrorTypeIdResultNumber = cWriter.SetResultNumber(
                opCodeNone,
                cWriter.System.System_Byte.ToPointerType());
            writer.WriteLine(
                "insertvalue {2} {0}, i32 {1}, 1",
                insertedErrorObjectResultNumber,
                getErrorTypeIdResultNumber,
                "{ i8*, i32 }");
            writer.WriteLine("resume {1} {0}", insertedErrorTypeIdResultNumber, "{ i8*, i32 }");
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="exceptionHandlingClause">
        /// </param>
        /// <param name="upperLevelExceptionHandlingClause">
        /// </param>
        public static void WriteRethrow(
            this CWriter cWriter,
            OpCodePart opCode,
            CatchOfFinallyClause exceptionHandlingClause,
            CatchOfFinallyClause upperLevelExceptionHandlingClause)
        {
            var writer = cWriter.Output;

            writer.WriteLine("; Rethrow");
            WriteRethrowInvoke(cWriter, exceptionHandlingClause);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="exceptionHandlingClause">
        /// </param>
        public static void WriteThrow(
            this CWriter cWriter,
            OpCodePart opCode,
            CatchOfFinallyClause exceptionHandlingClause)
        {
            var writer = cWriter.Output;

            var exceptionPointerType = opCode.OpCodeOperands[0].Result.Type;
            writer.Write("__cxa_throw((Byte*)");

            cWriter.UnaryOper(writer, opCode, string.Format("&(*__cxa_allocate_exception({0}) = ", CWriter.PointerSize));
            writer.Write(")");

            writer.Write(", (Byte*) &{0}, (Byte*) 0)", exceptionPointerType.GetRttiPointerInfoName());

            cWriter.needToWriteUnwindException = true;
            cWriter.needToWriteUnreachable = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        public static void WriteUnexpectedCall(this CWriter cWriter)
        {
            var writer = cWriter.Output;

            writer.Indent--;
            writer.WriteLine(".unexpected:");
            writer.Indent++;
            var result = cWriter.SetResultNumber(null, cWriter.System.System_Byte.ToPointerType());
            writer.WriteLine("load i8** %.error_object");
            writer.WriteLine("call void @__cxa_call_unexpected(i8* {0})", result);
            writer.WriteLine("unreachable");
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        public static void WriteUnreachable(this CWriter cWriter)
        {
            var writer = cWriter.Output;

            writer.Indent--;
            writer.WriteLine(".unreachable:");
            writer.Indent++;
            writer.WriteLine("unreachable");
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        public static void WriteUnwindException(this CWriter cWriter)
        {
            var writer = cWriter.Output;

            writer.Indent--;
            writer.WriteLine(".unwind_exception:");
            writer.Indent++;
            cWriter.WriteLandingPad(OpCodePart.CreateNop, LandingPadOptions.EmptyFilter, null);
            writer.WriteLine(string.Empty);
            writer.WriteLine("br label %.unexpected");
            cWriter.WriteUnexpectedCall();
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="exceptionHandlingClause">
        /// </param>
        private static void WriteRethrowInvoke(CWriter cWriter, CatchOfFinallyClause exceptionHandlingClause)
        {
            var writer = cWriter.Output;

            writer.WriteLine("invoke void @__cxa_rethrow()");
            if (exceptionHandlingClause != null)
            {
                writer.Indent++;
                writer.WriteLine(
                    "to label %.unreachable unwind label %.catch_with_cleanup_{0}_{1}",
                    exceptionHandlingClause.Offset,
                    exceptionHandlingClause.Offset + exceptionHandlingClause.Length);
                writer.Indent--;
                exceptionHandlingClause.RethrowCatchWithCleanUpRequired = true;
            }
            else
            {
                writer.Indent++;
                writer.WriteLine("to label %.unreachable unwind label %.unwind_exception");
                writer.Indent--;
                cWriter.needToWriteUnwindException = true;
            }

            cWriter.needToWriteUnreachable = true;
        }
    }
}