#ifdef _MSC_VER
#include <intrin.h>

template < typename T, int S = sizeof(T) >
T _interlocked_exchange(T volatile* location1, T value);

template < typename T >
inline T _interlocked_exchange<1>(T volatile* location1, T value)
{
	return (T) _InterlockedExchange1((int8_t volatile*)location1, (int8_t)value);
}

template < typename T >
inline T _interlocked_exchange<2>(T volatile* location1, T value)
{
	return (T) _InterlockedExchange2((int16_t volatile*)location1, (int16_t)value);
}

template < typename T >
inline T _interlocked_exchange<4>(T volatile* location1, T value)
{
	return (T) _InterlockedExchange4((int32_t volatile*)location1, (int32_t)value);
}

template < typename T >
inline T _interlocked_exchange<8>(T volatile* location1, T value)
{
	return (T) _InterlockedExchange8((int64_t volatile*)location1, (int64_t)value);
}

template < typename T >
inline T _interlocked_exchange(T* volatile* location1, T* value)
{
	return (T) _InterlockedExchangePointer((void* volatile*)location1, (void*)value);
}

#endif // _MSC_VER

// Method : System.Threading.Interlocked.Exchange<T>(ref T, T)
template <typename T> 
T CoreLib::System::Threading::Interlocked::Exchange_Ref(T& location1, T value)
{
#ifdef _MSC_VER
    return _interlocked_exchange((T volatile*)location1, value);
#else
	__sync_synchronize();
	return (int32_t)__sync_lock_test_and_set((T volatile*)location1, value);

#endif
}

// Method : System.Threading.Interlocked.CompareExchange<T>(ref T, T, T)
template <typename T> 
T CoreLib::System::Threading::Interlocked::CompareExchange_Ref(T& location1, T value, T comparand)
{
#ifdef _MSC_VER
    static_assert(sizeof(long) == sizeof(T), "Size of long must be the same as size of T");
    return (T) _InterlockedCompareExchange((long volatile*)location1, (long)value, (long)comparand);
#else
    return __sync_val_compare_and_swap((T volatile*)location1, comparand, value);
#endif

}
