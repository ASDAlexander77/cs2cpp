#include "CoreLib.h"

// Method : System.Reflection.CustomAttribute._ParseAttributeUsageAttribute(System.IntPtr, int, out int, out bool, out bool)
void CoreLib::System::Reflection::CustomAttribute::_ParseAttributeUsageAttribute_Out_Out_Out(CoreLib::System::IntPtr pCa, int32_t cCa, int32_t& targets, bool& inherited, bool& allowMultiple)
{
    throw 0xC000C000;
}

// Method : System.Reflection.CustomAttribute._CreateCaObject(System.Reflection.RuntimeModule, System.IRuntimeMethodInfo, byte**, byte*, int*)
object* CoreLib::System::Reflection::CustomAttribute::_CreateCaObject(CoreLib::System::Reflection::RuntimeModule* pModule, CoreLib::System::IRuntimeMethodInfo* pCtor, uint8_t** ppBlob, uint8_t* pEndBlob, int32_t* pcNamedArgs)
{
    throw 0xC000C000;
}

// Method : System.Reflection.CustomAttribute._GetPropertyOrFieldData(System.Reflection.RuntimeModule, byte**, byte*, out string, out bool, out System.RuntimeType, out object)
void CoreLib::System::Reflection::CustomAttribute::_GetPropertyOrFieldData_Out_Out_Out_Out(CoreLib::System::Reflection::RuntimeModule* pModule, uint8_t** ppBlobStart, uint8_t* pBlobEnd, string*& name, bool& bIsProperty, CoreLib::System::RuntimeType*& type, object*& value)
{
    throw 0xC000C000;
}
