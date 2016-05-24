#include "CoreLib.h"

// Method : System.Array.InternalGetReference(void*, int, int*)
void CoreLib::System::Array::InternalGetReference(void* elemRef, int32_t rank, int32_t* pIndices)
{
    throw 0xC000C000;
}

// Method : System.Array.InternalSetValue(void*, object)
void CoreLib::System::Array::InternalSetValue(void* target, object* value)
{
    throw 0xC000C000;
}

// Method : System.Array.GetUpperBound(int)
int32_t CoreLib::System::Array::GetUpperBound(int32_t dimension)
{
    throw 0xC000C000;
}

// Method : System.Array.GetLowerBound(int)
int32_t CoreLib::System::Array::GetLowerBound(int32_t dimension)
{
    throw 0xC000C000;
}

// Method : System.Array.GetLength(int)
int32_t CoreLib::System::Array::GetLength(int32_t dimension)
{
    throw 0xC000C000;
}

// Method : System.Array.Rank.get
int32_t CoreLib::System::Array::get_Rank()
{
    throw 0xC000C000;
}

// Method : System.Array.Length.get
int32_t CoreLib::System::Array::get_Length()
{
    throw 0xC000C000;
}

// Method : System.Array.Copy(System.Array, int, System.Array, int, int, bool)
void CoreLib::System::Array::Copy(CoreLib::System::Array* sourceArray, int32_t sourceIndex, CoreLib::System::Array* destinationArray, int32_t destinationIndex, int32_t length, bool reliable)
{
	if (length == 0)
	{
		return;
	}

	if (sourceArray == nullptr || destinationArray == nullptr)
	{
		throw __new<CoreLib::System::ArgumentNullException>();
	}

	if (length < 0)
	{
		throw __new<CoreLib::System::InvalidOperationException>();
	}

	if (sourceIndex < 0 || destinationIndex < 0)
	{
		throw __new<CoreLib::System::IndexOutOfRangeException>();
	}

	if (sourceIndex + length > sourceArray->get_Length() || destinationIndex + length > destinationArray->get_Length())
	{
		throw __new<CoreLib::System::IndexOutOfRangeException>();
	}

	CoreLib::System::TypedReference elemref;

	int32_t index = sourceIndex;
	sourceArray->InternalGetReference(static_cast<void*>(&elemref), 1, &index);
	auto pSrc = (int8_t*) (void*)elemref.Value;

	index = destinationIndex;
	destinationArray->InternalGetReference(static_cast<void*>(&elemref), 1, &index);
	auto pDest = (int8_t*) (void*)elemref.Value;

	auto elementSize = sourceArray->__array_element_size();
	std::memcpy(pDest, pSrc, length * elementSize);
}
