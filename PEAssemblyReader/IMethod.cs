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
    using Microsoft.CodeAnalysis;
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
        /// custom field
        /// </summary>
        bool IsExternal { get; }

        /// <summary>
        /// </summary>
        bool IsGenericMethod { get; }

        /// <summary>
        /// custom field
        /// </summary>
        bool IsUnmanaged { get; }

        /// <summary>
        /// custom field
        /// </summary>
        bool IsDllImport { get; }

        /// <summary>
        /// </summary>
        DllImportData DllImportData { get; }

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
        IEnumerable<IType> GetGenericParameters();

        /// <summary>
        /// </summary>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        IMethodBody GetMethodBody(IGenericContext genericContext = null);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IEnumerable<IParameter> GetParameters();

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        IType ResolveTypeParameter(IType type);

        /// <summary>
        /// </summary>
        /// <param name="ownerOfExplicitInterface">
        /// </param>
        /// <returns>
        /// </returns>
        string ToString(IType ownerOfExplicitInterface);
    }
}