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
            if (exceptionHandlingClause.OwnerTry.Catches.Last().Equals(exceptionHandlingClause))
            {
                writer.WriteLine("throw;");
                writer.Indent--;
                writer.WriteLine("}");
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
            opCodeOperand.Result = new ConstValue("_ex" + exceptionHandlingClause.OwnerTry.Offset, cWriter.System.System_Object);
            cWriter.WriteDynamicCast(
                writer,
                opCode,
                opCodeOperand,
                (exceptionHandlingClause.Catch is object)
                    ? cWriter.System.System_Exception
                    : exceptionHandlingClause.Catch,
                forceCast: true);

            writer.Write(" == 0) goto eh{0}", exceptionHandlingClause.Offset + exceptionHandlingClause.Length);
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
            writer.WriteLine("throw;");
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
    }
}