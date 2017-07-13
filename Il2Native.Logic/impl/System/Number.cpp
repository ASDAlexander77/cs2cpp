#include "CoreLib.h"

namespace CoreLib {
	namespace System {
		
		namespace _ = ::CoreLib::System;
		
#ifdef CORELIB_ONLY
		double Number::modf_Ref(double x, double& intpart)
		{
			return std::modf(x, &intpart);
		}
#endif
	}
}
