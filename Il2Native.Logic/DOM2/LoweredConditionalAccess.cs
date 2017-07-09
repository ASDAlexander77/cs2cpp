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

        public Expression WhenNullOpt { get; set; }

        public int Id { get; set; }

        internal void Parse(BoundLoweredConditionalAccess boundLoweredConditionalAccess)
        {
            base.Parse(boundLoweredConditionalAccess);
            this.Type = boundLoweredConditionalAccess.Receiver.Type;
            this.Receiver = Deserialize(boundLoweredConditionalAccess.Receiver) as Expression;
            this.WhenNotNull = Deserialize(boundLoweredConditionalAccess.WhenNotNull) as Expression;
            this.Id = boundLoweredConditionalAccess.Id;
            if (boundLoweredConditionalAccess.WhenNullOpt != null)
            {
                this.WhenNullOpt = Deserialize(boundLoweredConditionalAccess.WhenNullOpt) as Expression;
            }
        }

        internal override void Visit(Action<Base> visitor)
        {
            this.Receiver.Visit(visitor);
            this.WhenNotNull.Visit(visitor);
            if (this.WhenNullOpt != null)
            {
                this.WhenNullOpt.Visit(visitor);
            }

            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            // Finish it properly

            var localImpl = new LocalImpl { Name = ConditionalReceiver.GetName(this.Id), Type = Type };
            var local = new Local { LocalSymbol = localImpl };

            var block = new Block();
            block.Statements.Add(
                new ExpressionStatement
                {
                    Expression =
                        new AssignmentOperator
                        {
                            ApplyAutoType = true,
                            TypeDeclaration = true,
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
                                    Left = (Type.TypeKind == TypeKind.TypeParameter) ? (Expression) new Cast
                                    {
                                        Constrained = true,
                                        Operand = local,
                                        Type = SpecialType.System_Object.ToType()
                                    } : local,
                                    Right = new Literal { Value = ConstantValue.Create(null) },
                                    OperatorKind = BinaryOperatorKind.NotEqual
                                },
                            Consequence = this.WhenNotNull,
                            Alternative = this.WhenNullOpt ?? new DefaultOperator { Type = this.WhenNotNull.Type }
                        }
                });

            new LambdaCall
            {
                Lambda = new LambdaExpression { Statements = block, Type = Type }
            }.SetOwner(MethodOwner).WriteTo(c);
        }
    }
}
