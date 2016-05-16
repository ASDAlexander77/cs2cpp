#ifndef HEADER_System_Runtime_CompilerServices_JitHelpers_STUBS
#define HEADER_System_Runtime_CompilerServices_JitHelpers_STUBS
namespace CoreLib { namespace System { namespace Runtime { namespace CompilerServices { 
    
    // Method : System.Runtime.CompilerServices.JitHelpers.UnsafeCast<T>(object)
    template <typename T> 
    T JitHelpers::UnsafeCast(object* o)
    {
        return reinterpret_cast<T>(o);
    }
    
    // Method : System.Runtime.CompilerServices.JitHelpers.UnsafeEnumCast<T>(T)
    template <typename T> 
    int32_t JitHelpers::UnsafeEnumCast(T val)
    {
        return reinterpret_cast<int32_t>(val);
    }
    
    // Method : System.Runtime.CompilerServices.JitHelpers.UnsafeEnumCastLong<T>(T)
    template <typename T> 
    int64_t JitHelpers::UnsafeEnumCastLong(T val)
    {
        return reinterpret_cast<int64_t>(val);
    }
    
    // Method : System.Runtime.CompilerServices.JitHelpers.UnsafeCastToStackPointer<T>(ref T)
    template <typename T> 
    CoreLib::System::IntPtr JitHelpers::UnsafeCastToStackPointer_Ref(T& val)
    {
        return __init<CoreLib::System::IntPtr>((void*)&val);
    }
}}}}
#endif