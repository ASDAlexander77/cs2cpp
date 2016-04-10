__object_extras_storage __object_extras_storage_instance;

// String literal
string* operator "" _s(const wchar_t* ptr, size_t length)
{
	auto result = string::FastAllocateString(length);
	std::wcsncpy(&result->m_firstChar, ptr, length);
	return result;
}

int32_t __hash_code(object* _obj, int32_t _size)
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

bool __equals_helper(object* _obj1, int32_t _size1, object* _obj2, int32_t _size2)
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

