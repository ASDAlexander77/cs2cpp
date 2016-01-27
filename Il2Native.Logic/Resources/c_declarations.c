template <typename T> class __array : public <<%assemblyName%>>::System::Array
{
private:
	int32_t _length;
	T _data[0];
public:
	// TODO: finish checking boundries
	__array(size_t length) { _length = length; }
	inline T operator [](size_t index) { return _data[index]; }
};

inline string* operator "" _s(const wchar_t* str, size_t len)
{
	return new string((wchar_t*)str, 0, (int32_t)len);
}
