// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;
    using DOM;
    using DOM.Implementations;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class DelegateCreationExpression : ObjectCreationExpression
    {
        private bool clone;

        private Expression cloneArgument;

        public override Kinds Kind
        {
            get { return Kinds.DelegateCreationExpression; }
        }

        internal void Parse(BoundDelegateCreationExpression boundDelegateCreationExpression)
        {
            base.Parse(boundDelegateCreationExpression);
            var argument = Deserialize(boundDelegateCreationExpression.Argument) as Expression;
            Debug.Assert(argument != null);

            if (boundDelegateCreationExpression.MethodOpt != null && !(boundDelegateCreationExpression.Argument is BoundMethodGroup))
            {
                var methodGroup = new MethodGroup
                {
                    ReceiverOpt = argument,
                    Method = boundDelegateCreationExpression.MethodOpt
                };

                Arguments.Add(methodGroup);
            }
            else
            {
                if (argument.Type != null && argument.Type.TypeKind == TypeKind.Delegate)
                {
                    this.clone = true;
                    this.cloneArgument = argument;
                }
                else
                {
                    var methodGroup = argument as MethodGroup;
                    if (methodGroup != null && boundDelegateCreationExpression.MethodOpt != null)
                    {
                        methodGroup.Method = boundDelegateCreationExpression.MethodOpt;
                    }

                    Arguments.Add(argument);
                }
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.clone)
            {
                new Cast
                    {
                        Operand =
                            new Call
                                {
                                    ReceiverOpt = this.cloneArgument,
                                    Method = new MethodImpl { Name = "__clone", Parameters = ImmutableArray<IParameterSymbol>.Empty }
                                },
                        Type = Type,
                        CCast = true,
                    }.WriteTo(c);
            }
            else
            {
                var methodGroup = Arguments.First() as MethodGroup;
                var isStatic = methodGroup != null && methodGroup.Method.IsStatic;
                var newDelegateMethod = new CCodeDelegateWrapperClass((INamedTypeSymbol)Type).GetNewMethod(isStatic, true);
                if (newDelegateMethod.ContainingNamespace != null)
                {
                    c.WriteNamespace(newDelegateMethod.ContainingNamespace);
                    c.TextSpan("::");
                }

                c.WriteMethodName(newDelegateMethod, addTemplate: true);
                WriteCallArguments(Arguments, Method != null ? Method.Parameters : (IEnumerable<IParameterSymbol>)null, c);
            }
        }
    }
}
