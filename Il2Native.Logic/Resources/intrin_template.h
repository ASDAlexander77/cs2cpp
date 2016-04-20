namespace CoreLib { namespace System {
	struct IntPtr;
	struct UIntPtr;
}}

extern CoreLib::System::IntPtr _interlocked_exchange(CoreLib::System::IntPtr volatile* location1, CoreLib::System::IntPtr value);
extern CoreLib::System::IntPtr _interlocked_compare_exchange(CoreLib::System::IntPtr volatile* location1, CoreLib::System::IntPtr value, CoreLib::System::IntPtr comparand);

extern CoreLib::System::UIntPtr _interlocked_exchange(CoreLib::System::UIntPtr volatile* location1, CoreLib::System::UIntPtr value);
extern CoreLib::System::UIntPtr _interlocked_compare_exchange(CoreLib::System::UIntPtr volatile* location1, CoreLib::System::UIntPtr value, CoreLib::System::UIntPtr comparand);

#ifdef _MSC_VER

// InterlockedAdd
template < typename T >
inline typename std::enable_if<sizeof(T) == 4 && !std::is_pointer<T>::value, T>::type _interlocked_add(T volatile* location1, T value)
{
	return (T) InterlockedAdd((int32_t volatile*)location1, (int32_t)value);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 8 && !std::is_pointer<T>::value, T>::type _interlocked_add(T volatile* location1, T value)
{
	return (T) InterlockedAdd64((int64_t volatile*)location1, (int64_t)value);
}

// InterlockedSub
template < typename T >
inline typename std::enable_if<sizeof(T) == 4 && !std::is_pointer<T>::value, T>::type _interlocked_sub(T volatile* location1, T value)
{
	return (T) InterlockedAdd((int32_t volatile*)location1, -((int32_t)value));
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 8 && !std::is_pointer<T>::value, T>::type _interlocked_sub(T volatile* location1, T value)
{
	return (T) InterlockedAdd64((int64_t volatile*)location1, -((int64_t)value));
}

// InterlockedAnd
template < typename T >
inline typename std::enable_if<sizeof(T) == 1 && !std::is_pointer<T>::value, T>::type _interlocked_and(T volatile* location1, T value)
{
	return (T) InterlockedAnd8((char volatile*)location1, (char)value);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 2 && !std::is_pointer<T>::value, T>::type _interlocked_and(T volatile* location1, T value)
{
	return (T) InterlockedAnd16((int16_t volatile*)location1, (int16_t)value);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 4 && !std::is_pointer<T>::value, T>::type _interlocked_and(T volatile* location1, T value)
{
	return (T) InterlockedAnd((int32_t volatile*)location1, (int32_t)value);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 8 && !std::is_pointer<T>::value, T>::type _interlocked_and(T volatile* location1, T value)
{
	return (T) InterlockedAnd64((int64_t volatile*)location1, (int64_t)value);
}

// InterlockedOr
template < typename T >
inline typename std::enable_if<sizeof(T) == 1 && !std::is_pointer<T>::value, T>::type _interlocked_or(T volatile* location1, T value)
{
	return (T) InterlockedOr8((char volatile*)location1, (char)value);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 2 && !std::is_pointer<T>::value, T>::type _interlocked_or(T volatile* location1, T value)
{
	return (T) InterlockedOr16((int16_t volatile*)location1, (int16_t)value);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 4 && !std::is_pointer<T>::value, T>::type _interlocked_or(T volatile* location1, T value)
{
	return (T) InterlockedOr((int32_t volatile*)location1, (int32_t)value);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 8 && !std::is_pointer<T>::value, T>::type _interlocked_or(T volatile* location1, T value)
{
	return (T) InterlockedOr64((int64_t volatile*)location1, (int64_t)value);
}

// InterlockedXor
template < typename T >
inline typename std::enable_if<sizeof(T) == 1 && !std::is_pointer<T>::value, T>::type _interlocked_xor(T volatile* location1, T value)
{
	return (T) InterlockedXor8((char volatile*)location1, (char)value);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 2 && !std::is_pointer<T>::value, T>::type _interlocked_xor(T volatile* location1, T value)
{
	return (T) InterlockedXor16((int16_t volatile*)location1, (int16_t)value);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 4 && !std::is_pointer<T>::value, T>::type _interlocked_xor(T volatile* location1, T value)
{
	return (T) InterlockedXor((int32_t volatile*)location1, (int32_t)value);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 8 && !std::is_pointer<T>::value, T>::type _interlocked_xor(T volatile* location1, T value)
{
	return (T) InterlockedXor64((int64_t volatile*)location1, (int64_t)value);
}

// InterlockedExchange
inline bool _interlocked_exchange(bool volatile* location1, bool value)
{
	return (bool) InterlockedExchange8((char volatile*)location1, (char)value);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 1 && !std::is_pointer<T>::value, T>::type _interlocked_exchange(T volatile* location1, T value)
{
	return (T) InterlockedExchange8((char volatile*)location1, (char)value);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 2 && !std::is_pointer<T>::value, T>::type _interlocked_exchange(T volatile* location1, T value)
{
	return (T) InterlockedExchange16((int16_t volatile*)location1, (int16_t)value);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 4 && !std::is_pointer<T>::value, T>::type _interlocked_exchange(T volatile* location1, T value)
{
	return (T) InterlockedExchange((LONG volatile*)location1, (LONG)value);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 8 && !std::is_pointer<T>::value, T>::type _interlocked_exchange(T volatile* location1, T value)
{
	return (T) InterlockedExchange64((int64_t volatile*)location1, (int64_t)value);
}

template < typename T >
inline typename std::enable_if<std::is_pointer<T>::value, T>::type _interlocked_exchange(T volatile* location1, T value)
{
	return (T) InterlockedExchangePointer((void* volatile*)location1, (void*)value);
}

// InterlockedCompareExchange
inline bool _interlocked_compare_exchange(bool volatile* location1, bool value, bool comparand)
{
	return (bool) InterlockedCompareExchange16((int16_t volatile*)location1, (int16_t)value, (int16_t)comparand);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 2 && !std::is_pointer<T>::value, T>::type _interlocked_compare_exchange(T volatile* location1, T value, T comparand)
{
	return (T) InterlockedCompareExchange16((int16_t volatile*)location1, (int16_t)value, (int16_t)comparand);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 4 && !std::is_pointer<T>::value, T>::type _interlocked_compare_exchange(T volatile* location1, T value, T comparand)
{
	return (T) InterlockedCompareExchange((LONG volatile*)location1, (LONG)value, (LONG)comparand);
}

template < typename T >
inline typename std::enable_if<sizeof(T) == 8 && !std::is_pointer<T>::value, T>::type _interlocked_compare_exchange(T volatile* location1, T value, T comparand)
{
	return (T) InterlockedCompareExchange64((int64_t volatile*)location1, (int64_t)value, (int64_t)comparand);
}

template < typename T >
inline typename std::enable_if<std::is_pointer<T>::value, T>::type _interlocked_compare_exchange(T volatile* location1, T value, T comparand)
{
	return (T) InterlockedCompareExchangePointer((void* volatile*)location1, (void*)value, (void*)comparand);
}

#else // _MSC_VER

template < typename T >
inline T _interlocked_add(T volatile* location1, T value)
{
	return (T) __sync_add_and_fetch(location1, value);
}

template < typename T >
inline T _interlocked_sub(T volatile* location1, T value)
{
	return (T) __sync_add_and_fetch(location1, -value);
}

template < typename T >
inline T _interlocked_and(T volatile* location1, T value)
{
	return (T) __sync_and_and_fetch(location1, -value);
}

template < typename T >
inline T _interlocked_or(T volatile* location1, T value)
{
	return (T) __sync_or_and_fetch(location1, -value);
}

template < typename T >
inline T _interlocked_xor(T volatile* location1, T value)
{
	return (T) __sync_xor_and_fetch(location1, -value);
}

template < typename T >
inline T _interlocked_exchange(T volatile* location1, T value)
{
	__sync_synchronize();
	return __sync_lock_test_and_set(location1, value);
}

template < typename T >
inline T _interlocked_compare_exchange(T volatile* location1, T value, T comparand)
{
	return __sync_val_compare_and_swap(location1, comparand, value);
}

#endif 
