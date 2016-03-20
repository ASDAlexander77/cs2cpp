// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp;

    public class ForEachIteratorStatement : Statement
    {
        private Base _initialization;

        private readonly IList<Statement> locals = new List<Statement>();

        public Base Initialization
        {
            get { return this._initialization; }
            set { this._initialization = value; }
        }

        public override Kinds Kind
        {
            get { return Kinds.ForEachIteratorStatement; }
        }

        public IList<Statement> Locals
        {
            get { return this.locals; }
        }

        public TryStatement TryStatement { get; set; }

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
                ParseLocals(mainBlock.Locals, this.Locals);
            }

            var innerBlock = statementList as BoundBlock;
            if (innerBlock != null && (object)mainBlock != (object)innerBlock)
            {
                ParseLocals(innerBlock.Locals, this.Locals);
            }

            foreach (var boundStatement in IterateBoundStatementsList(statementList))
            {
                BoundTryStatement boundTryStatement = null;
                if (stage == Stages.Initialization)
                {
                    boundTryStatement = boundStatement as BoundTryStatement;
                    if (boundTryStatement != null)
                    {
                        stage = Stages.TryBody;
                    }
                    else
                    {
                        var boundGotoStatement = boundStatement as BoundGotoStatement;
                        if (boundGotoStatement != null)
                        {
                            continue;
                        }
                    }
                }

                switch (stage)
                {
                    case Stages.Initialization:
                        var statement = Deserialize(boundStatement, specialCase: SpecialCases.ForEachBody);
                        MergeOrSet(ref this._initialization, statement);
                        break;
                    case Stages.TryBody:
                        this.TryStatement = new TryStatement();

                        // apply special parsing to block of try
                        var whileStatement = new WhileStatement();
                        whileStatement.Parse(boundTryStatement.TryBlock.Statements.OfType<BoundStatementList>().First());

                        this.TryStatement.TryBlock = whileStatement;
                        if (boundTryStatement.FinallyBlockOpt != null)
                        {
                            this.TryStatement.FinallyBlockOpt = Deserialize(
                                boundTryStatement.FinallyBlockOpt,
                                specialCase: SpecialCases.ForEachBody);
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return stage == Stages.TryBody;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            foreach (var statement in this.Locals)
            {
                statement.Visit(visitor);
            }

            this.Initialization.Visit(visitor);
            this.TryStatement.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.Locals.Count > 0)
            {
                c.OpenBlock();

                foreach (var statement in this.Locals)
                {
                    statement.WriteTo(c);
                }
            }

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
                this.Initialization.WriteTo(c);
            }

            this.TryStatement.WriteTo(c);

            if (this.Locals.Count > 0)
            {
                c.EndBlock();

                // No normal ending of Statement as we do not need extra ;
                c.Separate();
            }
        }

        private enum Stages
        {
            Initialization,
            TryBody
        }
    }
}
