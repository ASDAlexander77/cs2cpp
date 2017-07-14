#include "CoreLib.h"

namespace CoreLib {
	namespace System {

		namespace _ = ::CoreLib::System;

		// Method : System.Math.Acos(double)
		double Math::Acos(double d)
		{
			return std::acos(d);
		}

		// Method : System.Math.Asin(double)
		double Math::Asin(double d)
		{
			return std::asin(d);
		}

		// Method : System.Math.Atan(double)
		double Math::Atan(double d)
		{
			return std::atan(d);
		}

		// Method : System.Math.Atan2(double, double)
		double Math::Atan2(double y, double x)
		{
			return std::atan2(y, x);
		}

		// Method : System.Math.Ceiling(double)
		double Math::Ceiling(double a)
		{
			return std::ceil(a);
		}

		// Method : System.Math.Cos(double)
		double Math::Cos(double d)
		{
			return std::cos(d);
		}

		// Method : System.Math.Cosh(double)
		double Math::Cosh(double value)
		{
			return std::cosh(value);
		}

		// Method : System.Math.Floor(double)
		double Math::Floor(double d)
		{
			return std::floor(d);
		}

		// Method : System.Math.Sin(double)
		double Math::Sin(double a)
		{
			return std::sin(a);
		}

		// Method : System.Math.Tan(double)
		double Math::Tan(double a)
		{
			return std::tan(a);
		}

		// Method : System.Math.Sinh(double)
		double Math::Sinh(double value)
		{
			return std::sinh(value);
		}

		// Method : System.Math.Tanh(double)
		double Math::Tanh(double value)
		{
			return std::tanh(value);
		}

		// Method : System.Math.Round(double)
		double Math::Round(double a)
		{
#if defined(__ANDROID__) || defined(PLATFORM_ANDROID)
			return std::floor(a + 0.5);
#else
			return std::round(a);
#endif
		}

		// Method : System.Math.SplitFractionDouble(double*)
		double Math::SplitFractionDouble(double* value)
		{
			throw 0xC000C000;
		}

		// Method : System.Math.Sqrt(double)
		double Math::Sqrt(double d)
		{
			return std::sqrt(d);
		}

		// Method : System.Math.Log(double)
		double Math::Log(double d)
		{
			return std::log(d);
		}

		// Method : System.Math.Log10(double)
		double Math::Log10(double d)
		{
			return std::log10(d);
		}

		// Method : System.Math.Exp(double)
		double Math::Exp(double d)
		{
			return std::exp(d);
		}

		// Method : System.Math.Pow(double, double)
		double Math::Pow(double x, double y)
		{
			return std::pow(x, y);
		}

		// Method : System.Math.Abs(float)
		float Math::Abs(float value)
		{
			return std::abs(value);
		}

		// Method : System.Math.Abs(double)
		double Math::Abs(double value)
		{
			return std::abs(value);
		}
	}
}