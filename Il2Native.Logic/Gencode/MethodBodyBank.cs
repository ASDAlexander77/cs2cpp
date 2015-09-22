namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using InternalMethods;
    using InternalMethods.RuntimeTypeHandler;
    using PEAssemblyReader;
    using SynthesizedMethods;

    public static class MethodBodyBank
    {
        private static readonly IDictionary<string, Func<IMethod, IMethod>> MethodsByFullName =
            new SortedDictionary<string, Func<IMethod, IMethod>>();

        private static readonly object Locker = new object();

        public static void Clear()
        {
            MethodsByFullName.Clear();
        }

        public static IMethod GetMethodWithCustomBodyOrDefault(IMethod method, ITypeResolver typeResolver)
        {
            if (MethodsByFullName.Count == 0)
            {
                lock (Locker)
                {
                    // we double check to filter threads waiting on 'lock'
                    if (MethodsByFullName.Count == 0)
                    {
                        RegisterAll(typeResolver);
                    }
                }
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
            if (method.IsDelegateFunctionBody())
            {
                if (method.Name == ".ctor" && method.DeclaringType.FullName != "System.Delegate")
                {
                    return typeResolver.GetDelegateConstructorMethod(method.DeclaringType).GetMethod(method);
                }

                if (method.Name == "Invoke")
                {
                    if (method.DeclaringType.BaseType.FullName == "System.Delegate")
                    {
                        return typeResolver.GetDelegateInvokeMethod(method).GetMethod(method);
                    }

                    if (method.DeclaringType.BaseType.FullName == "System.MulticastDelegate")
                    {
                        return typeResolver.GetMulticastDelegateInvoke(method).GetMethod(method);
                    }
                }

                if (method.Name == "BeginInvoke")
                {
                    return typeResolver.GetDelegateBeginInvokeMethod(method).GetMethod(method);
                }

                if (method.Name == "EndInvoke")
                {
                    return typeResolver.GetDelegateEndInvokeMethod(method).GetMethod(method);
                }
            }

            return method;
        }

        [Obsolete]
        public static SynthesizedMethodDecorator GetMethodDecorator(
            IMethod m,
            IEnumerable<object> code,
            IList<object> tokenResolutions,
            IList<IType> locals,
            IList<IParameter> parameters)
        {
            return new SynthesizedMethodDecorator(
                m,
                new SynthesizedMethodBodyDecorator(m != null ? m.GetMethodBody() : null, locals, null, Transform(code).ToArray()),
                parameters,
                new SynthesizedModuleResolver(m, tokenResolutions));
        }

        public static SynthesizedMethodDecorator GetMethodDecorator(
            IMethod m,
            byte[] code,
            IList<object> tokenResolutions,
            IList<IType> locals,
            IList<IParameter> parameters,
            IEnumerable<IExceptionHandlingClause> exceptionHandlingClauses)
        {
            return new SynthesizedMethodDecorator(
                m,
                new SynthesizedMethodBodyDecorator(m != null ? m.GetMethodBody() : null, locals, exceptionHandlingClauses, code),
                parameters,
                new SynthesizedModuleResolver(m, tokenResolutions));
        }

        [Obsolete]
        public static void Register(
            string methodFullName,
            object[] code,
            IList<object> tokenResolutions,
            IList<IType> locals,
            IList<IParameter> parameters)
        {
            Register(methodFullName, m => GetMethodDecorator(m, code, tokenResolutions, locals, parameters));
        }

        public static void Register(
            string methodFullName,
            byte[] code,
            IList<object> tokenResolutions,
            IList<IType> locals,
            IList<IParameter> parameters)
        {
            Register(methodFullName, m => GetMethodDecorator(
                m, code, tokenResolutions, locals, parameters, new IExceptionHandlingClause[0]));
        }

        public static void Register(string methodFullName, Func<IMethod, IMethod> func)
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
            ArrayGetLowerBoundGen.Register(typeResolver);
            ArrayGetUpperBoundGen.Register(typeResolver);
            ArrayGetLengthDimGen.Register(typeResolver);
            ArrayInternalGetReferenceGen.Register(typeResolver);
            ArrayInternalSetValueGen.Register(typeResolver);

            // String
            FastAllocateStringGen.Register(typeResolver);

            // TypedReference
            TypedReferenceInternalToObjectGen.Register(typeResolver);

            // Jit Helpers
            UnsafeCastGen.Register(typeResolver);
            UnsafeCastToStackPointerGen.Register(typeResolver);

            // Runtime helpers
            OffsetToStringDataGen.Register(typeResolver);
            InitializeArrayGen.Register(typeResolver);

            // Interlocked
            ExchangeGen.Register(typeResolver);
            CompareExchangeGen.Register(typeResolver);

            // AppDomain
            CreateDomainGen.Register(typeResolver);

            // RuntimeTypeHandler
            IsInterfaceGen.Register(typeResolver);
            GetBaseTypeGen.Register(typeResolver);
            GetGCHandleGen.Register(typeResolver);
            GetModuleGen.Register(typeResolver);
            ConstructNameGen.Register(typeResolver);
        }

        [Obsolete]
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