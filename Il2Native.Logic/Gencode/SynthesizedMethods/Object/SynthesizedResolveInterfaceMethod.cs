// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedResolveInterfaceMethod.cs" company="">
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
    public class SynthesizedResolveInterfaceMethod : SynthesizedIlCodeBuilderStaticMethod
    {
        public const string Name = ".dyniface";

        private readonly ITypeResolver typeResolver;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedResolveInterfaceMethod(ITypeResolver typeResolver)
            : base(null, Name, typeResolver.System.System_Object, typeResolver.System.System_Object.ToClass())
        {
            this.typeResolver = typeResolver;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            return typeResolver.GetResolveInterfaceMethod(Type, true);
        }
    }
}