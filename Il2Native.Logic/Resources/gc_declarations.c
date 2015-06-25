extern "C" Void GC_init();
extern "C" Void* GC_malloc(UInt32);
extern "C" Void* GC_malloc_atomic(UInt32);
extern "C" Void* GC_malloc_ignore_off_page(UInt32);
extern "C" Void* GC_malloc_atomic_ignore_off_page(UInt32);
typedef void (* __finalization_proc__)(Void* /* obj */, Void* /* client_data */);
extern "C" Void GC_register_finalizer(Void*, __finalization_proc__, Void*, Void*, Void**);

#undef GC_MALLOC
#define GC_MALLOC GC_malloc

#undef GC_MALLOC_ATOMIC
#define GC_MALLOC_ATOMIC GC_malloc_atomic

#undef GC_MALLOC_IGNORE_OFF_PAGE
#define GC_MALLOC_IGNORE_OFF_PAGE GC_malloc_ignore_off_page

#undef GC_MALLOC_ATOMIC_IGNORE_OFF_PAGE
#define GC_MALLOC_ATOMIC_IGNORE_OFF_PAGE GC_malloc_atomic_ignore_off_page

#define GC_REGISTER_FINALIZER GC_register_finalizer

#undef GC_INIT
#define GC_INIT GC_init

#undef GC_PTHREAD_CREATE
#define GC_PTHREAD_CREATE GC_pthread_create

#undef GC_PTHREAD_SIGMASK
#define GC_PTHREAD_SIGMASK GC_pthread_sigmask

#undef GC_PTHREAD_JOIN
#define GC_PTHREAD_JOIN GC_pthread_join

#undef GC_PTHREAD_DETACH
#define GC_PTHREAD_DETACH GC_pthread_detach

#undef GC_PTHREAD_CANCEL
#define GC_PTHREAD_CANCEL GC_pthread_cancel

#undef GC_PTHREAD_EXIT
#define GC_PTHREAD_EXIT GC_pthread_exit
