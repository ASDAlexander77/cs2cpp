#include "CoreLib.h"

// Method : System.DateTime.GetSystemTimeAsFileTime()
int64_t CoreLib::System::DateTime::GetSystemTimeAsFileTime()
{
	auto now = std::chrono::time_point_cast<std::chrono::nanoseconds>(std::chrono::system_clock::now());
	auto epoch = now.time_since_epoch();
	auto value = std::chrono::duration_cast<std::chrono::nanoseconds>(epoch);
	auto val = (int64_t) value.count();

	auto val_ticksSince1Jan1970 = val / 10; // ticks since 1 Jan 1970
	auto val_since0 = val_ticksSince1Jan1970 + 621671328000000000;
	auto val_since1601 = val_since0 - 504911232000000000;
	return val_since1601 / 10; // 100-nano seconds
}
