// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataModuleAdapter.cs" company="">
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
    using System.Reflection.Metadata;
    using System.Reflection.Metadata.Ecma335;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    /// <summary>
    /// </summary>
    public class MetadataModuleAdapter : IModule
    {
        /// <summary>
        /// </summary>
        /// <param name="moduleDef">
        /// </param>
        internal MetadataModuleAdapter(ModuleSymbol moduleDef)
        {
            this.moduleDef = moduleDef;
        }

        /// <summary>
        /// </summary>
        private ModuleSymbol moduleDef { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <param name="typeGenerics">
        /// </param>
        /// <param name="methodGenerics">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public IField ResolveField(int token, IType genericTypeContextOpt, IType genericTypeSpecializationContextOpt)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            var fieldHandle = MetadataTokens.Handle(token);

            var context = genericTypeContextOpt != null ? ((MetadataTypeAdapter)genericTypeContextOpt).TypeDef as PENamedTypeSymbol : null;
            var fieldSymbol = new MetadataDecoder(peModuleSymbol, context).GetSymbolForILToken(fieldHandle) as FieldSymbol;

            if (fieldSymbol != null)
            {
                return new MetadataFieldAdapter(fieldSymbol, genericTypeSpecializationContextOpt);
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <param name="typeGenerics">
        /// </param>
        /// <param name="methodGenerics">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IMember ResolveMember(int token, IType genericTypeContextOpt, IType genericTypeSpecializationContextOpt)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            var methodHandle = MetadataTokens.Handle(token);

            var context = genericTypeContextOpt != null ? ((MetadataTypeAdapter)genericTypeContextOpt).TypeDef as PENamedTypeSymbol : null;
            var methodSymbol = new MetadataDecoder(peModuleSymbol, context).GetSymbolForILToken(methodHandle) as MethodSymbol;
            if (methodSymbol != null)
            {
                if (methodSymbol.HasSpecialName && (methodSymbol.Name == ".ctor" || methodSymbol.Name == "..ctor"))
                {
                    return new MetadataConstructorAdapter(methodSymbol);
                }

                return new MetadataMethodAdapter(methodSymbol);
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <param name="typeGenerics">
        /// </param>
        /// <param name="methodGenerics">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public IMethod ResolveMethod(int token, IType genericTypeContextOpt, IType genericTypeSpecializationContextOpt)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            var methodHandle = MetadataTokens.Handle(token);

            var context = genericTypeContextOpt != null ? ((MetadataTypeAdapter)genericTypeContextOpt).TypeDef as PENamedTypeSymbol : null;
            var methodSymbol = new MetadataDecoder(peModuleSymbol, context).GetSymbolForILToken(methodHandle) as MethodSymbol;

            if (methodSymbol != null)
            {
                if (methodSymbol.HasSpecialName && (methodSymbol.Name == ".ctor" || methodSymbol.Name == "..ctor"))
                {
                    return new MetadataConstructorAdapter(methodSymbol);
                }

                return new MetadataMethodAdapter(methodSymbol);
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public string ResolveString(int token)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;
            var peModule = peModuleSymbol.Module;

            var stringHandle = MetadataTokens.Handle(token);

            switch (stringHandle.HandleType)
            {
                case HandleType.UserString:
                    return peModule.MetadataReader.GetUserString((UserStringHandle)stringHandle);
                case HandleType.String:
                    return peModule.MetadataReader.GetString((StringHandle)stringHandle);
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <param name="typeGenerics">
        /// </param>
        /// <param name="methodGenerics">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public IType ResolveType(int token, IType genericTypeContextOpt, IType genericTypeSpecializationContextOpt)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            var typedefHandle = MetadataTokens.Handle(token);

            var context = genericTypeContextOpt != null ? ((MetadataTypeAdapter)genericTypeContextOpt).TypeDef as PENamedTypeSymbol : null;
            var typeSymbol = new MetadataDecoder(peModuleSymbol, context).GetSymbolForILToken(typedefHandle) as TypeSymbol;

            if (typeSymbol != null)
            {
                return new MetadataTypeAdapter(typeSymbol);
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// </summary>
        /// <param name="fullName">
        /// </param>
        /// <param name="typeGenerics">
        /// </param>
        /// <param name="methodGenerics">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public IType ResolveType(string fullName, IType genericTypeContextOpt, IType genericTypeSpecializationContextOpt)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            var context = genericTypeContextOpt != null ? ((MetadataTypeAdapter)genericTypeContextOpt).TypeDef as PENamedTypeSymbol : null;
            var typeSymbol = new MetadataDecoder(peModuleSymbol, context).GetTypeSymbolForSerializedType(fullName);

            if (typeSymbol.TypeKind == TypeKind.Error)
            {
                // try to find it in CoreLib
                typeSymbol =
                    new MetadataDecoder(peModuleSymbol.ContainingAssembly.CorLibrary.Modules[0] as PEModuleSymbol).GetTypeSymbolForSerializedType(fullName);
            }

            if (typeSymbol != null && typeSymbol.TypeKind != TypeKind.Error)
            {
                return new MetadataTypeAdapter(typeSymbol);
            }

            throw new KeyNotFoundException();
        }
    }
}