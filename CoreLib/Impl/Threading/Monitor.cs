namespace System.Threading
{
    using System.Runtime.CompilerServices;
    using System.Runtime.ConstrainedExecution;
    using System.Security.Principal;

    /// <summary>
    /// Partial implementation of Monitor
    /// </summary>
    public partial class Monitor
    {
        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private static extern unsafe int pthread_mutex_lock(void* mutex);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private static extern unsafe int pthread_mutex_trylock(void* mutex);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private static extern unsafe int pthread_mutex_unlock(void* mutex);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private static extern unsafe int pthread_mutex_timedlock(void* mutex, int* timespec_timeout);

        /*=========================================================================
        ** Obtain the monitor lock of obj. Will block if another thread holds the lock
        ** Will not block if the current thread holds the lock,
        ** however the caller must ensure that the same number of Exit
        ** calls are made as there were Enter calls.
        **
        ** Exceptions: ArgumentNullException if object is null.
        =========================================================================*/
        public static void Enter(Object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            unsafe
            {
                var returnCode = pthread_mutex_lock(GetLockAddress(obj));
                switch ((Thread.ReturnCode)returnCode)
                {
                    case Thread.ReturnCode.EBUSY:
                        throw new InvalidOperationException("The implementation has detected an attempt to destroy the object referenced by mutex while it is locked or referenced (for example, while being used in a pthread_cond_timedwait() or pthread_cond_wait()) by another thread.");
                    case Thread.ReturnCode.EINVAL:
                        throw new InvalidOperationException("The value specified by mutex is invalid.");
                    case Thread.ReturnCode.EAGAIN:
                        throw new InvalidOperationException("The system lacked the necessary resources (other than memory) to initialize another mutex.");
                    case Thread.ReturnCode.EDEADLK:
                        throw new InvalidOperationException("The current thread already owns the mutex.");
                    case Thread.ReturnCode.EPERM:
                        throw new InvalidOperationException("The current thread does not own the mutex.");
                }
            }
        }

        /*=========================================================================
        ** Release the monitor lock. If one or more threads are waiting to acquire the
        ** lock, and the current thread has executed as many Exits as
        ** Enters, one of the threads will be unblocked and allowed to proceed.
        **
        ** Exceptions: ArgumentNullException if object is null.
        **             SynchronizationLockException if the current thread does not
        **             own the lock.
        =========================================================================*/
        public static void Exit(Object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            unsafe
            {
                var returnCode = pthread_mutex_unlock(GetLockAddress(obj));
                switch ((Thread.ReturnCode)returnCode)
                {
                    case Thread.ReturnCode.EBUSY:
                        throw new InvalidOperationException(
                            "The implementation has detected an attempt to destroy the object referenced by mutex while it is locked or referenced (for example, while being used in a pthread_cond_timedwait() or pthread_cond_wait()) by another thread.");
                    case Thread.ReturnCode.EINVAL:
                        throw new InvalidOperationException("The value specified by mutex is invalid.");
                    case Thread.ReturnCode.EAGAIN:
                        throw new InvalidOperationException("The system lacked the necessary resources (other than memory) to initialize another mutex.");
                    case Thread.ReturnCode.EDEADLK:
                        throw new InvalidOperationException("The current thread already owns the mutex.");
                    case Thread.ReturnCode.EPERM:
                        throw new InvalidOperationException("The current thread does not own the mutex.");
                }
            }
        }

        private static void ReliableEnter(Object obj, ref bool lockTaken)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            unsafe
            {
                var returnCode = pthread_mutex_trylock(GetLockAddress(obj));
                switch ((Thread.ReturnCode)returnCode)
                {
                    case Thread.ReturnCode.EINVAL:
                        throw new InvalidOperationException("The value specified by mutex is invalid.");
                    case Thread.ReturnCode.EAGAIN:
                        throw new InvalidOperationException("The system lacked the necessary resources (other than memory) to initialize another mutex.");
                    case Thread.ReturnCode.EDEADLK:
                        throw new InvalidOperationException("The current thread already owns the mutex.");
                    case Thread.ReturnCode.EPERM:
                        throw new InvalidOperationException("The current thread does not own the mutex.");
                }

                lockTaken = returnCode == 0;
            }
        }

        private static extern unsafe void* GetLockAddress(object o);

        private static void ReliableEnterTimeout(Object obj, int timeout, ref bool lockTaken)
        {
            unsafe
            {
                var timestruct = stackalloc int[2];
                timestruct[0] = timeout / 1000;
                timestruct[1] = (timeout % 1000) * 1000000;

                var returnCode = pthread_mutex_timedlock(GetLockAddress(obj), &timestruct[0]);
                switch ((Thread.ReturnCode)returnCode)
                {
                    case Thread.ReturnCode.EINVAL:
                        throw new InvalidOperationException("The value specified by mutex is invalid.");
                    case Thread.ReturnCode.EAGAIN:
                        throw new InvalidOperationException("The system lacked the necessary resources (other than memory) to initialize another mutex.");
                    case Thread.ReturnCode.EDEADLK:
                        throw new InvalidOperationException("The current thread already owns the mutex.");
                    case Thread.ReturnCode.EPERM:
                        throw new InvalidOperationException("The current thread does not own the mutex.");
                }

                lockTaken = returnCode == 0;
            }
        }

        private static bool IsEnteredNative(Object obj)
        {
            throw new NotImplementedException();
        }

        /*========================================================================
        ** Waits for notification from the object (via a Pulse/PulseAll). 
        ** timeout indicates how long to wait before the method returns.
        ** This method acquires the monitor waithandle for the object 
        ** If this thread holds the monitor lock for the object, it releases it. 
        ** On exit from the method, it obtains the monitor lock back. 
        ** If exitContext is true then the synchronization domain for the context 
        ** (if in a synchronized context) is exited before the wait and reacquired 
        **
            ** Exceptions: ArgumentNullException if object is null.
        ========================================================================*/
        private static bool ObjWait(bool exitContext, int millisecondsTimeout, Object obj)
        {
            throw new NotImplementedException();
        }

        /*========================================================================
        ** Sends a notification to a single waiting object. 
        * Exceptions: SynchronizationLockException if this method is not called inside
        * a synchronized block of code.
        ========================================================================*/
        private static void ObjPulse(Object obj)
        {
            throw new NotImplementedException();
        }

        /*========================================================================
        ** Sends a notification to all waiting objects. 
        ========================================================================*/
        private static void ObjPulseAll(Object obj)
        {
            throw new NotImplementedException();
        }
    }
}
