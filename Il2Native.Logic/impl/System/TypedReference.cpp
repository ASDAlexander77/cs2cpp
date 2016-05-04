#include "CoreLib.h"

// Method : System.TypedReference.InternalToObject(void*)
object* CoreLib::System::TypedReference::InternalToObject(void* value)
{
	auto trPtr = (CoreLib::System::TypedReference*)value;
    return ((__methods_table*)(void*)(trPtr->Type))->__box_ref((void*)(trPtr->Value));
}
