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
            base.Visit(visitor);
            this.Receiver.Visit(visitor);
            this.WhenNotNull.Visit(visitor);
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
            block.Statements.Add(
                new ReturnStatement
                {
                    ExpressionOpt =
                        new ConditionalOperator
                        {
                            Condition =
                                new BinaryOperator
                                {
                                    Left = local,
                                    Right = new Literal { Value = ConstantValue.Create(null) },
                                    OperatorKind = BinaryOperatorKind.NotEqual
                                },
                            Consequence = new Access { ReceiverOpt = local, Expression = this.WhenNotNull },
                            Alternative = new Literal { Value = ConstantValue.Create(null) }

                        }
                });
            new LambdaCall
            {
                Lambda = new LambdaExpression { Statements = block, Type = Type }
            }.WriteTo(c);
        }
    }
}
