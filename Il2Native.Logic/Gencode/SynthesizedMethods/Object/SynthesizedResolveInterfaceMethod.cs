// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedResolveInterfaceMethod.cs" company="">
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
    public class SynthesizedResolveInterfaceMethod : SynthesizedIlCodeBuilderThisMethod
    {
        public const string Name = ".dyniface";

        private readonly ICodeWriter codeWriter;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedResolveInterfaceMethod(IType type, ICodeWriter codeWriter)
            : base(null, Name, type, codeWriter.System.System_Object)
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
            return ObjectInfrastructure.GetResolveInterfaceMethod(this.codeWriter, Type, true);
        }
    }
}