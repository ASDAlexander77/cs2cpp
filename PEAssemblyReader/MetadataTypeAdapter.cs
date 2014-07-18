// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataTypeAdapter.cs" company="">
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    /// <summary>
    /// </summary>
    [DebuggerDisplay("Type = {FullName}")]
    public class MetadataTypeAdapter : IType
    {
        /// <summary>
        /// </summary>
        private readonly bool isByRef;

        /// <summary>
        /// </summary>
        private readonly Lazy<string> lazyNamespace;

        /// <summary>
        /// </summary>
        private readonly TypeSymbol typeDef;

        /// <summary>
        /// </summary>
        /// <param name="typeDef">
        /// </param>
        /// <param name="isByRef">
        /// </param>
        internal MetadataTypeAdapter(TypeSymbol typeDef, bool isByRef = false)
        {
            Debug.Assert(typeDef != null);

            this.typeDef = typeDef;
            this.isByRef = isByRef;

            this.lazyNamespace = new Lazy<string>(this.CalculateNamespace);
        }

        internal TypeSymbol TypeDef
        {
            get { return typeDef; }
        }

        /// <summary>
        /// </summary>
        public string AssemblyQualifiedName
        {
            get
            {
                var effective = this.typeDef;

                while (effective.TypeKind == TypeKind.ArrayType || effective.TypeKind == TypeKind.PointerType)
                {
                    var arrayType = effective as ArrayTypeSymbol;
                    if (arrayType != null)
                    {
                        effective = arrayType.ElementType;
                        continue;
                    }

                    var pointerType = effective as PointerTypeSymbol;
                    if (pointerType != null)
                    {
                        effective = pointerType.PointedAtType;
                        continue;
                    }

                    break;
                }

                return effective.ContainingAssembly.Identity.Name;
            }
        }

        /// <summary>
        /// </summary>
        public IType BaseType
        {
            get
            {
                return this.typeDef.BaseType != null ? this.typeDef.BaseType.ToType() : null;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool ContainsGenericParameters
        {
            get
            {
                var namedTypeSymbol = this.typeDef as NamedTypeSymbol;
                if (namedTypeSymbol != null)
                {
                    return namedTypeSymbol.TypeArguments.Any();
                }

                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IType DeclaringType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// </summary>
        public string FullName
        {
            get
            {
                var sb = new StringBuilder();
                this.AppendFullNamespace(sb);
                sb.Append(this.Name);
                return sb.ToString();
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public int GenericParameterPosition
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IEnumerable<IType> GenericTypeArguments
        {
            get
            {
                var namedTypeSymbol = this.typeDef as NamedTypeSymbol;
                if (namedTypeSymbol != null)
                {
                    return namedTypeSymbol.TypeArguments.Select(t => t.ToType());
                }

                throw new NotImplementedException();
            }
        }

        public IEnumerable<IType> GenericTypeParameters
        {
            get
            {
                var namedTypeSymbol = this.typeDef as NamedTypeSymbol;
                if (namedTypeSymbol != null)
                {
                    return namedTypeSymbol.TypeParameters.Select(t => t.ToType());
                }

                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// </summary>
        public bool HasElementType
        {
            get
            {
                return this.IsByRef || this.IsArray || this.IsPointer;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsArray
        {
            get
            {
                return this.typeDef.IsArray();
            }
        }

        /// <summary>
        /// </summary>
        public bool IsByRef
        {
            get
            {
                return this.isByRef;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsClass
        {
            get
            {
                if (this.UseAsClass)
                {
                    return true;
                }

                return this.typeDef.IsClassType() && !this.IsDerivedFromEnum() && !this.IsDerivedFromValueType() || this.FullName == "System.Enum";
            }
        }

        /// <summary>
        /// </summary>
        public bool IsDelegate
        {
            get
            {
                return this.typeDef.IsDelegateType() || this.IsDerivedFromDelegateType();
            }
        }

        /// <summary>
        /// </summary>
        public bool IsEnum
        {
            get
            {
                if (this.UseAsClass)
                {
                    return false;
                }

                return this.typeDef.IsEnumType() || this.IsDerivedFromEnum();
            }
        }

        /// <summary>
        /// </summary>
        public bool IsGenericParameter
        {
            get
            {
                return this.typeDef.IsTypeParameter();
            }
        }

        /// <summary>
        /// </summary>
        public bool IsGenericType
        {
            get
            {
                var namedTypeSymbol = this.typeDef as NamedTypeSymbol;
                if (namedTypeSymbol != null)
                {
                    return namedTypeSymbol.IsGenericType;
                }

                return false;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsGenericTypeDefinition
        {
            get
            {
                var namedTypeSymbol = this.typeDef as NamedTypeSymbol;
                if (namedTypeSymbol != null)
                {
                    return namedTypeSymbol.TypeArguments.Any(t => t.TypeKind == TypeKind.TypeParameter);
                }

                return false;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsInterface
        {
            get
            {
                return this.typeDef.IsInterfaceType();
            }
        }

        /// <summary>
        /// </summary>
        public bool IsNested
        {
            get
            {
                return this.typeDef.IsNestedType();
            }
        }

        /// <summary>
        /// </summary>
        public bool IsPointer
        {
            get
            {
                return this.typeDef.IsPointerType();
            }
        }

        /// <summary>
        /// </summary>
        public bool IsPrimitive
        {
            get
            {
                if (this.UseAsClass)
                {
                    return false;
                }

                if (this.typeDef.IsPrimitiveRecursiveStruct())
                {
                    return true;
                }

                switch (this.FullName)
                {
                    case "System.Boolean":
                    case "System.Byte":
                    case "System.Char":
                    case "System.Double":
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                    case "System.UInt16":
                    case "System.UInt32":
                    case "System.UInt64":
                    case "System.IntPtr":
                    case "System.UIntPtr":
                    case "System.SByte":
                    case "System.Single":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// </summary>
        public bool IsValueType
        {
            get
            {
                if (this.UseAsClass)
                {
                    return false;
                }

                if (this.FullName == "System.Enum")
                {
                    return false;
                }

                return this.typeDef.IsValueType || this.IsDerivedFromEnum() || this.IsDerivedFromValueType();
            }
        }

        public string MetadataName
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append(this.typeDef.Name);

                if (this.IsGenericType)
                {
                    sb.Append('`');
                    sb.Append(this.typeDef.GetArity());
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// </summary>
        public string MetadataFullName
        {
            get
            {
                var sb = new StringBuilder();
                this.AppendFullNamespace(sb);
                sb.Append(this.MetadataName);

                return sb.ToString();
            }
        }

        /// <summary>
        /// </summary>
        public IModule Module
        {
            get
            {
                return new MetadataModuleAdapter(this.typeDef.ContainingModule);
            }
        }

        /// <summary>
        /// </summary>
        public string Name
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append(this.typeDef.Name);

                if (this.IsGenericType)
                {
                    sb.Append('<');

                    var index = 0;
                    foreach (var genArg in this.GetGenericArguments())
                    {
                        if (index++ > 0)
                        {
                            sb.Append(", ");
                        }

                        sb.Append(genArg.FullName);
                    }

                    sb.Append('>');
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// </summary>
        public string Namespace
        {
            get
            {
#if DEBUG
                return this.typeDef.CalculateNamespace();
#else
                return this.lazyNamespace.Value;
#endif
            }
        }

        /// <summary>
        /// </summary>
        public bool UseAsClass { get; set; }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType Clone()
        {
            return this.typeDef.ToType();
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        public int CompareTo(object obj)
        {
            var type = obj as IType;
            if (type == null)
            {
                return 1;
            }

            return this.MetadataFullName.CompareTo(type.MetadataFullName);
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        public override bool Equals(object obj)
        {
            var type = obj as IType;
            if (type != null)
            {
                return this.CompareTo(type) == 0;
            }

            return base.Equals(obj);
        }

        public IType ResolveTypeParameter(IType typeParameter)
        {
            var typeParameters = this.GenericTypeParameters.ToList();
            var typeArguments = this.GenericTypeArguments.ToList();

            for (var index = 0; index < typeArguments.Count; index++)
            {
                if (typeParameters[index].TypeEquals(typeParameter))
                {
                    return typeArguments[index];
                }
            }

            throw new KeyNotFoundException();
        }

        public IEnumerable<IType> GetAllInterfaces()
        {
            return this.typeDef.AllInterfaces.Select(i => i.ToType()).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<IConstructor> GetConstructors(BindingFlags bindingFlags)
        {
            return
                this.typeDef.GetMembers()
                    .Where(m => m is MethodSymbol && this.IsAny(((MethodSymbol)m).MethodKind, MethodKind.Constructor, MethodKind.StaticConstructor))
                    .Select(f => new MetadataConstructorAdapter(f as MethodSymbol));
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType GetElementType()
        {
            if (this.IsByRef)
            {
                return this.typeDef.ToType();
            }

            if (this.IsArray)
            {
                return (this.typeDef as ArrayTypeSymbol).ElementType.ToType();
            }

            if (this.IsPointer)
            {
                return (this.typeDef as PointerTypeSymbol).PointedAtType.ToType();
            }

            Debug.Fail(string.Empty);

            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType GetEnumUnderlyingType()
        {
            if (this.typeDef.IsEnumType())
            {
                return this.typeDef.EnumUnderlyingType().ToType();
            }

            if (this.IsDerivedFromEnum())
            {
                return this.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).First().FieldType;
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<IField> GetFields(BindingFlags bindingFlags)
        {
            return this.typeDef.GetMembers().Where(m => m is FieldSymbol).Select(f => new MetadataFieldAdapter(f as FieldSymbol));
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<IType> GetGenericArguments()
        {
            var namedTypeSymbol = this.typeDef as NamedTypeSymbol;
            if (namedTypeSymbol != null)
            {
                return namedTypeSymbol.TypeArguments.Select(a => a.ToType());
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override int GetHashCode()
        {
            return this.FullName.GetHashCode();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<IType> GetInterfaces()
        {
            // return this.typeDef.Interfaces.Select(i => new MetadataTypeAdapter(i)).ToList();
            return this.EnumerableUniqueInterfaces().Select(@interface => @interface.ToType());
        }

        public IEnumerable<IType> GetInterfacesExcludingBaseAllInterfaces()
        {
            if (this.typeDef.BaseType == null)
            {
                return this.GetInterfaces();
            }

            var baseInterfaces = this.typeDef.BaseType.AllInterfaces;
            return this.EnumerableUniqueInterfaces().Where(i => !baseInterfaces.Contains(i)).Select(i => i.ToType()).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<IMethod> GetMethods(BindingFlags bindingFlags)
        {
            foreach (var method in
                this.typeDef.GetMembers()
                    .Where(m => m is MethodSymbol && !this.IsAny(((MethodSymbol)m).MethodKind, MethodKind.Constructor, MethodKind.StaticConstructor))
                    .Select(f => new MetadataMethodAdapter(f as MethodSymbol)))
            {
                yield return method;
            }

            if (bindingFlags.HasFlag(BindingFlags.FlattenHierarchy))
            {
                var baseType = this.BaseType;
                if (baseType != null)
                {
                    foreach (var method in baseType.GetMethods(bindingFlags))
                    {
                        yield return method;
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IEnumerable<IType> GetNestedTypes()
        {
            var peType = this.typeDef as NamedTypeSymbol;
            if (peType != null)
            {
                return peType.GetTypeMembers().Select(t => t.ToType());
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public bool IsAssignableFrom(IType type)
        {
            return type.IsDerivedFrom(this) || type.GetAllInterfaces().Contains(this);
        }

        /// <summary>
        /// </summary>
        /// <param name="rank">
        /// </param>
        /// <returns>
        /// </returns>
        public IType ToArrayType(int rank)
        {
            return new ArrayTypeSymbol(this.typeDef.ContainingAssembly, this.typeDef, rank: rank).ToType();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType ToClass()
        {
            var newType = this.typeDef.ToType();
            newType.UseAsClass = true;
            return newType;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType ToNormal()
        {
            var newType = this.typeDef.ToType();
            newType.UseAsClass = true;
            return newType;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType ToPointerType()
        {
            return new PointerTypeSymbol(this.typeDef).ToType();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            var result = new StringBuilder();

            // write Full Name
            if (this.HasElementType)
            {
                result.Append(this.GetElementType());

                if (!this.IsByRef)
                {
                    if (this.IsPointer)
                    {
                        result.Append('*');
                    }

                    if (this.IsArray)
                    {
                        result.Append("[]");
                    }
                }
                else
                {
                    result.Append('&');
                }
            }
            else
            {
                if ((this.IsPrimitive || this.Name == "Void") && this.Namespace == "System")
                {
                    result.Append(this.Name);
                }
                else
                {
                    result.Append(this.FullName);
                }

                // append generic types
                if (this.IsGenericType)
                {
                    result.Append('<');

                    var index = 0;
                    foreach (var genArg in this.GenericTypeArguments)
                    {
                        if (index++ > 0)
                        {
                            result.Append(", ");
                        }

                        result.Append(genArg);
                    }

                    result.Append('>');
                }
            }

            return result.ToString();
        }

        private void AppendFullNamespace(StringBuilder sb)
        {
            sb.Append(this.Namespace);
            if (sb.Length > 0)
            {
                sb.Append('.');
            }

            if (this.typeDef.ContainingType != null && this.typeDef.Kind != SymbolKind.TypeParameter)
            {
                sb.Append(this.typeDef.ContainingType.ToType().Name);
                sb.Append('+');
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private string CalculateNamespace()
        {
            return this.typeDef.CalculateNamespace();
        }

        private IEnumerable<TypeSymbol> EnumerableUniqueInterfaces()
        {
            TypeSymbol previous = null;

            foreach (var @interface in this.typeDef.Interfaces.Where(@interface => previous == null || !previous.AllInterfaces.Contains(@interface)))
            {
                previous = @interface;
                yield return @interface;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <param name="methodKind1">
        /// </param>
        /// <param name="methodKind2">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IsAny(MethodKind source, MethodKind methodKind1, MethodKind methodKind2)
        {
            return source == methodKind1 || source == methodKind2;
        }

        private bool IsDerivedFromDelegateType()
        {
            return this.BaseType != null && (this.BaseType.FullName == "System.Delegate" || this.BaseType.FullName == "System.MulticastDelegate");
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private bool IsDerivedFromEnum()
        {
            return this.BaseType != null && this.BaseType.FullName == "System.Enum";
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private bool IsDerivedFromValueType()
        {
            return this.BaseType != null && this.BaseType.FullName == "System.ValueType";
        }
    }
}