// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
/*============================================================
**
**
**
** Purpose: Capture execution  context for a thread
**
** 
===========================================================*/
namespace System.Threading
{    
    using System;
    using System.Security;
    using System.Runtime.Remoting;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
#if FEATURE_EXCEPTIONDISPATCHINFO
    using System.Runtime.ExceptionServices;
#endif
    using System.Security.Permissions;
#if FEATURE_REMOTING
    using System.Runtime.Remoting.Messaging;
#endif // FEATURE_REMOTING
    using System.Runtime.InteropServices;
    using System.Runtime.CompilerServices;
    using System.Runtime.ConstrainedExecution;
    using System.Diagnostics.Contracts;
    using System.Diagnostics.CodeAnalysis;

#if FEATURE_CORECLR
    [System.Security.SecurityCritical] // auto-generated
#endif
    [System.Runtime.InteropServices.ComVisible(true)]
    public delegate void ContextCallback(Object state);

#if FEATURE_CORECLR

    [SecurityCritical]
    internal struct ExecutionContextSwitcher
    {
        internal ExecutionContext m_ec;
        internal SynchronizationContext m_sc;

        internal void Undo()
        {
            SynchronizationContext.SetSynchronizationContext(m_sc);
            ExecutionContext.Restore(m_ec);
        }
    }

    public sealed class ExecutionContext : IDisposable
    {
        public static readonly ExecutionContext Default = new ExecutionContext();

        [ThreadStatic]
        [SecurityCritical]
        static ExecutionContext t_currentMaybeNull;

        private readonly Dictionary<IAsyncLocal, object> m_localValues;
        private readonly List<IAsyncLocal> m_localChangeNotifications;

        private ExecutionContext()
        {
            m_localValues = new Dictionary<IAsyncLocal, object>();
            m_localChangeNotifications = new List<IAsyncLocal>();
        }

        private ExecutionContext(ExecutionContext other)
        {
            m_localValues = new Dictionary<IAsyncLocal, object>(other.m_localValues);
            m_localChangeNotifications = new List<IAsyncLocal>(other.m_localChangeNotifications);
        }

        [SecuritySafeCritical]
        public static ExecutionContext Capture()
        {
            return t_currentMaybeNull ?? ExecutionContext.Default;
        }

#if FEATURE_CORRUPTING_EXCEPTIONS
#if FEATURE_CORECLR
        [System.Security.SecurityCritical] // auto-generated
#else
        [System.Security.SecuritySafeCritical]
#endif
        
        [HandleProcessCorruptedStateExceptions] 
#endif // FEATURE_CORRUPTING_EXCEPTIONS
        public static void Run(ExecutionContext executionContext, ContextCallback callback, Object state)
        {
            ExecutionContextSwitcher ecsw = default(ExecutionContextSwitcher);
            try
            {
                EstablishCopyOnWriteScope(ref ecsw);

                ExecutionContext.Restore(executionContext);
                callback(state);
            }
            catch
            {
                // Note: we have a "catch" rather than a "finally" because we want
                // to stop the first pass of EH here.  That way we can restore the previous
                // context before any of our callers' EH filters run.  That means we need to 
                // end the scope separately in the non-exceptional case below.
                ecsw.Undo();
                throw;
            }
            ecsw.Undo();
        }

        [SecurityCritical]
        internal static void Restore(ExecutionContext executionContext)
        {
            if (executionContext == null)
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NullContext"));

            ExecutionContext previous = t_currentMaybeNull ?? Default;
            t_currentMaybeNull = executionContext;

            if (previous != executionContext)
                OnContextChanged(previous, executionContext);
        }

        [SecurityCritical]
        static internal void EstablishCopyOnWriteScope(ref ExecutionContextSwitcher ecsw)
        {
            ecsw.m_ec = Capture();
            ecsw.m_sc = SynchronizationContext.CurrentNoFlow;
        }

#if FEATURE_CORRUPTING_EXCEPTIONS
#if FEATURE_CORECLR
        [System.Security.SecurityCritical] // auto-generated
#else
        [System.Security.SecuritySafeCritical]
#endif
        
        [HandleProcessCorruptedStateExceptions] 
#endif // FEATURE_CORRUPTING_EXCEPTIONS

