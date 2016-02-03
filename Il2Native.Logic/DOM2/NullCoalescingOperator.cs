namespace Il2Native.Logic.DOM2
{
    using DOM.Implementations;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class NullCoalescingOperator : Expression
    {
        public Expression LeftOperand { get; set; }
        public Expression RightOperand { get; set; }
        internal ConversionKind Kind { get; set; }

        internal void Parse(BoundNullCoalescingOperator boundNullCoalescingOperator)
        {
            base.Parse(boundNullCoalescingOperator);
            this.LeftOperand = Deserialize(boundNullCoalescingOperator.LeftOperand) as Expression;
            this.RightOperand = Deserialize(boundNullCoalescingOperator.RightOperand) as Expression;
            this.Kind = boundNullCoalescingOperator.LeftConversion.Kind;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            // Finish it properly

            var localImpl = new LocalImpl { Name = "__NullCoalescing", Type = Type };
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
                            Right = this.LeftOperand
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
                                }, Consequence = local, Alternative = this.RightOperand

                        }
                });
            new LambdaExpression { Block = block, Type = Type }.WriteTo(c);
        }
    }
}
