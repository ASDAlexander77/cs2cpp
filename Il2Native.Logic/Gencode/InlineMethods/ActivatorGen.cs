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

            if (method.DeclaringType.FullName != "System.Activator")
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
        /// <param name="llvmWriter">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public static void WriteActivatorFunction(
            this IMethod method,
            OpCodePart opCodeMethodInfo,
            LlvmWriter llvmWriter)
        {
            switch (method.MetadataName)
            {
                case "CreateInstance`1":

                    // this is dummy function which is not used now as using Boxing before calling CreateInstance is enough for us
                    var type = method.GetGenericArguments().First();

                    ////if (!type.IsStructureType())
                    ////{
                    ////    opCodeMethodInfo.Result = llvmWriter.WriteNewCallingDefaultConstructor(llvmWriter.Output, type);
                    ////}
                    ////else
                    ////{
                    ////    llvmWriter.WriteInit(opCodeMethodInfo, type, opCodeMethodInfo.Destination);
                    ////}
                    opCodeMethodInfo.Result = new ConstValue(null, type);

                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}