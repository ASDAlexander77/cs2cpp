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

            var methodHandle = MetadataTokens.MethodHandle((int)(token - 100663296u));
            var method = peModule.MetadataReader.GetMethod(methodHandle);
            var methodSymbol = new MetadataDecoder(peModuleSymbol).GetSymbolForILToken((Handle)methodHandle) as MethodSymbol;

            return new MetadataMethodAdapter(methodSymbol);
        }

        public IField ResolveField(int token, IType[] typeGenerics, IType[] methodGenerics)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;
            var peModule = peModuleSymbol.Module;

            var fieldHandle = MetadataTokens.FieldHandle((int)(token - 67108864u));
            var typeDef = peModule.MetadataReader.GetField(fieldHandle);
            var fieldSymbol = new MetadataDecoder(peModuleSymbol).GetSymbolForILToken((Handle)fieldHandle) as FieldSymbol;

            return new MetadataFieldAdapter(fieldSymbol);
        }

        public IType ResolveType(int token, IType[] typeGenerics, IType[] methodGenerics)
        {
            var peModuleSymbol = this.moduleDef as PEModuleSymbol;
            var peModule = peModuleSymbol.Module;

            var typedefHandle = MetadataTokens.TypeHandle((int)(token - 33554432u));
            var typeDef = peModule.MetadataReader.GetTypeDefinition(typedefHandle);
            var typeSymbol = new MetadataDecoder(peModuleSymbol).GetSymbolForILToken((Handle)typedefHandle) as TypeSymbol;

            return new MetadataTypeAdapter(typeSymbol);
        }
    }
}
