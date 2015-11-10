// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedNewMethod.cs" company="">
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
    public class SynthesizedInitMethod : SynthesizedIlCodeBuilderThisMethod
    {
        public const string Name = ".init";

        private ICodeWriter codeWriter;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedInitMethod(IType type, ICodeWriter codeWriter)
            : base(null, Name, type, codeWriter.System.System_Void)
        {
            this.codeWriter = codeWriter;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            return ObjectInfrastructure.GetInitMethod(this.codeWriter, Type);
        }
    }
}