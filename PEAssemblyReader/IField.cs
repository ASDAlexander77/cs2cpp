// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IField.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    using System;

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
        object ConstantValue { get; set; }

        /// <summary>
        /// </summary>
        bool IsVolatile { get; }

        /// <summary>
        /// </summary>
        bool IsThreadStatic { get; }

        /// <summary>
        /// </summary>
        bool IsFixed { get; }

        /// <summary>
        /// </summary>
        int FixedSize { get; }

        /// <summary>
        /// </summary>
        bool HasFixedElementField { get; }

        /// <summary>
        /// </summary>
        IField FixedElementField { get; }

        /// <summary>
        /// </summary>
        bool IsVirtualTable { get; }

        /// <summary>
        /// </summary>
        bool IsStaticClassInitialization { get; }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        byte[] GetFieldRVAData();

        /// <summary>
        /// </summary>
        bool IsMerge { get; }
    }
}