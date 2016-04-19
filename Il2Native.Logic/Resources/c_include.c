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
#include <shared_mutex>
#include <condition_variable>
#include <atomic>

#ifdef _MSC_VER
#include <intrin.h>
#endif

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

#ifdef DEBUG
# define GC_DEBUG
#endif

#ifndef GC_THREADS
# define GC_THREADS
#endif

#include "gc.h"
#include "javaxfc.h"
#undef min
#undef max
#undef INVALID_HANDLE_VALUE
#undef ZeroMemory
#undef Yield
#undef MemoryBarrier
#undef CreateEvent
#undef GetFullPathName
#undef CreateFile
