namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Reflection.Metadata;

    using Microsoft.CodeAnalysis;

    public class MetadataTypeAdapter : IType
    {
        private TypeHandle typeDef;

        private ModuleMetadata module;

        public MetadataTypeAdapter(TypeHandle typeDef, ModuleMetadata module)
        {
            this.typeDef = typeDef;
            this.module = module;
        }

        public string Name { get; private set; }

        public string Namespace { get; private set; }

        public string FullName { get; private set; }

        public string AssemblyQualifiedName { get; private set; }

        public bool HasElementType { get; private set; }

        public bool IsGenericType { get; private set; }

        public bool IsPointer { get; private set; }

        public bool IsArray { get; private set; }

        public bool IsByRef { get; private set; }

        public bool IsValueType { get; private set; }

        public bool IsGenericTypeDefinition { get; private set; }

        public bool IsGenericParameter { get; private set; }

        public bool ContainsGenericParameters { get; private set; }

        public bool IsInterface { get; private set; }

        public bool IsEnum { get; private set; }

        public bool IsPrimitive { get; private set; }

        public bool IsClass { get; private set; }

        public int GenericParameterPosition { get; private set; }

        public IEnumerable<IType> GenericTypeArguments { get; private set; }

        public IType DeclaringType { get; private set; }

        public Guid GUID { get; private set; }

        public IType BaseType { get; private set; }

        public IEnumerable<IField> GetFields(BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMethod> GetMethods(BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IConstructor> GetConstructors(BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IType> GetInterfaces()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IType> GetGenericArguments()
        {
            throw new NotImplementedException();
        }

        public IType GetElementType()
        {
            throw new NotImplementedException();
        }

        public bool IsAssignableFrom(IType type)
        {
            throw new NotImplementedException();
        }
    }
}
