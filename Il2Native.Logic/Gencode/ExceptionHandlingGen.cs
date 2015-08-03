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
    using System.Collections.Generic;
    using System.Diagnostics;
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
            this CIndentedTextWriter writer,
            TryClause tryClause)
        {
            var finallyClause = tryClause.Catches.FirstOrDefault(c => c.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally));
            if (finallyClause != null)
            {
                writer.WriteLine("int _finallyLeave{0};", finallyClause.Offset);
                writer.WriteLine("_finallyLeave{0} = 0;", finallyClause.Offset);
                writer.WriteLine("Void* _finallyEx{0};", finallyClause.Offset);
                writer.WriteLine("_finallyEx{0} = (Void*) 0;", finallyClause.Offset);
            }

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
            writer.Indent--;
            writer.Write("eh{0}_{1}:", exceptionHandlingClause.Offset, exceptionHandlingClause.Length);
            writer.Indent++;
            if (exceptionHandlingClause.OwnerTry.Catches.Last().Equals(exceptionHandlingClause))
            {
                writer.WriteLine(string.Empty);
                if (exceptionHandlingClause.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally))
                {
                    // leave jump table
                    writer.WriteLine("switch (_finallyLeave{0})", exceptionHandlingClause.Offset);
                    writer.WriteLine("{");
                    writer.Indent++;

                    var index = 0;
                    foreach (var finallyJump in exceptionHandlingClause.FinallyJumps)
                    {
                        writer.WriteLine("case {0}: goto {1};", ++index, finallyJump);
                    }

                    writer.Indent--;
                    writer.WriteLine("}");

                    writer.WriteLine("if (_finallyEx{0} != (Void*) 0)", exceptionHandlingClause.Offset);
                    writer.WriteLine("{");
                    writer.Indent++;
                    writer.WriteLine("throw _finallyEx{0};", exceptionHandlingClause.Offset);
                    writer.Indent--;
                    writer.Write("}");
                }
                else
                {
                    writer.WriteLine("throw;");
                    writer.Indent--;
                    writer.Write("}");
                }
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

            var exceptionHandlingClause = opCode.ExceptionHandlers.First();
            var tryOffset = exceptionHandlingClause.OwnerTry.Offset;

            writer.Indent--;
            writer.WriteLine(string.Empty);
            writer.WriteLine("}");
            writer.WriteLine("catch (Void* _ex{0})", tryOffset);
            writer.Write("{");
            writer.Indent++;

            if (exceptionHandlingClause.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally))
            {
                writer.WriteLine(string.Empty);
                writer.WriteLine("_finallyEx{0} = _ex{1};", exceptionHandlingClause.Offset, tryOffset);
                writer.Indent--;
                writer.WriteLine("}");

                writer.Indent--;
                writer.Write("finally{0}:", exceptionHandlingClause.Offset);
                writer.Indent++;
            }
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

                writer.Write(") == 0) goto eh{0}_{1}", exceptionHandlingClause.Offset, exceptionHandlingClause.Length);
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

        /// <summary>
        /// </summary>
        public static void WriteLeave(this CWriter cWriter, OpCodePart opCode, CatchOfFinallyClause exceptionHandlingClause, CatchOfFinallyClause upperLevelExceptionHandlingClause)
        {
            var writer = cWriter.Output;

            CatchOfFinallyClause effectiveFinally = null;
            if (exceptionHandlingClause != null &&
                exceptionHandlingClause.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally))
            {
                effectiveFinally = exceptionHandlingClause;
            }
            else if (upperLevelExceptionHandlingClause != null &&
                upperLevelExceptionHandlingClause.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally))
            {
                effectiveFinally = upperLevelExceptionHandlingClause;
            }

            if (effectiveFinally != null && opCode.JumpAddress() >= effectiveFinally.Offset)
            {
                effectiveFinally.FinallyJumps.Add(string.Concat("a", opCode.JumpAddress()));
                writer.WriteLine(
                    "_finallyLeave{0} = {1};",
                    effectiveFinally.Offset,
                    effectiveFinally.FinallyJumps.Count);
                writer.Write(string.Concat("goto finally", effectiveFinally.Offset));
            }
            else
            {
                writer.Write(string.Concat("goto a", opCode.JumpAddress()));
            }
        }
    }
}