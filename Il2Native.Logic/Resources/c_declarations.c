// cast internals
template <typename D, typename S> 
inline typename std::enable_if<is_class_type<D>::value && is_class_type<S>::value && !is_nullable_type<D>::value, D>::type as(S s)
{
	return dynamic_cast<D>(s);
}

template <typename D, typename S> 
inline typename std::enable_if<is_value_type<D>::value || is_value_type<S>::value, D>::type as(S s)
{
	return nullptr;
}

// special case for interfaces
// add flag to each interface to be able to create is_interface<T>::value
template <typename D, typename S> 
inline typename std::enable_if<is_interface_type<D>::value && is_class_type<S>::value, D>::type as(S s)
{
	return dynamic_interface_cast<D>(s);
}

template <typename D, typename S> 
inline typename std::enable_if<is_class_type<D>::value && is_interface_type<S>::value, D>::type as(S s)
{
	return dynamic_cast<D>(object_cast(s));
}

template <typename D, typename S> 
inline typename std::enable_if<is_interface_type<D>::value && is_interface_type<S>::value, D>::type as(S s)
{
	return dynamic_interface_cast<D>(object_cast(s));
}

template <typename TNullable, typename S, typename T = typename remove_nullable<TNullable>::type, typename _CLASS = typename valuetype_to_class<T>::type, typename _BareNullable = typename std::remove_pointer<TNullable>::type>
inline typename std::enable_if<is_nullable_type<TNullable>::value && is_class_type<S>::value, _BareNullable>::type as(S s)
{
	auto casted = as<_CLASS*>(s);
	auto val = __default<_BareNullable>();
	if (casted != nullptr)
	{
		val.hasValue = true;
		val.value = __unbox(casted);
	}

	return val;
}

// cast internals
template <typename D, typename S> 
inline typename std::enable_if<is_class_type<D>::value && is_class_type<S>::value, bool>::type is(S s)
{
	return dynamic_cast<D>(s) != nullptr;
}

template <typename D, typename S> 
inline typename std::enable_if<is_value_type<D>::value && is_value_type<S>::value, bool>::type is(S s)
{
	return false;
}

template <typename D, typename S, typename _CLASS = typename valuetype_to_class<S>::type*>
inline typename std::enable_if<!is_value_type<D>::value && is_value_type<S>::value && std::is_same<D, _CLASS>::value, bool>::type is(S s)
{
	return true;
}

template <typename D, typename S, typename _CLASS = typename valuetype_to_class<S>::type*>
inline typename std::enable_if<!is_value_type<D>::value && is_value_type<S>::value && !std::is_same<D, _CLASS>::value, bool>::type is(S s)
{
	return false;
}

