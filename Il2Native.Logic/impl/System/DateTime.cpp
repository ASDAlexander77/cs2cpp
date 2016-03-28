#include "CoreLib.h"

// Method : System.DateTime.GetSystemTimeAsFileTime()
int64_t CoreLib::System::DateTime::GetSystemTimeAsFileTime()
{
/*
	auto now = std::chrono::time_point_cast<std::chrono::seconds>(std::chrono::system_clock::now());
	auto epoch = now.time_since_epoch();
	auto value = std::chrono::duration_cast<std::chrono::seconds>(epoch);
    return (int64_t) value.count();
*/
    return 0;
}
