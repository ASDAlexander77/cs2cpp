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
        private ITypeResolver typeResolver;

        /// <summary>
        /// </summary>
        public SynthesizedUnboxMethod(IType type, ITypeResolver typeResolver)
            : base(null, ".unbox", type, type)
        {
            this.typeResolver = typeResolver;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            return typeResolver.GetUnboxMethod(Type);
        }
    }
}