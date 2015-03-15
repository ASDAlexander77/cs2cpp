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
        public static void WriteTry(
            this CIndentedTextWriter writer)
        {
            writer.WriteLine("try");
            writer.WriteLine("{");
            writer.Indent++;
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="exceptionHandlingClause">
        /// </param>
        public static void WriteCatchBegin(this CWriter cWriter, CatchOfFinallyClause exceptionHandlingClause)
        {
            // TODO: finish it to read casted exception
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
            writer.WriteLine("eh{0}:", exceptionHandlingClause.Offset + exceptionHandlingClause.Length);
            writer.WriteLine("// catch end");
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

            var tryOffset = opCode.ExceptionHandlers.First().OwnerTry.Offset;

            writer.Indent--;
            writer.WriteLine(string.Empty);
            writer.WriteLine("}");
            writer.WriteLine("catch (Void* _ex{0})", tryOffset);
            writer.Write("{");
            writer.Indent++;
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

            writer.Write("if (");

            var opCode = OpCodePart.CreateNop;
            var opCodeOperand = OpCodePart.CreateNop;
            opCodeOperand.Result = new ConstValue("_ex" + exceptionHandlingClause.OwnerTry.Offset, cWriter.System.System_Object.ToPointerType());
            cWriter.WriteDynamicCast(writer, opCode, opCodeOperand, exceptionHandlingClause.Catch);

            writer.WriteLine(" == 0) goto eh{0};", exceptionHandlingClause.Offset + exceptionHandlingClause.Length);
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

            // TODO: finish it
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
            writer.WriteLine("rethrow;");
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
            cWriter.UnaryOper(writer, opCode, "throw (Void*) ");
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
    }
}