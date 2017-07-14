#include "CoreLib.h"

namespace CoreLib {
	namespace System {
		namespace _ = ::CoreLib::System;

		// Method : System.ModuleHandle.GetToken(System.Reflection.RuntimeModule)
		int32_t ModuleHandle::GetToken(_::Reflection::RuntimeModule* module)
		{
			throw 0xC000C000;
		}

		// Method : System.ModuleHandle.ResolveType(System.Reflection.RuntimeModule, int, System.IntPtr*, int, System.IntPtr*, int, System.Runtime.CompilerServices.ObjectHandleOnStack)
		void ModuleHandle::ResolveType(_::Reflection::RuntimeModule* module, int32_t typeToken, _::IntPtr* typeInstArgs, int32_t typeInstCount, _::IntPtr* methodInstArgs, int32_t methodInstCount, _::Runtime::CompilerServices::ObjectHandleOnStack type)
		{
			throw 0xC000C000;
		}

		// Method : System.ModuleHandle.ResolveMethod(System.Reflection.RuntimeModule, int, System.IntPtr*, int, System.IntPtr*, int)
		_::RuntimeMethodHandleInternal ModuleHandle::ResolveMethod(_::Reflection::RuntimeModule* module, int32_t methodToken, _::IntPtr* typeInstArgs, int32_t typeInstCount, _::IntPtr* methodInstArgs, int32_t methodInstCount)
		{
			throw 0xC000C000;
		}

		// Method : System.ModuleHandle.ResolveField(System.Reflection.RuntimeModule, int, System.IntPtr*, int, System.IntPtr*, int, System.Runtime.CompilerServices.ObjectHandleOnStack)
		void ModuleHandle::ResolveField(_::Reflection::RuntimeModule* module, int32_t fieldToken, _::IntPtr* typeInstArgs, int32_t typeInstCount, _::IntPtr* methodInstArgs, int32_t methodInstCount, _::Runtime::CompilerServices::ObjectHandleOnStack retField)
		{
			throw 0xC000C000;
		}

		// Method : System.ModuleHandle._ContainsPropertyMatchingHash(System.Reflection.RuntimeModule, int, uint)
		bool ModuleHandle::_ContainsPropertyMatchingHash(_::Reflection::RuntimeModule* module, int32_t propertyToken, uint32_t hash)
		{
			throw 0xC000C000;
		}

#ifdef CORELIB_ONLY
		// Method : System.ModuleHandle.GetAssembly(System.Reflection.RuntimeModule, System.Runtime.CompilerServices.ObjectHandleOnStack)
		void ModuleHandle::GetAssembly(_::Reflection::RuntimeModule* handle, _::Runtime::CompilerServices::ObjectHandleOnStack retAssembly)
		{
			throw 0xC000C000;
		}
#endif

		// TODO: temporary solution
		_::RuntimeType rmt;

		// Method : System.ModuleHandle.GetModuleType(System.Reflection.RuntimeModule, System.Runtime.CompilerServices.ObjectHandleOnStack)
		void ModuleHandle::GetModuleType(_::Reflection::RuntimeModule* handle, _::Runtime::CompilerServices::ObjectHandleOnStack type)
		{
			*((_::RuntimeType**)(void*)type.m_ptr) = &rmt;
		}

		// Method : System.ModuleHandle._GetMetadataImport(System.Reflection.RuntimeModule)
		_::IntPtr ModuleHandle::_GetMetadataImport(_::Reflection::RuntimeModule* module)
		{
			throw 0xC000C000;
		}
	}
}