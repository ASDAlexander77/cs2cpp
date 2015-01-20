// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreadGen.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode
{
    using CodeParts;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class ThreadGen
    {
        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsThreadingFunction(this IMethod method)
        {
            if (!method.IsStatic)
            {
                return false;
            }

            if (method.DeclaringType.FullName != "System.Threading.Thread")
            {
                return false;
            }

            switch (method.MetadataName)
            {
                case "MemoryBarrier":
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
        public static void WriteThreadingFunction(
            this IMethod method,
            OpCodePart opCodeMethodInfo,
            LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            switch (method.MetadataName)
            {
                case "MemoryBarrier":
                    writer.WriteLine("fence acquire");
                    break;
            }
        }
    }
}