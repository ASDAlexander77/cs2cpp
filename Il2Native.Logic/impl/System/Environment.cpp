#include "CoreLib.h"

// Method : System.Environment.GetProcessorCount()
int32_t CoreLib::System::Environment::GetProcessorCount()
{
    throw 0xC000C000;
}

// Method : System.Environment.TickCount.get
int32_t CoreLib::System::Environment::get_TickCount()
{
    throw 0xC000C000;
}

// Method : System.Environment._Exit(int)
void CoreLib::System::Environment::_Exit(int32_t exitCode)
{
    throw 0xC000C000;
}
