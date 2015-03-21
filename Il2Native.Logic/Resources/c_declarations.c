#ifdef _MSC_VER
	typedef __int8 int8_t;
	typedef __int16 int16_t;
	typedef __int32 int32_t;
	typedef __int64 int64_t;
	typedef unsigned __int8 uint8_t;
	typedef unsigned __int16 uint16_t;
	typedef unsigned __int32 uint32_t;
	typedef unsigned __int64 uint64_t;
	
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
	extern void *__builtin_memcpy(void *,void *,uint32_t);
	#define memcpy __builtin_memcpy
	#define compare_and_swap __sync_val_compare_and_swap 
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

typedef Int32 Int;
typedef UInt32 UInt;

typedef Int32 (*anyFn)(...);

extern "C" Void* alloca(UInt);
extern "C" Byte* calloc(UInt);
extern "C" Void* __dynamic_cast(Void*, Void*, Void*, Int32);
extern "C" Void __cxa_pure_virtual();

// RTTI externals
extern "C" void* _ZTVN10__cxxabiv117__class_type_infoE;
extern "C" void* _ZTVN10__cxxabiv119__pointer_type_infoE;
extern "C" void* _ZTVN10__cxxabiv120__si_class_type_infoE;
extern "C" void* _ZTVN10__cxxabiv121__vmi_class_type_infoE;
extern "C" void* _ZTVN10__cxxabiv129__pointer_to_member_type_infoE;

inline Void* __dynamic_cast_null_test(Void* src, Void* rttiFrom, Void* rttiTo, Int32 offset)
{
	if (!src)
	{
		return 0;
	}

	return __dynamic_cast(src, rttiFrom, rttiTo, offset);
}
