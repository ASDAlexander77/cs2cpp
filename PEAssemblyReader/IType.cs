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
        int GenericParameterPosition { get; }

        /// <summary>
        /// </summary>
        IEnumerable<IType> GenericTypeArguments { get; }

        /// <summary>
        /// </summary>
        bool HasElementType { get; }

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
        bool IsInterface { get; }

        /// <summary>
        /// </summary>
        bool IsDelegate { get; }

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
        bool IsNested { get; }

        /// <summary>
        /// </summary>
        IModule Module { get; }

        /// <summary>
        /// to disable optimazing for the type
        /// </summary>
        bool UseAsClass { get; set; }

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
        IEnumerable<IType> GetGenericArguments();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IEnumerable<IType> GetInterfaces();

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
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        bool IsAssignableFrom(IType type);

        IType ToArrayType(int rank);

        IType ToPointerType();

        IType Clone();

        IType ToClass();

        IType ToNormal();
    }
}