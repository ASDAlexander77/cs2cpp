#ifdef __GC_MEMORY_DEBUG
Void* __new_interface_debug(Void* _object, Void** _vtbl, SByte* __file, Int32 __line) 
{
	__interface_data* __ret_interface;
	__ret_interface = (__interface_data*) GC_MALLOC(sizeof(__interface_data));
	if (__ret_interface) goto _continue;
	System_OutOfMemoryException* _new;
	_new = System_OutOfMemoryException_System_OutOfMemoryException__newFSByteP__Int32N(__file, __line);
	Void_System_OutOfMemoryException__ctorFN(_new);
	throw (Void*) _new;
_continue:
	__ret_interface->__vtbl = _vtbl;
	__ret_interface->__this = _object;
	return (Void*) __ret_interface;
}
#else
Void* __new_interface(Void* _object, Void** _vtbl) 
{
	__interface_data* __ret_interface;
	__ret_interface = (__interface_data*) GC_MALLOC(sizeof(__interface_data));
	if (__ret_interface) goto _continue;
	System_OutOfMemoryException* _new;
	_new = System_OutOfMemoryException_System_OutOfMemoryException__newFN();
	Void_System_OutOfMemoryException__ctorFN(_new);
	throw (Void*) _new;
_continue:
	__ret_interface->__vtbl = _vtbl;
	__ret_interface->__this = _object;
	return (Void*) __ret_interface;
}
#endif

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
