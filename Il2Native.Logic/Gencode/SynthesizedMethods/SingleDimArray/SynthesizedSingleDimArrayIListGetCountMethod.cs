namespace Il2Native.Logic.Gencode.SynthesizedMethods.SingleDimArray
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedSingleDimArrayIListGetCountMethod : SynthesizedThisMethod
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
        public SynthesizedSingleDimArrayIListGetCountMethod(IType arrayType, ITypeResolver typeResolver)
            : base("get_Count", arrayType, typeResolver.ResolveType("System.Int32"))
        {
            var codeList = new IlCodeBuilder();
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldlen);
            codeList.Add(Code.Ret);

            var locals = new List<IType>();

            this._methodBody =
                new SynthesizedMethodBodyDecorator(
                    null,
                    locals,
                    codeList.GetCode());

            this._parameters = new List<IParameter>();

            this._tokenResolutions = new List<object>();
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
