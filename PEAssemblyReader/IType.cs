namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IType : IName
    {
        bool HasElementType { get; }

        bool IsGenericType { get; }

        bool IsPointer { get; }

        bool IsArray { get; }

        bool IsByRef { get; }

        bool IsValueType { get; }

        bool IsGenericTypeDefinition { get; }

        bool IsGenericParameter { get; }

        bool ContainsGenericParameters { get; }

        bool IsInterface { get; }

        bool IsEnum { get; }
        
        bool IsPrimitive { get; }

        bool IsClass { get; }

        int GenericParameterPosition { get; }

        IEnumerable<IType> GenericTypeArguments { get; }

        Guid GUID { get; }

        IType BaseType { get; }

        IEnumerable<IField> GetFields(BindingFlags bindingFlags);

        IEnumerable<IMethod> GetMethods(BindingFlags bindingFlags);

        IEnumerable<IConstructor> GetConstructors(BindingFlags bindingFlags);

        IEnumerable<IType> GetInterfaces();

        IEnumerable<IType> GetGenericArguments();

        IType GetElementType();

        bool IsAssignableFrom(IType type);
    }
}
