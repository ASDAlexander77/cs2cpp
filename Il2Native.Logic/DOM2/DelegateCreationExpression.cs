namespace Il2Native.Logic.DOM2
{
    using System.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class DelegateCreationExpression : ObjectCreationExpression
    {
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
                var methodGroup = new MethodGroup { ReceiverOpt = argument };
                methodGroup.Methods.Add(boundDelegateCreationExpression.MethodOpt);
                Arguments.Add(methodGroup);
            }
            else
            {
                if (argument.Type != null && argument.Type.TypeKind == TypeKind.Delegate)
                {
                    NewOperator = true;
                    Arguments.Add(new PointerIndirectionOperator { Operand = argument });
                }
                else
                {
                    Arguments.Add(argument);
                }
            }
        }
    }
}
