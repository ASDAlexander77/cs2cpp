__object_extras_storage* __object_extras_storage_instance = nullptr;
__strings_storage* __strings_storage_instance = nullptr;

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
    atexit(__shutdown);
    GC_set_all_interior_pointers(1);
    GC_INIT();
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
        args->operator[](i) = string::CreateStringFromEncoding((uint8_t*)argv1, std::strlen(argv1), CoreLib::System::Text::Encoding::get_UTF8());
    }
    
    return args;
}
