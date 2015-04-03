namespace Il2Native.Logic.Gencode.SynthesizedMethods.SingleDimArray
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedSingleDimArrayIListGetItemMethod : SynthesizedThisMethod
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
        public SynthesizedSingleDimArrayIListGetItemMethod(IType arrayType, ITypeResolver typeResolver)
            : base("get_Item", arrayType, arrayType.GetElementType())
        {
            var codeList = new IlCodeBuilder();
            codeList.LoadArgument(0);
            codeList.LoadArgument(1);
            codeList.Add(Code.Ldelem, 1);
            codeList.Add(Code.Ret);

            var locals = new List<IType>();

            this._methodBody = 
                new SynthesizedMethodBodyDecorator(
                    null,
                    locals,
                    codeList.GetCode());

            this._parameters = new List<IParameter>();
            this._parameters.Add(typeResolver.System.System_Int32.ToParameter("index"));

            this._tokenResolutions = new List<object>();
            this._tokenResolutions.Add(arrayType.GetElementType());
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
