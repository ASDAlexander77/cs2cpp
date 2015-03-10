namespace Il2Native.Logic.Gencode.SynthesizedMethods.Base
{
    using System.Collections.Generic;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedIlCodeBuilderStaticMethod : SynthesizedStaticMethod
    {
        private readonly IMethodBody _methodBody;

        private readonly IList<IParameter> _parameters;

        private readonly IList<object> _tokenResolutions;

        /// <summary>
        /// </summary>
        public SynthesizedIlCodeBuilderStaticMethod(IlCodeBuilder codeBuilder, string name, IType declaringType, IType returningType)
            : base(name, declaringType, returningType)
        {
            this._methodBody = codeBuilder.GetMethodBody();
            this._parameters = codeBuilder.GetParameters();
            this._tokenResolutions = codeBuilder.GetTokenResolutions();
        }

        public override IModule Module
        {
            get { return new SynthesizedModuleResolver(null, this._tokenResolutions); }
        }

        public override IEnumerable<IParameter> GetParameters()
        {
            return this._parameters;
        }

        public override IMethodBody GetMethodBody(IGenericContext genericContext = null)
        {
            return this._methodBody;
        }
    }
}
