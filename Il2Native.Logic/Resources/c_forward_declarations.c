enum class GCNormal
{
	Default
};

enum class GCAtomic
{
	Default
};

// map valuetype to class
template<typename T> 
struct valuetype_to_class { typedef T type; };

// map class to valuetype
template<typename T> 
struct class_to_valuetype { typedef T type; };

template<typename T> 
struct gc_traits { constexpr static const GCNormal value = GCNormal::Default; };

template <typename T> struct convert_primitive_type_to_class
{
	typedef
		typename std::conditional< std::is_same< T, void >::value, CoreLib::System::Void, 
		typename std::conditional< std::is_same< T, int8_t >::value, CoreLib::System::SByte, 
		typename std::conditional< std::is_same< T, uint8_t >::value, CoreLib::System::Byte, 
		typename std::conditional< std::is_same< T, int16_t >::value, CoreLib::System::Int16, 
		typename std::conditional< std::is_same< T, uint16_t >::value, CoreLib::System::UInt16, 
		typename std::conditional< std::is_same< T, char16_t >::value, CoreLib::System::Char, 
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
		typename std::conditional< std::is_same< T, CoreLib::System::Char >::value, char16_t, 
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

template <typename T> struct is_pointer_type : std::integral_constant<bool, std::is_pointer<T>::value && !is_interface_type<T>::value && !std::is_base_of<object, typename std::remove_pointer<T>::type>::value>
{
};

extern void GC_CALLBACK __finalizer(void * obj, void * client_data);

inline void* __new_set0(size_t _size)
{
	auto mem = _size > 102400 
		? GC_MALLOC_IGNORE_OFF_PAGE(_size) 
		: GC_MALLOC(_size);
	return mem;
}

inline void* __new_set0(size_t _size, GCNormal)
{
	return __new_set0(_size);
}

inline void* __new_set0(size_t _size, GCAtomic)
{
	auto mem = _size > 102400 
		? GC_MALLOC_ATOMIC_IGNORE_OFF_PAGE(_size) 
		: GC_MALLOC_ATOMIC(_size);
	std::memset(mem, 0, _size);
	return mem;
}

inline void* __new_set0(size_t _size, GC_descr _type_descr)
{
	auto mem = _size > 102400 
		? GC_malloc_explicitly_typed_ignore_off_page(_size, _type_descr) 
		: GC_MALLOC_EXPLICITLY_TYPED(_size, _type_descr);
	return mem;
}

inline void* __new_set0_with_finalizer(size_t _size)
{
	auto mem = __new_set0(_size);
	GC_REGISTER_FINALIZER((void *)mem, __finalizer, (void *)nullptr, (GC_finalization_proc *)nullptr, (void **)nullptr);
	return mem;
}

inline void* __new_set0_with_finalizer(size_t _size, GCNormal _is_normal)
{
	auto mem = __new_set0(_size, _is_normal);
	GC_REGISTER_FINALIZER((void *)mem, __finalizer, (void *)nullptr, (GC_finalization_proc *)nullptr, (void **)nullptr);
	return mem;
}

inline void* __new_set0_with_finalizer(size_t _size, GCAtomic _is_atomic)
{
	auto mem = __new_set0(_size, _is_atomic);
	GC_REGISTER_FINALIZER((void *)mem, __finalizer, (void *)nullptr, (GC_finalization_proc *)nullptr, (void **)nullptr);
	return mem;
}

inline void* __new_set0_with_finalizer(size_t _size, GC_descr _type_descr)
{
	auto mem = __new_set0(_size, _type_descr);
	GC_REGISTER_FINALIZER((void *)mem, __finalizer, (void *)nullptr, (GC_finalization_proc *)nullptr, (void **)nullptr);
	return mem;
}

// debug versions
inline void* __new_set0(size_t _size, const char* _file, int _line)
{
#ifdef GC_DEBUG
	auto mem = _size > 102400 
		? GC_debug_malloc_ignore_off_page(_size, GC_ALLOC_PARAMS) 
		: GC_debug_malloc(_size, GC_ALLOC_PARAMS);
	return mem;
#else
	return __new_set0(_size);
#endif
}

inline void* __new_set0(size_t _size, GCNormal, const char* _file, int _line)
{
#ifdef GC_DEBUG
	return __new_set0(_size, _file, _line);
#else
	return __new_set0(_size);
#endif
}

inline void* __new_set0(size_t _size, GCAtomic, const char* _file, int _line)
{
#ifdef GC_DEBUG
	auto mem = _size > 102400 
		? GC_debug_malloc_atomic_ignore_off_page(_size, GC_ALLOC_PARAMS) 
		: GC_debug_malloc_atomic(_size, GC_ALLOC_PARAMS);
	return mem;
#else
	return __new_set0(_size, GCAtomic::Default);
#endif
}

inline void* __new_set0(size_t _size, GC_descr _type_descr, const char* _file, int _line)
{
#ifdef GC_DEBUG
	auto mem = _size > 102400 
		? GC_debug_malloc_ignore_off_page(_size, GC_ALLOC_PARAMS) 
		: GC_debug_malloc(_size, GC_ALLOC_PARAMS);
	return mem;
#else
	return __new_set0(_size, _type_descr);
#endif
}

inline void* __new_set0_with_finalizer(size_t _size, const char* _file, int _line)
{
	auto mem = __new_set0(_size, _file, _line);
	GC_REGISTER_FINALIZER((void *)mem, __finalizer, (void *)nullptr, (GC_finalization_proc *)nullptr, (void **)nullptr);
	return mem;
}

inline void* __new_set0_with_finalizer(size_t _size, GCNormal, const char* _file, int _line)
{
	auto mem = __new_set0(_size, _file, _line);
	GC_REGISTER_FINALIZER((void *)mem, __finalizer, (void *)nullptr, (GC_finalization_proc *)nullptr, (void **)nullptr);
	return mem;
}

inline void* __new_set0_with_finalizer(size_t _size, GCAtomic, const char* _file, int _line)
{
	auto mem = __new_set0(_size, GCAtomic::Default, _file, _line);
	GC_REGISTER_FINALIZER((void *)mem, __finalizer, (void *)nullptr, (GC_finalization_proc *)nullptr, (void **)nullptr);
	return mem;
}

inline void* __new_set0_with_finalizer(size_t _size, GC_descr _type_descr, const char* _file, int _line)
{
	auto mem = __new_set0(_size, _type_descr, _file, _line);
	GC_REGISTER_FINALIZER((void *)mem, __finalizer, (void *)nullptr, (GC_finalization_proc *)nullptr, (void **)nullptr);
	return mem;
}

/*
void* operator new (size_t _size);
void* operator delete (void* obj);
void* operator new[] (size_t _size);
void* operator delete[] (void* obj);
*/

void* operator new (size_t _size, GCNormal);
void* operator new (size_t _size, GCNormal, const char* _file, int _line);
void* operator new (size_t _size, int32_t _customSize, GCNormal);
void* operator new (size_t _size, int32_t _customSize, GCNormal, const char* _file, int _line);
void* operator new (size_t _size, GCAtomic);
void* operator new (size_t _size, GCAtomic, const char* _file, int _line);
void* operator new (size_t _size, int32_t _customSize, GCAtomic);
void* operator new (size_t _size, int32_t _customSize, GCAtomic, const char* _file, int _line);
void* operator new (size_t _size, GC_descr _type_descr);
void* operator new (size_t _size, GC_descr _type_descr, const char* _file, int _line);
void* operator new (size_t _size, int32_t _customSize, GC_descr _type_descr);
void* operator new (size_t _size, int32_t _customSize, GC_descr _type_descr, const char* _file, int _line);

template <typename T, typename... Tp> inline T* __new(Tp... params) 
{
	auto t = new T();		
	t->_ctor(params...);
	return t;
} 

template <typename T, typename... Tp> inline T* __new_debug(const char* _file, int _line, Tp... params) 
{
	auto t = new T();		
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
	volatile T t;
public:

	inline T __read()
	{
		return _interlocked_compare_exchange(&t, __default<T>(), __default<T>());
	}

	inline void __write(T value)
	{
		_interlocked_exchange(&t, value);
	}

	inline __volatile_t() { this->operator=(__default<T>()); }

	inline __volatile_t<T>& operator=(T value)
	{
		__write(value);
		return *this;
	}

	inline operator T()
	{
		return __read();
	}

	inline T operator ->()
	{
		return __read();
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

	template <typename D = __volatile_t<T>, class = typename std::enable_if<std::is_integral<T>::value> > D& operator+=(T other)
	{
		_interlocked_add(&t, other);
		return *this;
	}

	template <typename D = __volatile_t<T>, class = typename std::enable_if<std::is_integral<T>::value> > D& operator-=(T other)
	{
		_interlocked_sub(&t, other);
		return *this;
	}

	template <typename D = __volatile_t<T>, class = typename std::enable_if<std::is_integral<T>::value> > D& operator|=(T other)
	{
		_interlocked_or(&t, other);
		return *this;
	}

	template <typename D = __volatile_t<T>, class = typename std::enable_if<std::is_integral<T>::value> > D& operator&=(T other)
	{
		_interlocked_and(&t, other);
		return *this;
	}

	template <typename D = __volatile_t<T>, class = typename std::enable_if<std::is_integral<T>::value> > D& operator^=(T other)
	{
		_interlocked_xor(&t, other);
		return *this;
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

	template <typename D = __static<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > inline D& operator++()
	{
		ensure_cctor_called();
		t++;
		return *this;
	}

	template <typename D = __static<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > inline D operator++(int)
	{
		D tmp(*this);
		operator++();
		return tmp;
	}

	template <typename D = __static<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > inline D& operator--()
	{
		ensure_cctor_called();
		t--;
		return *this;
	}

	template <typename D = __static<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > inline D operator--(int)
	{
		D tmp(*this);
		operator--();
		return tmp;
	}

	inline __static<T, C>& operator+=(T value)
	{
		ensure_cctor_called();
		t += value;
		return *this;
	}

	inline __static<T, C>& operator-=(T value)
	{
		ensure_cctor_called();
		t -= value;
		return *this;
	}

	inline __static<T, C>& operator*=(T value)
	{
		ensure_cctor_called();
		t *= value;
		return *this;
	}

	inline __static<T, C>& operator/=(T value)
	{
		ensure_cctor_called();
		t /= value;
		return *this;
	}

	inline __static<T, C>& operator%=(T value)
	{
		ensure_cctor_called();
		t %= value;
		return *this;
	}

	inline __static<T, C>& operator|=(T value)
	{
		ensure_cctor_called();
		t |= value;
		return *this;
	}

	inline __static<T, C>& operator&=(T value)
	{
		ensure_cctor_called();
		t &= value;
		return *this;
	}

	inline __static<T, C>& operator^=(T value)
	{
		ensure_cctor_called();
		t ^= value;
		return *this;
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
	volatile T t;
public:

	inline T __read()
	{
		return _interlocked_compare_exchange(&t, __default<T>(), __default<T>());
	}

	inline void __write(T value)
	{
		_interlocked_exchange(&t, value);
	}

	inline __static_volatile() 
	{ 
		__write(__default<T>()); 
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
		__write(value);
		return *this;
	}

	inline operator T()
	{
		ensure_cctor_called();
		return __read();
	}

	inline T operator ->()
	{
		ensure_cctor_called();
		return __read();
	}

	template <typename D = __static_volatile<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > inline D& operator++()
	{
		ensure_cctor_called();
		_interlocked_add(&t, 1);
		return *this;
	}

	template <typename D = __static_volatile<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > inline D operator++(int)
	{
		D tmp(*this);
		operator++();
		return tmp;
	}

	template <typename D = __static_volatile<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > inline D& operator--()
	{
		ensure_cctor_called();
		_interlocked_sub(&t, 1);
		return *this;
	}

	template <typename D = __static_volatile<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > inline D operator--(int)
	{
		D tmp(*this);
		operator--();
		return tmp;
	}

	template <typename D = __static_volatile<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > inline D& operator+=(T other)
	{
		ensure_cctor_called();
		_interlocked_add(&t, other);
		return *this;
	}

	template <typename D = __static_volatile<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > inline D& operator-=(T other)
	{
		ensure_cctor_called();
		_interlocked_sub(&t, other);
		return *this;
	}

	template <typename D = __static_volatile<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > inline D& operator|=(T other)
	{
		ensure_cctor_called();
		_interlocked_or(&t, other);
		return *this;
	}

	template <typename D = __static_volatile<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > inline D& operator&=(T other)
	{
		ensure_cctor_called();
		_interlocked_and(&t, other);
		return *this;
	}

	template <typename D = __static_volatile<T, C>, class = typename std::enable_if<std::is_integral<T>::value> > inline D& operator^=(T other)
	{
		ensure_cctor_called();
		_interlocked_xor(&t, other);
		return *this;
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

// interface cast
template <typename C, typename T> 
inline typename std::enable_if<!is_value_type<T>::value, C>::type interface_cast (T t)
{
	if (t == nullptr)
	{
		return nullptr;
	}

	return t->operator C();
}

template <typename C, typename T> 
inline typename std::enable_if<is_value_type<T>::value, C>::type interface_cast (T t)
{
	return t->operator C();
}

template <typename C, typename T> 
inline typename std::enable_if<!is_interface_type<T>::value, C>::type dynamic_interface_cast (T t)
{
	if (t == nullptr)
	{
		return nullptr;
	}

	return reinterpret_cast<C>(t->__get_interface(&std::remove_pointer<C>::type::__type));
}

template <typename C, typename T> 
inline typename std::enable_if<is_interface_type<T>::value, C>::type dynamic_interface_cast (T t)
{
	if (t == nullptr)
	{
		return nullptr;
	}

	return reinterpret_cast<C>(object_cast(t)->__get_interface(&std::remove_pointer<C>::type::__type));
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

// Boxing internals
template <typename T> inline typename std::enable_if<is_struct_type<T>::value, T>::type* __box (T t)
{
	// we do not need to call __new here as it already constructed
	return new T(t);
}

template <typename T, typename _CLASS = typename valuetype_to_class<T>::type> inline typename std::enable_if<is_value_type<T>::value && !is_struct_type<T>::value && !is_interface_type<T>::value, _CLASS>::type* __box (T t)
{
	return __new<_CLASS>(t);
}

template <typename T> inline typename std::enable_if<!is_value_type<T>::value && !is_interface_type<T>::value && !is_pointer_type<T>::value, T>::type __box (T t)
{
	return t;
}

template <typename T> inline typename std::enable_if<is_interface_type<T>::value, object*>::type __box (T t)
{
	return object_cast(t);
}

object* __box_pointer(void* p);
template <typename T> inline typename std::enable_if<is_pointer_type<T>::value, object*>::type __box (T t)
{
	return __box_pointer((void*)t);
}

// box - DEBUG
template <typename T> inline typename std::enable_if<is_struct_type<T>::value, T>::type* __box_debug (const char* _file, int _line, T t)
{
	// we do not need to call __new here as it already constructed
	return new (_file, _line) T(t);
}

template <typename T, typename _CLASS = typename valuetype_to_class<T>::type> inline typename std::enable_if<is_value_type<T>::value && !is_struct_type<T>::value && !is_interface_type<T>::value, _CLASS>::type* __box_debug (const char* _file, int _line, T t)
{
	return __new_debug<_CLASS>(_file, _line, t);
}

template <typename T> inline typename std::enable_if<!is_value_type<T>::value && !is_interface_type<T>::value && !is_pointer_type<T>::value, T>::type __box_debug (const char* _file, int _line, T t)
{
	return t;
}

template <typename T> inline typename std::enable_if<is_interface_type<T>::value, object*>::type __box_debug (const char* _file, int _line, T t)
{
	return object_cast(t);
}

template <typename T> inline typename std::enable_if<is_pointer_type<T>::value, object*>::type __box_debug (const char* _file, int _line, T t)
{
	return __box_pointer((void*)t);
}

// Unboxing internals
template <typename T> 
inline T __unbox(T* t)
{
	return *t;
}

template <typename T> 
inline typename std::enable_if<is_class_type<T>::value, T>::type __unbox(object* o)
{
	return cast<T>(o);
}

template <typename T, typename _CLASS = typename valuetype_to_class<T>::type, typename _VAL = typename class_to_valuetype<T>::type> 
inline typename std::enable_if<is_value_type<T>::value, _VAL>::type __unbox(object* o)
{
	return *cast<_CLASS*>(o);
}

template <typename T> 
inline typename std::enable_if<is_interface_type<T>::value || is_pointer_type<T>::value, T>::type __unbox(T t)
{
	return t;
}

template <typename T> 
inline typename std::enable_if<is_interface_type<T>::value, T>::type __unbox(object* o)
{
	return dynamic_interface_cast<T>(o);
}

void* __unbox_pointer(object* obj);
template <typename T> 
inline typename std::enable_if<is_pointer_type<T>::value, T>::type __unbox(object* o)
{
	return (T) __unbox_pointer(o);
}

// box - by ref
template <typename T> inline object* __box_ref_t (T* t)
{
	return __box(*t);
}

// unbox - to address
template <typename T> 
inline void __unbox_to_t(T* t, object* val)
{
	*t = __unbox<T>(val);
}

template <typename D, typename S> inline D map_pointer_cast(S s)
{
	union { D d; S s; } u;
	u.s = s;
	return u.d;
}

class __methods_table
{
public:

	virtual CoreLib::System::Type* __get_type()
	{
		throw 0xC000C000;
	}

	// TODO: add new, unbox methods as virtual and/or abstract
	virtual object* __box_ref(void* ref)
	{
		// default implementation for references;
		return *(object**)ref;
	}

	virtual void __unbox_to(void* ref, object* value)
	{
		// default implementation for references;
		*(object**)ref = value;
	}
};

// Typeof internals
template <typename T> inline CoreLib::System::Type* _typeof()
{
	typedef typename valuetype_to_class<typename std::remove_pointer<T>::type>::type _T;
	return &_T::__type;
}

template <typename T> inline __methods_table* _typeMT()
{
	typedef typename valuetype_to_class<typename std::remove_pointer<T>::type>::type _T;
	return &_T::_methods_table;
}

struct __runtimetype_info
{
	// TODO: finish it
	////CoreLib::System::Reflection::RuntimeAssembly* __assembly;
	////CoreLib::System::Reflection::RuntimeModule* __module;
	const char16_t* __name;
	const char16_t* __namespace;
	int __cor_element_type;
	bool __is_generic_type_definition;
};

int32_t __hash_code(object* _obj, size_t _size);

bool __equals_helper(object* _obj1, size_t _size1, object* _obj2, size_t _size2);