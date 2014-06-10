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
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

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
        }

        /// <summary>
        /// </summary>
        public string AssemblyQualifiedName
        {
            get
            {
                var effective = this.typeDef;

                if (this.typeDef.TypeKind == TypeKind.ArrayType)
                {
                    effective = this.GetElementTypeSymbol();
                }

                return effective.ContainingAssembly.Identity.ToAssemblyName().FullName;
            }
        }

        /// <summary>
        /// </summary>
        public IType BaseType
        {
            get
            {
                return this.typeDef.BaseType != null ? new MetadataTypeAdapter(this.typeDef.BaseType) : null;
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
                throw new NotImplementedException();
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
                var metadataTypeName = this.typeDef.ContainingNamespace != null
                                           ? MetadataTypeName.FromNamespaceAndTypeName(this.typeDef.ContainingNamespace.ToString(), this.typeDef.Name)
                                           : MetadataTypeName.FromTypeName(this.typeDef.Name);
                return metadataTypeName.FullName;
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
                return this.typeDef.IsClassType();
            }
        }

        /// <summary>
        /// </summary>
        public bool IsEnum
        {
            get
            {
                return this.typeDef.IsEnumType();
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
                return this.typeDef.ContainsTypeParameter();
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
                    return namedTypeSymbol.TypeParameters.Any() && !namedTypeSymbol.TypeArguments.Any();
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
                return this.typeDef.IsPrimitiveRecursiveStruct();
            }
        }

        /// <summary>
        /// </summary>
        public bool IsValueType
        {
            get
            {
                return this.typeDef.IsValueType;
            }
        }

        /// <summary>
        /// </summary>
        public string Name
        {
            get
            {
                return this.typeDef.Name;
            }
        }

        /// <summary>
        /// </summary>
        public string Namespace
        {
            get
            {
                return this.typeDef.ContainingNamespace != null ? this.typeDef.ContainingNamespace.Name : string.Empty;
            }
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

            var val = type.Name.CompareTo(this.Name);
            if (val != 0)
            {
                return val;
            }

            val = type.Namespace.CompareTo(this.Namespace);
            if (val != 0)
            {
                return val;
            }

            return 0;
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
                    .Where(m => m is PEMethodSymbol && this.IsAny(((PEMethodSymbol)m).MethodKind, MethodKind.Constructor, MethodKind.StaticConstructor))
                    .Select(f => new MetadataConstructorAdapter(f as PEMethodSymbol));
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType GetElementType()
        {
            if (this.IsArray)
            {
                return new MetadataTypeAdapter((this.typeDef as ArrayTypeSymbol).ElementType);
            }

            if (this.IsByRef)
            {
                return new MetadataTypeAdapter(this.typeDef);
            }

            if (this.IsPointer)
            {
                return new MetadataTypeAdapter((this.typeDef as PointerTypeSymbol).PointedAtType);
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
                return new MetadataTypeAdapter(this.typeDef.EnumUnderlyingType());
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
            return this.typeDef.GetMembers().Where(m => m is PEFieldSymbol).Select(f => new MetadataFieldAdapter(f as FieldSymbol));
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
                return namedTypeSymbol.TypeArguments.Select(a => new MetadataTypeAdapter(a));
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<IType> GetInterfaces()
        {
            return this.typeDef.AllInterfaces.Select(i => new MetadataTypeAdapter(i));
        }

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        // TODO: finish filter by public etc
        public IEnumerable<IMethod> GetMethods(BindingFlags bindingFlags)
        {
            var filterPublic = bindingFlags.HasFlag(BindingFlags.Public);

            foreach (var method in
                this.typeDef.GetMembers()
                    .Where(m => m is PEMethodSymbol && !this.IsAny(((PEMethodSymbol)m).MethodKind, MethodKind.Constructor, MethodKind.StaticConstructor))
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
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public bool IsAssignableFrom(IType type)
        {
            return type.IsDerivedFrom(this) || type.GetInterfaces().Contains(this);
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
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
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

                if (this.IsPointer)
                {
                    result.Append('*');
                }

                if (this.IsByRef)
                {
                    result.Append('&');
                }

                if (this.IsArray)
                {
                    result.Append("[]");
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
        private TypeSymbol GetElementTypeSymbol()
        {
            if (this.IsArray)
            {
                return (this.typeDef as ArrayTypeSymbol).ElementType;
            }

            if (this.IsByRef)
            {
                return this.typeDef;
            }

            if (this.IsPointer)
            {
                return (this.typeDef as PointerTypeSymbol).PointedAtType;
            }

            return null;
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
    }
}