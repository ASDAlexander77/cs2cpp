#include "CoreLib.h"

// Method : System.TypedReference.InternalToObject(void*)
object* CoreLib::System::TypedReference::InternalToObject(void* value)
{
	auto trPtr = (CoreLib::System::TypedReference*)value;
    ((__methods_table*)(void*)(trPtr->Type))->__unbox_ref((void*)(trPtr->Value));
}
