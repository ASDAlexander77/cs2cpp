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
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedNewMethod : SynthesizedMethodTypeBase
    {
        private ITypeResolver typeResolver;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedNewMethod(IType type, ITypeResolver typeResolver)
            : base(type, ".new")
        {
            Type = type.ToClass();
            this.typeResolver = typeResolver;
        }

        public override System.Collections.Generic.IEnumerable<IParameter> GetParameters()
        {
            if (Type.IsMultiArray)
            {
                return ArrayMultiDimensionGen.GetParameters(Type, this.typeResolver);
            }

            return base.GetParameters();
        }
    }
}