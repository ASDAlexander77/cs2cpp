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
        bool IsPublic { get; }
        
        /// <summary>
        /// </summary>
        bool IsInternal { get; }

        /// <summary>
        /// </summary>
        bool IsProtected { get; }

        /// <summary>
        /// </summary>
        bool IsPrivate { get; }

        /// <summary>
        /// </summary>
        IModule Module { get; }
    }
}