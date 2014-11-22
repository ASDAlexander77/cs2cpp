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

        public static IMethod GetMethodBodyOrDefault(IMethod method, ICodeWriter codeWriter)
        {
            if (methodsByFullName.Count == 0)
            {
                RegisterAll(codeWriter);
            }

            Func<IMethod, IMethod> methodFactory;
            if (methodsByFullName.TryGetValue(method.ToString(), out methodFactory))
            {
                var newMethod = methodFactory.Invoke(method);
                if (newMethod != null)
                {
                    return newMethod;
                }
            }

            return method;
        }

        public static void Register(string methodFullName, object[] code, IList<object> tokenResolutions, IList<IType> locals, IList<IType> parameters)
        {
            MethodBodyBank.Register(methodFullName, m => MethodBodyBank.GetMethodDecorator(m, code, tokenResolutions, locals, parameters));
        }

        private static void Register(string methodFullName, Func<IMethod, IMethod> func)
        {
            methodsByFullName[methodFullName] = func;
        }

        private static void RegisterAll(ICodeWriter codeWriter)
        {
            GetHashCodeGen.Register(codeWriter);
            EqualsGen.Register(codeWriter);
            MemberwiseCloneGen.Register(codeWriter);
        }

        private static SynthesizedMethodDecorator GetMethodDecorator(IMethod m, IEnumerable<object> code, IList<object> tokenResolutions, IList<IType> locals, IList<IType> parameters)
        {
            return new SynthesizedMethodDecorator(
                m, 
                new SynthesizedMethodBodyDecorator(m.GetMethodBody(), locals, Transform(code).ToArray()),
                parameters,
                new SynthesizedModuleResolver(m, tokenResolutions));
        }

        private static IEnumerable<byte> Transform(IEnumerable<object> code)
        {
            foreach (var codeItem in code)
            {
                if (codeItem is Code)
                {
                    var @byte = (byte)(Code)codeItem;
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
                else
                {
                    yield return (byte) (int) codeItem;
                }
            }
        }
    }
}
