// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedNewArrayMethod.cs" company="">
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
    public class SynthesizedNewArrayMethod : SynthesizedMethodTypeBase
    {
        private ITypeResolver typeResolver;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedNewArrayMethod(IType type, ITypeResolver typeResolver)
            : base(type, string.Format(".newarr"))
        {
            this.typeResolver = typeResolver;
            Type = type.ToClass();
        }

        public override IType ReturnType
        {
            get
            {
                return Type.ToArrayType(1);
            }
        }

        public override System.Collections.Generic.IEnumerable<IParameter> GetParameters()
        {
            return new[] { this.typeResolver.ResolveType("System.Int32").ToParameter() };
        }
    }
}