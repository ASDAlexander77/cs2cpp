// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IName.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    using System;

    /// <summary>
    /// </summary>
    public interface IName : IComparable
    {
        /// <summary>
        /// </summary>
        string AssemblyQualifiedName { get; }

        /// <summary>
        /// </summary>
        IType DeclaringType { get; }

        /// <summary>
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// </summary>
        string Name { get; }

        /// <summary>
        /// </summary>
        string Namespace { get; }
    }
}