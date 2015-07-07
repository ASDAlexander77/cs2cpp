// Licensed under the MIT license.

/*============================================================
**
** 
** 
**
**
** Purpose: Exception for paths and/or filenames that are 
** too long.
**
**
===========================================================*/

using System;

namespace System.IO {

    [Serializable]
[System.Runtime.InteropServices.ComVisible(true)]
    public class PathTooLongException : IOException
    {
        public PathTooLongException() 
            : base(Environment.GetResourceString("IO.PathTooLong")) {
        }
        
        public PathTooLongException(String message) 
            : base(message) {
        }
        
        public PathTooLongException(String message, Exception innerException) 
            : base(message, innerException) {
        }
    }
}
