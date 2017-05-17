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

    public class CCodeEqualsVirtualMethod : CCodeMethodDeclaration
    {
        public CCodeEqualsVirtualMethod(INamedTypeSymbol type)
            : base(type, new CCodeEqualsMethod(type))
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
                                        Name = "__equals_helper",
                                        Parameters = ImmutableArray.Create<IParameterSymbol>(
                                            new ParameterImpl
                                            {
                                                Name = "_obj1",
                                                Type = new TypeImpl { SpecialType = SpecialType.System_Object }
                                            },
                                            new ParameterImpl
                                            {
                                                Name = "_obj2",
                                                Type = new TypeImpl { SpecialType = SpecialType.System_Object }
                                            })
                                    },
                                Arguments =
                                {
                                    new ThisReference { Type = type, ValueAsReference = true },
                                    new Parameter
                                    {
                                        ParameterSymbol =
                                            new ParameterImpl
                                            {
                                                Name = "obj",
                                                Type = new TypeImpl { SpecialType = SpecialType.System_Object }
                                            }
                                    }
                                }
                            }
                    }
                }
            };
        }

        public class CCodeEqualsMethod : MethodImpl
        {
            public CCodeEqualsMethod(INamedTypeSymbol type)
            {
                Name = "__equals";
                MetadataName = Name;
                MethodKind = MethodKind.Ordinary;
                ContainingType = type;
                IsVirtual = true;
                IsOverride = type.BaseType != null;
                ReturnType = new NamedTypeImpl { SpecialType = SpecialType.System_Boolean };
                Parameters = ImmutableArray.Create<IParameterSymbol>(
                    new ParameterImpl
                    {
                        Name = "obj",
                        Type = new TypeImpl { SpecialType = SpecialType.System_Object }
                    });
            }
        }
    }
}
