// Licensed under the MIT license.

/*============================================================
**
**
**
** Purpose: Debugging Macros for use in the Base Class Libraries
**
**
============================================================*/

namespace System {


    [Serializable]
    internal enum LogLevel {
        Trace  = 0,
        Status = 20,
        Warning= 40,
        Error  = 50,
        Panic  = 100,
    }
    
    // Only statics, does not need to be marked with the serializable attribute
    internal static class BCLDebug {
        public static void Log(string format)
        {
        }

        public static void Correctness(bool isClosed, string s)
        {
        }
    }
}

