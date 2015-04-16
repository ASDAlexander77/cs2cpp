extern "C" void GC_init();
extern "C" void* GC_malloc(UInt32);

#undef GC_MALLOC
#define GC_MALLOC GC_malloc

#undef GC_INIT
#define GC_INIT GC_init