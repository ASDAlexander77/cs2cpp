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
    using System.Diagnostics;
    using System.Text;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    /// <summary>
    /// </summary>
    [DebuggerDisplay("Name = {Name}, Type = {ParameterType.FullName}")]
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
        /// <param name="paramDef">
        /// </param>
        internal MetadataParameterAdapter(ParameterSymbol paramDef, IGenericContext genericContext)
            : this(paramDef)
        {
            this.GenericContext = genericContext;
        }

        /// <summary>
        /// </summary>
        public IGenericContext GenericContext { get; set; }

        /// <summary>
        /// </summary>
        public bool IsOut
        {
            get
            {
                return this.paramDef.RefKind == RefKind.Out;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsRef
        {
            get
            {
                return this.paramDef.RefKind == RefKind.Ref;
            }
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
                var paramType = this.paramDef.Type.ResolveGeneric(this.GenericContext);
                paramType.IsByRef = this.IsRef || this.IsOut;
                return paramType;
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