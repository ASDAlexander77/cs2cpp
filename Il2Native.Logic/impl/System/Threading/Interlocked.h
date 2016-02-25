#if defined(_MSC_VER)

/*
#include <windows.h>

template <typename T> inline T InterlockedCompareExchange(T** location1, T* value, T* comparand)
{
	return InterlockedCompareExchangePointer(location1, value, comparand);
}

inline int16_t InterlockedCompareExchange(int16_t* location1, int16_t value, int16_t comparand)
{
	return InterlockedCompareExchange16(location1, value, comparand);
}

inline uint16_t InterlockedCompareExchange(uint16_t* location1, uint16_t value, uint16_t comparand)
{
	return InterlockedCompareExchange16((int16_t*)location1, value, comparand);
}

inline int64_t InterlockedCompareExchange(int64_t* location1, int64_t value, int64_t comparand)
{
	return InterlockedCompareExchange64(location1, value, comparand);
}

inline uint64_t InterlockedCompareExchange(uint64_t* location1, uint64_t value, uint64_t comparand)
{
	return InterlockedCompareExchange64((int64_t*)location1, value, comparand);
}
*/

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
#elif defined(_MSC_VER)
	////return __X::InterlockedCompareExchange(&location1, value, comparand);
	return value;
#elif (__GNUC__ * 10000 + __GNUC_MINOR__ * 100 + __GNUC_PATCHLEVEL__) > 40100
	return __sync_val_compare_and_swap(&location1, comparand, value);
#else
#  error No implementation
#endif
}
