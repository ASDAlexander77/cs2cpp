// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

////////////////////////////////////////////////////////////////////////////////
// JitHelpers
//    Low-level Jit Helpers
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Threading;
using System.Runtime;
using System.Runtime.Versioning;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices {

    // Wrapper for address of a string variable on stack
    internal struct StringHandleOnStack
    {
        private IntPtr m_ptr;

        internal StringHandleOnStack(IntPtr pString)
        {
            m_ptr = pString;
        }
    }

    // Wrapper for address of a object variable on stack
    internal struct ObjectHandleOnStack
    {
        private IntPtr m_ptr;

        internal ObjectHandleOnStack(IntPtr pObject)
        {
            m_ptr = pObject;
        }
    }

    // Wrapper for StackCrawlMark
    internal struct StackCrawlMarkHandle
    {
        private IntPtr m_ptr;

        internal StackCrawlMarkHandle(IntPtr stackMark)
        {
            m_ptr = stackMark;
        }
    }

    // Helper class to assist with unsafe pinning of arbitrary objects. The typical usage pattern is:
    // fixed (byte * pData = &JitHelpers.GetPinningHelper(value).m_data)
    // {
    //    ... pData is what Object::GetData() returns in VM ...
    // }
    internal class PinningHelper
    {
        public byte m_data;
    }

    internal static class JitHelpers
    {
        // The special dll name to be used for DllImport of QCalls
        internal const string QCall = "QCall";

        // Wraps object variable into a handle. Used to return managed strings from QCalls.
        // s has to be a local variable on the stack.
        static internal StringHandleOnStack GetStringHandleOnStack(ref string s)
        {
            return new StringHandleOnStack(UnsafeCastToStackPointer(ref s));
        }

        // Wraps object variable into a handle. Used to pass managed object references in and out of QCalls.
        // o has to be a local variable on the stack.
        static internal ObjectHandleOnStack GetObjectHandleOnStack<T>(ref T o) where T : class
        {
            return new ObjectHandleOnStack(UnsafeCastToStackPointer(ref o));
        }

        // The IL body of this method is not critical, but its body will be replaced with unsafe code, so
        // this method is effectively critical
        static internal T UnsafeCast<T>(Object o) where T : class
        {
            // The body of this function will be replaced by the EE with unsafe code that just returns o!!!
            // See getILIntrinsicImplementation for how this happens.  
            throw new InvalidOperationException();
        }

        static internal int UnsafeEnumCast<T>(T val) where T : struct		// Actually T must be 4 byte (or less) enum
        {
            // should be return (int) val; but C# does not allow, runtime does this magically
            // See getILIntrinsicImplementation for how this happens.  
            throw new InvalidOperationException();
        }

        static internal long UnsafeEnumCastLong<T>(T val) where T : struct	// Actually T must be 8 byte enum
        {
            // should be return (long) val; but C# does not allow, runtime does this magically
            // See getILIntrinsicImplementation for how this happens.  
            throw new InvalidOperationException();
        }

        static internal IntPtr UnsafeCastToStackPointer<T>(ref T val)
        {
            // The body of this function will be replaced by the EE with unsafe code that just returns o!!!
            // See getILIntrinsicImplementation for how this happens.  
            throw new InvalidOperationException();
        }

        // Set the given element in the array without any type or range checks
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern static internal void UnsafeSetArrayElement(Object[] target, int index, Object element);

        // Used for unsafe pinning of arbitrary objects.
        static internal PinningHelper GetPinningHelper(Object o)
        {
            // This cast is really unsafe - call the private version that does not assert in debug
            return UnsafeCast<PinningHelper>(o);
        }
    }
}
