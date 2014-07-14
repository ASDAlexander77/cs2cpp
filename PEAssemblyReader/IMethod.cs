// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMethod.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// </summary>
    public interface IMethod : IMember, IMethodBody
    {
        /// <summary>
        /// </summary>
        CallingConventions CallingConvention { get; }

        /// <summary>
        /// </summary>
        string ExplicitName { get; }

        /// <summary>
        /// </summary>
        bool IsConstructor { get; }

        /// <summary>
        /// </summary>
        bool IsGenericMethod { get; }

        /// <summary>
        /// custom field
        /// </summary>
        bool IsInternalCall { get; }

        /// <summary>
        /// custom field
        /// </summary>
        bool IsExternal { get; }

        /// <summary>
        /// </summary>
        IType ReturnType { get; }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IEnumerable<IType> GetGenericArguments();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IMethodBody GetMethodBody();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IEnumerable<IParameter> GetParameters();

        /// <summary>
        /// </summary>
        /// <param name="ownerOfExplicitInterface">
        /// </param>
        /// <returns>
        /// </returns>
        string ToString(IType ownerOfExplicitInterface);
    }
}