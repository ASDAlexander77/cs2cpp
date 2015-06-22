////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System.Threading
{
    using System.Threading;
    using System.Runtime.InteropServices;
    using System;
    using System.Runtime.CompilerServices;

    // deliberately not [serializable]
    public sealed class Thread
    {
        private Delegate m_Delegate;
        private int m_Priority;
        [System.Reflection.FieldNoReflection]
        private object m_Thread;
        [System.Reflection.FieldNoReflection]
        private object m_AppDomain;
        private int    m_Id;

        static Thread()
        {
            CurrentThread = new Thread();
        }

        private Thread()
        {
        }

        public Thread(ThreadStart start)
        {
            throw new NotImplementedException();
        }
        
        extern public void Start();
        
        extern public void Abort();
        
        extern public void Suspend();
        
        extern public void Resume();

        extern public ThreadPriority Priority
        {            
            get;           
            set;
        }

        public int ManagedThreadId { get; private set; }

        extern public bool IsAlive
        {
            
            get;
        }
        
        extern public void Join();
        
        extern public bool Join(int millisecondsTimeout);
        
        extern public bool Join(TimeSpan timeout);
        
        public static void Sleep(int millisecondsTimeout)
        {
            throw new NotImplementedException();
        }

        public static Thread CurrentThread { get; private set; }

        extern public ThreadState ThreadState
        {
            
            get;
        }
        
        public static AppDomain GetDomain()
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void MemoryBarrier();
    }

    ////// declaring a local var of this enum type and passing it by ref into a function that needs to do a
    ////// stack crawl will both prevent inlining of the calle and pass an ESP point to stack crawl to

    // declaring a local var of this enum type and passing it by ref into a function that needs to do a
    // stack crawl will both prevent inlining of the calle and pass an ESP point to stack crawl to
    // Declaring these in EH clauses is illegal; they must declared in the main method body
    [Serializable]
    internal enum StackCrawlMark
    {
        LookForMe = 0,
        LookForMyCaller = 1,
        LookForMyCallersCaller = 2,
        LookForThread = 3
    }
}


