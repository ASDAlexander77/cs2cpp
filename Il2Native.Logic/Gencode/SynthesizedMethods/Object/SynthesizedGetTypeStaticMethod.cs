// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedGetTypeStaticMethod.cs" company="">
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
    public class SynthesizedGetTypeStaticMethod : SynthesizedIlCodeBuilderStaticMethod
    {
        public const string Name = ".sgettype";

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="typeResolver">
        /// </param>
        public SynthesizedGetTypeStaticMethod(IType type, ITypeResolver typeResolver)
            : base(typeResolver.GetGetTypeStaticMethod(type), Name, type, typeResolver.System.System_Type)
        {
        }
    }
}