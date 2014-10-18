// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataTypeAdapter.cs" company="">
//   
// </copyright>
// <summary>
//   
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
        private readonly Lazy<string> lazyName;

        /// <summary>
        /// </summary>
        private readonly Lazy<string> lazyFullName;

        /// <summary>
        /// </summary>
        private readonly Lazy<string> lazyMetadataName;

        /// <summary>
        /// </summary>
        private readonly Lazy<string> lazyMetadataFullName;

        /// <summary>
        /// </summary>
        private readonly Lazy<string> lazyNamespace;

        /// <summary>
        /// </summary>
        private readonly Lazy<IType> lazyBaseType;

        /// <summary>
        /// </summary>
        private readonly Lazy<string> lazyAssemblyQualifiedName;

        /// <summary>
        /// </summary>
        private readonly Lazy<string> lazyToString;

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
            Debug.Assert(typeDef.TypeKind != TypeKind.Error);

            this.typeDef = typeDef;
            this.IsByRef = isByRef;

            this.lazyName = new Lazy<string>(this.CalculateName);
            this.lazyFullName = new Lazy<string>(this.CalculateFullName);
            this.lazyMetadataName = new Lazy<string>(this.CalculateMetadataName);
            this.lazyMetadataFullName = new Lazy<string>(this.CalculateMetadataFullName);
            this.lazyNamespace = new Lazy<string>(this.CalculateNamespace);
            this.lazyBaseType = new Lazy<IType>(this.CalculateBaseType);
            this.lazyAssemblyQualifiedName = new Lazy<string>(this.CalculateAssemblyQualifiedName);
            this.lazyToString = new Lazy<string>(this.CalculateToString);
        }

        /// <summary>
        /// </summary>
        /// <param name="typeDef">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <param name="isByRef">
        /// </param>
        internal MetadataTypeAdapter(TypeSymbol typeDef, IGenericContext genericContext, bool isByRef = false)
            : this(typeDef, isByRef)
        {
            this.GenericContext = genericContext;
        }

        /// <summary>
        /// </summary>
        public string AssemblyQualifiedName
        {
            get
            {
                return this.lazyAssemblyQualifiedName.Value;
            }
        }

        /// <summary>
        /// </summary>
        public IType BaseType
        {
            get
            {
                return this.lazyBaseType.Value;
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
                if (!this.HasDeclaringType)
                {
                    return null;
                }

                return this.typeDef.ContainingType.ResolveGeneric(this.GenericContext);
            }
        }

        /// <summary>
        /// </summary>
        public string FullName
        {
            get
            {
                return this.lazyFullName.Value;
            }
        }

        /// <summary>
        /// </summary>
        public IGenericContext GenericContext { get; set; }

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
                    return namedTypeSymbol.TypeArguments.Select(t => t.ResolveGeneric(this.GenericContext));
                }

                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IEnumerable<IType> GenericTypeParameters
        {
            get
            {
                var namedTypeSymbol = this.typeDef as NamedTypeSymbol;
                if (namedTypeSymbol != null)
                {
                    // TODO: you do not need to fix GenericTypeParameters (otherwise you will map ResolvedType to ResolvedType but you need to map Generic Param to ResolvedType
                    return namedTypeSymbol.TypeParameters.Select(t => new MetadataTypeAdapter(t));
                }

                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// </summary>
        public bool HasDeclaringType
        {
            get
            {
                return this.typeDef.ContainingType != null;
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
        public bool IsByRef { get; set; }

        /// <summary>
        /// </summary>
        public bool IsPinned { get; set; }

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
                    switch (this.FullName)
                    {
                        case "System.IntPtr":
                        case "System.UIntPtr":
                            return false;
                    }

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

                if (this.FullName == "System.Enum" || this.IsPointer)
                {
                    return false;
                }

                return this.typeDef.IsValueType || this.IsDerivedFromEnum() || this.IsDerivedFromValueType();
            }
        }

        /// <summary>
        /// </summary>
        public string MetadataFullName
        {
            get
            {
                return this.lazyMetadataFullName.Value;
            }
        }

        /// <summary>
        /// </summary>
        public string MetadataName
        {
            get
            {
                return this.lazyMetadataName.Value;
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
                return this.lazyName.Value;
            }
        }

        /// <summary>
        /// </summary>
        public string Namespace
        {
            get
            {
                return this.lazyNamespace.Value;
            }
        }

        /// <summary>
        /// </summary>
        public bool UseAsClass { get; set; }

        /// <summary>
        /// </summary>
        internal TypeSymbol TypeDef
        {
            get
            {
                return this.typeDef;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType Clone()
        {
            return this.typeDef.ResolveGeneric(this.GenericContext);
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

            var cmp = this.MetadataName.CompareTo(type.MetadataName);
            if (cmp != 0)
            {
                return cmp;
            }

            cmp = this.Namespace.CompareTo(type.Namespace);
            if (cmp != 0)
            {
                return cmp;
            }

            if (!this.HasDeclaringType && !type.HasDeclaringType)
            {
                return 0;
            }

            if (!this.HasDeclaringType)
            {
                return -1;
            }

            if (!type.HasDeclaringType)
            {
                return 1;
            }

            return this.GetDeclaringTypeOriginal().CompareTo(type.GetDeclaringTypeOriginal());
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

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<IType> GetAllInterfaces()
        {
            return this.typeDef.AllInterfaces.Select(i => i.ResolveGeneric(this.GenericContext)).ToList();
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
                    .Select(f => new MetadataConstructorAdapter(f as MethodSymbol, this.GenericContext));
        }

        /// <summary>
        /// To prevent resolving generic type before actually using resolved generic type
        /// </summary>
        /// <returns>
        /// </returns>
        public IType GetDeclaringTypeOriginal()
        {
            return new MetadataTypeAdapter(this.typeDef.ContainingType);
        }

        public IType GetTypeDefinition()
        {
            return new MetadataTypeAdapter((this.typeDef as NamedTypeSymbol).ConstructedFrom);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType GetElementType()
        {
            if (this.IsByRef)
            {
                return this.typeDef.ResolveGeneric(this.GenericContext);
            }

            if (this.IsArray)
            {
                return (this.typeDef as ArrayTypeSymbol).ElementType.ResolveGeneric(this.GenericContext);
            }

            if (this.IsPointer)
            {
                return (this.typeDef as PointerTypeSymbol).PointedAtType.ResolveGeneric(this.GenericContext);
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
                return this.typeDef.EnumUnderlyingType().ResolveGeneric(this.GenericContext);
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
            return this.typeDef.GetMembers().Where(m => m is FieldSymbol).Select(f => new MetadataFieldAdapter(f as FieldSymbol, this.GenericContext));
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
                return namedTypeSymbol.TypeArguments.Select(a => a.ResolveGeneric(this.GenericContext));
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
            return this.EnumerableUniqueInterfaces().Select(@interface => @interface.ResolveGeneric(this.GenericContext));
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<IType> GetInterfacesExcludingBaseAllInterfaces()
        {
            if (this.typeDef.BaseType == null)
            {
                return this.GetInterfaces();
            }

            var baseInterfaces = this.typeDef.BaseType.AllInterfaces;
            return this.EnumerableUniqueInterfaces().Where(i => !baseInterfaces.Contains(i)).Select(i => i.ResolveGeneric(this.GenericContext)).ToList();
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
                    .Select(f => new MetadataMethodAdapter(f as MethodSymbol, this.GenericContext)))
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
                return peType.GetTypeMembers().Select(t => t.ResolveGeneric(this.GenericContext));
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
            if (type.IsDerivedFrom(this))
            {
                return true;
            }
            
            if (type.GetAllInterfaces().Contains(this))
            {
                return true;
            }

            if (this.IsArray && type.IsArray && type.GetElementType().IsDerivedFrom(this.GetElementType()))
            {
                return true;
            }

            if (this.IsPointer && type.IsPointer && type.GetElementType().IsDerivedFrom(this.GetElementType()))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="typeParameter">
        /// </param>
        /// <returns>
        /// </returns>
        public IType ResolveTypeParameter(IType typeParameter)
        {
            return ResolveTypeParameter(this, typeParameter);
        }

        private static IType ResolveTypeParameter(IType source, IType typeParameter)
        {
            var typeParameters = source.GenericTypeParameters.ToList();
            var typeArguments = source.GenericTypeArguments.ToList();

            for (var index = 0; index < typeArguments.Count; index++)
            {
                if (typeParameters[index].TypeEquals(typeParameter))
                {
                    return typeArguments[index];
                }
            }

            return source.IsNested ? ResolveTypeParameter(source.DeclaringType, typeParameter) : null;
        }

        /// <summary>
        /// </summary>
        /// <param name="rank">
        /// </param>
        /// <returns>
        /// </returns>
        public IType ToArrayType(int rank)
        {
            var containingAssembly = this.typeDef.IsArray() ? (this.typeDef as ArrayTypeSymbol).ElementType.ContainingAssembly : this.typeDef.ContainingAssembly;
            return new MetadataTypeAdapter(new ArrayTypeSymbol(containingAssembly, this.typeDef, rank: rank), this.GenericContext);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType ToClass()
        {
            var newType = this.typeDef.ResolveGeneric(this.GenericContext);
            newType.UseAsClass = true;
            return newType;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType ToNormal()
        {
            var newType = this.typeDef.ResolveGeneric(this.GenericContext);
            newType.UseAsClass = false;
            return newType;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType ToPointerType()
        {
            return new PointerTypeSymbol(this.typeDef).ResolveGeneric(this.GenericContext);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType ToDereferencedType()
        {
            return this.IsPointer ? GetElementType() : this.typeDef.ResolveGeneric(this.GenericContext);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            return this.lazyToString.Value;
        }

        private string CalculateToString()
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
            }

            return result.ToString();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private string CalculateNamespace()
        {
            return this.typeDef.CalculateNamespace();
        }

        private IType CalculateBaseType()
        {
            return this.typeDef.BaseType != null ? this.typeDef.BaseType.ResolveGeneric(this.GenericContext) : null;
        }

        private string CalculateName()
        {
            var sb = new StringBuilder();

            sb.Append(this.typeDef.Name);

            if (this.IsGenericType || this.IsGenericTypeDefinition)
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

            if (this.IsArray)
            {
                sb.Append(this.GetElementType().Name);
                sb.Append("[]");
            }

            if (this.IsPointer)
            {
                sb.Append(this.GetElementType().Name);
                sb.Append("*");
            }

            return sb.ToString();
        }

        private string CalculateFullName()
        {
            var sb = new StringBuilder();
            if (!this.IsGenericParameter)
            {
                this.typeDef.AppendFullNamespace(sb, this.Namespace, this.DeclaringType);
            }

            sb.Append(this.Name);
            return sb.ToString();
        }

        private string CalculateMetadataName()
        {
            var sb = new StringBuilder();

            sb.Append(this.typeDef.Name);

            if (this.IsGenericType || this.IsGenericTypeDefinition)
            {
                sb.Append('`');
                sb.Append(this.typeDef.GetArity());
            }

            if (this.IsArray)
            {
                sb.Append(this.GetElementType().MetadataName);
                sb.Append("[]");
            }

            if (this.IsPointer)
            {
                sb.Append(this.GetElementType().MetadataName);
                sb.Append("*");
            }

            return sb.ToString();
        }

        private string CalculateMetadataFullName()
        {
            var sb = new StringBuilder();
            this.typeDef.AppendFullNamespace(sb, this.Namespace, this.DeclaringType, true);
            sb.Append(this.MetadataName);

            return sb.ToString();
        }

        private string CalculateAssemblyQualifiedName()
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

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
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

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
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