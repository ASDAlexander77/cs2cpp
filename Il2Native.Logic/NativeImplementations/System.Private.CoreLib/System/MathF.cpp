#include "System.Private.CoreLib.h"

namespace CoreLib { namespace System { 
    namespace _ = ::CoreLib::System;
    // Method : System.MathF.Acos(float)
    float MathF::Acos(float x)
    {
		return std::acos(x);
    }
    
    // Method : System.MathF.Asin(float)
    float MathF::Asin(float x)
    {
		return std::asin(x);
    }
    
    // Method : System.MathF.Atan(float)
    float MathF::Atan(float x)
    {
		return std::atan(x);
    }
    
    // Method : System.MathF.Atan2(float, float)
    float MathF::Atan2(float y, float x)
    {
		return std::atan2(y, x);
    }
    
    // Method : System.MathF.Ceiling(float)
    float MathF::Ceiling(float x)
    {
		return std::ceil(x);
    }
    
    // Method : System.MathF.Cos(float)
    float MathF::Cos(float x)
    {
		return std::cos(x);
    }
    
    // Method : System.MathF.Cosh(float)
    float MathF::Cosh(float x)
    {
		return std::cosh(x);
    }
    
    // Method : System.MathF.Exp(float)
    float MathF::Exp(float x)
    {
		return std::exp(x);
    }
    
    // Method : System.MathF.Floor(float)
    float MathF::Floor(float x)
    {
		return std::floor(x);
    }
    
    // Method : System.MathF.Log(float)
    float MathF::Log(float x)
    {
		return std::log(x);
    }
    
    // Method : System.MathF.Log10(float)
    float MathF::Log10(float x)
    {
		return std::log10(x);
    }
    
    // Method : System.MathF.Pow(float, float)
    float MathF::Pow(float x, float y)
    {
		return std::pow(x, y);
    }
    
    // Method : System.MathF.Round(float)
    float MathF::Round(float x)
    {
#if defined(__ANDROID__) || defined(PLATFORM_ANDROID)
		return std::floor(x + 0.5);
#else
		return std::round(x);
#endif
    }
    
    // Method : System.MathF.Sin(float)
    float MathF::Sin(float x)
    {
		return std::sin(x);
    }
    
    // Method : System.MathF.Sinh(float)
    float MathF::Sinh(float x)
    {
		return std::sinh(x);
    }
    
    // Method : System.MathF.Sqrt(float)
    float MathF::Sqrt(float x)
    {
		return std::sqrt(x);
    }
    
    // Method : System.MathF.Tan(float)
    float MathF::Tan(float x)
    {
		return std::tan(x);
    }
    
    // Method : System.MathF.Tanh(float)
    float MathF::Tanh(float x)
    {
		return std::tanh(x);
    }
    
    // Method : System.MathF.SplitFractionSingle(float*)
    float MathF::SplitFractionSingle(float* x)
    {
		return std::modf(*x, x);
    }

}}

namespace CoreLib { namespace System { 
    namespace _ = ::CoreLib::System;
}}
