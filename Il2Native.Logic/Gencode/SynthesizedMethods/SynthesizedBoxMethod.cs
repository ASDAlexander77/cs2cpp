// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedBoxMethod.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System.Collections.Generic;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedBoxMethod : SynthesizedMethodTypeBase
    {
        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public SynthesizedBoxMethod(IType type)
            : base(type, ".box")
        {
        }

        /// <summary>
        /// </summary>
        public override IType ReturnType
        {
            get
            {
                return this.Type.ToClass();
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override IEnumerable<IParameter> GetParameters()
        {
            return new[] { this.Type.ToNormal().ToParameter() };
        }
    }
}