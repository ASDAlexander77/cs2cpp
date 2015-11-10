namespace Il2Native.Logic.Gencode.SynthesizedMethods.MultiDimArray
{
    using System.Collections.Generic;
    using System.Linq;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedMultiDimArrayGetMethod : SynthesizedThisMethod
    {
        private readonly IMethodBody _methodBody;

        private readonly IList<IParameter> _parameters;

        private readonly IList<object> _tokenResolutions;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="codeWriterer">
        /// </param>
        public SynthesizedMultiDimArrayGetMethod(IType type, ICodeWriter codeWriter)
            : base("Get", type, type.GetElementType())
        {
            object[] code;
            IList<object> tokenResolutions;
            IList<IType> locals;
            IList<IParameter> parameters;
            ArrayMultiDimensionGen.GetMultiDimensionArrayGet(type, codeWriter, out code, out tokenResolutions, out locals, out parameters);

            this._methodBody = new SynthesizedMethodBodyDecorator(
                null,
                locals,
                null,
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
