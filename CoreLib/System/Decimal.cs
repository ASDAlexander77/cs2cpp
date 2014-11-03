////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace System
{

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
    public struct Decimal
    {
        private const int SignMask = unchecked((int)0x80000000);
        private const int ScaleMask = 0x00FF0000;
        
        private int flags;
        private int hi;
        private int lo;
        private int mid;

        //The VB IDE starts to run amuck when it tries to do syntax checking on sources that contain Decimal
        //and causes the compiler to repeatedly crash unless it finds a constructor.
        public Decimal(int value)
        {
            if (value >= 0)
            {
                flags = 0;
            }
            else
            {
                flags = SignMask;
                value = -value;
            }
            lo = value;
            mid = 0;
            hi = 0;
           
        }

        [CLSCompliant(false)]
        public Decimal(uint value)
        {
            flags = 0;
            lo = (int)value;
            mid = 0;
            hi = 0;            
        }

        public Decimal(long value)
        {
            if (value >= 0)
            {
                flags = 0;
            }
            else
            {
                flags = SignMask;
                value = -value;
            }
            lo = (int)value;
            mid = (int)(value >> 32);
            hi = 0;            
        }

        //[CLSCompliant(false)]
        public Decimal(ulong value)
        {
            flags = 0;
            lo = (int)value;
            mid = (int)(value >> 32);
            hi = 0;            
        }

        //public Decimal(float value) { }

        //public Decimal(double value) { }

        //internal Decimal(Currency value) { }

        public Decimal(int[] bits)
        {
            if (bits == null)
                throw new ArgumentNullException("bits");
            if (bits.Length == 4)
            {
                int f = bits[3];
                if ((f & ~(SignMask | ScaleMask)) == 0 && (f & ScaleMask) <= (28 << 16))
                {
                    lo = bits[0];
                    mid = bits[1];
                    hi = bits[2];
                    flags = f;
                    return;
                }
            }
            throw new ArgumentException("Arg_DecBitCtor");
        }

        public Decimal(int lo, int mid, int hi, bool isNegative, byte scale)
        {
            if (scale > 28)
                throw new ArgumentOutOfRangeException("scale", "ArgumentOutOfRange_DecimalScale");
            this.lo = lo;
            this.mid = mid;
            this.hi = hi;
            this.flags = ((int)scale) << 16;
            if (isNegative)
                this.flags |= SignMask;
        }

        public static Decimal operator +(Decimal d1, Decimal d2)
        {
            return Add(d1, d2);
        }

        public static Decimal operator ++(Decimal d1)
        {
            Decimal result = new Decimal();
            result.lo = d1.lo + 1;
            result.mid = d1.mid;
            result.hi = d1.hi;
            return result;
        }

        public static Decimal operator -(Decimal d1, Decimal d2)
        {
            return Subtract(d1, d2);
        }

        public static Decimal operator --(Decimal d1)
        {
            Decimal result = new Decimal();
            result.lo = d1.lo - 1;
            result.mid = d1.mid;
            result.hi = d1.hi;
            return result;
        }

        // Adds two Decimal values.
        //
        // TODO: dummy function for now
        public static Decimal Add(Decimal d1, Decimal d2)
        {
            Decimal result = new Decimal();
            result.lo = d1.lo + d2.lo;
            result.mid = d1.mid + d2.mid;
            result.hi = d1.hi + d2.hi;
            return result;
        }

        public static Decimal Subtract(Decimal d1, Decimal d2)
        {
            Decimal result = new Decimal();
            result.lo = d1.lo - d2.lo;
            result.mid = d1.mid - d2.mid;
            result.hi = d1.hi - d2.hi;
            return result;
        }

        public static bool operator ==(Decimal d1, Decimal d2)
        {
            return Compare(d1, d2) == 0;
        }

        public static bool operator !=(Decimal d1, Decimal d2)
        {
            return Compare(d1, d2) != 0;
        }

        public static int Compare(Decimal d1, Decimal d2)
        {
            if (d1.hi > d2.hi)
            {
                return 1;
            }

            if (d1.hi < d2.hi)
            {
                return -1;
            }

            if (d1.mid > d2.mid)
            {
                return 1;
            }

            if (d1.mid < d2.mid)
            {
                return -1;
            }

            if (d1.lo > d2.lo)
            {
                return 1;
            }

            if (d1.lo < d2.lo)
            {
                return -1;
            }

            return 0;
        }

        public static explicit operator int(Decimal value)
        {
            return ToInt32(value);
        }

        public static int ToInt32(Decimal d)
        {
            //if ((d.flags & ScaleMask) != 0) d = Truncate(d);
            if (d.hi == 0 && d.mid == 0)
            {
                var i = d.lo;
                if (d.flags >= 0)
                {
                    if (i >= 0) return i;
                }
                else
                {
                    i = -i;
                    if (i <= 0) return i;
                }
            }

            throw new Exception("Overflow_Int32");
        }

    }
}


