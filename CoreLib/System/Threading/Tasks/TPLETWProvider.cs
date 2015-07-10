// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// =+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+
//
//
//
// EventSource for TPL.
//
// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Permissions;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    internal sealed class TplEtwProvider
    {
        /// Used to determine if tasks should generate Activity IDs for themselves
        internal bool TasksSetActivityIds;        // This keyword is set
        internal bool Debug;
        private  bool DebugActivityId;

        /// <summary>
        /// Defines the singleton instance for the TPL ETW provider.
        /// The TPL Event provider GUID is {2e5dba47-a3d2-4d16-8ee0-6671ffdcd7b5}.
        /// </summary>
        public static TplEtwProvider Log = new TplEtwProvider();
        /// <summary>Prevent external instantiation.  All logging should go through the Log instance.</summary>
        private TplEtwProvider() { }

        /// <summary>Type of a fork/join operation.</summary>
        public enum ForkJoinOperationType
        {
            /// <summary>Parallel.Invoke.</summary>
            ParallelInvoke=1,
            /// <summary>Parallel.For.</summary>
            ParallelFor=2,
            /// <summary>Parallel.ForEach.</summary>
            ParallelForEach=3
        }

        /// <summary>Configured behavior of a task wait operation.</summary>
        public enum TaskWaitBehavior : int
        {
            /// <summary>A synchronous wait.</summary>
            Synchronous = 1,
            /// <summary>An asynchronous await.</summary>
            Asynchronous = 2
        }

        //-----------------------------------------------------------------------------------
        //        
        // TPL Event IDs (must be unique)
        //

        /// <summary>The beginning of a parallel loop.</summary>
        private const int PARALLELLOOPBEGIN_ID = 1;
        /// <summary>The ending of a parallel loop.</summary>
        private const int PARALLELLOOPEND_ID = 2;
        /// <summary>The beginning of a parallel invoke.</summary>
        private const int PARALLELINVOKEBEGIN_ID = 3;
        /// <summary>The ending of a parallel invoke.</summary>
        private const int PARALLELINVOKEEND_ID = 4;
        /// <summary>A task entering a fork/join construct.</summary>
        private const int PARALLELFORK_ID = 5;
        /// <summary>A task leaving a fork/join construct.</summary>
        private const int PARALLELJOIN_ID = 6;

        /// <summary>A task is scheduled to a task scheduler.</summary>
        private const int TASKSCHEDULED_ID = 7;
        /// <summary>A task is about to execute.</summary>
        private const int TASKSTARTED_ID = 8;
        /// <summary>A task has finished executing.</summary>
        private const int TASKCOMPLETED_ID = 9;
        /// <summary>A wait on a task is beginning.</summary>
        private const int TASKWAITBEGIN_ID = 10;
        /// <summary>A wait on a task is ending.</summary>
        private const int TASKWAITEND_ID = 11;
        /// <summary>A continuation of a task is scheduled.</summary>
        private const int AWAITTASKCONTINUATIONSCHEDULED_ID = 12;
        /// <summary>A continuation of a taskWaitEnd is complete </summary>
        private const int TASKWAITCONTINUATIONCOMPLETE_ID = 13;
        /// <summary>A continuation of a taskWaitEnd is complete </summary>
        private const int TASKWAITCONTINUATIONSTARTED_ID = 19;

        private const int TRACEOPERATIONSTART_ID       = 14;
        private const int TRACEOPERATIONSTOP_ID        = 15;
        private const int TRACEOPERATIONRELATION_ID    = 16;
        private const int TRACESYNCHRONOUSWORKSTART_ID = 17;
        private const int TRACESYNCHRONOUSWORKSTOP_ID  = 18;

        public static Guid CreateGuidForTaskID(int continuationId)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled()
        {
            return false;
        }

        public void ParallelFork(int i, int i1, int forkJoinContextId)
        {
            throw new NotImplementedException();
        }

        public void AwaitTaskContinuationScheduled(int id, int i, int continuationId)
        {
            throw new NotImplementedException();
        }

        public void RunningContinuationList(int id, int p1, object currentContinuation)
        {
            throw new NotImplementedException();
        }

        public void TaskStarted(int id, int p1, int p2)
        {
            throw new NotImplementedException();
        }

        public void ParallelJoin(int i, int i1, int forkJoinContextId)
        {
            throw new NotImplementedException();
        }

        public void ParallelInvokeEnd(int i, int i1, int forkJoinContextId)
        {
            throw new NotImplementedException();
        }

        public void NewID(int newId)
        {
            throw new NotImplementedException();
        }

        public void TaskWaitBegin(int i, int i1, int id, TaskWaitBehavior synchronous, int i2, int getDomainId)
        {
            throw new NotImplementedException();
        }

        public void TaskScheduled(int id, int i, int id1, int i1, int options, int getDomainId)
        {
            throw new NotImplementedException();
        }

        public void TaskCompleted(int id, int p1, int p2, bool isFaulted)
        {
            throw new NotImplementedException();
        }

        public void ParallelInvokeBegin(int i, int i1, int forkJoinContextId, ForkJoinOperationType parallelInvoke, int length)
        {
            throw new NotImplementedException();
        }

        public void TaskWaitEnd(int id, int p1, int p2)
        {
            throw new NotImplementedException();
        }

        public void TaskWaitContinuationComplete(int id)
        {
            throw new NotImplementedException();
        }

        public void TaskWaitContinuationStarted(int continuationId)
        {
            throw new NotImplementedException();
        }

        public void RunningContinuation(int id, object continuationObject)
        {
            throw new NotImplementedException();
        }

        public void ParallelLoopEnd(int i, int i1, int forkJoinContextId, long nTotalIterations)
        {
            throw new NotImplementedException();
        }

        public void ParallelLoopBegin(int i, int i1, int forkJoinContextId, ForkJoinOperationType parallelFor, long fromInclusive, long toExclusive)
        {
            throw new NotImplementedException();
        }
    }
}
