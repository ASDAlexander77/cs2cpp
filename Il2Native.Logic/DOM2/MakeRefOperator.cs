namespace Il2Native.Logic.DOM2
{
    using DOM.Implementations;
    using Microsoft.CodeAnalysis.CSharp;

    public class MakeRefOperator : Expression
    {
        public Expression Operand { get; set; }

        internal void Parse(BoundMakeRefOperator boundMakeRefOperator)
        {
            base.Parse(boundMakeRefOperator);
            this.Operand = Deserialize(boundMakeRefOperator.Operand) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            // Finish it properly

            var localImpl = new LocalImpl { Name = "__MakeRef", Type = Type };
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
                            Right = new ObjectCreationExpression { Type = Type }
                        }
                });
            block.Statements.Add(new ReturnStatement { ExpressionOpt = local });
            new LambdaExpression { Statements = block, Type = Type }.WriteTo(c);
        }
    }
}
