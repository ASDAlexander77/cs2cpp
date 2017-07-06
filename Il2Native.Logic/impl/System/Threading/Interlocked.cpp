#include "CoreLib.h"

namespace CoreLib {
	namespace System {
		namespace Threading {

			// Method : System.Threading.Interlocked.Increment(ref int)
			int32_t Interlocked::Increment_Ref(int32_t& location)
			{
#ifdef _MSC_VER
				return (int32_t)InterlockedIncrement((LONG volatile*)&location);
#else
				return __sync_add_and_fetch((int32_t volatile*)&location, 1);
#endif
			}

			// Method : System.Threading.Interlocked.Increment(ref long)
			int64_t Interlocked::Increment_Ref(int64_t& location)
			{
#ifdef _MSC_VER
				return InterlockedAdd64((int64_t volatile*)&location, 1);
#else
				return __sync_add_and_fetch((int64_t volatile*)&location, 1);
#endif
			}

			// Method : System.Threading.Interlocked.Decrement(ref int)
			int32_t Interlocked::Decrement_Ref(int32_t& location)
			{
#ifdef _MSC_VER
				return (int32_t)InterlockedDecrement((long volatile*)&location);
#else
				return __sync_sub_and_fetch((int32_t volatile*)&location, 1);
#endif
			}

			// Method : System.Threading.Interlocked.Decrement(ref long)
			int64_t Interlocked::Decrement_Ref(int64_t& location)
			{
#ifdef _MSC_VER
				return InterlockedAdd64((int64_t volatile*)&location, -1LL);
#else
				return __sync_sub_and_fetch((int64_t volatile*)&location, 1);
#endif
			}

			// Method : System.Threading.Interlocked.Exchange(ref int, int)
			int32_t Interlocked::Exchange_Ref(int32_t& location1, int32_t value)
			{
#ifdef _MSC_VER
				return InterlockedExchange((LONG volatile*)&location1, value);
#else
				__sync_synchronize();
				return __sync_lock_test_and_set((int32_t volatile*)&location1, value);
#endif
			}

			// Method : System.Threading.Interlocked.Exchange(ref long, long)
			int64_tInterlocked::Exchange_Ref(int64_t& location1, int64_t value)
			{
#ifdef _MSC_VER
				return InterlockedExchange64((int64_t volatile*)&location1, value);
#else
				__sync_synchronize();
				return __sync_lock_test_and_set((int64_t volatile*)&location1, value);
#endif
			}

			// Method : System.Threading.Interlocked.Exchange(ref object, object)
			object* Interlocked::Exchange_Ref(object*& location1, object* value)
			{
#ifdef _MSC_VER
				return (object*)InterlockedExchangePointer((void* volatile*)&location1, value);
#else
				__sync_synchronize();
				return (object*)__sync_lock_test_and_set((void* volatile*)&location1, value);
#endif
			}

			// Method : System.Threading.Interlocked.Exchange(ref System.IntPtr, System.IntPtr)
			::CoreLib::System::IntPtr Interlocked::Exchange_Ref(::CoreLib::System::IntPtr& location1, ::CoreLib::System::IntPtr value)
			{
#ifdef _MSC_VER
				return __init<::CoreLib::System::IntPtr>(InterlockedExchangePointer((void* volatile*)&location1->m_value, value->m_value));
#else
				__sync_synchronize();
				return __init<::CoreLib::System::IntPtr>(__sync_lock_test_and_set((void* volatile*)&location1->m_value, value->m_value));
#endif
			}

			// Method : System.Threading.Interlocked.CompareExchange(ref int, int, int)
			int32_t Interlocked::CompareExchange_Ref(int32_t& location1, int32_t value, int32_t comparand)
			{
#ifdef _MSC_VER
				return InterlockedCompareExchange((LONG volatile*)&location1, value, comparand);
#else
				return __sync_val_compare_and_swap((int32_t volatile*)&location1, comparand, value);
#endif
			}

			// Method : System.Threading.Interlocked.CompareExchange(ref long, long, long)
			int64_t Interlocked::CompareExchange_Ref(int64_t& location1, int64_t value, int64_t comparand)
			{
#ifdef _MSC_VER
				return InterlockedCompareExchange64((int64_t volatile*)&location1, value, comparand);
#else
				return __sync_val_compare_and_swap((int64_t volatile*)&location1, comparand, value);
#endif
			}

			// Method : System.Threading.Interlocked.CompareExchange(ref object, object, object)
			object* Interlocked::CompareExchange_Ref(object*& location1, object* value, object* comparand)
			{
#ifdef _MSC_VER
				return (object*)InterlockedCompareExchangePointer((void* volatile*)&location1, value, comparand);
#else
				return (object*)__sync_val_compare_and_swap((void* volatile*)&location1, comparand, value);
#endif
			}

			// Method : System.Threading.Interlocked.CompareExchange(ref System.IntPtr, System.IntPtr, System.IntPtr)
			::CoreLib::System::IntPtr Interlocked::CompareExchange_Ref(::CoreLib::System::IntPtr& location1, ::CoreLib::System::IntPtr value, ::CoreLib::System::IntPtr comparand)
			{
#ifdef _MSC_VER
				return __init<::CoreLib::System::IntPtr>(InterlockedCompareExchangePointer((void* volatile*)&location1->m_value, value->m_value, comparand->m_value));
#else
				return __init<::CoreLib::System::IntPtr>(__sync_val_compare_and_swap((void* volatile*)&location1->m_value, comparand->m_value, value->m_value));
#endif
			}

			// Method : System.Threading.Interlocked.CompareExchange(ref int, int, int, ref bool)
			int32_t Interlocked::CompareExchange_Ref_Ref(int32_t& location1, int32_t value, int32_t comparand, bool& succeeded)
			{
#ifdef _MSC_VER
				int32_t val = *(int32_t*)location1;
				int32_t val_after = InterlockedCompareExchange((LONG volatile*)&location1, value, comparand);
				succeeded = val != val_after;
				return val_after;
#else
				auto val = *(int32_t*)location1;
				succeeded = __sync_bool_compare_and_swap((int32_t volatile*)&location1, comparand, value);
				return val;
#endif
			}

			// Method : System.Threading.Interlocked.ExchangeAdd(ref int, int)
			int32_t Interlocked::ExchangeAdd_Ref(int32_t& location1, int32_t value)
			{
#ifdef _MSC_VER
				return InterlockedExchangeAdd((LONG volatile*)&location1, value);
#else
				return __sync_fetch_and_add((int32_t volatile*)&location1, value);
#endif
			}

			// Method : System.Threading.Interlocked.ExchangeAdd(ref long, long)
			int64_t Interlocked::ExchangeAdd_Ref(int64_t& location1, int64_t value)
			{
#ifdef _MSC_VER
				return InterlockedExchangeAdd64((int64_t volatile*)&location1, value);
#else
				return __sync_fetch_and_add((int64_t volatile*)&location1, value);
#endif
			}
		}
	}
}