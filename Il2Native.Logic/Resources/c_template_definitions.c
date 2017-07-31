// TypedReference
template <typename T> inline ::CoreLib::System::TypedReference __makeref(T* t)
{
	::CoreLib::System::TypedReference __MakeRef;
	__MakeRef = __init<::CoreLib::System::TypedReference>();
	__MakeRef.Value = __init<::CoreLib::System::IntPtr>((void*)t);
	__MakeRef.Type = __init<::CoreLib::System::IntPtr>((void*)_typeMT<T>());
	return __MakeRef;
}

template <typename T> inline T& __refvalue(::CoreLib::System::TypedReference tr)
{
	return (T&)*(T*)((void*)tr.Value);
}

inline ::CoreLib::System::Type* __reftype(::CoreLib::System::TypedReference tr)
{
	return ((__methods_table*) ((void*)tr.Type))->__get_type();
}

template <typename T> inline T& __refvalue(::CoreLib::System::TypedReference* tr)
{
	return (T&)*(T*)((void*)tr->Value);
}

inline ::CoreLib::System::Type* __reftype(::CoreLib::System::TypedReference* tr)
{
	return ((__methods_table*)((void*)tr->Type))->__get_type();
}

// Pointer
template <typename T>
::CoreLib::System::RuntimeType __pointer<T>::__type = ::CoreLib::System::RuntimeType(&__pointer<T>::__rt_info);

template <typename T>
__runtimetype_info __pointer<T>::__rt_info = { _typeMT<::CoreLib::System::Reflection::Pointer>(), nullptr, nullptr, 15, false, 0, _runtime_typeof<::CoreLib::System::Reflection::Pointer>(), _runtime_typeof<T>() };

// Array
template <typename T>
int32_t  __array<T>::__array_element_size()
{
	return sizeof(T);
}

template <typename T>
bool  __array<T>::__is_primitive_type_array()
{
	return is_primitive_type<T>::value;
}

template <typename T>
uint32_t  __array<T>::__get_size()
{
	return sizeof(__array<T>) + get_Length() * __array_element_size();
}

template <typename T>
void*  __array<T>::__get_interface(::CoreLib::System::Type* value)
{
	if (_typeof<::CoreLib::System::Collections::Generic::IEnumerableT1<T>*>() == value)
	{
		return interface_cast<::CoreLib::System::Collections::Generic::IEnumerableT1<T>*>(this);
	}

	return base::__get_interface(value);
}

template <typename T>
void __array<T>::InternalGetReference(void* elemRef, int32_t rank, int32_t* pIndices)
{
	if (rank != 1)
	{
		throw __new<::CoreLib::System::RankException>(u"rank"_s);
	}

	if (elemRef == nullptr)
	{
		throw __new<::CoreLib::System::ArgumentNullException>(u"elemRef"_s);
	}	

	if (pIndices == nullptr)
	{
		throw __new<::CoreLib::System::ArgumentNullException>(u"pIndices"_s);
	}	

	auto index = pIndices[0];
	if (index < 0 || index >= this->_length)
	{
		throw __new<::CoreLib::System::IndexOutOfRangeException>();
	}	

	auto typedRef = reinterpret_cast<::CoreLib::System::TypedReference*>(elemRef);
	typedRef->Value = __init<::CoreLib::System::IntPtr>((void*)&this->_data[index]);
	typedRef->Type = __init<::CoreLib::System::IntPtr>((void*)_typeMT<T>());
}

template <typename T>
int32_t __array<T>::get_Length()
{
	return this->_length;
}

template <typename T>
int32_t __array<T>::get_Rank()
{
	return 1;
}

template <typename T>
::CoreLib::System::RuntimeType __array__type<T>::__type = ::CoreLib::System::RuntimeType(&__array__type<T>::__rt_info);

template <typename T>
__runtimetype_info __array__type<T>::__rt_info = { _typeMT<__array<T>>(), nullptr, nullptr, 20, false, 0, _runtime_typeof<::CoreLib::System::Array>(), _runtime_typeof<T>() };

// Method : 
template <typename T>
::CoreLib::System::Type* __array<T>::__get_type()
{
	return _typeof<__array<T>>();
}

// Method : 
template <typename T>
bool __array<T>::__is_type(::CoreLib::System::Type* value)
{
	return ((_typeof<__array<T>>() == value) || base::__is_type(value));
}

// IListT1
template <typename T>
T __array<T>::System_Collections_Generic_IListT1_get_Item(int32_t pos) 
{
	return this->_data[pos];
}

template <typename T>
void __array<T>::System_Collections_Generic_IListT1_set_Item(int32_t pos, T t)
{
	this->_data[pos] = t;
}

