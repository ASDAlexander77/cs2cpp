// Licensed under the MIT license. 

namespace System.Runtime.InteropServices
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    partial struct GCHandle
    {
        // Internal native calls that this implementation uses.
        internal static IntPtr InternalAlloc(Object value, GCHandleType type)
        {
            var data = new KeyValuePair<object, GCHandleType>(value, type);
            var typedRef = __makeref(data);
            unsafe
            {
                return new IntPtr(&typedRef);
            }
        }

        internal static void InternalFree(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        internal static Object InternalGet(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        internal static void InternalSet(IntPtr handle, Object value, bool isPinned)
        {
            throw new NotImplementedException();
        }

        internal static Object InternalCompareExchange(IntPtr handle, Object value, Object oldValue, bool isPinned)
        {
            throw new NotImplementedException();
        }

        internal static IntPtr InternalAddrOfPinnedObject(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        internal static void InternalCheckDomain(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        internal static GCHandleType InternalGetHandleType(IntPtr handle)
        {
            throw new NotImplementedException();
        }
    }
}
