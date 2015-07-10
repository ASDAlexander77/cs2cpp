// Licensed under the MIT license.

namespace System.Runtime.CompilerServices
{
    using System;

    partial struct DependentHandle
    {
        private static void nInitialize(Object primary, Object secondary, out IntPtr dependentHandle)
        {
            throw new NotImplementedException();
        }

        private static void nGetPrimary(IntPtr dependentHandle, out Object primary)
        {
            throw new NotImplementedException();
        }

        private static void nGetPrimaryAndSecondary(IntPtr dependentHandle, out Object primary, out Object secondary)
        {
            throw new NotImplementedException();
        }

        private static void nFree(IntPtr dependentHandle)
        {
            throw new NotImplementedException();
        }
    }
}