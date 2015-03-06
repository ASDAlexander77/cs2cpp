// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedGetSizeMethod.cs" company="">
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
    public class SynthesizedGetSizeMethod : SynthesizedThisMethod
    {
        public const string Name = ".getsize";

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="typeResolver">
        /// </param>
        public SynthesizedGetSizeMethod(IType type, ITypeResolver typeResolver)
            : base(Name, type, typeResolver.GetIntTypeByByteSize(CWriter.PointerSize))
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