#include "CoreLib.h"

// Method : System.Buffer.BlockCopy(System.Array, int, System.Array, int, int)
void CoreLib::System::Buffer::BlockCopy(CoreLib::System::Array* src, int32_t srcOffset, CoreLib::System::Array* dst, int32_t dstOffset, int32_t count)
{
	CoreLib::System::Array::Copy(src, srcOffset, dst, dstOffset, count, true);
}

// Method : System.Buffer.InternalBlockCopy(System.Array, int, System.Array, int, int)
void CoreLib::System::Buffer::InternalBlockCopy(CoreLib::System::Array* src, int32_t srcOffsetBytes, CoreLib::System::Array* dst, int32_t dstOffsetBytes, int32_t byteCount)
{
	if (byteCount == 0)
	{
		return;
	}

	if (src == nullptr || dst == nullptr)
	{
		throw __new<CoreLib::System::ArgumentNullException>();
	}

	if (byteCount < 0 || srcOffsetBytes < 0 || dstOffsetBytes < 0)
	{
		throw __new<CoreLib::System::InvalidOperationException>();
	}

	CoreLib::System::TypedReference elemref;

	int32_t index = 0;
	src->InternalGetReference(static_cast<void*>(&elemref), 1, &index);
	auto pSrc = (int8_t*) (void*)elemref.Value;

	dst->InternalGetReference(static_cast<void*>(&elemref), 1, &index);
	auto pDest = (int8_t*) (void*)elemref.Value;

	std::memcpy(pDest + dstOffsetBytes, pSrc + srcOffsetBytes, byteCount);
}

// Method : System.Buffer.IsPrimitiveTypeArray(System.Array)
bool CoreLib::System::Buffer::IsPrimitiveTypeArray(CoreLib::System::Array* array)
{
	if (array == nullptr)
	{
		throw __new<CoreLib::System::ArgumentNullException>();
	}

	return array->__is_primitive_type_array();
}

// Method : System.Buffer._GetByte(System.Array, int)
uint8_t CoreLib::System::Buffer::_GetByte(CoreLib::System::Array* array, int32_t index)
{
	if (array == nullptr)
	{
		throw __new<CoreLib::System::ArgumentNullException>();
	}

	CoreLib::System::TypedReference elemref;
	array->InternalGetReference(static_cast<void*>(&elemref), 1, &index);
	return *((int8_t*)(void*)elemref.Value + index);
}

// Method : System.Buffer._SetByte(System.Array, int, byte)
void CoreLib::System::Buffer::_SetByte(CoreLib::System::Array* array, int32_t index, uint8_t value)
{
	if (array == nullptr)
	{
		throw __new<CoreLib::System::ArgumentNullException>();
	}

	CoreLib::System::TypedReference elemref;
	array->InternalGetReference(static_cast<void*>(&elemref), 1, &index);
	*((int8_t*)(void*)elemref.Value + index) = value;
}

// Method : System.Buffer._ByteLength(System.Array)
int32_t CoreLib::System::Buffer::_ByteLength(CoreLib::System::Array* array)
{
	if (array == nullptr)
	{
		throw __new<CoreLib::System::ArgumentNullException>();
	}

	return array->get_Length() * array->__array_element_size();
}

// Method : System.Buffer.__Memmove(byte*, byte*, uint)
void CoreLib::System::Buffer::__Memmove(uint8_t* dest, uint8_t* src, uint32_t len)
{
	if (len == 0)
	{
		return;
	}

	if (src == nullptr || dest == nullptr)
	{
		throw __new<CoreLib::System::ArgumentNullException>();
	}

	std::memcpy(dest, src, len);
}
