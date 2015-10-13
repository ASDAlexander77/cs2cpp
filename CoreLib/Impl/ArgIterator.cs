// Licensed under the MIT license.

namespace System
{
    using System;

    [MergeCode]
    partial struct ArgIterator
    {
        [MergeCode]
        private ArgIterator(IntPtr arglist)
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        private unsafe ArgIterator(IntPtr arglist, void* ptr)
        {
            throw new NotImplementedException();
        }

        // reference to TypedReference is banned, so have to pass result as void pointer
        [MergeCode]
        private unsafe void FCallGetNextArg(void* result)
        {
            throw new NotImplementedException();
        }

        // reference to TypedReference is banned, so have to pass result as void pointer
        [MergeCode]
        private unsafe void InternalGetNextArg(void* result, RuntimeType rt)
        {
            throw new NotImplementedException();
        }

        // How many arguments are left in the list 
        [MergeCode]
        public int GetRemainingCount()
        {
            throw new NotImplementedException();
        }

        // Gets the type of the current arg, does NOT advance the iterator
        [MergeCode]
        private unsafe void* _GetNextArgType()
        {
            throw new NotImplementedException();
        }
    }
}