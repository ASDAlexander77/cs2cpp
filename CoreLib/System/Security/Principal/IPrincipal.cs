// Licensed under the MIT license.

namespace System.Security.Principal
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public interface IPrincipal
    {
        // Retrieve the identity object
        IIdentity Identity { get; }

        // Perform a check for a specific role
        bool IsInRole(string role);
    }
}