        private static void OnContextChanged(ExecutionContext previous, ExecutionContext current)
        {
            previous = previous ?? Default;

            foreach (IAsyncLocal local in previous.m_localChangeNotifications)
            {
                object previousValue;
                object currentValue;
                previous.m_localValues.TryGetValue(local, out previousValue);
                current.m_localValues.TryGetValue(local, out currentValue);

                if (previousValue != currentValue)
                    local.OnValueChanged(previousValue, currentValue, true);
            }

            if (current.m_localChangeNotifications != previous.m_localChangeNotifications)
            {
                try
                {
                    foreach (IAsyncLocal local in current.m_localChangeNotifications)
                    {
                        // If the local has a value in the previous context, we already fired the event for that local
                        // in the code above.
                        object previousValue;
                        if (!previous.m_localValues.TryGetValue(local, out previousValue))
                        {
                            object currentValue;
                            current.m_localValues.TryGetValue(local, out currentValue);

                            if (previousValue != currentValue)
                                local.OnValueChanged(previousValue, currentValue, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Environment.FailFast(
                        Environment.GetResourceString("ExecutionContext_ExceptionInAsyncLocalNotification"), 
                        ex);
                }
            }        
        }

        [SecurityCritical]
        internal static object GetLocalValue(IAsyncLocal local)
        {
            ExecutionContext current = t_currentMaybeNull;
            if (current == null)
                return null;

            object value;
            current.m_localValues.TryGetValue(local, out value);
            return value;
        }

        [SecurityCritical]
        internal static void SetLocalValue(IAsyncLocal local, object newValue, bool needChangeNotifications)
        {
            ExecutionContext current = t_currentMaybeNull ?? ExecutionContext.Default;

            object previousValue;
            bool hadPreviousValue = current.m_localValues.TryGetValue(local, out previousValue);

            if (previousValue == newValue)
                return;

            current = new ExecutionContext(current);
            current.m_localValues[local] = newValue;

            t_currentMaybeNull = current;

            if (needChangeNotifications)
            {
                if (hadPreviousValue)
                    Contract.Assert(current.m_localChangeNotifications.Contains(local));
                else
                    current.m_localChangeNotifications.Add(local);

                local.OnValueChanged(previousValue, newValue, false);
            }
        }

    #region Wrappers for CLR compat, to avoid ifdefs all over the BCL

        [Flags]
        internal enum CaptureOptions
        {
            None = 0x00,
            IgnoreSyncCtx = 0x01,
            OptimizeDefaultCase = 0x02,
        }

        [SecurityCritical]
        internal static ExecutionContext Capture(ref StackCrawlMark stackMark, CaptureOptions captureOptions)
        {
            return Capture();
        }

        [SecuritySafeCritical]
        [FriendAccessAllowed]
        internal static ExecutionContext FastCapture()
        {
            return Capture();
        }

        [SecurityCritical]
        [FriendAccessAllowed]
        internal static void Run(ExecutionContext executionContext, ContextCallback callback, Object state, bool preserveSyncCtx)
        {
            Run(executionContext, callback, state);
        }

        [SecurityCritical]
        internal bool IsDefaultFTContext(bool ignoreSyncCtx)
        {
            ExecutionContext current = t_currentMaybeNull;
            return current == null || current == Default;
        }

        [SecuritySafeCritical]
        public ExecutionContext CreateCopy()
        {
            return this; // since CoreCLR's ExecutionContext is immutable, we don't need to create copies.
        }

        public void Dispose()
        {
            // For CLR compat only
        }

        public static bool IsFlowSuppressed()
        {
            return false;
        }

        internal static ExecutionContext PreAllocatedDefault
        {
            [SecuritySafeCritical]
            get { return ExecutionContext.Default; }
        }

        internal bool IsPreAllocatedDefault
        {
            get { return this == ExecutionContext.Default; }
        }

    #endregion
    }

#else // FEATURE_CORECLR

    // Legacy desktop ExecutionContext implementation

    internal struct ExecutionContextSwitcher
    {
        internal ExecutionContext.Reader outerEC; // previous EC we need to restore on Undo
        internal bool outerECBelongsToScope;
#if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK        
        internal SecurityContextSwitcher scsw;
#endif // #if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK
        internal Object hecsw;
#if FEATURE_IMPERSONATION
        internal WindowsIdentity wi;
        internal bool cachedAlwaysFlowImpersonationPolicy;
        internal bool wiIsValid;
#endif
        internal Thread thread;

        [System.Security.SecurityCritical]  // auto-generated
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#if FEATURE_CORRUPTING_EXCEPTIONS
        [HandleProcessCorruptedStateExceptions]
#endif // FEATURE_CORRUPTING_EXCEPTIONS
        internal bool UndoNoThrow()
        {
            try
            {
                Undo();
            }
            catch
            {
                return false;
            }
            return true;
        }
        
        [System.Security.SecurityCritical]  // auto-generated
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal void Undo()
        {
            throw new NotImplementedException();
        }
    }


    public struct AsyncFlowControl: IDisposable
    {
        private bool useEC;
        private ExecutionContext _ec;
#if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK
        private SecurityContext _sc;
#endif // #if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK
        private Thread _thread;
#if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK
        [SecurityCritical]
        internal void Setup(SecurityContextDisableFlow flags)
        {
            useEC = false;
            Thread currentThread = Thread.CurrentThread;
            _sc = currentThread.GetMutableExecutionContext().SecurityContext;
            _sc._disableFlow = flags;
            _thread = currentThread;
        }
#endif // #if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK
        [SecurityCritical]
        internal void Setup()
        {
            throw new NotImplementedException();
        }
        
        public void Dispose()
        {
            Undo();
        }
        
        [SecuritySafeCritical]
        public void Undo()
        {
            throw new NotImplementedException();
        }
        
        public override int GetHashCode()
        {
            return _thread == null ? ToString().GetHashCode() : _thread.GetHashCode();
        }
        
        public override bool Equals(Object obj)
        {
            if (obj is AsyncFlowControl)
                return Equals((AsyncFlowControl)obj);
            else
                return false;
        }
    
        public bool Equals(AsyncFlowControl obj)
        {
            return obj.useEC == useEC && obj._ec == _ec &&
#if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK                            
                obj._sc == _sc && 
#endif // #if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK
                obj._thread == _thread;
        }
    
        public static bool operator ==(AsyncFlowControl a, AsyncFlowControl b)
        {
            return a.Equals(b);
        }
        
        public static bool operator !=(AsyncFlowControl a, AsyncFlowControl b)
        {
            return !(a == b);
        }
        
    }
    

    [Serializable] 
    public sealed class ExecutionContext : IDisposable
    {
        /*=========================================================================
        ** Data accessed from managed code that needs to be defined in 
        ** ExecutionContextObject  to maintain alignment between the two classes.
        ** DON'T CHANGE THESE UNLESS YOU MODIFY ExecutionContextObject in vm\object.h
        =========================================================================*/
#if FEATURE_CAS_POLICY        
        private HostExecutionContext _hostExecutionContext;
#endif // FEATURE_CAS_POLICY
        private SynchronizationContext _syncContext;
        private SynchronizationContext _syncContextNoFlow;
#if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK
        private SecurityContext     _securityContext;
#endif // #if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK
#if FEATURE_REMOTING
        [System.Security.SecurityCritical] // auto-generated
        private LogicalCallContext  _logicalCallContext;
        private IllogicalCallContext _illogicalCallContext;  // this call context follows the physical thread
#endif // #if FEATURE_REMOTING

        enum Flags
        {
            None = 0x0,
            IsNewCapture = 0x1,
            IsFlowSuppressed = 0x2,
            IsPreAllocatedDefault = 0x4
        }
        private Flags _flags;

        private Dictionary<IAsyncLocal, object> _localValues;
        private List<IAsyncLocal> _localChangeNotifications;

        internal bool isNewCapture 
        { 
            get
            { 
                return (_flags & (Flags.IsNewCapture | Flags.IsPreAllocatedDefault)) != Flags.None; 
            }
            set
            {
                Contract.Assert(!IsPreAllocatedDefault);
                if (value)
                    _flags |= Flags.IsNewCapture;
                else
                    _flags &= ~Flags.IsNewCapture;
            }
        }
        internal bool isFlowSuppressed 
        { 
            get 
            { 
                return (_flags & Flags.IsFlowSuppressed) != Flags.None; 
            }
            set
            {
                Contract.Assert(!IsPreAllocatedDefault);
                if (value)
                    _flags |= Flags.IsFlowSuppressed;
                else
                    _flags &= ~Flags.IsFlowSuppressed;
            }
        }
       

        private static readonly ExecutionContext s_dummyDefaultEC = new ExecutionContext(isPreAllocatedDefault: true);

        static internal ExecutionContext PreAllocatedDefault
        {
            [SecuritySafeCritical]
            get { return s_dummyDefaultEC; }
        }

        internal bool IsPreAllocatedDefault
        {
            get
            {
                // we use _flags instead of a direct comparison w/ s_dummyDefaultEC to avoid the static access on 
                // hot code paths.
                if ((_flags & Flags.IsPreAllocatedDefault) != Flags.None)
                {
                    Contract.Assert(this == s_dummyDefaultEC);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        internal ExecutionContext()
        {            
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        internal ExecutionContext(bool isPreAllocatedDefault)
        {
            if (isPreAllocatedDefault)
                _flags = Flags.IsPreAllocatedDefault;
        }

        // Read-only wrapper around ExecutionContext.  This enables safe reading of an ExecutionContext without accidentally modifying it.
        internal struct Reader
        {
            ExecutionContext m_ec;

            public Reader(ExecutionContext ec) { m_ec = ec; }

            public ExecutionContext DangerousGetRawExecutionContext() { return m_ec; }

            public bool IsNull { get { return m_ec == null; } }
            [SecurityCritical]
            public bool IsDefaultFTContext(bool ignoreSyncCtx) { return m_ec.IsDefaultFTContext(ignoreSyncCtx); }
            public bool IsFlowSuppressed 
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return IsNull ? false : m_ec.isFlowSuppressed; } 
            }
            //public Thread Thread { get { return m_ec._thread; } }
            public bool IsSame(ExecutionContext.Reader other) { return m_ec == other.m_ec; }

            public SynchronizationContext SynchronizationContext { get { return IsNull ? null : m_ec.SynchronizationContext; } }
            public SynchronizationContext SynchronizationContextNoFlow { get { return IsNull ? null : m_ec.SynchronizationContextNoFlow; } }

#if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK
            public SecurityContext.Reader SecurityContext 
            {
                [SecurityCritical]
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new SecurityContext.Reader(IsNull ? null : m_ec.SecurityContext); } 
            }
#endif

#if FEATURE_REMOTING
            public LogicalCallContext.Reader LogicalCallContext 
            {
                [SecurityCritical]
                get { return new LogicalCallContext.Reader(IsNull ? null : m_ec.LogicalCallContext); } 
            }

            public IllogicalCallContext.Reader IllogicalCallContext 
            {
                [SecurityCritical]
                get { return new IllogicalCallContext.Reader(IsNull ? null : m_ec.IllogicalCallContext); } 
            }
#endif

            [SecurityCritical]
            public object GetLocalValue(IAsyncLocal local)
            {
                if (IsNull)
                    return null;

                if (m_ec._localValues == null)
                    return null;

                object value;
                m_ec._localValues.TryGetValue(local, out value);
                return value;
            }

            [SecurityCritical]
            public bool HasSameLocalValues(ExecutionContext other)
            {
                var thisLocalValues = IsNull ? null : m_ec._localValues;
                var otherLocalValues = other == null ? null : other._localValues;
                return thisLocalValues == otherLocalValues;
            }

            [SecurityCritical]
            public bool HasLocalValues()
            {
                return !this.IsNull && m_ec._localValues != null;
            }
        }

        [SecurityCritical]
        internal static object GetLocalValue(IAsyncLocal local)
        {
            throw new NotImplementedException();
        }

        [SecurityCritical]
        internal static void SetLocalValue(IAsyncLocal local, object newValue, bool needChangeNotifications)
        {
            throw new NotImplementedException();
        }

        [SecurityCritical]
        internal static void OnAsyncLocalContextChanged(ExecutionContext previous, ExecutionContext current)
        {
            List<IAsyncLocal> previousLocalChangeNotifications = (previous == null) ? null : previous._localChangeNotifications;
            if (previousLocalChangeNotifications != null)
            {
                foreach (IAsyncLocal local in previousLocalChangeNotifications)
                {
                    object previousValue = null;
                    if (previous != null && previous._localValues != null)
                        previous._localValues.TryGetValue(local, out previousValue);

                    object currentValue = null;
                    if (current != null && current._localValues != null)
                        current._localValues.TryGetValue(local, out currentValue);

                    if (previousValue != currentValue)
                        local.OnValueChanged(previousValue, currentValue, true);
                }
            }

            List<IAsyncLocal> currentLocalChangeNotifications = (current == null) ? null : current._localChangeNotifications;
            if (currentLocalChangeNotifications != null && currentLocalChangeNotifications != previousLocalChangeNotifications)
            {
                try
                {
                    foreach (IAsyncLocal local in currentLocalChangeNotifications)
                    {
                        // If the local has a value in the previous context, we already fired the event for that local
                        // in the code above.
                        object previousValue = null;
                        if (previous == null ||
                            previous._localValues == null ||
                            !previous._localValues.TryGetValue(local, out previousValue))
                        {
                            object currentValue = null;
                            if (current != null && current._localValues != null)
                                current._localValues.TryGetValue(local, out currentValue);

                            if (previousValue != currentValue)
                                local.OnValueChanged(previousValue, currentValue, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Environment.FailFast(
                        Environment.GetResourceString("ExecutionContext_ExceptionInAsyncLocalNotification"),
                        ex);
                }
            }
        }


#if FEATURE_REMOTING
        internal LogicalCallContext LogicalCallContext
        {
            [System.Security.SecurityCritical]  // auto-generated
            get
            {
                if (_logicalCallContext == null)
                {
                _logicalCallContext = new LogicalCallContext();
                }
                return _logicalCallContext;
            }
            [System.Security.SecurityCritical]  // auto-generated
            set
            {
                Contract.Assert(this != s_dummyDefaultEC);
                _logicalCallContext = value;
            }
        }

        internal IllogicalCallContext IllogicalCallContext
        {
            get
            {
                if (_illogicalCallContext == null)
                {
                _illogicalCallContext = new IllogicalCallContext();
                }
                return _illogicalCallContext;
            }
            set
            {
                Contract.Assert(this != s_dummyDefaultEC);
                _illogicalCallContext = value;
            }
        }
#endif // #if FEATURE_REMOTING

        internal SynchronizationContext SynchronizationContext
        {
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            get
            {
                return _syncContext;
            }
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            set
            {
                Contract.Assert(this != s_dummyDefaultEC);
                _syncContext = value;
            }
        }

        internal SynchronizationContext SynchronizationContextNoFlow
        {
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            get
            {
                return _syncContextNoFlow;
            }
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            set
            {
                Contract.Assert(this != s_dummyDefaultEC);
                _syncContextNoFlow = value;
            }
        }

#if FEATURE_CAS_POLICY
    internal HostExecutionContext HostExecutionContext
    {
            get 
            {
                return _hostExecutionContext;
            }
            set 
            {
                Contract.Assert(this != s_dummyDefaultEC);
                _hostExecutionContext = value;
            }
    }
#endif // FEATURE_CAS_POLICY
#if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK
        internal  SecurityContext SecurityContext
        {
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            get
            {
                return _securityContext;
            }
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            set
            {
                Contract.Assert(this != s_dummyDefaultEC);
                        // store the new security context 
                        _securityContext = value;
                        // perform the reverse link too
                        if (value != null)
                            _securityContext.ExecutionContext = this;
            }
        }
#endif // #if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK


        public void Dispose()
        {
            if(this.IsPreAllocatedDefault)
                return; //Do nothing if this is the default context
#if FEATURE_CAS_POLICY
            if (_hostExecutionContext != null)
                _hostExecutionContext.Dispose();
#endif // FEATURE_CAS_POLICY
#if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK
            if (_securityContext != null)
                _securityContext.Dispose();
#endif //FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK
        }
        
        [DynamicSecurityMethod]
        [System.Security.SecurityCritical]  // auto-generated_required
        public static void Run(ExecutionContext executionContext, ContextCallback callback, Object state)
        {
            if (executionContext == null)
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NullContext"));
            if (!executionContext.isNewCapture)
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotNewCaptureContext"));
            
            Run(executionContext, callback, state, false);
        }

        // This method is special from a security perspective - the VM will not allow a stack walk to
        // continue past the call to ExecutionContext.Run.  If you change the signature to this method, make
        // sure to update SecurityStackWalk::IsSpecialRunFrame in the VM to search for the new signature.
        [DynamicSecurityMethod]
        [SecurityCritical]
        [FriendAccessAllowed]
        internal static void Run(ExecutionContext executionContext, ContextCallback callback, Object state, bool preserveSyncCtx)
        {
            RunInternal(executionContext, callback, state, preserveSyncCtx);
        }

        // Actual implementation of Run is here, in a non-DynamicSecurityMethod, because the JIT seems to refuse to inline callees into
        // a DynamicSecurityMethod.
        [SecurityCritical]
        [SuppressMessage("Microsoft.Concurrency", "CA8001", Justification = "Reviewed for thread safety")]
        internal static void RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, bool preserveSyncCtx)
        {
            throw new NotImplementedException();
        }

        [SecurityCritical]
        static internal void EstablishCopyOnWriteScope(ref ExecutionContextSwitcher ecsw)
        {
            EstablishCopyOnWriteScope(Thread.CurrentThread, false, ref ecsw);
        }

        [SecurityCritical]
        static private void EstablishCopyOnWriteScope(Thread currentThread, bool knownNullWindowsIdentity, ref ExecutionContextSwitcher ecsw)
        {
            throw new NotImplementedException();
        }

            
        // Sets the given execution context object on the thread.
        // Returns the previous one.
        [System.Security.SecurityCritical]  // auto-generated
        [DynamicSecurityMethodAttribute()]
        [MethodImplAttribute(MethodImplOptions.NoInlining)] // Methods containing StackCrawlMark local var has to be marked non-inlineable
#if FEATURE_CORRUPTING_EXCEPTIONS
        [HandleProcessCorruptedStateExceptions] 
#endif // FEATURE_CORRUPTING_EXCEPTIONS
        internal  static ExecutionContextSwitcher SetExecutionContext(ExecutionContext executionContext, bool preserveSyncCtx)
        {
#if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK                        
            StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
#endif // #if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK

            // Set up the switcher object to return;
            ExecutionContextSwitcher ecsw = new ExecutionContextSwitcher();
            return ecsw;    
        }

        //
        // Public CreateCopy.  Used to copy captured ExecutionContexts so they can be reused multiple times.
        // This should only copy the portion of the context that we actually capture.
        //
        [SecuritySafeCritical]
        public ExecutionContext CreateCopy()
        {
            if (!isNewCapture)
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotCopyUsedContext"));
            }
            ExecutionContext ec = new ExecutionContext();
            ec.isNewCapture = true;
            ec._syncContext = _syncContext == null ? null : _syncContext.CreateCopy();
            ec._localValues = _localValues;
            ec._localChangeNotifications = _localChangeNotifications;
#if FEATURE_CAS_POLICY
            // capture the host execution context
            ec._hostExecutionContext = _hostExecutionContext == null ? null : _hostExecutionContext.CreateCopy();
#endif // FEATURE_CAS_POLICY
#if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK
            if (_securityContext != null)
            {
                ec._securityContext = _securityContext.CreateCopy();
                ec._securityContext.ExecutionContext = ec;
            }
#endif // #if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK

#if FEATURE_REMOTING
            if (this._logicalCallContext != null)
                ec.LogicalCallContext = (LogicalCallContext)this.LogicalCallContext.Clone();

            Contract.Assert(this._illogicalCallContext == null);
#endif // #if FEATURE_REMOTING

            return ec;
        }

        //
        // Creates a complete copy, used for copy-on-write.
        //
        [SecuritySafeCritical]
        internal ExecutionContext CreateMutableCopy()
        {
            Contract.Assert(!this.isNewCapture);

            ExecutionContext ec = new ExecutionContext();

            // We don't deep-copy the SyncCtx, since we're still in the same context after copy-on-write.
            ec._syncContext = this._syncContext;
            ec._syncContextNoFlow = this._syncContextNoFlow;

#if FEATURE_CAS_POLICY
            // capture the host execution context
            ec._hostExecutionContext = this._hostExecutionContext == null ? null : _hostExecutionContext.CreateCopy();
#endif // FEATURE_CAS_POLICY

#if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK
            if (_securityContext != null)
            {
                ec._securityContext = this._securityContext.CreateMutableCopy();
                ec._securityContext.ExecutionContext = ec;
            }
#endif // #if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK

#if FEATURE_REMOTING
            if (this._logicalCallContext != null)
                ec.LogicalCallContext = (LogicalCallContext)this.LogicalCallContext.Clone();

            if (this._illogicalCallContext != null)
                ec.IllogicalCallContext = (IllogicalCallContext)this.IllogicalCallContext.CreateCopy();
#endif // #if FEATURE_REMOTING

            ec.isFlowSuppressed = this.isFlowSuppressed;

            return ec;
        }

        [System.Security.SecurityCritical]  // auto-generated_required
        public static AsyncFlowControl SuppressFlow()
        {
            if (IsFlowSuppressed())
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotSupressFlowMultipleTimes"));
            }
            Contract.EndContractBlock();
            AsyncFlowControl afc = new AsyncFlowControl();
            afc.Setup();
            return afc;
        }

        [SecuritySafeCritical]
        public static void RestoreFlow()
        {
            throw new NotImplementedException();
        }

        [Pure]
        public static bool IsFlowSuppressed()
        {
            throw new NotImplementedException();
        }

        [System.Security.SecuritySafeCritical]  // auto-generated
        [MethodImplAttribute(MethodImplOptions.NoInlining)] // Methods containing StackCrawlMark local var has to be marked non-inlineable
        public static ExecutionContext Capture()
        {
            // set up a stack mark for finding the caller
            StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
            return ExecutionContext.Capture(ref stackMark, CaptureOptions.None);            
        }

        //
        // Captures an ExecutionContext with optimization for the "default" case, and captures a "null" synchronization context.
        // When calling ExecutionContext.Run on the returned context, specify ignoreSyncCtx = true
        //
        [System.Security.SecuritySafeCritical]  // auto-generated
        [MethodImplAttribute(MethodImplOptions.NoInlining)] // Methods containing StackCrawlMark local var has to be marked non-inlineable
        [FriendAccessAllowed]
        internal static ExecutionContext FastCapture()
        {
            // set up a stack mark for finding the caller
            StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
            return ExecutionContext.Capture(ref stackMark, CaptureOptions.IgnoreSyncCtx | CaptureOptions.OptimizeDefaultCase);
        }


        [Flags]
        internal enum CaptureOptions
        {
            None = 0x00,

            IgnoreSyncCtx = 0x01,       //Don't flow SynchronizationContext

            OptimizeDefaultCase = 0x02, //Faster in the typical case, but can't show the result to users
                                        // because they could modify the shared default EC.
                                        // Use this only if you won't be exposing the captured EC to users.
        }

    // internal helper to capture the current execution context using a passed in stack mark
        [System.Security.SecurityCritical]  // auto-generated
        static internal ExecutionContext Capture(ref StackCrawlMark stackMark, CaptureOptions options)
        {
            throw new NotImplementedException();
        }

        [System.Security.SecurityCritical]  // auto-generated
        internal bool IsDefaultFTContext(bool ignoreSyncCtx)
        {
#if FEATURE_CAS_POLICY
            if (_hostExecutionContext != null)
                return false;
#endif // FEATURE_CAS_POLICY            
            if (!ignoreSyncCtx && _syncContext != null)
                return false;
#if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK            
            if (_securityContext != null && !_securityContext.IsDefaultFTSecurityContext())
                return false;
#endif //#if FEATURE_IMPERSONATION || FEATURE_COMPRESSEDSTACK
#if FEATURE_REMOTING
            if (_logicalCallContext != null && _logicalCallContext.HasInfo)
                return false;
            if (_illogicalCallContext != null && _illogicalCallContext.HasUserData)
                return false;
#endif //#if FEATURE_REMOTING
            return true;
        }
    } // class ExecutionContext

#endif //FEATURE_CORECLR
}


