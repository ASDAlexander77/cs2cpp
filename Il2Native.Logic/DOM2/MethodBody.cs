namespace Il2Native.Logic.DOM2
{
    using System.Linq;

    public class MethodBody : Block
    {
        internal override void WriteTo(CCodeWriterBase c)
        {
            // call constructors
            var constructors =
                this.Statements.TakeWhile(
                    s =>
                    s is ExpressionStatement && ((ExpressionStatement)s).Expression is Call
                    && ((Call)(((ExpressionStatement)s).Expression)).IsCallingBaseConstructor)
                    .Select(s => ((ExpressionStatement)s).Expression as Call)
                    .ToArray();

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
            foreach (var statement in this.Statements.Skip(constructors.Length))
            {
                statement.WriteTo(c);
            }

            c.EndBlock();
        }
    }
}
