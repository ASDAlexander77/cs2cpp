namespace Il2Native.Logic.Gencode.SynthesizedMethods.MultiDimArray
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    // TODO: create object using new(..., ...) instead of ctor
    public class SynthesizedMultiDimArrayCtorMethod : SynthesizedMethodTypeBase, IConstructor
    {
        /// <summary>
        /// </summary>
        private readonly ITypeResolver _typeResolver;

        private readonly IMethodBody _methodBody;

        private readonly IList<IParameter> _parameters;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="typeResolver">
        /// </param>
        public SynthesizedMultiDimArrayCtorMethod(IType type, ITypeResolver typeResolver)
            : base(type, ".ctor")
        {
            this._typeResolver = typeResolver;

            object[] code;
            IList<object> tokenResolutions;
            IList<IType> locals;
            IList<IParameter> parameters;
            ArrayMultiDimensionGen.GetMultiDimensionArrayCtor(type, typeResolver, out code, out tokenResolutions, out locals, out parameters);

            this._methodBody = new SynthesizedMethodBodyDecorator(
                null,
                locals,
                MethodBodyBank.Transform(code).ToArray());

            this._parameters = parameters;
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
            get { return this._typeResolver.ResolveType("System.Void"); }
        }

        public override IEnumerable<IParameter> GetParameters()
        {
            return this._parameters;
        }

        public IMethodBody GetMethodBody(IGenericContext genericContext = null)
        {
            return this._methodBody;
        }
    }
}
