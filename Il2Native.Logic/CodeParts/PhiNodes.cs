// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhiNodes.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.CodeParts
{
    using System.Collections.Generic;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class PhiNodes
    {
        /// <summary>
        /// </summary>
        public PhiNodes()
        {
            this.Values = new List<OpCodePart>();
            this.Labels = new List<int>();
        }

        /// <summary>
        /// </summary>
        public List<int> Labels { get; private set; }

        /// <summary>
        /// </summary>
        public List<OpCodePart> Values { get; private set; }

        /// <summary>
        /// </summary>
        public IType RequiredOutgoingType { get; set; }

        /// <summary>
        /// </summary>
        public PhiNodes UsedByAlternativeValues { get; set; }
    }
}