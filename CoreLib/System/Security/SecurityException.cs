// Licensed under the MIT license.

/*=============================================================================
**
** 
** 
**
**
** Purpose: Exception class for security
**
**
=============================================================================*/

namespace System.Security
{
    using System;

    [System.Runtime.InteropServices.ComVisible(true)]
    [Serializable] public class SecurityException : SystemException
    {
        internal static string GetResString(string sResourceName)
        {
            return Environment.GetResourceString(sResourceName);
        }

        public SecurityException() 
            : base(GetResString("Arg_SecurityException"))
        {
        }
    
        public SecurityException(String message) 
            : base(message)
        {
            // This is the constructor that gets called if you Assert but don't have permission to Assert.  (So don't assert in here.)
        }

        public SecurityException(String message, Exception inner) 
            : base(message, inner)
        {
        }
    }
}
