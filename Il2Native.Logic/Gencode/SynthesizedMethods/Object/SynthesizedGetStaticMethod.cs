namespace Il2Native.Logic.Gencode.SynthesizedMethods.Object
{
    using Base;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedGetStaticMethod : SynthesizedIlCodeBuilderStaticMethod
    {
        private readonly ITypeResolver typeResolver;
        
        private readonly IField field;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="typeResolver">
        /// </param>
        public SynthesizedGetStaticMethod(IType type, IField field, ITypeResolver typeResolver)
            : base(null, string.Concat("get_static_", field.Name), type, field.FieldType)
        {
            this.typeResolver = typeResolver;
            this.field = field;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            var codeBuilder = new IlCodeBuilder();
            typeResolver.GetGetStaticMethod(codeBuilder, this.Type, field);
            return codeBuilder;
        }
    }
}
