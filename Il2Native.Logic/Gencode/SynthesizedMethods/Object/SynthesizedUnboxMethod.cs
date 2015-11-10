// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedUnboxMethod.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using Il2Native.Logic.Gencode.SynthesizedMethods.Base;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedUnboxMethod : SynthesizedIlCodeBuilderThisMethod
    {
        private ICodeWriter codeWriter;

        /// <summary>
        /// </summary>
        public SynthesizedUnboxMethod(IType type, ICodeWriter codeWriter)
            : base(null, ".unbox", type, type)
        {
            this.codeWriter = codeWriter;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            return ObjectInfrastructure.GetUnboxMethod(this.codeWriter, Type);
        }
    }
}