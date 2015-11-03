namespace Il2Native.Logic.Gencode.SynthesizedMethods.Object
{
    using Base;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedSetStaticMethod : SynthesizedIlCodeBuilderStaticMethod
    {
        public const string SetStaticMethodPrefix = "set_static_";

        private readonly ITypeResolver typeResolver;
        
        private readonly IField field;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="typeResolver">
        /// </param>
        public SynthesizedSetStaticMethod(IType type, IField field, ITypeResolver typeResolver)
            : base(null, string.Concat(SetStaticMethodPrefix, field.Name), type, typeResolver.System.System_Void)
        {
            this.typeResolver = typeResolver;
            this.field = field;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            var codeBuilder = new IlCodeBuilder();
            typeResolver.GetSetStaticMethod(codeBuilder, this.Type, field);
            return codeBuilder;
        }
    }
}
