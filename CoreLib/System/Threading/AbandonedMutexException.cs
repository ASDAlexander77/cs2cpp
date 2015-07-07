// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//
//
// AbandonedMutexException
// Thrown when a wait completes because one or more mutexes was abandoned.
// AbandonedMutexs indicate serious error in user code or machine state.
////////////////////////////////////////////////////////////////////////////////

namespace System.Threading
{

    using System;
    using System.Threading;
    using System.Runtime.InteropServices;

    [Serializable]
    [ComVisibleAttribute(false)]
    public class AbandonedMutexException : SystemException
    {

        private int m_MutexIndex = -1;
        private Mutex m_Mutex = null;

        public AbandonedMutexException()
            : base(Environment.GetResourceString("Threading.AbandonedMutexException"))
        {
        }

        public AbandonedMutexException(String message)
            : base(message)
        {
        }

        public AbandonedMutexException(String message, Exception inner)
            : base(message, inner)
        {
        }

        public AbandonedMutexException(int location, WaitHandle handle)
            : base(Environment.GetResourceString("Threading.AbandonedMutexException"))
        {
            SetupException(location, handle);
        }

        public AbandonedMutexException(String message, int location, WaitHandle handle)
            : base(message)
        {
            SetupException(location, handle);
        }

        public AbandonedMutexException(String message, Exception inner, int location, WaitHandle handle)
            : base(message, inner)
        {
            SetupException(location, handle);
        }

        private void SetupException(int location, WaitHandle handle)
        {
            m_MutexIndex = location;
            if (handle != null)
                m_Mutex = handle as Mutex;
        }

        public Mutex Mutex
        {
            get
            {
                return m_Mutex;
            }
        }

        public int MutexIndex
        {
            get
            {
                return m_MutexIndex;
            }
        }

    }
}

