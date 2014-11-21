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
    public class SynthesizedThisMethodWithReturnType : SynthesizedMethodTypeBase
    {
        private IType returnType;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedThisMethodWithReturnType(string name, string declaringType, string returnType, LlvmWriter writer)
            : base(writer.ResolveType(declaringType), name)
        {
            this.returnType = writer.ResolveType(returnType);
        }

        /// <summary>
        /// </summary>
        public override CallingConventions CallingConvention
        {
            get
            {
                return CallingConventions.HasThis;
            }
        }

        /// <summary>
        /// </summary>
        public override IType ReturnType
        {
            get
            {
                return this.returnType;
            }
        }
    }
}