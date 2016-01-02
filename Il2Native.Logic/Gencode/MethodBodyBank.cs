namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Resources;
    using InternalMethods;
    using InternalMethods.ModuleHandle;
    using InternalMethods.RuntimeAssembly;
    using InternalMethods.RuntimeTypeHandler;
    using PEAssemblyReader;
    using SynthesizedMethods;

    public static class MethodBodyBank
    {
        public static IMethod GetMethodWithCustomBodyOrDefault(IMethod method, ICodeWriter codeWriter)
        {
            if (method == null)
            {
                return null;
            }

            Func<IMethod, IMethod> methodFactory;
            if (codeWriter.MethodsByFullName.TryGetValue(method.ToString(), out methodFactory))
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
                    return DelegateGen.GetDelegateConstructorMethod(codeWriter, method.DeclaringType).GetMethod(method);
                }

                if (method.Name == "Invoke")
                {
                    if (method.DeclaringType.BaseType.FullName == "System.Delegate")
                    {
                        return DelegateGen.GetDelegateInvokeMethod(codeWriter, method).GetMethod(method);
                    }

                    if (method.DeclaringType.BaseType.FullName == "System.MulticastDelegate")
                    {
                        return DelegateGen.GetMulticastDelegateInvoke(codeWriter, method).GetMethod(method);
                    }
                }

                if (method.Name == "BeginInvoke")
                {
                    return DelegateGen.GetDelegateBeginInvokeMethod(codeWriter, method).GetMethod(method);
                }

                if (method.Name == "EndInvoke")
                {
                    return DelegateGen.GetDelegateEndInvokeMethod(codeWriter, method).GetMethod(method);
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

        public static void RegisterAll(ICodeWriter codeWriter, IDictionary<string, Func<IMethod, IMethod>> methodsByFullName)
        {
            // Object
            GetHashCodeGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            EqualsGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            MemberwiseCloneGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            ObjectGetTypeGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            
            // Array
            ArrayCopyGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            ArrayClearGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            ArrayGetLengthGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            ArrayGetRankGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            ArrayGetLowerBoundGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            ArrayGetUpperBoundGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            ArrayGetLengthDimGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            ArrayInternalGetReferenceGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            ArrayInternalSetValueGen.Generate(codeWriter).RegisterInto(methodsByFullName);

            // String
            FastAllocateStringGen.Generate(codeWriter).RegisterInto(methodsByFullName);

            // TypedReference
            TypedReferenceInternalToObjectGen.Generate(codeWriter).RegisterInto(methodsByFullName);

            // Jit Helpers
            UnsafeCastGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            UnsafeCastToStackPointerGen.Generate(codeWriter).RegisterInto(methodsByFullName);

            // Runtime helpers
            OffsetToStringDataGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            InitializeArrayGen.Generate(codeWriter).RegisterInto(methodsByFullName);

            // Interlocked
            ExchangeGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            CompareExchangeGen.Generate(codeWriter).RegisterInto(methodsByFullName);

            // AppDomain
            CreateDomainGen.Generate(codeWriter).RegisterInto(methodsByFullName);

            // RuntimeTypeHandler
            IsInterfaceGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            GetBaseTypeGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            GetElementTypeGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            GetGCHandleGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            GetModuleGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            GetAssemblyGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            ConstructNameGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            IsGenericVariableGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            GetCorElementTypeGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            GetInterfacesGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            HasInstantiationGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            IsGenericTypeDefinitionGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            ContainsGenericVariablesGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            GetTokenGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            IsSecurityTransparentGen.Generate(codeWriter).RegisterInto(methodsByFullName);

            // ModuleHandle
            GetModuleTypeGen.Generate(codeWriter).RegisterInto(methodsByFullName);
            _GetMetadataImportGen.Generate(codeWriter).RegisterInto(methodsByFullName);

            // RuntimeAssembly
            IsReflectionOnlyGen.Generate(codeWriter).RegisterInto(methodsByFullName);
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