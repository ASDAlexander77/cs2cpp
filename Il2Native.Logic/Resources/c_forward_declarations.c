// map valuetype to class
template<typename T> 
struct valuetype_to_class { typedef T type; };

// map class to valuetype
template<typename T> 
struct class_to_valuetype { typedef T type; };

template <typename T> struct convert_primitive_type_to_class
{
	typedef
		typename std::conditional< std::is_same< T, void >::value, CoreLib::System::Void, 
		typename std::conditional< std::is_same< T, int8_t >::value, CoreLib::System::SByte, 
		typename std::conditional< std::is_same< T, uint8_t >::value, CoreLib::System::Byte, 
		typename std::conditional< std::is_same< T, int16_t >::value, CoreLib::System::Int16, 
		typename std::conditional< std::is_same< T, uint16_t >::value, CoreLib::System::UInt16, 
		typename std::conditional< std::is_same< T, wchar_t >::value, CoreLib::System::Char, 
		typename std::conditional< std::is_same< T, int32_t >::value, CoreLib::System::Int32, 
		typename std::conditional< std::is_same< T, uint32_t >::value, CoreLib::System::UInt32, 
		typename std::conditional< std::is_same< T, int64_t >::value, CoreLib::System::Int64, 
		typename std::conditional< std::is_same< T, uint64_t >::value, CoreLib::System::UInt64, 
		typename std::conditional< std::is_same< T, float >::value, CoreLib::System::Single, 
		typename std::conditional< std::is_same< T, double >::value, CoreLib::System::Double, 
		T 
		>::type>::type>::type>::type>::type>::type>::type>::type>::type>::type>::type>::type type;
};

template <typename T> struct convert_class_to_primitive_type
{
	typedef
		typename std::conditional< std::is_same< T, CoreLib::System::Void >::value, void, 
		typename std::conditional< std::is_same< T, CoreLib::System::SByte >::value, int8_t, 
		typename std::conditional< std::is_same< T, CoreLib::System::Byte >::value, uint8_t, 
		typename std::conditional< std::is_same< T, CoreLib::System::Int16 >::value, int16_t, 
		typename std::conditional< std::is_same< T, CoreLib::System::UInt16 >::value, uint16_t, 
		typename std::conditional< std::is_same< T, CoreLib::System::Char >::value, wchar_t, 
		typename std::conditional< std::is_same< T, CoreLib::System::Int32 >::value, int32_t, 
		typename std::conditional< std::is_same< T, CoreLib::System::UInt32 >::value, uint32_t, 
		typename std::conditional< std::is_same< T, CoreLib::System::Int64 >::value, int64_t, 
		typename std::conditional< std::is_same< T, CoreLib::System::UInt64 >::value, uint64_t, 
		typename std::conditional< std::is_same< T, CoreLib::System::Single >::value, float, 
		typename std::conditional< std::is_same< T, CoreLib::System::Double >::value, double, 
		T 
		>::type>::type>::type>::type>::type>::type>::type>::type>::type>::type>::type>::type type;
};

template <typename T> struct is_primitive_type : std::integral_constant<bool, std::is_enum<T>::value || std::is_integral<T>::value || std::is_floating_point<T>::value>
{
};

template <typename T> struct is_struct_type : std::integral_constant<bool, std::is_object<T>::value && std::is_base_of<object, T>::value>
{
};

template <typename T> struct is_value_type : std::integral_constant<bool, is_struct_type<T>::value || is_primitive_type<T>::value>
{
};

template <typename T> struct is_class_type : std::integral_constant<bool, std::is_pointer<T>::value && std::is_base_of<object, typename std::remove_pointer<T>::type>::value>
{
};

template <typename T> struct is_interface_type : std::integral_constant<bool, std::is_pointer<T>::value && !is_class_type<T>::value && !is_value_type<typename std::remove_pointer<T>::type>::value && !std::is_pointer<typename std::remove_pointer<T>::type>::value>
{
};

template <typename T> struct is_object : std::integral_constant<bool, std::is_pointer<T>::value && std::is_same<object, typename std::remove_pointer<T>::type>::value>
{
};