template <typename T>
int32_t __array<T>::System_Collections_Generic_IListT1_IndexOf(T t)
{
	for (auto index = 0; index < _length; index++)
	{
		if (t == this->_data[index])
		{
			return index;
		}
	}

	return -1;
}

template <typename T>
void __array<T>::System_Collections_Generic_IListT1_Insert(int32_t pos, T t)
{
	throw __new<::CoreLib::System::NotSupportedException>();
}

template <typename T>
void __array<T>::System_Collections_Generic_IListT1_RemoveAt(int32_t pos)
{
	throw __new<::CoreLib::System::NotSupportedException>();
}

// ICollectionT1
template <typename T>
int32_t __array<T>::System_Collections_Generic_ICollectionT1_get_Count() 
{
	return get_Length();
}

template <typename T>
bool __array<T>::System_Collections_Generic_ICollectionT1_get_IsReadOnly() 
{
	return get_IsReadOnly();
}

template <typename T>
void __array<T>::System_Collections_Generic_ICollectionT1_Add(T t) 
{
	throw __new<::CoreLib::System::NotSupportedException>();
}

template <typename T>
void __array<T>::System_Collections_Generic_ICollectionT1_Clear() 
{
	throw __new<::CoreLib::System::NotSupportedException>();
}

template <typename T>
bool __array<T>::System_Collections_Generic_ICollectionT1_Contains(T t) 
{
	for (auto index = 0; index < _length; index++)
	{
		if (t == this->_data[index])
		{
			return true;
		}
	}

	return false;
}

template <typename T>
void __array<T>::System_Collections_Generic_ICollectionT1_CopyTo(__array<T>* dest, int32_t pos) 
{
	CopyTo(dest, pos);
}

template <typename T>
bool __array<T>::System_Collections_Generic_ICollectionT1_Remove(T t) 
{
	throw __new<::CoreLib::System::NotSupportedException>();
}

// IEnumerableT1
template <typename T>
::CoreLib::System::Collections::Generic::IEnumeratorT1<T>* __array<T>::System_Collections_Generic_IEnumerableT1_GetEnumerator()
{
	return __new<::CoreLib::System::ArraySegmentT1<T>>(this)->System_Collections_Generic_IEnumerableT1_GetEnumerator();
}

// IList
template <typename T>
::CoreLib::System::Object* __array<T>::System_Collections_IList_get_Item(int32_t pos)
{
	return __box(this->_data[pos]);
}

template <typename T>
void __array<T>::System_Collections_IList_set_Item(int32_t pos, ::CoreLib::System::Object* obj)
{
	this->_data[pos] = __unbox<T>(obj);
}

template <typename T>
int32_t __array<T>::System_Collections_IList_Add(::CoreLib::System::Object* obj) 
{
	throw __new<::CoreLib::System::NotSupportedException>();
}

template <typename T>
bool __array<T>::System_Collections_IList_Contains(::CoreLib::System::Object* obj) 
{
	for (auto index = 0; index < _length; index++)
	{
		if (__box(this->_data[index])->Equals(obj))
		{
			return true;
		}
	}

	return false;
}

template <typename T>
void __array<T>::System_Collections_IList_Clear()
{
	throw __new<::CoreLib::System::NotSupportedException>();
}

template <typename T>
bool __array<T>::System_Collections_IList_get_IsReadOnly()
{
	return get_IsReadOnly();
}

template <typename T>
bool __array<T>::System_Collections_IList_get_IsFixedSize() 
{
	return get_IsFixedSize();
}

template <typename T>
int32_t __array<T>::System_Collections_IList_IndexOf(::CoreLib::System::Object* obj)
{
	for (auto index = 0; index < _length; index++)
	{
		if (__box(this->_data[index])->Equals(obj))
		{
			return index;
		}
	}

	return -1;
}

template <typename T>
void __array<T>::System_Collections_IList_Insert(int32_t, ::CoreLib::System::Object* obj)
{
	throw __new<::CoreLib::System::NotSupportedException>();
}

template <typename T>
void __array<T>::System_Collections_IList_Remove(::CoreLib::System::Object* obj)
{
	throw __new<::CoreLib::System::NotSupportedException>();
}

template <typename T>
void __array<T>::System_Collections_IList_RemoveAt(int32_t pos) 
{
	throw __new<::CoreLib::System::NotSupportedException>();
}

// ICollection
template <typename T>
void __array<T>::System_Collections_ICollection_CopyTo(::CoreLib::System::Array* dest, int32_t length) 	
{
	CopyTo(dest, length);
}

