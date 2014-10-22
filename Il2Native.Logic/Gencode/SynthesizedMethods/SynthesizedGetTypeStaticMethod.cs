namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System.Collections.Generic;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedGetTypeStaticMethod : SynthesizedMethodTypeBase
    {
        private IType systemType;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
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
