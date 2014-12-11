////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////namespace System
namespace System
{

    using System;
    using System.Reflection;
    using System.Threading;
    using System.Runtime.CompilerServices;
    [Serializable()]
    public abstract class Delegate
    {
        private Object _target;
        private IntPtr _methodPtr;

        public override bool Equals(Object obj)
        {
            throw new NotImplementedException();
        }

        public static Delegate Combine(Delegate a, Delegate b)
        {
            throw new NotImplementedException();
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

        public static Delegate Remove(Delegate source, Delegate value)
        {
            throw new NotImplementedException();
        }


        public static bool operator ==(Delegate d1, Delegate d2)
        {
            throw new NotImplementedException();
        }


        public static bool operator !=(Delegate d1, Delegate d2)
        {
            throw new NotImplementedException();
        }

        public unsafe void* ToPointer()
        {
            return _methodPtr.ToPointer();
        }
    }
}


