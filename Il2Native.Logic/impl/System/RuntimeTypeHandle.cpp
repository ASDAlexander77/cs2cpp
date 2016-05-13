#include "CoreLib.h"

// Method : System.RuntimeTypeHandle.IsInstanceOfType(System.RuntimeType, object)
bool CoreLib::System::RuntimeTypeHandle::IsInstanceOfType(CoreLib::System::RuntimeType* type, object* o)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetValueInternal(System.RuntimeTypeHandle)
CoreLib::System::IntPtr CoreLib::System::RuntimeTypeHandle::GetValueInternal(CoreLib::System::RuntimeTypeHandle handle)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.CreateInstance(System.RuntimeType, bool, bool, ref bool, ref System.RuntimeMethodHandleInternal, ref bool)
object* CoreLib::System::RuntimeTypeHandle::CreateInstance_Ref_Ref_Ref(CoreLib::System::RuntimeType* type, bool publicOnly, bool noCheck, bool& canBeCached, CoreLib::System::RuntimeMethodHandleInternal& ctor, bool& bNeedSecurityCheck)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.CreateCaInstance(System.RuntimeType, System.IRuntimeMethodInfo)
object* CoreLib::System::RuntimeTypeHandle::CreateCaInstance(CoreLib::System::RuntimeType* type, CoreLib::System::IRuntimeMethodInfo* ctor)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.Allocate(System.RuntimeType)
object* CoreLib::System::RuntimeTypeHandle::Allocate(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.CreateInstanceForAnotherGenericParameter(System.RuntimeType, System.RuntimeType)
object* CoreLib::System::RuntimeTypeHandle::CreateInstanceForAnotherGenericParameter(CoreLib::System::RuntimeType* type, CoreLib::System::RuntimeType* genericParameter)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetCorElementType(System.RuntimeType)
CoreLib::System::Reflection::enum_CorElementType CoreLib::System::RuntimeTypeHandle::GetCorElementType(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

CoreLib::System::Reflection::RuntimeAssembly ra;

// Method : System.RuntimeTypeHandle.GetAssembly(System.RuntimeType)
CoreLib::System::Reflection::RuntimeAssembly* CoreLib::System::RuntimeTypeHandle::GetAssembly(CoreLib::System::RuntimeType* type)
{
    return &ra;
}

CoreLib::System::Reflection::RuntimeModule rm;

// Method : System.RuntimeTypeHandle.GetModule(System.RuntimeType)
CoreLib::System::Reflection::RuntimeModule* CoreLib::System::RuntimeTypeHandle::GetModule(CoreLib::System::RuntimeType* type)
{
    return &rm;
}

// Method : System.RuntimeTypeHandle.GetBaseType(System.RuntimeType)
CoreLib::System::RuntimeType* CoreLib::System::RuntimeTypeHandle::GetBaseType(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetAttributes(System.RuntimeType)
CoreLib::System::Reflection::enum_TypeAttributes CoreLib::System::RuntimeTypeHandle::GetAttributes(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetElementType(System.RuntimeType)
CoreLib::System::RuntimeType* CoreLib::System::RuntimeTypeHandle::GetElementType(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.CompareCanonicalHandles(System.RuntimeType, System.RuntimeType)
bool CoreLib::System::RuntimeTypeHandle::CompareCanonicalHandles(CoreLib::System::RuntimeType* left, CoreLib::System::RuntimeType* right)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetArrayRank(System.RuntimeType)
int32_t CoreLib::System::RuntimeTypeHandle::GetArrayRank(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetToken(System.RuntimeType)
int32_t CoreLib::System::RuntimeTypeHandle::GetToken(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetMethodAt(System.RuntimeType, int)
CoreLib::System::RuntimeMethodHandleInternal CoreLib::System::RuntimeTypeHandle::GetMethodAt(CoreLib::System::RuntimeType* type, int32_t slot)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetFirstIntroducedMethod(System.RuntimeType)
CoreLib::System::RuntimeMethodHandleInternal CoreLib::System::RuntimeTypeHandle::GetFirstIntroducedMethod(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetNextIntroducedMethod(ref System.RuntimeMethodHandleInternal)
void CoreLib::System::RuntimeTypeHandle::GetNextIntroducedMethod_Ref(CoreLib::System::RuntimeMethodHandleInternal& method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetFields(System.RuntimeType, System.IntPtr*, int*)
bool CoreLib::System::RuntimeTypeHandle::GetFields(CoreLib::System::RuntimeType* type, CoreLib::System::IntPtr* result, int32_t* count)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetInterfaces(System.RuntimeType)
__array<CoreLib::System::Type*>* CoreLib::System::RuntimeTypeHandle::GetInterfaces(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetConstraints(System.RuntimeTypeHandle, System.Runtime.CompilerServices.ObjectHandleOnStack)
void CoreLib::System::RuntimeTypeHandle::GetConstraints(CoreLib::System::RuntimeTypeHandle handle, CoreLib::System::Runtime::CompilerServices::ObjectHandleOnStack types)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetGCHandle(System.RuntimeTypeHandle, System.Runtime.InteropServices.GCHandleType)
CoreLib::System::IntPtr CoreLib::System::RuntimeTypeHandle::GetGCHandle(CoreLib::System::RuntimeTypeHandle handle, CoreLib::System::Runtime::InteropServices::enum_GCHandleType type)
{
    auto gcHandle = __init<CoreLib::System::Runtime::InteropServices::GCHandle>(handle.m_type, type);
	return gcHandle.m_handle;
}

// Method : System.RuntimeTypeHandle.GetNumVirtuals(System.RuntimeType)
int32_t CoreLib::System::RuntimeTypeHandle::GetNumVirtuals(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.VerifyInterfaceIsImplemented(System.RuntimeTypeHandle, System.RuntimeTypeHandle)
void CoreLib::System::RuntimeTypeHandle::VerifyInterfaceIsImplemented(CoreLib::System::RuntimeTypeHandle handle, CoreLib::System::RuntimeTypeHandle interfaceHandle)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetInterfaceMethodImplementationSlot(System.RuntimeTypeHandle, System.RuntimeTypeHandle, System.RuntimeMethodHandleInternal)
int32_t CoreLib::System::RuntimeTypeHandle::GetInterfaceMethodImplementationSlot(CoreLib::System::RuntimeTypeHandle handle, CoreLib::System::RuntimeTypeHandle interfaceHandle, CoreLib::System::RuntimeMethodHandleInternal interfaceMethodHandle)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.IsComObject(System.RuntimeType, bool)
bool CoreLib::System::RuntimeTypeHandle::IsComObject(CoreLib::System::RuntimeType* type, bool isGenericCOM)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.IsContextful(System.RuntimeType)
bool CoreLib::System::RuntimeTypeHandle::IsContextful(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.IsInterface(System.RuntimeType)
bool CoreLib::System::RuntimeTypeHandle::IsInterface(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle._IsVisible(System.RuntimeTypeHandle)
bool CoreLib::System::RuntimeTypeHandle::_IsVisible(CoreLib::System::RuntimeTypeHandle typeHandle)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.IsSecurityCritical(System.RuntimeTypeHandle)
bool CoreLib::System::RuntimeTypeHandle::IsSecurityCritical(CoreLib::System::RuntimeTypeHandle typeHandle)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.IsSecuritySafeCritical(System.RuntimeTypeHandle)
bool CoreLib::System::RuntimeTypeHandle::IsSecuritySafeCritical(CoreLib::System::RuntimeTypeHandle typeHandle)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.IsSecurityTransparent(System.RuntimeTypeHandle)
bool CoreLib::System::RuntimeTypeHandle::IsSecurityTransparent(CoreLib::System::RuntimeTypeHandle typeHandle)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.HasProxyAttribute(System.RuntimeType)
bool CoreLib::System::RuntimeTypeHandle::HasProxyAttribute(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.IsValueType(System.RuntimeType)
bool CoreLib::System::RuntimeTypeHandle::IsValueType(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.ConstructName(System.RuntimeTypeHandle, System.TypeNameFormatFlags, System.Runtime.CompilerServices.StringHandleOnStack)
void CoreLib::System::RuntimeTypeHandle::ConstructName(CoreLib::System::RuntimeTypeHandle handle, CoreLib::System::enum_TypeNameFormatFlags formatFlags, CoreLib::System::Runtime::CompilerServices::StringHandleOnStack retString)
{
	(string**)((void*)StringHandleOnStack.m_ptr) = string::CtorCharPtr(((__runtimetype_info*)(void*)handle.m_type.m_handle)->__name);
}

// Method : System.RuntimeTypeHandle._GetUtf8Name(System.RuntimeType)
void* CoreLib::System::RuntimeTypeHandle::_GetUtf8Name(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetDeclaringType(System.RuntimeType)
CoreLib::System::RuntimeType* CoreLib::System::RuntimeTypeHandle::GetDeclaringType(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetDeclaringMethod(System.RuntimeType)
CoreLib::System::IRuntimeMethodInfo* CoreLib::System::RuntimeTypeHandle::GetDeclaringMethod(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetDefaultConstructor(System.RuntimeTypeHandle, System.Runtime.CompilerServices.ObjectHandleOnStack)
void CoreLib::System::RuntimeTypeHandle::GetDefaultConstructor(CoreLib::System::RuntimeTypeHandle handle, CoreLib::System::Runtime::CompilerServices::ObjectHandleOnStack method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetTypeByName(string, bool, bool, bool, System.Runtime.CompilerServices.StackCrawlMarkHandle, bool, System.Runtime.CompilerServices.ObjectHandleOnStack)
void CoreLib::System::RuntimeTypeHandle::GetTypeByName(string* name, bool throwOnError, bool ignoreCase, bool reflectionOnly, CoreLib::System::Runtime::CompilerServices::StackCrawlMarkHandle stackMark, bool loadTypeFromPartialName, CoreLib::System::Runtime::CompilerServices::ObjectHandleOnStack type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetTypeByNameUsingCARules(string, System.Reflection.RuntimeModule, System.Runtime.CompilerServices.ObjectHandleOnStack)
void CoreLib::System::RuntimeTypeHandle::GetTypeByNameUsingCARules(string* name, CoreLib::System::Reflection::RuntimeModule* scope, CoreLib::System::Runtime::CompilerServices::ObjectHandleOnStack type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetInstantiation(System.RuntimeTypeHandle, System.Runtime.CompilerServices.ObjectHandleOnStack, bool)
void CoreLib::System::RuntimeTypeHandle::GetInstantiation(CoreLib::System::RuntimeTypeHandle type, CoreLib::System::Runtime::CompilerServices::ObjectHandleOnStack types, bool fAsRuntimeTypeArray)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.Instantiate(System.RuntimeTypeHandle, System.IntPtr*, int, System.Runtime.CompilerServices.ObjectHandleOnStack)
void CoreLib::System::RuntimeTypeHandle::Instantiate(CoreLib::System::RuntimeTypeHandle handle, CoreLib::System::IntPtr* pInst, int32_t numGenericArgs, CoreLib::System::Runtime::CompilerServices::ObjectHandleOnStack type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.MakeArray(System.RuntimeTypeHandle, int, System.Runtime.CompilerServices.ObjectHandleOnStack)
void CoreLib::System::RuntimeTypeHandle::MakeArray(CoreLib::System::RuntimeTypeHandle handle, int32_t rank, CoreLib::System::Runtime::CompilerServices::ObjectHandleOnStack type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.MakeSZArray(System.RuntimeTypeHandle, System.Runtime.CompilerServices.ObjectHandleOnStack)
void CoreLib::System::RuntimeTypeHandle::MakeSZArray(CoreLib::System::RuntimeTypeHandle handle, CoreLib::System::Runtime::CompilerServices::ObjectHandleOnStack type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.MakeByRef(System.RuntimeTypeHandle, System.Runtime.CompilerServices.ObjectHandleOnStack)
void CoreLib::System::RuntimeTypeHandle::MakeByRef(CoreLib::System::RuntimeTypeHandle handle, CoreLib::System::Runtime::CompilerServices::ObjectHandleOnStack type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.MakePointer(System.RuntimeTypeHandle, System.Runtime.CompilerServices.ObjectHandleOnStack)
void CoreLib::System::RuntimeTypeHandle::MakePointer(CoreLib::System::RuntimeTypeHandle handle, CoreLib::System::Runtime::CompilerServices::ObjectHandleOnStack type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.IsCollectible(System.RuntimeTypeHandle)
bool CoreLib::System::RuntimeTypeHandle::IsCollectible(CoreLib::System::RuntimeTypeHandle handle)
{
    return true;
}

// Method : System.RuntimeTypeHandle.HasInstantiation(System.RuntimeType)
bool CoreLib::System::RuntimeTypeHandle::HasInstantiation(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetGenericTypeDefinition(System.RuntimeTypeHandle, System.Runtime.CompilerServices.ObjectHandleOnStack)
void CoreLib::System::RuntimeTypeHandle::GetGenericTypeDefinition(CoreLib::System::RuntimeTypeHandle type, CoreLib::System::Runtime::CompilerServices::ObjectHandleOnStack retType)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.IsGenericTypeDefinition(System.RuntimeType)
bool CoreLib::System::RuntimeTypeHandle::IsGenericTypeDefinition(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.IsGenericVariable(System.RuntimeType)
bool CoreLib::System::RuntimeTypeHandle::IsGenericVariable(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.GetGenericVariableIndex(System.RuntimeType)
int32_t CoreLib::System::RuntimeTypeHandle::GetGenericVariableIndex(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.ContainsGenericVariables(System.RuntimeType)
bool CoreLib::System::RuntimeTypeHandle::ContainsGenericVariables(CoreLib::System::RuntimeType* handle)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle.SatisfiesConstraints(System.RuntimeType, System.IntPtr*, int, System.IntPtr*, int, System.RuntimeType)
bool CoreLib::System::RuntimeTypeHandle::SatisfiesConstraints(CoreLib::System::RuntimeType* paramType, CoreLib::System::IntPtr* pTypeContext, int32_t typeContextLength, CoreLib::System::IntPtr* pMethodContext, int32_t methodContextLength, CoreLib::System::RuntimeType* toType)
{
    throw 0xC000C000;
}

// Method : System.RuntimeTypeHandle._GetMetadataImport(System.RuntimeType)
CoreLib::System::IntPtr CoreLib::System::RuntimeTypeHandle::_GetMetadataImport(CoreLib::System::RuntimeType* type)
{
    throw 0xC000C000;
}
