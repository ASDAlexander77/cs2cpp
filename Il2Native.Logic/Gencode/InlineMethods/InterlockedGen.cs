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
            writer.Write("fetch_and_{0}(", op);
            cWriter.Pop();
            writer.Write(", 1)");
        }

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
        public static void AddInterlockBase(
            this OpCodePart opCodeMethodInfo,
            CWriter cWriter,
            string op)
        {
            var writer = cWriter.Output;
            writer.Write("fetch_and_{0}(", op);
            cWriter.Pop();
            writer.Write(", ");
            cWriter.Pop();
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
                case "ExchangeAdd":
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

                case "ExchangeAdd":
                    opCodeMethodInfo.AddInterlockBase(cWriter, "add");
                    break;

                case "Decrement":
                    opCodeMethodInfo.IncDecInterlockBase(cWriter, "sub");
                    break;
            }
        }
    }
}