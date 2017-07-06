#include "CoreLib.h"

namespace CoreLib {
	namespace System {

		namespace _ = ::CoreLib::System;

		// Method : System.TypedReference.InternalToObject(void*)
		object* TypedReference::InternalToObject(void* value)
		{
			auto trPtr = (_::TypedReference*)value;
			return ((__methods_table*)(void*)(trPtr->Type))->__box_ref((void*)(trPtr->Value));
		}
	}
}
