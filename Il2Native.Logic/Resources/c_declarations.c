// interface cast
template <typename C, typename T> 
inline C interface_cast (T t)
{
	return nullptr;
}

template <typename C, typename T> 
inline C dynamic_interface_cast (T t)
{
	return nullptr;
}

// object cast (interface etc)
template <typename T> 
inline object* object_cast (T t)
{
	return nullptr;
}

// Boxing internals
template <typename T> inline typename std::enable_if<is_struct_type<T>::value, T>::type* __box (T t)
{
	// as we working with __init structs we do not need to call Ctors second time here
	auto mem = new T;
	std::memcpy(mem, &t, sizeof(T));
	return mem;
}

template <typename T> inline typename std::enable_if<!is_struct_type<T>::value && !is_primitive_type<T>::value && !is_interface_type<T>::value, T>::type __box (T t)
{
	return t;
}

template <typename D, typename S> inline typename std::enable_if<is_interface_type<S>::value && std::is_same<D, S>::value, D>::type __box (S s)
{
	return s;
}

template <typename D, typename S> inline typename std::enable_if<is_interface_type<S>::value && std::is_same<D, object*>::value, object*>::type __box (S s)
{
	return object_cast(s);
}

// Unboxing internals
template <typename D, typename S> inline D __unbox(S* c)
{
	// TODO: finish it
	D d;
	return d;
}

// cast internals
template <typename D, typename S> inline D as(S s)
{
	return dynamic_cast<D>(s);
}

// cast internals
template <typename D, typename S> inline D cast(S s)
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
template <typename D, typename S> 
inline typename std::enable_if<is_class_type<D>::value && is_class_type<S>::value, bool>::type is(S s)
{
	return as<D>(s) != nullptr;
}

template <typename D, typename S> 
inline typename std::enable_if<is_value_type<D>::value || is_value_type<S>::value, bool>::type is(S s)
{
	return false;
}

// special case for interfaces
// add flag to each interface to be able to create is_interface<T>::value
template <typename D, typename S> 
inline typename std::enable_if<is_interface_type<D>::value && is_class_type<S>::value, bool>::type is(S s)
{
	return dynamic_interface_cast<D>(s) != nullptr;
}

template <typename D, typename S> 
inline typename std::enable_if<is_class_type<D>::value && is_interface_type<S>::value, bool>::type is(S s)
{
	return dynamic_cast<D>(object_cast(s)) != nullptr;
}

// Constrained internals (for templates)
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

// Decimals
int32_t DecAddSub(int32_t* d1, int32_t* d2, int32_t* res, uint8_t bSign);
int32_t DecCmp(int32_t* d1, int32_t* d2);
int32_t DecFromR4(float fltIn, int32_t* pdec);
int32_t DecFromR8(double dblIn, int32_t* pdec);
int32_t DecDiv(int32_t* d1, int32_t* d2, int32_t* res);

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

// Activator
template <typename T> 
typename std::enable_if<is_class_type<T>::value, T>::type __create_instance()
{
	typedef typename std::remove_pointer<T>::type _T;
	return __new<_T>();
}

template <typename T> 
T __create_instance()
{
	return __init<T>();
}

// Arrays internals
template <typename T> class __array : public virtual CoreLib::System::Array, public virtual CoreLib::System::Collections::Generic::IListT1<T>
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

	static __array<T>* __new_array(int32_t length)
	{
		auto size = sizeof(__array<T>) + length * sizeof(T);
		return new (size) __array<T>(length);
	}

	template <typename... Ta> static __array<T>* __new_array_init(Ta... items)
	{
		auto size = sizeof(__array<T>) + sizeof...(items) * sizeof(T);
		auto instance = new (size) __array<T>(sizeof...(items));

		// initialize
		T tmp[] = {items...};
		memcpy(&instance->_data[0], &tmp, size);

		return instance;
	}

	inline const T operator [](int32_t index) const { return _data[index]; }
	inline T& operator [](int32_t index) { return _data[index]; }
	inline operator int32_t() const { return (size_t)_length; }

	// Array
	virtual int32_t __array_element_size() override;
	virtual void InternalGetReference(void*, int32_t, int32_t*) override;
	virtual int32_t get_Length() override;
	virtual int32_t get_Rank() override;

	// IListT1
	T System_Collections_Generic_IListT1_get_Item(int32_t);
	void System_Collections_Generic_IListT1_set_Item(int32_t, T);
	int32_t System_Collections_Generic_IListT1_IndexOf(T);
	void System_Collections_Generic_IListT1_Insert(int32_t, T);
	void System_Collections_Generic_IListT1_RemoveAt(int32_t);

	// ICollectionT1
	int32_t System_Collections_Generic_ICollectionT1_get_Count();
	bool System_Collections_Generic_ICollectionT1_get_IsReadOnly();
	void System_Collections_Generic_ICollectionT1_Add(T);
	void System_Collections_Generic_ICollectionT1_Clear(); 
	bool System_Collections_Generic_ICollectionT1_Contains(T);
	void System_Collections_Generic_ICollectionT1_CopyTo(__array<T>*, int32_t);
	bool System_Collections_Generic_ICollectionT1_Remove(T);

	// IEnumerableT1
	CoreLib::System::Collections::Generic::IEnumeratorT1<T>* System_Collections_Generic_IEnumerableT1_GetEnumerator();

	// IList
	CoreLib::System::Object* System_Collections_IList_get_Item(int32_t);
	void System_Collections_IList_set_Item(int32_t, CoreLib::System::Object*);
	int32_t System_Collections_IList_Add(CoreLib::System::Object*);
	bool System_Collections_IList_Contains(CoreLib::System::Object*);
	void System_Collections_IList_Clear();
	bool System_Collections_IList_get_IsReadOnly();
	bool System_Collections_IList_get_IsFixedSize(); 
	int32_t System_Collections_IList_IndexOf(CoreLib::System::Object*);
	void System_Collections_IList_Insert(int32_t, CoreLib::System::Object*);
	void System_Collections_IList_Remove(CoreLib::System::Object*);
	void System_Collections_IList_RemoveAt(int32_t);

	// ICollection
	void System_Collections_ICollection_CopyTo(CoreLib::System::Array*, int32_t);
	int32_t System_Collections_ICollection_get_Count();
	CoreLib::System::Object* System_Collections_ICollection_get_SyncRoot();
	bool System_Collections_ICollection_get_IsSynchronized();

	// IEnumerable
	CoreLib::System::Collections::IEnumerator* System_Collections_IEnumerable_GetEnumerator();
};

template <typename T, int32_t RANK> class __multi_array : public virtual CoreLib::System::Array
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
