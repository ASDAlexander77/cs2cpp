extern "C" void GC_init();
extern "C" void* GC_malloc(UInt32);
extern "C" void* GC_malloc_atomic(UInt32);
extern "C" void* GC_malloc_ignore_off_page(UInt32);
extern "C" void* GC_malloc_atomic_ignore_off_page(UInt32);

#undef GC_MALLOC
#define GC_MALLOC GC_malloc

#undef GC_MALLOC_ATOMIC
#define GC_MALLOC_ATOMIC GC_malloc_atomic

#undef GC_MALLOC_IGNORE_OFF_PAGE
#define GC_MALLOC_IGNORE_OFF_PAGE GC_malloc_ignore_off_page

#undef GC_MALLOC_ATOMIC_IGNORE_OFF_PAGE
#define GC_MALLOC_ATOMIC_IGNORE_OFF_PAGE GC_malloc_atomic_ignore_off_page

#undef GC_INIT
#define GC_INIT GC_init