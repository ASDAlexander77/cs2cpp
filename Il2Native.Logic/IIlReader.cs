namespace Il2Native.Logic
{
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
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
        ISet<IMethod> CalledMethods { get; set; }

        /// <summary>
        /// </summary>
        ISet<IField> StaticFields { get; set; }

        /// <summary>
        /// </summary>
        ISet<IType> UsedStructTypes { get; set; }

        /// <summary>
        /// </summary>
        ISet<IType> UsedArrayTypes { get; set; }

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
        void AddRtti(IType type);
    }
}
