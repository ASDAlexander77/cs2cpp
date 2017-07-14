#include "CoreLib.h"

namespace CoreLib { namespace System { 

	// Method : System.GC.GetGCLatencyMode()
	int32_t GC::GetGCLatencyMode()
	{
		throw 0xC000C000;
	}

	// Method : System.GC.SetGCLatencyMode(int)
	int32_t GC::SetGCLatencyMode(int32_t newLatencyMode)
	{
		throw 0xC000C000;
	}

	// Method : System.GC.GetLOHCompactionMode()
	int32_t GC::GetLOHCompactionMode()
	{
		throw 0xC000C000;
	}

	// Method : System.GC.SetLOHCompactionMode(int)
	void GC::SetLOHCompactionMode(int32_t newLOHCompactionyMode)
	{
		throw 0xC000C000;
	}

	// Method : System.GC.GetGenerationWR(System.IntPtr)
	int32_t GC::GetGenerationWR(::CoreLib::System::IntPtr handle)
	{
		return 0;
	}

	// Method : System.GC.GetTotalMemory()
	int64_t GC::GetTotalMemory()
	{
		return GC_get_total_bytes();
	}

	// Method : System.GC._Collect(int, int)
	void GC::_Collect(int32_t generation, int32_t mode)
	{
		GC_gcollect();
	}

	// Method : System.GC.GetMaxGeneration()
	int32_t GC::GetMaxGeneration()
	{
		return 0;
	}

	// Method : System.GC._CollectionCount(int, int)
	int32_t GC::_CollectionCount(int32_t generation, int32_t getSpecialGCCount)
	{
		return GC_get_gc_no();
	}

	// Method : System.GC.IsServerGC()
	bool GC::IsServerGC()
	{
		return false;
	}

	// Method : System.GC._AddMemoryPressure(ulong)
	void GC::_AddMemoryPressure(uint64_t bytesAllocated)
	{
		throw 0xC000C000;
	}

	// Method : System.GC._RemoveMemoryPressure(ulong)
	void GC::_RemoveMemoryPressure(uint64_t bytesAllocated)
	{
		throw 0xC000C000;
	}

	// Method : System.GC.GetGeneration(object)
	int32_t GC::GetGeneration(object* obj)
	{
		return 0;
	}

	// Method : System.GC._WaitForPendingFinalizers()
	void GC::_WaitForPendingFinalizers()
	{
		while (GC_should_invoke_finalizers()) 
		{
			::CoreLib::System::Threading::Thread::Sleep(10);
		}
	}

	// Method : System.GC._SuppressFinalize(object)
	void GC::_SuppressFinalize(object* o)
	{
		GC_REGISTER_FINALIZER((void *)o, nullptr, (void *)nullptr, (GC_finalization_proc *)nullptr, (void **)nullptr);
	}

	// Method : System.GC._ReRegisterForFinalize(object)
	void GC::_ReRegisterForFinalize(object* o)
	{
		GC_REGISTER_FINALIZER((void *)o, __finalizer, (void *)nullptr, (GC_finalization_proc *)nullptr, (void **)nullptr);
	}

	// Method : System.GC._RegisterForFullGCNotification(int, int)
	bool GC::_RegisterForFullGCNotification(int32_t maxGenerationPercentage, int32_t largeObjectHeapPercentage)
	{
		throw 0xC000C000;
	}

	// Method : System.GC._CancelFullGCNotification()
	bool GC::_CancelFullGCNotification()
	{
		throw 0xC000C000;
	}

	// Method : System.GC._WaitForFullGCApproach(int)
	int32_t GC::_WaitForFullGCApproach(int32_t millisecondsTimeout)
	{
		throw 0xC000C000;
	}

	// Method : System.GC._WaitForFullGCComplete(int)
	int32_t GC::_WaitForFullGCComplete(int32_t millisecondsTimeout)
	{
		throw 0xC000C000;
	}
}}
