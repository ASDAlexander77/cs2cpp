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
extern void *__builtin_memset(void *,int32_t,uint32_t);
#define memset __builtin_memset
extern void *__builtin_memcpy(void *,const void *,uint32_t);
#define compare_and_swap __sync_val_compare_and_swap 
#define sync_synchronize __sync_synchronize
#define fetch_and_add __sync_fetch_and_add
#define fetch_and_sub __sync_fetch_and_sub
#define swap __sync_lock_test_and_set
#define alloca __builtin_alloca

inline void* memcpy(void* dst, void* src, int32_t size)
{
	return __builtin_memcpy(dst, src, size);
}

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

#define GC_MALLOC calloc
#define GC_MALLOC_ATOMIC calloc
#define GC_MALLOC_IGNORE_OFF_PAGE calloc
#define GC_MALLOC_ATOMIC_IGNORE_OFF_PAGE calloc

extern "C" Byte* calloc(UInt32);
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

struct System_InvalidCastException;
extern "C" System_InvalidCastException* System_InvalidCastException_System_InvalidCastException__newFN();
extern "C" Void Void_System_InvalidCastException__ctorFN(System_InvalidCastException* __this);
inline Void* __dynamic_cast_null_test_throw(Void* src, Void* rttiFrom, Void* rttiTo, Int32 offset)
{
	if (!src)
	{
		return 0;
	}

	Void* casted = __dynamic_cast(src, rttiFrom, rttiTo, offset);
	if (!casted)
	{
		System_InvalidCastException* _new0;
		_new0 = System_InvalidCastException_System_InvalidCastException__newFN();
		Void_System_InvalidCastException__ctorFN(_new0);
		throw (Void*) _new0;
	}

	return casted;
}

struct System_DivideByZeroException;
extern "C" System_DivideByZeroException* System_DivideByZeroException_System_DivideByZeroException__newFN();
extern "C" Void Void_System_DivideByZeroException__ctorFN(System_DivideByZeroException* __this);
template < typename T > T __safe_divide(T num, T div)
{
	if (!div)
	{
		System_DivideByZeroException* _new0;
		_new0 = System_DivideByZeroException_System_DivideByZeroException__newFN();
		Void_System_DivideByZeroException__ctorFN(_new0);
		throw (Void*) _new0;
	}

	return num / div;
}
