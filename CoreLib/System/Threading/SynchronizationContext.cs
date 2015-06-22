// Licensed under the MIT license.

namespace System.Threading
{    
    public class SynchronizationContext
    {
        public SynchronizationContext()
        {
        }
                        
        public virtual void Send(SendOrPostCallback d, Object state)
        {
            d(state);
        }

        public virtual void Post(SendOrPostCallback d, Object state)
        {
            throw new NotImplementedException();
        }

        
        /// <summary>
        ///     Optional override for subclasses, for responding to notification that operation is starting.
        /// </summary>
        public virtual void OperationStarted()
        {
        }

        /// <summary>
        ///     Optional override for subclasses, for responding to notification that operation has completed.
        /// </summary>
        public virtual void OperationCompleted()
        {
        }

        // set SynchronizationContext on the current thread
        [System.Security.SecurityCritical]  // auto-generated_required
        public static void SetSynchronizationContext(SynchronizationContext syncContext)
        {
        }

        // Get the current SynchronizationContext on the current thread
        public static SynchronizationContext Current 
        {
            get      
            {
                return GetThreadLocalContext();
            }
        }

        // Get the last SynchronizationContext that was set explicitly (not flowed via ExecutionContext.Capture/Run)        
        internal static SynchronizationContext CurrentNoFlow
        {
            get
            {
                return GetThreadLocalContext();
            }
        }

        private static SynchronizationContext GetThreadLocalContext()
        {
            SynchronizationContext context = null;
            return context;
        }

        // helper to Clone this SynchronizationContext, 
        public virtual SynchronizationContext CreateCopy()
        {
            // the CLR dummy has an empty clone function - no member data
            return new SynchronizationContext();
        }
    }
}
