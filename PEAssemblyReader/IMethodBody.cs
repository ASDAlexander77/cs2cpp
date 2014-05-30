namespace PEAssemblyReader
{
    using System.Collections.Generic;

    public interface IMethodBody
    {
        IEnumerable<ILocalVariable> LocalVariables { get; }

        IEnumerable<IExceptionHandlingClause> ExceptionHandlingClauses { get; }

        byte[] GetILAsByteArray();
    }
}
