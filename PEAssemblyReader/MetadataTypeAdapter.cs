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
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    /// <summary>
    /// </summary>
    [DebuggerDisplay("Type = {FullName}")]
    public class MetadataTypeAdapter : IType
    {
        /// <summary>
        /// </summary>
        private readonly Lazy<string> lazyAssemblyQualifiedName;

        /// <summary>
        /// </summary>
        private readonly Lazy<IType> lazyBaseType;

        /// <summary>
        /// </summary>
        private readonly IDictionary<BindingFlags, Lazy<IEnumerable<IConstructor>>> lazyConstructors =
            new SortedDictionary<BindingFlags, Lazy<IEnumerable<IConstructor>>>();

        /// <summary>
        /// </summary>
        private readonly Lazy<IType> lazyDeclaringType;

        /// <summary>
        /// </summary>
        private readonly Lazy<IType> lazyDeclaringTypeOriginal;

        /// <summary>
        /// </summary>
        private readonly IDictionary<BindingFlags, Lazy<IEnumerable<IField>>> lazyFields = new SortedDictionary<BindingFlags, Lazy<IEnumerable<IField>>>();

        /// <summary>
        /// </summary>
        private readonly Lazy<string> lazyFullName;

        /// <summary>
        /// </summary>
        private readonly Lazy<IType> lazyGetElementType;

        /// <summary>
        /// </summary>
        private readonly Lazy<string> lazyMetadataFullName;

        /// <summary>
        /// </summary>
        private readonly Lazy<string> lazyMetadataName;

        /// <summary>
        /// </summary>
        private readonly IDictionary<BindingFlags, Lazy<IEnumerable<IMethod>>> lazyMethods = new SortedDictionary<BindingFlags, Lazy<IEnumerable<IMethod>>>();

        /// <summary>
        /// </summary>
        private readonly Lazy<MetadataModuleAdapter> lazyModule;

        /// <summary>
        /// </summary>
        private readonly Lazy<string> lazyName;

        /// <summary>
        /// </summary>
        private readonly Lazy<string> lazyNamespace;

        /// <summary>
        /// </summary>
        private readonly Lazy<string> lazyToString;

        /// <summary>
        /// </summary>
        private readonly TypeSymbol typeDef;

        /// <summary>
        /// </summary>
        private IType customBaseType;

        /// <summary>
        /// </summary>
        /// <param name="typeDef">
        /// </param>
        /// <param name="isByRef">
        /// </param>
        /// <param name="isPinned">
        /// </param>
        internal MetadataTypeAdapter(TypeSymbol typeDef, bool isByRef = false, bool isPinned = false)
        {
            Debug.Assert(typeDef != null);

            this.typeDef = typeDef;
            this.IsByRef = isByRef;
            this.IsPinned = isPinned;

            var def = typeDef as ByRefReturnErrorTypeSymbol;
            if (def != null)
            {
                this.typeDef = def.ReferencedType;
                this.IsByRef = true;
            }

            Debug.Assert(this.typeDef.TypeKind != TypeKind.Error || this.typeDef is UnboundArgumentErrorTypeSymbol);

            this.lazyName = new Lazy<string>(this.CalculateName);
            this.lazyFullName = new Lazy<string>(this.CalculateFullName);
            this.lazyMetadataName = new Lazy<string>(this.CalculateMetadataName);
            this.lazyMetadataFullName = new Lazy<string>(this.CalculateMetadataFullName);
            this.lazyNamespace = new Lazy<string>(this.CalculateNamespace);
            this.lazyBaseType = new Lazy<IType>(this.CalculateBaseType);
            this.lazyAssemblyQualifiedName = new Lazy<string>(this.CalculateAssemblyQualifiedName);
            this.lazyToString = new Lazy<string>(this.CalculateToString);
            this.lazyModule = new Lazy<MetadataModuleAdapter>(this.CalculateModule);
            this.lazyDeclaringTypeOriginal = new Lazy<IType>(this.CalculateDeclaringTypeOriginal);
            this.lazyDeclaringType = new Lazy<IType>(this.CalculateDeclaringType);
            this.lazyGetElementType = new Lazy<IType>(this.CalculateGetElementType);

            var peTypeSymbol = typeDef as PENamedTypeSymbol;
            if (peTypeSymbol != null)
            {
                this.Token = MetadataTokens.GetToken(peTypeSymbol.Handle);
            }
        }

        /// <summary>
        /// </summary>
        public int ArrayRank
        {
            get
            {
                return this.typeDef.IsArray() ? ((ArrayTypeSymbol)this.typeDef).Rank : 0;
            }
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
                return this.customBaseType ?? this.lazyBaseType.Value;
            }

            set
            {
                this.customBaseType = value;
            }
        }

        /// <summary>
        /// </summary>
        public bool ContainsGenericParameters
        {
            get
            {
                return this.AnyGenericParameters();
            }
        }

        /// <summary>
        /// </summary>
        public IType DeclaringType
        {
            get
            {
                return this.lazyDeclaringType.Value;
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
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IEnumerable<IType> GenericTypeArguments
        {
            get
            {
                return this.CalculateGenericArguments();
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
                return this.CalculateGenericParameters();
            }
        }

        /// <summary>
        /// </summary>
        public bool HasDeclaringType
        {
            get
            {
                return this.DeclaringType != null;
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
        public IType InterfaceOwner { get; set; }

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
        public bool IsClass
        {
            get
            {
                if (this.UseAsClass)
                {
                    return true;
                }

                return this.typeDef.IsClassType() && !this.IsDerivedFromEnum() && !this.IsDerivedFromValueType()
                       || this.typeDef.SpecialType == SpecialType.System_Enum;
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
                return this.typeDef.IsTypeParameter() || this.typeDef is UnboundArgumentErrorTypeSymbol;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsGenericType
        {
            get
            {
                return this.CalculateIsGenericType();
            }
        }

        /// <summary>
        /// </summary>
        public bool IsGenericTypeDefinition
        {
            get
            {
                return this.CalculateIsGenericTypeDefinition();
            }
        }

        /// <summary>
        /// </summary>
        public bool IsGenericTypeDefinitionLocal
        {
            get
            {
                return this.AnyGenericParameters() && this.GenericTypeArguments.Any(tp => tp.IsGenericParameter || tp.IsGenericTypeDefinition);
            }
        }

        /// <summary>
        /// </summary>
        public bool IsGenericTypeLocal
        {
            get
            {
                return this.AnyGenericParameters() && !this.GenericTypeArguments.Any(tp => tp.IsGenericParameter || tp.IsGenericTypeDefinition);
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
        public bool IsMultiArray
        {
            get
            {
                return this.typeDef.IsArray() && !this.typeDef.IsSingleDimensionalArray();
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
        public bool IsObject
        {
            get
            {
                return this.typeDef.SpecialType == SpecialType.System_Object;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsPinned { get; set; }

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

                return this.CalculateIsPrimitive();
            }
        }

        /// <summary>
        /// </summary>
        public bool IsAbstract
        {
            get
            {
                return this.typeDef.IsAbstract;
            }
        } 

        /// <summary>
        /// </summary>
        public bool IsString
        {
            get
            {
                return this.typeDef.SpecialType == SpecialType.System_String;
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

                var isEnum = this.IsEnum;
                if ((isEnum && this.typeDef.SpecialType == SpecialType.System_Enum) || this.IsPointer)
                {
                    return false;
                }

                return this.typeDef.IsValueType || isEnum || this.IsDerivedFromValueType();
            }
        }


        public bool IsPrivateImplementationDetails
        {
            get
            {
                return this.FullName.StartsWith("<PrivateImplementationDetails>");
            }
        }

        public bool IsStaticArrayInit
        {
            get
            {
                return this.Name.StartsWith("__StaticArrayInitTypeSize");
            }
        }

        /// <summary>
        /// </summary>
        public bool IsVirtualTableImplementation
        {
            get
            {
                return this.UseAsVirtualTableImplementation;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsVirtualTable
        {
            get
            {
                return this.UseAsVirtualTable;
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
                return this.lazyModule.Value;
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
        public int? Token { get; private set; }

        /// <summary>
        /// </summary>
        public bool UseAsClass { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsVirtualTableImplementation { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsVirtualTable { get; set; }

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
        /// <param name="interfaceOwner">
        /// </param>
        /// <returns>
        /// </returns>
        public IType AsVirtualTableImplementation(IType interfaceOwner = null)
        {
            var typeAdapter = new MetadataTypeAdapter(this.typeDef, this.IsByRef, this.IsPinned);
            typeAdapter.UseAsVirtualTableImplementation = true;
            typeAdapter.InterfaceOwner = interfaceOwner;
            return typeAdapter;
        }

        public IType AsVirtualTable()
        {
            var typeAdapter = new MetadataTypeAdapter(this.typeDef, this.IsByRef, this.IsPinned);
            typeAdapter.UseAsVirtualTable = true;
            typeAdapter.UseAsClass = this.UseAsClass;
            return typeAdapter;
        }

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<IConstructor> CalculateConstructors(BindingFlags bindingFlags)
        {
            return this.IterateConstructors(bindingFlags).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<IField> CalculateFields(BindingFlags bindingFlags)
        {
            return this.IterateFields(bindingFlags).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<IMethod> CalculateMethods(BindingFlags bindingFlags)
        {
            return this.IterateMethods(bindingFlags).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="setUseAsClass">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        public IType Clone(bool setUseAsClass = false, bool value = false)
        {
            var typeAdapter = new MetadataTypeAdapter(this.typeDef, this.IsByRef, this.IsPinned);
            if (setUseAsClass)
            {
                typeAdapter.UseAsClass = value;
            }

            return typeAdapter;
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

            var cmp = 0;
            if (this.IsGenericType && !this.IsGenericTypeDefinition && type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                cmp = this.Name.CompareTo(type.Name);
                if (cmp != 0)
                {
                    return cmp;
                }
            }
            else
            {
                cmp = this.MetadataName.CompareTo(type.MetadataName);
                if (cmp != 0)
                {
                    return cmp;
                }
            }

            if (this.IsGenericParameter && type.IsGenericParameter)
            {
                return 0;
            }

            cmp = this.Namespace.CompareTo(type.Namespace);
            if (cmp != 0)
            {
                return cmp;
            }

            cmp = this.IsArray.CompareTo(type.IsArray);
            if (cmp != 0)
            {
                return cmp;
            }

            cmp = this.IsClass.CompareTo(type.IsClass);
            if (cmp != 0)
            {
                return cmp;
            }

            cmp = this.IsByRef.CompareTo(type.IsByRef);
            if (cmp != 0)
            {
                return cmp;
            }

            cmp = this.IsPointer.CompareTo(type.IsPointer);
            if (cmp != 0)
            {
                return cmp;
            }

            cmp = this.IsVirtualTableImplementation.CompareTo(type.IsVirtualTableImplementation);
            if (cmp != 0)
            {
                return cmp;
            }

            cmp = this.IsVirtualTable.CompareTo(type.IsVirtualTable);
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

            cmp = this.GetDeclaringTypeOriginal().CompareTo(type.GetDeclaringTypeOriginal());
            return cmp;
        }

        /// <summary>
        /// </summary>
        /// <param name="types">
        /// </param>
        /// <returns>
        /// </returns>
        public IType Construct(params IType[] types)
        {
            var builder = ImmutableArray.CreateBuilder<TypeSymbol>();
            builder.AddRange(types.Select(t => ((MetadataTypeAdapter)t).TypeDef));
            return new ConstructedNamedTypeSymbol(this.typeDef as NamedTypeSymbol, builder.ToImmutable()).ToAdapter();
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
                if (this.Token.HasValue && type.Token.HasValue && this.Token.Value != type.Token.Value)
                {
                    return false;
                }

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
            if (this.customBaseType != null)
            {
                return this.typeDef.Interfaces.Select(i => i.ToAdapter()).Union(this.customBaseType.GetAllInterfaces()).ToList();
            }

            return this.typeDef.AllInterfaces.Select(i => i.ToAdapter()).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<IConstructor> GetConstructors(BindingFlags bindingFlags)
        {
            Lazy<IEnumerable<IConstructor>> lazyConstructorsByFlags;
            if (!this.lazyConstructors.TryGetValue(bindingFlags, out lazyConstructorsByFlags))
            {
                lazyConstructorsByFlags = new Lazy<IEnumerable<IConstructor>>(() => this.CalculateConstructors(bindingFlags));
                this.lazyConstructors[bindingFlags] = lazyConstructorsByFlags;
            }

            return lazyConstructorsByFlags.Value;
        }

        /// <summary>
        /// To prevent resolving generic type before actually using resolved generic type
        /// </summary>
        /// <returns>
        /// </returns>
        public IType GetDeclaringTypeOriginal()
        {
            return this.lazyDeclaringTypeOriginal.Value;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType GetElementType()
        {
            return this.lazyGetElementType.Value;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType GetEnumUnderlyingType()
        {
            if (this.typeDef.IsEnumType())
            {
                return this.typeDef.EnumUnderlyingType().ToAdapter();
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
            Lazy<IEnumerable<IField>> lazyFieldsByFlags;
            if (!this.lazyFields.TryGetValue(bindingFlags, out lazyFieldsByFlags))
            {
                lazyFieldsByFlags = new Lazy<IEnumerable<IField>>(() => this.CalculateFields(bindingFlags));
                this.lazyFields[bindingFlags] = lazyFieldsByFlags;
            }

            return lazyFieldsByFlags.Value;
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
        public IEnumerable<IType> GetInterfaces(bool unique = true)
        {
            if (!unique)
            {
                return this.typeDef.Interfaces.Select(@interface => @interface.ToAdapter());
            }

            return this.EnumerableUniqueInterfaces().Select(@interface => @interface.ToAdapter());
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
            return this.EnumerableUniqueInterfaces().Where(i => !baseInterfaces.Contains(i)).Select(i => i.ToAdapter()).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<IMethod> GetMethods(BindingFlags bindingFlags)
        {
            Lazy<IEnumerable<IMethod>> lazyMethodsByFlags;
            if (!this.lazyMethods.TryGetValue(bindingFlags, out lazyMethodsByFlags))
            {
                lazyMethodsByFlags = new Lazy<IEnumerable<IMethod>>(() => this.CalculateMethods(bindingFlags));
                this.lazyMethods[bindingFlags] = lazyMethodsByFlags;
            }

            return lazyMethodsByFlags.Value;
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
                return peType.GetTypeMembers().Select(t => t.ToAdapter());
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType GetTypeDefinition()
        {
            if (this.typeDef.IsArray())
            {
                var elementType = GetBareTypeSymbol(this.typeDef, false);
                return new MetadataTypeAdapter(elementType).GetTypeDefinition().ToArrayType((this.typeDef as ArrayTypeSymbol).Rank);
            }

            if (this.typeDef.IsPointerType())
            {
                var elementType = GetBareTypeSymbol(this.typeDef, false);
                return new MetadataTypeAdapter(elementType).GetTypeDefinition().ToPointerType();
            }

            return new MetadataTypeAdapter((this.typeDef as NamedTypeSymbol).ConstructedFrom);
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

            if (this.IsArray && type.IsArray && this.GetElementType().IsAssignableFrom(type.GetElementType()))
            {
                return true;
            }

            // TODO: temporary hack to allow cast Array to IEnumerable
            if (this.IsInterface && type.IsArray)
            {
                return true;
            }

            if (this.IsPointer && type.IsPointer && this.GetElementType().IsAssignableFrom(type.GetElementType()))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="rank">
        /// </param>
        /// <returns>
        /// </returns>
        public IType ToArrayType(int rank)
        {
            var containingAssembly = GetBareTypeSymbol(this.typeDef).ContainingAssembly;
            return new MetadataTypeAdapter(new ArrayTypeSymbol(containingAssembly, this.typeDef, rank: rank), this.IsByRef, this.IsPinned);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType ToByRefType()
        {
            var newType = this.typeDef.ToAdapter(true);
            Debug.Assert(newType.IsByRef);
            return newType;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType ToByRefTypeAndPinned()
        {
            var newType = this.typeDef.ToAdapter(true, true);
            Debug.Assert(newType.IsByRef);
            Debug.Assert(newType.IsPinned);
            return newType;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType ToClass()
        {
            if (this.UseAsClass || !this.IsValueType)
            {
                return this;
            }

            return this.Clone(true, true);
        }

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
        public IField ToField(
            IType containingType, string name, bool isPublic = false, bool isReadOnly = false, bool isStatic = false, bool isFixed = false, int fixedSize = 0, bool isVirtualTable = false)
        {
            TypeSymbol containingTypeSymbol = null;
            var metadataTypeAdapter = containingType as MetadataTypeAdapter;
            if (metadataTypeAdapter != null)
            {
                containingTypeSymbol = metadataTypeAdapter.TypeDef;
            }

            return
                new MetadataFieldAdapter(
                    new SynthesizedFieldSymbol(
                        containingTypeSymbol as NamedTypeSymbol,
                        isFixed ? new PointerTypeSymbol(this.typeDef) : this.typeDef,
                        name,
                        isPublic,
                        isReadOnly,
                        isStatic),
                    containingTypeSymbol,
                    isFixed ? this.ToPointerType() : this,
                    isFixed,
                    fixedSize,
                    isVirtualTable);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType ToNormal()
        {
            if (!this.UseAsClass && !this.UseAsVirtualTableImplementation && !this.UseAsVirtualTable)
            {
                return this;
            }

            return this.Clone(true, false);
        }

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
        public IParameter ToParameter(string name, bool isOut = false, bool isRef = false)
        {
            Debug.Assert(name != "value");

            var refKind = RefKind.None;
            if (isOut)
            {
                refKind |= RefKind.Out;
            }

            if (isRef)
            {
                refKind |= RefKind.Ref;
            }

            return new MetadataParameterAdapter(new SynthesizedParameterSymbol(null, this.typeDef, 0, refKind, name), false);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType ToPinned()
        {
            var newType = this.typeDef.ToAdapter(false, true);
            Debug.Assert(newType.IsPinned);
            return newType;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType ToPointerType()
        {
            return new PointerTypeSymbol(this.typeDef).ToAdapter();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            return this.lazyToString.Value;
        }

        /// <summary>
        /// </summary>
        /// <param name="interfaceOwner">
        /// </param>
        /// <returns>
        /// </returns>
        public IType ToVirtualTableImplementation(IType interfaceOwner = null)
        {
            if (this.UseAsVirtualTableImplementation)
            {
                return this;
            }

            return this.AsVirtualTableImplementation(interfaceOwner);
        }

        public IType ToVirtualTable()
        {
            if (this.UseAsVirtualTable)
            {
                return this;
            }

            return this.AsVirtualTable();
        }

        /// <summary>
        /// </summary>
        /// <param name="typeDef">
        /// </param>
        /// <param name="recursive">
        /// </param>
        /// <returns>
        /// </returns>
        internal static TypeSymbol GetBareTypeSymbol(TypeSymbol typeDef, bool recursive = true)
        {
            var currentTypeDef = typeDef;
            while (currentTypeDef != null)
            {
                var arrayTypeSymbol = currentTypeDef as ArrayTypeSymbol;
                if (arrayTypeSymbol != null)
                {
                    currentTypeDef = arrayTypeSymbol.ElementType;
                    if (recursive)
                    {
                        continue;
                    }
                }

                var pointerTypeSymbol = currentTypeDef as PointerTypeSymbol;
                if (pointerTypeSymbol != null)
                {
                    currentTypeDef = pointerTypeSymbol.PointedAtType;
                    if (recursive)
                    {
                        continue;
                    }
                }

                return currentTypeDef;
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        internal TypeSymbol GetElementTypeSymbol()
        {
            if (this.IsByRef)
            {
                return this.typeDef;
            }

            if (this.IsArray)
            {
                return (this.typeDef as ArrayTypeSymbol).ElementType;
            }

            if (this.IsPointer)
            {
                return (this.typeDef as PointerTypeSymbol).PointedAtType;
            }

            Debug.Fail(string.Empty);

            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private bool AnyGenericArguments()
        {
            var namedTypeSymbol = this.typeDef as NamedTypeSymbol;
            if (namedTypeSymbol != null && namedTypeSymbol.TypeArguments.Length != 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private bool AnyGenericParameters()
        {
            var namedTypeSymbol = this.typeDef as NamedTypeSymbol;
            if (namedTypeSymbol != null && namedTypeSymbol.TypeParameters.Length != 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
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
        private IType CalculateBaseType()
        {
            return this.typeDef.BaseType != null ? this.typeDef.BaseType.ToAdapter() : null;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private IType CalculateDeclaringType()
        {
            var containingType = GetBareTypeSymbol(this.typeDef).ContainingType;
            if (containingType == null)
            {
                return null;
            }

            return containingType.ToAdapter();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private IType CalculateDeclaringTypeOriginal()
        {
            return new MetadataTypeAdapter(GetBareTypeSymbol(this.typeDef).ContainingType);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private string CalculateFullName()
        {
            var sb = new StringBuilder();
            if (!this.IsGenericParameter)
            {
                this.typeDef.AppendFullNamespace(sb, this.Namespace, this.DeclaringType, false, '.');
            }

            sb.Append(this.Name);
            var result = sb.ToString();

            return result;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private IEnumerable<IType> CalculateGenericArguments()
        {
            var namedTypeSymbol = this.typeDef as NamedTypeSymbol;
            if (namedTypeSymbol != null && namedTypeSymbol.TypeArguments.Length != 0)
            {
                return namedTypeSymbol.TypeArguments.Select(a => a.ToAdapter());
            }

            return new IType[0];
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private IEnumerable<IType> CalculateGenericParameters()
        {
            var namedTypeSymbol = this.typeDef as NamedTypeSymbol;
            if (namedTypeSymbol != null && namedTypeSymbol.TypeParameters.Length != 0)
            {
                return namedTypeSymbol.TypeParameters.Select(a => new MetadataTypeAdapter(a));
            }

            return new IType[0];
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private IType CalculateGetElementType()
        {
            var typeSymbol = this.GetElementTypeSymbol();
            if (typeSymbol != null)
            {
                return typeSymbol.ToAdapter();
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private bool CalculateIsGenericType()
        {
            IType current = this;
            while (current != null)
            {
                if (current.IsGenericTypeLocal)
                {
                    return true;
                }

                if (current.HasElementType && current.GetElementType().IsGenericType)
                {
                    return true;
                }

                if (!current.IsNested)
                {
                    break;
                }

                current = current.DeclaringType;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private bool CalculateIsGenericTypeDefinition()
        {
            IType current = this;
            while (current != null)
            {
                if (current.IsGenericTypeDefinitionLocal)
                {
                    return true;
                }

                if (current.HasElementType && current.ToBareType().IsGenericTypeDefinition)
                {
                    return true;
                }

                if (!current.IsNested)
                {
                    break;
                }

                current = current.DeclaringType;
            }

            return current.IsGenericParameter;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private bool CalculateIsPrimitive()
        {
            if (this.typeDef.IsPrimitiveRecursiveStruct())
            {
                switch (this.typeDef.SpecialType)
                {
                    case SpecialType.System_IntPtr:
                    case SpecialType.System_UIntPtr:
                        return false;
                }

                return true;
            }

            switch (this.typeDef.SpecialType)
            {
                case SpecialType.System_Boolean:
                case SpecialType.System_Byte:
                case SpecialType.System_Char:
                case SpecialType.System_Double:
                case SpecialType.System_Int16:
                case SpecialType.System_Int32:
                case SpecialType.System_Int64:
                case SpecialType.System_UInt16:
                case SpecialType.System_UInt32:
                case SpecialType.System_UInt64:
                case SpecialType.System_SByte:
                case SpecialType.System_Single:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private string CalculateMetadataFullName()
        {
            var sb = new StringBuilder();
            this.typeDef.AppendFullNamespace(sb, this.Namespace, this.DeclaringType, true);
            sb.Append(this.MetadataName);

            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private string CalculateMetadataName()
        {
            var sb = new StringBuilder();

            sb.Append(this.typeDef.Name);

            if (!this.IsGenericParameter)
            {
                if (this.IsGenericType || this.IsGenericTypeDefinition)
                {
                    sb.Append('`');
                    sb.Append(this.typeDef.GetArity());
                }
            }

            if (this.IsArray)
            {
                sb.Append(this.GetElementType().MetadataName);
                sb.Append("[");
                for (var i = 0; i < this.ArrayRank - 1; i++)
                {
                    sb.Append(",");
                }

                sb.Append("]");
            }

            if (this.IsPointer)
            {
                sb.Append(this.GetElementType().MetadataName);
                sb.Append("*");
            }

            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private MetadataModuleAdapter CalculateModule()
        {
            var module = this.typeDef.ContainingModule;
            if (module != null)
            {
                return new MetadataModuleAdapter(module);
            }

            return new MetadataModuleAdapter(GetBareTypeSymbol(this.typeDef).ContainingModule);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private string CalculateName()
        {
            if (this.IsByRef)
            {
                return this.GetElementType().Name;
            }

            var sb = new StringBuilder();

            sb.Append(this.typeDef.Name);

            if (!this.IsGenericParameter)
            {
                if ((this.IsGenericType || this.IsGenericTypeDefinition) && this.typeDef.GetArity() > 0)
                {
                    sb.Append('<');

                    var index = 0;
                    foreach (var genArg in this.GenericTypeArguments)
                    {
                        if (index++ > 0)
                        {
                            sb.Append(",");
                        }

                        sb.Append(genArg.FullName);
                    }

                    sb.Append('>');
                }
            }

            if (this.IsArray)
            {
                sb.Append(this.GetElementType().Name);
                sb.Append("[");
                for (var i = 0; i < this.ArrayRank - 1; i++)
                {
                    sb.Append(",");
                }

                sb.Append("]");
            }

            if (this.IsPointer)
            {
                sb.Append(this.GetElementType().Name);
                sb.Append("*");
            }

            var result = sb.ToString();
            return result;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private string CalculateNamespace()
        {
            return GetBareTypeSymbol(this.typeDef).CalculateNamespace();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
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
                        result.Append("[");
                        for (var i = 0; i < this.ArrayRank - 1; i++)
                        {
                            result.Append(",");
                        }

                        result.Append("]");
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
            return this.BaseType != null && this.BaseType.IsDelegate;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private bool IsDerivedFromEnum()
        {
            return this.BaseType != null && this.BaseType.IsEnum;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private bool IsDerivedFromValueType()
        {
            return this.BaseType != null && this.BaseType.IsValueType;
        }

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        private IEnumerable<IConstructor> IterateConstructors(BindingFlags bindingFlags)
        {
            if (this.typeDef.IsUnboundGenericType())
            {
                return
                    this.typeDef.OriginalDefinition.GetMembers()
                        .Where(m => m is MethodSymbol && this.IsAny(((MethodSymbol)m).MethodKind, MethodKind.Constructor, MethodKind.StaticConstructor))
                        .Select(f => new MetadataConstructorAdapter(f as MethodSymbol));
            }

            return
                this.typeDef.GetMembers()
                    .Where(m => m is MethodSymbol && this.IsAny(((MethodSymbol)m).MethodKind, MethodKind.Constructor, MethodKind.StaticConstructor))
                    .Select(f => new MetadataConstructorAdapter(f as MethodSymbol));
        }

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        private IEnumerable<IField> IterateFields(BindingFlags bindingFlags)
        {
            var typeSymbol = this.typeDef;
            if (typeSymbol.IsUnboundGenericType())
            {
                do
                {
                    foreach (var field in typeSymbol.OriginalDefinition.GetMembers().Where(m => m is FieldSymbol).Select(f => new MetadataFieldAdapter(f as FieldSymbol)))
                    {
                        yield return field;
                    }
                }
                while (bindingFlags.HasFlag(BindingFlags.FlattenHierarchy) && (typeSymbol = typeSymbol.BaseType) != null);
                yield break;
            }

            do
            {
                foreach (var field in typeSymbol.GetMembers().Where(m => m is FieldSymbol).Select(f => new MetadataFieldAdapter(f as FieldSymbol)))
                {
                    yield return field;
                }
            }
            while (bindingFlags.HasFlag(BindingFlags.FlattenHierarchy) && (typeSymbol = typeSymbol.BaseType) != null);
        }

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        private IEnumerable<IMethod> IterateMethods(BindingFlags bindingFlags)
        {
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

            if (this.typeDef.IsUnboundGenericType())
            {
                foreach (var method in
                    this.typeDef.OriginalDefinition.GetMembers()
                        .Where(m => m is MethodSymbol && !this.IsAny(((MethodSymbol)m).MethodKind, MethodKind.Constructor, MethodKind.StaticConstructor))
                        .Select(f => new MetadataMethodAdapter(f as MethodSymbol)))
                {
                    yield return method;
                }
            }
            else
            {
                foreach (var method in
                    this.typeDef.GetMembers()
                        .Where(m => m is MethodSymbol && !this.IsAny(((MethodSymbol)m).MethodKind, MethodKind.Constructor, MethodKind.StaticConstructor))
                        .Select(f => new MetadataMethodAdapter(f as MethodSymbol)))
                {
                    yield return method;
                }
            }
        }
    }
}