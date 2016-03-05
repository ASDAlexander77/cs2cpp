#include "CoreLib.h"

// Method : decimal.Decimal(float)
void CoreLib::System::Decimal::_ctor(float value)
{
    DecFromR4(value, &this->flags);
}

// Method : decimal.Decimal(double)
void CoreLib::System::Decimal::_ctor(double value)
{
    DecFromR8(value, &this->flags);
}

// Method : decimal.FCallAddSub(ref decimal, ref decimal, byte)
void CoreLib::System::Decimal::FCallAddSub_Ref_Ref(CoreLib::System::Decimal& d1, CoreLib::System::Decimal& d2, uint8_t bSign)
{
    CoreLib::System::Decimal res;
    if (DecAddSub(&d1.flags, &d2.flags, &res.flags, bSign) != 0)
    {
        throw __new<CoreLib::System::OverflowException>();
    }

    d1 = res;
}

// Method : decimal.FCallAddSubOverflowed(ref decimal, ref decimal, byte, ref bool)
void CoreLib::System::Decimal::FCallAddSubOverflowed_Ref_Ref_Ref(CoreLib::System::Decimal& d1, CoreLib::System::Decimal& d2, uint8_t bSign, bool& overflowed)
{
    CoreLib::System::Decimal res;
    overflowed = DecAddSub(&d1.flags, &d2.flags, &res.flags, bSign) != 0;
    d1 = res;
}

// Method : decimal.FCallCompare(ref decimal, ref decimal)
int32_t CoreLib::System::Decimal::FCallCompare_Ref_Ref(CoreLib::System::Decimal& d1, CoreLib::System::Decimal& d2)
{
    DecCmp(&d1.flags, &d2.flags);
}

// Method : decimal.FCallDivide(ref decimal, ref decimal)
void CoreLib::System::Decimal::FCallDivide_Ref_Ref(CoreLib::System::Decimal& d1, CoreLib::System::Decimal& d2)
{
	CoreLib::System::Decimal res;
	int code;
	if ((code = DecDiv(&d1.flags, &d2.flags, &res.flags)) != 0)
	{
		if (code == 2)
		{
			throw __new<CoreLib::System::DivideByZeroException>();
		}

		throw __new<CoreLib::System::OverflowException>();
	}

	d1 = res;
}

// Method : decimal.FCallDivideOverflowed(ref decimal, ref decimal, ref bool)
void CoreLib::System::Decimal::FCallDivideOverflowed_Ref_Ref_Ref(CoreLib::System::Decimal& d1, CoreLib::System::Decimal& d2, bool& overflowed)
{
	CoreLib::System::Decimal res;
	int code;
	if ((code = DecDiv(&d1.flags, &d2.flags, &res.flags)) != 0)
	{
		if (code == 2)
		{
			throw __new<CoreLib::System::DivideByZeroException>();
		}

		overflowed = true;
	}

	d1 = res;
}

// Method : decimal.GetHashCode()
int32_t CoreLib::System::Decimal::GetHashCode()
{
    throw 0xC000C000;
}

// Method : decimal.FCallFloor(ref decimal)
void CoreLib::System::Decimal::FCallFloor_Ref(CoreLib::System::Decimal& d)
{
    throw 0xC000C000;
}

// Method : decimal.FCallMultiply(ref decimal, ref decimal)
void CoreLib::System::Decimal::FCallMultiply_Ref_Ref(CoreLib::System::Decimal& d1, CoreLib::System::Decimal& d2)
{
    throw 0xC000C000;
}

// Method : decimal.FCallMultiplyOverflowed(ref decimal, ref decimal, ref bool)
void CoreLib::System::Decimal::FCallMultiplyOverflowed_Ref_Ref_Ref(CoreLib::System::Decimal& d1, CoreLib::System::Decimal& d2, bool& overflowed)
{
    throw 0xC000C000;
}

// Method : decimal.FCallRound(ref decimal, int)
void CoreLib::System::Decimal::FCallRound_Ref(CoreLib::System::Decimal& d, int32_t decimals)
{
    throw 0xC000C000;
}

// Method : decimal.FCallToCurrency(ref System.Currency, decimal)
void CoreLib::System::Decimal::FCallToCurrency_Ref(CoreLib::System::Currency& result, CoreLib::System::Decimal d)
{
    throw 0xC000C000;
}

// Method : decimal.ToDouble(decimal)
double CoreLib::System::Decimal::ToDouble(CoreLib::System::Decimal d)
{
    throw 0xC000C000;
}

// Method : decimal.FCallToInt32(decimal)
int32_t CoreLib::System::Decimal::FCallToInt32(CoreLib::System::Decimal d)
{
    throw 0xC000C000;
}

// Method : decimal.ToSingle(decimal)
float CoreLib::System::Decimal::ToSingle(CoreLib::System::Decimal d)
{
    throw 0xC000C000;
}

// Method : decimal.FCallTruncate(ref decimal)
void CoreLib::System::Decimal::FCallTruncate_Ref(CoreLib::System::Decimal& d)
{
    throw 0xC000C000;
}
