namespace Il2Native.Logic.Gencode.SynthesizedMethods.Object
{
    using Base;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedGetStaticAddressMethod : SynthesizedIlCodeBuilderStaticMethod
    {
        public const string GetStaticAddresasMethodPrefix = "get_static_addr_";

        private readonly ITypeResolver typeResolver;
        
        private readonly IField field;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="typeResolver">
        /// </param>
        public SynthesizedGetStaticAddressMethod(IType type, IField field, ITypeResolver typeResolver)
            : base(null, string.Concat(GetStaticAddresasMethodPrefix, field.Name), type, field.FieldType.ToPointerType())
        {
            this.typeResolver = typeResolver;
            this.field = field;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            var codeBuilder = new IlCodeBuilder();
            typeResolver.GetGetStaticAddressMethod(codeBuilder, this.Type, field);
            return codeBuilder;
        }
    }
}
