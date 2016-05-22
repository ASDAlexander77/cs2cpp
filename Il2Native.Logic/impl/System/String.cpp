#include "CoreLib.h"

// Method : string.String(char[], int, int)
void CoreLib::System::String::_ctor(__array<char16_t>* value, int32_t startIndex, int32_t length)
{
    throw 0xC000C000;
}

// Method : string.String(char[])
void CoreLib::System::String::_ctor(__array<char16_t>* value)
{
    throw 0xC000C000;
}

// Method : string.String(char, int)
void CoreLib::System::String::_ctor(char16_t c, int32_t count)
{
    throw 0xC000C000;
}

// Method : string.String(char*, int, int)
void CoreLib::System::String::_ctor(char16_t* src, int32_t startIndex, int32_t length)
{
    throw 0xC000C000;
}

// Method : string.String(sbyte*)
void CoreLib::System::String::_ctor(int8_t* src)
{
    throw 0xC000C000;
}

// Method : string.String(sbyte*, int, int)
void CoreLib::System::String::_ctor(int8_t* src, int32_t startIndex, int32_t length)
{
    throw 0xC000C000;
}

// Method : string.String(sbyte*, int, int, System.Text.Encoding)
void CoreLib::System::String::_ctor(int8_t* src, int32_t startIndex, int32_t length, CoreLib::System::Text::Encoding* enc)
{
    throw 0xC000C000;
}

// Method : string.FastAllocateString(int)
string* CoreLib::System::String::FastAllocateString(int32_t length)
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
int32_t CoreLib::System::String::LastIndexOf(char16_t value, int32_t startIndex)
{
    throw 0xC000C000;
}

// Method : string.LastIndexOf(char, int, int)
int32_t CoreLib::System::String::LastIndexOf(char16_t value, int32_t startIndex, int32_t count)
{
    throw 0xC000C000;
}

// Method : string.LastIndexOf(string, System.StringComparison)
int32_t CoreLib::System::String::LastIndexOf(string* value, CoreLib::System::enum_StringComparison sc)
{
    throw 0xC000C000;
}

// Method : string.LastIndexOf(string, int, System.StringComparison)
int32_t CoreLib::System::String::LastIndexOf(string* value, int32_t startIndex, CoreLib::System::enum_StringComparison sc)
{
    throw 0xC000C000;
}

// Method : string.LastIndexOf(string, int, int)
int32_t CoreLib::System::String::LastIndexOf(string* value, int32_t startIndex, int32_t count)
{
    throw 0xC000C000;
}

// Method : string.LastIndexOfAny(char[])
int32_t CoreLib::System::String::LastIndexOfAny(__array<char16_t>* anyOf)
{
    throw 0xC000C000;
}

// Method : string.LastIndexOfAny(char[], int)
int32_t CoreLib::System::String::LastIndexOfAny(__array<char16_t>* anyOf, int32_t startIndex)
{
    throw 0xC000C000;
}

// Method : string.LastIndexOfAny(char[], int, int)
int32_t CoreLib::System::String::LastIndexOfAny(__array<char16_t>* anyOf, int32_t startIndex, int32_t count)
{
    throw 0xC000C000;
}
