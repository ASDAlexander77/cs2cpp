// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;

    public class NamedTypeImpl : TypeImpl, INamedTypeSymbol
    {
        public NamedTypeImpl()
        {
        }

        public NamedTypeImpl(INamedTypeSymbol typeSymbol)
            : base(typeSymbol)
        {
            Arity = typeSymbol.Arity;
            IsGenericType = typeSymbol.IsGenericType;
            IsUnboundGenericType = typeSymbol.IsUnboundGenericType;
            IsScriptClass = typeSymbol.IsScriptClass;
            IsImplicitClass = typeSymbol.IsImplicitClass;
            MemberNames = typeSymbol.MemberNames;
            TypeParameters = typeSymbol.TypeParameters;
            TypeArguments = typeSymbol.TypeArguments;
            OriginalDefinition = typeSymbol.OriginalDefinition;
            DelegateInvokeMethod = typeSymbol.DelegateInvokeMethod;
            EnumUnderlyingType = typeSymbol.EnumUnderlyingType;
            ConstructedFrom = typeSymbol.ConstructedFrom;
            InstanceConstructors = typeSymbol.InstanceConstructors;
            StaticConstructors = typeSymbol.StaticConstructors;
            Constructors = typeSymbol.Constructors;
            AssociatedSymbol = typeSymbol.AssociatedSymbol;
            MightContainExtensionMethods = typeSymbol.MightContainExtensionMethods;
        }

        public int Arity { get; private set; }

        public bool IsGenericType { get; set; }

        public bool IsUnboundGenericType { get; private set; }

        public bool IsScriptClass { get; private set; }

        public bool IsImplicitClass { get; private set; }

        public IEnumerable<string> MemberNames { get; private set; }

        public ImmutableArray<ITypeParameterSymbol> TypeParameters { get; set; }

        public ImmutableArray<ITypeSymbol> TypeArguments { get; set; }

        public INamedTypeSymbol OriginalDefinition { get; private set; }

        public IMethodSymbol DelegateInvokeMethod { get; private set; }

        public INamedTypeSymbol EnumUnderlyingType { get; private set; }

        public INamedTypeSymbol ConstructedFrom { get; private set; }

        public INamedTypeSymbol Construct(params ITypeSymbol[] typeArguments)
        {
            throw new NotImplementedException();
        }

        public INamedTypeSymbol ConstructUnboundGenericType()
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<CustomModifier> GetTypeArgumentCustomModifiers(int ordinal)
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<IMethodSymbol> InstanceConstructors { get; private set; }
        public ImmutableArray<IMethodSymbol> StaticConstructors { get; private set; }
        public ImmutableArray<IMethodSymbol> Constructors { get; private set; }
        public ISymbol AssociatedSymbol { get; private set; }
        public bool MightContainExtensionMethods { get; private set; }

        public bool IsComImport => throw new NotImplementedException();

        public INamedTypeSymbol TupleUnderlyingType => throw new NotImplementedException();

        public ImmutableArray<IFieldSymbol> TupleElements => throw new NotImplementedException();
    }
}
