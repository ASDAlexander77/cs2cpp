#ifdef _MSC_VER

::CoreLib::System::IntPtr _interlocked_exchange(::CoreLib::System::IntPtr volatile* location1, ::CoreLib::System::IntPtr value)
{
	return __init<::CoreLib::System::IntPtr>(InterlockedExchangePointer((void* volatile*)&location1->_value, value._value));
}

::CoreLib::System::IntPtr _interlocked_compare_exchange(::CoreLib::System::IntPtr volatile* location1, ::CoreLib::System::IntPtr value, ::CoreLib::System::IntPtr comparand)
{
	return __init<::CoreLib::System::IntPtr>(InterlockedCompareExchangePointer((void* volatile*)&location1->_value, value._value, comparand._value));
}

::CoreLib::System::UIntPtr _interlocked_exchange(::CoreLib::System::UIntPtr volatile* location1, ::CoreLib::System::UIntPtr value)
{
	return __init<::CoreLib::System::UIntPtr>(InterlockedExchangePointer((void* volatile*)&location1->_value, value._value));
}

::CoreLib::System::UIntPtr _interlocked_compare_exchange(::CoreLib::System::UIntPtr volatile* location1, ::CoreLib::System::UIntPtr value, ::CoreLib::System::UIntPtr comparand)
{
	return __init<::CoreLib::System::UIntPtr>(InterlockedCompareExchangePointer((void* volatile*)&location1->_value, value._value, comparand._value));
}

#else // _MSC_VER

::CoreLib::System::IntPtr _interlocked_exchange(::CoreLib::System::IntPtr volatile* location1, ::CoreLib::System::IntPtr value)
{
	__sync_synchronize();
	return __init<::CoreLib::System::IntPtr>(__sync_lock_test_and_set(&location1->_value, value._value));
}

::CoreLib::System::IntPtr _interlocked_compare_exchange(::CoreLib::System::IntPtr volatile* location1, ::CoreLib::System::IntPtr value, ::CoreLib::System::IntPtr comparand)
{
	return __init<::CoreLib::System::IntPtr>(__sync_val_compare_and_swap(&location1->_value, comparand._value, value._value));
}

::CoreLib::System::UIntPtr _interlocked_exchange(::CoreLib::System::UIntPtr volatile* location1, ::CoreLib::System::UIntPtr value)
{
	__sync_synchronize();
	return __init<::CoreLib::System::UIntPtr>(__sync_lock_test_and_set(&location1->_value, value._value));
}

::CoreLib::System::UIntPtr _interlocked_compare_exchange(::CoreLib::System::UIntPtr volatile* location1, ::CoreLib::System::UIntPtr value, ::CoreLib::System::UIntPtr comparand)
{
	return __init<::CoreLib::System::UIntPtr>(__sync_val_compare_and_swap(&location1->_value, comparand._value, value._value));
}

#endif 
