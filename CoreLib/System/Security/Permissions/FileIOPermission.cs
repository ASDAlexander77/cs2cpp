// Licensed under the MIT license.

namespace System.Security.Permissions
{
    using System;

    [Serializable]
    [Flags]
    [System.Runtime.InteropServices.ComVisible(true)]
    public enum FileIOPermissionAccess
    {
        NoAccess = 0x00,
        Read = 0x01,
        Write = 0x02,
        Append = 0x04,
        PathDiscovery = 0x08,
        AllAccess = 0x0F,
    }
}
