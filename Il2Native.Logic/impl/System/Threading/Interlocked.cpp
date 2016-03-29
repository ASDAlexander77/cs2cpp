#include "CoreLib.h"
#ifdef _MSC_VER
#include <intrin.h>
#endif // _MSC_VER

// Method : System.Threading.Interlocked.Increment(ref int)
inline int32_t CoreLib::System::Threading::Interlocked::Increment_Ref(int32_t& location)
{
#ifdef _MSC_VER
    return _InterlockedIncrement((int32_t volatile*)location);
#else
    return __sync_add_and_fetch((int32_t volatile*)location, 1);
#endif
}

// Method : System.Threading.Interlocked.Increment(ref long)
inline int32_t CoreLib::System::Threading::Interlocked::Increment_Ref(int64_t& location)
{
#ifdef _MSC_VER
    return _InterlockedIncrement((int64_t volatile*)location);
#else
    return __sync_add_and_fetch((int64_t volatile*)location, 1);
#endif
}

// Method : System.Threading.Interlocked.Decrement(ref int)
inline int32_t CoreLib::System::Threading::Interlocked::Decrement_Ref(int32_t& location)
{
#ifdef _MSC_VER
    return _InterlockedDecrement((int32_t volatile*)location);
#else
    return __sync_sub_and_fetch((int32_t volatile*)location, 1);
#endif
}

// Method : System.Threading.Interlocked.Decrement(ref long)
inline int64_t CoreLib::System::Threading::Interlocked::Decrement_Ref(int64_t& location)
{
#ifdef _MSC_VER
    return _InterlockedDecrement((int64_t volatile*)location);
#else
    return __sync_sub_and_fetch((int64_t volatile*)location, 1);
#endif
}

// Method : System.Threading.Interlocked.Exchange(ref int, int)
inline int32_t CoreLib::System::Threading::Interlocked::Exchange_Ref(int32_t& location1, int32_t value)
{
#ifdef _MSC_VER
    return _InterlockedExchange((int32_t volatile*)location1, value);
#else
    return __sync_swap((int32_t volatile*)location1, value);
#endif
}

// Method : System.Threading.Interlocked.Exchange(ref long, long)
inline int64_t CoreLib::System::Threading::Interlocked::Exchange_Ref(int64_t& location1, int64_t value)
{
#ifdef _MSC_VER
    return _InterlockedExchange((int64_t volatile*)location1, value);
#else
    return __sync_swap((int64_t volatile*)location1, value);
#endif
}

// Method : System.Threading.Interlocked.Exchange(ref object, object)
inline object* CoreLib::System::Threading::Interlocked::Exchange_Ref(object*& location1, object* value)
{
#ifdef _MSC_VER
    return _InterlockedExchangePointer((void* volatile*)location1, value);
#else
    return __sync_swap((void* volatile*)location1, value);
#endif
}

// Method : System.Threading.Interlocked.Exchange(ref System.IntPtr, System.IntPtr)
inline int32_t CoreLib::System::Threading::Interlocked::Exchange_Ref(CoreLib::System::IntPtr& location1, CoreLib::System::IntPtr value)
{
#ifdef _MSC_VER
    return _InterlockedExchange((int64_t volatile*)location1, value);
#else
    return __sync_swap((int64_t volatile*)location1, value);
#endif
}

// Method : System.Threading.Interlocked.CompareExchange(ref int, int, int)
inline int32_t CoreLib::System::Threading::Interlocked::CompareExchange_Ref(int32_t& location1, int32_t value, int32_t comparand)
{
#ifdef _MSC_VER
    return _InterlockedCompareExchange((int32_t volatile*)location1, exchange, comparand);
#else
    return __sync_val_compare_and_swap((int32_t volatile*)location1, comparand, exchange);
#endif
}

// Method : System.Threading.Interlocked.CompareExchange(ref long, long, long)
inline int64_t CoreLib::System::Threading::Interlocked::CompareExchange_Ref(int64_t& location1, int64_t value, int64_t comparand)
{
#ifdef _MSC_VER
    return _InterlockedCompareExchange((int64_t volatile*)location1, exchange, comparand);
#else
    return __sync_val_compare_and_swap((int64_t volatile*)location1, comparand, exchange);
#endif
}

// Method : System.Threading.Interlocked.CompareExchange(ref object, object, object)
inline object* CoreLib::System::Threading::Interlocked::CompareExchange_Ref(object*& location1, object* value, object* comparand)
{
#ifdef _MSC_VER
    return _InterlockedCompareExchangePointer((void* volatile*)location1, exchange, comparand);
#else
    return __sync_val_compare_and_swap((void* volatile*)location1, comparand, exchange);
#endif
}

// Method : System.Threading.Interlocked.CompareExchange(ref System.IntPtr, System.IntPtr, System.IntPtr)
inline CoreLib::System::IntPtr CoreLib::System::Threading::Interlocked::CompareExchange_Ref(CoreLib::System::IntPtr& location1, CoreLib::System::IntPtr value, CoreLib::System::IntPtr comparand)
{
#ifdef _MSC_VER
    return _InterlockedCompareExchange((int64_t volatile*)location1, exchange, comparand);
#else
    return __sync_val_compare_and_swap((int64_t volatile*)location1, comparand, exchange);
#endif
}

// Method : System.Threading.Interlocked.CompareExchange(ref int, int, int, ref bool)
inline int32_t CoreLib::System::Threading::Interlocked::CompareExchange_Ref_Ref(int32_t& location1, int32_t value, int32_t comparand, bool& succeeded)
{
#ifdef _MSC_VER
	auto val = (int32_t*)location1;
    auto val_after = _InterlockedCompareExchange((int32_t volatile*)location1, exchange, comparand);
	succeeded = val != val_after;
	return val_after;
#else
	auto val = (int32_t*)location1;
    succeeded = __sync_bool_compare_and_swap((int32_t volatile*)location1, comparand, exchange);
	return val;
#endif
}

// Method : System.Threading.Interlocked.ExchangeAdd(ref int, int)
inline int32_t CoreLib::System::Threading::Interlocked::ExchangeAdd_Ref(int32_t& location1, int32_t value)
{
#ifdef _MSC_VER
    return _InterlockedExchangeAdd((int32_t volatile*)addend, value);
#else
    return __sync_fetch_and_add((int32_t volatile*)addend, value);
#endif
}

// Method : System.Threading.Interlocked.ExchangeAdd(ref long, long)
inline int64_t CoreLib::System::Threading::Interlocked::ExchangeAdd_Ref(int64_t& location1, int64_t value)
{
#ifdef _MSC_VER
    return _InterlockedExchangeAdd((int64_t volatile*)addend, value);
#else
    return __sync_fetch_and_add((int64_t volatile*)addend, value);
#endif
}
