// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IType.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// </summary>
    public interface IType : IName
    {
        /// <summary>
        /// </summary>
        IType BaseType { get; }

        /// <summary>
        /// </summary>
        bool ContainsGenericParameters { get; }

        /// <summary>
        /// </summary>
        IEnumerable<IType> GenericTypeArguments { get; }

        /// <summary>
        /// </summary>
        IEnumerable<IType> GenericTypeParameters { get; }

        /// <summary>
        /// </summary>
        bool HasDeclaringType { get; }

        /// <summary>
        /// </summary>
        bool HasElementType { get; }

        /// <summary>
        /// </summary>
        bool IsArray { get; }

        /// <summary>
        /// </summary>
        bool IsMultiArray { get; }

        /// <summary>
        /// </summary>
        int ArrayRank { get; }

        /// <summary>
        /// </summary>
        bool IsByRef { get; }

        /// <summary>
        /// </summary>
        bool IsClass { get; }

        /// <summary>
        /// </summary>
        bool IsDelegate { get; }

        /// <summary>
        /// </summary>
        bool IsEnum { get; }

        /// <summary>
        /// </summary>
        bool IsObject { get; }

        /// <summary>
        /// </summary>
        bool IsGenericParameter { get; }

        /// <summary>
        /// </summary>
        bool IsGenericType { get; }

        /// <summary>
        /// </summary>
        bool IsGenericTypeDefinition { get; }

        /// <summary>
        /// </summary>
        bool IsGenericTypeDefinitionLocal { get; }

        /// <summary>
        /// </summary>
        bool IsGenericTypeLocal { get; }

        /// <summary>
        /// </summary>
        bool IsInterface { get; }

        /// <summary>
        /// </summary>
        bool IsNested { get; }

        /// <summary>
        /// </summary>
        bool IsPinned { get; }

        /// <summary>
        /// </summary>
        bool IsPointer { get; }

        /// <summary>
        /// </summary>
        bool IsPrimitive { get; }

        /// <summary>
        /// </summary>
        bool IsValueType { get; }

        /// <summary>
        /// </summary>
        IModule Module { get; }

        /// <summary>
        /// </summary>
        bool UseAsClass { get; }

        /// <summary>
        /// </summary>
        /// <param name="setUseAsClass">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        IType Clone(bool setUseAsClass = false, bool value = false, bool isByRef = false, bool isPinned = false);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IEnumerable<IType> GetAllInterfaces();

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        IEnumerable<IConstructor> GetConstructors(BindingFlags bindingFlags);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IType GetDeclaringTypeOriginal();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IType GetElementType();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IType GetEnumUnderlyingType();

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        IEnumerable<IField> GetFields(BindingFlags bindingFlags);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IEnumerable<IType> GetInterfaces();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IEnumerable<IType> GetInterfacesExcludingBaseAllInterfaces();

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        IEnumerable<IMethod> GetMethods(BindingFlags bindingFlags);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IEnumerable<IType> GetNestedTypes();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IType GetTypeDefinition();

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        bool IsAssignableFrom(IType type);

        /// <summary>
        /// </summary>
        /// <param name="rank">
        /// </param>
        /// <returns>
        /// </returns>
        IType ToArrayType(int rank);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IType ToClass();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IType ToDereferencedType();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IType ToByRefType();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IType ToNormal();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IType ToPointerType();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IParameter ToParameter(bool isOut = false, bool isRef = false, string name = "value");
    }
}