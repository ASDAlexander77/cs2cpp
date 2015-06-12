// Licensed under the MIT license.

namespace System.Reflection
{
    using System;

    [Serializable]
    [Flags]
    [System.Runtime.InteropServices.ComVisible(true)]
    public enum CallingConventions
    {
        Standard = 0x0001,
        VarArgs = 0x0002,
        Any = Standard | VarArgs,
        HasThis = 0x0020,
        ExplicitThis = 0x0040,
    }
}
