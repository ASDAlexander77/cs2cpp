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
        int ArrayRank { get; }

        /// <summary>
        /// </summary>
        IType BaseType { get; set; }

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
        IType InterfaceOwner { get; }

        /// <summary>
        /// </summary>
        bool IsArray { get; }

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
        bool IsMultiArray { get; }

        /// <summary>
        /// </summary>
        bool IsNested { get; }

        /// <summary>
        /// </summary>
        bool IsObject { get; }

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
        bool IsAbstract { get; }

        /// <summary>
        /// </summary>
        bool IsString { get; }

        /// <summary>
        /// </summary>
        bool IsValueType { get; }

        /// <summary>
        /// </summary>
        bool IsVirtualTableImplementation { get; }

        /// <summary>
        /// </summary>
        bool IsVirtualTable { get; }

        /// <summary>
        /// </summary>
        bool IsPrivateImplementationDetails { get; }

        /// <summary>
        /// </summary>
        bool IsStaticArrayInit { get; }

        /// <summary>
        /// </summary>
        IModule Module { get; }

        /// <summary>
        /// </summary>
        bool UseAsClass { get; }

        /// <summary>
        /// </summary>
        bool UseAsVirtualTableImplementation { get; }

        /// <summary>
        /// </summary>
        bool UseAsVirtualTable { get; }

        /// <summary>
        /// </summary>
        /// <param name="setUseAsClass">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        IType Clone(bool setUseAsClass = false, bool value = false);

        /// <summary>
        /// </summary>
        /// <param name="typeParameters">
        /// </param>
        /// <returns>
        /// </returns>
        IType Construct(params IType[] typeParameters);

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
        IEnumerable<IType> GetInterfaces(bool unique = true);

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
        IType ToByRefType();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IType ToByRefTypeAndPinned();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IType ToClass();

        /// <summary>
        /// </summary>
        /// <param name="containingType">
        /// </param>
        /// <param name="name">
        /// </param>
        /// <param name="isPublic">
        /// </param>
        /// <param name="isReadOnly">
        /// </param>
        /// <param name="isStatic">
        /// </param>
        /// <param name="isFixed">
        /// </param>
        /// <returns>
        /// </returns>
        IField ToField(IType containingType, string name = "value", bool isPublic = false, bool isReadOnly = false, bool isStatic = false, bool isFixed = false, int fixedSize = 0, bool isVirtualTable = false);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IType ToNormal();

        /// <summary>
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="isOut">
        /// </param>
        /// <param name="isRef">
        /// </param>
        /// <returns>
        /// </returns>
        IParameter ToParameter(string name, bool isOut = false, bool isRef = false);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IType ToPointerType();

        /// <summary>
        /// </summary>
        /// <param name="interfaceOwner">
        /// </param>
        /// <returns>
        /// </returns>
        IType ToVirtualTableImplementation(IType interfaceOwner = null);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IType ToVirtualTable();
    }
}