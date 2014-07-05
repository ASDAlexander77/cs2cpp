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
        private Lazy<string> lazyNamespace;

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

            this.lazyNamespace = new Lazy<string>(calculateNamespace);
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
                var sb = new StringBuilder();

                sb.Append(this.Namespace);
                if (sb.Length > 0)
                {
                    sb.Append('.');
                }

                if (this.typeDef.ContainingType != null)
                {
                    sb.Append(this.typeDef.ContainingType.Name);
                    sb.Append('+');
                }

                sb.Append(this.typeDef.Name);

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

                return this.typeDef.IsClassType() && !this.IsDerivedFromEnum() && !this.IsDerivedFromValueType();
            }
        }

        public bool IsDelegate
        {
            get
            {
                return this.typeDef.IsDelegateType();
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

        private bool IsDerivedFromEnum()
        {
            return this.BaseType != null && this.BaseType.FullName == "System.Enum";
        }

        private bool IsDerivedFromValueType()
        {
            return this.BaseType != null && this.BaseType.FullName == "System.ValueType";
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

                return this.typeDef.IsValueType || this.IsDerivedFromEnum() || this.IsDerivedFromValueType();
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

        public bool UseAsClass { get; set; }

        public bool IsCCall { get { return false; } }

        public IEnumerable<IType> GetNestedTypes()
        {
            var peType = this.typeDef as PENamedTypeSymbol;
            if (peType != null)
            {
                return peType.GetTypeMembers().Select(t => new MetadataTypeAdapter(t));
            }

            throw new NotImplementedException();
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
                return this.lazyNamespace.Value;
            }
        }

        private string calculateNamespace()
        {
            return this.typeDef.CalculateNamespace();
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

        public IType CreateArray(int rank)
        {
            return new MetadataTypeAdapter(new ArrayTypeSymbol(this.typeDef.ContainingAssembly, this.typeDef, rank: rank));
        }

        public IType CreatePointer()
        {
            return new MetadataTypeAdapter(new PointerTypeSymbol(this.typeDef));
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
            if (this.IsByRef)
            {
                return new MetadataTypeAdapter(this.typeDef);
            }

            if (this.IsArray)
            {
                return new MetadataTypeAdapter((this.typeDef as ArrayTypeSymbol).ElementType);
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
            return this.FullName.GetHashCode();
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