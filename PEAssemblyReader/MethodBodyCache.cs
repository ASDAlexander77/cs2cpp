namespace PEAssemblyReader
{
    using System.Collections.Generic;
    using System.Reflection.Metadata;
    using System.Reflection.Metadata.Ecma335;

    public static class MethodBodyCache
    {
        private static IDictionary<string, IDictionary<int, MethodBodyBlock>> methodBodyByAssemblyModuleAndMethod = new SortedDictionary<string, IDictionary<int, MethodBodyBlock>>();

        public static bool TryGet(string assemblyHandle, Handle methodHandle, out MethodBodyBlock methodBody)
        {
            var assemblyNum = assemblyHandle;
            var methodBodyNum = MetadataTokens.GetRowNumber(methodHandle);

            IDictionary<int, MethodBodyBlock> methodBodyByMethod;
            if (methodBodyByAssemblyModuleAndMethod.TryGetValue(assemblyNum, out methodBodyByMethod))
            {
                if (methodBodyByMethod.TryGetValue(methodBodyNum, out methodBody))
                {
                    return true;
                }
            }

            methodBody = null;
            return false;
        }

        public static bool Register(string assemblyHandle, Handle methodHandle, MethodBodyBlock methodBody)
        {
            var assemblyNum = assemblyHandle;
            var methodNum = MetadataTokens.GetRowNumber(methodHandle);

            IDictionary<int, MethodBodyBlock> methodBodyByMethod;
            lock (methodBodyByAssemblyModuleAndMethod)
            {
                if (!methodBodyByAssemblyModuleAndMethod.TryGetValue(assemblyNum, out methodBodyByMethod))
                {
                    methodBodyByAssemblyModuleAndMethod[assemblyNum] =
                        methodBodyByMethod = new SortedDictionary<int, MethodBodyBlock>();
                }
            }

            lock (methodBodyByMethod)
            {
                MethodBodyBlock currentMethodBody;
                if (!methodBodyByMethod.TryGetValue(methodNum, out currentMethodBody))
                {
                    methodBodyByMethod[methodNum] = methodBody;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
