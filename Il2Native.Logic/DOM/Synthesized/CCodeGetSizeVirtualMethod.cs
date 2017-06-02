// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System;
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeGetSizeVirtualMethod : CCodeInternalImplementationMethodDeclaration
    {
        public CCodeGetSizeVirtualMethod(INamedTypeSymbol type)
            : base(type, new CCodeGetSizeMethod(type))
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            MethodBodyOpt = new MethodBody(Method)
            {
                Statements =
                {
                    new ReturnStatement()
                    {
                        ExpressionOpt = new SizeOfOperator { SourceType = new TypeExpression { Type = new ValueTypeAsClassTypeImpl(type), SuppressReference = true } }
                    }
                }
            };
        }

        public class CCodeGetSizeMethod : MethodImpl
        {
            public CCodeGetSizeMethod(INamedTypeSymbol type)
            {
                Name = "__get_size";
                MetadataName = Name;
                MethodKind = MethodKind.Ordinary;
                ContainingType = type;
                ContainingNamespace = type.ContainingNamespace;
                IsVirtual = true;
                IsOverride = type.BaseType != null;
                ReturnType = new NamedTypeImpl { SpecialType = SpecialType.System_UInt32 };
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
            }
        }
    }
}
