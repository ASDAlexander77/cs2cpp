#include "CoreLib.h"

namespace CoreLib {
	namespace System {
		namespace Runtime {
			namespace InteropServices {

#if !_MSC_VER	
				int32_t __last_error = 0;
#endif

				// Method : System.Runtime.InteropServices.Marshal.GetLastWin32Error()
				int32_t Marshal::GetLastWin32Error()
				{
#if _MSC_VER	
					return GetLastError();
#else
					return __last_error;
#endif
				}

				// Method : System.Runtime.InteropServices.Marshal.SetLastWin32Error(int)
				void Marshal::SetLastWin32Error(int32_t error)
				{
#if _MSC_VER	
					SetLastError(error);
#else
					__last_error = error;
#endif
				}
			}
		}
	}
}