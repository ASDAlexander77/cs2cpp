// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedGetTypeMethod.cs" company="">
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
    public class SynthesizedGetTypeMethod : SynthesizedMethodTypeBase
    {
        /// <summary>
        /// </summary>
        private readonly LlvmWriter writer;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedGetTypeMethod(IType type, LlvmWriter writer)
            : base(type, "GetType")
        {
            this.writer = writer;
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
            get { return this.writer.ResolveType("System.Type"); }
        }
    }
}