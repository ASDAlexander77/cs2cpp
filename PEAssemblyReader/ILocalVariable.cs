namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;

    public interface ILocalVariable
    {
        string Name { get; }

        IType LocalType { get; }
    }
}
