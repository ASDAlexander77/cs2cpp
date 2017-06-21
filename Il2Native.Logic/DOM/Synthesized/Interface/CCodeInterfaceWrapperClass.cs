﻿// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Text;

    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;
    using Synthesized;
    using System.Diagnostics;

    public class CCodeInterfaceWrapperClass : CCodeClass
    {
        private readonly INamedTypeSymbol @interface;

        public CCodeInterfaceWrapperClass(INamedTypeSymbol type, INamedTypeSymbol @interface)
            : base(type.IsValueType ? new ValueTypeAsClassTypeImpl(type) : type)
        {
            this.@interface = @interface;
            this.CreateMemebers();
            this.GetMembersImplementation();
        }

        public static string GetName(INamedTypeSymbol type, INamedTypeSymbol @interface)
        {
            var sb = new StringBuilder();
            ////sb.Append(type.MetadataName.CleanUpName());
            ////sb.Append("_");
            GetGenericNameRecursive(sb, @interface);
            return sb.ToString();
        }

        public static NamedTypeImpl GetReceiver(INamedTypeSymbol namedTypeSymbol, INamedTypeSymbol @interface)
        {
            return new NamedTypeImpl { Name = GetName(namedTypeSymbol, @interface), ContainingType = namedTypeSymbol };
        }

        public void GetMembersImplementation()
        {
            var namedTypeSymbol = (INamedTypeSymbol)Type;
            Definitions.Add(new CCodeObjectCastOperatorDefinition(namedTypeSymbol, GetReceiver(namedTypeSymbol, this.@interface)));

            foreach (var method in this.@interface.GetMembers()
                .OfType<IMethodSymbol>()
                .Union(this.@interface.EnumerateInterfaceMethods())
                .Select(this.CreateWrapperMethod)
                .Select(m => new CCodeMethodDefinition(m) { MethodBodyOpt = this.CreateMethodBody(m) }))
            {
                Definitions.Add(method);
            }
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("class");
            c.WhiteSpace();
            this.Name(c);

            c.WhiteSpace();
            c.TextSpan(":");
            c.WhiteSpace();
            c.TextSpan("public");
            c.WhiteSpace();
            c.WriteTypeFullName(this.@interface);
            c.NewLine();
            c.OpenBlock();

            c.DecrementIndent();
            c.TextSpanNewLine("public:");
            c.IncrementIndent();

            // write default constructor
            this.Name(c);
            c.TextSpan("(");
            c.WriteType(Type, false, true, true);
            c.WhiteSpace();
            c.TextSpan("class_");
            c.TextSpan(")");
            c.WhiteSpace();
            c.TextSpan(":");
            c.WhiteSpace();
            c.TextSpan("_class{class_}");
            c.WhiteSpace();
            c.TextSpanNewLine("{}");

            foreach (var declaration in Declarations)
            {
                declaration.WriteTo(c);
            }

            c.EndBlockWithoutNewLine();
        }

        public static void GetGenericNameRecursive(StringBuilder sb, INamedTypeSymbol @interface)
        {
            sb.Append(@interface.MetadataName.CleanUpName());
            GetGenericArgumentsRecursive(sb, @interface);
        }

        public static void GetGenericArgumentsRecursive(StringBuilder sb, INamedTypeSymbol @interface)
        {
            if (@interface.IsGenericType)
            {
                sb.Append("_");
                foreach (var typeArg in @interface.GetTemplateArguments())
                {
                    var namedType = typeArg as INamedTypeSymbol;
                    if (namedType != null)
                    {
                        GetGenericNameRecursive(sb, namedType);
                    }
                    else
                    {
                        sb.Append(typeArg.MetadataName.CleanUpName());
                    }
                }
            }
        }

        private void CreateMemebers()
        {
            var namedTypeSymbol = (INamedTypeSymbol)Type;
            Declarations.Add(new CCodeFieldDeclaration(new FieldImpl { Name = "_class", Type = Type }));
            Declarations.Add(new CCodeObjectCastOperatorDeclaration(namedTypeSymbol, GetReceiver(namedTypeSymbol, this.@interface)));
            foreach (var interfaceMethod in this.@interface.GetMembers().OfType<IMethodSymbol>().Union(this.@interface.EnumerateInterfaceMethods()))
            {
                Declarations.Add(new CCodeMethodDeclaration(Type, this.CreateWrapperMethod(interfaceMethod)));
            }
        }

        private MethodBody CreateMethodBody(IMethodSymbol interfaceMethod)
        {
            var actualMethodToCall = interfaceMethod;

            var callGenericMethodFromInterfaceMethod = false;
            if (Type.TypeKind != TypeKind.Interface)
            {
                var implementationForInterfaceMember = Type.FindImplementationForInterfaceMember(interfaceMethod.OriginalDefinition) as IMethodSymbol;
                Debug.Assert(implementationForInterfaceMember != null, "Method for interface can't be found");
                actualMethodToCall = implementationForInterfaceMember;
                callGenericMethodFromInterfaceMethod = implementationForInterfaceMember.IsGenericMethod;
            }

            var callMethod = new Call()
            {
                ReceiverOpt = new FieldAccess { ReceiverOpt = new ThisReference(), Field = new FieldImpl { Name = "_class", Type = Type }, Type = Type },
                Method = actualMethodToCall,
                CallGenericMethodFromInterfaceMethod = callGenericMethodFromInterfaceMethod,
                SpecialCaseInterfaceWrapperCall = true
            };

            foreach (var paramExpression in interfaceMethod.Parameters.Select(p => new Parameter { ParameterSymbol = p }))
            {
                callMethod.Arguments.Add(paramExpression);
            }

            Statement mainStatement;
            if (!actualMethodToCall.ReturnsVoid)
            {
                mainStatement = new ReturnStatement { ExpressionOpt = callMethod };
            }
            else
            {
                mainStatement = new ExpressionStatement { Expression = callMethod };
            }

            return new MethodBody(actualMethodToCall) { Statements = { mainStatement } };
        }

        private MethodImpl CreateWrapperMethod(IMethodSymbol method)
        {
            var namedTypeSymbol = (INamedTypeSymbol)Type;

            return new MethodImpl(method)
            {
                ReceiverType = GetReceiver(namedTypeSymbol, this.@interface),
                IsAbstract = false,
                IsOverride = true,
                IsVirtual = false,
                ContainingNamespace = namedTypeSymbol.ContainingNamespace,
                // special case to write method name as MetadataName
                TypeArguments = ImmutableArray<ITypeSymbol>.Empty,
                OriginalDefinition = method
            };
        }

        private void Name(CCodeWriterBase c)
        {
            c.TextSpan(GetName((INamedTypeSymbol)Type, this.@interface));
        }
    }
}
