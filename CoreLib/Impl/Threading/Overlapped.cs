namespace System.Threading
{
    partial class OverlappedData
    {
        unsafe private NativeOverlapped* AllocateNativeOverlapped()
        {
            throw new NotImplementedException();
        }

        unsafe internal static void FreeNativeOverlapped(NativeOverlapped* nativeOverlappedPtr)
        {
            throw new NotImplementedException();
        }

        unsafe internal static OverlappedData GetOverlappedFromNative(NativeOverlapped* nativeOverlappedPtr)
        {
            throw new NotImplementedException();
        }

        unsafe internal static void CheckVMForIOPacket(out NativeOverlapped* pOVERLAP, out uint errorCode, out uint numBytes)
        {
            throw new NotImplementedException();
        }
    }
}