template <typename D, typename _CLASS = typename valuetype_to_class<D>::type*>
inline typename std::enable_if<is_value_type<D>::value, bool>::type is(object* o)
{
	return dynamic_cast<_CLASS>(o) != nullptr;
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

template <typename D, typename S> 
inline typename std::enable_if<is_interface_type<D>::value && is_interface_type<S>::value, bool>::type is(S s)
{
	return dynamic_interface_cast<D>(object_cast(s)) != nullptr;
}

// Constrained internals (for templates)
template <typename D, typename S> 
inline typename std::enable_if<std::is_same<D, S>::value, D>::type constrained (S s)
{
	return s;
}

template <typename D, typename S> 
inline typename std::enable_if<is_class_type<S>::value && !std::is_same<D, S>::value && !is_interface_type<D>::value, D>::type constrained (S s)
{
	return static_cast<D>(s);
}

template <typename D, typename S>
inline typename std::enable_if<is_class_type<S>::value && is_interface_type<D>::value, D>::type constrained(S s)
{
	return interface_cast<D>(s);
}

template <typename D, typename S> 
inline typename std::enable_if<is_value_type<S>::value && is_interface_type<D>::value, D>::type constrained (S& s)
{
	return interface_cast<D>(s);
}

template <typename D, typename S>
inline typename std::enable_if<is_interface_type<S>::value && is_interface_type<D>::value, D>::type constrained(S s)
{
	return interface_cast<D>(s);
}

template <typename D, typename S> 
inline typename std::enable_if<is_value_type<S>::value && is_class_type<D>::value, D>::type constrained (S s)
{
	return __box(s);
}

template <typename D, typename S> 
inline typename std::enable_if<is_interface_type<S>::value && is_class_type<D>::value && !is_object<D>::value, D>::type constrained (S s)
{
	return cast<D>(object_cast(s));
}

template <typename D, typename S> 
inline typename std::enable_if<is_interface_type<S>::value && is_class_type<D>::value && is_object<D>::value, D>::type constrained (S s)
{
	return object_cast(s);
}

// Decimals
int32_t DecAddSub(int32_t* d1, int32_t* d2, int32_t* res, uint8_t bSign);
int32_t DecCmp(int32_t* d1, int32_t* d2);
int32_t DecFromR4(float fltIn, int32_t* pdec);
int32_t DecFromR8(double dblIn, int32_t* pdec);
int32_t DecMul(int32_t* d1, int32_t* d2, int32_t* res);
int32_t DecDiv(int32_t* d1, int32_t* d2, int32_t* res);
int32_t R8FromDec(int32_t* pdec, double* pdblOut);

// Finally block
class Finally
{
public:
	std::function<void()> _dtor;
	Finally(std::function<void()> dtor) : _dtor(dtor) {};
	~Finally() { _dtor(); }
};

// Activator
template <typename T>
inline typename std::enable_if<std::is_pointer<T>::value, T>::type __create_instance()
{
	typedef typename std::remove_pointer<T>::type _T;
	return __new<_T>();
}

template <typename T>
inline typename std::enable_if<is_struct_type<T>::value, T>::type __create_instance()
{
	return __init<T>();
}

template <typename T>
inline typename std::enable_if<is_primitive_type<T>::value, T>::type __create_instance()
{
	return T();
}

template <typename T>
inline typename std::enable_if<std::is_void<T>::value, T>::type __create_instance()
{
	return;
}

template<typename T>
struct type_holder<__pointer<T>> { typedef __pointer<T> type; };

template <typename T> struct __pointer
{
	static CoreLib::System::RuntimeType __type;
	static __runtimetype_info __rt_info;
};

// Arrays internals
template <typename T> struct __array__type
{
	static CoreLib::System::RuntimeType __type;
	static __runtimetype_info __rt_info;
};

template<typename T>
struct type_holder<__array<T>> { typedef __array__type<T> type; };

template <typename T> class __array : public CoreLib::System::Array
{
public:
	int32_t _length;
	T _data[0];

	typedef CoreLib::System::Array base;
	__array(int32_t length) { _length = length; }
	__array(const __array<T>&) = delete;
	__array(__array<T>&&) = delete;

	static __array<T>* __new_array(int32_t length)
	{
		return allocate_array(length);
	}

	static __array<T>* __new_array_debug(const char* _file, int _line, int32_t length)
	{
		return allocate_array_debug(_file, _line, length);
	}

	template <typename... Ta> static __array<T>* __new_array_init(Ta... items)
	{
		auto instance = allocate_array(sizeof...(items));
		__init_array(instance, items...);
		return instance;
	}

	template <typename... Ta> static __array<T>* __new_array_init_debug(const char* _file, int _line, Ta... items)
	{
		auto instance = allocate_array_debug(_file, _line, sizeof...(items));
		__init_array(instance, items...);
		return instance;
	}

	inline static __array<T>* allocate_array(int32_t length)
	{
		auto size = sizeof(__array<T>) + length * sizeof(T);
		auto pointer = ::operator new (size, gc_traits<T>::value);
		return new (pointer) __array<T>(length);
	}

	inline static __array<T>* allocate_array_debug(const char* _file, int _line, int32_t length)
	{
		auto size = sizeof(__array<T>) + length * sizeof(T);
		auto pointer = ::operator new (size, gc_traits<T>::value, _file, _line);
		return new (pointer) __array<T>(length);
	}

	template <typename... Ta> inline static void __init_array(__array<T>* instance, Ta... items)
	{
		T tmp[] = {items...};
		auto data_size = sizeof...(items) * sizeof(T);
		memcpy(&instance->_data[0], &tmp, data_size);
	}

	inline const T operator [](int32_t index) const 
	{ 
		if (index < 0 || index >= _length)
		{
			throw __new<CoreLib::System::IndexOutOfRangeException>();
		}

		if (this == nullptr)
		{
			throw __new<CoreLib::System::NullReferenceException>();
		}

		return _data[index]; 
	}

	inline T& operator [](int32_t index) 
	{
		if (index < 0 || index >= _length)
		{
			throw __new<CoreLib::System::IndexOutOfRangeException>();
		}

		if (this == nullptr)
		{
			throw __new<CoreLib::System::NullReferenceException>();
		}

		return _data[index]; 
	}

	inline operator int32_t() const 
	{ 
		if (this == nullptr)
		{
			throw __new<CoreLib::System::NullReferenceException>();
		}

		return (size_t)_length; 
	}

	virtual object* __clone()
	{
		if (this == nullptr)
		{
			throw __new<CoreLib::System::NullReferenceException>();
		}

		auto instance = allocate_array(this->_length);
		auto data_size = this->_length * sizeof(T);
		memcpy(&instance->_data[0], &this->_data[0], data_size);		
		return instance;
	}

	virtual CoreLib::System::Type* __get_type() override;
	virtual bool __is_type(CoreLib::System::Type*) override;
	virtual uint32_t __get_size() override;

	// Array
	virtual int32_t __array_element_size() override;
	virtual bool __is_primitive_type_array() override;
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

	class __array_IListT1 : public CoreLib::System::Collections::Generic::IListT1<T>
	{
	public:
		__array_IListT1(__array<T>* class_) : _class{class_} {}
		operator object*()
		{
			return this->_class;
		}
		__array<T>* _class;
		T System_Collections_Generic_IListT1_get_Item(int32_t);
		void System_Collections_Generic_IListT1_set_Item(int32_t, T);
		int32_t System_Collections_Generic_IListT1_IndexOf(T);
		void System_Collections_Generic_IListT1_Insert(int32_t, T);
		void System_Collections_Generic_IListT1_RemoveAt(int32_t);
		int32_t System_Collections_Generic_ICollectionT1_get_Count();
		bool System_Collections_Generic_ICollectionT1_get_IsReadOnly();
		void System_Collections_Generic_ICollectionT1_Add(T);
		void System_Collections_Generic_ICollectionT1_Clear();
		bool System_Collections_Generic_ICollectionT1_Contains(T);
		void System_Collections_Generic_ICollectionT1_CopyTo(__array<T>*, int32_t);
		bool System_Collections_Generic_ICollectionT1_Remove(T);
		CoreLib::System::Collections::Generic::IEnumeratorT1<T>* System_Collections_Generic_IEnumerableT1_GetEnumerator();
		CoreLib::System::Collections::IEnumerator* System_Collections_IEnumerable_GetEnumerator();
	};
	operator CoreLib::System::Collections::Generic::IListT1<T>*()
	{
		return new (GCNormal::Default) __array_IListT1(this);
	}
	class __array_ICollectionT1 : public CoreLib::System::Collections::Generic::ICollectionT1<T>
	{
	public:
		__array_ICollectionT1(__array<T>* class_) : _class{class_} {}
		operator object*()
		{
			return this->_class;
		}
		__array<T>* _class;
		int32_t System_Collections_Generic_ICollectionT1_get_Count();
		bool System_Collections_Generic_ICollectionT1_get_IsReadOnly();
		void System_Collections_Generic_ICollectionT1_Add(T);
		void System_Collections_Generic_ICollectionT1_Clear();
		bool System_Collections_Generic_ICollectionT1_Contains(T);
		void System_Collections_Generic_ICollectionT1_CopyTo(__array<T>*, int32_t);
		bool System_Collections_Generic_ICollectionT1_Remove(T);
		CoreLib::System::Collections::Generic::IEnumeratorT1<T>* System_Collections_Generic_IEnumerableT1_GetEnumerator();
		CoreLib::System::Collections::IEnumerator* System_Collections_IEnumerable_GetEnumerator();
	};
	operator CoreLib::System::Collections::Generic::ICollectionT1<T>*()
	{
		return new (GCNormal::Default) __array_ICollectionT1(this);
	}
	class __array_IEnumerableT1 : public CoreLib::System::Collections::Generic::IEnumerableT1<T>
	{
	public:
		__array_IEnumerableT1(__array<T>* class_) : _class{class_} {}
		operator object*()
		{
			return this->_class;
		}
		__array<T>* _class;
		CoreLib::System::Collections::Generic::IEnumeratorT1<T>* System_Collections_Generic_IEnumerableT1_GetEnumerator();
		CoreLib::System::Collections::IEnumerator* System_Collections_IEnumerable_GetEnumerator();
	};
	operator CoreLib::System::Collections::Generic::IEnumerableT1<T>*()
	{
		return new (GCNormal::Default) __array_IEnumerableT1(this);
	}
	class __array_IEnumerable : public CoreLib::System::Collections::IEnumerable
	{
	public:
		__array_IEnumerable(__array<T>* class_) : _class{class_} {}
		operator object*()
		{
			return this->_class;
		}
		__array<T>* _class;
		CoreLib::System::Collections::IEnumerator* System_Collections_IEnumerable_GetEnumerator();
	};
	operator CoreLib::System::Collections::IEnumerable*()
	{
		return new __array_IEnumerable(this);
	}
	class __array_IList : public CoreLib::System::Collections::IList
	{
	public:
		__array_IList(__array<T>* class_) : _class{class_} {}
		operator object*()
		{
			return this->_class;
		}
		__array<T>* _class;
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
		void System_Collections_ICollection_CopyTo(CoreLib::System::Array*, int32_t);
		int32_t System_Collections_ICollection_get_Count();
		CoreLib::System::Object* System_Collections_ICollection_get_SyncRoot();
		bool System_Collections_ICollection_get_IsSynchronized();
		CoreLib::System::Collections::IEnumerator* System_Collections_IEnumerable_GetEnumerator();
	};
	operator CoreLib::System::Collections::IList*()
	{
		return new (GCNormal::Default) __array_IList(this);
	}
	class __array_ICollection : public CoreLib::System::Collections::ICollection
	{
	public:
		__array_ICollection(__array<T>* class_) : _class{class_} {}
		operator object*()
		{
			return this->_class;
		}
		__array<T>* _class;
		void System_Collections_ICollection_CopyTo(CoreLib::System::Array*, int32_t);
		int32_t System_Collections_ICollection_get_Count();
		CoreLib::System::Object* System_Collections_ICollection_get_SyncRoot();
		bool System_Collections_ICollection_get_IsSynchronized();
		CoreLib::System::Collections::IEnumerator* System_Collections_IEnumerable_GetEnumerator();
	};
	operator CoreLib::System::Collections::ICollection*()
	{
		return new (GCNormal::Default) __array_ICollection(this);
	}
};

template <typename T, int32_t RANK> struct __multi_array__type
{
	static CoreLib::System::RuntimeType __type;
	static __runtimetype_info __rt_info;
};

template <typename T, int32_t RANK>
struct type_holder<__multi_array<T, RANK>> { typedef __multi_array__type<T, RANK> type; };

template <typename T, int32_t RANK> class __multi_array : public CoreLib::System::Array
{
public:
	int32_t _lowerBoundries[RANK];
	int32_t _upperBoundries[RANK];
	T _data[0];

	typedef CoreLib::System::Array base;

	__multi_array()
	{
	}

	__multi_array(std::initializer_list<int32_t> boundries) : _lowerBoundries{0}
	{
		std::copy(std::begin(boundries), std::end(boundries), _upperBoundries);
	}

	const T operator [](std::initializer_list<int32_t> indexes) const 
	{ 
		if (this == nullptr)
		{
			throw __new<CoreLib::System::NullReferenceException>();
		}

		return _data[calculate_index(indexes)]; 
	}

	T& operator [](std::initializer_list<int32_t> indexes) 
	{ 
		if (this == nullptr)
		{
			throw __new<CoreLib::System::NullReferenceException>();
		}

		return _data[calculate_index(indexes)];  
	}

	int32_t calculate_index(std::initializer_list<int32_t>& indexes)
	{
		auto index = 0;
		auto index_multiplier = 1;
		auto rank = 0;
		for (auto levelIndex : indexes)
		{
			index += levelIndex * index_multiplier;
			auto lower = _lowerBoundries[rank];
			auto upper = _upperBoundries[rank];
			if (levelIndex < lower || levelIndex >= upper)
			{
				throw __new<CoreLib::System::IndexOutOfRangeException>();
			}

			index_multiplier *= (upper - lower);
			rank++;
		}

		return index;
	}

	int32_t calculate_index(int32_t* indexes)
	{
		auto index = 0;
		auto index_multiplier = 1;
		auto rank = 0;
		for (auto index = 0; index < RANK; index++)
		{
			auto levelIndex = *(indexes + index);
			index += levelIndex * index_multiplier;
			auto lower = _lowerBoundries[rank];
			auto upper = _upperBoundries[rank];
			if (levelIndex < lower || levelIndex >= upper)
			{
				throw __new<CoreLib::System::IndexOutOfRangeException>();
			}

			index_multiplier *= (upper - lower);
			rank++;
		}

		return index;
	}

	template <typename... Ta> static __multi_array<T, RANK>* __new_array(std::initializer_list<int32_t> boundries)
	{
		return allocate_multiarray(boundries);
	}

	template <typename... Ta> static __multi_array<T, RANK>* __new_array_debug(const char* _file, int _line, std::initializer_list<int32_t> boundries)
	{
		return allocate_multiarray_debug(_file, _line, boundries);
	}

	template <typename... Ta> static __multi_array<T, RANK>* __new_array_init(std::initializer_list<int32_t> boundries, Ta... items)
	{
		auto instance = allocate_multiarray(boundries);
		__init_array(instance, items...);
		return instance;
	}

	template <typename... Ta> static __multi_array<T, RANK>* __new_array_init_debug(const char* _file, int _line, std::initializer_list<int32_t> boundries, Ta... items)
	{
		auto instance = allocate_multiarray_debug(_file, _line, boundries);
		__init_array(instance, items...);
		return instance;
	}

	inline static __multi_array<T, RANK>* allocate_multiarray(std::initializer_list<int32_t> boundries)
	{
		auto length = std::accumulate(std::begin(boundries), std::end(boundries), 1, std::multiplies<int32_t>());
		auto size = sizeof(__multi_array<T, RANK>) + length * sizeof(T);
		auto pointer = ::operator new (size, gc_traits<T>::value);
		return new (pointer) __multi_array<T, RANK>(boundries);
	}

	inline static __multi_array<T, RANK>* allocate_multiarray(size_t length)
	{
		auto size = sizeof(__multi_array<T, RANK>) + length * sizeof(T);
		auto pointer = ::operator new (size, gc_traits<T>::value);
		return new (pointer) __multi_array<T, RANK>();
	}

	inline static __multi_array<T, RANK>* allocate_multiarray_debug(const char* _file, int _line, std::initializer_list<int32_t> boundries)
	{
		auto length = std::accumulate(std::begin(boundries), std::end(boundries), 1, std::multiplies<int32_t>());
		auto size = sizeof(__multi_array<T, RANK>) + length * sizeof(T);
		auto pointer = ::operator new (size, gc_traits<T>::value, _file, _line);
		return new (pointer) __multi_array<T, RANK>(boundries);
	}

	template <typename... Ta> inline static void __init_array(__multi_array<T, RANK>* instance, Ta... items)
	{
		// initialize
		T tmp[] = {items...};
		auto data_size =  sizeof...(items) * sizeof(T);
		memcpy(&instance->_data[0], &tmp, data_size);
	}

	virtual object* __clone() override
	{
		if (this == nullptr)
		{
			throw __new<CoreLib::System::NullReferenceException>();
		}

		auto length = 1;
		for (auto rank = 0; rank < RANK; rank++)
		{
			length *= (_upperBoundries[rank] - _lowerBoundries[rank]);
		}

		auto instance = allocate_multiarray(length);
		auto data_size = length * sizeof(T);
		memcpy(&instance->_lowerBoundries[0], &this->_lowerBoundries[0], sizeof(int32_t) * RANK);
		memcpy(&instance->_upperBoundries[0], &this->_upperBoundries[0], sizeof(int32_t) * RANK);
		memcpy(&instance->_data[0], &this->_data[0], data_size);		
		return instance;
	}

	virtual CoreLib::System::Type* __get_type() override;
	virtual bool __is_type(CoreLib::System::Type*) override;

	virtual int32_t __array_element_size() override;
	virtual bool __is_primitive_type_array() override;
	virtual void InternalGetReference(void*, int32_t, int32_t*) override;
	virtual int32_t get_Length() override;
	virtual int32_t GetUpperBound(int32_t dimension) override;
	virtual int32_t GetLowerBound(int32_t dimension) override;
	virtual int32_t GetLength(int32_t dimension) override;
	virtual int32_t get_Rank() override;
};

// allocator
template<class T>
class gc_allocator_with_realloc {
public:
	typedef T value_type;
	typedef size_t size_type;
	typedef ptrdiff_t difference_type;

	typedef T* pointer;
	typedef const T* const_pointer;
	typedef T& reference;
	typedef const T& const_reference;

	gc_allocator_with_realloc() {}
	gc_allocator_with_realloc(const gc_allocator_with_realloc&) {}
	~gc_allocator_with_realloc() {}

	pointer address(reference r) const  { return &r; }
	const_pointer address(const_reference r) const  { return &r; }

	pointer allocate(size_type n, const_pointer = 0) 
	{
		return static_cast<pointer>(GC_MALLOC_UNCOLLECTABLE(n * sizeof(value_type)));
	}

	void deallocate(pointer p, size_type) 
	{
		GC_FREE(p);
	}

	pointer reallocate(pointer p, size_type n) 
	{
		return static_cast<pointer>(GC_REALLOC(p, n * sizeof(value_type)));
	}

	size_type max_size() const  
	{
		return static_cast<size_type>(-1) / sizeof(value_type);
	}

	void construct(pointer p, const value_type& val) 
	{
		new(p) value_type(val);
	}
	void destroy(pointer p) { p->~value_type(); }

	template <class U>
	gc_allocator_with_realloc(const gc_allocator_with_realloc<U>&) {}

	template<class U>
	struct rebind 
	{
		typedef gc_allocator_with_realloc<U> other;
	};
};

// gc_allocator_with_realloc<void> specialization.
template<>
class gc_allocator_with_realloc<void> {
public:
	typedef void value_type;
	typedef size_t size_type;
	typedef ptrdiff_t difference_type;
	typedef void* pointer;
	typedef const void* const_pointer;

	template<class U>
	struct rebind 
	{
		typedef gc_allocator_with_realloc<U> other;
	};
};

template<class T>
inline bool operator==(const gc_allocator_with_realloc<T>&, const gc_allocator_with_realloc<T>&) 
{
	return true;
}

template<class T>
inline bool operator!=(const gc_allocator_with_realloc<T>&, const gc_allocator_with_realloc<T>&) 
{
	return false;
}

#ifdef NO_TIMED_MUTEX
#ifndef GC_PTHREADS
struct __monitor
{
	HANDLE	_mutex;
	HANDLE	_cond;
	LONG volatile _waiting;

public:
	__monitor()
	{
		_mutex = CreateMutex(NULL, FALSE, NULL);
		_cond = CreateSemaphore(NULL, 0, 0x7FFFFFFF, NULL);
		_waiting = 0;
	}

	~__monitor()
	{
		CloseHandle(_cond);
		CloseHandle(_mutex);
	}

	void lock()
	{
		WaitForSingleObject(_mutex, INFINITE);
	}

	bool try_lock()
	{
		return WaitForSingleObject(_mutex, 10) ==  WAIT_OBJECT_0;
	}

	template< class Rep, class Period >
	bool try_lock_for( const std::chrono::duration<Rep,Period>& timeout_duration )
	{
		return WaitForSingleObject(_mutex, std::chrono::duration_cast<std::chrono::milliseconds>(timeout_duration).count()) ==  WAIT_OBJECT_0;;
	}

	void unlock()
	{
		ReleaseMutex(_mutex);
	}

	void notify_one()
	{
		if (_interlocked_compare_exchange(&_waiting, 0, 0) > 0)
		{
			_interlocked_sub(_waiting, 1);
			ReleaseSemaphore(_cond, 1, NULL);
		}
	}

	void notify_all()
	{
		auto _count = _interlocked_compare_exchange(&_waiting, 0, 0);
		if (_count > 0)
		{
			_interlocked_exchange(&_waiting, 0);
			ReleaseSemaphore(_cond, _count, NULL);
		}
	}

	void wait()
	{
		WaitForSingleObject(_mutex, INFINITE);
	}

	template< class Rep, class Period >
	std::cv_status wait_for(const std::chrono::duration<Rep, Period>& rel_time)
	{
		auto result = WaitForSingleObject(_mutex, std::chrono::duration_cast<std::chrono::milliseconds>(rel_time).count());
		return result == WAIT_OBJECT_0 ? cv_status::no_timeout : cv_status::timeout;
	}
};
#else

#include <time.h>

struct __monitor
{
	pthread_mutex_t	_mutex;
	pthread_cond_t	_cond;

public:
	__monitor()
	{
		pthread_mutex_init(&_mutex, 0);
		pthread_cond_init(&_cond, 0);
	}

	~__monitor()
	{
		pthread_cond_destroy(&_cond);
		pthread_mutex_destroy(&_mutex);
	}

	void lock()
	{
		pthread_mutex_lock(&_mutex);
	}

	bool try_lock()
	{
		return pthread_mutex_trylock(&_mutex) == 0;
	}

	template< class Rep, class Period >
	bool try_lock_for( const std::chrono::duration<Rep,Period>& timeout_duration )
	{
		struct timespec timestruct;
		auto millisecondsTimeout = std::chrono::duration_cast<std::chrono::milliseconds>(timeout_duration).count();

		clock_gettime(CLOCK_REALTIME, &timestruct);

		timestruct.tv_sec += millisecondsTimeout / 1000;
		timestruct.tv_nsec += (millisecondsTimeout % 1000) * 1000000;

		return pthread_cond_timedwait(&_cond, &_mutex, &timestruct);
	}

	void unlock()
	{
		pthread_mutex_unlock(&_mutex);
	}

	void notify_one()
	{
		pthread_cond_signal(&_cond);
	}

	void notify_all()
	{
		pthread_cond_broadcast(&_cond);
	}

	void wait()
	{
		pthread_cond_wait(&_cond, &_mutex);
	}

	template< class Rep, class Period >
	std::cv_status wait_for( const std::chrono::duration<Rep, Period>& rel_time)
	{
		struct timespec timestruct;
		auto millisecondsTimeout = std::chrono::duration_cast<std::chrono::milliseconds>(rel_time).count();

		clock_gettime(CLOCK_REALTIME, &timestruct);

		timestruct.tv_sec += millisecondsTimeout / 1000;
		timestruct.tv_nsec += (millisecondsTimeout % 1000) * 1000000;

		auto result = pthread_cond_timedwait(&_cond, &_mutex, &timestruct);
		return result == ETIMEDOUT ? std::cv_status::timeout : std::cv_status::no_timeout;
	}
};
#endif
#else
struct __monitor
{
	std::timed_mutex _mutex;
	std::condition_variable_any	_cond;

public:
	__monitor()
	{
	}

	void lock()
	{
		_mutex.lock();
	}

	bool try_lock()
	{
		return _mutex.try_lock();
	}

	template< class Rep, class Period >
	bool try_lock_for( const std::chrono::duration<Rep,Period>& timeout_duration )
	{
		return _mutex.try_lock_for(timeout_duration);
	}

	void unlock()
	{
		_mutex.unlock();
	}

	void notify_one()
	{
		_cond.notify_one();
	}

	void notify_all()
	{
		_cond.notify_all();
	}

	void wait()
	{
		_cond.wait(_mutex);
	}

	template< class Rep, class Period >
	std::cv_status wait_for(const std::chrono::duration<Rep, Period>& rel_time)
	{
		return _cond.wait_for(_mutex, rel_time);
	}
};
#endif

struct __object_extras
{
	__monitor monitor;
};

class __object_extras_storage
{
public:

	typedef std::unordered_map<object*, __object_extras*, std::hash<const object*>, std::equal_to<const object*>, gc_allocator_with_realloc<std::pair<const object*, __object_extras*> > > map;

	__object_extras* operator[] (object* obj)
	{
		std::lock_guard<std::mutex> lock(mutex);
		map::const_iterator got = __extras.find (obj);
		if (got != __extras.end())
		{
			return got->second;
		}

		auto new_object_extras = new __object_extras();
		__extras[obj] = new_object_extras;
		return new_object_extras;
	}

	void free(object* obj)
	{
		std::lock_guard<std::mutex> lock(mutex);
		map::const_iterator got = __extras.find (obj);
		if (got != __extras.end())
		{
			delete got->second;
			__extras.erase(got);
		}
	}

	~__object_extras_storage()
	{
		std::lock_guard<std::mutex> lock(mutex);
		for (auto item : __extras) 
		{
			delete item.second;
		}  
	}

	map __extras;
	mutable std::mutex mutex;
};

extern __object_extras_storage* __object_extras_storage_instance;

class __strings_storage
{
public:

	typedef std::unordered_map<const char16_t*, string*, std::hash<const char16_t*>, std::equal_to<const char16_t*>, gc_allocator_with_realloc<std::pair<const char16_t*, string*> > > map;

	string* operator() (const char16_t* str, size_t length)
	{
		std::lock_guard<std::mutex> lock(mutex);
		map::const_iterator got = __strings.find (str);
		if (got != __strings.end())
		{
			return got->second;
		}

		auto _new_string = string::FastAllocateString(length);
		string::wstrcpy(&_new_string->m_firstChar, (char16_t*)str, length);
		__strings[str] = _new_string;
		return _new_string;
	}

	void free(const char16_t* str)
	{
		std::lock_guard<std::mutex> lock(mutex);
		map::const_iterator got = __strings.find (str);
		if (got != __strings.end())
		{
			delete got->second;
			__strings.erase(got);
		}
	}

	~__strings_storage()
	{
		std::lock_guard<std::mutex> lock(mutex);
		for (auto item : __strings) 
		{
			delete item.second;
		}  
	}

	map __strings;
	mutable std::mutex mutex;
};

extern __strings_storage* __strings_storage_instance;

// String literal
inline string* operator "" _s(const char16_t* ptr, size_t length)
{
	return __strings_storage_instance->operator()(ptr, length);
}

// Enum operators
template < typename T, class = typename std::enable_if<std::is_enum<T>::value, T>::type >
inline T operator |(T left, T right)
{
	typedef typename std::underlying_type<T>::type U;
	return (T) ((U) left | (U) right);
}

template < typename T, class = typename std::enable_if<std::is_enum<T>::value, T>::type >
inline T operator &(T left, T right)
{
	typedef typename std::underlying_type<T>::type U;
	return (T) ((U) left & (U) right);
}

template < typename T, class = typename std::enable_if<std::is_enum<T>::value, T>::type >
inline T operator ^(T left, T right)
{
	typedef typename std::underlying_type<T>::type U;
	return (T) ((U) left ^ (U) right);
}

template < typename T, class = typename std::enable_if<std::is_enum<T>::value, T>::type >
inline T operator ~(T left)
{
	typedef typename std::underlying_type<T>::type U;
	return (T) ~((U) left);
}

template < typename T, class = typename std::enable_if<std::is_enum<T>::value, T>::type >
inline T operator |=(T& left, T right)
{
	typedef typename std::underlying_type<T>::type U;
	return left = (T) (left | right);
}

template < typename T, class = typename std::enable_if<std::is_enum<T>::value, T>::type >
inline T operator &=(T& left, T right)
{
	typedef typename std::underlying_type<T>::type U;
	return left = (T) (left & right);
}

template < typename T, class = typename std::enable_if<std::is_enum<T>::value, T>::type >
inline T operator ^=(T& left, T right)
{
	typedef typename std::underlying_type<T>::type U;
	return left = (T) (left ^ right);
}

// Equals helper
template < typename T, class = typename std::enable_if<is_struct_type<T>::value, T>::type >
inline bool operator ==(const T& left, const T& right)
{
	return std::memcmp((void*)&left, (void*)&right, sizeof(T)) == 0;
}

// string helpers
inline string* __utf8_to_string(char* str)
{
	return string::CreateStringFromEncoding((uint8_t*)str, std::strlen(str), CoreLib::System::Text::Encoding::get_UTF8());
}

inline string* __ascii_to_string(char* str)
{
	return string::CreateStringFromEncoding((uint8_t*)str, std::strlen(str), CoreLib::System::Text::Encoding::get_ASCII());
}

inline string* __wchar_t_to_string(char16_t* str)
{
	return string::CtorCharPtr(str);
}

// support functions
void __startup();
void __shutdown();
__array<string*>* __get_arguments(int32_t argc, char* argv[]);