extern void GC_CALLBACK __finalizer(void * obj, void * client_data);

inline void* __new_set0(size_t _size, bool _is_atomic = false)
{
	auto mem = _size > 102400 
		? _is_atomic 
		? GC_MALLOC_ATOMIC_IGNORE_OFF_PAGE(_size) 
		: GC_MALLOC_IGNORE_OFF_PAGE(_size) 
		: _is_atomic 
		? GC_MALLOC_ATOMIC(_size) 
		: GC_MALLOC(_size);
	if (_is_atomic)
	{
		std::memset(mem, 0, _size);
	}

	return mem;
}

inline void* __new_set0_with_finalizer(size_t _size, bool _is_atomic = false)
{
	auto mem = __new_set0(_size, _is_atomic);
	GC_REGISTER_FINALIZER((void *)mem, __finalizer, (void *)nullptr, (GC_finalization_proc *)nullptr, (void **)nullptr);
	return mem;
}


template <typename T, typename... Tp> inline T* __new(Tp... params) 
{
	auto t = new (sizeof(T), is_primitive_type<T>::value) T();		
	t->_ctor(params...);
	return t;
} 

template <typename T, typename... Tp> inline T __init(Tp... params) 
{
	auto t = T();		
	t._ctor(params...);
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
	inline __enum& operator--()
	{
		// actual decrement takes place here
		_value--;
		return *this;
	}
	inline __enum operator--(int)
	{
		__enum tmp(*this);
		operator--();
		return tmp;
	}
};

template <typename T> struct __unbound_generic_type
{
};

// Default
template <typename T> 
inline typename std::enable_if<std::is_pointer<T>::value, T>::type __default()
{
	return nullptr;
}

template <typename T> 
inline typename std::enable_if<is_struct_type<T>::value, T>::type __default()
{
	return __init<T>();
}

template <typename T> 
inline typename std::enable_if<is_primitive_type<T>::value, T>::type __default()
{
	return T();
}

template <typename T> 
inline typename std::enable_if<std::is_void<T>::value, T>::type __default()
{
	return;
}

template< typename T >
struct __volatile_t 
{
	T t;
public:

	inline __volatile_t() { this->operator=(__default<T>()); }

	inline __volatile_t<T>& operator=(T value)
	{
		_interlocked_compare_exchange(&t, value, __default<T>());
		return *this;
	}

	inline operator T()
	{
		return _interlocked_exchange(&t, __default<T>());
	}

	inline T operator ->()
	{
		return _interlocked_exchange(&t, __default<T>());
	}

	template <typename D = __volatile_t<T>, class = typename std::enable_if<std::is_integral<T>::value> > D& operator++()
	{
		_interlocked_add(&t, 1);
		return *this;
	}

	template <typename D = __volatile_t<T>, class = typename std::enable_if<std::is_integral<T>::value> > D operator++(int)
	{
		D tmp(*this);
		operator++();
		return tmp;
	}

	template <typename D = __volatile_t<T>, class = typename std::enable_if<std::is_integral<T>::value> > D& operator--()
	{
		_interlocked_sub(&t, 1);
		return *this;
	}

	template <typename D = __volatile_t<T>, class = typename std::enable_if<std::is_integral<T>::value> > D operator--(int)
	{
		D tmp(*this);
		operator--();
		return tmp;
	}

	template <typename D, class = typename std::enable_if<std::is_enum<T>::value && std::is_integral<D>::value> > inline explicit operator D()
	{
		return (D) operator T();
	}
};

template< typename T, typename C >
struct __static 
{
	T t;
public:

	inline void ensure_cctor_called()
	{
		if (!C::_cctor_called)
		{
			C::_cctor_lock.lock();
			if (!C::_cctor_called && !C::_cctor_being_called)
			{
				C::_cctor();
			}

			C::_cctor_lock.unlock();
		}
	}

	inline __static<T, C>& operator=(const T& value)
	{
		ensure_cctor_called();
		t = value;
		return *this;
	}

