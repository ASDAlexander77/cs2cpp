// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayGen.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode
{
    using System;

    using Il2Native.Logic.CodeParts;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class ArrayGen
    {
        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsArrayFunction(this IMethod method)
        {
            if (method.DeclaringType.FullName != "System.Array")
            {
                return false;
            }

            switch (method.Name)
            {
                case "get_Length":
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
        /// <param name="llvmWriter">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public static void WriteArrayFunction(this IMethod method, OpCodePart opCodeMethodInfo, LlvmWriter llvmWriter)
        {
            switch (method.Name)
            {
                case "get_Length":
                    llvmWriter.WriteArrayGetLength(opCodeMethodInfo);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}