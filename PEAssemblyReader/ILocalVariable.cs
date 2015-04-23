// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILocalVariable.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    /// <summary>
    /// </summary>
    public interface ILocalVariable
    {
        /// <summary>
        /// </summary>
        int LocalIndex { get; }

        /// <summary>
        /// </summary>
        IType LocalType { get; }

        /// <summary>
        /// </summary>
        string Name { get; }
    }
}