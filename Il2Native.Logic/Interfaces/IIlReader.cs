namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;

    using Il2Native.Logic.CodeParts;

    using PEAssemblyReader;

    public interface IIlReader : ITypeResolver
    {
        /// <summary>
        /// </summary>
        string AssemblyQualifiedName { get; }

        /// <summary>
        /// </summary>
        bool IsCoreLib { get; }

        /// <summary>
        /// </summary>
        bool CompactMode { get; }

        /// <summary>
        /// </summary>
        string ModuleName { get; }

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
        IEnumerable<IType> CompileSourceWithRoslyn(string assemblyName, params string[] source);

        /// <summary>
        /// </summary>
        IEnumerable<OpCodePart> OpCodes(IMethod method, IGenericContext genericContext, Queue<IMethod> stackCall = null);
    }
}
