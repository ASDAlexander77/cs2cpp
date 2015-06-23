// Licensed under the MIT license.

namespace System.Security.Principal
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public interface IIdentity
    {
        // Access to the name string
        string Name { get; }

        // Access to Authentication 'type' info
        string AuthenticationType { get; }

        // Determine if this represents the unauthenticated identity
        bool IsAuthenticated { get; }
    }
}
