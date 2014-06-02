using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PEAssemblyReader
{
    public class MetadataModuleAdapter : IModule
    {
        private ModuleSymbol moduleDef { get; set; }

        internal MetadataModuleAdapter(ModuleSymbol moduleDef)
        {
            this.moduleDef = moduleDef;
        }

        public string ResolveString(int token)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;
            var peModule = peModuleSymbol.Module;

            var stringHandle = MetadataTokens.StringHandle(token);
            var stringValue = peModule.MetadataReader.GetString(stringHandle);
            if (stringValue != null)
            {
                return stringValue;
            }

            // try to resolve in other modules
            foreach (var assemblySymbol in peModuleSymbol.GetReferencedAssemblySymbols())
            {
                foreach (var moduleInAssemblySymbol in assemblySymbol.Modules)
                {
                    stringValue = (moduleInAssemblySymbol as PEModuleSymbol).Module.MetadataReader.GetString(stringHandle);
                    if (stringValue != null)
                    {
                        return stringValue;
                    }
                }
            }

            throw new NotImplementedException();
        }

        public IMember ResolveMember(int token, IType[] typeGenerics, IType[] methodGenerics)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;
            var peModule = peModuleSymbol.Module;

            var methodHandle = MetadataTokens.MethodHandle((int)(token - 100663296u));
            var method = peModule.MetadataReader.GetMethod(methodHandle);
            var methodSymbol = new MetadataDecoder(peModuleSymbol).GetSymbolForILToken((Handle)methodHandle) as MethodSymbol;
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

        public IMethod ResolveMethod(int token, IType[] typeGenerics, IType[] methodGenerics)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;
            var peModule = peModuleSymbol.Module;

            var methodHandle = MetadataTokens.MethodHandle(token);
            var methodSymbol = new MetadataDecoder(peModuleSymbol).GetSymbolForILToken((Handle)methodHandle) as MethodSymbol;

            if (methodSymbol != null)
            {
                return new MetadataMethodAdapter(methodSymbol);
            }

            // try to resolve in other modules
            foreach (var assemblySymbol in peModuleSymbol.GetReferencedAssemblySymbols())
            {
                foreach (var moduleInAssemblySymbol in assemblySymbol.Modules)
                {
                    methodSymbol = new MetadataDecoder(moduleInAssemblySymbol as PEModuleSymbol).GetSymbolForILToken((Handle)methodHandle) as MethodSymbol;
                    if (methodSymbol != null)
                    {
                        return new MetadataMethodAdapter(methodSymbol);
                    }
                }
            }

            throw new KeyNotFoundException();
        }

        public IField ResolveField(int token, IType[] typeGenerics, IType[] methodGenerics)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;
            var peModule = peModuleSymbol.Module;

            var fieldHandle = MetadataTokens.FieldHandle(token);
            var fieldSymbol = new MetadataDecoder(peModuleSymbol).GetSymbolForILToken((Handle)fieldHandle) as FieldSymbol;

            if (fieldSymbol != null)
            {
                return new MetadataFieldAdapter(fieldSymbol);
            }

            // try to resolve in other modules
            foreach (var assemblySymbol in peModuleSymbol.GetReferencedAssemblySymbols())
            {
                foreach (var moduleInAssemblySymbol in assemblySymbol.Modules)
                {
                    fieldSymbol = new MetadataDecoder(moduleInAssemblySymbol as PEModuleSymbol).GetSymbolForILToken((Handle)fieldHandle) as FieldSymbol;
                    if (fieldSymbol != null)
                    {
                        return new MetadataFieldAdapter(fieldSymbol);
                    }
                }
            }

            throw new KeyNotFoundException();
        }

        public IType ResolveType(int token, IType[] typeGenerics, IType[] methodGenerics)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;
            var peModule = peModuleSymbol.Module;

            var typedefHandle = MetadataTokens.TypeHandle(token);
            var typeSymbol = new MetadataDecoder(peModuleSymbol).GetSymbolForILToken((Handle)typedefHandle) as TypeSymbol;

            if (typeSymbol != null)
            {
                return new MetadataTypeAdapter(typeSymbol);
            }

            // try to resolve in other modules
            foreach (var assemblySymbol in peModuleSymbol.GetReferencedAssemblySymbols())
            {
                foreach (var moduleInAssemblySymbol in assemblySymbol.Modules)
                {
                    typeSymbol = new MetadataDecoder(moduleInAssemblySymbol as PEModuleSymbol).GetSymbolForILToken((Handle)typedefHandle) as TypeSymbol;
                    if (typeSymbol != null)
                    {
                        return new MetadataTypeAdapter(typeSymbol);
                    }
                }
            }

            throw new KeyNotFoundException();
        }
    }
}
