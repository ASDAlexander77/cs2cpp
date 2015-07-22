extern "C" Void* Memcpy(Byte* dst, Byte* src, Int32 size)
{
	return __builtin_memcpy(dst, src, size);
}

extern "C" Void Exit(Int32 status)
{
	return exit(status);
}

extern "C" Void __pure_virtual()
{
	__throw_invalid_operation();
}


#if defined(_WIN32) || defined(__WIN32__) || defined(WIN32)
extern "C" Byte* _fullpath(Byte* absPath, const Byte* relPath, Int32 maxLength);
extern "C" Byte* __get_full_path(Byte* partial, Byte* full)
{
	return (Byte*)_fullpath(full, partial, 260);
}
#else
extern "C" Byte* realpath(Byte* path, const Byte* resolved_path);
extern "C" Byte* __get_full_path(Byte* partial, Byte* full)
{
	return (Byte*)realpath(partial, full);
}
#endif
