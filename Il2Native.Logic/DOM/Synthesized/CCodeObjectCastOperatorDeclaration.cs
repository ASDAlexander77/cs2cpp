// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeObjectCastOperatorDeclaration : CCodeMethodDeclaration
    {
        public CCodeObjectCastOperatorDeclaration(INamedTypeSymbol type)
            : base(type, new ObjectCastOperatorMethod(type))
        {
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.WriteMethodPrefixes(Method, true);
            c.TextSpan("operator object*");
            c.WriteMethodParameters(Method, true, MethodBodyOpt != null, containingNamespace: Method.ContainingNamespace);
            c.WriteMethodSuffixes(Method, true);
            if (MethodBodyOpt == null)
            {
                c.EndStatement();
            }
            else
            {
                MethodBodyOpt.WriteTo(c);
            }
        }

        public class ObjectCastOperatorMethod : MethodImpl
        {
            public ObjectCastOperatorMethod(INamedTypeSymbol type)
            {
                Name = "operator object*";
                MethodKind = MethodKind.BuiltinOperator;
                ReceiverType = type;
                ContainingType = type;
                ContainingNamespace = type.ContainingNamespace;
                IsVirtual = true;
                IsAbstract = true;
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
            }
        }
    }
}