template <typename T>
int32_t __array<T>::System_Collections_ICollection_get_Count() 	
{
	return get_Length();
}

template <typename T>
::CoreLib::System::Object* __array<T>::System_Collections_ICollection_get_SyncRoot() 	
{
	return get_SyncRoot();
}

template <typename T>
bool __array<T>::System_Collections_ICollection_get_IsSynchronized() 	
{
	return get_IsSynchronized();
}

// IEnumerable
template <typename T>
::CoreLib::System::Collections::IEnumerator* __array<T>::System_Collections_IEnumerable_GetEnumerator()	
{
	return GetEnumerator();
}

// Method : 
template <typename T> 
T __array<T>::__array_IListT1::get_Item(int32_t index)
{
	return this->_class->System_Collections_Generic_IListT1_get_Item(index);
}

// Method : 
template <typename T> 
void __array<T>::__array_IListT1::set_Item(int32_t index, T value)
{
	return this->_class->System_Collections_Generic_IListT1_set_Item(index, value);
}

// Method : 
template <typename T> 
int32_t __array<T>::__array_IListT1::IndexOf(T item)
{
	return this->_class->System_Collections_Generic_IListT1_IndexOf(item);
}

// Method : 
template <typename T> 
void __array<T>::__array_IListT1::Insert(int32_t index, T item)
{
	return this->_class->System_Collections_Generic_IListT1_Insert(index, item);
}

// Method : 
template <typename T> 
void __array<T>::__array_IListT1::RemoveAt(int32_t index)
{
	return this->_class->System_Collections_Generic_IListT1_RemoveAt(index);
}

// Method : 
template <typename T> 
int32_t __array<T>::__array_IListT1::get_Count()
{
	return this->_class->System_Collections_Generic_ICollectionT1_get_Count();
}

// Method : 
template <typename T> 
bool __array<T>::__array_IListT1::get_IsReadOnly()
{
	return this->_class->System_Collections_Generic_ICollectionT1_get_IsReadOnly();
}

// Method : 
template <typename T> 
void __array<T>::__array_IListT1::Add(T item)
{
	return this->_class->System_Collections_Generic_ICollectionT1_Add(item);
}

// Method : 
template <typename T> 
void __array<T>::__array_IListT1::Clear()
{
	return this->_class->System_Collections_Generic_ICollectionT1_Clear();
}

// Method : 
template <typename T> 
bool __array<T>::__array_IListT1::Contains(T item)
{
	return this->_class->System_Collections_Generic_ICollectionT1_Contains(item);
}

// Method : 
template <typename T> 
void __array<T>::__array_IListT1::CopyTo(__array<T>* array, int32_t arrayIndex)
{
	return this->_class->System_Collections_Generic_ICollectionT1_CopyTo(array, arrayIndex);
}

// Method : 
template <typename T> 
bool __array<T>::__array_IListT1::Remove(T item)
{
	return this->_class->System_Collections_Generic_ICollectionT1_Remove(item);
}

// Method : 
template <typename T> 
::CoreLib::System::Collections::Generic::IEnumeratorT1<T>* __array<T>::__array_IListT1::System_Collections_Generic_IEnumerableT1_GetEnumerator()
{
	return this->_class->System_Collections_Generic_IEnumerableT1_GetEnumerator();
}

// Method : 
template <typename T> 
::CoreLib::System::Collections::IEnumerator* __array<T>::__array_IListT1::GetEnumerator()
{
	return this->_class->System_Collections_IEnumerable_GetEnumerator();
}

// Method : 
template <typename T> 
int32_t __array<T>::__array_ICollectionT1::get_Count()
{
	return this->_class->System_Collections_Generic_ICollectionT1_get_Count();
}

// Method : 
template <typename T> 
bool __array<T>::__array_ICollectionT1::get_IsReadOnly()
{
	return this->_class->System_Collections_Generic_ICollectionT1_get_IsReadOnly();
}

// Method : 
template <typename T> 
void __array<T>::__array_ICollectionT1::Add(T item)
{
	return this->_class->System_Collections_Generic_ICollectionT1_Add(item);
}

// Method : 
template <typename T> 
void __array<T>::__array_ICollectionT1::Clear()
{
	return this->_class->System_Collections_Generic_ICollectionT1_Clear();
}

// Method : 
template <typename T> 
bool __array<T>::__array_ICollectionT1::Contains(T item)
{
	return this->_class->System_Collections_Generic_ICollectionT1_Contains(item);
}

