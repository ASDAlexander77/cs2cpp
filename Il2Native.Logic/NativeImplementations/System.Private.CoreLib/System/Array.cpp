#include "System.Private.CoreLib.h"

namespace CoreLib { namespace System { 
    namespace _ = ::CoreLib::System;
    // Method : System.Array.InternalCreate(void*, int, int*, int*)
    _::Array* Array::InternalCreate(void* elementType, int32_t rank, int32_t* pLengths, int32_t* pLowerBounds)
    {
        throw 3221274624U;
    }
    
    // Method : System.Array.Copy(System.Array, int, System.Array, int, int, bool)
    void Array::Copy(_::Array* sourceArray, int32_t sourceIndex, _::Array* destinationArray, int32_t destinationIndex, int32_t length, bool reliable)
    {
		if (length == 0)
		{
			return;
		}

		if (sourceArray == nullptr || destinationArray == nullptr)
		{
			throw __new<_::ArgumentNullException>();
		}

		if (length < 0)
		{
			throw __new<_::InvalidOperationException>();
		}

		if (sourceIndex < 0 || destinationIndex < 0)
		{
			throw __new<_::IndexOutOfRangeException>();
		}

		if (sourceIndex + length > sourceArray->get_Length() || destinationIndex + length > destinationArray->get_Length())
		{
			throw __new<_::IndexOutOfRangeException>();
		}

		_::TypedReference elemref;

		int32_t index = sourceIndex;
		sourceArray->InternalGetReference(static_cast<void*>(&elemref), 1, &index);
		auto pSrc = (int8_t*)(void*)elemref.Value;

		index = destinationIndex;
		destinationArray->InternalGetReference(static_cast<void*>(&elemref), 1, &index);
		auto pDest = (int8_t*)(void*)elemref.Value;

		auto elementSize = sourceArray->__array_element_size();
		std::memcpy(pDest, pSrc, length * elementSize);
	}
    
    // Method : System.Array.Clear(System.Array, int, int)
    void Array::Clear(_::Array* array, int32_t index, int32_t length)
    {
		if (length == 0)
		{
			return;
		}

		if (array == nullptr)
		{
			throw __new<_::ArgumentNullException>();
		}

		if (length < 0)
		{
			throw __new<_::InvalidOperationException>();
		}

		if (index < 0)
		{
			throw __new<_::IndexOutOfRangeException>();
		}

		if (index + length > array->get_Length())
		{
			throw __new<_::IndexOutOfRangeException>();
		}

		_::TypedReference elemref;

		array->InternalGetReference(static_cast<void*>(&elemref), 1, &index);
		auto pSrc = (int8_t*)(void*)elemref.Value;

		auto elementSize = array->__array_element_size();
		std::memset(pSrc, 0, length * elementSize);
	}
    
    // Method : System.Array.InternalGetReference(void*, int, int*)
    void Array::InternalGetReference(void* elemRef, int32_t rank, int32_t* pIndices)
    {
        throw 3221274624U;
    }
    
    // Method : System.Array.InternalSetValue(void*, object)
    void Array::InternalSetValue(void* target, object* value)
    {
		if (target == nullptr)
		{
			throw __new<_::ArgumentNullException>(u"target"_s);
		}

		auto typedRef = reinterpret_cast<_::TypedReference*>(target);

		try
		{
			((__methods_table*)(void*)(typedRef->Type))->__unbox_to((void*)typedRef->Value, value);
		}
		catch (_::InvalidCastException*)
		{
			throw __new<_::ArgumentException>();
		}
	}
    
    // Method : System.Array.Length.get
    int32_t Array::get_Length()
    {
        throw 3221274624U;
    }
    
    // Method : System.Array.LongLength.get
    int64_t Array::get_LongLength()
    {
        throw 3221274624U;
    }
    
    // Method : System.Array.GetLength(int)
    int32_t Array::GetLength(int32_t dimension)
    {
        throw 3221274624U;
    }
    
    // Method : System.Array.Rank.get
    int32_t Array::get_Rank()
    {
        throw 3221274624U;
    }
    
    // Method : System.Array.GetUpperBound(int)
    int32_t Array::GetUpperBound(int32_t dimension)
    {
        throw 3221274624U;
    }
    
    // Method : System.Array.GetLowerBound(int)
    int32_t Array::GetLowerBound(int32_t dimension)
    {
        throw 3221274624U;
    }
    
    // Method : System.Array.GetDataPtrOffsetInternal()
    int32_t Array::GetDataPtrOffsetInternal()
    {
        throw 3221274624U;
    }
    
    // Method : System.Array.TrySZBinarySearch(System.Array, int, int, object, out int)
    bool Array::TrySZBinarySearch_Out(_::Array* sourceArray, int32_t sourceIndex, int32_t count, object* value, int32_t& retVal)
    {
        throw 3221274624U;
    }
    
    // Method : System.Array.TrySZIndexOf(System.Array, int, int, object, out int)
    bool Array::TrySZIndexOf_Out(_::Array* sourceArray, int32_t sourceIndex, int32_t count, object* value, int32_t& retVal)
    {
        throw 3221274624U;
    }
    
    // Method : System.Array.TrySZLastIndexOf(System.Array, int, int, object, out int)
    bool Array::TrySZLastIndexOf_Out(_::Array* sourceArray, int32_t sourceIndex, int32_t count, object* value, int32_t& retVal)
    {
        throw 3221274624U;
    }
    
    // Method : System.Array.TrySZReverse(System.Array, int, int)
    bool Array::TrySZReverse(_::Array* array, int32_t index, int32_t count)
    {
        throw 3221274624U;
    }
    
    // Method : System.Array.TrySZSort(System.Array, System.Array, int, int)
    bool Array::TrySZSort(_::Array* keys, _::Array* items, int32_t left, int32_t right)
    {
        throw 3221274624U;
    }
    
    // Method : System.Array.Initialize()
    void Array::Initialize()
    {
        throw 3221274624U;
    }

}}

namespace CoreLib { namespace System { 
    namespace _ = ::CoreLib::System;
}}
