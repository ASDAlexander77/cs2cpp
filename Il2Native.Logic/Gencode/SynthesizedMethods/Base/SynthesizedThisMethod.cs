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
    public class SynthesizedThisMethod : SynthesizedMethodTypeBase
    {
        private readonly IType returnType;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedThisMethod(string name, IType declaringType, IType returnType, bool isVirtual = false)
            : base(declaringType, name)
        {
            this.returnType = returnType;
            IsVirtual = isVirtual;
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
            get { return this.returnType; }
        }
    }
}