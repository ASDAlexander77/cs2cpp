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
    public static class MonitorGen
    {
        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsMonitorFunction(this IMethod method)
        {
            if (!method.IsStatic)
            {
                return false;
            }

            if (method.DeclaringType == null || method.DeclaringType.FullName != "System.Threading.Monitor")
            {
                return false;
            }

            switch (method.MetadataName)
            {
                case "GetLockAddress":
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
        public static void WriteMonitorFunction(
            this IMethod method,
            OpCodePart opCodeMethodInfo,
            CWriter cWriter)
        {
            var writer = cWriter.Output;

            switch (method.MetadataName)
            {
                case "GetLockAddress":

                    if (cWriter.GetMultiThreadingSupport())
                    {
                        var estimatedResult = cWriter.EstimatedResultOf(opCodeMethodInfo.OpCodeOperands[0]);
                        cWriter.UnaryOper(writer, opCodeMethodInfo, 0, "__get_lock_address((Void*)", estimatedResult.Type);
                        writer.Write(")");
                    }
                    else
                    {
                        writer.Write("__throw_not_supported");
                    }

                    break;
            }
        }
    }
}