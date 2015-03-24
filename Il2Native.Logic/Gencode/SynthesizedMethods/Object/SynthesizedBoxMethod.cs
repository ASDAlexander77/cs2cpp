// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedBoxMethod.cs" company="">
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
    public class SynthesizedBoxMethod : SynthesizedIlCodeBuilderStaticMethod
    {
        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public SynthesizedBoxMethod(IType type, ITypeResolver typeResolver)
            : base(typeResolver.GetBoxMethod(type, false), ".box", type, type.ToClass())
        {
        }
    }
}