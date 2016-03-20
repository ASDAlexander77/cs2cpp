// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class ForStatement : BlockStatement
    {
        public Expression ConditionOpt { get; set; }

        public Base IncrementingOpt { get; set; }

        public Base InitializationOpt { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.ForStatement; }
        }

        internal bool Parse(BoundStatementList boundStatementList)
        {
            if (boundStatementList == null)
            {
                throw new ArgumentNullException();
            }

            var stage = Stages.Initialization;
            foreach (var boundStatement in IterateBoundStatementsList(boundStatementList))
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
                        this.ConditionOpt = Deserialize(boundConditionalGoto.Condition) as Expression;
                        Debug.Assert(this.ConditionOpt != null);
                        continue;
                    }

                    var boundGoto = boundStatement as BoundGotoStatement;
                    if (boundGoto != null)
                    {
                        // empty condition
                        continue;
                    }
                }

                var statement = Deserialize(boundStatement);
                if (statement != null)
                {
                    switch (stage)
                    {
                        case Stages.Initialization:
                            this.InitializationOpt = statement;
                            break;
                        case Stages.Body:
                            Statements = statement;
                            break;
                        case Stages.Incrementing:
                            this.IncrementingOpt = statement;
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
            if (this.InitializationOpt != null)
            {
                this.InitializationOpt.Visit(visitor);
            }

            if (this.IncrementingOpt != null)
            {
                this.IncrementingOpt.Visit(visitor);
            }

            if (this.ConditionOpt != null)
            {
                this.ConditionOpt.Visit(visitor);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("for");
            c.WhiteSpace();
            c.TextSpan("(");

            PrintStatementAsExpression(c, this.InitializationOpt);
            
            c.TextSpan(";");
            c.WhiteSpace();

            if (this.ConditionOpt != null)
            {
                this.ConditionOpt.WriteTo(c);
            }

            c.TextSpan(";");
            c.WhiteSpace();

            PrintStatementAsExpression(c, this.IncrementingOpt);

            c.TextSpan(")");

            c.NewLine();
            base.WriteTo(c);
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
