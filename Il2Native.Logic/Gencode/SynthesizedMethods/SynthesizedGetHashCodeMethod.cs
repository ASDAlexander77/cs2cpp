namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedGetHashCodeMethod : SynthesizedMethodTypeBase
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
        public SynthesizedGetHashCodeMethod(IType type, LlvmWriter writer)
            : base(type, "GetHashCode")
        {
            this.writer = writer;
        }

        /// <summary>
        /// </summary>
        public override IType ReturnType
        {
            get
            {
                return this.writer.ResolveType("System.Int32");
            }
        }
    }
}
