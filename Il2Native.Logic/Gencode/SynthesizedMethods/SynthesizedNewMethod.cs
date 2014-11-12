namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System.Reflection;

    using PEAssemblyReader;
    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    public class SynthesizedNewMethod : SynthesizedMethodTypeBase
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
        public SynthesizedNewMethod(IType type, LlvmWriter writer)
            : base(type, ".new")
        {
            this.writer = writer;
            Type = type.ToClass();
        }
    }
}
