#include "System.Private.CoreLib.h"

namespace CoreLib { namespace Microsoft { namespace Win32 { 
    namespace _ = ::CoreLib::Microsoft::Win32;
    // Method : Microsoft.Win32.UnsafeNativeMethods.ManifestEtw.EventRegister(ref System.Guid, Microsoft.Win32.UnsafeNativeMethods.ManifestEtw.EtwEnableCallback, void*, ref long)
    uint32_t UnsafeNativeMethods_ManifestEtw::EventRegister_Ref_Ref(::CoreLib::System::Guid& providerId, _::UnsafeNativeMethods_ManifestEtw_EtwEnableCallback* enableCallback, void* callbackContext, int64_t& registrationHandle)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.UnsafeNativeMethods.ManifestEtw.EventUnregister(long)
    uint32_t UnsafeNativeMethods_ManifestEtw::EventUnregister(int64_t registrationHandle)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.UnsafeNativeMethods.ManifestEtw.EventWriteString(long, byte, long, string)
    int32_t UnsafeNativeMethods_ManifestEtw::EventWriteString(int64_t registrationHandle, uint8_t level, int64_t keyword, string* msg)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.UnsafeNativeMethods.ManifestEtw.EventWriteTransfer(long, ref System.Diagnostics.Tracing.EventDescriptor, System.Guid*, System.Guid*, int, System.Diagnostics.Tracing.EventProvider.EventData*)
    int32_t UnsafeNativeMethods_ManifestEtw::EventWriteTransfer_Ref(int64_t registrationHandle, ::CoreLib::System::Diagnostics::Tracing::EventDescriptor& eventDescriptor, ::CoreLib::System::Guid* activityId, ::CoreLib::System::Guid* relatedActivityId, int32_t userDataCount, ::CoreLib::System::Diagnostics::Tracing::EventProvider_EventData* userData)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.UnsafeNativeMethods.ManifestEtw.EventActivityIdControl(Microsoft.Win32.UnsafeNativeMethods.ManifestEtw.ActivityControl, ref System.Guid)
    int32_t UnsafeNativeMethods_ManifestEtw::EventActivityIdControl_Ref(_::UnsafeNativeMethods_ManifestEtw_ActivityControl__enum ControlCode, ::CoreLib::System::Guid& ActivityId)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.UnsafeNativeMethods.ManifestEtw.EventSetInformation(long, Microsoft.Win32.UnsafeNativeMethods.ManifestEtw.EVENT_INFO_CLASS, void*, int)
    int32_t UnsafeNativeMethods_ManifestEtw::EventSetInformation(int64_t registrationHandle, _::UnsafeNativeMethods_ManifestEtw_EVENT_INFO_CLASS__enum informationClass, void* eventInformation, int32_t informationLength)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.UnsafeNativeMethods.ManifestEtw.EnumerateTraceGuidsEx(Microsoft.Win32.UnsafeNativeMethods.ManifestEtw.TRACE_QUERY_INFO_CLASS, void*, int, void*, int, ref int)
    int32_t UnsafeNativeMethods_ManifestEtw::EnumerateTraceGuidsEx_Ref(_::UnsafeNativeMethods_ManifestEtw_TRACE_QUERY_INFO_CLASS__enum TraceQueryInfoClass, void* InBuffer, int32_t InBufferSize, void* OutBuffer, int32_t OutBufferSize, int32_t& ReturnLength)
    {
        throw 3221274624U;
    }

}}}

namespace CoreLib { namespace Microsoft { namespace Win32 { 
    namespace _ = ::CoreLib::Microsoft::Win32;
}}}
