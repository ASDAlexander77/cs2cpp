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
}


