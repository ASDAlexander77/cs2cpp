template <typename T> struct is_primitive_type : std::integral_constant<bool, std::is_enum<T>::value || std::is_integral<T>::value || std::is_floating_point<T>::value>
{
};

template <typename T> struct is_struct_type : std::integral_constant<bool, std::is_object<T>::value && std::is_base_of<std::decay<object>, T>::value>
{
};

template <typename T> struct is_value_type : std::integral_constant<bool, is_struct_type<T>::value || is_primitive_type<T>::value>
{
};

template <typename T> struct is_class_type : std::integral_constant<bool, std::is_pointer<T>::value && std::is_base_of<std::decay<object>, T>::value>
{
};

inline void* __new_set0(size_t _size)
{
	auto mem = ::operator new(_size);
	std::memset(mem, 0, _size);
	return mem;
}

template <typename T, typename... Tp> inline T* __new(Tp... params) 
{
	auto t = new T();		
	t->_ctor(params...);
	return t;
} 

template <typename T, typename... Tp> inline T __init(Tp... params) 
{
	auto t = T();		
	t->_ctor(params...);
	return t;
} 

template <typename T> class __array;

template <typename T, typename TUnderlying> struct __enum
{
public: 
	TUnderlying _value;
	__enum() = default;
	inline __enum(TUnderlying value) : _value(value) {}
	inline operator TUnderlying() { return _value; }
	inline __enum& operator++()
	{
		// actual increment takes place here
		_value++;
		return *this;
	}
	inline __enum operator++(int)
	{
		__enum tmp(*this);
		operator++();
		return tmp;
	}
};

template <typename T> struct __unbound_generic_type
{
};

template< typename T, typename C>
class __static 
{
	T t;
public:

	inline __static<T, C>& operator=(const T& value)
	{
		if (!C::_cctor_called)
		{
			C::_cctor();
		}

		t = value;

		return *this;
	}

	inline operator const T&()
	{
		if (!C::_cctor_called)
		{
			C::_cctor();
		}

		return t;
	}

	inline T& operator ->()
	{
		if (!C::_cctor_called)
		{
			C::_cctor();
		}

		return t;
	}

	template <typename D, class = typename std::enable_if<std::is_enum<T>::value && std::is_integral<D>::value> > inline explicit operator D()
	{
		if (!C::_cctor_called)
		{
			C::_cctor();
		}

		return (D)t;
	}
};
