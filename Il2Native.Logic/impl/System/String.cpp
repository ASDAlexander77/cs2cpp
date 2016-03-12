#include "CoreLib.h"

// Method : string.String(char[], int, int)
void CoreLib::System::String::_ctor(__array<wchar_t>* value, int32_t startIndex, int32_t length)
{
    throw 3221274624U;
}

// Method : string.String(char[])
void CoreLib::System::String::_ctor(__array<wchar_t>* value)
{
    throw 3221274624U;
}

// Method : string.String(char, int)
void CoreLib::System::String::_ctor(wchar_t c, int32_t count)
{
    throw 3221274624U;
}

// Method : string.String(char*, int, int)
void CoreLib::System::String::_ctor(wchar_t* src, int32_t startIndex, int32_t length)
{
    throw 3221274624U;
}

// Method : string.String(sbyte*)
void CoreLib::System::String::_ctor(int8_t* src)
{
    throw 3221274624U;
}

// Method : string.String(sbyte*, int, int)
void CoreLib::System::String::_ctor(int8_t* src, int32_t startIndex, int32_t length)
{
    throw 3221274624U;
}

// Method : string.String(sbyte*, int, int, System.Text.Encoding)
void CoreLib::System::String::_ctor(int8_t* src, int32_t startIndex, int32_t length, CoreLib::System::Text::Encoding* enc)
{
    throw 3221274624U;
}

// Method : string.FastAllocateString(int)
string* CoreLib::System::String::FastAllocateString(int32_t length)
{
	auto size = sizeof(string) + (length + 1) * sizeof(wchar_t);
	auto str = new ((int32_t)size) string;
	str->m_stringLength = length;
	return str;
}

// Method : string.LastIndexOf(char, int)
int32_t CoreLib::System::String::LastIndexOf(wchar_t value, int32_t startIndex)
{
    throw 3221274624U;
}

// Method : string.LastIndexOf(char, int, int)
int32_t CoreLib::System::String::LastIndexOf(wchar_t value, int32_t startIndex, int32_t count)
{
    throw 3221274624U;
}

// Method : string.LastIndexOf(string, System.StringComparison)
int32_t CoreLib::System::String::LastIndexOf(string* value, CoreLib::System::enum_StringComparison sc)
{
    throw 3221274624U;
}

// Method : string.LastIndexOf(string, int, System.StringComparison)
int32_t CoreLib::System::String::LastIndexOf(string* value, int32_t startIndex, CoreLib::System::enum_StringComparison sc)
{
    throw 3221274624U;
}

// Method : string.LastIndexOf(string, int, int)
int32_t CoreLib::System::String::LastIndexOf(string* value, int32_t startIndex, int32_t count)
{
    throw 3221274624U;
}

// Method : string.LastIndexOfAny(char[])
int32_t CoreLib::System::String::LastIndexOfAny(__array<wchar_t>* anyOf)
{
    throw 3221274624U;
}

// Method : string.LastIndexOfAny(char[], int)
int32_t CoreLib::System::String::LastIndexOfAny(__array<wchar_t>* anyOf, int32_t startIndex)
{
    throw 3221274624U;
}

// Method : string.LastIndexOfAny(char[], int, int)
int32_t CoreLib::System::String::LastIndexOfAny(__array<wchar_t>* anyOf, int32_t startIndex, int32_t count)
{
    throw 3221274624U;
}
