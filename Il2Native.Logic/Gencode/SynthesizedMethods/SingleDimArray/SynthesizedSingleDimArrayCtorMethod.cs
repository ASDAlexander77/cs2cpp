namespace Il2Native.Logic.Gencode.SynthesizedMethods.SingleDimArray
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedSingleDimArrayCtorMethod : SynthesizedMethodTypeBase, IConstructor
    {
        /// <summary>
        /// </summary>
        private readonly ITypeResolver typeResolver;

        private readonly IMethodBody _methodBody;

        private readonly IList<IParameter> _parameters;

        private readonly IList<object> _tokenResolutions;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="typeResolver">
        /// </param>
        public SynthesizedSingleDimArrayCtorMethod(IType arrayType, ITypeResolver typeResolver)
            : base(arrayType, ".ctor")
        {
            this.typeResolver = typeResolver;

            object[] code;
            IList<object> tokenResolutions;
            IList<IType> locals;
            IList<IParameter> parameters;
            ArraySingleDimensionGen.GetSingleDimensionArrayCtor(arrayType, typeResolver, out code, out tokenResolutions, out locals, out parameters);

            this._methodBody = new SynthesizedMethodBodyDecorator(
                null,
                locals,
                null,
                MethodBodyBank.Transform(code).ToArray());

            this._parameters = parameters;
            this._tokenResolutions = tokenResolutions;
        }

        /// <summary>
        /// </summary>
        public override CallingConventions CallingConvention
        {
            get { return CallingConventions.HasThis; }
        }

        /// <summary>
        /// </summary>
        public override IType ReturnType
        {
            get { return this.typeResolver.System.System_Void; }
        }

        public override IEnumerable<IParameter> GetParameters()
        {
            return this._parameters;
        }

        public override IMethodBody GetMethodBody(IGenericContext genericContext = null)
        {
            return this._methodBody;
        }

        public IModule Module
        {
            get { return new SynthesizedModuleResolver(null, this._tokenResolutions); }
        }
    }
}
