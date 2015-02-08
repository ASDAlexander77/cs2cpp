namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using InternalMethods;
    using PEAssemblyReader;
    using SynthesizedMethods;

    public static class MethodBodyBank
    {
        private static readonly IDictionary<string, Func<IMethod, IMethod>> MethodsByFullName =
            new SortedDictionary<string, Func<IMethod, IMethod>>();

        public static IMethod GetMethodBodyOrDefault(IMethod method, ITypeResolver typeResolver)
        {
            if (MethodsByFullName.Count == 0)
            {
                RegisterAll(typeResolver);
            }

            Func<IMethod, IMethod> methodFactory;
            if (MethodsByFullName.TryGetValue(method.ToString(), out methodFactory))
            {
                var newMethod = methodFactory.Invoke(method);
                if (newMethod != null)
                {
                    return newMethod;
                }
            }

            // dynamiclly generated method for MulticastDelegate
            if (method.IsDelegateFunctionBody()
                && (method.Name == "Invoke")
                && (method.DeclaringType.BaseType.FullName == "System.MulticastDelegate"))
            {
                object[] code;
                IList<object> tokenResolutions;
                IList<IType> locals;
                IList<IParameter> parameters;
                DelegateGen.GetMulticastDelegateInvoke(
                    method,
                    typeResolver,
                    out code,
                    out tokenResolutions,
                    out locals,
                    out parameters);
                return GetMethodDecorator(method, code, tokenResolutions, locals, parameters);
            }

            return method;
        }

        public static SynthesizedMethodDecorator GetMethodDecorator(
            IMethod m,
            IEnumerable<object> code,
            IList<object> tokenResolutions,
            IList<IType> locals,
            IList<IParameter> parameters)
        {
            return new SynthesizedMethodDecorator(
                m,
                new SynthesizedMethodBodyDecorator(m != null ? m.GetMethodBody() : null, locals, Transform(code).ToArray()),
                parameters,
                new SynthesizedModuleResolver(m, tokenResolutions));
        }

        public static void Register(
            string methodFullName,
            object[] code,
            IList<object> tokenResolutions,
            IList<IType> locals,
            IList<IParameter> parameters)
        {
            Register(methodFullName, m => GetMethodDecorator(m, code, tokenResolutions, locals, parameters));
        }

        private static void Register(string methodFullName, Func<IMethod, IMethod> func)
        {
            MethodsByFullName[methodFullName] = func;
        }

        private static void RegisterAll(ITypeResolver typeResolver)
        {
            // Object
            GetHashCodeGen.Register(typeResolver);
            EqualsGen.Register(typeResolver);
            MemberwiseCloneGen.Register(typeResolver);
            ObjectGetTypeGen.Register(typeResolver);
            
            // Array
            ArrayCopyGen.Register(typeResolver);
            ArrayClearGen.Register(typeResolver);
            ArrayGetLengthGen.Register(typeResolver);
            ArrayGetRankGen.Register(typeResolver);

#if MSCORLIB
            UnsafeCastToStackPointerGen.Register(typeResolver);
#endif
        }

        public static IEnumerable<byte> Transform(IEnumerable<object> code)
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
                    var @int = Convert.ToInt32(codeItem);
                    if (@int > 0)
                    {
                        yield return (byte)@int;
                    }
                    else
                    {
                        yield return (byte)(sbyte)@int;
                    }
                }
            }
        }
    }
}