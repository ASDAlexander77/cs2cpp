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

    using Microsoft.CodeAnalysis;

    /// <summary>
    /// </summary>
    public interface IMethod : IMember
    {
        int? Token { get; }

        /// <summary>
        /// </summary>
        CallingConventions CallingConvention { get; }

        /// <summary>
        /// </summary>
        DllImportData DllImportData { get; }

        /// <summary>
        /// </summary>
        string ExplicitName { get; }

        /// <summary>
        /// </summary>
        bool IsConstructor { get; }

        /// <summary>
        /// </summary>
        bool IsDestructor { get; }

        /// <summary>
        /// custom field
        /// </summary>
        bool IsUnmanagedDllImport { get; }

        /// <summary>
        /// custom field
        /// </summary>
        bool IsExplicitInterfaceImplementation { get; }

        /// <summary>
        /// custom field
        /// </summary>
        IType ExplicitInterface { get; }

        /// <summary>
        /// custom field
        /// </summary>
        bool IsExternal { get; }

        /// <summary>
        /// </summary>
        bool IsGenericMethod { get; }

        /// <summary>
        /// </summary>
        bool IsGenericMethodDefinition { get; }

        /// <summary>
        /// custom field
        /// </summary>
        bool IsUnmanaged { get; }

        /// <summary>
        /// custom field
        /// </summary>
        bool IsUnmanagedMethodReference { get; }

        /// <summary>
        /// custom field
        /// </summary>
        bool IsInline { get; }

        /// <summary>
        /// custom field
        /// </summary>
        bool IsMerge { get; }

        /// custom field
        /// </summary>
        bool IsAnonymousDelegate { get; }

        /// <summary>
        /// custom field
        /// </summary>
        bool HasProceduralBody { get; }

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
        IMethod GetMethodDefinition();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IEnumerable<IParameter> GetParameters();

        /// <summary>
        /// </summary>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        IMethod ToSpecialization(IGenericContext genericContext);

        /// <summary>
        /// </summary>
        /// <param name="ownerOfExplicitInterface">
        /// </param>
        /// <returns>
        /// </returns>
        string ToString(IType ownerOfExplicitInterface, bool shortName = false);
    }
}