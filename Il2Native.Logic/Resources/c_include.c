#include <cstdint>
#include <type_traits>
#include <functional>
#include <initializer_list>
#include <limits>
#include <cmath>
#include <cstring>
#include <cwchar>
#include <numeric>
#include <atomic>
#include <thread>
#include <chrono> 
#include <unordered_map>
#include <mutex>
#include <condition_variable>
#include <atomic>
#include <cstdlib>

#ifndef thread_local
# if __STDC_VERSION__ >= 201112 && !defined __STDC_NO_THREADS__
#  define thread_local _Thread_local
# elif defined _WIN32 && ( \
	defined _MSC_VER || \
	defined __ICL || \
	defined __DMC__ || \
	defined __BORLANDC__ )
#  define thread_local __declspec(thread) 
/* note that ICC (linux) and Clang are covered by __GNUC__ */
# elif defined __GNUC__ || \
	defined __SUNPRO_C || \
	defined __xlC__
#  define thread_local __thread
# else
#  error "Cannot define thread_local"
# endif
#endif

#ifdef HAVE_CONFIG_H
# include "config.h"
#endif

#ifndef NDEBUG
# define GC_DEBUG
#endif

#ifndef GC_THREADS
# define GC_THREADS
#endif

#if defined _WIN32 || defined _WIN64 || defined PLATFORM_ANDROID || defined __ANDROID__
#define GC_NOT_DLL
#endif

#include "gc.h"
#include "gc_typed.h"
#include "javaxfc.h"

#ifdef _MSC_VER
#include <Windows.h>
#endif

#undef min
#undef max
#undef INVALID_HANDLE_VALUE
#undef ZeroMemory
#undef Yield
#undef MemoryBarrier
#undef CreateEvent
#undef GetFullPathName
#undef CreateFile
#undef HKEY_CLASSES_ROOT
#undef HKEY_CURRENT_USER
#undef HKEY_LOCAL_MACHINE
#undef HKEY_USERS
#undef HKEY_PERFORMANCE_DATA
#undef HKEY_CURRENT_CONFIG
#undef HKEY_DYN_DATA
#undef DEBUG

#ifdef GC_ADD_CALLER
# define GC_ALLOC_PARAMS GC_RETURN_ADDR, _file, _line
#else
# define GC_ALLOC_PARAMS _file, _line
#endif

#ifndef _WIN32
#define __stdcall
#endif

#if defined PLATFORM_ANDROID || defined __ANDROID__
// timed_mutex not supported
#define NO_TIMED_MUTEX
#endif