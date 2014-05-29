namespace PEAssemblyReader
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection.Metadata;

    using Microsoft.Cci;

    using Roslyn.Utilities;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// Helper class to resolve metadata tokens and signatures.
    /// </summary>
    public class MetadataDecoder : MetadataDecoder<ITypeSymbol, IMethodSymbol, IFieldSymbol, IAssemblySymbol, ISymbol>
    {
        private ConcurrentDictionary<TypeHandle, ITypeSymbol> cache = new ConcurrentDictionary<TypeHandle, ITypeSymbol>();

        private AssemblyMetadata assemblyMetadata;

        public MetadataDecoder(ModuleMetadata module, AssemblyMetadata assemblyMetadata) :
            base(module.Module, assemblyMetadata.Assembly.Identity)
        {
            this.assemblyMetadata = assemblyMetadata;
        }

        public IEnumerable<ITypeSymbol> GetTypes()
        {
            foreach (var module in this.assemblyMetadata.Modules)
            {
                foreach (var @namespace in module.GroupTypesByNamespace())
                {
                    foreach (var type in @namespace)
                    {
                        yield return new TypeSymbolAdapter(type, module);
                    }
                }
            }
        }

        public ITypeSymbol FindTypeSymbolByName(string namespaceName, string typeName)
        {
            foreach (var module in this.assemblyMetadata.Modules)
            {
                var @namespace = module.GroupTypesByNamespace().First(g => g.Key == namespaceName);
                if (@namespace == null)
                {
                    continue;
                }

                var type =
                    @namespace.Select(t => new Tuple<TypeHandle, string>(t, module.Module.GetTypeDefNameOrThrow(t))).FirstOrDefault(pair => pair.Item2 == typeName);
                if (type != null)
                {
                    return new TypeSymbolAdapter(type.Item1, module);
                }
            }

            throw new KeyNotFoundException("could not find a type");
        }

        protected override void EnqueueTypeSymbolInterfacesAndBaseTypes(Queue<TypeHandle> typeDefsToSearch, Queue<ITypeSymbol> typeSymbolsToSearch, ITypeSymbol typeSymbol)
        {
            throw new NotImplementedException();
        }

        protected override void EnqueueTypeSymbol(Queue<TypeHandle> typeDefsToSearch, Queue<ITypeSymbol> typeSymbolsToSearch, ITypeSymbol typeSymbol)
        {
            throw new NotImplementedException();
        }

        protected override IMethodSymbol FindMethodSymbolInType(ITypeSymbol type, MethodHandle methodDef)
        {
            throw new NotImplementedException();
        }

        protected override IFieldSymbol FindFieldSymbolInType(ITypeSymbol type, FieldHandle fieldDef)
        {
            throw new NotImplementedException();
        }

        internal override ISymbol GetSymbolForMemberRef(MemberReferenceHandle memberRef, ITypeSymbol implementingTypeSymbol = null, bool methodsOnly = false)
        {
            throw new NotImplementedException();
        }

        protected override MethodHandle GetMethodHandle(IMethodSymbol method)
        {
            throw new NotImplementedException();
        }

        protected override ConcurrentDictionary<TypeHandle, ITypeSymbol> GetTypeHandleToTypeMap()
        {
            return this.cache;
        }

        protected override ConcurrentDictionary<TypeReferenceHandle, ITypeSymbol> GetTypeRefHandleToTypeMap()
        {
            throw new NotImplementedException();
        }

        protected override ITypeSymbol LookupTopLevelTypeDefSymbol(ref MetadataTypeName emittedName, out bool isNoPiaLocalType)
        {
            var namespaceName = emittedName.NamespaceName;
            var typeName = emittedName.TypeName;

            isNoPiaLocalType = false;

            return this.FindTypeSymbolByName(namespaceName, typeName);
        }

        protected override ITypeSymbol SubstituteNoPiaLocalType(TypeHandle typeDef, ref MetadataTypeName name, string interfaceGuid, string scope, string identifier)
        {
            throw new NotImplementedException();
        }

        protected override ITypeSymbol LookupTopLevelTypeDefSymbol(int referencedAssemblyIndex, ref MetadataTypeName emittedName)
        {
            throw new NotImplementedException();
        }

        protected override ITypeSymbol LookupTopLevelTypeDefSymbol(string moduleName, ref MetadataTypeName emittedName, out bool isNoPiaLocalType)
        {
            throw new NotImplementedException();
        }

        protected override ITypeSymbol LookupNestedTypeDefSymbol(ITypeSymbol container, ref MetadataTypeName emittedName)
        {
            throw new NotImplementedException();
        }

        protected override ITypeSymbol GetUnsupportedMetadataTypeSymbol(BadImageFormatException mrEx = null)
        {
            throw new NotImplementedException();
        }

        protected override ITypeSymbol GetByRefReturnTypeSymbol(ITypeSymbol referencedType)
        {
            throw new NotImplementedException();
        }

        protected override ITypeSymbol SubstituteTypeParameters(ITypeSymbol generic, ITypeSymbol[] arguments, bool[] refersToNoPiaLocalType)
        {
            throw new NotImplementedException();
        }

        protected override ITypeSymbol SubstituteWithUnboundIfGeneric(ITypeSymbol type)
        {
            throw new NotImplementedException();
        }

        protected override ITypeSymbol GetGenericTypeParamSymbol(int position)
        {
            throw new NotImplementedException();
        }

        protected override ITypeSymbol GetGenericMethodTypeParamSymbol(int position)
        {
            throw new NotImplementedException();
        }

        protected override ITypeSymbol GetSZArrayTypeSymbol(ITypeSymbol elementType, ImmutableArray<ModifierInfo> customModifiers)
        {
            throw new NotImplementedException();
        }

        protected override ITypeSymbol GetArrayTypeSymbol(int dims, ITypeSymbol elementType)
        {
            throw new NotImplementedException();
        }

        protected override ITypeSymbol MakePointerTypeSymbol(ITypeSymbol type, ImmutableArray<ModifierInfo> customModifiers)
        {
            throw new NotImplementedException();
        }

        protected override ITypeSymbol GetSpecialType(SpecialType specialType)
        {
            return new SpecialTypeSymbolAdapter(specialType);
        }

        protected override ITypeSymbol SystemTypeSymbol
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override ITypeSymbol GetEnumUnderlyingType(ITypeSymbol type)
        {
            throw new NotImplementedException();
        }

        protected override bool IsVolatileModifierType(ITypeSymbol type)
        {
            throw new NotImplementedException();
        }

        protected override PrimitiveTypeCode GetPrimitiveTypeCode(ITypeSymbol type)
        {
            throw new NotImplementedException();
        }
    }
}
