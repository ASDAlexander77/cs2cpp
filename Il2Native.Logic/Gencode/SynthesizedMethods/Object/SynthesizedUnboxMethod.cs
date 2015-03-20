// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedUnboxMethod.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using Il2Native.Logic.Gencode.SynthesizedMethods.Base;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedUnboxMethod : SynthesizedIlCodeBuilderThisMethod
    {
        /// <summary>
        /// </summary>
        public SynthesizedUnboxMethod(IType type, ITypeResolver typeResolver)
            : base(typeResolver.GetUnboxMethod(type), ".unbox", type, type)
        {
        }
    }
}