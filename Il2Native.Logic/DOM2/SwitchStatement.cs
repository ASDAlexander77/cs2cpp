namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;

    using Il2Native.Logic.DOM.Implementations;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class SwitchStatement : Statement
    {
        private Expression boundExpression;
        private IList<SwitchSection> switchCases = new List<SwitchSection>();

        private MethodSymbol stringEquality;

        internal void Parse(BoundSwitchStatement boundSwitchStatement)
        {
            if (boundSwitchStatement == null)
            {
                throw new ArgumentNullException();
            }

            this.boundExpression = Deserialize(boundSwitchStatement.BoundExpression) as Expression;
            foreach (var boundSwitchSection in boundSwitchStatement.SwitchSections)
            {
                var switchSection = new SwitchSection();
                switchSection.Parse(boundSwitchSection);
                switchCases.Add(switchSection);
            }

            this.stringEquality = boundSwitchStatement.StringEquality;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            Local localCase = null;
            if (stringEquality != null)
            {
                c.OpenBlock();

                var localImpl = new LocalImpl { Name = "__SwitchExpression", Type = boundExpression.Type };
                var local = new Local { LocalSymbol = localImpl };

                var localImplCase = new LocalImpl { Name = "__SwitchCase", Type = new TypeImpl { SpecialType = SpecialType.System_Int32 } };
                localCase = new Local { LocalSymbol = localImplCase };

                new VariableDeclaration { Local = localCase }.WriteTo(c);
                new VariableDeclaration { Local = local }.WriteTo(c);

                // first if
                IfStatement first = null;
                IfStatement last = null;
                var caseIndex = 0;

                foreach (var switchSection in this.switchCases)
                {
                    foreach (var label in switchSection.Labels)
                    {
                        if (label.Value == null)
                        {
                            // default case;
                            continue;
                        }

                        caseIndex++;

                        // compare
                        var callEqual = new Call() { Method = this.stringEquality };
                        callEqual.Arguments.Add(local);
                        callEqual.Arguments.Add(new Literal { Value = label.Value });

                        // set value
                        var setExpr = new ExpressionStatement
                                          {
                                              Expression =
                                                  new AssignmentOperator
                                                      {
                                                          Left = localCase,
                                                          Right = new Literal { Value = ConstantValue.Create(caseIndex) }
                                                      }
                                          };

                        var ifStatement = new IfStatement
                                              {
                                                  Condition =
                                                      new BinaryOperator
                                                          {
                                                              OperatorKind = BinaryOperatorKind.Equal,
                                                              Left = new Literal { Value = ConstantValue.Create(0) },
                                                              Right = callEqual
                                                          },
                                                  IfStatements = setExpr
                                              };

                        first = first ?? ifStatement;
                        if (last != null)
                        {
                            last.ElseStatements = ifStatement;
                        }

                        last = ifStatement;

                        // remap case value
                        label.Value = ConstantValue.Create(caseIndex);
                    }
                }

                if (first != null)
                {
                    first.WriteTo(c);
                }

                c.Separate();
            }

            c.TextSpan("switch");
            c.WhiteSpace();
            c.TextSpan("(");
            if (stringEquality != null)
            {
                localCase.WriteTo(c);
            }
            else
            {
                this.boundExpression.WriteTo(c);
            }

            c.TextSpan(")");
            c.NewLine();
            c.OpenBlock();
            foreach (var switchSection in this.switchCases)
            {
                switchSection.WriteTo(c);
            }

            c.EndBlock();

            if (stringEquality != null)
            {
                c.EndBlock();
            }

            c.Separate();
        }
    }
}
