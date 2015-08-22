////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////namespace System
namespace System
{
    using System.Reflection;

    [Serializable()]
    public abstract class Delegate
    {
        internal Object _target;
        internal IntPtr _methodPtr;

        public override bool Equals(Object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            Delegate d = (Delegate)obj;

            if (_target == d._target && _methodPtr == d._methodPtr)
                return true;

            return false;
        }

        public MethodInfo Method
        {

            get
            {
                throw new NotImplementedException();
            }
        }

        public Object Target
        {

            get
            {
                return this._target;
            }
        }

        public static Delegate Combine(Delegate a, Delegate b)
        {
            if ((Object)a == null)
                return b;

            return a.CombineImpl(b);
        }

        public static Delegate Remove(Delegate source, Delegate value)
        {
            if (source == null)
                return null;

            if (value == null)
                return source;

            if (source.GetType() != value.GetType())
                throw new ArgumentException("DlgtTypeMis");

            return source.RemoveImpl(value);
        }


        public static bool operator ==(Delegate d1, Delegate d2)
        {
            if ((Object)d1 == null)
                return (Object)d2 == null;

            return d1.Equals(d2);
        }


        public static bool operator !=(Delegate d1, Delegate d2)
        {
            if ((Object)d1 == null)
                return (Object)d2 != null;

            return !d1.Equals(d2);
        }

        public unsafe void* ToPointer()
        {
            return _methodPtr.ToPointer();
        }

        protected virtual Delegate CombineImpl(Delegate d)
        {
            throw new NotSupportedException("Combine");
        }

        protected virtual Delegate RemoveImpl(Delegate d)
        {
            return (d.Equals(this)) ? null : this;
        }
    }

    // These flags effect the way BindToMethodInfo and BindToMethodName are allowed to bind a delegate to a target method. Their
    // values must be kept in sync with the definition in vm\comdelegate.h.
    internal enum DelegateBindingFlags
    {
        StaticMethodOnly = 0x00000001, // Can only bind to static target methods
        InstanceMethodOnly = 0x00000002, // Can only bind to instance (including virtual) methods
        OpenDelegateOnly = 0x00000004, // Only allow the creation of delegates open over the 1st argument
        ClosedDelegateOnly = 0x00000008, // Only allow the creation of delegates closed over the 1st argument
        NeverCloseOverNull = 0x00000010, // A null target will never been considered as a possible null 1st argument
        CaselessMatching = 0x00000020, // Use case insensitive lookup for methods matched by name
        SkipSecurityChecks = 0x00000040, // Skip security checks (visibility, link demand etc.)
        RelaxedSignature = 0x00000080, // Allow relaxed signature matching (co/contra variance)
    }
}


