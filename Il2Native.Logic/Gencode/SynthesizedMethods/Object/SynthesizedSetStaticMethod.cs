namespace Il2Native.Logic.Gencode.SynthesizedMethods.Object
{
    using Base;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedSetStaticMethod : SynthesizedIlCodeBuilderStaticMethod
    {
        public const string SetStaticMethodPrefix = "set_static_";

        private readonly ICodeWriter codeWriter;
        
        private readonly IField field;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="codeWriterer">
        /// </param>
        public SynthesizedSetStaticMethod(IType type, IField field, ICodeWriter codeWriter)
            : base(null, string.Concat(SetStaticMethodPrefix, field.Name), type, codeWriter.System.System_Void)
        {
            this.codeWriter = codeWriter;
            this.field = field;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            var codeBuilder = new IlCodeBuilder();
            ObjectInfrastructure.GetSetStaticMethod(this.codeWriter, codeBuilder, this.Type, field);
            return codeBuilder;
        }
    }
}
