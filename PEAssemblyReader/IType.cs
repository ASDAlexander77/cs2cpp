namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IType
    {
        string Name { get; }

        string FullName { get; }

        bool HasElementType { get; }

        bool IsGenericType { get; }

        bool IsPointer { get; }

        bool IsValueType { get; }

        bool IsGenericTypeDefinition { get; }

        bool IsGenericParameter { get; }

        bool ContainsGenericParameters { get; }

        bool IsInterface { get; }

        bool IsEnum { get; }
        
        bool IsPrimitive { get; }

        IEnumerable<IType> GenericTypeArguments { get; }

        IType DeclaringType { get; }

        Guid GUID { get; }

        IType BaseType { get; }

        IEnumerable<IField> GetFields(BindingFlags bindingFlags);

        IEnumerable<IMethod> GetMethods(BindingFlags bindingFlags);

        IEnumerable<IType> GetInterfaces();

        IEnumerable<IType> GetGenericArguments();

        IType GetElementType();
    }
}
