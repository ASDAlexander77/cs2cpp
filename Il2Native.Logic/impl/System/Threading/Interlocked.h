#if _WIN32 && _MSC_VER
#include <xatomic.h>
#endif

// Method : System.Threading.Interlocked.Exchange<T>(ref T, T)
template <typename T> 
T CoreLib::System::Threading::Interlocked::Exchange_Ref(T& location1, T value)
{
	throw 0xC000C000;
}

// Method : System.Threading.Interlocked.CompareExchange<T>(ref T, T, T)
template <typename T> 
T CoreLib::System::Threading::Interlocked::CompareExchange_Ref(T& location1, T value, T comparand)
{
#if __ENVIRONMENT_MAC_OS_X_VERSION_MIN_REQUIRED__ >= 1050
	return OSAtomicCompareAndSwapPtr (comparand, value, &location1);
#elif _WIN32 && _MSC_VER
	auto local_comparand = comparand;
	std::_Atomic_compare_exchange_strong_4((volatile std::_Uint4_t*)&location1, (std::_Uint4_t*)&local_comparand, (std::_Uint4_t)value, std::memory_order_relaxed, std::memory_order_relaxed);
	return local_comparand;
#elif (__GNUC__ * 10000 + __GNUC_MINOR__ * 100 + __GNUC_PATCHLEVEL__) > 40100
	return __sync_val_compare_and_swap(&location1, comparand, value);
#else
#  error No implementation
#endif
}
