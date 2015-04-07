// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICodeWriter.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic
{
    using System.Collections.Generic;
    using CodeParts;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public interface ICodeWriter : ITypeResolver
    {
        /// <summary>
        /// </summary>
        void Initialize(IType type);

        /// <summary>
        /// </summary>
        void Close();

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        bool IsTypeDefinitionWritten(IType type);

        /// <summary>
        /// </summary>
        /// <param name="rawText">
        /// </param>
        void WriteRawText(string rawText);

        /// <summary>
        /// </summary>
        /// <param name="count">
        /// </param>
        void WriteAfterFields(int count);

        /// <summary>
        /// </summary>
        /// <param name="count">
        /// </param>
        void WriteBeforeFields(int count);

        /// <summary>
        /// </summary>
        void WriteEnd();

        /// <summary>
        /// </summary>
        /// <param name="field">
        /// </param>
        /// <param name="number">
        /// </param>
        /// <param name="count">
        /// </param>
        void WriteFieldEnd(IField field, int number, int count);

        /// <summary>
        /// </summary>
        /// <param name="field">
        /// </param>
        /// <param name="number">
        /// </param>
        /// <param name="count">
        /// </param>
        void WriteFieldStart(IField field, int number, int count);

        /// <summary>
        /// </summary>
        /// <param name="fieldType">
        /// </param>
        void WriteFieldType(IType fieldType);

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        void WriteMethod(IMethod method, IMethod methodOpCodeHolder, IGenericContext genericContext);

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        void WritePostDeclarationsAndInternalDefinitions(IType type, bool staticOnly = false);

        /// <summary>
        /// </summary>
        /// <param name="moduleName">
        /// </param>
        /// <param name="assemblyName">
        /// </param>
        /// <param name="isCoreLib">
        /// </param>
        /// <param name="allReference">
        /// </param>
        /// ///
        void WriteStart(IIlReader ilReader);

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        void WriteTypeEnd(IType type);

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        void WriteTypeStart(IType type, IGenericContext genericContext);
    }
}