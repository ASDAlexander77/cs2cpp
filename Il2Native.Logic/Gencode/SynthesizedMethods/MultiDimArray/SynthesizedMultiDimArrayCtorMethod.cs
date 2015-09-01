namespace Il2Native.Logic.Gencode.SynthesizedMethods.MultiDimArray
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Il2Native.Logic.Gencode.SynthesizedMethods.Base;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedMultiDimArrayCtorMethod : SynthesizedIlCodeBuilderThisMethod, IConstructor
    {
        private ITypeResolver typeResolver;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="typeResolver">
        /// </param>
        public SynthesizedMultiDimArrayCtorMethod(IType arrayType, ITypeResolver typeResolver)
            : base(null, ".ctor", arrayType, typeResolver.System.System_Void)
        {
            this.typeResolver = typeResolver;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            IlCodeBuilder ilCodeBuilder;
            ArrayMultiDimensionGen.GetMultiDimensionArrayCtor(Type, typeResolver, out ilCodeBuilder);
            return ilCodeBuilder;
        }
    }
}
