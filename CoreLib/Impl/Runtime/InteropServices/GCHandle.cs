// Licensed under the MIT license. 

namespace System.Runtime.InteropServices
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [MergeCode]
    partial struct GCHandle
    {
        [MergeCode]
        private static object syncObject = new object();

        [MergeCode]
        private static Dictionary<int, KeyValuePair<object, GCHandleType>> handlers;

        // Internal native calls that this implementation uses.
        [MergeCode]
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

        [MergeCode]
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

        [MergeCode]
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

        [MergeCode]
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

        [MergeCode]
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

        [MergeCode]
        internal static IntPtr InternalAddrOfPinnedObject(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        internal static void InternalCheckDomain(IntPtr handle)
        {
        }

        [MergeCode]
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
