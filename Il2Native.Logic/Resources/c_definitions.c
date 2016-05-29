__object_extras_storage* __object_extras_storage_instance = nullptr;
__strings_storage* __strings_storage_instance = nullptr;

/*
void* operator new (size_t _size)
{
	return GC_MALLOC_UNCOLLECTABLE(size);
}

void operator delete (void* obj);
{
	GC_FREE(obj);
}

void* operator new[] (size_t _size);
{
	return GC_MALLOC_UNCOLLECTABLE(size);
}

void operator delete[] (void* obj);
{
	GC_FREE(obj);
}
*/

void* operator new (size_t _size, GCNormal)
{
    return __new_set0(_size, GCNormal::Default);
}

void* operator new (size_t _size, GCNormal, const char* _file, int _line)
{
    return __new_set0(_size, GCNormal::Default, _file, _line);
}

void* operator new (size_t _size, int32_t _customSize, GCNormal)
{
    return __new_set0(_customSize, GCNormal::Default);
}

void* operator new (size_t _size, int32_t _customSize, GCNormal, const char* _file, int _line)
{
    return __new_set0(_customSize, GCAtomic::Default, _file, _line);
}

void* operator new (size_t _size, GCAtomic)
{
    return __new_set0(_size, GCAtomic::Default);
}

void* operator new (size_t _size, GCAtomic, const char* _file, int _line)
{
    return __new_set0(_size, GCAtomic::Default, _file, _line);
}

void* operator new (size_t _size, int32_t _customSize, GCAtomic)
{
    return __new_set0(_customSize, GCAtomic::Default);
}

void* operator new (size_t _size, int32_t _customSize, GCAtomic, const char* _file, int _line)
{
    return __new_set0(_customSize, GCAtomic::Default, _file, _line);
}

void* operator new (size_t _size, GC_descr _type_descr)
{
    return __new_set0(_size, _type_descr);
}

void* operator new (size_t _size, GC_descr _type_descr, const char* _file, int _line)
{
    return __new_set0(_size, _type_descr, _file, _line);
}

void* operator new (size_t _size, int32_t _customSize, GC_descr _type_descr)
{
    return __new_set0(_customSize, _type_descr);
}

void* operator new (size_t _size, int32_t _customSize, GC_descr _type_descr, const char* _file, int _line)
{
    return __new_set0(_customSize, _type_descr, _file, _line);
}

void GC_CALLBACK __finalizer(void * obj, void * client_data)
{
	if (obj == nullptr)
	{
		return;
	}

	((object*)obj)->Finalize();
}

int32_t __hash_code(object* _obj, size_t _size)
{
	if (_obj == nullptr)
	{
		return 0;
	}

	auto bytes = (int8_t*) _obj;

	const int32_t p = 16777619;
	auto hash = 2166136261;

	for (int i = 0; i < _size; i++)
	{
		hash = (hash ^ bytes[i]) * p;
	}

	hash += hash << 13;
	hash ^= hash >> 7;
	hash += hash << 3;
	hash ^= hash >> 17;
	hash += hash << 5;

	return hash;
}

bool __equals_helper(object* _obj1, size_t _size1, object* _obj2, size_t _size2)
{
	if (_size1 != _size2)
	{
		return false;
	}

	if ((_obj1 == nullptr || _obj2 == nullptr) && _obj1 != _obj2)
	{
		return false;
	}

	return std::memcmp((const void*)_obj1, (const void*)_obj2, _size1) == 0;
}

object* __box_pointer(void* p)
{
	return __box((int32_t)p);
}

void* __unbox_pointer(object* obj)
{
	return (void*) __unbox<valuetype_to_class<int32_t>::type>(obj);
}

bool __shutdown_called = false;
void __shutdown()
{
	if (__shutdown_called = true)
	{
		return;
	}

	delete __object_extras_storage_instance;
	delete __strings_storage_instance;

	GC_finalize_all();
}

void __startup()
{
/*
#if (defined(PLATFORM_ANDROID) || defined(__ANDROID__))
	struct GC_stack_base sb;
#endif
*/
    atexit(__shutdown);
    GC_set_all_interior_pointers(1);
    GC_INIT();
/*
#if (defined(PLATFORM_ANDROID) || defined(__ANDROID__))
	GC_allow_register_threads();
	GC_register_my_thread(&sb);
#endif
*/
	__object_extras_storage_instance = new __object_extras_storage();
	__strings_storage_instance = new __strings_storage();
}

__array<string*>* __get_arguments(int32_t argc, char* argv[])
{
    auto arguments_count = argc > 0 ? argc - 1 : 0;
    auto args = __array<string*>::__new_array(arguments_count);
    for( auto i = 0; i < arguments_count; i++ )
    {
        auto argv1 = argv[i + 1];
        args->operator[](i) = __utf8_to_string(argv1);
    }
    
    return args;
}
