extern "C" void GC_init();
extern "C" void* GC_malloc(UInt);
extern "C" void* GC_malloc_atomic(UInt);
extern "C" void* GC_realloc(Byte*, UInt);
extern "C" UInt GC_get_heap_size();