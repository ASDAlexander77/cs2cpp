namespace System.Threading.Tasks
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Utility class for allocating structs as heap variables
    /// </summary>
    internal class Shared<T>
    {
        internal T Value;

        internal Shared(T value)
        {
            this.Value = value;
        }

    }

    /// <summary>
    /// Represents the current stage in the lifecycle of a <see cref="Task"/>.
    /// </summary>
    public enum TaskStatus
    {
        /// <summary> 
        /// The task has been initialized but has not yet been scheduled.
        /// </summary>
        Created,
        /// <summary> 
        /// The task is waiting to be activated and scheduled internally by the .NET Framework infrastructure.
        /// </summary>
        WaitingForActivation,
        /// <summary>
        /// The task has been scheduled for execution but has not yet begun executing.
        /// </summary>
        WaitingToRun,
        /// <summary>
        /// The task is running but has not yet completed.
        /// </summary>
        Running,
        // /// <summary>
        // /// The task is currently blocked in a wait state.
        // /// </summary>
        // Blocked,
        /// <summary>
        /// The task has finished executing and is implicitly waiting for
        /// attached child tasks to complete.
        /// </summary>
        WaitingForChildrenToComplete,
        /// <summary>
        /// The task completed execution successfully.
        /// </summary>
        RanToCompletion,
        /// <summary>
        /// The task acknowledged cancellation by throwing an OperationCanceledException with its own CancellationToken
        /// while the token was in signaled state, or the task's CancellationToken was already signaled before the
        /// task started executing.
        /// </summary>
        Canceled,
        /// <summary>
        /// The task completed due to an unhandled exception.
        /// </summary>
        Faulted
    }

    public class Task : IAsyncResult, IDisposable
    {
        internal Task()
        {
        }

        internal Task(bool canceled, TaskCreationOptions creationOptions, CancellationToken ct)
        {
        }

        public bool IsCompleted { get; private set; }

        public WaitHandle AsyncWaitHandle { get; private set; }

        public object AsyncState { get; private set; }

        public bool CompletedSynchronously { get; private set; }

        public bool IsWaitNotificationEnabledOrNotRanToCompletion { get; private set; }

        public bool IsRanToCompletion { get; private set; }

        public TaskStatus Status { get; private set; }

        public Exception Exception { get; private set; }

        public CancellationToken CancellationToken { get; private set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public static Task Delay(int delay)
        {
            return new Task();
        }

        public static Task Delay(TimeSpan delay)
        {
            return new Task();
        }

        public TaskAwaiter GetAwaiter()
        {
            return new TaskAwaiter(this);
        }

        public void NotifyDebuggerOfWaitCompletionIfNecessary()
        {
            throw new NotImplementedException();
        }

        public bool InternalWait(int infinite, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        internal void SetContinuationForAwait(Action continuation, bool continueOnCapturedContext, bool flowExecutionContext, ref StackCrawlMark stackMark)
        {
            throw new NotImplementedException();
        }

        public void SetNotificationForWaitCompletion(bool enabled)
        {
            throw new NotImplementedException();
        }

        public bool TrySetCanceled(CancellationToken cancellationToken, OperationCanceledException oce)
        {
            throw new NotImplementedException();
        }

        public bool TrySetException(Exception exception)
        {
            throw new NotImplementedException();
        }
    }

    public class Task<TResult> : Task
    {
        public Task()
        {
        }

        public Task(TResult result)
        {
            this.Result = result;
        }

        internal Task(bool canceled, TResult result, TaskCreationOptions creationOptions, CancellationToken ct)
            : base(canceled, creationOptions, ct)
        {
            if (!canceled)
            {
                this.Result = result;
            }
        }

        public TResult Result { get; private set; }

        public TResult ResultOnSuccess { get; private set; }

        public bool TrySetResult(TResult result)
        {
            this.Result = result;
            return true;
        }
    }

    /// <summary>
    /// Specifies flags that control optional behavior for the creation and execution of tasks.
    /// </summary>
    // NOTE: These options are a subset of TaskContinuationsOptions, thus before adding a flag check it is
    // not already in use.
    [Flags]
    [Serializable]
    public enum TaskCreationOptions
    {
        /// <summary>
        /// Specifies that the default behavior should be used.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// A hint to a <see cref="System.Threading.Tasks.TaskScheduler">TaskScheduler</see> to schedule a
        /// task in as fair a manner as possible, meaning that tasks scheduled sooner will be more likely to
        /// be run sooner, and tasks scheduled later will be more likely to be run later.
        /// </summary>
        PreferFairness = 0x01,

        /// <summary>
        /// Specifies that a task will be a long-running, course-grained operation. It provides a hint to the
        /// <see cref="System.Threading.Tasks.TaskScheduler">TaskScheduler</see> that oversubscription may be
        /// warranted. 
        /// </summary>
        LongRunning = 0x02,

        /// <summary>
        /// Specifies that a task is attached to a parent in the task hierarchy.
        /// </summary>
        AttachedToParent = 0x04,

        /// <summary>
        /// Specifies that an InvalidOperationException will be thrown if an attempt is made to attach a child task to the created task.
        /// </summary>
        DenyChildAttach = 0x08,

        /// <summary>
        /// Prevents the ambient scheduler from being seen as the current scheduler in the created task.  This means that operations
        /// like StartNew or ContinueWith that are performed in the created task will see TaskScheduler.Default as the current scheduler.
        /// </summary>
        HideScheduler = 0x10,

        // 0x20 is already being used in TaskContinuationOptions

        /// <summary>
        /// Forces continuations added to the current task to be executed asynchronously.
        /// This option has precedence over TaskContinuationOptions.ExecuteSynchronously
        /// </summary>
        RunContinuationsAsynchronously = 0x40
    }


    /// <summary>
    /// Task creation flags which are only used internally.
    /// </summary>
    [Flags]
    [Serializable]
    internal enum InternalTaskOptions
    {
        /// <summary> Specifies "No internal task options" </summary>
        None,

        /// <summary>Used to filter out internal vs. public task creation options.</summary>
        InternalOptionsMask = 0x0000FF00,

        ChildReplica = 0x0100,
        ContinuationTask = 0x0200,
        PromiseTask = 0x0400,
        SelfReplicating = 0x0800,

        /// <summary>
        /// Store the presence of TaskContinuationOptions.LazyCancellation, since it does not directly
        /// translate into any TaskCreationOptions.
        /// </summary>
        LazyCancellation = 0x1000,

        /// <summary>Specifies that the task will be queued by the runtime before handing it over to the user. 
        /// This flag will be used to skip the cancellationtoken registration step, which is only meant for unstarted tasks.</summary>
        QueuedByRuntime = 0x2000,

        /// <summary>
        /// Denotes that Dispose should be a complete nop for a Task.  Used when constructing tasks that are meant to be cached/reused.
        /// </summary>
        DoNotDispose = 0x4000
    }

    /// <summary>
    /// Specifies flags that control optional behavior for the creation and execution of continuation tasks.
    /// </summary>
    [Flags]
    [Serializable]
    public enum TaskContinuationOptions
    {
        /// <summary>
        /// Default = "Continue on any, no task options, run asynchronously"
        /// Specifies that the default behavior should be used.  Continuations, by default, will
        /// be scheduled when the antecedent task completes, regardless of the task's final <see
        /// cref="System.Threading.Tasks.TaskStatus">TaskStatus</see>.
        /// </summary>
        None = 0,

        // These are identical to their meanings and values in TaskCreationOptions

        /// <summary>
        /// A hint to a <see cref="System.Threading.Tasks.TaskScheduler">TaskScheduler</see> to schedule a
        /// task in as fair a manner as possible, meaning that tasks scheduled sooner will be more likely to
        /// be run sooner, and tasks scheduled later will be more likely to be run later.
        /// </summary>
        PreferFairness = 0x01,

        /// <summary>
        /// Specifies that a task will be a long-running, course-grained operation.  It provides
        /// a hint to the <see cref="System.Threading.Tasks.TaskScheduler">TaskScheduler</see> that
        /// oversubscription may be warranted.
        /// </summary>
        LongRunning = 0x02,
        /// <summary>
        /// Specifies that a task is attached to a parent in the task hierarchy.
        /// </summary>
        AttachedToParent = 0x04,

        /// <summary>
        /// Specifies that an InvalidOperationException will be thrown if an attempt is made to attach a child task to the created task.
        /// </summary>
        DenyChildAttach = 0x08,
        /// <summary>
        /// Prevents the ambient scheduler from being seen as the current scheduler in the created task.  This means that operations
        /// like StartNew or ContinueWith that are performed in the created task will see TaskScheduler.Default as the current scheduler.
        /// </summary>
        HideScheduler = 0x10,

        /// <summary>
        /// In the case of continuation cancellation, prevents completion of the continuation until the antecedent has completed.
        /// </summary>
        LazyCancellation = 0x20,

        RunContinuationsAsynchronously = 0x40,

        // These are specific to continuations

        /// <summary>
        /// Specifies that the continuation task should not be scheduled if its antecedent ran to completion.
        /// This option is not valid for multi-task continuations.
        /// </summary>
        NotOnRanToCompletion = 0x10000,
        /// <summary>
        /// Specifies that the continuation task should not be scheduled if its antecedent threw an unhandled
        /// exception. This option is not valid for multi-task continuations.
        /// </summary>
        NotOnFaulted = 0x20000,
        /// <summary>
        /// Specifies that the continuation task should not be scheduled if its antecedent was canceled. This
        /// option is not valid for multi-task continuations.
        /// </summary>
        NotOnCanceled = 0x40000,
        /// <summary>
        /// Specifies that the continuation task should be scheduled only if its antecedent ran to
        /// completion. This option is not valid for multi-task continuations.
        /// </summary>
        OnlyOnRanToCompletion = NotOnFaulted | NotOnCanceled,
        /// <summary>
        /// Specifies that the continuation task should be scheduled only if its antecedent threw an
        /// unhandled exception. This option is not valid for multi-task continuations.
        /// </summary>
        OnlyOnFaulted = NotOnRanToCompletion | NotOnCanceled,
        /// <summary>
        /// Specifies that the continuation task should be scheduled only if its antecedent was canceled.
        /// This option is not valid for multi-task continuations.
        /// </summary>
        OnlyOnCanceled = NotOnRanToCompletion | NotOnFaulted,
        /// <summary>
        /// Specifies that the continuation task should be executed synchronously. With this option
        /// specified, the continuation will be run on the same thread that causes the antecedent task to
        /// transition into its final state. If the antecedent is already complete when the continuation is
        /// created, the continuation will run on the thread creating the continuation.  Only very
        /// short-running continuations should be executed synchronously.
        /// </summary>
        ExecuteSynchronously = 0x80000
    }

    // Special internal struct that we use to signify that we are not interested in
    // a Task<VoidTaskResult>'s result.
    internal struct VoidTaskResult { }

    // Interface to which all completion actions must conform.
    // This interface allows us to combine functionality and reduce allocations.
    // For example, see Task.SetOnInvokeMres, and its use in Task.SpinThenBlockingWait().
    // This code:
    //      ManualResetEvent mres = new ManualResetEventSlim(false, 0);
    //      Action<Task> completionAction = delegate { mres.Set() ; };
    //      AddCompletionAction(completionAction);
    // gets replaced with this:
    //      SetOnInvokeMres mres = new SetOnInvokeMres();
    //      AddCompletionAction(mres);
    // For additional examples of where this is used, see internal classes Task.SignalOnInvokeCDE,
    // Task.WhenAllPromise, Task.WhenAllPromise<T>, TaskFactory.CompleteOnCountdownPromise,
    // TaskFactory.CompleteOnCountdownPromise<T>, and TaskFactory.CompleteOnInvokePromise.
    internal interface ITaskCompletionAction
    {
        void Invoke(Task completingTask);
    }
}
