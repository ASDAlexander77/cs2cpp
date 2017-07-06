#include "CoreLib.h"

namespace CoreLib {
	namespace System {
		
		namespace _ = ::CoreLib::System;
		
		double Number::modf_Ref(double x, double& intpart)
		{
			return std::modf(x, &intpart);
		}
	}
}
