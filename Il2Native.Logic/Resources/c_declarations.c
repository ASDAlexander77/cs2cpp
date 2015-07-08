#ifdef _MSC_VER
#error Not supported yet
#elif __GNUC__ >= 3
typedef signed char int8_t;
typedef short int16_t;
typedef int int32_t;
typedef long long int64_t;
typedef unsigned char uint8_t;
typedef unsigned short uint16_t;
typedef unsigned int uint32_t;
typedef unsigned long long uint64_t;

#define compare_and_swap __sync_val_compare_and_swap 
#define sync_synchronize __sync_synchronize
#define fetch_and_add __sync_fetch_and_add
#define fetch_and_sub __sync_fetch_and_sub
#define swap __sync_lock_test_and_set
#define alloca __builtin_alloca

extern void *__builtin_memset(void *,int32_t,uint32_t);
#define Memset __builtin_memset

extern void *__builtin_memcpy(void *,const void *,uint32_t);

#endif

typedef void Void;
typedef bool Boolean;
typedef int8_t SByte;
typedef int16_t Int16;
typedef int32_t Int32;
typedef int64_t Int64;
typedef uint8_t Byte;
typedef uint16_t Char;
typedef uint16_t UInt16;
typedef uint32_t UInt32;
typedef uint64_t UInt64;
typedef float Single;
typedef double Double;

#define GC_MALLOC __alloc
#define GC_MALLOC_ATOMIC __alloc
#define GC_MALLOC_IGNORE_OFF_PAGE __alloc
#define GC_MALLOC_ATOMIC_IGNORE_OFF_PAGE __alloc
#define GC_PTHREAD_CREATE pthread_create
#define GC_PTHREAD_SIGMASK pthread_sigmask
#define GC_PTHREAD_JOIN pthread_join
#define GC_PTHREAD_DETACH pthread_detach
#define GC_PTHREAD_CANCEL pthread_cancel
#define GC_PTHREAD_KILL pthread_kill
#define GC_PTHREAD_EXIT pthread_exit

extern "C" Void* calloc(UInt32, UInt32);
extern "C" Void exit(Int32 status);
extern "C" Void* __dynamic_cast(Void*, Void*, Void*, Int32);
extern "C" Void __cxa_pure_virtual();

// RTTI externals
extern "C" void* _ZTVN10__cxxabiv117__class_type_infoE;
extern "C" void* _ZTVN10__cxxabiv119__pointer_type_infoE;
extern "C" void* _ZTVN10__cxxabiv120__si_class_type_infoE;
extern "C" void* _ZTVN10__cxxabiv121__vmi_class_type_infoE;
extern "C" void* _ZTVN10__cxxabiv129__pointer_to_member_type_infoE;

// Float
extern "C" Double fmod (Double, Double);

inline Void* __alloc(UInt32 size)
{
	return (Void*) calloc(1, size);
}

inline Void* __interface_to_object(Void* _interface)
{
	if (!_interface)
	{
		return 0;
	}

	return (Void*) ((Byte*)_interface + *(*(Int32**)_interface - 2));
}

inline Void* __dynamic_cast_null_test(Void* src, Void* rttiFrom, Void* rttiTo, Int32 offset)
{
	if (!src)
	{
		return 0;
	}

	return __dynamic_cast(src, rttiFrom, rttiTo, offset);
}

inline Void* __get_mutex_address(Void* _object)
{
	return (Void*) ((Void**)_object - 1);
}

inline Void* __get_cond_address(Void* _object)
{
	return (Void*) ((Void**)_object - 2);
}

class System_Object;
extern "C" Int32 pthread_key_create(Int32* key, Void* destructor);
extern "C" Int32 pthread_key_delete(Int32 key);
extern "C" System_Object* pthread_getspecific(Int32 key);
extern "C" Int32 pthread_setspecific(Int32 key, System_Object* value);

inline Int32 __create_thread_static(Int32* key, Void* destructor)
{
	return pthread_key_create(key, destructor);
}

inline Int32 __delete_thread_static(Int32 key)
{
	return pthread_key_delete(key);
}

inline System_Object* __get_thread_static(Int32 key)
{
	return pthread_getspecific(key);
}

inline Void __set_thread_static(Int32 key, System_Object* _object)
{
	pthread_setspecific(key, _object);
}

