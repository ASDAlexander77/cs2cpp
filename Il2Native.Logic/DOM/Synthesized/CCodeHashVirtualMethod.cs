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

    public class CCodeHashVirtualMethod : CCodeMethodDeclaration
    {
        public CCodeHashVirtualMethod(INamedTypeSymbol type)
            : base(new CCodeHashMethod(type))
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
                        ExpressionOpt =
                            new Call
                            {
                                Type = type,
                                Method =
                                    new MethodImpl
                                    {
                                        Name = "__hash_code",
                                        Parameters = ImmutableArray.Create<IParameterSymbol>(
                                            new ParameterImpl { Name = "_class", Type = type },
                                            new ParameterImpl
                                            {
                                                Name = "_size",
                                                Type = new TypeImpl { SpecialType = SpecialType.System_Int32 }
                                            })
                                    },
                                Arguments =
                                {
                                    new ThisReference { Type = type, ValueAsReference = true },
                                    new SizeOfOperator { SourceType = new TypeExpression { Type = new ValueTypeAsClassTypeImpl(type), SuppressReference = true } }
                                }
                            }
                    }
                }
            };
        }

        public class CCodeHashMethod : MethodImpl
        {
            public CCodeHashMethod(INamedTypeSymbol type)
            {
                Name = "__hash";
                MetadataName = Name;
                MethodKind = MethodKind.Ordinary;
                ContainingType = type;
                IsVirtual = true;
                IsOverride = type.BaseType != null;
                ReturnType = new NamedTypeImpl { SpecialType = SpecialType.System_Int32 };
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
            }
        }
    }
}
