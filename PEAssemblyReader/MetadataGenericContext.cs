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
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    // TODO: using generic context is not correct now, (or get rid of it, or make it right, see gtest-256.cs)
    /// <summary>
    /// </summary>
    public class MetadataGenericContext : IGenericContext
    {
        /// <summary>
        /// </summary>
        protected MetadataGenericContext()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="map">
        /// </param>
        protected MetadataGenericContext(object[] map)
        {
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
        }

        /// <summary>
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.TypeDefinition == null && this.TypeSpecialization == null
                       && this.MethodDefinition == null && this.MethodSpecialization == null;
            }
        }

        public bool IsCustom
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        public IMethod MethodDefinition { get; private set; }

        /// <summary>
        /// </summary>
        public IMethod MethodSpecialization { get; private set; }

        /// <summary>
        /// </summary>
        public IType TypeDefinition { get; set; }

        /// <summary>
        /// </summary>
        public IType TypeSpecialization { get; private set; }

        public IGenericContext Clone()
        {
            return (IGenericContext)this.MemberwiseClone();
        }

        public void AppendMap(IGenericContext genericContext)
        {
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

            return DiscoverFrom(method.DeclaringType, allowToUseDefinitionAsSpecialization);
        }

        public static IGenericContext DiscoverFrom(IType declType, bool allowToUseDefinitionAsSpecialization = false)
        {
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
            if (type == null)
            {
                return;
            }

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
            if (method == null)
            {
                return;
            }

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