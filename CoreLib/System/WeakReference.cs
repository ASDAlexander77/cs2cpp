////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System
{

    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;
    [Serializable()]
    public class WeakReference
    {
        internal IntPtr m_handle;
        
        public WeakReference(Object target)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsAlive
        {
            
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual Object Target
        {
            
            get
            {
                throw new NotImplementedException();
            }
            
            set
            {
                throw new NotImplementedException();
            }
        }

    }
}