struct System_InvalidCastException;
Void Void_System_InvalidCastException__ctorFN(System_InvalidCastException* __this);
System_InvalidCastException* System_InvalidCastException_System_InvalidCastException__newFN();
inline Void* __dynamic_cast_null_test_throw(Void* src, Void* rttiFrom, Void* rttiTo, Int32 offset)
{
	if (!src)
	{
		return 0;
	}

	Void* casted = __dynamic_cast(src, rttiFrom, rttiTo, offset);
	if (!casted)
	{
		System_InvalidCastException* _new;
		_new = System_InvalidCastException_System_InvalidCastException__newFN();
		Void_System_InvalidCastException__ctorFN(_new);
		throw (::Void*) _new;
	}

	return casted;
}

struct System_DivideByZeroException;
Void Void_System_DivideByZeroException__ctorFN(System_DivideByZeroException* __this);
System_DivideByZeroException* System_DivideByZeroException_System_DivideByZeroException__newFN();
template < typename T > T __check_divide(T div)
{
	if (!div)
	{
		System_DivideByZeroException* _new;
		_new = System_DivideByZeroException_System_DivideByZeroException__newFN();
		Void_System_DivideByZeroException__ctorFN(_new);
		throw (::Void*) _new;
	}

	return div;
}

struct System_NotSupportedException;
Void Void_System_NotSupportedException__ctorFN(System_NotSupportedException* __this);
System_NotSupportedException* System_NotSupportedException_System_NotSupportedException__newFN();
inline Void* __throw_not_supported()
{
	System_NotSupportedException* _new;
	_new = System_NotSupportedException_System_NotSupportedException__newFN();
	Void_System_NotSupportedException__ctorFN(_new);
	throw (::Void*) _new;
}

struct System_OverflowException;
Void Void_System_OverflowException__ctorFN(System_OverflowException* __this);
System_OverflowException* System_OverflowException_System_OverflowException__newFN();

inline SByte __add_ovf(SByte a, SByte b)
{
	SByte s = (Byte) a + (Byte) b;
	if (b >= 0)
	{
		if (s < a)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}
	else
	{
		if (s >= a)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}

	return s;
}

inline Int16 __add_ovf(Int16 a, Int16 b)
{
	Int16 s = (UInt16) a + (UInt16) b;
	if (b >= 0)
	{
		if (s < a)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}
	else
	{
		if (s >= a)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}

	return s;
}

inline Int32 __add_ovf(Int32 a, Int32 b)
{
	Int32 s = (UInt32) a + (UInt32) b;
	if (b >= 0)
	{
		if (s < a)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}
	else
	{
		if (s >= a)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}

	return s;
}

inline Int64 __add_ovf(Int64 a, Int64 b)
{
	Int64 s = (UInt64) a + (UInt64) b;
	if (b >= 0)
	{
		if (s < a)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}
	else
	{
		if (s >= a)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}

	return s;
}

template < typename T > inline T __add_ovf_un(T a, T b)
{
	if ((T)-1 - (T)a <= (T) b)
	{
		System_OverflowException* _new;
		_new = System_OverflowException_System_OverflowException__newFN();
		Void_System_OverflowException__ctorFN(_new);
		throw (::Void*) _new;
	}

	return a + b;
}

inline SByte __sub_ovf(SByte a, SByte b)
{
	SByte s = (Byte) a - (Byte) b;
	if (b >= 0)
	{
		if (s > a)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}
	else
	{
		if (s <= a)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}

	return s;
}

inline Byte __sub_ovf(Byte a, Byte b)
{
	if (a < b)
	{
		System_OverflowException* _new;
		_new = System_OverflowException_System_OverflowException__newFN();
		Void_System_OverflowException__ctorFN(_new);
		throw (::Void*) _new;
	}

	return a - b;
}

inline Int16 __sub_ovf(Int16 a, Int16 b)
{
	Int16 s = (UInt16) a - (UInt16) b;
	if (b >= 0)
	{
		if (s > a)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}
	else
	{
		if (s <= a)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}

	return s;
}

inline Char __sub_ovf(Char a, Char b)
{
	if (a < b)
	{
		System_OverflowException* _new;
		_new = System_OverflowException_System_OverflowException__newFN();
		Void_System_OverflowException__ctorFN(_new);
		throw (::Void*) _new;
	}

	return a - b;
}

inline Int32 __sub_ovf(Int32 a, Int32 b)
{
	Int32 s = (UInt32) a - (UInt32) b;
	if (b >= 0)
	{
		if (s > a)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}
	else
	{
		if (s <= a)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}

	return s;
}

