// Licensed under the MIT license.

namespace System.Runtime.CompilerServices
{
    using System;
    using Collections;
    using Collections.Generic;

    partial struct DependentHandle
    {
        private static object syncObject = new object();

        private static Dictionary<int, KeyValuePair<object, object>> handlers;

        private static void nInitialize(Object primary, Object secondary, out IntPtr dependentHandle)
        {
            lock (syncObject)
            {
                if (handlers == null)
                {
                    handlers = new Dictionary<int, KeyValuePair<object, object>>();
                }

                var index = handlers.Count;
                handlers.Add(index, new KeyValuePair<object, object>(primary, secondary));
                dependentHandle = new IntPtr(index);
            }
        }

        private static void nGetPrimary(IntPtr dependentHandle, out Object primary)
        {
            lock (syncObject)
            {
                if (handlers == null)
                {
                    throw new NullReferenceException();
                }

                primary = handlers[dependentHandle.ToInt32()].Key;
            }
        }

        private static void nGetPrimaryAndSecondary(IntPtr dependentHandle, out Object primary, out Object secondary)
        {
            lock (syncObject)
            {
                if (handlers == null)
                {
                    throw new NullReferenceException();
                }

                primary = handlers[dependentHandle.ToInt32()].Key;
                secondary = handlers[dependentHandle.ToInt32()].Value;
            }
        }

        private static void nFree(IntPtr dependentHandle)
        {
            lock (syncObject)
            {
                if (handlers == null)
                {
                    throw new NullReferenceException();
                }

                handlers.Remove(dependentHandle.ToInt32());
            }
        }
    }
}