// Arrays internals
template <typename T> class __array : public <<%assemblyName%>>::System::Array
{
	int32_t _rank;
	int32_t _length;
	T _data[0];
public:
    typedef CoreLib::System::Array base;
	// TODO: finish checking boundries
	__array(size_t length) : _rank(1) { _length = length; }
	inline const T operator [](int32_t index) const { return _data[index]; }
	inline T& operator [](int32_t index) { return _data[index]; }
	inline operator int32_t() const { return (size_t)_length; }
};

template <typename T, int32_t RANK> class __multi_array : public <<%assemblyName%>>::System::Array
{
	int32_t _rank;
	int32_t _length;
	int32_t _lowerBoundries[RANK];
	int32_t _upperBoundries[RANK];
	T _data[0];
public:
    typedef CoreLib::System::Array base;
	// TODO: finish checking boundries
	template <typename... Ta> __multi_array(Ta... boundries) : _rank(RANK), _lowerBoundries{0}, _upperBoundries{boundries...} {}
	inline const T operator [](std::initializer_list<int32_t> indexes) const { return _data[0]; }
	inline T& operator [](std::initializer_list<int32_t> indexes) { return _data[0]; }
	inline operator int32_t() const { return _length; }
};

template <typename T, int N> class __array_init : public <<%assemblyName%>>::System::Array
{
	int32_t _length;
	T _data[N];
public:
    template <typename... Ta> __array_init(Ta... items) : _length(sizeof...(items)), _data{items...} {} 
};

// Boxing internals
template <typename T, typename = std::enable_if<std::is_base_of<<<%assemblyName%>>::System::Object, T>::value> > inline T* __box (T* t)
{
	return t;
}

template <typename T> inline T* __box (T t)
{
	return new T(t);
}

// Unboxing internals
template <typename D, typename S> inline D __unbox(S* c)
{
	// TODO: finish it
	D d;
	return d;
}

// interface cast internals
template <typename D, typename S> inline D interface_cast(S v)
{
	return (D) nullptr;
}

// interface cast internals
template <typename D, typename S> inline D as(S v)
{
	return (D) nullptr;
}

// interface cast internals
template <typename D, typename S> inline bool is(S v)
{
	return as<D>(v) != nullptr;
}

// Constrained internals (for templates)
template <typename C, typename T, typename = std::enable_if<std::is_base_of<<<%assemblyName%>>::System::Object, T>::value> > inline C constrained (T* t)
{
	return nullptr;
}

template <typename C, typename T> inline C constrained (T t)
{
	return nullptr;
}

// Typeof internals
template <typename T> inline <<%assemblyName%>>::System::Type* _typeof()
{
	// TODO: finish it
	T* t;
	return nullptr;
}

// String literal
inline string* operator "" _s(const wchar_t* str, size_t len)
{
	return string::CtorCharPtrStartLength((wchar_t*)str, 0, (int32_t)len);
}

// Finally block
class Finally
{
public:
	std::function<void()> _dtor;
	Finally(std::function<void()> dtor) : _dtor(dtor) {};
	~Finally() { _dtor(); }
};

template< typename T >
class __lazy
{
	typedef std::function<T()> initializer;

	T t;
	initializer _init;
	bool _created;
public:
	__lazy(initializer init)
	{
		_created = false;
		_init = init;
	}

	inline T& operator=(const T& value)
	{
		_created = true;
		t = value;
	}

	inline operator T()
	{
		if (!_created)
		{			
			t = _init();
			_created = true;
		}

		return t;
	}

	inline T operator ->()
	{
		if (!_created)
		{			
			t = _init();
		}

		return t;
	}
};

// Activator
template <typename T> 
typename std::enable_if<std::is_base_of<<<%assemblyName%>>::System::Object, T>::value && std::is_pointer<T>::value>::type __create_instance()
{
	typedef typename std::remove_pointer<T>::type _T;
	return new _T();
}

template <typename T> 
T __create_instance()
{
	return T();
}