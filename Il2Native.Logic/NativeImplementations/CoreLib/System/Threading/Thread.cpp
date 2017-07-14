#include "CoreLib.h"

namespace CoreLib {
	namespace System {
		namespace Threading {

			thread_local Thread* __current_thread;

			// Method : System.Threading.Thread.ManagedThreadId.get
			int32_t Thread::get_ManagedThreadId()
			{
#ifndef GC_PTHREADS
				return (int32_t)GetCurrentThreadId();
#else
				return (int32_t)pthread_self();
#endif
			}

			void* __stdcall __thread_inner_proc(void* params)
			{
				auto __this = (Thread*)params;

				auto threadStart = as<::CoreLib::System::Threading::ThreadStart*>(__this->m_Delegate);
				if (threadStart != nullptr)
				{
					threadStart->Invoke();
					return 0;
				}

				auto parameterizedThreadStart = as<::CoreLib::System::Threading::ParameterizedThreadStart*>(__this->m_Delegate);
				if (parameterizedThreadStart != nullptr)
				{
					parameterizedThreadStart->Invoke(__this->m_ThreadStartArg);
					return 0;
				}

				throw __new<::CoreLib::System::InvalidOperationException>();
			}

#ifdef CORELIB_ONLY
			// Method : System.Threading.Thread.StartInternal(System.Security.Principal.IPrincipal, ref System.Threading.StackCrawlMark)
			void Thread::StartInternal_Ref(::CoreLib::System::Security::Principal::IPrincipal* principal, ::CoreLib::System::Threading::StackCrawlMark__enum& stackMark)
			{
				auto voidPtr = (void*)this->DONT_USE_InternalThread;
				if (voidPtr != nullptr)
				{
					throw __new<::CoreLib::System::Threading::ThreadStateException>();
				}

				__current_thread = this;

				int32_t threadId;
#ifndef GC_PTHREADS
				this->DONT_USE_InternalThread._value = CreateThread(
					nullptr,                // default security attributes
					0,                      // use default stack size  
					(LPTHREAD_START_ROUTINE)__thread_inner_proc,    // thread function name
					this,					// argument to thread function 
					0,                      // use default creation flags 
					(LPDWORD)&threadId);	// returns the thread identifier 
#else
				auto t = __new_set0(sizeof(pthread_t), true);
				pthread_create((pthread_t *)t, 0, __thread_inner_proc, this);
				this->DONT_USE_InternalThread._value = t;
#endif
			}
#endif 

			// Method : System.Threading.Thread.InternalGetCurrentThread()
			::CoreLib::System::IntPtr Thread::InternalGetCurrentThread()
			{
				return __current_thread->DONT_USE_InternalThread;
			}

#ifdef CORELIB_ONLY
			// Method : System.Threading.Thread.AbortInternal()
			void Thread::AbortInternal()
			{
				auto voidPtr = (void*)this->DONT_USE_InternalThread;
				if (voidPtr == nullptr)
				{
					throw __new<::CoreLib::System::InvalidOperationException>();
				}

#ifndef GC_PTHREADS
				CloseHandle((HANDLE)voidPtr);
#else
				pthread_detach(*(pthread_t*)voidPtr);
#endif
			}

			// Method : System.Threading.Thread.GetPriorityNative()
			int32_t Thread::GetPriorityNative()
			{
				throw 0xC000C000;
			}

			// Method : System.Threading.Thread.SetPriorityNative(int)
			void Thread::SetPriorityNative(int32_t priority)
			{
				throw 0xC000C000;
			}

			// Method : System.Threading.Thread.IsAlive.get
			bool Thread::get_IsAlive()
			{
				auto voidPtr = (void*)this->DONT_USE_InternalThread;
				if (voidPtr == nullptr)
				{
					throw __new<::CoreLib::System::InvalidOperationException>();
				}

				return true;
			}

			// Method : System.Threading.Thread.IsThreadPoolThread.get
			bool Thread::get_IsThreadPoolThread()
			{
				return false;
			}

