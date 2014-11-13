// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedGetTypeStaticMethod.cs" company="">
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
    public class SynthesizedGetTypeStaticMethod : SynthesizedMethodTypeBase
    {
        /// <summary>
        /// </summary>
        private readonly IType systemType;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public SynthesizedGetTypeStaticMethod(IType type, LlvmWriter llvmWriter)
            : base(type, ".getType")
        {
            this.systemType = llvmWriter.ResolveType("System.Type");
        }

        /// <summary>
        /// </summary>
        public override IType ReturnType
        {
            get
            {
                return this.systemType;
            }
        }
    }
}