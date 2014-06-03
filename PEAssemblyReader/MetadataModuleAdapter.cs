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
        public IField ResolveField(int token, IType[] typeGenerics, IType[] methodGenerics)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            Handle fieldHandle = MetadataTokens.Handle(token);
            var fieldSymbol = new MetadataDecoder(peModuleSymbol).GetSymbolForILToken(fieldHandle) as FieldSymbol;

            if (fieldSymbol != null)
            {
                return new MetadataFieldAdapter(fieldSymbol);
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
        public IMember ResolveMember(int token, IType[] typeGenerics, IType[] methodGenerics)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            Handle methodHandle = MetadataTokens.Handle(token);
            var methodSymbol = new MetadataDecoder(peModuleSymbol).GetSymbolForILToken(methodHandle) as MethodSymbol;
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
        public IMethod ResolveMethod(int token, IType[] typeGenerics, IType[] methodGenerics)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            Handle methodHandle = MetadataTokens.Handle(token);
            var methodSymbol = new MetadataDecoder(peModuleSymbol).GetSymbolForILToken(methodHandle) as MethodSymbol;

            if (methodSymbol != null)
            {
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
        public IType ResolveType(int token, IType[] typeGenerics, IType[] methodGenerics)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;

            Handle typedefHandle = MetadataTokens.Handle(token);
            var typeSymbol = new MetadataDecoder(peModuleSymbol).GetSymbolForILToken(typedefHandle) as TypeSymbol;

            if (typeSymbol != null)
            {
                return new MetadataTypeAdapter(typeSymbol);
            }

            throw new KeyNotFoundException();
        }
    }
}