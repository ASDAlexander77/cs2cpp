// Licensed under the MIT license. 

namespace System.Runtime.InteropServices
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    partial struct GCHandle
    {
        private static object syncObject = new object();

        private static Dictionary<int, KeyValuePair<object, GCHandleType>> handlers;

        // Internal native calls that this implementation uses.
        internal static IntPtr InternalAlloc(Object value, GCHandleType type)
        {
            lock (syncObject)
            {
                if (handlers == null)
                {
                    handlers = new Dictionary<int, KeyValuePair<object, GCHandleType>>();
                }

                var index = handlers.Count;
                handlers.Add(index, new KeyValuePair<object, GCHandleType>(value, type));
                return new IntPtr(index);
            }
        }

        internal static void InternalFree(IntPtr handle)
        {
            lock (syncObject)
            {
                if (handlers == null)
                {
                    throw new NullReferenceException();
                }

                handlers.Remove(handle.ToInt32());
            }
        }

        internal static Object InternalGet(IntPtr handle)
        {
            lock (syncObject)
            {
                if (handlers == null)
                {
                    throw new NullReferenceException();
                }

                return handlers[handle.ToInt32()].Key;
            }
        }

        internal static void InternalSet(IntPtr handle, Object value, bool isPinned)
        {
            lock (syncObject)
            {
                if (handlers == null)
                {
                    throw new NullReferenceException();
                }

                handlers[handle.ToInt32()] = new KeyValuePair<object, GCHandleType>(value, isPinned ? GCHandleType.Pinned : default(GCHandleType));
            }
        }

        internal static Object InternalCompareExchange(IntPtr handle, Object value, Object oldValue, bool isPinned)
        {
            lock (syncObject)
            {
                var original = handlers[handle.ToInt32()].Value;
                if (Object.ReferenceEquals(original, value) || (oldValue != null && original.Equals(oldValue)))
                {
                    handlers[handle.ToInt32()] = new KeyValuePair<object, GCHandleType>(value, isPinned ? GCHandleType.Pinned : default(GCHandleType));
                }

                return original;
            }
        }

        internal static IntPtr InternalAddrOfPinnedObject(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        internal static void InternalCheckDomain(IntPtr handle)
        {
        }

        internal static GCHandleType InternalGetHandleType(IntPtr handle)
        {
            lock (syncObject)
            {
                if (handlers == null)
                {
                    throw new NullReferenceException();
                }

                return handlers[handle.ToInt32()].Value;
            }
        }
    }
}
