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
        private static bool AnyPendingFinalizers()
        {
            throw new NotImplementedException();
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

        public static extern void ReRegisterForFinalize(Object obj);
        
        public static int MaxGeneration { get; set; }

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


