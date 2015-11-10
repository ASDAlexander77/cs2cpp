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
    using System;
    using System.Collections.Generic;
    using CodeParts;
    using PEAssemblyReader;

    public interface ICodeWriter : ITypeResolver
    {
        IIlReader IlReader { get; set; }

        IDictionary<string, Func<IMethod, IMethod>> MethodsByFullName { get; }

        bool GcSupport { get; }

        bool GcDebug { get; }

        bool MultiThreadingSupport { get; }

        bool Unsafe { get; }

        string GetAllocator(bool isAtomicAllocation, bool isBigObj, bool debugOrigignalRequired);

        string GetStaticFieldName(IField field);
    }

    /// <summary>
    /// </summary>
    public interface ICodeWriterEx : ICodeWriter
    {
        bool IsHeader { get; set; }

        bool IsSplit { get; set; }

        string SplitNamespace { get; set; }

        string FileHeader { get; set; }

        /// <summary>
        /// </summary>
        void Initialize();

        /// <summary>
        /// </summary>
        void Close();

        /// <summary>
        /// </summary>
        /// <param name="rawText">
        /// </param>
        void WriteRawText(string rawText);

        /// <summary>
        /// </summary>
        void WriteInheritance();

        /// <summary>
        /// </summary>
        void WriteAfterFields();

        /// <summary>
        /// </summary>
        void WriteBeforeFields();

        /// <summary>
        /// </summary>
        void WriteAfterMethods(IType type);

        /// <summary>
        /// </summary>
        void WriteBeforeMethods(IType type);


        /// <summary>
        /// </summary>
        void WriteEnd();

        /// <summary>
        /// </summary>
        /// <param name="field">
        /// </param>
        void WriteField(IField field);

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
        void WritePreDeclarations(IType type);

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        void WritePostDeclarations(IType type);

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        void WritePreDefinitions(IType type);

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        void WritePostDefinitions(IType type);

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
        void WriteStart();

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        void WriteTypeStart(IType type, IGenericContext genericContext);

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        void WriteTypeEnd(IType type);

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericContext"></param>
        void WriteForwardTypeDeclaration(IType type, IGenericContext genericContext);

        /// <summary>
        /// </summary>
        /// <param name="methodDecl"></param>
        /// <param name="ownerOfExplicitInterface"></param>
        void WriteMethodForwardDeclaration(IMethod methodDecl, IType ownerOfExplicitInterface, IGenericContext genericContext);

        /// <summary>
        /// </summary>
        void WriteStaticField(IField field, bool definition = true, IType typeForRuntimeTypeInfo = null);

        /// <summary>
        /// </summary>
        void WriteVirtualTableImplementations(IType type, bool declaration = false);

        /// <summary>
        /// </summary>
        IMethod GenerateMainMethod(IMethod mainMethod);

        /// <summary>
        /// </summary>
        IEnumerable<IMethod> VirtualTableImplementations(IType type);
    }
}