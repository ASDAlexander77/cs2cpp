// Arrays internals
template <typename T> class __array : public <<%assemblyName%>>::System::Array
{
	int32_t _length;
	T _data[0];
public:
	// TODO: finish checking boundries
	__array(size_t length) { _length = length; }
	inline T operator [](size_t index) { return _data[index]; }
	inline operator size_t() { return (size_t)_length; }
};

template <typename T, int N> class __array_init : public <<%assemblyName%>>::System::Array
{
	int32_t _length;
	T _data[N];
public:
    template <typename... Ta> __array_init(Ta... items) : _length(sizeof...(items)), _data{items...} {} 
};

// Boxing internals
template <typename D, typename S> inline D* __box(S v)
{
	return (D*) new S(v);
}

// Unboxing internals
template <typename D, typename S> inline D __unbox(S* c)
{
	// TODO: finish it
	D d;
	return d;
}

// String literal
inline string* operator "" _s(const wchar_t* str, size_t len)
{
	return new string((wchar_t*)str, 0, (int32_t)len);
}

// Finally block
class Finally
{
public:
	std::function<void()> _dtor;
	Finally(std::function<void()> dtor) : _dtor(dtor) {};
	~Finally() { _dtor(); }
};