inline Int64 __sub_ovf(Int64 a, Int64 b)
{
	Int64 s = (UInt64) a - (UInt64) b;
	if (b >= 0)
	{
		if (s > a)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}
	else
	{
		if (s <= a)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}

	return s;
}

template < typename T > inline T __sub_ovf_un(T a, T b)
{
	if ((T)a < (T) b)
	{
		System_OverflowException* _new;
		_new = System_OverflowException_System_OverflowException__newFN();
		Void_System_OverflowException__ctorFN(_new);
		throw (::Void*) _new;
	}

	return a - b;
}

#define CHAR_BIT 8
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

		System_OverflowException* _new;
		_new = System_OverflowException_System_OverflowException__newFN();
		Void_System_OverflowException__ctorFN(_new);
		throw (::Void*) _new;
	}
	if (b == MIN)
	{
		if (a == 0 || a == 1)
		{
			return a * b;
		}

		System_OverflowException* _new;
		_new = System_OverflowException_System_OverflowException__newFN();
		Void_System_OverflowException__ctorFN(_new);
		throw (::Void*) _new;
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
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}
	else
	{
		if (abs_a > MIN / -abs_b)
		{
			System_OverflowException* _new;
			_new = System_OverflowException_System_OverflowException__newFN();
			Void_System_OverflowException__ctorFN(_new);
			throw (::Void*) _new;
		}
	}

	return a * b;
}

inline Byte __mul_ovf_un(Byte a, Byte b)
{
	if (a != 0 && b > ((Byte)-1) / a)
	{
		System_OverflowException* _new;
		_new = System_OverflowException_System_OverflowException__newFN();
		Void_System_OverflowException__ctorFN(_new);
		throw (::Void*) _new;
	}

	return a * b;
}

inline UInt16 __mul_ovf_un(UInt16 a, UInt16 b)
{
	if (a != 0 && b > ((UInt16)-1) / a)
	{
		System_OverflowException* _new;
		_new = System_OverflowException_System_OverflowException__newFN();
		Void_System_OverflowException__ctorFN(_new);
		throw (::Void*) _new;
	}

	return a * b;
}

inline UInt32 __mul_ovf_un(UInt32 a, UInt32 b)
{
	if (a != 0 && b > ((UInt32)-1) / a)
	{
		System_OverflowException* _new;
		_new = System_OverflowException_System_OverflowException__newFN();
		Void_System_OverflowException__ctorFN(_new);
		throw (::Void*) _new;
	}

	return a * b;
}

inline UInt64 __mul_ovf_un(UInt64 a, UInt64 b)
{
	if (a != 0 && b > ((UInt64)-1) / a)
	{
		System_OverflowException* _new;
		_new = System_OverflowException_System_OverflowException__newFN();
		Void_System_OverflowException__ctorFN(_new);
		throw (::Void*) _new;
	}

	return a * b;
}

inline SByte __mul_ovf_un(SByte a, SByte b)
{
	if ((Byte)a != 0 && (Byte)b > ((Byte)-1) / (Byte)a)
	{
		System_OverflowException* _new;
		_new = System_OverflowException_System_OverflowException__newFN();
		Void_System_OverflowException__ctorFN(_new);
		throw (::Void*) _new;
	}

	return a * b;
}

inline Int16 __mul_ovf_un(Int16 a, UInt16 b)
{
	if ((UInt16)a != 0 && (UInt16)b > ((UInt16)-1) / (UInt16)a)
	{
		System_OverflowException* _new;
		_new = System_OverflowException_System_OverflowException__newFN();
		Void_System_OverflowException__ctorFN(_new);
		throw (::Void*) _new;
	}

	return a * b;
}

inline Int32 __mul_ovf_un(Int32 a, Int32 b)
{
	if ((UInt32)a != 0 && (UInt32)b > ((UInt32)-1) / (UInt32)a)
	{
		System_OverflowException* _new;
		_new = System_OverflowException_System_OverflowException__newFN();
		Void_System_OverflowException__ctorFN(_new);
		throw (::Void*) _new;
	}

	return a * b;
}

inline Int64 __mul_ovf_un(Int64 a, Int64 b)
{
	if ((UInt64)a != 0 && (UInt64)b > ((UInt64)-1) / (UInt64)a)
	{
		System_OverflowException* _new;
		_new = System_OverflowException_System_OverflowException__newFN();
		Void_System_OverflowException__ctorFN(_new);
		throw (::Void*) _new;
	}

	return a * b;
}