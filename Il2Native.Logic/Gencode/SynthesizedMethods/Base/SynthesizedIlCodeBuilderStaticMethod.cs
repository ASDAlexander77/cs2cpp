namespace Il2Native.Logic.Gencode.SynthesizedMethods.Base
{
    using System.Collections.Generic;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedIlCodeBuilderStaticMethod : SynthesizedInlinedTextMethod
    {
        private IMethodBody _methodBody;

        private IList<IParameter> _parameters;

        private IList<object> _tokenResolutions;

        private IlCodeBuilder _codeBuilder;

        /// <summary>
        /// </summary>
        public SynthesizedIlCodeBuilderStaticMethod(IlCodeBuilder codeBuilder, string name, IType declaringType, IType returningType)
            : base(name, declaringType, returningType)
        {
            this._codeBuilder = codeBuilder;
            IsStatic = true;
        }

        public override IModule Module
        {
            get
            {
                this.EnsureCreated();
                return new SynthesizedModuleResolver(null, this._tokenResolutions);
            }
        }

        public override IEnumerable<IParameter> GetParameters()
        {
            this.EnsureCreated();
            return this._parameters;
        }

        public override IMethodBody GetMethodBody(IGenericContext genericContext = null)
        {
            this.EnsureCreated();
            return this._methodBody;
        }

        protected virtual IlCodeBuilder GetIlCodeBuilder()
        {
            return this._codeBuilder;
        }

        private void EnsureCreated()
        {
            if (this._methodBody != null || this._parameters != null || this._tokenResolutions != null)
            {
                return;
            }

            var codeBuilder = GetIlCodeBuilder();

            this._methodBody = codeBuilder.GetMethodBody();
            this._parameters = codeBuilder.GetParameters();
            this._tokenResolutions = codeBuilder.GetTokenResolutions();
        }
    }
}
