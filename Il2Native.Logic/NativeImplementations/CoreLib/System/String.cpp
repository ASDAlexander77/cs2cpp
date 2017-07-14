#include "CoreLib.h"

namespace CoreLib {
	namespace System {

		namespace _ = ::CoreLib::System;

		// Method : string.String(char[], int, int)
		void String::_ctor(__array<char16_t>* value, int32_t startIndex, int32_t length)
		{
			throw 0xC000C000;
		}

		// Method : string.String(char[])
		void String::_ctor(__array<char16_t>* value)
		{
			throw 0xC000C000;
		}

		// Method : string.String(char, int)
		void String::_ctor(char16_t c, int32_t count)
		{
			throw 0xC000C000;
		}

		// Method : string.String(char*, int, int)
		void String::_ctor(char16_t* src, int32_t startIndex, int32_t length)
		{
			throw 0xC000C000;
		}

		// Method : string.String(sbyte*)
		void String::_ctor(int8_t* src)
		{
			throw 0xC000C000;
		}

		// Method : string.String(sbyte*, int, int)
		void String::_ctor(int8_t* src, int32_t startIndex, int32_t length)
		{
			throw 0xC000C000;
		}

		// Method : string.String(sbyte*, int, int, System.Text.Encoding)
		void String::_ctor(int8_t* src, int32_t startIndex, int32_t length, _::Text::Encoding* enc)
		{
			throw 0xC000C000;
		}

		// Method : string.FastAllocateString(int)
		string* String::FastAllocateString(int32_t length)
		{
			auto size = sizeof(string) + (length + 1) * sizeof(char16_t);
#ifdef NDEBUG
			auto str = new ((size_t)size) string;
#else
			auto str = new ((size_t)size, __FILE__, __LINE__) string;
#endif
			str->m_stringLength = length;
			return str;
		}

		// Method : string.LastIndexOf(char, int)
		int32_t String::LastIndexOf(char16_t value, int32_t startIndex)
		{
			throw 0xC000C000;
		}

		// Method : string.LastIndexOf(char, int, int)
		int32_t String::LastIndexOf(char16_t value, int32_t startIndex, int32_t count)
		{
			throw 0xC000C000;
		}

		// Method : string.LastIndexOf(string, System.StringComparison)
		int32_t String::LastIndexOf(string* value, _::StringComparison__enum sc)
		{
			throw 0xC000C000;
		}

		// Method : string.LastIndexOf(string, int, System.StringComparison)
		int32_t String::LastIndexOf(string* value, int32_t startIndex, _::StringComparison__enum sc)
		{
			throw 0xC000C000;
		}

		// Method : string.LastIndexOf(string, int, int)
		int32_t String::LastIndexOf(string* value, int32_t startIndex, int32_t count)
		{
			throw 0xC000C000;
		}

		// Method : string.LastIndexOfAny(char[])
		int32_t String::LastIndexOfAny(__array<char16_t>* anyOf)
		{
			throw 0xC000C000;
		}

		// Method : string.LastIndexOfAny(char[], int)
		int32_t String::LastIndexOfAny(__array<char16_t>* anyOf, int32_t startIndex)
		{
			throw 0xC000C000;
		}

		// Method : string.LastIndexOfAny(char[], int, int)
		int32_t String::LastIndexOfAny(__array<char16_t>* anyOf, int32_t startIndex, int32_t count)
		{
			throw 0xC000C000;
		}
	}
}