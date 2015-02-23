// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataGenericContext.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    /// <summary>
    /// </summary>
    public class MetadataGenericContext : IGenericContext
    {
        /// <summary>
        /// </summary>
        private IMethod methodSpecialization;

        /// <summary>
        /// </summary>
        private IType typeSpecialization;

        /// <summary>
        /// </summary>
        protected MetadataGenericContext()
            : this(new SortedList<IType, IType>())
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="map">
        /// </param>
        protected MetadataGenericContext(IDictionary<IType, IType> map)
        {
            this.Map = map;
        }

        /// <summary>
        /// </summary>
        /// <param name="map">
        /// </param>
        protected MetadataGenericContext(object[] map)
        {
            this.CustomMap = new Dictionary<string, IType>();
            for (var index = 0; index < map.Length - 1; index++)
            {
                this.CustomMap[map[index].ToString()] = (IType)map[index + 1];
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="allowToUseDefinitionAsSpecialization">
        /// </param>
        protected MetadataGenericContext(IType type, bool allowToUseDefinitionAsSpecialization = false)
            : this()
        {
            this.Init(type, allowToUseDefinitionAsSpecialization);
            if (this.TypeSpecialization != null)
            {
                this.TypeSpecialization.GenericMap(this.Map);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="allowToUseDefinitionAsSpecialization">
        /// </param>
        protected MetadataGenericContext(IMethod method, bool allowToUseDefinitionAsSpecialization = false)
            : this(method.DeclaringType, allowToUseDefinitionAsSpecialization)
        {
            this.Init(method, allowToUseDefinitionAsSpecialization);
            if (this.MethodSpecialization != null)
            {
                this.MethodSpecialization.GenericMap(this.Map);
            }
        }

        /// <summary>
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (this.Map == null || this.Map.Count == 0) && this.TypeDefinition == null && this.TypeSpecialization == null
                       && this.MethodDefinition == null && this.MethodSpecialization == null && (this.CustomMap == null || this.CustomMap.Count == 0);
            }
        }

        public bool IsCustom
        {
            get
            {
                return !(this.CustomMap == null || this.CustomMap.Count == 0);
            }
        }

        /// <summary>
        /// </summary>
        public IDictionary<IType, IType> Map { get; private set; }

        /// <summary>
        /// </summary>
        public IDictionary<string, IType> CustomMap { get; private set; }

        /// <summary>
        /// </summary>
        public IMethod MethodDefinition { get; private set; }

        /// <summary>
        /// </summary>
        public IMethod MethodSpecialization
        {
            get
            {
                return this.methodSpecialization;
            }

            private set
            {
                this.methodSpecialization = value;
                if (this.MethodSpecialization != null)
                {
                    this.MethodSpecialization.GenericMap(this.Map);
                }
            }
        }

        /// <summary>
        /// </summary>
        public IType TypeDefinition { get; set; }

        /// <summary>
        /// </summary>
        public IType TypeSpecialization
        {
            get
            {
                return this.typeSpecialization;
            }

            private set
            {
                this.typeSpecialization = value;
                if (this.TypeSpecialization != null)
                {
                    this.TypeSpecialization.GenericMap(this.Map);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="definitionMethod">
        /// </param>
        /// <param name="specializationMethod">
        /// </param>
        /// <returns>
        /// </returns>
        public static IGenericContext CreateMap(IMethod definitionMethod, IMethod specializationMethod)
        {
            var context = new MetadataGenericContext();
            context.Map.GenericMap(definitionMethod.GetGenericParameters(), specializationMethod.GetGenericArguments());
            context.Map.GenericMap(definitionMethod.DeclaringType.GenericTypeParameters, specializationMethod.DeclaringType.GenericTypeArguments);
            return context;
        }

        public static IGenericContext Create(IType typeDefinition, IType typeSpecialization)
        {
            var context = new MetadataGenericContext(typeDefinition);
            context.TypeSpecialization = typeSpecialization;
            return context;
        }

        public static IGenericContext Create(IType typeDefinition, IType typeSpecialization, IMethod methodDefinition, IMethod methodSpecialization)
        {
            var context = new MetadataGenericContext(typeDefinition);
            context.TypeSpecialization = typeSpecialization;
            context.MethodDefinition = methodDefinition;
            context.MethodSpecialization = methodSpecialization;
            return context;
        }

        public static IGenericContext Create(params object[] map)
        {
            return new MetadataGenericContext(map);
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="allowToUseDefinitionAsSpecialization">
        /// </param>
        /// <returns>
        /// </returns>
        public static IGenericContext DiscoverFrom(IMethod method, bool allowToUseDefinitionAsSpecialization = false)
        {
            if (method.IsGenericMethod || method.IsGenericMethodDefinition)
            {
                return new MetadataGenericContext(method, allowToUseDefinitionAsSpecialization);
            }

            var declType = method.DeclaringType;
            while (declType != null)
            {
                if (declType.IsGenericType || declType.IsGenericTypeDefinition)
                {
                    return new MetadataGenericContext(declType, allowToUseDefinitionAsSpecialization);
                }

                if (declType.IsNested)
                {
                    declType = declType.DeclaringType;
                    continue;
                }

                break;
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="typeParameter">
        /// </param>
        /// <returns>
        /// </returns>
        public IType ResolveTypeParameter(IType typeParameter)
        {
            IType resolved = null;
            if ((this.CustomMap != null && this.CustomMap.TryGetValue(typeParameter.ToString(), out resolved))
                || this.Map.TryGetValue(typeParameter, out resolved))
            {
                if (typeParameter.IsByRef && typeParameter.IsPinned)
                {
                    return resolved.ToByRefTypeAndPinned();
                }

                if (typeParameter.IsByRef)
                {
                    return resolved.ToByRefType();
                }

                if (typeParameter.IsPinned)
                {
                    return resolved.ToPinned();
                }

                return resolved;
            }

            return typeParameter;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="allowToUseDefinitionAsSpecialization">
        /// </param>
        private void Init(IType type, bool allowToUseDefinitionAsSpecialization = false)
        {
            if (type.IsGenericTypeDefinition)
            {
                this.TypeDefinition = type;
                if (allowToUseDefinitionAsSpecialization)
                {
                    this.TypeSpecialization = type;
                }
            }

            if (type.IsGenericType)
            {
                this.TypeSpecialization = type;
                if (this.TypeDefinition == null)
                {
                    this.TypeDefinition = type.GetTypeDefinition();
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="allowToUseDefinitionAsSpecialization">
        /// </param>
        private void Init(IMethod method, bool allowToUseDefinitionAsSpecialization = false)
        {
            if (method.IsGenericMethodDefinition)
            {
                this.MethodDefinition = method;
                if (allowToUseDefinitionAsSpecialization)
                {
                    this.MethodSpecialization = method;
                }
            }

            if (method.IsGenericMethod)
            {
                this.MethodSpecialization = method;
                if (this.MethodDefinition == null)
                {
                    this.MethodDefinition = method.GetMethodDefinition();
                }
            }
        }
    }
}