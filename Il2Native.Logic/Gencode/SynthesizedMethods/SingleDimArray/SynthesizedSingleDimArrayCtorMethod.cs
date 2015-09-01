namespace Il2Native.Logic.Gencode.SynthesizedMethods.SingleDimArray
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Il2Native.Logic.Gencode.SynthesizedMethods.Base;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedSingleDimArrayCtorMethod : SynthesizedIlCodeBuilderThisMethod, IConstructor
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
        public SynthesizedSingleDimArrayCtorMethod(IType arrayType, ITypeResolver typeResolver)
            : base(null, ".ctor", arrayType, typeResolver.System.System_Void)
        {
            this.typeResolver = typeResolver;

            IlCodeBuilder code;
            ArraySingleDimensionGen.GetSingleDimensionArrayCtor(arrayType, typeResolver, out code);
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            IlCodeBuilder code;
            ArraySingleDimensionGen.GetSingleDimensionArrayCtor(Type, typeResolver, out code);
            return code;
        }
    }
}
