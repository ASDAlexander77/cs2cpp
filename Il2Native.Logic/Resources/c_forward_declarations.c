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

inline void* __new (size_t _size)
{
	auto mem = ::operator new(_size);
	std::memset(mem, 0, _size);
	return mem;
}

template< typename T, typename C>
class __static 
{
	T t;
public:

	inline T& operator=(T value)
	{
		if (!C::_cctor_called)
		{
			C::_cctor();
		}

		t = value;

		return *this;
	}

	inline operator T&()
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

	inline T* operator &()
	{
		if (!C::_cctor_called)
		{
			C::_cctor();
		}

		return &t;
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

template <typename T> struct is_primitive_type : std::integral_constant<bool, std::is_enum<T>::value || std::is_integral<T>::value || std::is_floating_point<T>::value>
{
};

template <typename T> struct is_struct_type : std::integral_constant<bool, std::is_object<T>::value && std::is_base_of<object, T>::value>
{
};

template <typename T> struct is_value_type : std::integral_constant<bool, std::is_struct_type<T>::value || std::is_primitive_type<T>::value>
{
};

template <typename T> struct is_class_type : std::integral_constant<bool, std::is_pointer<T>::value && std::is_base_of<object, T>::value>
{
};