// Method : 
template <typename T> 
void __array<T>::__array_ICollectionT1::CopyTo(__array<T>* array, int32_t arrayIndex)
{
	return this->_class->System_Collections_Generic_ICollectionT1_CopyTo(array, arrayIndex);
}

// Method : 
template <typename T> 
bool __array<T>::__array_ICollectionT1::Remove(T item)
{
	return this->_class->System_Collections_Generic_ICollectionT1_Remove(item);
}

// Method : 
template <typename T> 
::CoreLib::System::Collections::Generic::IEnumeratorT1<T>* __array<T>::__array_ICollectionT1::System_Collections_Generic_IEnumerableT1_GetEnumerator()
{
	return this->_class->System_Collections_Generic_IEnumerableT1_GetEnumerator();
}

// Method : 
template <typename T> 
::CoreLib::System::Collections::IEnumerator* __array<T>::__array_ICollectionT1::GetEnumerator()
{
	return this->_class->System_Collections_IEnumerable_GetEnumerator();
}

// Method : 
template <typename T> 
::CoreLib::System::Collections::Generic::IEnumeratorT1<T>* __array<T>::__array_IEnumerableT1::System_Collections_Generic_IEnumerableT1_GetEnumerator()
{
	return this->_class->System_Collections_Generic_IEnumerableT1_GetEnumerator();
}

// Method : 
template <typename T> 
::CoreLib::System::Collections::IEnumerator* __array<T>::__array_IEnumerableT1::GetEnumerator()
{
	return this->_class->System_Collections_IEnumerable_GetEnumerator();
}

// Method : 
template <typename T> 
::CoreLib::System::Collections::IEnumerator* __array<T>::__array_IEnumerable::GetEnumerator()
{
	return this->_class->System_Collections_IEnumerable_GetEnumerator();
}

// Method : 
template <typename T> 
object* __array<T>::__array_IList::get_Item(int32_t index)
{
	return this->_class->System_Collections_IList_get_Item(index);
}

// Method : 
template <typename T> 
void __array<T>::__array_IList::set_Item(int32_t index, object* value)
{
	return this->_class->System_Collections_IList_set_Item(index, value);
}

// Method : 
template <typename T> 
int32_t __array<T>::__array_IList::Add(object* value)
{
	return this->_class->System_Collections_IList_Add(value);
}

// Method : 
template <typename T> 
bool __array<T>::__array_IList::Contains(object* value)
{
	return this->_class->System_Collections_IList_Contains(value);
}

// Method : 
template <typename T> 
void __array<T>::__array_IList::Clear()
{
	return this->_class->System_Collections_IList_Clear();
}

// Method : 
template <typename T> 
bool __array<T>::__array_IList::get_IsReadOnly()
{
	return this->_class->System_Collections_IList_get_IsReadOnly();
}

// Method : 
template <typename T> 
bool __array<T>::__array_IList::get_IsFixedSize()
{
	return this->_class->System_Collections_IList_get_IsFixedSize();
}

// Method : 
template <typename T> 
int32_t __array<T>::__array_IList::IndexOf(object* value)
{
	return this->_class->System_Collections_IList_IndexOf(value);
}

// Method : 
template <typename T> 
void __array<T>::__array_IList::Insert(int32_t index, object* value)
{
	return this->_class->System_Collections_IList_Insert(index, value);
}

// Method : 
template <typename T> 
void __array<T>::__array_IList::Remove(object* value)
{
	return this->_class->System_Collections_IList_Remove(value);
}

// Method : 
template <typename T> 
void __array<T>::__array_IList::RemoveAt(int32_t index)
{
	return this->_class->System_Collections_IList_RemoveAt(index);
}

// Method : 
template <typename T> 
void __array<T>::__array_IList::CopyTo(::CoreLib::System::Array* array, int32_t index)
{
	return this->_class->System_Collections_ICollection_CopyTo(array, index);
}

// Method : 
template <typename T> 
int32_t __array<T>::__array_IList::get_Count()
{
	return this->_class->System_Collections_ICollection_get_Count();
}

// Method : 
template <typename T> 
object* __array<T>::__array_IList::get_SyncRoot()
{
	return this->_class->System_Collections_ICollection_get_SyncRoot();
}

// Method : 
template <typename T> 
bool __array<T>::__array_IList::get_IsSynchronized()
{
	return this->_class->System_Collections_ICollection_get_IsSynchronized();
}

// Method : 
template <typename T> 
::CoreLib::System::Collections::IEnumerator* __array<T>::__array_IList::GetEnumerator()
{
	return this->_class->System_Collections_IEnumerable_GetEnumerator();
}

