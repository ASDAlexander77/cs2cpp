// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedGetTypeMethod.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System.Linq;

    using Il2Native.Logic.Gencode.SynthesizedMethods.Base;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedGetTypeMethod : SynthesizedIlCodeBuilderThisMethod
    {
        public const string Name = ".gettype";

        private ICodeWriter codeWriter;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedGetTypeMethod(IType type, ICodeWriter codeWriter)
            : base(null, Name, type, codeWriter.System.System_Type)
        {
            this.codeWriter = codeWriter;
            if (type.IsObject || (type.IsInterface && !type.GetInterfaces().Any()))
            {
                IsVirtual = true;
            }
            else
            {
                IsOverride = true;
            }
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            return ObjectInfrastructure.GetGetTypeMethod(this.codeWriter, Type);
        }
    }
}