namespace Il2Native.Logic.Gencode.SynthesizedMethods.Object
{
    using Base;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedGetStaticAddressMethod : SynthesizedIlCodeBuilderStaticMethod
    {
        public const string GetStaticAddresasMethodPrefix = "get_static_addr_";

        private readonly ICodeWriter codeWriter;
        
        private readonly IField field;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="codeWriterer">
        /// </param>
        public SynthesizedGetStaticAddressMethod(IType type, IField field, ICodeWriter codeWriter)
            : base(null, string.Concat(GetStaticAddresasMethodPrefix, field.Name), type, field.FieldType.ToPointerType())
        {
            this.codeWriter = codeWriter;
            this.field = field;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            var codeBuilder = new IlCodeBuilder();
            this.codeWriter.GetGetStaticAddressMethod(codeBuilder, this.Type, this.field);
            return codeBuilder;
        }
    }
}
