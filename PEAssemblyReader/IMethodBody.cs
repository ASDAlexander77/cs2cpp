// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMethodBody.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    public interface IMethodBody
    {
        /// <summary>
        /// </summary>
        IEnumerable<IExceptionHandlingClause> ExceptionHandlingClauses { get; }

        /// <summary>
        /// </summary>
        IEnumerable<ILocalVariable> LocalVariables { get; }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        byte[] GetILAsByteArray();
    }
}