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
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedGetTypeStaticMethod : SynthesizedMethodTypeBase
    {
        /// <summary>
        /// </summary>
        private readonly IType systemType;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="typeResolver">
        /// </param>
        public SynthesizedGetTypeStaticMethod(IType type, ITypeResolver typeResolver)
            : base(type, ".sgettype")
        {
            this.systemType = typeResolver.System.System_Type;
        }

        /// <summary>
        /// </summary>
        public override IType ReturnType
        {
            get { return this.systemType; }
        }
    }
}