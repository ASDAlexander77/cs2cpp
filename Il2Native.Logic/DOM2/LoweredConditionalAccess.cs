// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using DOM.Implementations;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class LoweredConditionalAccess : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.LoweredConditionalAccess; }
        }

        public Expression Receiver { get; set; }

        public Expression WhenNotNull { get; set; }

        internal void Parse(BoundLoweredConditionalAccess boundLoweredConditionalAccess)
        {
            base.Parse(boundLoweredConditionalAccess);
            this.Type = boundLoweredConditionalAccess.Receiver.Type;
            this.Receiver = Deserialize(boundLoweredConditionalAccess.Receiver) as Expression;
            this.WhenNotNull = Deserialize(boundLoweredConditionalAccess.WhenNotNull) as Expression;
        }

        internal override void Visit(Action<Base> visitor)
        {
            this.Receiver.Visit(visitor);
            this.WhenNotNull.Visit(visitor);
            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            // Finish it properly

            var localImpl = new LocalImpl { Name = "__ConditionalAccess", Type = Type };
            var local = new Local { LocalSymbol = localImpl };

            var block = new Block();
            block.Statements.Add(new VariableDeclaration { Local = local });
            block.Statements.Add(
                new ExpressionStatement
                {
                    Expression =
                        new AssignmentOperator
                        {
                            Left = local,
                            Right = this.Receiver
                        }
                });

            // prepare access expression;
            Expression access = new Access { ReceiverOpt = local, Expression = this.WhenNotNull };
            var call = this.WhenNotNull as Call;
            if (call != null)
            {
                access = call;
                call.ReceiverOpt = local;
            }

            var fieldAccess = this.WhenNotNull as FieldAccess;
            if (fieldAccess != null)
            {
                access = fieldAccess;
                fieldAccess.ReceiverOpt = local;
            }

            block.Statements.Add(
                new ReturnStatement
                {
                    ExpressionOpt =
                        new ConditionalOperator
                        {
                            Condition =
                                new BinaryOperator
                                {
                                    Left = (Type.TypeKind == TypeKind.TypeParameter) ? (Expression) new Cast
                                    {
                                        Constrained = true,
                                        Operand = local,
                                        Type = new TypeImpl { SpecialType = SpecialType.System_Object }
                                    } : local,
                                    Right = new Literal { Value = ConstantValue.Create(null) },
                                    OperatorKind = BinaryOperatorKind.NotEqual
                                },
                            Consequence = access,
                            Alternative = new Literal { Value = ConstantValue.Create(null) }

                        }
                });

            new LambdaCall
            {
                Lambda = new LambdaExpression { Statements = block, Type = Type }
            }.WriteTo(c);

            // clean up
            if (call != null)
            {
                call.ReceiverOpt = null;
            }

            if (fieldAccess != null)
            {
                fieldAccess.ReceiverOpt = null;
            }
        }
    }
}
