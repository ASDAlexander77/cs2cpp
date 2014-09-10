using System.Runtime.CompilerServices;

namespace System.DiagnosticsX
{
    public static class Debugger
    {
        public static extern bool IsAttached
        {
            
            get;
        }

        
        public static extern void Break();
    }
}


