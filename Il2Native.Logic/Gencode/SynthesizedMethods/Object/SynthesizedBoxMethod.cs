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
        private ITypeResolver typeResolver;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public SynthesizedBoxMethod(IType type, ITypeResolver typeResolver)
            : base(null, ".box", type, type.ToClass())
        {
            this.typeResolver = typeResolver;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            return typeResolver.GetBoxMethod(Type, false);
        }
    }
}