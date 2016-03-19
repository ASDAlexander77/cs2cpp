#include "CoreLib.h"
#if _WIN32 && _MSC_VER
#include <xatomic.h>
#endif

// Method : System.Threading.Interlocked.Increment(ref int)
int32_t CoreLib::System::Threading::Interlocked::Increment_Ref(int32_t& location)
{
#if _WIN32 && _MSC_VER
	return std::_Atomic_fetch_add_4((volatile std::_Uint4_t*)&location, 1, std::memory_order_relaxed);
#else
	#  error No implementation
#endif
}

// Method : System.Threading.Interlocked.Increment(ref long)
int32_t CoreLib::System::Threading::Interlocked::Increment_Ref(int64_t& location)
{
#if _WIN32 && _MSC_VER
	return std::_Atomic_fetch_add_8((volatile std::_Uint8_t*)&location, 1, std::memory_order_relaxed);
#else
	#  error No implementation
#endif
}

// Method : System.Threading.Interlocked.Decrement(ref int)
int32_t CoreLib::System::Threading::Interlocked::Decrement_Ref(int32_t& location)
{
#if _WIN32 && _MSC_VER
	return std::_Atomic_fetch_sub_4((volatile std::_Uint4_t*)&location, 1, std::memory_order_relaxed);
#else
	#  error No implementation
#endif
}

// Method : System.Threading.Interlocked.Decrement(ref long)
int64_t CoreLib::System::Threading::Interlocked::Decrement_Ref(int64_t& location)
{
#if _WIN32 && _MSC_VER
	return std::_Atomic_fetch_sub_8((volatile std::_Uint8_t*)&location, 1, std::memory_order_relaxed);
#else
	#  error No implementation
#endif
}

// Method : System.Threading.Interlocked.Exchange(ref int, int)
int32_t CoreLib::System::Threading::Interlocked::Exchange_Ref(int32_t& location1, int32_t value)
{
    throw 3221274624U;
}

// Method : System.Threading.Interlocked.Exchange(ref long, long)
int64_t CoreLib::System::Threading::Interlocked::Exchange_Ref(int64_t& location1, int64_t value)
{
    throw 3221274624U;
}

// Method : System.Threading.Interlocked.Exchange(ref object, object)
object* CoreLib::System::Threading::Interlocked::Exchange_Ref(object*& location1, object* value)
{
    throw 3221274624U;
}

// Method : System.Threading.Interlocked.Exchange(ref System.IntPtr, System.IntPtr)
int32_t CoreLib::System::Threading::Interlocked::Exchange_Ref(CoreLib::System::IntPtr& location1, CoreLib::System::IntPtr value)
{
    throw 3221274624U;
}

// Method : System.Threading.Interlocked.CompareExchange(ref int, int, int)
int32_t CoreLib::System::Threading::Interlocked::CompareExchange_Ref(int32_t& location1, int32_t value, int32_t comparand)
{
    throw 3221274624U;
}

// Method : System.Threading.Interlocked.CompareExchange(ref long, long, long)
int64_t CoreLib::System::Threading::Interlocked::CompareExchange_Ref(int64_t& location1, int64_t value, int64_t comparand)
{
    throw 3221274624U;
}

// Method : System.Threading.Interlocked.CompareExchange(ref object, object, object)
object* CoreLib::System::Threading::Interlocked::CompareExchange_Ref(object*& location1, object* value, object* comparand)
{
    throw 3221274624U;
}

// Method : System.Threading.Interlocked.CompareExchange(ref System.IntPtr, System.IntPtr, System.IntPtr)
CoreLib::System::IntPtr CoreLib::System::Threading::Interlocked::CompareExchange_Ref(CoreLib::System::IntPtr& location1, CoreLib::System::IntPtr value, CoreLib::System::IntPtr comparand)
{
    throw 3221274624U;
}

// Method : System.Threading.Interlocked.CompareExchange(ref int, int, int, ref bool)
int32_t CoreLib::System::Threading::Interlocked::CompareExchange_Ref_Ref(int32_t& location1, int32_t value, int32_t comparand, bool& succeeded)
{
    throw 3221274624U;
}

// Method : System.Threading.Interlocked.ExchangeAdd(ref int, int)
int32_t CoreLib::System::Threading::Interlocked::ExchangeAdd_Ref(int32_t& location1, int32_t value)
{
    throw 3221274624U;
}

// Method : System.Threading.Interlocked.ExchangeAdd(ref long, long)
int64_t CoreLib::System::Threading::Interlocked::ExchangeAdd_Ref(int64_t& location1, int64_t value)
{
    throw 3221274624U;
}
