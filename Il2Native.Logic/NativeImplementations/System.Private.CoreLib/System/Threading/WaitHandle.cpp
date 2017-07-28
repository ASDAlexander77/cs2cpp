#include "System.Private.CoreLib.h"

namespace CoreLib { namespace System { namespace Threading { 
    namespace _ = ::CoreLib::System::Threading;
    // Method : System.Threading.WaitHandle.WaitOneNative(System.Runtime.InteropServices.SafeHandle, uint, bool, bool)
    int32_t WaitHandle::WaitOneNative(::CoreLib::System::Runtime::InteropServices::SafeHandle* waitableSafeHandle, uint32_t millisecondsTimeout, bool hasThreadAffinity, bool exitContext)
    {
#ifndef GC_PTHREADS
		return ::WaitForSingleObject((HANDLE)waitableSafeHandle->handle.ToInt32(), millisecondsTimeout);
#else
#error NOT IMPLEMENTED YET
#endif
	}
    
    // Method : System.Threading.WaitHandle.WaitMultiple(System.Threading.WaitHandle[], int, bool, bool)
    int32_t WaitHandle::WaitMultiple(__array<_::WaitHandle*>* waitHandles, int32_t millisecondsTimeout, bool exitContext, bool WaitAll)
    {
        throw 3221274624U;
    }
    
    // Method : System.Threading.WaitHandle.SignalAndWaitOne(Microsoft.Win32.SafeHandles.SafeWaitHandle, Microsoft.Win32.SafeHandles.SafeWaitHandle, int, bool, bool)
    int32_t WaitHandle::SignalAndWaitOne(::CoreLib::Microsoft::Win32::SafeHandles::SafeWaitHandle* waitHandleToSignal, ::CoreLib::Microsoft::Win32::SafeHandles::SafeWaitHandle* waitHandleToWaitOn, int32_t millisecondsTimeout, bool hasThreadAffinity, bool exitContext)
    {
        throw 3221274624U;
    }

}}}

namespace CoreLib { namespace System { namespace Threading { 
    namespace _ = ::CoreLib::System::Threading;
}}}
