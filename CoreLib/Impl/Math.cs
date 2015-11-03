namespace System
{
    using Runtime.CompilerServices;

    public static partial class Math
    {
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double log10(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double log(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double pow(double value, double power);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double ceil(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double floor(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double cos(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double cosh(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double sin(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double sinh(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double tan(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double tanh(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double sqrt(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double acos(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double asin(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double atan(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double atan2(double y, double x);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double exp(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern float fabsf(float value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double fabs(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double fmod(double value1, double value2);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double round(double value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern unsafe double modf(double p, double* value);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern double copysign(double value1, double value2);

        public static double Acos(double d)
        {
            return acos(d);
        }

        public static double Asin(double d)
        {
            return asin(d);
        }

        public static double Atan(double x)
        {
            return atan(x);
        }

        public static double Ceiling(double a)
        {
            return ceil(a);
        }

        public static double Cos(double a)
        {
            return cos(a);
        }

        public static double Cosh(double value)
        {
            return cosh(value);
        }

        public static double Floor(double d)
        {
            return floor(d);
        }

        public static double Round(double d)
        {
            //return round(a);

            double tempVal;
            double flrTempVal;
            // If the number has no fractional part do nothing
            // This shortcut is necessary to workaround precision loss in borderline cases on some platforms
            if (d == (long)d)
                return d;
            tempVal = (d + 0.5);
            //We had a number that was equally close to 2 integers. 
            //We need to return the even one.
            flrTempVal = floor(tempVal);
            if (flrTempVal == tempVal)
            {
                if (0 != fmod(tempVal, 2.0))
                {
                    flrTempVal -= 1.0;
                }
            }

            flrTempVal = copysign(flrTempVal, d);
            return flrTempVal;
        }

        public static double Sin(double a)
        {
            return sin(a);
        }

        public static double Tan(double a)
        {
            return tan(a);
        }

        public static double Sinh(double value)
        {
            return sinh(value);
        }

        public static double Tanh(double value)
        {
            return tanh(value);
        }

        private static unsafe double SplitFractionDouble(double* value)
        {
            return modf(*value, value);
        }

        public static double Exp(double x)
        {
            if (Double.IsInfinity(x))
            {
                if (x < 0)
                    return (+0.0);
                return (x);      // Must be + infinity
            }

            return exp(x);
        }

        public static double Pow(double x, double y)
        {
            unsafe
            {
                double r1;
                if (Double.IsInfinity(y))
                {
                    if (*(ulong*)&x == 0x3FF0000000000000)
                    {
                        return x;
                    }

                    if (*(ulong*)&x == 0xBFF0000000000000)
                    {
                        *(ulong*)&r1 = 0xFFF8000000000000;
                        return r1;
                    }
                }
                else if (Double.IsNaN(y) || Double.IsNaN(x))
                {
                    *(ulong*)&r1 = 0xFFF8000000000000;
                    return r1;
                }
            }

            return pow(x, y);
        }

        public static double Sqrt(double d)
        {
            return sqrt(d);
        }

        public static double Log(double d)
        {
            return log(d);
        }

        public static double Log10(double d)
        {
            return log10(d);
        }

        public static float Abs(float value)
        {
            return fabsf(value);
        }

        public static double Abs(double value)
        {
            return fabs(value);
        }
    }
}
