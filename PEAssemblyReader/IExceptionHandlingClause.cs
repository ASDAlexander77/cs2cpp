namespace PEAssemblyReader
{
    using System.Reflection;

    public interface IExceptionHandlingClause
    {
        IType CatchType { get; }

        ExceptionHandlingClauseOptions Flags { get; }

        int TryOffset { get; }

        int TryLength { get; }

        int HandlerOffset { get; }

        int HandlerLength { get; }
    }
}
