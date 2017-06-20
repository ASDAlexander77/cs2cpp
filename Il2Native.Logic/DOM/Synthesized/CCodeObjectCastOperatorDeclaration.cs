// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeObjectCastOperatorDeclaration : CCodeInternalImplementationMethodDeclaration
    {
        public CCodeObjectCastOperatorDeclaration(INamedTypeSymbol type)
            : this(type, type)
        {
        }

        public CCodeObjectCastOperatorDeclaration(INamedTypeSymbol type, ITypeSymbol receiverType)
            : base(type, new ObjectCastOperatorMethod(type, receiverType))
        {
        }

        public class ObjectCastOperatorMethod : MethodImpl
        {
            public ObjectCastOperatorMethod(INamedTypeSymbol type, ITypeSymbol receiverType)
            {
                Name = @"@operator object*";
                MethodKind = MethodKind.BuiltinOperator;
                ReceiverType = receiverType;
                ContainingType = type;
                ContainingNamespace = type.ContainingNamespace;
                IsVirtual = true;
                IsAbstract = receiverType.TypeKind == TypeKind.Interface;
                IsOverride = receiverType.TypeKind != TypeKind.Interface;
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
                ReturnType = null;
                ReturnsVoid = false;
            }
        }
    }
}
