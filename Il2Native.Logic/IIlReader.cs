namespace Il2Native.Logic
{
    using System.Collections.Generic;

    using Il2Native.Logic.CodeParts;

    using PEAssemblyReader;

    public interface IIlReader
    {
        /// <summary>
        /// </summary>
        string AssemblyQualifiedName { get; }

        /// <summary>
        /// </summary>
        bool IsCoreLib { get; }

        /// <summary>
        /// </summary>
        string ModuleName { get; }

        /// <summary>
        /// </summary>
        ISet<MethodKey> CalledMethods { get; set; }

        /// <summary>
        /// </summary>
        ISet<IField> StaticFields { get; set; }

        /// <summary>
        /// </summary>
        ISet<IType> UsedTypeDefinitions { get; set; }

        /// <summary>
        /// </summary>
        ISet<IType> UsedTypeDeclarations { get; set; }

        /// <summary>
        /// </summary>
        ISet<IType> UsedArrayTypes { get; set; }

        /// <summary>
        /// </summary>
        ISet<IType> UsedTypeTokens { get; set; }

        /// <summary>
        /// </summary>
        ISet<IType> UsedVirtualTables { get; set; }

        /// <summary>
        /// </summary>
        ISet<IType> UsedRtti { get; set; }

        /// <summary>
        /// </summary>
        IDictionary<int, string> UsedStrings { get; set; }

        /// <summary>
        /// </summary>
        IList<IConstBytes> UsedConstBytes { get; set; }

        /// <summary>
        /// </summary>
        IList<IMethod> StaticConstructors { get; set; }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IEnumerable<string> References();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IEnumerable<string> AllReferences();

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IEnumerable<IType> CompileSourceWithRoslyn(params string[] source);

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        void AddArrayType(IType type);

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        void AddTypeToken(IType type);

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        void AddRtti(IType type);

        /// <summary>
        /// </summary>
        void AddCalledMethod(IMethod method, IType ownerOfExplicitInterface = null);

        /// <summary>
        /// </summary>
        void AddUsedTypeDefinition(IType type);

        /// <summary>
        /// </summary>
        void AddVirtualTable(IType type);

        /// <summary>
        /// </summary>
        void AddStaticField(IField field);

        /// <summary>
        /// </summary>
        IEnumerable<OpCodePart> OpCodes(IMethod method, IGenericContext genericContext, Queue<IMethod> stackCall = null);
    }
}
