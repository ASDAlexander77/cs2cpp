namespace PEAssemblyReader
{
    using System.Collections.Generic;

    public interface IMethodBody
    {
        IEnumerable<ILocalVariable> LocalVariables { get; }

        byte[] IL { get; }
    }
}
