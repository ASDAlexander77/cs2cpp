template <typename T> class __array : public <<%assemblyName%>>::System::Array
{
	int32_t _length;
	T _data[0];
public:
	// TODO: finish checking boundries
	__array(size_t length) { _length = length; }
	inline T operator [](size_t index) { return _data[index]; }
};

template <typename T, int N> class __array_init : public <<%assemblyName%>>::System::Array
{
	int32_t _length;
	T _data[N];
public:
    template <typename... Ta> __array_init(int length, Ta... items) : _length(length), _data{items...} {} 
};

template <typename D, typename S> inline D* __box(S v)
{
	return (D*) new S(v);
}

template <typename D, typename S> inline D __unbox(S* c)
{
	// TODO: finish it
	D d;
	return d;
}

inline string* operator "" _s(const wchar_t* str, size_t len)
{
	return new string((wchar_t*)str, 0, (int32_t)len);
}
