extern "C" void GC_init();
extern "C" void* GC_malloc(u32);
extern "C" void* GC_malloc_atomic(u32);
extern "C" void* GC_realloc(i8*, u32);
extern "C" u32 GC_get_heap_size();