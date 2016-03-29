#include "CoreLib.h"

// Method : System.DateTime.GetSystemTimeAsFileTime()
int64_t CoreLib::System::DateTime::GetSystemTimeAsFileTime()
{
	auto now = std::chrono::time_point_cast<std::chrono::nanoseconds>(std::chrono::system_clock::now());
	auto epoch = now.time_since_epoch();
	auto value = std::chrono::duration_cast<std::chrono::nanoseconds>(epoch);
	auto val = (int64_t) value.count();

	auto val_system = (val / 100) + 116436096000000000 + 8639999870000;
	return val_system;
}
