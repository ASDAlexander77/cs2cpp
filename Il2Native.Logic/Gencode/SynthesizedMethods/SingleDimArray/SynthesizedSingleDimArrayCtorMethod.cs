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
        private readonly ICodeWriter codeWriter;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="codeWriterer">
        /// </param>
        public SynthesizedSingleDimArrayCtorMethod(IType arrayType, ICodeWriter codeWriter)
            : base(null, ".ctor", arrayType, codeWriter.System.System_Void)
        {
            this.codeWriter = codeWriter;

            IlCodeBuilder code;
            ArraySingleDimensionGen.GetSingleDimensionArrayCtor(arrayType, codeWriter, out code);
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            IlCodeBuilder code;
            ArraySingleDimensionGen.GetSingleDimensionArrayCtor(Type, this.codeWriter, out code);
            return code;
        }
    }
}
