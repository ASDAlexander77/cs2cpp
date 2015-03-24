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

        private ITypeResolver _typeResolver;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedGetTypeMethod(IType type, ITypeResolver typeResolver)
            : base(null, Name, type, typeResolver.System.System_Type)
        {
            this._typeResolver = typeResolver;
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
            return this._typeResolver.GetGetTypeMethod(Type);
        }
    }
}