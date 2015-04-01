// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActivatorGen.cs" company="">
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
    using CodeParts;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class ActivatorGen
    {
        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsActivatorFunction(this IMethod method)
        {
            if (!method.IsStatic)
            {
                return false;
            }

            if (method.DeclaringType == null || method.DeclaringType.FullName != "System.Activator")
            {
                return false;
            }

            switch (method.MetadataName)
            {
                case "CreateInstance`1":
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
        /// <exception cref="NotImplementedException">
        /// </exception>
        public static void WriteActivatorFunction(
            this IMethod method,
            OpCodePart opCodeMethodInfo,
            CWriter cWriter)
        {
            switch (method.MetadataName)
            {
                case "CreateInstance`1":

                    // this is dummy function which is not used now as using Boxing before calling CreateInstance is enough for us
                    var type = method.GetGenericArguments().First();

                    if (!type.IsStructureType())
                    {
                        opCodeMethodInfo.Result = cWriter.WriteNewCallingDefaultConstructor(cWriter, type, true);
                    }
                    else
                    {
                        cWriter.WriteInit(opCodeMethodInfo, type);
                    }

                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}