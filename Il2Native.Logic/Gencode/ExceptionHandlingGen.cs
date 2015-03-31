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
                if (exceptionHandlingClause.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally))
                {
                    var tryOffset = exceptionHandlingClause.OwnerTry.Offset;

                    writer.WriteLine("if (_ex{0} != (Void*) 0)", tryOffset);

                    writer.WriteLine("{");
                    writer.Indent++;
                }

                writer.WriteLine("throw;");

                if (exceptionHandlingClause.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally))
                {
                    writer.Indent--;
                    writer.WriteLine("}");
                }

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

            var exceptionType = exceptionHandlingClause.Catch ?? cWriter.System.System_Exception;

            var variable = GetExceptionCaseVariable(exceptionHandlingClause);

            exceptionType.WriteTypePrefix(cWriter);
            writer.Write(" ");
            writer.Write(variable);
            writer.WriteLine(";");

            if (!exceptionType.IsObject)
            {
                writer.Write("if ((");
                writer.Write(variable);
                writer.Write(" = ");

                var opCode = OpCodePart.CreateNop;
                var opCodeOperand = OpCodePart.CreateNop;
                opCodeOperand.Result = new ConstValue("_ex" + exceptionHandlingClause.OwnerTry.Offset, cWriter.System.System_Object);
                cWriter.WriteDynamicCast(writer, opCode, opCodeOperand, exceptionType, forceCast: true);

                writer.Write(") == 0) goto eh{0}", exceptionHandlingClause.Offset + exceptionHandlingClause.Length);
            }
            else
            {
                writer.Write(variable);
                writer.Write(" = (");
                cWriter.System.System_Object.WriteTypePrefix(cWriter);
                writer.Write(") _ex{0}", exceptionHandlingClause.OwnerTry.Offset);
            }
        }

        public static string GetExceptionCaseVariable(CatchOfFinallyClause exceptionHandlingClause)
        {
            return string.Format("_case{0}", exceptionHandlingClause.Offset);
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
            writer.Write("throw");
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

        public static void WriteFinallyThrow(this CWriter cWriter)
        {
            var writer = cWriter.Output;
            writer.Write("throw (Void*) 0");
        }
    }
}