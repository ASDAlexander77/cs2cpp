#ifdef _MSC_VER
#include <intrin.h>
#endif // _MSC_VER

// Method : System.Threading.Interlocked.Exchange<T>(ref T, T)
template <typename T> 
T CoreLib::System::Threading::Interlocked::Exchange_Ref(T& location1, T value)
{
#ifdef _MSC_VER
    static_assert(sizeof(long) == sizeof(T), "Size of long must be the same as size of T");
    return _InterlockedExchange((long volatile*)location1, value);
#else
    return __sync_swap((T volatile*)location1, value);
#endif
}

// Method : System.Threading.Interlocked.CompareExchange<T>(ref T, T, T)
template <typename T> 
T CoreLib::System::Threading::Interlocked::CompareExchange_Ref(T& location1, T value, T comparand)
{
#ifdef _MSC_VER
    static_assert(sizeof(long) == sizeof(T), "Size of long must be the same as size of T");
    return _InterlockedCompareExchange((long volatile*)destination, value, comparand);
#else
    return __sync_val_compare_and_swap((T volatile*)destination, comparand, value);
#endif

}
