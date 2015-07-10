////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System
{
    //This class only static members and doesn't require the serializable keyword.

    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class GC
    {
        public static int MaxGeneration { get; set; }

        private static bool AnyPendingFinalizers()
        {
            return false;
        }

        public static void WaitForPendingFinalizers()
        {
            while (AnyPendingFinalizers()) System.Threading.Thread.Sleep(10);
        }
        
        public static void SuppressFinalize(Object obj)
        {
        }

        public static void KeepAlive(object internalWaitHandles)
        {
        }

        public static void ReRegisterForFinalize(Object obj)
        {            
        }
        
        public static int GetGeneration(object p0)
        {
            return 0;
        }

        public static int CollectionCount(int p0)
        {
            return 0;
        }
    }
}


