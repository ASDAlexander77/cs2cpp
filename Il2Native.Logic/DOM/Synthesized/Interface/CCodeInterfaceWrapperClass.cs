// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
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

    public class CCodeInterfaceWrapperClass : CCodeClass
    {
        private readonly INamedTypeSymbol @interface;

        public CCodeInterfaceWrapperClass(INamedTypeSymbol type, INamedTypeSymbol @interface)
            : base(type.IsValueType ? new ValueTypeAsClassTypeImpl(type) : type)
        {
            this.@interface = @interface;
            this.CreateMemebers();
        }

        public static string GetName(INamedTypeSymbol type, INamedTypeSymbol @interface)
        {
            var sb = new StringBuilder();
            sb.Append(type.MetadataName.CleanUpName());
            sb.Append("_");
            GetGenericNameRecursive(sb, @interface);
            return sb.ToString();
        }

        public IEnumerable<CCodeMethodDefinition> GetMembersImplementation()
        {
            return this.@interface.GetMembers()
                .OfType<IMethodSymbol>()
                .Union(this.@interface.EnumerateInterfaceMethods())
                .Select(this.CreateWrapperMethod)
                .Select(m => new CCodeMethodDefinitionWrapper(m) { MethodBodyOpt = this.CreateMethodBody(m) });
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

            // add new method
            var namedTypeSymbol = (INamedTypeSymbol)Type;
            // not needed as we use global normal allocator
            ////new CCodeNewOperatorDeclaration(@interface).WriteTo(c);
            new CCodeObjectCastOperatorDefinition(namedTypeSymbol).WriteTo(c);

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
            Declarations.Add(new CCodeFieldDeclaration(new FieldImpl { Name = "_class", Type = Type }));
            foreach (var method in this.@interface.GetMembers().OfType<IMethodSymbol>().Union(this.@interface.EnumerateInterfaceMethods()))
            {
                Declarations.Add(new CCodeMethodDeclaration(Type, this.CreateWrapperMethod(method)));
            }
        }

        private MethodBody CreateMethodBody(IMethodSymbol method)
        {
            var callMethod = new Call()
            {
                ReceiverOpt = new FieldAccess { ReceiverOpt = new ThisReference(), Field = new FieldImpl { Name = "_class", Type = Type }, Type = Type },
                Method = method,
                InterfaceWrapperSpecialCall = Type.TypeKind != TypeKind.Interface
            };

            foreach (var paramExpression in method.Parameters.Select(p => new Parameter { ParameterSymbol = p }))
            {
                callMethod.Arguments.Add(paramExpression);
            }

            Statement mainStatement;
            if (!method.ReturnsVoid)
            {
                mainStatement = new ReturnStatement { ExpressionOpt = callMethod };
            }
            else
            {
                mainStatement = new ExpressionStatement { Expression = callMethod };
            }

            return new MethodBody(method) { Statements = { mainStatement } };
        }

        private MethodImpl CreateWrapperMethod(IMethodSymbol method)
        {
            var namedTypeSymbol = (INamedTypeSymbol)Type;

            return new MethodImpl(method)
            {
                ReceiverType = new NamedTypeImpl { Name = GetName(namedTypeSymbol, this.@interface), ContainingType = (INamedTypeSymbol)Type },
                IsAbstract = false,
                ContainingType = namedTypeSymbol,
                ContainingNamespace = Type.ContainingNamespace,
                // special case to write method name as MetadataName
                TypeArguments = ImmutableArray<ITypeSymbol>.Empty
            };
        }

        private void Name(CCodeWriterBase c)
        {
            c.TextSpan(GetName((INamedTypeSymbol)Type, this.@interface));
        }

        public class CCodeMethodDefinitionWrapper : CCodeMethodDefinition
        {
            public CCodeMethodDefinitionWrapper(IMethodSymbol method)
                : base(method)
            {
            }

            public override bool IsGeneric
            {
                get { return Method.IsGenericMethod && !Method.IsVirtualGenericMethod() && !Method.IsInterfaceGenericMethodSpecialCase(); }
            }
        }
    }
}
