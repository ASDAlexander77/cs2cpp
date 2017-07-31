#include "System.Private.CoreLib.h"

namespace CoreLib { namespace System { 
    namespace _ = ::CoreLib::System;
    // Method : System.RuntimeTypeHandle.IsInstanceOfType(System.RuntimeType, object)
    bool RuntimeTypeHandle::IsInstanceOfType(_::RuntimeType* type, object* o)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetValueInternal(System.RuntimeTypeHandle)
    _::IntPtr RuntimeTypeHandle::GetValueInternal(_::RuntimeTypeHandle handle)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.CreateInstance(System.RuntimeType, bool, ref bool, ref System.RuntimeMethodHandleInternal)
    object* RuntimeTypeHandle::CreateInstance_Ref_Ref(_::RuntimeType* type, bool publicOnly, bool& canBeCached, _::RuntimeMethodHandleInternal& ctor)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.CreateCaInstance(System.RuntimeType, System.IRuntimeMethodInfo)
    object* RuntimeTypeHandle::CreateCaInstance(_::RuntimeType* type, _::IRuntimeMethodInfo* ctor)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.Allocate(System.RuntimeType)
    object* RuntimeTypeHandle::Allocate(_::RuntimeType* type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.CreateInstanceForAnotherGenericParameter(System.RuntimeType, System.RuntimeType)
    object* RuntimeTypeHandle::CreateInstanceForAnotherGenericParameter(_::RuntimeType* type, _::RuntimeType* genericParameter)
    {
		return (((__runtimetype_info*)(void*)type->m_handle)->_methods_table->__new();
    }
    
    // Method : System.RuntimeTypeHandle.GetCorElementType(System.RuntimeType)
    _::Reflection::CorElementType__enum RuntimeTypeHandle::GetCorElementType(_::RuntimeType* type)
    {
		return _::Reflection::CorElementType__enum(((__runtimetype_info*)(void*)type->m_handle)->__cor_element_type);
    }

	// TODO: temporary solution
	_::Reflection::RuntimeAssembly ra;
    
    // Method : System.RuntimeTypeHandle.GetAssembly(System.RuntimeType)
    _::Reflection::RuntimeAssembly* RuntimeTypeHandle::GetAssembly(_::RuntimeType* type)
    {
		return &ra;
    }
    
	// TODO: temporary solution
	_::Reflection::RuntimeModule rm;

    // Method : System.RuntimeTypeHandle.GetModule(System.RuntimeType)
    _::Reflection::RuntimeModule* RuntimeTypeHandle::GetModule(_::RuntimeType* type)
    {
		return &rm;
	}
    
    // Method : System.RuntimeTypeHandle.GetBaseType(System.RuntimeType)
    _::RuntimeType* RuntimeTypeHandle::GetBaseType(_::RuntimeType* type)
    {
		return ((__runtimetype_info*)(void*)type->m_handle)->__base_type;
    }
    
    // Method : System.RuntimeTypeHandle.GetAttributes(System.RuntimeType)
    _::Reflection::TypeAttributes__enum RuntimeTypeHandle::GetAttributes(_::RuntimeType* type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetElementType(System.RuntimeType)
    _::RuntimeType* RuntimeTypeHandle::GetElementType(_::RuntimeType* type)
    {
		return ((__runtimetype_info*)(void*)type->m_handle)->__element_type;
    }
    
    // Method : System.RuntimeTypeHandle.CompareCanonicalHandles(System.RuntimeType, System.RuntimeType)
    bool RuntimeTypeHandle::CompareCanonicalHandles(_::RuntimeType* left, _::RuntimeType* right)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetArrayRank(System.RuntimeType)
    int32_t RuntimeTypeHandle::GetArrayRank(_::RuntimeType* type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetToken(System.RuntimeType)
    int32_t RuntimeTypeHandle::GetToken(_::RuntimeType* type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetMethodAt(System.RuntimeType, int)
    _::RuntimeMethodHandleInternal RuntimeTypeHandle::GetMethodAt(_::RuntimeType* type, int32_t slot)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetFirstIntroducedMethod(System.RuntimeType)
    _::RuntimeMethodHandleInternal RuntimeTypeHandle::GetFirstIntroducedMethod(_::RuntimeType* type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetNextIntroducedMethod(ref System.RuntimeMethodHandleInternal)
    void RuntimeTypeHandle::GetNextIntroducedMethod_Ref(_::RuntimeMethodHandleInternal& method)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetFields(System.RuntimeType, System.IntPtr*, int*)
    bool RuntimeTypeHandle::GetFields(_::RuntimeType* type, _::IntPtr* result, int32_t* count)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetInterfaces(System.RuntimeType)
    __array<_::Type*>* RuntimeTypeHandle::GetInterfaces(_::RuntimeType* type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetConstraints(System.RuntimeTypeHandle, System.Runtime.CompilerServices.ObjectHandleOnStack)
    void RuntimeTypeHandle::GetConstraints(_::RuntimeTypeHandle handle, _::Runtime::CompilerServices::ObjectHandleOnStack types)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetGCHandle(System.RuntimeTypeHandle, System.Runtime.InteropServices.GCHandleType)
    _::IntPtr RuntimeTypeHandle::GetGCHandle(_::RuntimeTypeHandle handle, _::Runtime::InteropServices::GCHandleType__enum type)
    {
		auto gcHandle = __init<_::Runtime::InteropServices::GCHandle>(handle.m_type, type);
		return gcHandle.m_handle;
	}
    
    // Method : System.RuntimeTypeHandle.GetNumVirtuals(System.RuntimeType)
    int32_t RuntimeTypeHandle::GetNumVirtuals(_::RuntimeType* type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.VerifyInterfaceIsImplemented(System.RuntimeTypeHandle, System.RuntimeTypeHandle)
    void RuntimeTypeHandle::VerifyInterfaceIsImplemented(_::RuntimeTypeHandle handle, _::RuntimeTypeHandle interfaceHandle)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetInterfaceMethodImplementationSlot(System.RuntimeTypeHandle, System.RuntimeTypeHandle, System.RuntimeMethodHandleInternal)
    int32_t RuntimeTypeHandle::GetInterfaceMethodImplementationSlot(_::RuntimeTypeHandle handle, _::RuntimeTypeHandle interfaceHandle, _::RuntimeMethodHandleInternal interfaceMethodHandle)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.IsComObject(System.RuntimeType, bool)
    bool RuntimeTypeHandle::IsComObject(_::RuntimeType* type, bool isGenericCOM)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.IsInterface(System.RuntimeType)
    bool RuntimeTypeHandle::IsInterface(_::RuntimeType* type)
    {
		// TODO: finish it
		return false;
    }
    
    // Method : System.RuntimeTypeHandle._IsVisible(System.RuntimeTypeHandle)
    bool RuntimeTypeHandle::_IsVisible(_::RuntimeTypeHandle typeHandle)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.IsValueType(System.RuntimeType)
    bool RuntimeTypeHandle::IsValueType(_::RuntimeType* type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.ConstructName(System.RuntimeTypeHandle, System.TypeNameFormatFlags, System.Runtime.CompilerServices.StringHandleOnStack)
    void RuntimeTypeHandle::ConstructName(_::RuntimeTypeHandle handle, _::TypeNameFormatFlags__enum formatFlags, _::Runtime::CompilerServices::StringHandleOnStack retString)
    {
		auto ref = (string**)(void*)retString.m_ptr;

		switch (formatFlags)
		{
		case TypeNameFormatFlags__enum::FormatBasic:
			*ref = ((string*)nullptr)->CtorCharPtr((char16_t*)((__runtimetype_info*)(void*)handle.m_type->m_handle)->__name);
			break;
		case TypeNameFormatFlags__enum::FormatNamespace:
			*ref = ((string*)nullptr)->CtorCharPtr((char16_t*)((__runtimetype_info*)(void*)handle.m_type->m_handle)->__namespace);
			break;
		case TypeNameFormatFlags__enum::FormatNamespace | TypeNameFormatFlags__enum::FormatFullInst:
			auto namespaceValue = ((string*)nullptr)->CtorCharPtr((char16_t*)((__runtimetype_info*)(void*)handle.m_type->m_handle)->__namespace);
			if (string::IsNullOrEmpty(namespaceValue))
			{
				*ref = ((string*)nullptr)->CtorCharPtr((char16_t*)((__runtimetype_info*)(void*)handle.m_type->m_handle)->__name);
			}
			else
			{
				*ref = string::Concat(namespaceValue, u"."_s, ((string*)nullptr)->CtorCharPtr((char16_t*)((__runtimetype_info*)(void*)handle.m_type->m_handle)->__name));
			}

			break;
		}
	}
    
    // Method : System.RuntimeTypeHandle._GetUtf8Name(System.RuntimeType)
    void* RuntimeTypeHandle::_GetUtf8Name(_::RuntimeType* type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.CanCastTo(System.RuntimeType, System.RuntimeType)
    bool RuntimeTypeHandle::CanCastTo(_::RuntimeType* type, _::RuntimeType* target)
    {
		// TODO: finish it
		return false;
    }
    
    // Method : System.RuntimeTypeHandle.GetDeclaringType(System.RuntimeType)
    _::RuntimeType* RuntimeTypeHandle::GetDeclaringType(_::RuntimeType* type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetDeclaringMethod(System.RuntimeType)
    _::IRuntimeMethodInfo* RuntimeTypeHandle::GetDeclaringMethod(_::RuntimeType* type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetDefaultConstructor(System.RuntimeTypeHandle, System.Runtime.CompilerServices.ObjectHandleOnStack)
    void RuntimeTypeHandle::GetDefaultConstructor(_::RuntimeTypeHandle handle, _::Runtime::CompilerServices::ObjectHandleOnStack method)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetTypeByName(string, bool, bool, bool, System.Runtime.CompilerServices.StackCrawlMarkHandle, System.IntPtr, bool, System.Runtime.CompilerServices.ObjectHandleOnStack, System.Runtime.CompilerServices.ObjectHandleOnStack)
    void RuntimeTypeHandle::GetTypeByName(string* name, bool throwOnError, bool ignoreCase, bool reflectionOnly, _::Runtime::CompilerServices::StackCrawlMarkHandle stackMark, _::IntPtr pPrivHostBinder, bool loadTypeFromPartialName, _::Runtime::CompilerServices::ObjectHandleOnStack type, _::Runtime::CompilerServices::ObjectHandleOnStack keepalive)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetTypeByNameUsingCARules(string, System.Reflection.RuntimeModule, System.Runtime.CompilerServices.ObjectHandleOnStack)
    void RuntimeTypeHandle::GetTypeByNameUsingCARules(string* name, _::Reflection::RuntimeModule* scope, _::Runtime::CompilerServices::ObjectHandleOnStack type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.GetInstantiation(System.RuntimeTypeHandle, System.Runtime.CompilerServices.ObjectHandleOnStack, bool)
    void RuntimeTypeHandle::GetInstantiation(_::RuntimeTypeHandle type, _::Runtime::CompilerServices::ObjectHandleOnStack types, bool fAsRuntimeTypeArray)
    {
		auto arity = ((__runtimetype_info*)(void*)type.m_type->m_handle)->__arity;
		auto array_of_types = __array<::CoreLib::System::Type*>::__new_array(arity);
		for (auto i = 0; i < array_of_types->operator int32_t(); i++)
		{
			array_of_types->_data[i] = _typeof<void>();
		}

		*(void**)types.m_ptr.INTPTR_VALUE_FIELD = (void*)array_of_types;
    }
    
    // Method : System.RuntimeTypeHandle.Instantiate(System.RuntimeTypeHandle, System.IntPtr*, int, System.Runtime.CompilerServices.ObjectHandleOnStack)
    void RuntimeTypeHandle::Instantiate(_::RuntimeTypeHandle handle, _::IntPtr* pInst, int32_t numGenericArgs, _::Runtime::CompilerServices::ObjectHandleOnStack type)
    {
		*(void**)type.m_ptr.INTPTR_VALUE_FIELD = (void*)_typeof<void>();
    }
    
    // Method : System.RuntimeTypeHandle.MakeArray(System.RuntimeTypeHandle, int, System.Runtime.CompilerServices.ObjectHandleOnStack)
    void RuntimeTypeHandle::MakeArray(_::RuntimeTypeHandle handle, int32_t rank, _::Runtime::CompilerServices::ObjectHandleOnStack type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.MakeSZArray(System.RuntimeTypeHandle, System.Runtime.CompilerServices.ObjectHandleOnStack)
    void RuntimeTypeHandle::MakeSZArray(_::RuntimeTypeHandle handle, _::Runtime::CompilerServices::ObjectHandleOnStack type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.MakeByRef(System.RuntimeTypeHandle, System.Runtime.CompilerServices.ObjectHandleOnStack)
    void RuntimeTypeHandle::MakeByRef(_::RuntimeTypeHandle handle, _::Runtime::CompilerServices::ObjectHandleOnStack type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.MakePointer(System.RuntimeTypeHandle, System.Runtime.CompilerServices.ObjectHandleOnStack)
    void RuntimeTypeHandle::MakePointer(_::RuntimeTypeHandle handle, _::Runtime::CompilerServices::ObjectHandleOnStack type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.IsCollectible(System.RuntimeTypeHandle)
    bool RuntimeTypeHandle::IsCollectible(_::RuntimeTypeHandle handle)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.HasInstantiation(System.RuntimeType)
    bool RuntimeTypeHandle::HasInstantiation(_::RuntimeType* type)
    {
		return ((__runtimetype_info*)(void*)type->m_handle)->__is_generic_type_definition;
    }
    
    // Method : System.RuntimeTypeHandle.GetGenericTypeDefinition(System.RuntimeTypeHandle, System.Runtime.CompilerServices.ObjectHandleOnStack)
    void RuntimeTypeHandle::GetGenericTypeDefinition(_::RuntimeTypeHandle type, _::Runtime::CompilerServices::ObjectHandleOnStack retType)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.IsGenericTypeDefinition(System.RuntimeType)
    bool RuntimeTypeHandle::IsGenericTypeDefinition(_::RuntimeType* type)
    {
		return ((__runtimetype_info*)(void*)type->m_handle)->__is_generic_type_definition;
    }
    
    // Method : System.RuntimeTypeHandle.IsGenericVariable(System.RuntimeType)
    bool RuntimeTypeHandle::IsGenericVariable(_::RuntimeType* type)
    {
		return false;
    }
    
    // Method : System.RuntimeTypeHandle.GetGenericVariableIndex(System.RuntimeType)
    int32_t RuntimeTypeHandle::GetGenericVariableIndex(_::RuntimeType* type)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle.ContainsGenericVariables(System.RuntimeType)
    bool RuntimeTypeHandle::ContainsGenericVariables(_::RuntimeType* handle)
    {
		return false;
    }
    
    // Method : System.RuntimeTypeHandle.SatisfiesConstraints(System.RuntimeType, System.IntPtr*, int, System.IntPtr*, int, System.RuntimeType)
    bool RuntimeTypeHandle::SatisfiesConstraints(_::RuntimeType* paramType, _::IntPtr* pTypeContext, int32_t typeContextLength, _::IntPtr* pMethodContext, int32_t methodContextLength, _::RuntimeType* toType)
    {
        throw 3221274624U;
    }
    
    // Method : System.RuntimeTypeHandle._GetMetadataImport(System.RuntimeType)
    _::IntPtr RuntimeTypeHandle::_GetMetadataImport(_::RuntimeType* type)
    {
        throw 3221274624U;
    }

}}

namespace CoreLib { namespace System { 
    namespace _ = ::CoreLib::System;
}}
