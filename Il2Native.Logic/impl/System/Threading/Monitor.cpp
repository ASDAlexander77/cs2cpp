#include "CoreLib.h"

// Method : System.Threading.Monitor.Enter(object)
void CoreLib::System::Threading::Monitor::Enter(object* obj)
{
    __locks[(void*)obj].lock();  
}

// Method : System.Threading.Monitor.ReliableEnter(object, ref bool)
void CoreLib::System::Threading::Monitor::ReliableEnter_Ref(object* obj, bool& lockTaken)
{
	lockTaken = __locks[(void*)obj].try_lock();
}

// Method : System.Threading.Monitor.Exit(object)
void CoreLib::System::Threading::Monitor::Exit(object* obj)
{
    __locks[(void*)obj].unlock();
}

// Method : System.Threading.Monitor.ReliableEnterTimeout(object, int, ref bool)
void CoreLib::System::Threading::Monitor::ReliableEnterTimeout_Ref(object* obj, int32_t timeout, bool& lockTaken)
{
    lockTaken = __locks[(void*)obj].try_lock_for(std::chrono::milliseconds(timeout));
}

// Method : System.Threading.Monitor.IsEnteredNative(object)
bool CoreLib::System::Threading::Monitor::IsEnteredNative(object* obj)
{
    auto lockTaken = __locks[(void*)obj].try_lock_for(std::chrono::milliseconds(1));
	if (lockTaken)
	{
		__locks[(void*)obj].unlock();
	}

	return !lockTaken;
}

// Method : System.Threading.Monitor.ObjWait(bool, int, object)
bool CoreLib::System::Threading::Monitor::ObjWait(bool exitContext, int32_t millisecondsTimeout, object* obj)
{
    std::unique_lock<std::mutex> unique_lock(__locks[(void*)obj]);
	__conditions[(void*)obj].wait(unique_lock);
}

// Method : System.Threading.Monitor.ObjPulse(object)
void CoreLib::System::Threading::Monitor::ObjPulse(object* obj)
{
    std::unique_lock<std::mutex> unique_lock(__locks[(void*)obj]);
	__conditions[(void*)obj].notify_one(unique_lock);
}

// Method : System.Threading.Monitor.ObjPulseAll(object)
void CoreLib::System::Threading::Monitor::ObjPulseAll(object* obj)
{
    std::unique_lock<std::mutex> unique_lock(__locks[(void*)obj]);
	__conditions[(void*)obj].notify_all(unique_lock);
}
