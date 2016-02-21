#include "CoreLib.h"

// Method : System.RuntimeMethodHandle.GetFunctionPointer(System.RuntimeMethodHandleInternal)
CoreLib::System::IntPtr CoreLib::System::RuntimeMethodHandle::GetFunctionPointer(CoreLib::System::RuntimeMethodHandleInternal handle)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.CheckLinktimeDemands(System.IRuntimeMethodInfo, System.Reflection.RuntimeModule, bool)
void CoreLib::System::RuntimeMethodHandle::CheckLinktimeDemands(CoreLib::System::IRuntimeMethodInfo* method, CoreLib::System::Reflection::RuntimeModule* module, bool isDecoratedTargetSecurityTransparent)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.IsCAVisibleFromDecoratedType(System.RuntimeTypeHandle, System.IRuntimeMethodInfo, System.RuntimeTypeHandle, System.Reflection.RuntimeModule)
bool CoreLib::System::RuntimeMethodHandle::IsCAVisibleFromDecoratedType(CoreLib::System::RuntimeTypeHandle attrTypeHandle, CoreLib::System::IRuntimeMethodInfo* attrCtor, CoreLib::System::RuntimeTypeHandle sourceTypeHandle, CoreLib::System::Reflection::RuntimeModule* sourceModule)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle._GetCurrentMethod(ref System.Threading.StackCrawlMark)
CoreLib::System::IRuntimeMethodInfo* CoreLib::System::RuntimeMethodHandle::_GetCurrentMethod_Ref(CoreLib::System::Threading::enum_StackCrawlMark& stackMark)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.GetAttributes(System.RuntimeMethodHandleInternal)
CoreLib::System::Reflection::enum_MethodAttributes CoreLib::System::RuntimeMethodHandle::GetAttributes(CoreLib::System::RuntimeMethodHandleInternal method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.GetImplAttributes(System.IRuntimeMethodInfo)
CoreLib::System::Reflection::enum_MethodImplAttributes CoreLib::System::RuntimeMethodHandle::GetImplAttributes(CoreLib::System::IRuntimeMethodInfo* method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.ConstructInstantiation(System.IRuntimeMethodInfo, System.TypeNameFormatFlags, System.Runtime.CompilerServices.StringHandleOnStack)
void CoreLib::System::RuntimeMethodHandle::ConstructInstantiation(CoreLib::System::IRuntimeMethodInfo* method, CoreLib::System::enum_TypeNameFormatFlags format, CoreLib::System::Runtime::CompilerServices::StringHandleOnStack retString)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.GetDeclaringType(System.RuntimeMethodHandleInternal)
CoreLib::System::RuntimeType* CoreLib::System::RuntimeMethodHandle::GetDeclaringType(CoreLib::System::RuntimeMethodHandleInternal method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.GetSlot(System.RuntimeMethodHandleInternal)
int32_t CoreLib::System::RuntimeMethodHandle::GetSlot(CoreLib::System::RuntimeMethodHandleInternal method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.GetMethodDef(System.IRuntimeMethodInfo)
int32_t CoreLib::System::RuntimeMethodHandle::GetMethodDef(CoreLib::System::IRuntimeMethodInfo* method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.GetName(System.RuntimeMethodHandleInternal)
string* CoreLib::System::RuntimeMethodHandle::GetName(CoreLib::System::RuntimeMethodHandleInternal method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle._GetUtf8Name(System.RuntimeMethodHandleInternal)
void* CoreLib::System::RuntimeMethodHandle::_GetUtf8Name(CoreLib::System::RuntimeMethodHandleInternal method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.MatchesNameHash(System.RuntimeMethodHandleInternal, uint)
bool CoreLib::System::RuntimeMethodHandle::MatchesNameHash(CoreLib::System::RuntimeMethodHandleInternal method, uint32_t hash)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.InvokeMethod(object, object[], System.Signature, bool)
object* CoreLib::System::RuntimeMethodHandle::InvokeMethod(object* target, __array<object*>* arguments, CoreLib::System::Signature* sig, bool constructor)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.GetSpecialSecurityFlags(System.IRuntimeMethodInfo)
uint32_t CoreLib::System::RuntimeMethodHandle::GetSpecialSecurityFlags(CoreLib::System::IRuntimeMethodInfo* method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.PerformSecurityCheck(object, System.RuntimeMethodHandleInternal, System.RuntimeType, uint)
void CoreLib::System::RuntimeMethodHandle::PerformSecurityCheck(object* obj, CoreLib::System::RuntimeMethodHandleInternal method, CoreLib::System::RuntimeType* parent, uint32_t invocationFlags)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle._IsTokenSecurityTransparent(System.Reflection.RuntimeModule, int)
bool CoreLib::System::RuntimeMethodHandle::_IsTokenSecurityTransparent(CoreLib::System::Reflection::RuntimeModule* module, int32_t metaDataToken)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle._IsSecurityCritical(System.IRuntimeMethodInfo)
bool CoreLib::System::RuntimeMethodHandle::_IsSecurityCritical(CoreLib::System::IRuntimeMethodInfo* method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle._IsSecuritySafeCritical(System.IRuntimeMethodInfo)
bool CoreLib::System::RuntimeMethodHandle::_IsSecuritySafeCritical(CoreLib::System::IRuntimeMethodInfo* method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle._IsSecurityTransparent(System.IRuntimeMethodInfo)
bool CoreLib::System::RuntimeMethodHandle::_IsSecurityTransparent(CoreLib::System::IRuntimeMethodInfo* method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.GetMethodInstantiation(System.RuntimeMethodHandleInternal, System.Runtime.CompilerServices.ObjectHandleOnStack, bool)
void CoreLib::System::RuntimeMethodHandle::GetMethodInstantiation(CoreLib::System::RuntimeMethodHandleInternal method, CoreLib::System::Runtime::CompilerServices::ObjectHandleOnStack types, bool fAsRuntimeTypeArray)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.HasMethodInstantiation(System.RuntimeMethodHandleInternal)
bool CoreLib::System::RuntimeMethodHandle::HasMethodInstantiation(CoreLib::System::RuntimeMethodHandleInternal method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.GetStubIfNeeded(System.RuntimeMethodHandleInternal, System.RuntimeType, System.RuntimeType[])
CoreLib::System::RuntimeMethodHandleInternal CoreLib::System::RuntimeMethodHandle::GetStubIfNeeded(CoreLib::System::RuntimeMethodHandleInternal method, CoreLib::System::RuntimeType* declaringType, __array<CoreLib::System::RuntimeType*>* methodInstantiation)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.GetMethodFromCanonical(System.RuntimeMethodHandleInternal, System.RuntimeType)
CoreLib::System::RuntimeMethodHandleInternal CoreLib::System::RuntimeMethodHandle::GetMethodFromCanonical(CoreLib::System::RuntimeMethodHandleInternal method, CoreLib::System::RuntimeType* declaringType)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.IsGenericMethodDefinition(System.RuntimeMethodHandleInternal)
bool CoreLib::System::RuntimeMethodHandle::IsGenericMethodDefinition(CoreLib::System::RuntimeMethodHandleInternal method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.IsTypicalMethodDefinition(System.IRuntimeMethodInfo)
bool CoreLib::System::RuntimeMethodHandle::IsTypicalMethodDefinition(CoreLib::System::IRuntimeMethodInfo* method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.GetTypicalMethodDefinition(System.IRuntimeMethodInfo, System.Runtime.CompilerServices.ObjectHandleOnStack)
void CoreLib::System::RuntimeMethodHandle::GetTypicalMethodDefinition(CoreLib::System::IRuntimeMethodInfo* method, CoreLib::System::Runtime::CompilerServices::ObjectHandleOnStack outMethod)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.StripMethodInstantiation(System.IRuntimeMethodInfo, System.Runtime.CompilerServices.ObjectHandleOnStack)
void CoreLib::System::RuntimeMethodHandle::StripMethodInstantiation(CoreLib::System::IRuntimeMethodInfo* method, CoreLib::System::Runtime::CompilerServices::ObjectHandleOnStack outMethod)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.IsDynamicMethod(System.RuntimeMethodHandleInternal)
bool CoreLib::System::RuntimeMethodHandle::IsDynamicMethod(CoreLib::System::RuntimeMethodHandleInternal method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.Destroy(System.RuntimeMethodHandleInternal)
void CoreLib::System::RuntimeMethodHandle::Destroy(CoreLib::System::RuntimeMethodHandleInternal method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.GetResolver(System.RuntimeMethodHandleInternal)
CoreLib::System::Resolver* CoreLib::System::RuntimeMethodHandle::GetResolver(CoreLib::System::RuntimeMethodHandleInternal method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.GetCallerType(System.Runtime.CompilerServices.StackCrawlMarkHandle, System.Runtime.CompilerServices.ObjectHandleOnStack)
void CoreLib::System::RuntimeMethodHandle::GetCallerType(CoreLib::System::Runtime::CompilerServices::StackCrawlMarkHandle stackMark, CoreLib::System::Runtime::CompilerServices::ObjectHandleOnStack retType)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.IsConstructor(System.RuntimeMethodHandleInternal)
bool CoreLib::System::RuntimeMethodHandle::IsConstructor(CoreLib::System::RuntimeMethodHandleInternal method)
{
    throw 0xC000C000;
}

// Method : System.RuntimeMethodHandle.GetLoaderAllocator(System.RuntimeMethodHandleInternal)
CoreLib::System::Reflection::LoaderAllocator* CoreLib::System::RuntimeMethodHandle::GetLoaderAllocator(CoreLib::System::RuntimeMethodHandleInternal method)
{
    throw 0xC000C000;
}
