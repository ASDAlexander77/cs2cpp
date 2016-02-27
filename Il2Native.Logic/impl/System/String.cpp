#include "CoreLib.h"

// Method : string.FastAllocateString(int)
string* CoreLib::System::String::FastAllocateString(int32_t length)
{
	auto size = sizeof(string) + length * sizeof(wchar_t);
    auto mem = ::operator new (size);
	std::memset(mem, 0, size);
    new (mem) string;
    auto str = reinterpret_cast<string*>(mem);
    str->m_stringLength = length;
    return str;
}

// Method : string.LastIndexOf(char, int)
int32_t CoreLib::System::String::LastIndexOf(wchar_t value, int32_t startIndex)
{
    throw 0xC000C000;
}

// Method : string.LastIndexOf(char, int, int)
int32_t CoreLib::System::String::LastIndexOf(wchar_t value, int32_t startIndex, int32_t count)
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
int32_t CoreLib::System::String::LastIndexOfAny(__array<wchar_t>* anyOf)
{
    throw 0xC000C000;
}

// Method : string.LastIndexOfAny(char[], int)
int32_t CoreLib::System::String::LastIndexOfAny(__array<wchar_t>* anyOf, int32_t startIndex)
{
    throw 0xC000C000;
}

// Method : string.LastIndexOfAny(char[], int, int)
int32_t CoreLib::System::String::LastIndexOfAny(__array<wchar_t>* anyOf, int32_t startIndex, int32_t count)
{
    throw 0xC000C000;
}
