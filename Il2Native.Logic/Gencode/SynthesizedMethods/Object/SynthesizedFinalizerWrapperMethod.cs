// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedFinalizerWrapperMethod.cs" company="">
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
    public class SynthesizedFinalizerWrapperMethod : SynthesizedIlCodeBuilderStaticMethod
    {
        public const string Name = ".finalizerWrapper";

        private ITypeResolver _typeResolver;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedFinalizerWrapperMethod(IType type, ITypeResolver typeResolver)
            : base(null, Name, type, typeResolver.System.System_Void)
        {
            _typeResolver = typeResolver;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            return _typeResolver.GetFinalizerWrapperMethod(Type);
        }
    }
}