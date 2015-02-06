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
        void Close();

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        void DisableWrite(bool value);

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        bool IsProcessed(IType type);

        /// <summary>
        /// </summary>
        /// <param name="rawText">
        /// </param>
        void Write(string rawText);

        /// <summary>
        /// </summary>
        /// <param name="ilCode">
        /// </param>
        void Write(OpCodePart ilCode);

        /// <summary>
        /// </summary>
        void WriteAfterConstructors();

        /// <summary>
        /// </summary>
        /// <param name="count">
        /// </param>
        void WriteAfterFields(int count);

        /// <summary>
        /// </summary>
        void WriteAfterMethods();

        /// <summary>
        /// </summary>
        void WriteBeforeConstructors();

        /// <summary>
        /// </summary>
        /// <param name="count">
        /// </param>
        void WriteBeforeFields(int count);

        /// <summary>
        /// </summary>
        void WriteBeforeMethods();

        /// <summary>
        /// </summary>
        /// <param name="ctor">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        void WriteConstructorEnd(IConstructor ctor, IGenericContext genericContext);

        /// <summary>
        /// </summary>
        /// <param name="ctor">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        void WriteConstructorStart(IConstructor ctor, IGenericContext genericContext);

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
        /// <param name="type">
        /// </param>
        /// <param name="number">
        /// </param>
        /// <param name="count">
        /// </param>
        void WriteForwardDeclaration(IType type, int number, int count);

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        void WriteMethodEnd(IMethod method, IGenericContext genericContext);

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        void WriteMethodStart(IMethod method, IGenericContext genericContext, bool linkOnceOdr = false, bool noLocalVars = false);

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        void WritePostDeclarationsAndInternalDefinitions(IType type, bool staticOnly = false);

        /// <summary>
        /// </summary>
        void WriteRequiredTypesForBody();

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
        void WriteStoredText();

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