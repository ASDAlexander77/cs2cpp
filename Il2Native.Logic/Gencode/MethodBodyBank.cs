namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Resources;
    using InternalMethods;
    using InternalMethods.ModuleHandle;
    using InternalMethods.RuntimeTypeHandler;
    using PEAssemblyReader;
    using SynthesizedMethods;

    public static class MethodBodyBank
    {
        public static IMethod GetMethodWithCustomBodyOrDefault(IMethod method, ITypeResolver typeResolver)
        {
            if (method == null)
            {
                return null;
            }

            Func<IMethod, IMethod> methodFactory;
            if (typeResolver.MethodsByFullName.TryGetValue(method.ToString(), out methodFactory))
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
        public static Tuple<string, Func<IMethod, IMethod>> Register(
            string methodFullName,
            object[] code,
            IList<object> tokenResolutions,
            IList<IType> locals,
            IList<IParameter> parameters)
        {
            return new Tuple<string, Func<IMethod, IMethod>>(methodFullName, m => GetMethodDecorator(m, code, tokenResolutions, locals, parameters));
        }

        public static Tuple<string, Func<IMethod, IMethod>> Register(
            string methodFullName,
            byte[] code,
            IList<object> tokenResolutions,
            IList<IType> locals,
            IList<IParameter> parameters,
            IExceptionHandlingClause[] exceptionHandlingClause = null)
        {
            return new Tuple<string, Func<IMethod, IMethod>>(
                methodFullName,
                m => GetMethodDecorator(m, code, tokenResolutions, locals, parameters, exceptionHandlingClause ?? new IExceptionHandlingClause[0]));
        }

        public static void RegisterAll(IDictionary<string, Func<IMethod, IMethod>> methodsByFullName)
        {
        }

        public static void RegisterInto(this IEnumerable<Tuple<string, Func<IMethod, IMethod>>> methods, IDictionary<string, Func<IMethod, IMethod>> methodsByFullName)
        {
            foreach (var method in methods)
            {
                methodsByFullName[method.Item1] = method.Item2;
            }    
        }

        public static void RegisterAll(ITypeResolver typeResolver, IDictionary<string, Func<IMethod, IMethod>> methodsByFullName)
        {
            // Object
            GetHashCodeGen.Register(typeResolver).RegisterInto(methodsByFullName);
            EqualsGen.Register(typeResolver).RegisterInto(methodsByFullName);
            MemberwiseCloneGen.Register(typeResolver).RegisterInto(methodsByFullName);
            ObjectGetTypeGen.Register(typeResolver).RegisterInto(methodsByFullName);
            
            // Array
            ArrayCopyGen.Register(typeResolver).RegisterInto(methodsByFullName);
            ArrayClearGen.Register(typeResolver).RegisterInto(methodsByFullName);
            ArrayGetLengthGen.Register(typeResolver).RegisterInto(methodsByFullName);
            ArrayGetRankGen.Register(typeResolver).RegisterInto(methodsByFullName);
            ArrayGetLowerBoundGen.Register(typeResolver).RegisterInto(methodsByFullName);
            ArrayGetUpperBoundGen.Register(typeResolver).RegisterInto(methodsByFullName);
            ArrayGetLengthDimGen.Register(typeResolver).RegisterInto(methodsByFullName);
            ArrayInternalGetReferenceGen.Register(typeResolver).RegisterInto(methodsByFullName);
            ArrayInternalSetValueGen.Register(typeResolver).RegisterInto(methodsByFullName);

            // String
            FastAllocateStringGen.Register(typeResolver).RegisterInto(methodsByFullName);

            // TypedReference
            TypedReferenceInternalToObjectGen.Register(typeResolver).RegisterInto(methodsByFullName);

            // Jit Helpers
            UnsafeCastGen.Register(typeResolver).RegisterInto(methodsByFullName);
            UnsafeCastToStackPointerGen.Register(typeResolver).RegisterInto(methodsByFullName);

            // Runtime helpers
            OffsetToStringDataGen.Register(typeResolver).RegisterInto(methodsByFullName);
            InitializeArrayGen.Register(typeResolver).RegisterInto(methodsByFullName);

            // Interlocked
            ExchangeGen.Register(typeResolver).RegisterInto(methodsByFullName);
            CompareExchangeGen.Register(typeResolver).RegisterInto(methodsByFullName);

            // AppDomain
            CreateDomainGen.Register(typeResolver).RegisterInto(methodsByFullName);

            // RuntimeTypeHandler
            IsInterfaceGen.Register(typeResolver).RegisterInto(methodsByFullName);
            GetBaseTypeGen.Register(typeResolver).RegisterInto(methodsByFullName);
            GetElementTypeGen.Register(typeResolver).RegisterInto(methodsByFullName);
            GetGCHandleGen.Register(typeResolver).RegisterInto(methodsByFullName);
            GetModuleGen.Register(typeResolver).RegisterInto(methodsByFullName);
            GetAssemblyGen.Register(typeResolver).RegisterInto(methodsByFullName);
            ConstructNameGen.Register(typeResolver).RegisterInto(methodsByFullName);
            IsGenericVariableGen.Register(typeResolver).RegisterInto(methodsByFullName);
            GetCorElementTypeGen.Register(typeResolver).RegisterInto(methodsByFullName);
            GetInterfacesGen.Register(typeResolver).RegisterInto(methodsByFullName);
            HasInstantiationGen.Register(typeResolver).RegisterInto(methodsByFullName);
            IsGenericTypeDefinitionGen.Register(typeResolver).RegisterInto(methodsByFullName);
            ContainsGenericVariablesGen.Register(typeResolver).RegisterInto(methodsByFullName);

            // ModuleHandle
            GetModuleTypeGen.Register(typeResolver).RegisterInto(methodsByFullName);
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