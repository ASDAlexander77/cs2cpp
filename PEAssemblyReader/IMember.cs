// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMember.cs" company="">
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
    public interface IMember : IName
    {
        /// <summary>
        /// </summary>
        bool IsAbstract { get; }

        /// <summary>
        /// </summary>
        bool IsOverride { get; }

        /// <summary>
        /// </summary>
        bool IsStatic { get; }

        /// <summary>
        /// </summary>
        bool IsVirtual { get; }

        /// <summary>
        /// </summary>
        IModule Module { get; }
    }
}