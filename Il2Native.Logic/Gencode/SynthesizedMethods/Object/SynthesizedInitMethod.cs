// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedNewMethod.cs" company="">
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
    public class SynthesizedInitMethod : SynthesizedIlCodeBuilderThisMethod
    {
        public const string Name = ".init";

        private ITypeResolver _typeResolver;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedInitMethod(IType type, ITypeResolver typeResolver)
            : base(null, Name, type, typeResolver.System.System_Void)
        {
            _typeResolver = typeResolver;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            return _typeResolver.GetInitMethod(Type);
        }
    }
}