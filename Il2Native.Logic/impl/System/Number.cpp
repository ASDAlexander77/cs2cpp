#include "CoreLib.h"

double CoreLib::System::Number::modf_Ref(double x, double& intpart)
{
    return std::modf(x, &intpart);
}
