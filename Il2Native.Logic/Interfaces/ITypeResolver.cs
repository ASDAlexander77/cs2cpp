namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;

    using PEAssemblyReader;

    public interface ITypeResolver
    {
        SystemTypes System { get; }

        IIlReader IlReader { get; }

        IDictionary<string, Func<IMethod, IMethod>> MethodsByFullName { get; }

        // TODO: should be in ICodeWriter
        bool GcSupport { get; }

        // TODO: should be in ICodeWriter
        bool GcDebug { get; }

        // TODO: should be in ICodeWriter
        bool MultiThreadingSupport { get; }

        // TODO: should be in ICodeWriter
        bool Unsafe { get; }

        // TODO: should be in ICodeWriter
        string GetAllocator(bool isAtomicAllocation, bool isBigObj, bool debugOrigignalRequired);

        IType ResolveType(string fullTypeName, IGenericContext genericContext = null);

        string GetStaticFieldName(IField field);
    }
}
