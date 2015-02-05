// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedGetHashCodeMethod.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System.Reflection;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedGetHashCodeMethod : SynthesizedThisMethod
    {
        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedGetHashCodeMethod(IType type, ITypeResolver typeResolver)
            : base("GetHashCode", type, typeResolver.ResolveType("System.Int32"))
        {
            if (type.IsObject)
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