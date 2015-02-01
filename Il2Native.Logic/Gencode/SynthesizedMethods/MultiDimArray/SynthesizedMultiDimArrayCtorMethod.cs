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
        private readonly ITypeResolver typeResolver;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="typeResolver">
        /// </param>
        public SynthesizedMultiDimArrayCtorMethod(IType type, ITypeResolver typeResolver)
            : base(type, ".ctor")
        {
            this.typeResolver = typeResolver;
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
            get { return this.typeResolver.ResolveType("System.Void"); }
        }

        public override IEnumerable<IParameter> GetParameters()
        {
            var intType = typeResolver.ResolveType("System.Int32");
            return Enumerable.Range(0, Type.ArrayRank).Select(n => intType.ToParameter());
        }

        public IMethodBody GetMethodBody(IGenericContext genericContext = null)
        {
            return new SynthesizedMethodBodyDecorator(
                null,
                new IType[0],
                MethodBodyBank.Transform(new [] { (object)Code.Ret }).ToArray());
        }
    }
}
