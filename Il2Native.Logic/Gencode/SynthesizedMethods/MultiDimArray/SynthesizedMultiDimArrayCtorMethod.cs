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
        private ICodeWriter codeWriter;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="codeWriterer">
        /// </param>
        public SynthesizedMultiDimArrayCtorMethod(IType arrayType, ICodeWriter codeWriter)
            : base(null, ".ctor", arrayType, codeWriter.System.System_Void)
        {
            this.codeWriter = codeWriter;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            IlCodeBuilder ilCodeBuilder;
            ArrayMultiDimensionGen.GetMultiDimensionArrayCtor(Type, this.codeWriter, out ilCodeBuilder);
            return ilCodeBuilder;
        }
    }
}
