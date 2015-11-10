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
            GetHashCodeGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            EqualsGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            MemberwiseCloneGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            ObjectGetTypeGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            
            // Array
            ArrayCopyGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            ArrayClearGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            ArrayGetLengthGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            ArrayGetRankGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            ArrayGetLowerBoundGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            ArrayGetUpperBoundGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            ArrayGetLengthDimGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            ArrayInternalGetReferenceGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            ArrayInternalSetValueGen.Generate(typeResolver).RegisterInto(methodsByFullName);

            // String
            FastAllocateStringGen.Generate(typeResolver).RegisterInto(methodsByFullName);

            // TypedReference
            TypedReferenceInternalToObjectGen.Generate(typeResolver).RegisterInto(methodsByFullName);

            // Jit Helpers
            UnsafeCastGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            UnsafeCastToStackPointerGen.Generate(typeResolver).RegisterInto(methodsByFullName);

            // Runtime helpers
            OffsetToStringDataGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            InitializeArrayGen.Generate(typeResolver).RegisterInto(methodsByFullName);

            // Interlocked
            ExchangeGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            CompareExchangeGen.Generate(typeResolver).RegisterInto(methodsByFullName);

            // AppDomain
            CreateDomainGen.Generate(typeResolver).RegisterInto(methodsByFullName);

            // RuntimeTypeHandler
            IsInterfaceGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            GetBaseTypeGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            GetElementTypeGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            GetGCHandleGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            GetModuleGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            GetAssemblyGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            ConstructNameGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            IsGenericVariableGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            GetCorElementTypeGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            GetInterfacesGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            HasInstantiationGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            IsGenericTypeDefinitionGen.Generate(typeResolver).RegisterInto(methodsByFullName);
            ContainsGenericVariablesGen.Generate(typeResolver).RegisterInto(methodsByFullName);

            // ModuleHandle
            GetModuleTypeGen.Generate(typeResolver).RegisterInto(methodsByFullName);
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