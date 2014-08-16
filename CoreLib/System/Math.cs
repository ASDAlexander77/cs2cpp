////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Microsoft Corporation.  All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System
{
    //This class contains only static members and doesn't require serialization.
    using System;
    using System.Runtime.CompilerServices;
    public static class Math
    {
        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public static extern double @llvm_log10_f64(double value);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public static extern double @llvm_log_f64(double value);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public static extern double @llvm_pow_f64(double value, double power);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public static extern double @llvm_floor_f64(double value);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public static extern double @llvm_cos_f64(double value);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public static extern double @llvm_sin_f64(double value);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public static extern double @llvm_sqrt_f64(double value);

        // Public Constants

        // Summary:
        //     Represents the ratio of the circumference of a circle to its diameter, specified
        //     by the constant, p.
        public const double PI = 3.14159265358979323846;
        // Summary:
        //     Represents the natural logarithmic base, specified by the constant, e
        public const double E = 2.7182818284590452354;


        // Methods

        // Summary:
        //     Returns the absolute value of an integer number.
        //
        // Parameters:
        //   value:
        //     A number in the range System.Double.MinValue=value=System.Double.MaxValue.
        //
        // Returns:
        //     An integer, x, such that 0 = x =System.Integer.MaxValue.
        public static int Abs(int val)
        {
            return (val >= 0) ? val : -val;
        }
        //
        // Summary:
        //     Returns the larger of two integer numbers.
        //
        // Parameters:
        //   val1:
        //     The first of two integert numbers to compare.
        //
        //   val2:
        //     The second of two integer numbers to compare.
        //
        // Returns:
        //     Parameter val1 or val2, whichever is larger. 
        public static int Max(int val1, int val2)
        {
            return (val1 >= val2) ? val1 : val2;
        }
        //
        // Summary:
        //     Returns the smaller of two integer numbers.
        //
        // Parameters:
        //   val1:
        //     The first of two integer numbers to compare.
        //
        //   val2:
        //     The second of two integer numbers to compare.
        //
        // Returns:
        //     Parameter val1 or val2, whichever is smaller. 
        public static int Min(int val1, int val2)
        {
            return (val1 <= val2) ? val1 : val2;
        }


        // Summary:
        //     Returns the absolute value of a double-precision floating-point number.
        //
        // Parameters:
        //   value:
        //     A number in the range System.Double.MinValue=value=System.Double.MaxValue.
        //
        // Returns:
        //     A double-precision floating-point number, x, such that 0 = x =System.Double.MaxValue.
        public static double Abs(double val)
        {
            return (val >= 0) ? val : -val;
        }
        //
        // Summary:
        //     Returns the angle whose cosine is the specified number.
        //
        // Parameters:
        //   d:
        //     A number representing a cosine, where -1 =d= 1.
        //
        // Returns:
        //     An angle, ?, measured in radians, such that 0 =?=p -or- System.Double.NaN
        //     if d < -1 or d > 1.

        public static double Acos(double d)
        {
            var div = (-d * d + 1.0);
            return Atan(-d / (div * div)) + 2.0 * Atan(1.0);
        }
        //
        // Summary:
        //     Returns the angle whose sine is the specified number.
        //
        // Parameters:
        //   d:
        //     A number representing a sine, where -1 =d= 1.
        //
        // Returns:
        //     An angle, ?, measured in radians, such that -p/2 =?=p/2 -or- System.Double.NaN
        //     if d < -1 or d > 1.

        public static double Asin(double d)
        {
            var div = (-d * d + 1.0);
            return Atan(d / (div * div));
        }
        //
        // Summary:
        //     Returns the angle whose tangent is the specified number.
        //
        // Parameters:
        //   d:
        //     A number representing a tangent.
        //
        // Returns:
        //     An angle, ?, measured in radians, such that -p/2 =?=p/2.  -or- System.Double.NaN
        //     if d equals System.Double.NaN, -p/2 rounded to double precision (-1.5707963267949)
        //     if d equals System.Double.NegativeInfinity, or p/2 rounded to double precision
        //     (1.5707963267949) if d equals System.Double.PositiveInfinity.

        public static double Atan(double x)
        {
            return Atan2(x, 1.0);
        }
        //
        // Summary:
        //     Returns the angle whose tangent is the quotient of two specified numbers.
        //
        // Parameters:
        //   y:
        //     The y coordinate of a point.
        //
        //   x:
        //     The x coordinate of a point.
        //
        // Returns:
        //     An angle, ?, measured in radians, such that -p=?=p, and tan(?) = y / x, where
        //     (x, y) is a point in the Cartesian plane. Observe the following: For (x,
        //     y) in quadrant 1, 0 < ? < p/2.  For (x, y) in quadrant 2, p/2 < ?=p.  For
        //     (x, y) in quadrant 3, -p < ? < -p/2.  For (x, y) in quadrant 4, -p/2 < ?
        //     < 0.  For points on the boundaries of the quadrants, the return value is
        //     the following: If y is 0 and x is not negative, ? = 0.  If y is 0 and x is
        //     negative, ? = p.  If y is positive and x is 0, ? = p/2.  If y is negative
        //     and x is 0, ? = -p/2.


        const double CV_PI = 3.1415926535897932384626433832795;
        const double atan2_p1 = 0.9997878412794807 * (180.0 / CV_PI);
        const double atan2_p3 = -0.3258083974640975 * (180.0 / CV_PI);
        const double atan2_p5 = 0.1555786518463281 * (180.0 / CV_PI);
        const double atan2_p7 = -0.04432655554792128 * (180.0 / CV_PI);
        const double DBL_EPSILON = 2.2204460492503131e-16;

        public static double Atan2(double y, double x)
        {
            var ax = Abs(x);
            var ay = Abs(y);
            double a, c, c2;
            if( ax >= ay )
            {
                c = ay/(ax + DBL_EPSILON);
                c2 = c*c;
                a = (((atan2_p7*c2 + atan2_p5)*c2 + atan2_p3)*c2 + atan2_p1)*c;
            }
            else
            {
                c = ax/(ay + DBL_EPSILON);
                c2 = c*c;
                a = 90.0 - (((atan2_p7*c2 + atan2_p5)*c2 + atan2_p3)*c2 + atan2_p1)*c;
            }
            if( x < 0.0 )
                a = 180.0 - a;
            if( y < 0.0 )
                a = 360.0 - a;
            return a;
        }
        //
        // Summary:
        //     Returns the smallest integer greater than or equal to the specified double-precision
        //     floating-point number.
        //
        // Parameters:
        //   a:
        //     A double-precision floating-point number.
        //
        // Returns:
        //     The smallest integer greater than or equal to a. If a is equal to System.Double.NaN,
        //     System.Double.NegativeInfinity, or System.Double.PositiveInfinity, that value
        //     is returned.

        public static extern double Ceiling(double d);        //
        // Summary:
        //     Returns the cosine of the specified angle.
        //
        // Parameters:
        //   d:
        //     An angle, measured in radians.
        //
        // Returns:
        //     The cosine of d.

        public static double Cos(double a)
        {
            return llvm_cos_f64(a);
        }

        //
        // Summary:
        //     Returns the hyperbolic cosine of the specified angle.
        //
        // Parameters:
        //   value:
        //     An angle, measured in radians.
        //
        // Returns:
        //     The hyperbolic cosine of value. If value is equal to System.Double.NegativeInfinity
        //     or System.Double.PositiveInfinity, System.Double.PositiveInfinity is returned.
        //     If value is equal to System.Double.NaN, System.Double.NaN is returned.

        public static extern double Cosh(double a);
        //
        // Summary:
        //     Returns the remainder resulting from the division of a specified number by
        //     another specified number.
        //
        // Parameters:
        //   x:
        //     A dividend.
        //
        //   y:
        //     A divisor.
        //
        // Returns:
        //     A number equal to x - (y Q), where Q is the quotient of x / y rounded to
        //     the nearest integer (if x / y falls halfway between two integers, the even
        //     integer is returned).  If x - (y Q) is zero, the value +0 is returned if
        //     x is positive, or -0 if x is negative.  If y = 0, System.Double.NaN (Not-A-Number)
        //     is returned.

        public static extern double IEEERemainder(double x, double y);
        //
        // Summary:
        //     Returns e raised to the specified power.
        //
        // Parameters:
        //   d:
        //     A number specifying a power.
        //
        // Returns:
        //     The number e raised to the power d. If d equals System.Double.NaN or System.Double.PositiveInfinity,
        //     that value is returned. If d equals System.Double.NegativeInfinity, 0 is
        //     returned.

        public static extern double Exp(double d);
        //
        // Summary:
        //     Returns the largest integer less than or equal to the specified double-precision
        //     floating-point number.
        //
        // Parameters:
        //   d:
        //     A double-precision floating-point number.
        //
        // Returns:
        //     The largest integer less than or equal to d. If d is equal to System.Double.NaN,
        //     System.Double.NegativeInfinity, or System.Double.PositiveInfinity, that value
        //     is returned.

        public static double Floor(double d)
        {
            return llvm_floor_f64(d);
        }

        //
        // Summary:
        //     Returns the natural (base e) logarithm of a specified number.
        //
        // Parameters:
        //   d:
        //     A number whose logarithm is to be found.
        //
        // Returns:
        //     Sign of d Returns Positive The natural logarithm of d; that is, ln d, or
        //     log ed Zero System.Double.NegativeInfinity Negative System.Double.NaN If
        //     d is equal to System.Double.NaN, returns System.Double.NaN. If d is equal
        //     to System.Double.PositiveInfinity, returns System.Double.PositiveInfinity.

        public static double Log(double d)
        {
            return @llvm_log_f64(d);
        }
        //
        // Summary:
        //     Returns the base 10 logarithm of a specified number.
        //
        // Parameters:
        //   d:
        //     A number whose logarithm is to be found.
        //
        // Returns:
        //     Sign of d Returns Positive The base 10 log of d; that is, log 10d. Zero System.Double.NegativeInfinity
        //     Negative System.Double.NaN If d is equal to System.Double.NaN, this method
        //     returns System.Double.NaN. If d is equal to System.Double.PositiveInfinity,
        //     this method returns System.Double.PositiveInfinity.

        public static double Log10(double d)
        {
            return @llvm_log10_f64(d);
        }
        //
        // Summary:
        //     Returns the larger of two double-precision floating-point numbers.
        //
        // Parameters:
        //   val1:
        //     The first of two double-precision floating-point numbers to compare.
        //
        //   val2:
        //     The second of two double-precision floating-point numbers to compare.
        //
        // Returns:
        //     Parameter val1 or val2, whichever is larger. If val1, val2, or both val1
        //     and val2 are equal to System.Double.NaN, System.Double.NaN is returned.
        public static double Max(double x, double y)
        {
            return (x >= y) ? x : y;
        }
        //
        // Summary:
        //     Returns the smaller of two double-precision floating-point numbers.
        //
        // Parameters:
        //   val1:
        //     The first of two double-precision floating-point numbers to compare.
        //
        //   val2:
        //     The second of two double-precision floating-point numbers to compare.
        //
        // Returns:
        //     Parameter val1 or val2, whichever is smaller. If val1, val2, or both val1
        //     and val2 are equal to System.Double.NaN, System.Double.NaN is returned.
        public static double Min(double x, double y)
        {
            return (x <= y) ? x : y;
        }
        //
        // Summary:
        //     Returns a specified number raised to the specified power.
        //
        // Parameters:
        //   x:
        //     A double-precision floating-point number to be raised to a power.
        //
        //   y:
        //     A double-precision floating-point number that specifies a power.
        //
        // Returns:
        //     The number x raised to the power y.

        public static double Pow(double x, double y)
        {
            return @llvm_pow_f64(x, y);
        }

        //
        // Summary:
        //     Rounds a double-precision floating-point value to the nearest integer.
        //
        // Parameters:
        //   a:
        //     A double-precision floating-point number to be rounded.
        //
        // Returns:
        //     The integer nearest a. If the fractional component of a is halfway between
        //     two integers, one of which is even and the other odd, then the even number
        //     is returned.

        public static extern double Round(double d);
        //
        //
        // Summary:
        //     Returns a value indicating the sign of a double-precision floating-point
        //     number.
        //
        // Parameters:
        //   value:
        //     A signed number.
        //
        // Returns:
        //     A number indicating the sign of value.  Number Description -1 value is less
        //     than zero. 0 value is equal to zero. 1 value is greater than zero.
        //
        // Exceptions:
        //   System.ArithmeticException:
        //     value is equal to System.Double.NaN.
        //     is returned.

        public static extern int Sign(double value);
        // Summary:
        //     Returns the sine of the specified angle.
        //
        // Parameters:
        //   a:
        //     An angle, measured in radians.
        //
        // Returns:
        //     The sine of a. If a is equal to System.Double.NaN, System.Double.NegativeInfinity,
        //     or System.Double.PositiveInfinity, this method returns System.Double.NaN.

        public static double Sin(double a)
        {
            return llvm_sin_f64(a);
        }
        //
        // Summary:
        //     Returns the hyperbolic sine of the specified angle.
        //
        // Parameters:
        //   value:
        //     An angle, measured in radians.
        //
        // Returns:
        //     The hyperbolic sine of value. If value is equal to System.Double.NegativeInfinity,
        //     System.Double.PositiveInfinity, or System.Double.NaN, this method returns
        //     a System.Double equal to value.

        public static extern double Sinh(double value);
        //
        // Summary:
        //     Returns the square root of a specified number.
        //
        // Parameters:
        //   d:
        //     A number.
        //
        // Returns:
        //     Value of d Returns Zero, or positive The positive square root of d. Negative
        //     System.Double.NaN If d is equal to System.Double.NaN or System.Double.PositiveInfinity,
        //     that value is returned.

        public static double Sqrt(double d)
        {
            return llvm_sqrt_f64(d);
        }
        //
        // Summary:
        //     Returns the tangent of the specified angle.
        //
        // Parameters:
        //   a:
        //     An angle, measured in radians.
        //
        // Returns:
        //     The tangent of a. If a is equal to System.Double.NaN, System.Double.NegativeInfinity,
        //     or System.Double.PositiveInfinity, this method returns System.Double.NaN.

        public static double Tan(double a)
        {
            return Sin(a) / Cos(a);
        }
        //
        // Summary:
        //     Returns the hyperbolic tangent of the specified angle.
        //
        // Parameters:
        //   value:
        //     An angle, measured in radians.
        //
        // Returns:
        //     The hyperbolic tangent of value. If value is equal to System.Double.NegativeInfinity,
        //     this method returns -1. If value is equal to System.Double.PositiveInfinity,
        //     this method returns 1. If value is equal to System.Double.NaN, this method
        //     returns System.Double.NaN.

        public static extern double Tanh(double value);
        //
        // Summary:
        //     Calculates the integral part of a specified double-precision floating-point
        //     number.
        //
        // Parameters:
        //   d:
        //     A number to truncate.
        //
        // Returns:
        //     The integral part of d; that is, the number that remains after any fractional
        //     digits have been discarded.

        public static extern double Truncate(double d);
    }
}


