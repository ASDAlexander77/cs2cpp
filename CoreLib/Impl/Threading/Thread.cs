namespace System.Threading
{
    using System.Runtime.CompilerServices;
    using System.Security.Principal;

    /// <summary>
    /// Partial implementation of Thread
    /// </summary>
    public partial class Thread
    {
        private static AppDomain currentDomain;

        /// <summary>
        /// </summary>
        private static Thread currentThread;

        /// <summary>
        /// </summary>
        private static int threadId;

        /// <summary>
        /// </summary>
        private ThreadState state;

        /// <summary>
        /// </summary>
        private object abortReason;

        /// <summary>
        /// </summary>
        private Delegate start;

        /// <summary>
        /// </summary>
        private int maxStackSize;

        /// <summary>
        /// </summary>
        private int pthread;

        /// <summary>
        /// </summary>
        private PthreadAttr pthreadAttr;

        /// <summary>
        /// </summary>
        public int ManagedThreadId
        {
            get
            {
                return this.m_ManagedThreadId;
            }
        }

        /*=========================================================================
        ** Returns true if the thread has been started and is not dead.
        =========================================================================*/
        public bool IsAlive
        {
            get
            {
                // if send SIG 0, no actions
                return GC_PTHREAD_KILL(this.pthread, 0) != 0;
            }
        }

        /*=========================================================================
        ** Returns true if the thread is a threadpool thread.
        =========================================================================*/
        public bool IsThreadPoolThread
        {
            get
            {
                return false;
            }
        }

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private static extern int pthread_attr_init(ref PthreadAttr attr);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private static extern int pthread_attr_destroy(ref PthreadAttr attr);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private static extern int pthread_attr_setstacksize(ref PthreadAttr attr, int stacksize);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private static extern unsafe int GC_PTHREAD_CREATE(ref int pthread, ref PthreadAttr attr, void* startRoutine, object arg);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private static extern int GC_PTHREAD_CANCEL(int pthread);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private static extern int GC_PTHREAD_KILL(int pthread, int signal);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private static extern int GC_PTHREAD_JOIN(int pthread, object retVal);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private static extern unsafe int pthread_setcancelstate(int state, int* oldstate);
        
        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private static extern unsafe int pthread_setcanceltype(int type, int* oldtype);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private static extern unsafe int nanosleep(int* tv_sec, int* tv_nsec);

        /// <summary>
        /// Helper function to set the AbortReason for a thread abort.
        /// Checks that they're not already set, and then atomically updates
        /// the reason info (object + ADID).
        /// </summary>
        /// <param name="o">
        /// </param>
        internal void SetAbortReason(object o)
        {
            this.abortReason = o;
        }

        /// <summary>
        /// Helper function to retrieve the AbortReason from a thread
        /// abort.  Will perform cross-AppDomain marshalling if the object
        /// lives in a different AppDomain from the requester.
        /// </summary>
        /// <returns>
        /// </returns>
        internal object GetAbortReason()
        {
            return this.abortReason;
        }

        /// <summary>
        /// Helper function to clear the AbortReason.  Takes care of
        /// AppDomain related cleanup if required.
        /// </summary>
        internal void ClearAbortReason()
        {
            this.abortReason = null;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static Thread GetCurrentThreadNative()
        {
            return currentThread;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static AppDomain GetDomainInternal()
        {
            return currentDomain;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static AppDomain GetFastDomainInternal()
        {
            return currentDomain;
        }

        /// <summary>
        /// </summary>
        /// <param name="millisecondsTimeout">
        /// </param>
        private static void SleepInternal(int millisecondsTimeout)
        {
            unsafe
            {
                var timestruct = stackalloc int[2];
                timestruct[0] = millisecondsTimeout / 1000;
                timestruct[1] = (millisecondsTimeout % 1000) * 1000000;
                var returnCode = nanosleep(&timestruct[0], null);
                switch ((ReturnCode)returnCode)
                {
                    case ReturnCode.EFAULT:
                        throw new InvalidOperationException("Problem with copying information from user space.");
                    case ReturnCode.EINTR:
                        throw new InvalidOperationException("The pause has been interrupted by a signal that was delivered to the thread.  The remaining sleep time has been written into *rem so that the thread can easily call nanosleep() again and continue with the pause.");
                    case ReturnCode.EINVAL:
                        throw new InvalidOperationException("The value in the tv_nsec field was not in the range 0 to 999999999 or tv_sec was negative.");
                }
            }
        }

        /// <summary>
        /// Helper method to get a logical thread ID for StringBuilder (for
        /// correctness) and for FileStream's async code path (for perf, to
        /// avoid creating a Thread instance).
        /// </summary>
        /// <returns>
        /// </returns>
        internal static IntPtr InternalGetCurrentThread()
        {
            return new IntPtr(0);
        }

        /// <summary>
        /// </summary>
        /// <param name="t">
        /// </param>
        /// <param name="name">
        /// </param>
        /// <param name="len">
        /// </param>
        private static void InformThreadNameChange(ThreadHandle t, string name, int len)
        {
        }

#if! FEATURE_LEAK_CULTURE_INFO

        /// <summary>
        /// </summary>
        private static void nativeInitCultureAccessors()
        {
        }

#endif

        /// <summary>
        /// wait for a length of time proportial to 'iterations'.  Each iteration is should
        /// only take a few machine instructions.  Calling this API is preferable to coding
        /// a explict busy loop because the hardware can be informed that it is busy waiting.
        /// </summary>
        /// <param name="iterations">
        /// </param>
        private static void SpinWaitInternal(int iterations)
        {
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static ulong GetProcessDefaultStackSize()
        {
            return 16 * 1024;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool YieldInternal()
        {
            return false;
        }


        /// <summary>
        /// </summary>
        private void InternalFinalize()
        {
        }

        private delegate void pthread_tart_routine_delegate(object arg);

        private static void pthread_tart_routine(object arg)
        {
            unsafe
            {
                var returnCode = pthread_setcancelstate((int)PThreadCancel.Enable, null);
                switch ((ReturnCode)returnCode)
                {
                    case ReturnCode.EINVAL:
                        throw new InvalidOperationException("Invalid value for state");
                }
            }

            var parameterizedStart = arg as ParameterizedStart;
            if (parameterizedStart != null)
            {
                ((ParameterizedThreadStart)parameterizedStart.@delegate)(parameterizedStart.obj);
            }
            else
            {
                ((ThreadStart)arg)();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="principal">
        /// </param>
        /// <param name="stackMark">
        /// </param>
        private void StartInternal(IPrincipal principal, ref StackCrawlMark stackMark)
        {
            unsafe
            {
                var returnCode = pthread_attr_init(ref pthreadAttr);
                switch ((ReturnCode)returnCode)
                {
                    case ReturnCode.ENOMEM:
                        throw new InvalidOperationException("Insufficient memory exists to initialise the thread attributes object.");
                }

                if (this.maxStackSize > 0)
                {
                    returnCode = pthread_attr_setstacksize(ref pthreadAttr, this.maxStackSize);
                    switch ((ReturnCode)returnCode)
                    {
                        case ReturnCode.EINVAL:
                            throw new InvalidOperationException("The stack size is less than PTHREAD_STACK_MIN (16384) bytes.");
                    }
                }

                returnCode = GC_PTHREAD_CREATE(
                    ref pthread,
                    ref pthreadAttr,
                    new pthread_tart_routine_delegate(pthread_tart_routine).ToPointer(),
                    (this.start is ThreadStart) ? (object)this.start : (object)new ParameterizedStart { @delegate = this.start, obj = m_ThreadStartArg });
                switch ((ReturnCode)returnCode)
                {
                    case ReturnCode.EPERM:
                        throw new InvalidOperationException("The caller does not have appropriate permission to set the required scheduling parameters or scheduling policy.");
                    case ReturnCode.EINVAL:
                        throw new InvalidOperationException("The value specified by attr is invalid.");
                    case ReturnCode.EAGAIN:
                        throw new InvalidOperationException("The system lacked the necessary resources to create another thread, or the system-imposed limit on the total number of threads in a process PTHREAD_THREADS_MAX would be exceeded.");
                }
            }

            this.state = ThreadState.Running;
        }

        /// <summary>
        /// </summary>
        /// <param name="millisecondsTimeout">
        /// </param>
        /// <returns>
        /// </returns>
        private bool JoinInternal(int millisecondsTimeout)
        {
            this.state = ThreadState.WaitSleepJoin;

            var returnCode = GC_PTHREAD_JOIN(this.pthread, null);
            switch ((ReturnCode)returnCode)
            {
                case ReturnCode.EDEADLK:
                    throw new InvalidOperationException("A deadlock was detected (e.g., two threads tried to join with each other); or thread specifies the calling thread.");
                case ReturnCode.EINVAL:
                    throw new InvalidOperationException("Another thread is already waiting to join with this thread or thread is not a joinable thread");
                case ReturnCode.ESRCH:
                    throw new InvalidOperationException("No thread with the ID thread could be found.");
            }

            this.state = ThreadState.Running;

            return true;
        }

        /// <summary>
        /// </summary>
        private void AbortInternal()
        {
            this.state = ThreadState.AbortRequested;

            var returnCode = GC_PTHREAD_KILL(this.pthread, (int)Signals.Terminate);
            switch ((ReturnCode)returnCode)
            {
                case ReturnCode.EINVAL:
                    throw new InvalidOperationException("An invalid signal was specified.");
                case ReturnCode.ESRCH:
                    throw new InvalidOperationException("No thread with the ID thread could be found.");
            }

            this.state = ThreadState.Aborted;
        }

        /// <summary>
        /// </summary>
        private void CancelInternal()
        {
            this.state = ThreadState.AbortRequested;

            var returnCode = GC_PTHREAD_CANCEL(this.pthread);
            switch ((ReturnCode)returnCode)
            {
                case ReturnCode.ESRCH:
                    throw new InvalidOperationException("No thread with the ID thread could be found.");
            }

            this.state = ThreadState.Aborted;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private int GetPriorityNative()
        {
            return 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="priority">
        /// </param>
        private void SetPriorityNative(int priority)
        {
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private int GetThreadStateNative()
        {
            return (int)this.state;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private bool IsBackgroundNative()
        {
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="isBackground">
        /// </param>
        private void SetBackgroundNative(bool isBackground)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="start">
        /// </param>
        /// <param name="maxStackSize">
        /// </param>
        private void SetStart(Delegate start, int maxStackSize)
        {
            if (this.m_ManagedThreadId == 0)
            {
                m_ManagedThreadId = ++threadId;
            }

            this.start = start;
            this.maxStackSize = maxStackSize;
        }

        private class ParameterizedStart
        {
            public Delegate @delegate;
            public object obj;
        }

        private unsafe struct PthreadAttr
        {
            private int state;
            private void* stack;
            private int size;
            private SchedParam @param;
        }

        private struct SchedParam
        {
            private int schedPriority;
        }

        private enum PThreadCancel
        {
            Disable = 0,
            Enable = 1
        }

        private enum Signals
        {
            Terminate = 15
        }

        private enum ReturnCode
        {
            EPERM = 1,      /* Operation not permitted */
            ENOENT = 2,      /* No such file or directory */
            ESRCH = 3,      /* No such process */
            EINTR = 4,      /* Interrupted system call */
            EIO = 5,      /* I/O error */
            ENXIO = 6,      /* No such device or address */
            E2BIG = 7,      /* Arg list too long */
            ENOEXEC = 8,      /* Exec format error */
            EBADF = 9,      /* Bad file number */
            ECHILD = 10,      /* No child processes */
            EAGAIN = 11,      /* Try again */
            ENOMEM = 12,      /* Out of memory */
            EACCES = 13,      /* Permission denied */
            EFAULT = 14,      /* Bad address */
            ENOTBLK = 15,      /* Block device required */
            EBUSY = 16,      /* Device or resource busy */
            EEXIST = 17,      /* File exists */
            EXDEV = 18,      /* Cross-device link */
            ENODEV = 19,      /* No such device */
            ENOTDIR = 20,      /* Not a directory */
            EISDIR = 21,      /* Is a directory */
            EINVAL = 22,      /* Invalid argument */
            ENFILE = 23,      /* File table overflow */
            EMFILE = 24,      /* Too many open files */
            ENOTTY = 25,      /* Not a typewriter */
            ETXTBSY = 26,      /* Text file busy */
            EFBIG = 27,      /* File too large */
            ENOSPC = 28,      /* No space left on device */
            ESPIPE = 29,      /* Illegal seek */
            EROFS = 30,      /* Read-only file system */
            EMLINK = 31,      /* Too many links */
            EPIPE = 32,      /* Broken pipe */
            EDOM = 33,      /* Math argument out of domain of func */
            ERANGE = 34,      /* Math result not representable */
            EDEADLK = 35,      /* Resource deadlock would occur */
            ENAMETOOLONG = 36,      /* File name too long */
            ENOLCK = 37,      /* No record locks available */
            ENOSYS = 38,      /* Function not implemented */
            ENOTEMPTY = 39,      /* Directory not empty */
            ELOOP = 40,      /* Too many symbolic links encountered */
            EWOULDBLOCK = EAGAIN,   /* Operation would block */
            ENOMSG = 42,      /* No message of desired type */
            EIDRM = 43,      /* Identifier removed */
            ECHRNG = 44,      /* Channel number out of range */
            EL2NSYNC = 45,      /* Level 2 not synchronized */
            EL3HLT = 46,      /* Level 3 halted */
            EL3RST = 47,      /* Level 3 reset */
            ELNRNG = 48,      /* Link number out of range */
            EUNATCH = 49,      /* Protocol driver not attached */
            ENOCSI = 50,      /* No CSI structure available */
            EL2HLT = 51,      /* Level 2 halted */
            EBADE = 52,      /* Invalid exchange */
            EBADR = 53,      /* Invalid request descriptor */
            EXFULL = 54,      /* Exchange full */
            ENOANO = 55,      /* No anode */
            EBADRQC = 56,      /* Invalid request code */
            EBADSLT = 57,      /* Invalid slot */

            EDEADLOCK = EDEADLK,

            EBFONT = 59,      /* Bad font file format */
            ENOSTR = 60,      /* Device not a stream */
            ENODATA = 61,      /* No data available */
            ETIME = 62,      /* Timer expired */
            ENOSR = 63,      /* Out of streams resources */
            ENONET = 64,      /* Machine is not on the network */
            ENOPKG = 65,      /* Package not installed */
            EREMOTE = 66,      /* Object is remote */
            ENOLINK = 67,      /* Link has been severed */
            EADV = 68,      /* Advertise error */
            ESRMNT = 69,      /* Srmount error */
            ECOMM = 70,      /* Communication error on send */
            EPROTO = 71,      /* Protocol error */
            EMULTIHOP = 72,      /* Multihop attempted */
            EDOTDOT = 73,      /* RFS specific error */
            EBADMSG = 74,      /* Not a data message */
            EOVERFLOW = 75,      /* Value too large for defined data type */
            ENOTUNIQ = 76,      /* Name not unique on network */
            EBADFD = 77,      /* File descriptor in bad state */
            EREMCHG = 78,      /* Remote address changed */
            ELIBACC = 79,      /* Can not access a needed shared library */
            ELIBBAD = 80,      /* Accessing a corrupted shared library */
            ELIBSCN = 81,      /* .lib section in a.out corrupted */
            ELIBMAX = 82,      /* Attempting to link in too many shared libraries */
            ELIBEXEC = 83,      /* Cannot exec a shared library directly */
            EILSEQ = 84,      /* Illegal byte sequence */
            ERESTART = 85,      /* Interrupted system call should be restarted */
            ESTRPIPE = 86,      /* Streams pipe error */
            EUSERS = 87,      /* Too many users */
            ENOTSOCK = 88,      /* Socket operation on non-socket */
            EDESTADDRREQ = 89,      /* Destination address required */
            EMSGSIZE = 90,      /* Message too long */
            EPROTOTYPE = 91,      /* Protocol wrong type for socket */
            ENOPROTOOPT = 92,      /* Protocol not available */
            EPROTONOSUPPORT = 93,      /* Protocol not supported */
            ESOCKTNOSUPPORT = 94,      /* Socket type not supported */
            EOPNOTSUPP = 95,      /* Operation not supported on transport endpoint */
            EPFNOSUPPORT = 96,      /* Protocol family not supported */
            EAFNOSUPPORT = 97,      /* Address family not supported by protocol */
            EADDRINUSE = 98,      /* Address already in use */
            EADDRNOTAVAIL = 99,      /* Cannot assign requested address */
            ENETDOWN = 100,     /* Network is down */
            ENETUNREACH = 101,     /* Network is unreachable */
            ENETRESET = 102,     /* Network dropped connection because of reset */
            ECONNABORTED = 103,     /* Software caused connection abort */
            ECONNRESET = 104,     /* Connection reset by peer */
            ENOBUFS = 105,     /* No buffer space available */
            EISCONN = 106,     /* Transport endpoint is already connected */
            ENOTCONN = 107,     /* Transport endpoint is not connected */
            ESHUTDOWN = 108,     /* Cannot send after transport endpoint shutdown */
            ETOOMANYREFS = 109,     /* Too many references: cannot splice */
            ETIMEDOUT = 110,     /* Connection timed out */
            ECONNREFUSED = 111,     /* Connection refused */
            EHOSTDOWN = 112,     /* Host is down */
            EHOSTUNREACH = 113,     /* No route to host */
            EALREADY = 114,     /* Operation already in progress */
            EINPROGRESS = 115,     /* Operation now in progress */
            ESTALE = 116,     /* Stale NFS file handle */
            EUCLEAN = 117,     /* Structure needs cleaning */
            ENOTNAM = 118,     /* Not a XENIX named type file */
            ENAVAIL = 119,     /* No XENIX semaphores available */
            EISNAM = 120,     /* Is a named type file */
            EREMOTEIO = 121,     /* Remote I/O error */
            EDQUOT = 122,     /* Quota exceeded */

            ENOMEDIUM = 123,     /* No medium found */
            EMEDIUMTYPE = 124     /* Wrong medium type */
        }
    }
}