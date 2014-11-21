namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Il2Native.Logic.Gencode.InternalMethods;
    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using PEAssemblyReader;

    public static class MethodBodyBank
    {
        private static IDictionary<string, Func<IMethod, IMethod>> methodsByFullName = new SortedDictionary<string, Func<IMethod, IMethod>>();

        public static IMethod GetMethodBodyOrDefault(IMethod method)
        {
            if (methodsByFullName.Count == 0)
            {
                RegisterAll();
            }

            Func<IMethod, IMethod> methodFactory;
            if (methodsByFullName.TryGetValue(method.FullName, out methodFactory))
            {
                var newMethod = methodFactory.Invoke(method);
                if (newMethod != null)
                {
                    return newMethod;
                }
            }

            return method;
        }

        private static void Register(string methodFullName, Func<IMethod, IMethod> func)
        {
            methodsByFullName[methodFullName] = func;
        }

        private static void RegisterAll()
        {
            MethodBodyBank.Register(GetHashCodeGen.Name, m => GetMethodDecorator(m, GetHashCodeGen.ByteCode));
        }

        private static SynthesizedMethodDecorator GetMethodDecorator(IMethod m, IEnumerable<Code> code)
        {
            return new SynthesizedMethodDecorator(
                m, new SynthesizedMethodBodyDecorator(m.GetMethodBody(), Transform(code).ToArray()), new SynthesizedModuleResolver());
        }

        private static IEnumerable<byte> Transform(IEnumerable<Code> code)
        {
            foreach (var @byte in code.Select(codeItem => (byte)codeItem))
            {
                if (@byte >= 0xE1)
                {
                    yield return 0xFE;
                    yield return (byte)(@byte - 0xE1);
                }
                else
                {
                    yield return @byte;
                }
            }
        }
    }
}
