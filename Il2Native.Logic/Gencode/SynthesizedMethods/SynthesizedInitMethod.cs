// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedInitMethod.cs" company="">
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
    public class SynthesizedInitMethod : SynthesizedMethodTypeBase
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
        public SynthesizedInitMethod(IType type, LlvmWriter writer)
            : base(type, ".init")
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
            get { return this.writer.ResolveType("System.Void"); }
        }
    }
}