// Method : 
template <typename T> 
void __array<T>::__array_ICollection::CopyTo(::CoreLib::System::Array* array, int32_t index)
{
	return this->_class->System_Collections_ICollection_CopyTo(array, index);
}

// Method : 
template <typename T> 
int32_t __array<T>::__array_ICollection::get_Count()
{
	return this->_class->System_Collections_ICollection_get_Count();
}

// Method : 
template <typename T> 
object* __array<T>::__array_ICollection::get_SyncRoot()
{
	return this->_class->System_Collections_ICollection_get_SyncRoot();
}

// Method : 
template <typename T> 
bool __array<T>::__array_ICollection::get_IsSynchronized()
{
	return this->_class->System_Collections_ICollection_get_IsSynchronized();
}

// Method : 
template <typename T> 
::CoreLib::System::Collections::IEnumerator* __array<T>::__array_ICollection::GetEnumerator()
{
	return this->_class->System_Collections_IEnumerable_GetEnumerator();
}

// multi array
// Array
template <typename T, int32_t RANK>
int32_t  __multi_array<T, RANK>::__array_element_size()
{
	return sizeof(T);
}

template <typename T, int32_t RANK>
bool  __multi_array<T, RANK>::__is_primitive_type_array()
{
	return is_primitive_type<T>::value;
}

template <typename T, int32_t RANK>
int32_t __multi_array<T, RANK>::GetUpperBound(int32_t dimension)
{
	if (dimension >= RANK)
	{
		throw __new<::CoreLib::System::IndexOutOfRangeException>();
	}	

	return this->_upperBoundries[dimension] - 1;
}

template <typename T, int32_t RANK>
int32_t __multi_array<T, RANK>::GetLowerBound(int32_t dimension)
{
	if (dimension >= RANK)
	{
		throw __new<::CoreLib::System::IndexOutOfRangeException>();
	}	

	return this->_lowerBoundries[dimension];
}

template <typename T, int32_t RANK>
int32_t __multi_array<T, RANK>::GetLength(int32_t dimension)
{
	if (dimension >= RANK)
	{
		throw __new<::CoreLib::System::IndexOutOfRangeException>();
	}	

	return this->_upperBoundries[dimension] - this->_lowerBoundries[dimension];
}

template <typename T, int32_t RANK>
void __multi_array<T, RANK>::InternalGetReference(void* elemRef, int32_t rank, int32_t* pIndices)
{
	if (rank != RANK)
	{
		throw __new<::CoreLib::System::RankException>(u"rank"_s);
	}

	if (elemRef == nullptr)
	{
		throw __new<::CoreLib::System::ArgumentNullException>(u"elemRef"_s);
	}	

	if (pIndices == nullptr)
	{
		throw __new<::CoreLib::System::ArgumentNullException>(u"pIndices"_s);
	}	

	auto index = calculate_index(pIndices);
	if (index < 0 || index >= this->get_Length())
	{
		throw __new<::CoreLib::System::IndexOutOfRangeException>();
	}	

	auto typedRef = reinterpret_cast<::CoreLib::System::TypedReference*>(elemRef);
	typedRef->Value = __init<::CoreLib::System::IntPtr>((void*)&this->_data[index]);
	typedRef->Type = __init<::CoreLib::System::IntPtr>((void*)_typeMT<T>());
}

template <typename T, int32_t RANK>
int32_t __multi_array<T, RANK>::get_Length()
{
	auto length = 1;
	for (auto rank = 0; rank < RANK; rank++)
	{
		length *= (_upperBoundries[rank] - _lowerBoundries[rank]);
	}

	return length;
}

template <typename T, int32_t RANK>
int32_t __multi_array<T, RANK>::get_Rank()
{
	return RANK;
}

template <typename T, int32_t RANK>
::CoreLib::System::RuntimeType __multi_array__type<T, RANK>::__type = ::CoreLib::System::RuntimeType(&__multi_array__type<T, RANK>::__rt_info);

template <typename T, int32_t RANK>
__runtimetype_info __multi_array__type<T, RANK>::__rt_info = { _typeMT<__multi_array<T, RANK>>(), nullptr, nullptr, 20, false, 0, _runtime_typeof<::CoreLib::System::Array>(), _runtime_typeof<T>() };

// Method : 
template <typename T, int32_t RANK>
::CoreLib::System::Type* __multi_array<T, RANK>::__get_type()
{
	return _typeof<__multi_array<T, RANK>>();
}

// Method : 
template <typename T, int32_t RANK>
bool __multi_array<T, RANK>::__is_type(::CoreLib::System::Type* value)
{
	return ((_typeof<__multi_array<T, RANK>>() == value) || base::__is_type(value));
}