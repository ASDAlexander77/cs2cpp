namespace PEAssemblyReader
{
    using System;
    using System.Linq;

    using Microsoft.CodeAnalysis;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis.CSharp;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    public class PEAssemblyReader
    {
        public PEAssemblyReader(string fileName)
        {
            //var assemblyMetadata = AssemblyMetadata.CreateFromFile(fileName);

            var assembly = AssemblyMetadata.CreateFromFile(fileName).Assembly;
            var assemblySymbol = new PEAssemblySymbol(assembly, DocumentationProvider.Default, isLinked: false, importOptions: MetadataImportOptions.All);

            foreach (var module in assemblySymbol.Modules)
            {
                var typeWithNamespaces = module.TypeWithNamespaceNames.ToArray();

                foreach(var typeWithNamespace in typeWithNamespaces)
                {
                    var metadataTypeName = MetadataTypeName.FromNamespaceAndTypeName(typeWithNamespace.Value, typeWithNamespace.Key);
                    var symbol = module.LookupTopLevelMetadataType(ref metadataTypeName);
                }
            }

            /*
            var assemblyMetadata = AssemblyMetadata.CreateFromFile(fileName);
            var module0 = assemblyMetadata.ManifestModule;

            var decoder = new PEAssemblyReaderMetadataDecoder(module0, assemblyMetadata);
            
            var appDomainType = decoder.FindTypeSymbolByName("System", "AppDomain");
            var method = appDomainType.GetMembers("CreateInstanceAndUnwrap").First() as IMethodSymbol;
            */

            // get method params, 1 param is ReturnType
            //byte callingConvention;
            //BadImageFormatException metadataException;
            //var paramInfos = decoder.GetSignatureForMethod(method, out callingConvention, out metadataException);

            //var parameter = module0.Module.MetadataReader.GetParameter(paramInfos[1].Handle);
            //var parameterName = module0.Module.MetadataReader.GetString(parameter.Name);

            // get method locals
            ////ImmutableArray<MetadataDecoder.LocalInfo> localInfo;
            ////if (!methodBody.LocalSignature.IsNil)
            ////{
            ////    var signature = peModule0.MetadataReader.GetLocalSignature(methodBody.LocalSignature);
            ////    localInfo = decoder.DecodeLocalSignatureOrThrow(signature);
            ////}
            ////else
            ////{
            ////    localInfo = ImmutableArray<MetadataDecoder.LocalInfo>.Empty;
            ////}
        }
    }
}
