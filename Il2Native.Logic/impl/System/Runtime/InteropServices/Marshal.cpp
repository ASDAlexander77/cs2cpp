#include "CoreLib.h"

// Method : System.Runtime.InteropServices.Marshal.GetLastWin32Error()
int32_t CoreLib::System::Runtime::InteropServices::Marshal::GetLastWin32Error()
{
    throw 0xC000C000;
}

// Method : System.Runtime.InteropServices.Marshal.SetLastWin32Error(int)
void CoreLib::System::Runtime::InteropServices::Marshal::SetLastWin32Error(int32_t error)
{
    throw 0xC000C000;
}