			// Method : System.Threading.Thread.JoinInternal(int)
			bool Thread::JoinInternal(int32_t millisecondsTimeout)
			{
				auto voidPtr = (void*)this->DONT_USE_InternalThread;
				if (voidPtr == nullptr)
				{
					throw __new<::CoreLib::System::InvalidOperationException>();
				}

#ifndef GC_PTHREADS
				return WaitForSingleObject((HANDLE)voidPtr, millisecondsTimeout == -1 ? INFINITE : millisecondsTimeout) == WAIT_OBJECT_0;
#else
				return pthread_join(*(pthread_t*)voidPtr, 0) == 0;
#endif
			}
#endif

			// Method : System.Threading.Thread.SleepInternal(int)
			void Thread::SleepInternal(int32_t millisecondsTimeout)
			{
				std::this_thread::sleep_for(std::chrono::milliseconds(millisecondsTimeout));
			}

			// Method : System.Threading.Thread.SpinWaitInternal(int)
			void Thread::SpinWaitInternal(int32_t iterations)
			{
				for (int i = 0; i < iterations; i++)
				{
					std::this_thread::yield();
				}
			}

			// Method : System.Threading.Thread.YieldInternal()
			bool Thread::YieldInternal()
			{
				std::this_thread::yield();
				return true;
			}

			// Method : System.Threading.Thread.GetCurrentThreadNative()
			Thread* Thread::GetCurrentThreadNative()
			{
				return __current_thread;
			}

#ifdef CORELIB_ONLY
			// Method : System.Threading.Thread.GetProcessDefaultStackSize()
			uint64_t Thread::GetProcessDefaultStackSize()
			{
				throw 0xC000C000;
			}
#endif

			// Method : System.Threading.Thread.SetStart(System.Delegate, int)
			void Thread::SetStart(::CoreLib::System::Delegate* start, int32_t maxStackSize)
			{
				this->m_Delegate = start;
			}

			// Method : System.Threading.Thread.InternalFinalize()
			void Thread::InternalFinalize()
			{
				// This function is intentionally blank.
			}

#ifdef CORELIB_ONLY
			// Method : System.Threading.Thread.IsBackgroundNative()
			bool Thread::IsBackgroundNative()
			{
				return false;
			}

			// Method : System.Threading.Thread.SetBackgroundNative(bool)
			void Thread::SetBackgroundNative(bool isBackground)
			{
			}

			// Method : System.Threading.Thread.GetThreadStateNative()
			int32_t Thread::GetThreadStateNative()
			{
				return 0;
			}
#endif

			// Method : System.Threading.Thread.nativeInitCultureAccessors()
			void Thread::nativeInitCultureAccessors()
			{
				throw 0xC000C000;
			}

			// Method : System.Threading.Thread.GetDomainInternal()
			::CoreLib::System::AppDomain* Thread::GetDomainInternal()
			{
				throw 0xC000C000;
			}

			// Method : System.Threading.Thread.GetFastDomainInternal()
			::CoreLib::System::AppDomain* Thread::GetFastDomainInternal()
			{
				throw 0xC000C000;
			}

			// Method : System.Threading.Thread.InformThreadNameChange(System.Threading.ThreadHandle, string, int)
			void Thread::InformThreadNameChange(::CoreLib::System::Threading::ThreadHandle t, string* name, int32_t len)
			{
				throw 0xC000C000;
			}

			// Method : System.Threading.Thread.MemoryBarrier()
#ifdef CORELIB_ONLY
#undef MemoryBarrier
			void Thread::MemoryBarrier()
			{
				std::atomic_thread_fence(std::memory_order_relaxed);
			}

			// Method : System.Threading.Thread.SetAbortReason(object)
			void Thread::SetAbortReason(object* o)
			{
				throw 0xC000C000;
			}

			// Method : System.Threading.Thread.GetAbortReason()
			object* Thread::GetAbortReason()
			{
				throw 0xC000C000;
			}

			// Method : System.Threading.Thread.ClearAbortReason()
			void Thread::ClearAbortReason()
			{
				throw 0xC000C000;
			}
#endif
		}
	}
}