	inline operator const T&()
	{
		ensure_cctor_called();
		return t;
	}

	inline T operator ->()
	{
		ensure_cctor_called();
		return t;
	}

	inline T* operator &()
	{
		ensure_cctor_called();
		return &t;
	}

	template <typename D, class = typename std::enable_if<std::is_enum<T>::value && std::is_integral<D>::value> > inline explicit operator D()
	{
		ensure_cctor_called();
		return (D)t;
	}
};

template< typename T, typename C >
struct __static_volatile
{
	T t;
public:

	inline __static_volatile() 
	{ 
		_interlocked_compare_exchange(&t,  __default<T>(), __default<T>()); 
	}

	inline void ensure_cctor_called()
	{
		if (!C::_cctor_called)
		{
			C::_cctor_lock.lock();
			if (!C::_cctor_called && !C::_cctor_being_called)
			{
				C::_cctor();
			}

			C::_cctor_lock.unlock();
		}
	}

	inline __static_volatile<T, C>& operator=(T value)
	{
		ensure_cctor_called();
		_interlocked_compare_exchange(&t, value, __default<T>());
		return *this;
	}

	inline operator T()
	{
		ensure_cctor_called();
		return _interlocked_exchange(&t, __default<T>());
	}

	inline T operator ->()
	{
		ensure_cctor_called();
		return _interlocked_exchange(&t, __default<T>());
	}

	template <typename D = __static_volatile<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > D& operator++()
	{
		_interlocked_add(&t, 1);
		return *this;
	}

	template <typename D = __static_volatile<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > D operator++(int)
	{
		D tmp(*this);
		operator++();
		return tmp;
	}

	template <typename D = __static_volatile<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > D& operator--()
	{
		_interlocked_sub(&t, 1);
		return *this;
	}

	template <typename D = __static_volatile<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > D operator--(int)
	{
		D tmp(*this);
		operator--();
		return tmp;
	}

	template <typename D, class = typename std::enable_if<std::is_enum<T>::value && std::is_integral<D>::value> > inline explicit operator D()
	{
		ensure_cctor_called();
		return (D) operator T();
	}
};

// object cast (interface etc)
template <typename T> 
inline typename std::enable_if<!is_interface_type<T>::value, object*>::type object_cast (T t)
{
	return static_cast<object*>(t);
}

template <typename T> 
inline typename std::enable_if<is_interface_type<T>::value, object*>::type object_cast (T t)
{
	if (t == nullptr)
	{
		return nullptr;
	}

	return t->operator object*();
}

// cast internals
template <typename D, typename S> inline typename std::enable_if<!is_value_type<D>::value && !is_interface_type<S>::value, D>::type cast(S s)
{
	if (s == nullptr)
	{
		return nullptr;
	}

	auto d = dynamic_cast<D>(s);
	if (d == nullptr)
	{
		throw __new<CoreLib::System::InvalidCastException>();
	}

	return d;
}

// cast internals
template <typename D, typename S> inline typename std::enable_if<is_class_type<D>::value && !is_object<D>::value && is_interface_type<S>::value, D>::type cast(S s)
{
	if (s == nullptr)
	{
		return nullptr;
	}

	auto d = dynamic_cast<D>(object_cast(s));
	if (d == nullptr)
	{
		throw __new<CoreLib::System::InvalidCastException>();
	}

	return d;
}

template <typename D, typename S> inline typename std::enable_if<is_object<D>::value && is_interface_type<S>::value, D>::type cast(S s)
{
	if (s == nullptr)
	{
		return nullptr;
	}

	return object_cast(s);
}

template <typename T, typename _CLASS = typename valuetype_to_class<T>::type> 
inline typename std::enable_if<is_value_type<T>::value, _CLASS>::type cast(object* o)
{
	return *cast<_CLASS*>(o);
}

template <typename D, typename S> inline D map_pointer_cast(S s)
{
	union { D d; S s; } u;
	u.s = s;
	return u.d;
}

int32_t __hash_code(object* _obj, size_t _size);

bool __equals_helper(object* _obj1, size_t _size1, object* _obj2, size_t _size2);