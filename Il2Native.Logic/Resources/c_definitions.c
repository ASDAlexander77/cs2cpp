string* operator "" _s(const wchar_t* str, size_t len)
{
	return new string((wchar_t*)str, 0, (int32_t)len);
}
