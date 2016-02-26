// Arrays internals
template <typename T> class __array : public CoreLib::System::Array
{
public:
	int32_t _rank;
	int32_t _length;
	T _data[1];
    typedef CoreLib::System::Array base;
	// TODO: finish checking boundries
	__array(int32_t length) : _rank(1) { _length = length; }
	__array(const __array<T>&) = delete;
	__array(__array<T>&&) = delete;
	inline const T operator [](int32_t index) const { return _data[index]; }
	inline T& operator [](int32_t index) { return _data[index]; }
	inline operator int32_t() const { return (size_t)_length; }

	static __array<T>* Allocate(int32_t length)
	{
		auto mem = __new(sizeof(__array<T>) + (length - 1) * sizeof(T));
		new (mem) __array<T>(length);
		return reinterpret_cast<__array<T>*>(mem);
	}
};

template <typename T, int32_t RANK> class __multi_array : public CoreLib::System::Array
{
public:
	int32_t _rank;
	int32_t _length;
	int32_t _lowerBoundries[RANK];
	int32_t _upperBoundries[RANK];
	T _data[1];
    typedef CoreLib::System::Array base;
	// TODO: finish checking boundries
	template <typename... Ta> __multi_array(Ta... boundries) : _rank(RANK), _lowerBoundries{0}, _upperBoundries{boundries...} {}
	inline const T operator [](std::initializer_list<int32_t> indexes) const { return _data[0]; }
	inline T& operator [](std::initializer_list<int32_t> indexes) { return _data[0]; }
	inline operator int32_t() const { return _length; }
};

template <typename T, int N> class __array_init : public CoreLib::System::Array
{
	int32_t _rank;
	int32_t _length;
	T _data[N];
public:
    template <typename... Ta> __array_init(Ta... items) : _rank(1), _length(sizeof...(items)), _data{items...} {} 
};

// Boxing internals
template <typename T, typename = std::enable_if<std::is_base_of<object, T>::value> > inline T* __box (T* t)
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

// cast internals
template <typename D, typename S> inline D as(S v)
{
	return (D) nullptr;
}

// cast internals
template <typename D, typename S> 
inline bool is(typename std::enable_if<std::is_base_of<object, S>::value, S>::type* v)
{
	return as<D>(v) != nullptr;
}

template <typename D, typename S> 
inline bool is(S v)
{
	return false;
}

// Constrained internals (for templates)
template <typename C, typename T> 
inline C constrained (typename std::enable_if<std::is_base_of<object, T>::value, T>::type* t)
{
	return nullptr;
}

template <typename C, typename T> 
inline C constrained (T t)
{
	return nullptr;
}

// Typeof internals
template <typename T> inline CoreLib::System::Type* _typeof()
{
	// TODO: finish it
	T* t;
	return nullptr;
}

// String literal
string* operator "" _s(const wchar_t* ptr, size_t length);

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

// Default
template <typename T> 
inline typename std::enable_if<std::is_pointer<T>::value, T>::type __default()
{
	return nullptr;
}

template <typename T> 
inline typename std::enable_if<!std::is_pointer<T>::value && !std::is_void<T>::value, T>::type __default()
{
	return T();
}

template <typename T> 
inline typename std::enable_if<std::is_void<T>::value, T>::type __default()
{
	return;
}

// Activator
template <typename T> 
typename std::enable_if<is_class_type<T>::value, T>::type __create_instance()
{
	typedef typename std::remove_pointer<T>::type _T;
	return new _T();
}

template <typename T> 
T __create_instance()
{
	return T();
}