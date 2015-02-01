// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedGetHashCodeMethod.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System.Reflection;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedGetHashCodeMethod : SynthesizedMethodTypeBase
    {
        /// <summary>
        /// </summary>
        private readonly ITypeResolver typeResolver;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedGetHashCodeMethod(IType type, ITypeResolver typeResolver)
            : base(type, "GetHashCode")
        {
            this.typeResolver = typeResolver;
        }

        /// <summary>
        /// </summary>
        public override CallingConventions CallingConvention
        {
            get { return CallingConventions.HasThis; }
        }

        /// <summary>
        /// </summary>
        public override IType ReturnType
        {
            get { return this.typeResolver.ResolveType("System.Int32"); }
        }
    }
}