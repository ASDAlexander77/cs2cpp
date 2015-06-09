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

        // Returns handle for interop with EE. The handle is guaranteed to be non-null.
        internal RuntimeTypeHandle GetNativeHandle()
        {
            // Create local copy to avoid a race condition
            RuntimeType type = m_type;
            if (type == null)
            {
                throw new ArgumentNullException(null, Environment.GetResourceString("Arg_InvalidHandle"));
            }

            return new RuntimeTypeHandle(type);
        }

        private static void ConstructName(RuntimeTypeHandle handle, TypeNameFormatFlags formatFlags, StringHandleOnStack retString)
        {
            throw new NotImplementedException();
        }

        internal string ConstructName(TypeNameFormatFlags formatFlags)
        {
            string name = null;
            ConstructName(GetNativeHandle(), formatFlags, JitHelpers.GetStringHandleOnStack(ref name));
            return name;
        }

        internal static bool HasElementType(RuntimeType type)
        {
            CorElementType corElemType = GetCorElementType(type);

            return ((corElemType == CorElementType.Array || corElemType == CorElementType.SzArray) // IsArray
                   || (corElemType == CorElementType.Ptr)                                          // IsPointer
                   || (corElemType == CorElementType.ByRef));                                      // IsByRef
        }

        internal static CorElementType GetCorElementType(RuntimeType type)
        {
            throw new NotImplementedException();
        }
    }
}


