namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class ForStatement : BlockStatement
    {
        private enum Stages
        {
            Initialization,
            Body,
            Incrementing,
            End
        }

        public override Kinds Kind
        {
            get { return Kinds.ForStatement; }
        }

        public Base Initialization { get; set; }
        
        public Base Incrementing { get; set; }
        
        public Expression Condition { get; set; }

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
                        this.Condition = Deserialize(boundConditionalGoto.Condition) as Expression;
                        Debug.Assert(this.Condition != null);
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
                            this.Initialization = statement;
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
            this.Initialization.Visit(visitor);
            this.Incrementing.Visit(visitor);
            this.Condition.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("for");
            c.WhiteSpace();
            c.TextSpan("(");

            PrintStatementAsExpression(c, this.Initialization);
            
            c.TextSpan(";");
            c.WhiteSpace();

            if (this.Condition != null)
            {
                this.Condition.WriteTo(c);
            }

            c.TextSpan(";");
            c.WhiteSpace();

            PrintStatementAsExpression(c, this.Incrementing);

            c.TextSpan(")");

            c.NewLine();
            base.WriteTo(c);
        }
    }
}
