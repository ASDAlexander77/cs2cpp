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
    public class SynthesizedNewMethod : SynthesizedIlCodeBuilderInlinedTextMethod
    {
        private readonly ITypeResolver typeResolver;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedNewMethod(IType type, ITypeResolver typeResolver)
            : base(typeResolver.GetNewMethod(type), ".new", type, type.ToClass())
        {
            Type = type.ToClass();
            this.typeResolver = typeResolver;
        }

        public override System.Collections.Generic.IEnumerable<IParameter> GetParameters()
        {
            return this.Type.IsArray ? ArrayMultiDimensionGen.GetParameters(this.Type, this.typeResolver) : base.GetParameters();
        }
    }
}