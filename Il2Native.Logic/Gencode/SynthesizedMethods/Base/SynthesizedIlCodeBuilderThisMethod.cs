namespace Il2Native.Logic.Gencode.SynthesizedMethods.Base
{
    using System.Collections.Generic;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedIlCodeBuilderThisMethod : SynthesizedThisMethod
    {
        private IMethodBody _methodBody;

        private IList<IParameter> _parameters;

        private IList<object> _tokenResolutions;
        
        private IlCodeBuilder _codeBuilder;

        /// <summary>
        /// </summary>
        public SynthesizedIlCodeBuilderThisMethod(IlCodeBuilder codeBuilder, string name, IType declaringType, IType returningType, bool isVirtual = false, bool isOverride = false)
            : base(name, declaringType, returningType, isVirtual, isOverride)
        {
            _codeBuilder = codeBuilder;
        }

        public override IModule Module
        {
            get
            {
                this.EnsureCreated(GetIlCodeBuilder());
                return new SynthesizedModuleResolver(null, this._tokenResolutions);
            }
        }

        public override IEnumerable<IParameter> GetParameters()
        {
            this.EnsureCreated(GetIlCodeBuilder());
            return this._parameters;
        }

        public override IMethodBody GetMethodBody(IGenericContext genericContext = null)
        {
            this.EnsureCreated(GetIlCodeBuilder());
            return this._methodBody;
        }

        protected virtual IlCodeBuilder GetIlCodeBuilder()
        {
            return _codeBuilder;
        }

        private void EnsureCreated(IlCodeBuilder codeBuilder)
        {
            if (codeBuilder == null || this._methodBody != null)
            {
                return;
            }

            this._methodBody = codeBuilder.GetMethodBody();
            this._parameters = codeBuilder.GetParameters();
            this._tokenResolutions = codeBuilder.GetTokenResolutions();
        }
    }
}
