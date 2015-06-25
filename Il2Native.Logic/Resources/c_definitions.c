extern "C" Void* Memcpy(Byte* dst, Byte* src, Int32 size)
{
	return __builtin_memcpy(dst, src, size);
}

extern "C" Void Exit(Int32 status)
{
	return exit(status);
}
