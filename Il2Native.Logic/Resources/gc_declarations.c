extern "C" void GC_init();
extern "C" void* GC_malloc(UInt32);
extern "C" void* GC_malloc_atomic(UInt32);
extern "C" void* GC_realloc(Byte*, UInt32);
extern "C" UInt32 GC_get_heap_size();