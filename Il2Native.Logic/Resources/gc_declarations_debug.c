#undef GC_MALLOC
#undef GC_MALLOC_ATOMIC
#undef GC_MALLOC_IGNORE_OFF_PAGE
#undef GC_MALLOC_ATOMIC_IGNORE_OFF_PAGE
#undef GC_REGISTER_FINALIZER

#undef GC_PTHREAD_CREATE
#define GC_PTHREAD_CREATE GC_pthread_create

#undef GC_PTHREAD_SIGMASK
#define GC_PTHREAD_SIGMASK GC_pthread_sigmask

#undef GC_PTHREAD_JOIN
#define GC_PTHREAD_JOIN GC_pthread_join

#undef GC_PTHREAD_DETACH
#define GC_PTHREAD_DETACH GC_pthread_detach

#undef GC_PTHREAD_CANCEL
#define GC_PTHREAD_CANCEL pthread_cancel

#undef GC_PTHREAD_KILL
#define GC_PTHREAD_KILL pthread_kill

#undef GC_PTHREAD_EXIT
#define GC_PTHREAD_EXIT GC_pthread_exit

#ifdef HAVE_CONFIG_H
# include "config.h"
#endif

#ifndef GC_DEBUG
# define GC_DEBUG
#endif

#ifndef DEBUG_THREADS
# define DEBUG_THREADS
#endif

#ifndef GC_PTHREADS
# define GC_PTHREADS
#endif

#define GC_NO_THREAD_REDIRECTS 1

#include "gc.h"

#ifdef GC_ADD_CALLER
# define __GC_EXTRAS GC_RETURN_ADDR, (const char*)__file, __line
#else
# define __GC_EXTRAS (const char*)__file, __line
#endif

#undef GC_MALLOC
#undef GC_MALLOC_ATOMIC
#undef GC_MALLOC_IGNORE_OFF_PAGE
#undef GC_MALLOC_ATOMIC_IGNORE_OFF_PAGE
#define GC_MALLOC(sz) GC_debug_malloc(sz, __GC_EXTRAS)
#define GC_MALLOC_ATOMIC(sz) GC_debug_malloc_atomic(sz, __GC_EXTRAS)
#define GC_MALLOC_IGNORE_OFF_PAGE(sz) GC_debug_malloc_ignore_off_page(sz, __GC_EXTRAS)
#define GC_MALLOC_ATOMIC_IGNORE_OFF_PAGE(sz) GC_debug_malloc_atomic_ignore_off_page(sz, __GC_EXTRAS)

#define GC_MALLOC_ORIGINAL(sz) GC_debug_malloc(sz, GC_EXTRAS)

#define __GC_MEMORY_DEBUG 1