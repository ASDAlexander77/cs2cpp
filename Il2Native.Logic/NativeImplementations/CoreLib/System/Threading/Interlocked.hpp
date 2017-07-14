#ifndef HEADER_System_Threading_Interlocked_STUBS
#define HEADER_System_Threading_Interlocked_STUBS
namespace CoreLib {
	namespace System {
		namespace Threading {

#ifdef CORELIB_ONLY
			// Method : System.Threading.Interlocked.Exchange<T>(ref T, T)
			template <typename T>
			T Interlocked::Exchange_RefT1(T& location1, T value)
			{
				return _interlocked_exchange((T volatile*)&location1, value);
			}

			// Method : System.Threading.Interlocked.CompareExchange<T>(ref T, T, T)
			template <typename T>
			T Interlocked::CompareExchange_RefT1(T& location1, T value, T comparand)
			{
				return _interlocked_compare_exchange((T volatile*)&location1, value, comparand);
			}
#endif
		}
	}
}
#endif