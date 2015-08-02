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

        private readonly ITypeResolver typeResolver;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedResolveInterfaceMethod(IType type, ITypeResolver typeResolver)
            : base(null, Name, type, typeResolver.System.System_Object)
        {
            this.typeResolver = typeResolver;
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
            return typeResolver.GetResolveInterfaceMethod(Type, true);
        }
    }
}