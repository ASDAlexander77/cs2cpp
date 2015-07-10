// Licensed under the MIT license.

namespace System.Diagnostics.Tracing
{
    public partial class EventSource
    {
        /// <summary>
        /// The human-friendly name of the eventSource.  It defaults to the simple name of the class
        /// </summary>
        public string Name { get { return string.Empty; } }
        /// <summary>
        /// Every eventSource is assigned a GUID to uniquely identify it to the system. 
        /// </summary>
        public Guid Guid { get { return Guid.Empty; } }

        /// <summary>
        /// Returns true if the eventSource has been enabled at all. This is the prefered test
        /// to be performed before a relatively expensive EventSource operation.
        /// </summary>
        public bool IsEnabled()
        {
            return false;
        }


        /// <summary>
        /// When a thread starts work that is on behalf of 'something else' (typically another 
        /// thread or network request) it should mark the thread as working on that other work.
        /// This API marks the current thread as working on activity 'activityID'. It returns 
        /// whatever activity the thread was previously marked with. There is a convention that
        /// callers can assume that callees restore this activity mark before the callee returns. 
        /// To encourage this this API returns the old activity, so that it can be restored later.
        /// 
        /// All events created with the EventSource on this thread are also tagged with the 
        /// activity ID of the thread. 
        /// 
        /// It is common, and good practice after setting the thread to an activity to log an event
        /// with a 'start' opcode to indicate that precise time/thread where the new activity 
        /// started.
        /// </summary>
        /// <param name="activityId">A Guid that represents the new activity with which to mark 
        /// the current thread</param>
        /// <param name="oldActivityThatWillContinue">The Guid that represents the current activity  
        /// which will continue at some point in the future, on the current thread</param>
        public static void SetCurrentThreadActivityId(Guid activityId, out Guid oldActivityThatWillContinue)
        {
            oldActivityThatWillContinue = new Guid();
        }

        public static void SetCurrentThreadActivityId(Guid activityId)
        {
        }

        /// <summary>
        /// Retrieves the ETW activity ID associated with the current thread.
        /// </summary>
        public static Guid CurrentThreadActivityId
        {
            get
            {
                // We ignore errors to keep with the convention that EventSources do not throw 
                // errors. Note we can't access m_throwOnWrites because this is a static method.
                Guid retVal = new Guid();
                return retVal;
            }
        }
    }
}

