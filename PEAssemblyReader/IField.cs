// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IField.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
namespace PEAssemblyReader
{
    /// <summary>
    /// </summary>
    public interface IField : IMember, IEquatable<IField>
    {
        /// <summary>
        /// </summary>
        IType FieldType { get; }

        /// <summary>
        /// </summary>
        bool IsConst { get; }

        /// <summary>
        /// </summary>
        byte[] GetFieldRVAData();
    }
}