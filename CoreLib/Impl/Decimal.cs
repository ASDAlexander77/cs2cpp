namespace System
{
    public partial struct Decimal
    {
        internal static unsafe bool NumberToDecimal(byte* numberBytes, ref decimal value)
        {
            Number.NUMBER* number = (Number.NUMBER*)numberBytes;

            Decimal d;
            d.flags = 0;
            d.hi = 0;
            d.lo = 0;
            d.mid = 0;
            char* digitsPtr = number->digits;
            char* p = digitsPtr;
            int e = number->scale;
            if (*p > 0)
            {
                if (e > Number.DECIMAL_PRECISION || e < -Number.DECIMAL_PRECISION) return false;
                while ((e > 0 || *p > 0 && e > -28)
                       && (d.hi < 0x19999999
                           || d.hi == 0x19999999 && (d.mid < 0x99999999 || d.mid == 0x99999999 && (d.lo < 0x99999999 || d.lo == 0x99999999 && *p <= '5'))))
                {
                    DecMul10(&d);
                    if (*p > 0) DecAddInt32(&d, (uint)(*p++ - '0'));
                    e--;
                }
                if (*p++ >= '5')
                {
                    bool round = true;
                    if (*(p - 1) == '5' && *(p - 2) % 2 == 0)
                    {
                        // Check if previous digit is even, only if the when we are unsure whether hows to do Banker's rounding
                        // For digits > 5 we will be roundinp up anyway.
                        int count = 20; // Look at the next 20 digits to check to round
                        while (*p == '0' && count != 0)
                        {
                            p++;
                            count--;
                        }
                        if (*p == '\0' || count == 0) round = false; // Do nothing
                    }

                    if (round)
                    {
                        DecAddInt32(&d, 1);
                        if ((d.hi | d.mid | d.lo) == 0)
                        {
                            d.hi = 0x19999999;
                            d.mid = unchecked((int)0x99999999);
                            d.lo = unchecked((int)0x9999999A);
                            e++;
                        }
                    }
                }

                if (e > 0) return false;
                d.flags = -e << 16;
                d.flags |= (number->sign ? DECIMAL_NEG : 0) << 24;
                value = d;
            }

            return true;
        }


        private unsafe static void DecMul10(decimal* value)
        {
            decimal d = *value;
            DecShiftLeft(value);
            DecShiftLeft(value);
            DecAdd(value, &d);
            DecShiftLeft(value);
        }

        private unsafe static void DecAdd(decimal* value, decimal* d)
        {
            if (D32AddCarry(&value->lo, (uint)d->lo))
            {
                if (D32AddCarry(&value->mid, 1))
                {
                    D32AddCarry(&value->hi, 1);
                }
            }
            if (D32AddCarry(&value->mid, (uint)d->mid))
            {
                D32AddCarry(&value->hi, 1);
            }

            D32AddCarry(&value->hi, (uint)d->hi);
        }

        private unsafe static void DecShiftLeft(decimal* value)
        {
            var c0 = (value->lo & 0x80000000) > 0 ? 1 : 0;
            var c1 = (value->mid & 0x80000000) > 0 ? 1 : 0;
            value->lo <<= 1;
            value->mid = value->mid << 1 | c0;
            value->hi = value->hi << 1 | c1;
        }

        private unsafe static void DecAddInt32(decimal* value, uint i)
        {
            if (D32AddCarry(&value->lo, i))
            {
                if (D32AddCarry(&value->mid, 1))
                {
                    D32AddCarry(&value->hi, 1);
                }
            }
        }

        private unsafe static bool D32AddCarry(int* value, uint i)
        {
            var v = (uint)*value;
            var sum = v + i;
            *value = (int)sum;
            return sum < v || sum < i;
        }
    }
}
