namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;

    public interface ILocalVariable
    {
        string Name { get; }

        int LocalIndex { get; }

        IType LocalType { get; }
    }
}
