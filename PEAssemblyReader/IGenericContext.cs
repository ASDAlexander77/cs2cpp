// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGenericContext.cs" company="">
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
    public interface IGenericContext
    {
        /// <summary>
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// </summary>
        bool IsCustom { get; }

        /// <summary>
        /// </summary>
        IDictionary<IType, IType> Map { get; }

        /// <summary>
        /// </summary>
        IDictionary<string, IType> CustomMap { get; }

        /// <summary>
        /// </summary>
        IMethod MethodDefinition { get; }

        /// <summary>
        /// </summary>
        IMethod MethodSpecialization { get; }

        /// <summary>
        /// </summary>
        IType TypeDefinition { get; }

        /// <summary>
        /// </summary>
        IType TypeSpecialization { get; }

        /// <summary>
        /// </summary>
        /// <param name="typeParameter">
        /// </param>
        /// <returns>
        /// </returns>
        IType ResolveTypeParameter(IType typeParameter);

        /// <summary>
        /// </summary>
        IGenericContext Clone();

        /// <summary>
        /// </summary>
        void AppendMap(IGenericContext genericContext);
    }
}