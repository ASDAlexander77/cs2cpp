// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedCastMethod.cs" company="">
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
    public class SynthesizedCastMethod : SynthesizedIlCodeBuilderStaticMethod
    {
        public const string Name = ".cast";

        private readonly ICodeWriter codeWriter;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedCastMethod(ICodeWriter codeWriter)
            : base(null, Name, codeWriter.System.System_Object, codeWriter.System.System_Object.ToClass())
        {
            this.codeWriter = codeWriter;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            return ObjectInfrastructure.GetDynamicCastMethod(this.codeWriter, Type, true);
        }
    }
}