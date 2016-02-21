#include "CoreLib.h"

// Method : System.AppDomain.CreateDomain(string)
CoreLib::System::AppDomain* CoreLib::System::AppDomain::CreateDomain(string* friendlyName)
{
    throw 0xC000C000;
}

// Method : System.AppDomain.Unload(System.AppDomain)
void CoreLib::System::AppDomain::Unload(CoreLib::System::AppDomain* domain)
{
    throw 0xC000C000;
}
