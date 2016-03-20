// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp;

    public class ForEachSimpleArrayStatement : BlockStatement
    {
        private Base _initialization;

        private readonly IList<Statement> locals = new List<Statement>();

        public Expression Condition { get; set; }

        public Base Incrementing { get; set; }

        public Base Initialization
        {
            get { return this._initialization; }
            set { this._initialization = value; }
        }

        public override Kinds Kind
        {
            get { return Kinds.ForEachSimpleArrayStatement; }
        }

        internal bool Parse(BoundStatementList boundStatementList)
        {
            if (boundStatementList == null)
            {
                throw new ArgumentNullException();
            }

            var stage = Stages.Initialization;
            var statementList = Unwrap(boundStatementList);

            // in case of multi array if current statmentList contains BoundBlock you should process all statements into Initial stage
            if (statementList.Statements.Last() is BoundBlock &&
                !(statementList.Statements.Take(statementList.Statements.Length - 1).All(s => s is BoundBlock)))
            {
                return false;
            }

            var mainBlock = boundStatementList as BoundBlock;
            if (mainBlock != null)
            {
                ParseLocals(mainBlock.Locals, this.locals);
            }

            var innerBlock = statementList as BoundBlock;
            if (innerBlock != null && (object)mainBlock != (object)innerBlock)
            {
                ParseLocals(innerBlock.Locals, this.locals);
            }

            foreach (var boundStatement in IterateBoundStatementsList(statementList))
            {
                var boundLabelStatement = boundStatement as BoundLabelStatement;
                if (boundLabelStatement != null)
                {
                    if (boundLabelStatement.Label.Name.StartsWith("<start") && stage == Stages.Initialization)
                    {
                        stage = Stages.Body;
                        continue;
                    }

                    if (boundLabelStatement.Label.Name.StartsWith("<continue") && stage == Stages.Body)
                    {
                        stage = Stages.Incrementing;
                        continue;
                    }

                    if (boundLabelStatement.Label.Name.StartsWith("<end") && stage == Stages.Incrementing)
                    {
                        stage = Stages.End;
                        continue;
                    }

                    if (boundLabelStatement.Label.Name.StartsWith("<break") && stage == Stages.End)
                    {
                        continue;
                    }
                }

                if (stage == Stages.Initialization)
                {
                    var boundGotoStatement = boundStatement as BoundGotoStatement;
                    if (boundGotoStatement != null)
                    {
                        continue;
                    }
                }

                if (stage == Stages.End)
                {
                    var boundConditionalGoto = boundStatement as BoundConditionalGoto;
                    if (boundConditionalGoto != null)
                    {
                        this.Condition = Deserialize(boundConditionalGoto.Condition) as Expression;
                        Debug.Assert(this.Condition != null);
                        continue;
                    }
                }

                var statement = Deserialize(boundStatement, specialCase: stage != Stages.Body ? SpecialCases.ForEachBody : SpecialCases.None);
                if (statement != null)
                {
                    switch (stage)
                    {
                        case Stages.Initialization:
                            MergeOrSet(ref this._initialization, statement);
                            break;
                        case Stages.Body:
                            Statements = statement;
                            break;
                        case Stages.Incrementing:
                            this.Incrementing = statement;
                            break;
                        default:
                            return false;
                    }
                }
            }

            return stage == Stages.End;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            foreach (var statement in this.locals)
            {
                statement.Visit(visitor);
            }

            this.Initialization.Visit(visitor);
            this.Incrementing.Visit(visitor);
            this.Condition.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.locals.Count > 0)
            {
                c.OpenBlock();

                foreach (var statement in this.locals)

                {
                    statement.WriteTo(c);
                }
            }

            c.TextSpan("for");
            c.WhiteSpace();
            c.TextSpan("(");
            
            var block = this.Initialization as Block;
            if (block != null)
            {
                var any = false;
                foreach (var initializationItem in block.Statements)
                {
                    if (any)
                    {
                        c.TextSpan(",");
                        c.WhiteSpace();
                    }

                    PrintStatementAsExpression(c, initializationItem);
                    any = true;
                }
            }
            else
            {
                PrintStatementAsExpression(c, this.Initialization);
            }

            c.TextSpan(";");
            c.WhiteSpace();

            this.Condition.WriteTo(c);

            c.TextSpan(";");
            c.WhiteSpace();

            PrintStatementAsExpression(c, this.Incrementing);

            c.TextSpan(")");

            c.NewLine();
            base.WriteTo(c);

            if (this.locals.Count > 0)
            {
                c.EndBlock();
            }
        }

        private enum Stages
        {
            Initialization,
            Body,
            Incrementing,
            End
        }
    }
}
