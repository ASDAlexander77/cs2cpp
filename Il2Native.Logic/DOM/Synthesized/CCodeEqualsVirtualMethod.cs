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

    public class CCodeEqualsVirtualMethod : CCodeInternalImplementationMethodDeclaration
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
                                        Parameters = ImmutableArray.Create(
                                            SpecialType.System_Object.ToType().ToParameter("_obj1"),
                                            SpecialType.System_Object.ToType().ToParameter("_obj2"))
                                    },
                                Arguments =
                                {
                                    new ThisReference { Type = type, ValueAsReference = true },
                                    new Parameter
                                    {
                                        ParameterSymbol =
                                            SpecialType.System_Object.ToType().ToParameter("obj")
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
                ReturnType = SpecialType.System_Boolean.ToType();
                Parameters = ImmutableArray.Create(SpecialType.System_Object.ToType().ToParameter("obj"));
            }
        }
    }
}
