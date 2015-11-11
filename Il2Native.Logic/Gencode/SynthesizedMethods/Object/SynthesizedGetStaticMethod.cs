namespace Il2Native.Logic.Gencode.SynthesizedMethods.Object
{
    using Base;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedGetStaticMethod : SynthesizedIlCodeBuilderStaticMethod
    {
        public const string GetStaticMethodPrefix = "get_static_";

        private readonly ICodeWriter codeWriter;
        
        private readonly IField field;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="codeWriterer">
        /// </param>
        public SynthesizedGetStaticMethod(IType type, IField field, ICodeWriter codeWriter)
            : base(null, string.Concat(GetStaticMethodPrefix, field.Name), type, field.FieldType)
        {
            this.codeWriter = codeWriter;
            this.field = field;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            var codeBuilder = new IlCodeBuilder();
            ObjectInfrastructure.GetGetStaticMethod(this.codeWriter, codeBuilder, this.Type, field);
            return codeBuilder;
        }
    }
}
