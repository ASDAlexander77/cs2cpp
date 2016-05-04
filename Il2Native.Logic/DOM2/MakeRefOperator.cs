// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System.Linq;

    using DOM.Implementations;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class MakeRefOperator : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.MakeRefOperator; }
        }

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

            var intPtrType = new NamedTypeImpl { SpecialType = SpecialType.System_IntPtr, IsValueType = true };
            var valueField = new FieldImpl { Name = "Value", Type = intPtrType };
            block.Statements.Add(
                new ExpressionStatement
                    {
                        Expression =
                            new AssignmentOperator
                                {
                                    Left = new FieldAccess { ReceiverOpt = local, Field = valueField },
                                    Right =
                                        new ObjectCreationExpression
                                            {
                                                Type = valueField.Type,
                                                Arguments =
                                                    {
                                                        this.Operand.IsReference
                                                            ? this.Operand
                                                            : new AddressOfOperator { Operand = this.Operand }
                                                    }
                                            }
                                }
                    });

            var typeField = new FieldImpl { Name = "Type", Type = intPtrType };
            block.Statements.Add(
                new ExpressionStatement
                    {
                        Expression =
                            new AssignmentOperator
                                {
                                    Left = new FieldAccess { ReceiverOpt = local, Field = typeField },
                                    Right =
                                        new ObjectCreationExpression
                                            {
                                                Type = typeField.Type,
                                                Arguments =
                                                    {
                                                        new AddressOfOperator
                                                            {
                                                                Operand =
                                                                    new FieldAccess
                                                                        {
                                                                            Field = new FieldImpl
                                                                                        {
                                                                                            Name = "_methods_table", 
                                                                                            ContainingSymbol = this.Operand.Type, 
                                                                                            IsStatic = true
                                                                                        }
                                                                        }
                                                            }
                                                    }
                                            }
                                }
                    });

            block.Statements.Add(new ReturnStatement { ExpressionOpt = local });
            new LambdaExpression { Statements = block, Type = Type }.WriteTo(c);
        }
    }
}
