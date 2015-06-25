#undef GC_MALLOC
#undef GC_MALLOC_ATOMIC
#undef GC_MALLOC_IGNORE_OFF_PAGE
#undef GC_MALLOC_ATOMIC_IGNORE_OFF_PAGE
#undef GC_REGISTER_FINALIZER

#ifdef HAVE_CONFIG_H
# include "config.h"
#endif

#ifndef GC_DEBUG
# define GC_DEBUG
#endif

#ifndef GC_THREADS
# define GC_THREADS
#endif

#define GC_NO_THREAD_REDIRECTS 1

#include "gc.h"