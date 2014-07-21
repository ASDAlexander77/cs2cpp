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
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public IField ResolveField(int token, IGenericContext genericContext)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            var fieldHandle = MetadataTokens.Handle(token);

            var fieldSymbol = peModuleSymbol.GetMetadataDecoder(genericContext).GetSymbolForILToken(fieldHandle) as FieldSymbol;
            if (fieldSymbol != null)
            {
                return new MetadataFieldAdapter(fieldSymbol, genericContext);
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IMember ResolveMember(int token, IGenericContext genericContext)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            var methodHandle = MetadataTokens.Handle(token);

            var methodSymbol = peModuleSymbol.GetMetadataDecoder(genericContext).GetSymbolForILToken(methodHandle) as MethodSymbol;
            if (methodSymbol != null)
            {
                if (methodSymbol.MethodKind == MethodKind.Constructor)
                {
                    return new MetadataConstructorAdapter(methodSymbol, genericContext);
                }

                return new MetadataMethodAdapter(methodSymbol, genericContext);
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public IMethod ResolveMethod(int token, IGenericContext genericContext)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            var methodHandle = MetadataTokens.Handle(token);

            var methodSymbol = peModuleSymbol.GetMetadataDecoder(genericContext).GetSymbolForILToken(methodHandle) as MethodSymbol;
            if (methodSymbol != null)
            {
                if (methodSymbol.MethodKind == MethodKind.Constructor)
                {
                    return new MetadataConstructorAdapter(methodSymbol, genericContext);
                }

                return new MetadataMethodAdapter(methodSymbol, genericContext);
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
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public IType ResolveType(int token, IGenericContext genericContext)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            var typedefHandle = MetadataTokens.Handle(token);

            var typeSymbol = peModuleSymbol.GetMetadataDecoder(genericContext).GetSymbolForILToken(typedefHandle) as TypeSymbol;
            if (typeSymbol != null && typeSymbol.TypeKind != TypeKind.Error)
            {
                return typeSymbol.ResolveGeneric(genericContext);
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// </summary>
        /// <param name="fullName">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public IType ResolveType(string fullName, IGenericContext genericContext)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            var typeSymbol = peModuleSymbol.GetMetadataDecoder(genericContext).GetTypeSymbolForSerializedType(fullName);
            if (typeSymbol.TypeKind == TypeKind.Error)
            {
                // try to find it in CoreLib
                typeSymbol =
                    new MetadataDecoder(peModuleSymbol.ContainingAssembly.CorLibrary.Modules[0] as PEModuleSymbol).GetTypeSymbolForSerializedType(fullName);
            }

            if (typeSymbol != null && typeSymbol.TypeKind != TypeKind.Error)
            {
                return typeSymbol.ResolveGeneric(genericContext);
            }

            throw new KeyNotFoundException();
        }
    }
}