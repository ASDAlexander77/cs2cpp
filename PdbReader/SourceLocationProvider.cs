// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="SourceLocationProvider.cs">
//   
// </copyright>
// <summary>
//   
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace Microsoft.Cci
{
    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    internal sealed class UsedNamespace : IUsedNamespace
    {
        /// <summary>
        /// </summary>
        private readonly IName alias;

        /// <summary>
        /// </summary>
        private readonly IName namespaceName;

        /// <summary>
        /// </summary>
        /// <param name="alias">
        /// </param>
        /// <param name="namespaceName">
        /// </param>
        internal UsedNamespace(IName alias, IName namespaceName)
        {
            this.alias = alias;
            this.namespaceName = namespaceName;
        }

        /// <summary>
        /// </summary>
        public IName Alias
        {
            get
            {
                return this.alias;
            }
        }

        /// <summary>
        /// </summary>
        public IName NamespaceName
        {
            get
            {
                return this.namespaceName;
            }
        }
    }

    /// <summary>
    /// </summary>
    internal class NamespaceScope : INamespaceScope
    {
        /// <summary>
        /// </summary>
        private readonly IEnumerable<IUsedNamespace> usedNamespaces;

        /// <summary>
        /// </summary>
        /// <param name="usedNamespaces">
        /// </param>
        internal NamespaceScope(IEnumerable<IUsedNamespace> usedNamespaces)
        {
            this.usedNamespaces = usedNamespaces;
        }

        /// <summary>
        /// </summary>
        public IEnumerable<IUsedNamespace> UsedNamespaces
        {
            get
            {
                return this.usedNamespaces;
            }
        }
    }

    /// <summary>
    /// </summary>
    internal sealed class PdbIteratorScope : ILocalScope
    {
        /// <summary>
        /// </summary>
        private readonly uint length;

        /// <summary>
        /// </summary>
        private readonly uint offset;

        /// <summary>
        /// </summary>
        /// <param name="offset">
        /// </param>
        /// <param name="length">
        /// </param>
        internal PdbIteratorScope(uint offset, uint length)
        {
            this.offset = offset;
            this.length = length;
        }

        /// <summary>
        /// </summary>
        public uint Length
        {
            get
            {
                return this.length;
            }
        }

        /// <summary>
        /// </summary>
        public uint Offset
        {
            get
            {
                return this.offset;
            }
        }
    }
}