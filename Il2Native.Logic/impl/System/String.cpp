#include "CoreLib.h"

// Method : string.String(char[], int, int)
CoreLib::System::String::String(__array<wchar_t>* value, int32_t startIndex, int32_t length)
{
    throw 3221274624U;
}

// Method : string.String(char[])
CoreLib::System::String::String(__array<wchar_t>* value)
{
    throw 3221274624U;
}

// Method : string.String(char, int)
CoreLib::System::String::String(wchar_t c, int32_t count)
{
    throw 3221274624U;
}

// Method : string.String(char*, int, int)
CoreLib::System::String::String(wchar_t* src, int32_t startIndex, int32_t length)
{
    throw 3221274624U;
}

// Method : string.String(sbyte*)
CoreLib::System::String::String(int8_t* src)
{
    throw 3221274624U;
}

// Method : string.String(sbyte*, int, int)
CoreLib::System::String::String(int8_t* src, int32_t startIndex, int32_t length)
{
    throw 3221274624U;
}

// Method : string.String(sbyte*, int, int, System.Text.Encoding)
CoreLib::System::String::String(int8_t* src, int32_t startIndex, int32_t length, CoreLib::System::Text::Encoding* enc)
{
    throw 3221274624U;
}

// Method : string.FastAllocateString(int)
string* CoreLib::System::String::FastAllocateString(int32_t length)
{
    auto mem = ::operator new (sizeof(string) + length * sizeof(wchar_t));
    new (mem) string;
    auto str = static_cast<string*>(mem);
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
