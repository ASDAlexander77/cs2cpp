// String literal
string* operator "" _s(const wchar_t* ptr, size_t length)
{
	auto result = string::FastAllocateString(length);
	std::wcsncpy(&result->m_firstChar, ptr, length);
	return result;
}

__object_extras_storage __object_extras_storage_instance;


