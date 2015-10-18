// Licensed under the MIT license.

namespace System
{
    using System;

    partial struct ArgIterator
    {
        private ArgIterator(IntPtr arglist)
        {
            throw new NotImplementedException();
        }

        private unsafe ArgIterator(IntPtr arglist, void* ptr)
        {
            throw new NotImplementedException();
        }

        // reference to TypedReference is banned, so have to pass result as void pointer
        private unsafe void FCallGetNextArg(void* result)
        {
            throw new NotImplementedException();
        }

        // reference to TypedReference is banned, so have to pass result as void pointer
        private unsafe void InternalGetNextArg(void* result, RuntimeType rt)
        {
            throw new NotImplementedException();
        }

        // How many arguments are left in the list 
        public int GetRemainingCount()
        {
            throw new NotImplementedException();
        }

        // Gets the type of the current arg, does NOT advance the iterator
        private unsafe void* _GetNextArgType()
        {
            throw new NotImplementedException();
        }
    }
}