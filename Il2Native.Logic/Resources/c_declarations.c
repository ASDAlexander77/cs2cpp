// Arrays internals
template <typename T> class __array : public <<%assemblyName%>>::System::Array
{
	int32_t _rank;
	int32_t _length;
	T _data[0];
public:
	// TODO: finish checking boundries
	__array(size_t length) : _rank(1) { _length = length; }
	inline const T operator [](size_t index) const { return _data[index]; }
	inline T& operator [](size_t index) { return _data[index]; }
	inline operator size_t() const { return (size_t)_length; }
};

template <typename T, size_t RANK> class __multi_array : public <<%assemblyName%>>::System::Array
{
	int32_t _rank;
	int32_t _length;
	int32_t _lowerBoundries[RANK];
	int32_t _upperBoundries[RANK];
	T _data[0];
public:
	// TODO: finish checking boundries
	template <typename... Ta> __multi_array(Ta... boundries) : _rank(RANK), _lowerBoundries{0}, _upperBoundries{boundries...} {}
	inline const T operator [](std::initializer_list<T> indexes) const { return _data[0]; }
	inline T& operator [](std::initializer_list<T> indexes) { return _data[0]; }
	inline operator size_t() const { return (size_t)_length; }
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
template <typename D, typename C, typename S> inline D __unbox(S* c)
{
	// TODO: finish it
	D d;
	return d;
}

// Unboxing internals
template <typename T> inline <<%assemblyName%>>::System::Type* typeof()
{
	// TODO: finish it
	return nullptr;
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
