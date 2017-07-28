#include "System.Private.CoreLib.h"

namespace CoreLib { namespace System { 
    namespace _ = ::CoreLib::System;
    // Method : System.TypedReference.InternalMakeTypedReference(void*, object, System.IntPtr[], System.RuntimeType)
    void TypedReference::InternalMakeTypedReference(void* result, object* target, __array<_::IntPtr>* flds, _::RuntimeType* lastFieldType)
    {
        throw 3221274624U;
    }
    
    // Method : System.TypedReference.InternalToObject(void*)
    object* TypedReference::InternalToObject(void* value)
    {
		auto trPtr = (_::TypedReference*)value;
		return ((__methods_table*)(void*)(trPtr->Type))->__box_ref((void*)(trPtr->Value));
    }
    
    // Method : System.TypedReference.InternalSetTypedReference(void*, object)
    void TypedReference::InternalSetTypedReference(void* target, object* value)
    {
		auto trPtr = (_::TypedReference*)target;
		trPtr->Value.INTPTR_VALUE_FIELD = (void*)value;
	}

}}

namespace CoreLib { namespace System { 
    namespace _ = ::CoreLib::System;
}}
