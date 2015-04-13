// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterlockedGen.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Globalization;
    using System.Linq;
    using CodeParts;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class InterlockedGen
    {
        /// <summary>
        /// </summary>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="oper">
        /// </param>
        /// <param name="attribs">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void IncDecInterlockBase(
            this OpCodePart opCodeMethodInfo,
            CWriter cWriter,
            string op)
        {
            var writer = cWriter.Output;
            var estimatedResult = cWriter.EstimatedResultOf(opCodeMethodInfo.OpCodeOperands[0]);
            cWriter.UnaryOper(writer, opCodeMethodInfo, 0, string.Format("fetch_and_{0}(", op), estimatedResult.Type);
            writer.Write(", 1)");
        }

        public static void Exchange(
            this OpCodePart opCodeMethodInfo,
            CWriter cWriter)
        {
            var writer = cWriter.Output;
            var estimatedResult0 = cWriter.EstimatedResultOf(opCodeMethodInfo.OpCodeOperands[0]);
            var estimatedResult1 = cWriter.EstimatedResultOf(opCodeMethodInfo.OpCodeOperands[1]);

            var typeFix = string.Empty;
            var typeFixOp1 = string.Empty;
            var typeFixOp2 = string.Empty;

            if (estimatedResult1.Type.TypeEquals(cWriter.System.System_Single))
            {
                typeFix = "Int32";
            }
            else if (estimatedResult1.Type.TypeEquals(cWriter.System.System_Double))
            {
                typeFix = "Int64";
            }

            if (!string.IsNullOrEmpty(typeFix))
            {
                typeFixOp1 = string.Format("({0}*)", typeFix);
                typeFixOp2 = string.Format("*({0}*)&", typeFix);
            }

            cWriter.UnaryOper(writer, opCodeMethodInfo, 0, string.Format("swap({0}", typeFixOp1), estimatedResult0.Type);
            cWriter.UnaryOper(writer, opCodeMethodInfo, 1, string.Format(", {0}", typeFixOp2), estimatedResult1.Type);
            writer.Write(")");
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="oper">
        /// </param>
        /// <param name="attribs">
        /// </param>
        /// <param name="extractValue">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void CompareExchange(
            this OpCodePart opCodeMethodInfo,
            CWriter cWriter,
            int[] operands)
        {
            var writer = cWriter.Output;
            var estimatedResult = cWriter.EstimatedResultOf(opCodeMethodInfo.OpCodeOperands[operands[2]]);

            var typeFix = string.Empty;
            var typeFixOp1 = string.Empty;
            var typeFixOp2 = string.Empty;

            if (estimatedResult.Type.TypeEquals(cWriter.System.System_Single))
            {
                typeFix = "Int32";
            }
            else if (estimatedResult.Type.TypeEquals(cWriter.System.System_Double))
            {
                typeFix = "Int64";
            }

            if (!string.IsNullOrEmpty(typeFix))
            {
                typeFixOp1 = string.Format("({0}*)", typeFix);
                typeFixOp2 = string.Format("*({0}*)&", typeFix);
            }

            cWriter.UnaryOper(writer, opCodeMethodInfo, operands[0], string.Format("compare_and_swap({0}", typeFixOp1), estimatedResult.Type.ToPointerType());
            cWriter.UnaryOper(writer, opCodeMethodInfo, operands[1], string.Format(", {0}", typeFixOp2), estimatedResult.Type);
            cWriter.UnaryOper(writer, opCodeMethodInfo, operands[2], string.Format(", {0}", typeFixOp2), estimatedResult.Type);
            writer.Write(")");
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsInterlockedFunction(this IMethod method)
        {
            if (!method.IsStatic)
            {
                return false;
            }

            if (method.DeclaringType == null || method.DeclaringType.FullName != "System.Threading.Interlocked")
            {
                return false;
            }

            switch (method.MetadataName)
            {
                case "Increment":
                case "Decrement":
                case "Exchange`1":
                case "Exchange":
                case "CompareExchange`1":
                case "CompareExchange":
                    return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void WriteInterlockedFunction(
            this IMethod method,
            OpCodePart opCodeMethodInfo,
            CWriter cWriter)
        {
            switch (method.MetadataName)
            {
                case "Increment":
                    opCodeMethodInfo.IncDecInterlockBase(cWriter, "add");
                    break;

                case "Decrement":
                    opCodeMethodInfo.IncDecInterlockBase(cWriter, "sub");
                    break;

                case "Exchange`1":
                case "Exchange":
                    opCodeMethodInfo.Exchange(cWriter);
                    break;

                case "CompareExchange`1":
                case "CompareExchange":
                    opCodeMethodInfo.CompareExchange(cWriter, new[] { 0, 2, 1 }); 
                    break;
            }
        }
    }
}