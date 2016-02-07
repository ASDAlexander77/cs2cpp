namespace Il2Native.Logic.DOM2
{
    using System.Linq;
    using System.Net.Sockets;

    public class MethodBody : Block
    {
        internal override void WriteTo(CCodeWriterBase c)
        {
            // get actual statements
            var statements = this.Statements;
            if (statements.Count == 1 && statements.First() is BlockStatement)
            {
                var blockStatement = statements.First() as BlockStatement;
                if (blockStatement != null)
                {
                    var block = blockStatement.Statements as Block;
                    if (block != null)
                    {
                        statements = block.Statements;
                    }
                }
            }

            // call constructors
            var constructors = statements.TakeWhile(s => IsConstructorCall(s)).Select(s => GetCall(s)).ToArray();
            if (constructors.Length > 0)
            {
                c.WhiteSpace();
                c.TextSpan(":");
                c.WhiteSpace();

                var any = false;
                foreach (var constructorCall in constructors)
                {
                    if (any)
                    {
                        c.TextSpan(",");
                        c.WhiteSpace();
                    }

                    constructorCall.WriteTo(c);
                    any = true;
                }
            }

            c.NewLine();

            c.OpenBlock();
            foreach (var statement in statements.Skip(constructors.Length))
            {
                statement.WriteTo(c);
            }

            c.EndBlock();
        }

        private static Call GetCall(Statement s)
        {
            var expressionStatement = s as ExpressionStatement;
            if (expressionStatement != null)
            {
                return expressionStatement.Expression as Call;
            }

            return null;
        }

        private static bool IsConstructorCall(Statement s)
        {
            var expressionStatement = s as ExpressionStatement;
            if (expressionStatement != null)
            {
                var call = expressionStatement.Expression as Call;
                if (call != null)
                {
                    return call.IsCallingConstructor && call.ReceiverOpt is ThisReference;
                }
            }

            return false;
        }
    }
}
