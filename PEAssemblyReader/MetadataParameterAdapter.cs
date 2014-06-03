// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataParameterAdapter.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    /// <summary>
    /// </summary>
    public class MetadataParameterAdapter : IParameter
    {
        /// <summary>
        /// </summary>
        private readonly ParameterSymbol paramDef;

        /// <summary>
        /// </summary>
        /// <param name="paramDef">
        /// </param>
        internal MetadataParameterAdapter(ParameterSymbol paramDef)
        {
            this.paramDef = paramDef;
        }

        /// <summary>
        /// </summary>
        public string Name
        {
            get
            {
                return this.paramDef.Name;
            }
        }

        /// <summary>
        /// </summary>
        public IType ParameterType
        {
            get
            {
                return new MetadataTypeAdapter(this.paramDef.Type);
            }
        }

        public override string ToString()
        {
            return this.ParameterType.ToString();
        }
    }
}