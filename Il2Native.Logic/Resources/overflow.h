template < typename T >
inline typename std::enable_if<std::is_signed<T>::value && std::is_integral<T>::value, T>::type checked_unary_minus(T operand)
{
	if (operand == std::numeric_limits<T>::min())
	{
		throw __new<CoreLib::System::OverflowException>();
	}

	return -operand;
}

template < typename T >
inline typename std::enable_if<!std::is_signed<T>::value || !std::is_integral<T>::value, T>::type checked_unary_minus(T operand)
{
	return -operand;
}

template < typename D, typename S >
inline D checked_static_cast(S operand)
{
	if (operand < std::numeric_limits<D>::min() || operand > std::numeric_limits<D>::max())
	{
		throw __new<CoreLib::System::OverflowException>();
	}

	return static_cast<D>(operand);
}

inline int8_t __add_ovf(int8_t a, int8_t b)
{
	int8_t s = (uint8_t) a + (uint8_t) b;
	if (b >= 0)
	{
		if (s < a)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}
	else
	{
		if (s >= a)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}

	return s;
}

inline int16_t __add_ovf(int16_t a, int16_t b)
{
	int16_t s = (uint16_t) a + (uint16_t) b;
	if (b >= 0)
	{
		if (s < a)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}
	else
	{
		if (s >= a)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}

	return s;
}

inline int32_t __add_ovf(int32_t a, int32_t b)
{
	int32_t s = (uint32_t) a + (uint32_t) b;
	if (b >= 0)
	{
		if (s < a)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}
	else
	{
		if (s >= a)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}

	return s;
}

inline int64_t __add_ovf(int64_t a, int64_t b)
{
	int64_t s = (uint64_t) a + (uint64_t) b;
	if (b >= 0)
	{
		if (s < a)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}
	else
	{
		if (s >= a)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}

	return s;
}

template < typename T > inline T __add_ovf_un(T a, T b)
{
	if ((T)-1 - (T)a <= (T) b)
	{
		throw __new<CoreLib::System::OverflowException>();
	}

	return a + b;
}

inline int8_t __sub_ovf(int8_t a, int8_t b)
{
	int8_t s = (uint8_t) a - (uint8_t) b;
	if (b >= 0)
	{
		if (s > a)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}
	else
	{
		if (s <= a)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}

	return s;
}

inline uint8_t __sub_ovf(uint8_t a, uint8_t b)
{
	if (a < b)
	{
		throw __new<CoreLib::System::OverflowException>();
	}

	return a - b;
}

inline int16_t __sub_ovf(int16_t a, int16_t b)
{
	int16_t s = (uint16_t) a - (uint16_t) b;
	if (b >= 0)
	{
		if (s > a)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}
	else
	{
		if (s <= a)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}

	return s;
}

inline char16_t __sub_ovf(char16_t a, char16_t b)
{
	if (a < b)
	{
		throw __new<CoreLib::System::OverflowException>();
	}

	return a - b;
}

inline int32_t __sub_ovf(int32_t a, int32_t b)
{
	int32_t s = (uint32_t) a - (uint32_t) b;
	if (b >= 0)
	{
		if (s > a)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}
	else
	{
		if (s <= a)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}

	return s;
}

inline int64_t __sub_ovf(int64_t a, int64_t b)
{
	int64_t s = (uint64_t) a - (uint64_t) b;
	if (b >= 0)
	{
		if (s > a)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}
	else
	{
		if (s <= a)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}

	return s;
}

template < typename T > inline T __sub_ovf_un(T a, T b)
{
	if ((T)a < (T) b)
	{
		throw __new<CoreLib::System::OverflowException>();
	}

	return a - b;
}

#ifndef CHAR_BIT
# define CHAR_BIT 8
#endif
template < typename T > inline T __mul_ovf(T a, T b)
{
	const int N = (int)(sizeof(T) * CHAR_BIT);
	const T MIN = (T)1 << (N-1);
	const T MAX = ~MIN;
	if (a == MIN)
	{
		if (b == 0 || b == 1)
		{
			return a * b;
		}

		throw __new<CoreLib::System::OverflowException>();
	}
	if (b == MIN)
	{
		if (a == 0 || a == 1)
		{
			return a * b;
		}

		throw __new<CoreLib::System::OverflowException>();
	}

	T sa = a >> (N - 1);
	T abs_a = (a ^ sa) - sa;
	T sb = b >> (N - 1);
	T abs_b = (b ^ sb) - sb;
	if (abs_a < 2 || abs_b < 2)
		return a * b;
	if (sa == sb)
	{
		if (abs_a > MAX / abs_b)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}
	else
	{
		if (abs_a > MIN / -abs_b)
		{
			throw __new<CoreLib::System::OverflowException>();
		}
	}

	return a * b;
}

inline uint8_t __mul_ovf_un(uint8_t a, uint8_t b)
{
	if (a != 0 && b > ((uint8_t)-1) / a)
	{
		throw __new<CoreLib::System::OverflowException>();
	}

	return a * b;
}

inline uint16_t __mul_ovf_un(uint16_t a, uint16_t b)
{
	if (a != 0 && b > ((uint16_t)-1) / a)
	{
		throw __new<CoreLib::System::OverflowException>();
	}

	return a * b;
}

inline uint32_t __mul_ovf_un(uint32_t a, uint32_t b)
{
	if (a != 0 && b > ((uint32_t)-1) / a)
	{
		throw __new<CoreLib::System::OverflowException>();
	}

	return a * b;
}

inline uint64_t __mul_ovf_un(uint64_t a, uint64_t b)
{
	if (a != 0 && b > ((uint64_t)-1) / a)
	{
		throw __new<CoreLib::System::OverflowException>();
	}

	return a * b;
}

inline int8_t __mul_ovf_un(int8_t a, int8_t b)
{
	if ((uint8_t)a != 0 && (uint8_t)b > ((uint8_t)-1) / (uint8_t)a)
	{
		throw __new<CoreLib::System::OverflowException>();
	}

	return a * b;
}

inline int16_t __mul_ovf_un(int16_t a, uint16_t b)
{
	if ((uint16_t)a != 0 && (uint16_t)b > ((uint16_t)-1) / (uint16_t)a)
	{
		throw __new<CoreLib::System::OverflowException>();
	}

	return a * b;
}

inline int32_t __mul_ovf_un(int32_t a, int32_t b)
{
	if ((uint32_t)a != 0 && (uint32_t)b > ((uint32_t)-1) / (uint32_t)a)
	{
		throw __new<CoreLib::System::OverflowException>();
	}

	return a * b;
}

inline int64_t __mul_ovf_un(int64_t a, int64_t b)
{
	if ((uint64_t)a != 0 && (uint64_t)b > ((uint64_t)-1) / (uint64_t)a)
	{
		throw __new<CoreLib::System::OverflowException>();
	}

	return a * b;
}
