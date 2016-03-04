#include "CoreLib.h"

double CoreLib::System::Number::modf_Ref(double x, double& intpart)
{
    return std::modf(x, &intpart);
}

/* 
// Method : System.Number.FormatDecimal(decimal, string, System.Globalization.NumberFormatInfo)
string* CoreLib::System::Number::FormatDecimal(CoreLib::System::Decimal value, string* format, CoreLib::System::Globalization::NumberFormatInfo* info)
{
    throw 0xC000C000;
}

// Method : System.Number.FormatDouble(double, string, System.Globalization.NumberFormatInfo)
string* CoreLib::System::Number::FormatDouble(double value, string* format, CoreLib::System::Globalization::NumberFormatInfo* info)
{
    throw 0xC000C000;
}

// Method : System.Number.FormatInt32(int, string, System.Globalization.NumberFormatInfo)
string* CoreLib::System::Number::FormatInt32(int32_t value, string* format, CoreLib::System::Globalization::NumberFormatInfo* info)
{
    throw 0xC000C000;
}

// Method : System.Number.FormatUInt32(uint, string, System.Globalization.NumberFormatInfo)
string* CoreLib::System::Number::FormatUInt32(uint32_t value, string* format, CoreLib::System::Globalization::NumberFormatInfo* info)
{
    throw 0xC000C000;
}

// Method : System.Number.FormatInt64(long, string, System.Globalization.NumberFormatInfo)
string* CoreLib::System::Number::FormatInt64(int64_t value, string* format, CoreLib::System::Globalization::NumberFormatInfo* info)
{
    throw 0xC000C000;
}

// Method : System.Number.FormatUInt64(ulong, string, System.Globalization.NumberFormatInfo)
string* CoreLib::System::Number::FormatUInt64(uint64_t value, string* format, CoreLib::System::Globalization::NumberFormatInfo* info)
{
    throw 0xC000C000;
}

// Method : System.Number.FormatSingle(float, string, System.Globalization.NumberFormatInfo)
string* CoreLib::System::Number::FormatSingle(float value, string* format, CoreLib::System::Globalization::NumberFormatInfo* info)
{
    throw 0xC000C000;
}

// Method : System.Number.NumberBufferToDecimal(byte*, ref decimal)
bool CoreLib::System::Number::NumberBufferToDecimal_Ref(uint8_t* number, CoreLib::System::Decimal& value)
{
    throw 0xC000C000;
}

// Method : System.Number.NumberBufferToDouble(byte*, ref double)
bool CoreLib::System::Number::NumberBufferToDouble_Ref(uint8_t* number, double& value)
{
    throw 0xC000C000;
}

// Method : System.Number.FormatNumberBuffer(byte*, string, System.Globalization.NumberFormatInfo, char*)
string* CoreLib::System::Number::FormatNumberBuffer(uint8_t* number, string* format, CoreLib::System::Globalization::NumberFormatInfo* info, wchar_t* allDigits)
{
    throw 0xC000C000;
}
*/