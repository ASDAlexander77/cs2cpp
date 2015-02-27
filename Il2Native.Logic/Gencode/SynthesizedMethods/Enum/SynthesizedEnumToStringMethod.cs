namespace Il2Native.Logic.Gencode.SynthesizedMethods.Enum
{
    using System.Collections.Generic;
    using System.Linq;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedEnumToStringMethod : SynthesizedThisMethod
    {
        private readonly IMethodBody _methodBody;

        private readonly IList<IParameter> _parameters;

        private readonly IList<object> _tokenResolutions;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="typeResolver">
        /// </param>
        public SynthesizedEnumToStringMethod(IType type, ITypeResolver typeResolver)
            : base("ToString", type, typeResolver.System.System_String, isOverride: true)
        {
            object[] code;
            IList<object> tokenResolutions;
            IList<IType> locals;
            IList<IParameter> parameters;
            EnumGen.GetEnumToStringMethod(type, typeResolver, out code, out tokenResolutions, out locals, out parameters);

            this._methodBody = new SynthesizedMethodBodyDecorator(
                null,
                locals,
                MethodBodyBank.Transform(code).ToArray());

            this._parameters = parameters;
            this._tokenResolutions = tokenResolutions;
        }

        public override IEnumerable<IParameter> GetParameters()
        {
            return this._parameters;
        }

        public override IMethodBody GetMethodBody(IGenericContext genericContext = null)
        {
            return this._methodBody;
        }

        public override IModule Module
        {
            get { return new SynthesizedModuleResolver(null, this._tokenResolutions); }
        }
    }
}
