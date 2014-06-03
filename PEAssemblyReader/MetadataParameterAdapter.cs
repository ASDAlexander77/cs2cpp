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
    using System.Text;

    using Microsoft.CodeAnalysis;
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
                return new MetadataTypeAdapter(this.paramDef.Type, false, this.IsRef, this.IsOut);
            }
        }

        public bool IsRef 
        {
            get
            {
                return this.paramDef.RefKind == RefKind.Ref;
            }
        }

        public bool IsOut
        {
            get
            {
                return this.paramDef.RefKind == RefKind.Out;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            var result = new StringBuilder();

            if (this.IsRef)
            {
                result.Append("Ref ");
            }

            if (this.IsOut)
            {
                result.Append("Out ");
            }

            result.Append(this.ParameterType);
            return result.ToString();
        }
    }
}