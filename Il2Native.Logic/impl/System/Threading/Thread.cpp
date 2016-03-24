#include "CoreLib.h"

// Method : System.Threading.Thread.ManagedThreadId.get
int32_t CoreLib::System::Threading::Thread::get_ManagedThreadId()
{
	throw 3221274624U;
}

#ifdef GC_H
extern "C" void GC_init_parallel();
#endif

// Method : System.Threading.Thread.StartInternal(System.Security.Principal.IPrincipal, ref System.Threading.StackCrawlMark)
void CoreLib::System::Threading::Thread::StartInternal_Ref(CoreLib::System::Security::Principal::IPrincipal* principal, CoreLib::System::Threading::enum_StackCrawlMark& stackMark)
{
	auto voidPtr = (void*)this->DONT_USE_InternalThread;
	if (voidPtr != nullptr)
	{
		throw __new<CoreLib::System::Threading::ThreadStateException>();
	}

	auto thread_ptr = __new_set0(sizeof(std::thread));
	new (thread_ptr) std::thread([=](){

#ifdef GC_H
		GC_init_parallel();
#endif

		auto threadStart = as<CoreLib::System::Threading::ThreadStart*>(this->m_Delegate);
		if (threadStart != nullptr)
		{
			threadStart->Invoke();
			return;
		}

		auto parameterizedThreadStart = as<CoreLib::System::Threading::ParameterizedThreadStart*>(this->m_Delegate);
		if (parameterizedThreadStart != nullptr)
		{
			parameterizedThreadStart->Invoke(this->m_ThreadStartArg);
			return;
		}

		throw __new<CoreLib::System::InvalidOperationException>();
	});

	this->DONT_USE_InternalThread.m_value = thread_ptr;
}

// Method : System.Threading.Thread.InternalGetCurrentThread()
CoreLib::System::IntPtr CoreLib::System::Threading::Thread::InternalGetCurrentThread()
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.AbortInternal()
void CoreLib::System::Threading::Thread::AbortInternal()
{
	auto voidPtr = (void*)this->DONT_USE_InternalThread;
	if (voidPtr == nullptr)
	{
		throw __new<CoreLib::System::InvalidOperationException>();
	}

	auto threadPtr = reinterpret_cast<std::thread*>(voidPtr);
	return threadPtr->detach();
}

// Method : System.Threading.Thread.GetPriorityNative()
int32_t CoreLib::System::Threading::Thread::GetPriorityNative()
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.SetPriorityNative(int)
void CoreLib::System::Threading::Thread::SetPriorityNative(int32_t priority)
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.IsAlive.get
bool CoreLib::System::Threading::Thread::get_IsAlive()
{
	auto voidPtr = (void*)this->DONT_USE_InternalThread;
	if (voidPtr == nullptr)
	{
		throw __new<CoreLib::System::InvalidOperationException>();
	}

	auto threadPtr = reinterpret_cast<std::thread*>(voidPtr);
	return threadPtr->joinable();
}

// Method : System.Threading.Thread.IsThreadPoolThread.get
bool CoreLib::System::Threading::Thread::get_IsThreadPoolThread()
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.JoinInternal(int)
bool CoreLib::System::Threading::Thread::JoinInternal(int32_t millisecondsTimeout)
{
	auto voidPtr = (void*)this->DONT_USE_InternalThread;
	if (voidPtr == nullptr)
	{
		throw __new<CoreLib::System::InvalidOperationException>();
	}

	auto threadPtr = reinterpret_cast<std::thread*>(voidPtr);
	if (threadPtr->joinable())
	{
		threadPtr->join();
		return true;
	}

	return false;
}

// Method : System.Threading.Thread.SleepInternal(int)
void CoreLib::System::Threading::Thread::SleepInternal(int32_t millisecondsTimeout)
{
	std::this_thread::sleep_for(std::chrono::milliseconds(millisecondsTimeout));
}

// Method : System.Threading.Thread.SpinWaitInternal(int)
void CoreLib::System::Threading::Thread::SpinWaitInternal(int32_t iterations)
{
	for (int i = 0; i < iterations; i++)
	{
		std::this_thread::yield();
	}
}

// Method : System.Threading.Thread.YieldInternal()
bool CoreLib::System::Threading::Thread::YieldInternal()
{
	std::this_thread::yield();
	return true;
}

// Method : System.Threading.Thread.GetCurrentThreadNative()
CoreLib::System::Threading::Thread* CoreLib::System::Threading::Thread::GetCurrentThreadNative()
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.GetProcessDefaultStackSize()
uint64_t CoreLib::System::Threading::Thread::GetProcessDefaultStackSize()
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.SetStart(System.Delegate, int)
void CoreLib::System::Threading::Thread::SetStart(CoreLib::System::Delegate* start, int32_t maxStackSize)
{
	this->m_Delegate = start;
}

// Method : System.Threading.Thread.InternalFinalize()
void CoreLib::System::Threading::Thread::InternalFinalize()
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.IsBackgroundNative()
bool CoreLib::System::Threading::Thread::IsBackgroundNative()
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.SetBackgroundNative(bool)
void CoreLib::System::Threading::Thread::SetBackgroundNative(bool isBackground)
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.GetThreadStateNative()
int32_t CoreLib::System::Threading::Thread::GetThreadStateNative()
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.nativeInitCultureAccessors()
void CoreLib::System::Threading::Thread::nativeInitCultureAccessors()
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.GetDomainInternal()
CoreLib::System::AppDomain* CoreLib::System::Threading::Thread::GetDomainInternal()
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.GetFastDomainInternal()
CoreLib::System::AppDomain* CoreLib::System::Threading::Thread::GetFastDomainInternal()
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.InformThreadNameChange(System.Threading.ThreadHandle, string, int)
void CoreLib::System::Threading::Thread::InformThreadNameChange(CoreLib::System::Threading::ThreadHandle t, string* name, int32_t len)
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.MemoryBarrier()
void CoreLib::System::Threading::Thread::MemoryBarrier()
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.SetAbortReason(object)
void CoreLib::System::Threading::Thread::SetAbortReason(object* o)
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.GetAbortReason()
object* CoreLib::System::Threading::Thread::GetAbortReason()
{
	throw 3221274624U;
}

// Method : System.Threading.Thread.ClearAbortReason()
void CoreLib::System::Threading::Thread::ClearAbortReason()
{
	throw 3221274624U;
}
