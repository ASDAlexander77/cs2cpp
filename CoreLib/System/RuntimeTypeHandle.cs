////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////namespace System
namespace System
{

    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /**
     *  This value type is used for making Type.GetTypeFromHandle() type safe.
     *
     *  SECURITY : m_ptr cannot be set to anything other than null by untrusted
     *  code.
     *
     *  This corresponds to EE TypeHandle.
     */
    [Serializable()]
    public struct RuntimeTypeHandle
    {
        private RuntimeType m_type;

        internal RuntimeTypeHandle(RuntimeType type)
        {
            m_type = type;
        }

        internal RuntimeType GetRuntimeType()
        {
            return m_type;
        }
    }
}


