// Licensed under the MIT license.

namespace System.Threading
{
    [MergeCode]
    partial class OverlappedData
    {
        [MergeCode]
        unsafe private NativeOverlapped* AllocateNativeOverlapped()
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        unsafe internal static void FreeNativeOverlapped(NativeOverlapped* nativeOverlappedPtr)
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        unsafe internal static OverlappedData GetOverlappedFromNative(NativeOverlapped* nativeOverlappedPtr)
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        unsafe internal static void CheckVMForIOPacket(out NativeOverlapped* pOVERLAP, out uint errorCode, out uint numBytes)
        {
            throw new NotImplementedException();
        }
    }
}
