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
        private ITypeResolver _typeResolver;

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
            _typeResolver = typeResolver;
        }

        public override System.Collections.Generic.IEnumerable<IParameter> GetParameters()
        {
            if (Type.IsMultiArray)
            {
                return ArrayMultiDimensionGen.GetParameters(Type, _typeResolver);
            }

            return base.GetParameters();
        }
    }
}