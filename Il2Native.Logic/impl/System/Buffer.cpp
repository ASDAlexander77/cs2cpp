#include "CoreLib.h"

namespace CoreLib {
	namespace System {
		namespace _ = ::CoreLib::System;

		// Method : System.Buffer.BlockCopy(System.Array, int, System.Array, int, int)
		void Buffer::BlockCopy(_::Array* src, int32_t srcOffset, _::Array* dst, int32_t dstOffset, int32_t count)
		{
			InternalBlockCopy(src, srcOffset, dst, dstOffset, count);
		}

		// Method : System.Buffer.InternalBlockCopy(System.Array, int, System.Array, int, int)
		void Buffer::InternalBlockCopy(_::Array* src, int32_t srcOffsetBytes, _::Array* dst, int32_t dstOffsetBytes, int32_t byteCount)
		{
			if (byteCount == 0)
			{
				return;
			}

			if (src == nullptr || dst == nullptr)
			{
				throw __new<_::ArgumentNullException>();
			}

			if (byteCount < 0 || srcOffsetBytes < 0 || dstOffsetBytes < 0)
			{
				throw __new<_::ArgumentException>();
			}

			if (byteCount > (_ByteLength(src) - srcOffsetBytes) || byteCount > (_ByteLength(dst) - dstOffsetBytes))
			{
				throw __new<_::ArgumentException>();
			}

			if (!Buffer::IsPrimitiveTypeArray(src) || !Buffer::IsPrimitiveTypeArray(dst))
			{
				throw __new<_::ArgumentException>();
			}

			_::TypedReference elemref;

			auto zero_index = 0;
			src->InternalGetReference(static_cast<void*>(&elemref), 1, &zero_index);
			auto pSrc = (int8_t*)(void*)elemref.Value;

			dst->InternalGetReference(static_cast<void*>(&elemref), 1, &zero_index);
			auto pDest = (int8_t*)(void*)elemref.Value;

			std::memcpy(pDest + dstOffsetBytes, pSrc + srcOffsetBytes, byteCount);
		}

		// Method : System.Buffer.IsPrimitiveTypeArray(System.Array)
		bool Buffer::IsPrimitiveTypeArray(_::Array* array)
		{
			if (array == nullptr)
			{
				throw __new<_::ArgumentNullException>();
			}

			return array->__is_primitive_type_array();
		}

		// Method : System.Buffer._GetByte(System.Array, int)
		uint8_t Buffer::_GetByte(_::Array* array, int32_t index)
		{
			if (array == nullptr)
			{
				throw __new<_::ArgumentNullException>();
			}

			_::TypedReference elemref;
			auto zero_index = 0;
			array->InternalGetReference(static_cast<void*>(&elemref), 1, &zero_index);
			return *((int8_t*)(void*)elemref.Value + index);
		}

		// Method : System.Buffer._SetByte(System.Array, int, byte)
		void Buffer::_SetByte(_::Array* array, int32_t index, uint8_t value)
		{
			if (array == nullptr)
			{
				throw __new<_::ArgumentNullException>();
			}

			_::TypedReference elemref;
			auto zero_index = 0;
			array->InternalGetReference(static_cast<void*>(&elemref), 1, &zero_index);
			*((int8_t*)(void*)elemref.Value + index) = value;
		}

		// Method : System.Buffer._ByteLength(System.Array)
		int32_t Buffer::_ByteLength(_::Array* array)
		{
			if (array == nullptr)
			{
				throw __new<_::ArgumentNullException>();
			}

			return array->get_Length() * array->__array_element_size();
		}

#ifdef CORELIB_ONLY
		// Method : System.Buffer.__Memmove(byte*, byte*, uint)
		void Buffer::__Memmove(uint8_t* dest, uint8_t* src, uint32_t len)
		{
			if (len == 0)
			{
				return;
			}

			if (src == nullptr || dest == nullptr)
			{
				throw __new<_::ArgumentNullException>();
			}

			std::memcpy(dest, src, len);
		}
#endif
	}
}