////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System
{
    //This class only static members and doesn't require the serializable keyword.

    using System;
    using System.Runtime.CompilerServices;

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
            ////throw new NotImplementedException();
        }

        public static extern void ReRegisterForFinalize(Object obj);
    }
}


