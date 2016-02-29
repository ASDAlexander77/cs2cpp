#include "CoreLib.h"

// Method : System.Buffer.BlockCopy(System.Array, int, System.Array, int, int)
void CoreLib::System::Buffer::BlockCopy(CoreLib::System::Array* src, int32_t srcOffset, CoreLib::System::Array* dst, int32_t dstOffset, int32_t count)
{
    throw 0xC000C000;
}

// Method : System.Buffer.InternalBlockCopy(System.Array, int, System.Array, int, int)
void CoreLib::System::Buffer::InternalBlockCopy(CoreLib::System::Array* src, int32_t srcOffsetBytes, CoreLib::System::Array* dst, int32_t dstOffsetBytes, int32_t byteCount)
{
	if (byteCount <= 0)
	{
		return;
	}

    CoreLib::System::TypedReference elemref;

	int32_t index = 0;
    src->InternalGetReference(static_cast<void*>(&elemref), 1, &index);
	auto pSrc = (void*)elemref.Value

    dst->InternalGetReference(static_cast<void*>(&elemref), 1, &index);
	auto pDest = (void*)elemref.Value

	std::memcpy(pDest + dstOffsetBytes, pSrc + srcOffsetBytes, byteCount);
}

// Method : System.Buffer.IsPrimitiveTypeArray(System.Array)
bool CoreLib::System::Buffer::IsPrimitiveTypeArray(CoreLib::System::Array* array)
{
    throw 0xC000C000;
}

// Method : System.Buffer._GetByte(System.Array, int)
uint8_t CoreLib::System::Buffer::_GetByte(CoreLib::System::Array* array, int32_t index)
{
    throw 0xC000C000;
}

// Method : System.Buffer._SetByte(System.Array, int, byte)
void CoreLib::System::Buffer::_SetByte(CoreLib::System::Array* array, int32_t index, uint8_t value)
{
    throw 0xC000C000;
}

// Method : System.Buffer._ByteLength(System.Array)
int32_t CoreLib::System::Buffer::_ByteLength(CoreLib::System::Array* array)
{
    throw 0xC000C000;
}

// Method : System.Buffer.__Memmove(byte*, byte*, uint)
void CoreLib::System::Buffer::__Memmove(uint8_t* dest, uint8_t* src, uint32_t len)
{
    throw 0xC000C000;
}
