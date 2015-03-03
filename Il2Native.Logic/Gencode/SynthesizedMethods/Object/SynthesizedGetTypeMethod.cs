// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedGetTypeMethod.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System;
    using System.Linq;
    using System.Reflection;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedGetTypeMethod : SynthesizedThisMethod
    {
        public const string Name = ".gettype";

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="typeResolver">
        /// </param>
        public SynthesizedGetTypeMethod(IType type, ITypeResolver typeResolver)
            : base(Name, type, typeResolver.System.System_Type)
        {
            if (type.IsObject || (type.IsInterface && !type.GetInterfaces().Any()))
            {
                IsVirtual = true;
            }
            else
            {
                IsOverride = true;
            }
        }
    }
}