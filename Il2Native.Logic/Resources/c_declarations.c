#ifdef _MSC_VER
	typedef __int8 int8_t;
	typedef __int16 int16_t;
	typedef __int32 int32_t;
	typedef __int64 int64_t;
	
#elif __GNUC__ >= 3
	typedef signed char int8_t;
	typedef short int16_t;
	typedef int int32_t;
	typedef long long int64_t;
	extern void *__builtin_memset(void *,int,unsigned int);
	#define memset __builtin_memset
#endif

typedef bool i1;
typedef int8_t i8;
typedef int16_t i16;
typedef int32_t i32;
typedef int64_t i64;

typedef i32 (*anyFn)(...);

extern i8* malloc(i32);
