// String literal
string* operator "" _s(const wchar_t* ptr, size_t length)
{
	auto result = string::FastAllocateString(length);
	std::wcsncpy(&result->m_firstChar, ptr, length);
	return result;
}

std::unordered_map<void*, std::timed_mutex> __locks;
std::unordered_map<void*, std::condition_variable_any> __conditions;