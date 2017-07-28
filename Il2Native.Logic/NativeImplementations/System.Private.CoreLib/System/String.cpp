#include "System.Private.CoreLib.h"

namespace CoreLib { namespace System { 
    namespace _ = ::CoreLib::System;
    // Method : string.String(char*)
    void String::_ctor(char16_t* value)
    {
        throw 3221274624U;
    }
    
    // Method : string.String(char*, int, int)
    void String::_ctor(char16_t* value, int32_t startIndex, int32_t length)
    {
        throw 3221274624U;
    }
    
    // Method : string.String(sbyte*)
    void String::_ctor(int8_t* value)
    {
        throw 3221274624U;
    }
    
    // Method : string.String(sbyte*, int, int)
    void String::_ctor(int8_t* value, int32_t startIndex, int32_t length)
    {
        throw 3221274624U;
    }
    
    // Method : string.String(sbyte*, int, int, System.Text.Encoding)
    void String::_ctor(int8_t* value, int32_t startIndex, int32_t length, _::Text::Encoding* enc)
    {
        throw 3221274624U;
    }
    
    // Method : string.String(char[], int, int)
    void String::_ctor(__array<char16_t>* value, int32_t startIndex, int32_t length)
    {
        throw 3221274624U;
    }
    
    // Method : string.String(char[])
    void String::_ctor(__array<char16_t>* value)
    {
        throw 3221274624U;
    }
    
    // Method : string.String(char, int)
    void String::_ctor(char16_t c, int32_t count)
    {
        throw 3221274624U;
    }
    
    // Method : string.this[int].get
    char16_t String::get_Chars(int32_t index)
    {
		return ((char16_t*)&this->_firstChar)[index];
    }
    
    // Method : string.Length.get
    int32_t String::get_Length()
    {
		return this->m_stringLength;
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
    
    // Method : string.IsFastSort()
    bool String::IsFastSort()
    {
        throw 3221274624U;
    }
    
    // Method : string.IsAscii()
    bool String::IsAscii()
    {
        throw 3221274624U;
    }
    
    // Method : string.SetTrailByte(byte)
    void String::SetTrailByte(uint8_t data)
    {
        throw 3221274624U;
    }
    
    // Method : string.TryGetTrailByte(out byte)
    bool String::TryGetTrailByte_Out(uint8_t& data)
    {
        throw 3221274624U;
    }
    
    // Method : string.CompareOrdinalHelper(string, int, int, string, int, int)
    int32_t String::CompareOrdinalHelper(string* strA, int32_t indexA, int32_t countA, string* strB, int32_t indexB, int32_t countB)
    {
        throw 3221274624U;
    }
    
    // Method : string.nativeCompareOrdinalIgnoreCaseWC(string, sbyte*)
    int32_t String::nativeCompareOrdinalIgnoreCaseWC(string* strA, int8_t* strBBytes)
    {
        throw 3221274624U;
    }
    
    // Method : string.InternalMarvin32HashString(string, int, long)
    int32_t String::InternalMarvin32HashString(string* s, int32_t strLen, int64_t additionalEntropy)
    {
        throw 3221274624U;
    }
    
    // Method : string.InternalUseRandomizedHashing()
    bool String::InternalUseRandomizedHashing()
    {
        throw 3221274624U;
    }
    
    // Method : string.ReplaceInternal(string, string)
    string* String::ReplaceInternal(string* oldValue, string* newValue)
    {
        throw 3221274624U;
    }
    
    // Method : string.IndexOfAny(char[], int, int)
    int32_t String::IndexOfAny(__array<char16_t>* anyOf, int32_t startIndex, int32_t count)
    {
        throw 3221274624U;
    }
    
    // Method : string.LastIndexOfAny(char[], int, int)
    int32_t String::LastIndexOfAny(__array<char16_t>* anyOf, int32_t startIndex, int32_t count)
    {
        throw 3221274624U;
    }

}}

namespace CoreLib { namespace System { 
    namespace _ = ::CoreLib::System;
}}
