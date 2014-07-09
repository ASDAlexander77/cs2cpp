// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExceptionHandlingClause.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    using System.Reflection;

    /// <summary>
    /// </summary>
    public interface IExceptionHandlingClause
    {
        /// <summary>
        /// </summary>
        IType CatchType { get; }

        /// <summary>
        /// </summary>
        ExceptionHandlingClauseOptions Flags { get; }

        /// <summary>
        /// </summary>
        int HandlerLength { get; }

        /// <summary>
        /// </summary>
        int HandlerOffset { get; }

        /// <summary>
        /// </summary>
        int TryLength { get; }

        /// <summary>
        /// </summary>
        int TryOffset { get; }
    }
}